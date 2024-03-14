using System;
using System.Collections.Generic;
using System.Text;

namespace PCIBusiness
{
	public abstract class BaseData : StdDisposable
	{
		protected int    returnCode;
		protected string returnMessage;
		protected string returnData;
		protected string sql;
		protected DBConn dbConn;

		public override void Close()
		{
		// This will automatically be called by the base class destructor (StdDisposable).

		//	Clean up the derived class
			CleanUp();

		//	Clean up the base class
			Tools.CloseDB(ref dbConn);
			dbConn        = null;
			returnCode    = 0;
			returnMessage = "";
			returnData    = "";
		}

		public virtual void CleanUp()
		{
		//	This method can be overridden in the derived class to CLEAN UP stuff - not to initialize in the beginning
		//	Nothing here, so can completely override it in the derived class
		}

		public abstract void LoadData(DBConn dbConn);

//		protected void ExecuteSQLDebug()
//		{
//			Tools.LogInfo("SQLDebug/1","SQL = "+sql);
//			if ( ExecuteSQL(null,false) == 0 )
//			{
//				Tools.LogInfo("SQLDebug/2","Execution successful, column count = " + dbConn.ColumnCount.ToString());
//				for ( int k = 0 ; k < dbConn.ColumnCount ; k++ )
//					Tools.LogInfo("SQLDebug/3","(Col " + k.ToString()
//						                     + ") Name = " + dbConn.ColName(k)
//						                     + ", Type = " + dbConn.ColDataType(dbConn.ColName(k))
//						                     + ", Value = " + dbConn.ColValue(k));
//			}
//			else
//				Tools.LogInfo("SQLDebug/4","Execution failed, return code = " + returnCode.ToString());
//			Tools.CloseDB(ref dbConn);
//		}

		protected int ExecuteSQLUpdate(byte execMode=1,bool closeConnection=true)
		{
			if ( ExecuteSQL(null,execMode) == 0 )
				if ( execMode == 1 )
				{
					returnCode    = dbConn.ColLong  ("ReturnCode");
					returnMessage = dbConn.ColString("ReturnMessage",0,0);
					returnData    = dbConn.ColString("ReturnData",0,0);
				}
				else if ( execMode == 2 )
					returnCode    = dbConn.ReturnValue;

			if (closeConnection)
				Tools.CloseDB(ref dbConn);
			return returnCode;
		}

		protected int ExecuteSQL (object[][] parms          =null,
		                          byte       execMode       =1,
		                          bool       closeConnection=true,
		                          string     connectionName ="")
		{
		//	"execMode" can be
		//	1 : Execute and expect a result set with at least 1 row
		//	2 : Execute and expect a SQL integer return value
		//	3 : Execute and expect nothing

			returnCode    = 0;
			returnMessage = "";
			returnData    = "";

			if ( ! Tools.OpenDB(ref dbConn,connectionName) )
			{
				returnCode    = 1;
				returnMessage = "[BaseData.ExecuteSQL/1] Unable to connect to SQL database"
				              + ( connectionName.Length > 0 ? ", connection '" + connectionName + "'" : "" );
			}
			else if ( ! dbConn.Execute(sql,closeConnection,parms) )
			{
				returnCode    = 2;
				returnMessage = "[BaseData.ExecuteSQL/2] SQL execution failed";
			}
			else if ( dbConn.EOF && execMode == 1 )
			{
				returnCode    = 3;
				returnMessage = "[BaseData.ExecuteSQL/3] SQL successfully executed, but no data returned";
			}
			else if ( execMode == 2 && dbConn.ReturnValue < 0 )
			{
				returnCode    = 4;
				returnMessage = "[BaseData.ExecuteSQL/4] SQL successfully executed, but the return code was invalid/missing";
			}
			return returnCode;
		}

		public virtual int RowNumber // Override if needed
		{
			set { }
			get { return 0; }
		}

		public int      ReturnCode
		{
			get { return returnCode; }
		}
		public string   ReturnMessage 
		{
			get { return Tools.NullToString(returnMessage); }
		}

		public string   ReturnData
		{
			get { return Tools.NullToString(returnData); }
		}
		public string   SQL
		{
			get { return Tools.NullToString(sql); }
		}
	}
}
