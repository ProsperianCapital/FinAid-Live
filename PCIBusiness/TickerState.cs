using System;

namespace PCIBusiness
{
	public class TickerState : BaseData
	{
		private string    dbConnection;
		private string    tickerCode;
		private string    tickerStatus;
		private string    userCode;
		private string    origin;
//		private DateTime  dateUpdated;

		public  string    DBConnection
		{
			get { return   Tools.NullToString(dbConnection); }
			set { dbConnection = value.Trim(); }
		}
		public  string    TickerCode
		{
			get { return   Tools.NullToString(tickerCode); }
			set { tickerCode = value.Trim(); }
		}
		public  string    TickerStatus
		{
			get { return   Tools.NullToString(tickerStatus); }
			set { tickerStatus = value.Trim(); }
		}
		public  string    UserCode
		{
			get { return   Tools.NullToString(userCode); }
			set { userCode = value.Trim(); }
		}
		public  string    Origin
		{
			get { return   Tools.NullToString(origin); }
			set { origin = value.Trim(); }
		}

		private int DoSQL()
		{
			try
			{
				Tools.LogInfo("TickerState.DoSQL/1",sql,10);
				int  ret = ExecuteSQL(null,1,false,DBConnection);
				if ( ret == 0 )
				{
					LoadData(dbConn);
					Tools.LogInfo("TickerState.DoSQL/2","Success, tickerCode="+tickerCode + " / tickerStatus=" + tickerStatus,10);
				}
				else
					Tools.LogInfo("TickerState.DoSQL/3","Error, ret="+ret.ToString() + " (" + ReturnMessage + ")",222);
				return ret;
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TickerState.DoSQL/8",ex.Message,222);
				Tools.LogException("TickerState.DoSQL/9",sql,ex);
			}
			finally
			{
				Tools.CloseDB(ref dbConn);
			}
			return 205;
		}

		public int Update()
		{
			if ( TickerStatus.Length < 1 )
				return 103;

			sql = "exec sp_TickerState  @TickerStatus=" + Tools.DBString(TickerStatus)
			                        + ",@TickerCode="   + Tools.DBString(TickerCode)
			                        + ",@UserCode="     + Tools.DBString(UserCode)
			                        + ",@ActionOrigin=" + Tools.DBString(Origin);
			return DoSQL();
		}

		public int Enquire()
		{
			sql = "exec sp_TickerState";
			return DoSQL();
		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "TickerState.LoadData";
			tickerCode        = dbConn.ColString("TickerCode");
			tickerStatus      = dbConn.ColString("TickerStatus");
//	May not be returned, so don't log errors
			userCode          = dbConn.ColString("UserCode",0,0);
			origin            = dbConn.ColString("ActionOrigin",0,0);
//			dateUpdated       = dbConn.ColDate  ("ActionDate",0,0);

			Tools.LogInfo("TickerState.LoadData","tickerCode="+tickerCode+", tickerStatus="+tickerStatus,10);
		}
	}
}
