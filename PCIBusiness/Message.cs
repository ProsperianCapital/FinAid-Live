using System;
using System.Net;

namespace PCIBusiness
{
	public abstract class Message : StdDisposable
	{
	//	Provider details
		protected Provider provider;
		protected string   providerID;
		protected string   userID;

	//	protected string   providerAddress;
	//	protected string   providerUserName;
	//	protected string   providerPassword;

	//	Result of Send()
		protected string resultMsg;
		protected string resultDetail;
		protected string resultCode;
		protected string resultID;

	//	Message details
	//	protected string messageCode;
		protected long   messageID;
		protected string messageBody;

		private DBConn   dbConn;

//		public string    ProviderAddress
//		{
//			get { return Tools.NullToString(providerAddress); }
//			set
//			{
//				if ( string.IsNullOrWhiteSpace(value) )
//					providerAddress = "";
//				else
//				{
//					providerAddress = value.Trim();
//					if ( providerAddress.EndsWith("/") || providerAddress.EndsWith("\\") )
//						providerAddress = providerAddress.Substring(0,providerAddress.Length-1);
//				}
//			}
//		}
		public string   UserID
		{
			get { return Tools.NullToString(userID); }
			set { userID = value.Trim(); }
		}
		public string   ProviderID
		{
			get { return Tools.NullToString(providerID); }
			set { providerID = value.Trim(); }
		}
		public int      ProviderCode
		{
			get { return Tools.StringToInt(providerID); }
		}
//		public string   ProviderPassword
//		{
//			get { return Tools.NullToString(providerPassword); }
//			set { providerPassword = value.Trim(); }
//		}

		public string   ResultMessage
		{
			get { return Tools.NullToString(resultMsg); }
		}
		public string   ResultDetail
		{
			get { return Tools.NullToString(resultDetail); }
		}
		public string   ResultCode
		{
			get { return Tools.NullToString(resultCode); }
		}
		public string   ResultID
		{
			get { return Tools.NullToString(resultID); }
		}

		public long     MessageID
		{
			get { return messageID; }
			set { messageID = value; }
		}
		public string   MessageBody
		{
			get { return Tools.NullToString(messageBody); }
			set { messageBody = value.Trim(); }
		}

		public abstract int Send(byte mode=0);

		public virtual byte LoadProvider()
		{
			if ( providerID.Length > 0 ) // && userID.Length > 0 )
				if ( provider == null || provider.BureauCode != providerID || provider.UserCode != userID )
					provider = (new Providers()).LoadOne(1,providerID,userID);

			if ( provider == null || provider.BureauCode != providerID || provider.UserCode != userID )
				return 10;

			return 0;
		}

		public abstract string Recipient { get; }

		public byte UpdateStatus()
		{
			if ( messageID < 1 )
				return 10;

			try
			{
//				string sql = "exec sp_eMailStatus_Upd @Key = "           + messageID.ToString()
//				           +                        ",@StatusCode = "    + Tools.DBString(resultCode)
//				           +                        ",@StatusMessage = " + Tools.DBString(resultMsg);
				string sql = "exec sp_Messaging_Upd_MessageSend"
				           +     " @MessageID="              + messageID.ToString()
				           +     ",@MessageBureauReference=" + Tools.DBString(resultCode);

				Tools.LogInfo("UpdateStatus/1",sql,Constants.LogSeverity,this);

				if ( ! Tools.OpenDB(ref dbConn) )
					return 20;
				else if ( ! dbConn.Execute(sql,true) )
					return 30;
				return 0;
			}
			catch (Exception ex)
			{ }
			finally
			{
				Tools.CloseDB(ref dbConn);
			}
			return 40;
		}

		public virtual void Clear()
		{
			Tools.CloseDB(ref dbConn);
			messageID    = 0;
			messageBody  = "";
			resultMsg    = "";
			resultDetail = "";
			resultCode   = "";
			resultID     = "";
		}

		public override void Close()
		{
			Clear();
			dbConn     = null;
			provider   = null;
			providerID = "";
			userID     = "";
		}

		public Message()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			messageID                            = 0;
			providerID                           = "";
			userID                               = "";
			provider                             = null;
		}
	}
}