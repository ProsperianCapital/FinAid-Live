using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
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
		private string        sourceInfo;

		public string SourceInfo
		{
			get { return sourceInfo; }
			set 
			{
				if ( sourceInfo.Length == 0 )
					sourceInfo = value;
			}
		}

		private string ModuleName(string from)
		{
			if ( sourceInfo.Length > 0 )
				from = from + " [" + sourceInfo + "]";
			return from;
		}

		public bool Open()
		{
			isEOF = true;

			if ( dbConn == null )
			{
//				string connName   = "";
				string connString = "";

				try
				{
//					connName = Tools.ConfigValue("DBConnection");
//					if ( connName == null || connName.Trim().Length == 0 )
//						connName = "LiveDB";

					ConnectionStringSettings db = ConfigurationManager.ConnectionStrings["DBConn"];
					connString = db.ConnectionString;
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

		public bool Execute ( string sql, bool autoClose=true, object[][] parms=null, int timeOut=0 )
		{
			isEOF = true;
			sql   = sql.Trim();

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

				sqlCmd.CommandText = sql;
				sqlCmd.CommandType = System.Data.CommandType.Text;

				if ( parms != null )
					for ( int k = 0; k < parms.Length; k++ )
					{
						sqlCmd.Parameters.Add ( "@"+k.ToString(), (SqlDbType) parms[k][0] );
						sqlCmd.Parameters[k].Value = parms[k][1];
					}

				if (autoClose)
					dataReader = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
				else
					dataReader = sqlCmd.ExecuteReader();
          
				if ( dataReader != null )
					if ( dataReader.HasRows )
						isEOF = ! dataReader.Read();
			}
			catch (Exception ex)
			{
				Tools.LogException ( ModuleName("DBConn.Execute"), sql, ex );
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
				Tools.LogException("DBConn.NextResultSet","",ex);
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
 
   public Constants.DBColumnStatus ColStatus(string colName,int colNo=999999999)
   {
      try
		{
			if ( colNo >= 0 && colNo < 99999999 )
				dataReader.GetValue(colNo);
			else
				colNo = dataReader.GetOrdinal(colName);
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

   public string ColDataType(string colName,int colNumber=999999)
   {
      try
      {
			if ( colName.Length < 1 && colNumber >= 0 && colNumber <= 99999 )
				colNo = colNumber;
			else
				colNo = dataReader.GetOrdinal(colName);
         return dataReader.GetDataTypeName(colNo);
      }
      catch (Exception ex)
      {
         Tools.LogException ( ModuleName("DBConn.ColDataType"), colName, ex );
      }
      return "";
   }

   public byte ColByte(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetByte(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColByte"), colName, ex );
      }
      return 0;
   }

   public short ColShort(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt16(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColShort/1"), colName, ex );
      }
      return 0;
   }

	public short ColShort(int colNumber)
	{
      try
      {
         if ( ! dataReader.IsDBNull(colNumber) ) 
            return dataReader.GetInt16(colNumber);
      }
      catch (Exception ex)
      {
			Tools.LogException ( ModuleName("DBConn.ColShort/2"), "ColNo=" + colNumber.ToString(), ex );
      }
		return 0;
	}

   public int ColLong(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt32(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColLong/1"), colName, ex );
      }
      return 0;
   }

	public int ColLong(int colNumber)
	{
      try
      {
         if ( ! dataReader.IsDBNull(colNumber) ) 
            return dataReader.GetInt32(colNumber);
      }
      catch (Exception ex)
      {
			Tools.LogException ( ModuleName("DBConn.ColLong/2"), "ColNo=" + colNumber.ToString(), ex );
      }
		return 0;
	}

   public long ColBig(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetInt64(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColBig"), colName, ex );
      }
      return 0;
   }

   public decimal ColDecimal(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetDecimal(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColDecimal"), colName, ex );
      }
      return 0;
   }

   public string ColString(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) )
				return dataReader.GetString(colNo).Trim();

//				if ( dataReader.GetDataTypeName(colNo).ToUpper() == "NVARCHAR" )
//					return dataReader.GetSqlString(colNo).ToString().Trim();
//				else
//					return dataReader.GetString(colNo).Trim();
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColString"), colName, ex );
      }
      return "";
   }

   public DateTime ColDate(string colName,byte errorMode=1)
   {
      try
      {
         colNo = dataReader.GetOrdinal(colName);
         if ( ! dataReader.IsDBNull(colNo) ) 
            return dataReader.GetDateTime(colNo);
      }
      catch (Exception ex)
      {
			if ( errorMode == 1 )
				Tools.LogException ( ModuleName("DBConn.ColDate"), colName, ex );
      }
      return Constants.C_NULLDATE();
   } 

		public string ColValue(int colNumber)
		{
			try
			{
				string tmp = dataReader[colNumber].ToString();
				return tmp.Trim();
			}
			catch (Exception ex)
			{
				Tools.LogException ( ModuleName("DBConn.ColValue"), "ColNo=" + colNumber.ToString(), ex );
			}
			return "";
		}

		public string ColName(int colNumber)
		{
			try
			{
				return dataReader.GetName(colNumber);
			}
			catch (Exception ex)
			{
				Tools.LogException ( ModuleName("DBConn.ColName"), "ColNo=" + colNumber.ToString(), ex );
			}
			return "";
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
			isEOF      = true;
			sourceInfo = "";
		}

		~DBConn()
		{
			Dispose(false);
		}
	}
}
