using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Text;

namespace PCIBusiness
{
	public class DBConn : IDisposable
	{
	/// <summary>
	/// 
	/// A generic database connection routine that wraps and hides all native objects and methods.
	/// Typical usage is as follows:
	/// 
	///	sqlStr = "select * from Blah";
	/// 
	///   DBConn conn;
	///   if ( Tools.OpenDB(ref conn) )
	///      if ( conn.Execute(sqlStr,true) )
	///         while ( ! conn.EOF )
	///         {
	///            x1 = conn.ColString("ColumnName1");
	///            x2 = conn.ColLong  ("ColumnName2");
	///            x3 = conn.ColDate  ("ColumnName3");
	///			   conn.NextRow();
	///         }
	///   Tools.CloseDB(ref conn);  
	///   
	/// Or ...
	/// 
	///   sql      = "exec someStoredProc @0, @1";
	///   sqlParm0 = "ABC";
	///   sqlParm1 = 24;
	///   conn.Execute(sqlString,true,(new object[][]{new object[]{System.Data.SqlDbType.VarChar,sqlParm0},new object[]{System.Data.SqlDbType.Int,sqlParm1}});
	///   
	/// No try/catch or cleaning up needed - DBConn takes care of all this.
	/// 
	/// </summary>

		private SqlConnection dbConn;
		private SqlCommand    sqlCmd;    
		private SqlDataReader dataReader;
		private bool          isEOF;
		private int           colNo;
		private int           returnValue;
		private string        sourceInfo;

		public string SourceInfo
		{
			get { return Tools.NullToString(sourceInfo); }
			set 
			{
				if ( string.IsNullOrWhiteSpace(sourceInfo) && ! string.IsNullOrWhiteSpace(value) )
					sourceInfo = value.Trim();
			}
		}

		public int ReturnValue
		{
			get { return returnValue; }
		}

		private string ModuleName(string from)
		{
			if ( sourceInfo.Length > 0 )
				from = from + " [" + sourceInfo + "]";
			return from;
		}

		public bool Open(string connectionName="")
		{
			isEOF = true;

			if ( dbConn == null )
			{
				string connString = "";
				if ( string.IsNullOrWhiteSpace(connectionName) )
					connectionName = "DBConn";

				try
				{
					ConnectionStringSettings db = ConfigurationManager.ConnectionStrings[connectionName];
					connString                  = db.ConnectionString;
					if ( connString == null || connString.Length < 5 )
						return false;
					dbConn = new SqlConnection(connString);
					dbConn.Open();
				}
				catch (Exception ex)
				{
					Tools.LogException ( ModuleName("DBConn.Open"), "DB Connection String=" + Tools.NullToString(connString), ex );
					return false;
				}
			}
			else if ( dbConn.State != ConnectionState.Open )
				dbConn.Open();

			return true;
		}

		public bool Close() 
		{
		// Make absolutely sure that EVERYTHING is closed and destroyed

			isEOF = true;

			if (sqlCmd != null) 
			{
				try
				{
					sqlCmd.Cancel();
				}	catch { }
				try
				{
					sqlCmd.Dispose();
				}	catch { }
			}

			if (dataReader != null) 
				try
				{
					dataReader.Close();
				}	catch { }
              
			if (dbConn != null) 
			{
				try
				{
					dbConn.Close();
				}	catch { }
				try
				{
					dbConn.Dispose();
				}	catch { }
			}

			sqlCmd     = null;
			dataReader = null;
			dbConn     = null;
			return true;
		}

		public bool Execute ( string sql, bool autoClose=true, object[][] parms=null, byte execMode=0, int timeOut=0 )
		{
			isEOF       = true;
			sql         = sql.Trim();
			returnValue = 0;

			try
			{
				if ( dataReader != null ) 
					if ( ! dataReader.IsClosed ) 
						dataReader.Close();

				if ( sqlCmd == null )
				{
					sqlCmd                = new SqlCommand();
					sqlCmd.Connection     = dbConn;
					sqlCmd.CommandTimeout = ( timeOut > 60 ) ? timeOut : 900; // 900 = 15 minutes 
				}
				else
					try
					{
						sqlCmd.Cancel();
						sqlCmd.Connection.Open();
					}
					catch { }

				sqlCmd.CommandText      = sql;
				sqlCmd.CommandType      = System.Data.CommandType.Text;
				SqlParameter returnParm = null;

				if ( parms != null )
					for ( int k = 0; k < parms.Length; k++ )
					{
						sqlCmd.Parameters.Add ( "@"+k.ToString(), (SqlDbType) parms[k][0] );
						sqlCmd.Parameters[k].Value = parms[k][1];
					}
	
				if ( execMode == 2 ) // SQL "return" value, MUST be a stored procedure
				{
					returnParm           = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int);
					returnParm.Direction = System.Data.ParameterDirection.ReturnValue;
					sqlCmd.CommandType   = CommandType.StoredProcedure;
					sqlCmd.Parameters.Add(returnParm);
				}

				if (autoClose)
					dataReader = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
				else
					dataReader = sqlCmd.ExecuteReader();
 
				if ( execMode == 2 ) // SQL "return" value
					returnValue = System.Convert.ToInt32(returnParm.Value);
         
				if ( dataReader != null )
					if ( dataReader.HasRows )
						isEOF = ! dataReader.Read();
			}
			catch (Exception ex)
			{
				Tools.LogException ( ModuleName("DBConn.Execute"), DatabaseDetails + ". SQL = " + sql, ex );
				return false;
			}
			return true;
		}

		public bool EOF
		{
			get { return isEOF; }
		}

		public bool NextRow()
		{
			isEOF = ( ! dataReader.Read() );
			return  ( ! isEOF );
		}

		public bool NextResultSet()
		{
			try
			{
				if ( dataReader.NextResult() )
					return NextRow();
			}
			catch (Exception ex)
			{
				Tools.LogException(ModuleName("DBConn.NextResultSet"),DatabaseDetails,ex);
			}
			return false;
		}

		public int ColumnCount 
		{
			get
			{
				try
				{
					return dataReader.FieldCount;
				}
				catch { }
				return 0;
			}
		}
 
   public Constants.DBColumnStatus ColStatus(string colName,int colNumber=999999)
   {
      try
		{
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
			{
				colNo = colNumber;
				dataReader.GetValue(colNo);
			}

//			if ( colNumber >= 0 && colNumber < 9999 )
//				dataReader.GetValue(colNo);
//			else
//				colNo = dataReader.GetOrdinal(colName);
		}
		catch
		{
			return Constants.DBColumnStatus.InvalidColumn; // 1 
		}

		try
		{
         if ( dataReader.IsDBNull(colNo) )
		      return Constants.DBColumnStatus.ValueIsNull; // 3
		   else
			   return Constants.DBColumnStatus.ColumnOK; // 4
      }
      catch { }
      return Constants.DBColumnStatus.EOF; // 2
   }

   public string ColDataType(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         return dataReader.GetDataTypeName(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColDataType"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return "";
   }

   public byte ColByte(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetByte(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColByte"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return 0;
   }

   public short ColShort(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt16(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColShort"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return 0;
   }

   public int ColLong(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt32(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColLong"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return 0;
   }

   public long ColBig(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt64(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColBig"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return 0;
   }

   public decimal ColDecimal(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetDecimal(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColDecimal"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return 0;
   }

   public string ColString(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
			if ( ! dataReader.IsDBNull(colNo) )
				return dataReader.GetString(colNo).Trim();

//			if ( dataReader.IsDBNull(colNo) )
//				return "";
//			else if ( convertMode == 177 ) // It may be a GUID
//			{
//				string x = dataReader.GetDataTypeName(colNo);
//				Type   y = dataReader.GetFieldType(colNo);
//				if ( dataReader.GetDataType(colNo) == 
//				Guid x = dataReader.GetGuid(colNo);
//				return x.ToString();
//			}
//			else
//				return dataReader.GetString(colNo).Trim();
     }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColString"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return "";
   }

   public string ColGuid(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
			if ( dataReader.IsDBNull(colNo) )
				return "";
			try
			{
				return (dataReader.GetGuid(colNo)).ToString();
			}
			catch { }
			return dataReader.GetString(colNo).Trim();
		}
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColGuid"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return "";
   }

   public string ColUniCode(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;

			if ( dataReader.IsDBNull(colNo) )
				return "";

			SqlString str     = dataReader.GetSqlString(colNo);
			byte[]    uniCode = str.GetUnicodeBytes();
			return Encoding.Unicode.GetString(uniCode);

/*
			string    pre     = "\\u";
			SqlString str1    = dataReader.GetSqlString(colNo);
			string    str2    = str1.ToString();

			byte[]    uniCodeA = str1.GetUnicodeBytes();
			byte[]    uniCodeB = Encoding.Unicode.GetBytes(str2);
			byte[]    uniCodeC = Encoding.UTF8.GetBytes(str2);

			string    retA1    = "";
			string    retA2    = "";
			string    retA3    = "";
			string    retB1    = "";
			string    retB2    = "";
			string    retB3    = "";
			string    retC1    = "";
			string    retC2    = "";
			string    retC3    = "";

//			if ( uniCode.Length < 1 )
//				return "";

			for ( int k = 0 ; k < uniCodeA.Length ; k ++ )
				retA1 = retA1 + pre + uniCodeA[k];

			retA2 = Encoding.UTF8.GetString(uniCodeA);
			retA3 = Encoding.Unicode.GetString(uniCodeA);

			for ( int k = 0 ; k < uniCodeB.Length ; k ++ )
				retB1 = retB1 + pre + uniCodeB[k];

			retB2 = Encoding.UTF8.GetString(uniCodeB);
			retB3 = Encoding.Unicode.GetString(uniCodeB);

			for ( int k = 0 ; k < uniCodeC.Length ; k ++ )
				retC1 = retC1 + pre + uniCodeC[k];

			retC2 = Encoding.UTF8.GetString(uniCodeC);
			retC3 = Encoding.Unicode.GetString(uniCodeC);

			string retD1 = System.Text.UnicodeEncoding.Unicode.GetString(uniCodeA);
			string retD2 = System.Text.UnicodeEncoding.Unicode.GetString(uniCodeB);
			string retD3 = System.Text.UnicodeEncoding.Unicode.GetString(uniCodeC);

			if ( errorMode == 29 )
			{
				Tools.LogInfo ( ModuleName("DBConn.ColUniCode/A"), "retA1 = '" + retA1 + "', retA2 = '" + retA2 + "', retA3 = '" + retA3 + "'", 255 );
				Tools.LogInfo ( ModuleName("DBConn.ColUniCode/B"), "retB1 = '" + retB1 + "', retB2 = '" + retB2 + "', retB3 = '" + retB3 + "'", 255 );
				Tools.LogInfo ( ModuleName("DBConn.ColUniCode/C"), "retC1 = '" + retC1 + "', retC2 = '" + retC2 + "', retC3 = '" + retC3 + "'", 255 );
				Tools.LogInfo ( ModuleName("DBConn.ColUniCode/D"), "retD1 = '" + retD1 + "', retD2 = '" + retD2 + "', retD3 = '" + retD3 + "'", 255 );
			}
			return retB1;
*/

      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColUniCode"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return "";
   }

   public DateTime ColDate(string colName,int colNumber=999999,byte errorMode=1)
   {
      try
      {
			if ( colName.Length > 0 )
				colNo = dataReader.GetOrdinal(colName);
			else
				colNo = colNumber;
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetDateTime(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColDate"), "ColName=" + colName + ", ColNumber=" + colNumber.ToString(), ex );
      }
      return Constants.DateNull;
   } 

		public string ColValue(int colNumber,byte errorMode=1)
		{
			try
			{
				return (dataReader[colNumber].ToString()).Trim();
			}
			catch (Exception ex)
			{
				if ( errorMode == 1 )
					Tools.LogException ( ModuleName("DBConn.ColValue"), "ColNumber=" + colNumber.ToString(), ex );
			}
			return "";
		}

		public string ColName(int colNumber,byte errorMode=1)
		{
			try
			{
				return dataReader.GetName(colNumber);
			}
			catch (Exception ex)
			{
				if ( errorMode == 1 )
					Tools.LogException ( ModuleName("DBConn.ColName"), "ColNumber=" + colNumber.ToString(), ex );
			}
			return "";
		}

		public int ColNumber(string colName,byte errorMode=1)
		{
			try
			{
				return dataReader.GetOrdinal(colName);
			}
			catch (Exception ex)
			{
				if ( errorMode == 1 )
					Tools.LogException ( ModuleName("DBConn.ColNumber"), "ColName=" + colName, ex );
			}
			return -8;
		}

		public string DatabaseDetails
		{
			get
			{
				string ret = "";
				try
				{
					string[] connStr = dbConn.ConnectionString.Split(';');
					foreach (string piece in connStr) // NOT "passsword"
						if ( piece.ToUpper().StartsWith("SERVE") ||
						     piece.ToUpper().StartsWith("DATA")  ||
						     piece.ToUpper().StartsWith("UID")   ||
						     piece.ToUpper().StartsWith("USER")  ||
						     piece.ToUpper().StartsWith("DB") )
							ret = ret + ", " + piece;
				}
				catch
				{
					ret = "Error";
				}		
				if ( ret.Length < 1 )
					ret = "Not connected";
				else if ( ret.StartsWith(",") )			
					ret = ret.Substring(1).Trim();
				return "DBConnection : " + ret;
			}
		}

	// Routines for cleaning up
	// ------------------------

		protected virtual void Dispose(bool vDispose)
		{
			Close();
		}

		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public DBConn() : base()
		{
			isEOF       = true;
			sourceInfo  = "";
			returnValue = 0;
		}

		~DBConn()
		{
			Dispose(false);
		}
	}
}
