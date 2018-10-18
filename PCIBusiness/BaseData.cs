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

		protected int ExecuteSQLUpdate(bool closeConnection=true,bool getReturnCodes=true)
		{
			if ( ExecuteSQL(null) == 0 && getReturnCodes )
			{
				returnCode    = dbConn.ColLong  ("ReturnCode");
				returnMessage = dbConn.ColString("ReturnMessage");
				returnData    = dbConn.ColString("ReturnData");
			}
			if (closeConnection)
				Tools.CloseDB(ref dbConn);
			return returnCode;
		}

		protected int ExecuteSQL (object[][] parms          =null,
		                          bool       eofIsError     =true,
		                          bool       closeConnection=true)
		{
			returnCode    = 0;
			returnMessage = "";
			returnData    = "";

			if ( ! Tools.OpenDB(ref dbConn) )
			{
				returnCode    = 1;
				returnMessage = "[BaseData.ExecuteSQL/1] Unable to connect to SQL database";
			}
			else if ( ! dbConn.Execute(sql,closeConnection,parms) )
			{
				returnCode    = 2;
				returnMessage = "[BaseData.ExecuteSQL/2] SQL execution failed";
			}
			else if ( dbConn.EOF && eofIsError )
			{
				returnCode    = 3;
				returnMessage = "[BaseData.ExecuteSQL/3] SQL successfully executed, but no data returned";
			}
			return returnCode;
		}

		public virtual int RowNumber // Override if needed
		{
			set { }
			get { return 0; }
		}

		public string Message 
		{
			get { return Tools.NullToString(returnMessage); }
		}

		public string SQLData
		{
			get { return Tools.NullToString(returnData); }
		}
	}
}
