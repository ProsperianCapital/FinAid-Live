using System;
using System.Text;

namespace PCIBusiness
{
	public class Provider : BaseData
	{
		private string  bureauCode;
		private string  bureauName;
		private byte    bureauStatus;
		private string  providerURL;
		private string  merchantKey;
//		private string  merchantAccount;
		private string  userID;
		private string  userPassword;
		private int     cardCount;
		private int     paymentCount;

		public  string  BureauCode
		{
			set { bureauCode = value.Trim(); }
			get { return Tools.NullToString(bureauCode); }
		}
		public  string  BureauName
		{
			get { return Tools.NullToString(bureauName); }
		}
		public  string  BureauURL
		{
			get { return Tools.NullToString(providerURL); }
		}
		public  byte    BureauStatusCode
		{
			get { return bureauStatus; }
		}
		public  string  BureauStatusName
		{
			get 
			{
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24)  ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate) )
					return "Live";
				return "In development";
			}
		}
//		public  string  MerchantAccount
//		{
//			get { return Tools.NullToString(merchantAccount); }
//		}
		public  string  MerchantKey
		{
			get { return Tools.NullToString(merchantKey); }
		}
		public  string  MerchantUserID
		{
			get { return Tools.NullToString(userID); }
		}

		public int CardsToBeTokenized
		{
			set { cardCount = value; }
			get { return cardCount; }
		}

		public int PaymentsToBeProcessed
		{
			set { paymentCount = value; }
			get { return paymentCount; }
		}

//		public string ConnectionDetails(byte mode,string separator="")
//		{
//			if ( mode == 1 ) // HTML
//				return "<table>"
//					  + "<tr><td>Payment Provider</td><td class='Red'> : " + BureauName + "</td></tr>"
//					  + "<tr><td>Bureau Code</td><td class='Red'> : " + BureauCode + "</td></tr>"
//					  + "<tr><td>Status</td><td class='Red'> : " + StatusName + "</td></tr>"
//					  + "<tr><td colspan='2'><hr /></td></tr>"
//					  + "<tr><td>Go To URL</td><td> : " + "" + "</td></tr>"
//					  + "<tr><td>Return To URL</td><td> : " + "" + "</td></tr>"
//					  + "<tr><td>User ID</td><td> : " + "" + "</td></tr>"
//					  + "<tr><td>Password</td><td> : " + "" + "</td></tr>"
//					  + "</table>";
//
//			if ( Tools.NullToString(separator).Length < 1 )
//				separator = Environment.NewLine;
//
//			return "Payment Provider : " + BureauName + separator
//			     + "Bureau Code : " + BureauCode + separator
//			     + "URL : " + separator
//			     + "User ID : " + separator
//			     + "Password : ";
//		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "Provider.LoadData";
			merchantKey       = dbConn.ColString("Safekey");
			providerURL       = dbConn.ColString("url");
			userID            = dbConn.ColString("MerchantUserId");
			userPassword      = dbConn.ColString("MerchantUserPassword");
			bureauName        = "";
			bureauStatus      = 0;
		}

		public Provider() : base()
		{
			cardCount    = 0;
			paymentCount = 0;
		}
	}
}
