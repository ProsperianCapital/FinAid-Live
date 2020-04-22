using System;
using System.Text;
using System.Collections.Generic;

namespace PCIBusiness
{
	public class MiscList : BaseList
	{
		private int colNo;

		public override BaseData NewItem()
		{
			return new MiscData();
		}

		public int ExecQuery(string sqlQuery,byte loadRows,string dataClass="",bool noRowsIsError=true)
		{
			sql = sqlQuery;

			if ( loadRows == 0 )
				return base.ExecuteSQL(null,false,noRowsIsError);

			if ( string.IsNullOrWhiteSpace(dataClass) )
				return base.LoadDataFromSQL();

//	The above creates "MiscData" objects.
//	Below creates objects of type {dataClass} using Reflection.

			string err = "Invalid class name";
			try
			{
				Type classType  = (System.Reflection.Assembly.Load("PCIBusiness")).GetType("PCIBusiness."+dataClass);
				if ( classType != null )
					return base.LoadDataFromSQL(null,0,classType);
			}
			catch (Exception ex)
			{
				err = ex.Message;
			}
			Tools.LogException("MiscList.ExecQuery",err + " (DataClass=" + dataClass + ", SQL=" + sqlQuery + ")");
			return 0;
		}

		public void Add(MiscData dataX)
		{
			if ( objList == null )
				objList = new List<BaseData>();
			objList.Add(dataX);
		}

		public string GetColumn(int colNumber)
		{
			try
			{
				if ( dbConn != null )
					return dbConn.ColValue(colNumber);
			}
			catch
			{ }
			return "";
		}
		public string GetColumn(string colName,byte errorMode=1)
		{
			try
			{
				if ( dbConn != null )
				{
					int x = dbConn.ColNumber(colName,errorMode);
					if ( x >= 0 )
						return GetColumn(x);	
				}
			}
			catch
			{ }
			return "";
		}
		public string NextColumn
		{
			get
			{
				if ( colNo < 0 )
					colNo = 0;
				else
					colNo++;
				return GetColumn(colNo);
			}
		}

		public bool EOF
		{
			get
			{
				if ( dbConn == null )
					return false;
				return dbConn.EOF;
			}
		}

		public bool NextRow()
		{
			if ( dbConn == null )
				return false;
			return dbConn.NextRow();
		}
	}
}
