using System;

namespace PCIBusiness
{
	public class Provider : BaseData
	{
//	Bureau stuff
		private string  bureauCode;
		private string  bureauName;
		private string  bureauType;
		private byte    bureauStatus;
		private string  bureauURL;
		private string  bureauPort;
		private string  merchantKey;
		private string  userID;
		private string  userPassword;
		private string  userDescription;

//	Prosperian stuff
		private string  userCode;
		private string  sender;

//	Payment stuff
		private int     cardCount;
		private int     paymentCount;

		private Transaction transaction;

		public  string  BureauCode
		{
			set { bureauCode = value.Trim(); }
			get { return Tools.NullToString(bureauCode); }
		}
		public  string  BureauName
		{
			get { return Tools.NullToString(bureauName); }
		}
		public  string  BureauType
		{
			get { return Tools.NullToString(bureauType).ToUpper(); }
		}
		public  string  BureauURL
		{
			set { bureauURL = value.Trim(); }
			get { return Tools.NullToString(bureauURL); }
		}
		public  int     Port
		{
			get { return Tools.StringToInt(bureauPort); }
		}
		public  string  Sender
		{
			get { return Tools.NullToString(sender); }
		}
		public  string  UserCode
		{
			get { return Tools.NullToString(userCode); }
		}
		public  byte    BureauStatusCode
		{
			get
			{
				bureauStatus = 1; // Development
				if ( bureauType == "PAYMENT" )
					if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU)        ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB)         ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24)         ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate)      ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGenius)   ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate)     ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS)       ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Peach)       ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ecentric)    ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx)     ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA)  ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_EU)   ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_Asia) ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) ||
					     bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
						bureauStatus = 3; // Live
//					else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
//						bureauStatus = 2; // Disabled

				return bureauStatus;
			}
		}
		public  string  BureauStatusName
		{
			get 
			{
				if ( BureauStatusCode == 3 )
					return "Live";
				if ( BureauStatusCode == 2 )
					return "Disabled";
				return "In development";
			}
		}
//		public  string  MerchantAccount
//		{
//			get { return Tools.NullToString(merchantAccount); }
//		}
		public  string  MerchantKey
		{
			set { merchantKey = value.Trim(); }
			get { return Tools.NullToString(merchantKey); }
		}
		public  string  MerchantUserID
		{
			get { return Tools.NullToString(userID); }
		}
		public  string  MerchantPassword
		{
			get { return Tools.NullToString(userPassword); }
		}

		public Transaction Transaction
		{
			get
			{
				if ( transaction == null )
					transaction = Tools.CreateTransaction(bureauCode);
				return transaction;
			}
		}

		public bool ThreeDEnabled
		{
			get
			{
			//	Type classType  = (System.Reflection.Assembly.Load("PCIBusiness")).GetType("PCIBusiness.TransactionPayGate");
			//	Transaction x = (Transaction)Activator.CreateInstance(classType);
				if ( Transaction != null )
					return Transaction.EnabledFor3d((byte)Constants.TransactionType.ManualPayment);
				return false;
			}
		}

		public byte PaymentType
		{
		//	Change as required for each payment provider
			get
			{
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
					return (byte)Constants.TransactionType.CardPayment;
				return (byte)Constants.TransactionType.TokenPayment;
			}
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

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "Provider.LoadData";
			bureauStatus      = 0;

			if ( dbConn.ColStatus("BureauUserDescription") == Constants.DBColumnStatus.ColumnOK )
			{
//	Messaging providers
				bureauCode      = dbConn.ColString("BureauCode");
				bureauName      = dbConn.ColString("BureauName");
				userDescription = dbConn.ColString("BureauUserDescription");
				userID          = dbConn.ColString("BureauUserName");
				userPassword    = dbConn.ColString("BureauPassword");
				merchantKey     = dbConn.ColString("BureauPassword");
				bureauURL       = dbConn.ColString("BureauAddress");
				userCode        = dbConn.ColString("UserCode");
				bureauType      = dbConn.ColString("Type");
				sender          = dbConn.ColString("Sender",0,0);
				bureauPort      = dbConn.ColString("Port",0,0);
			}
			else
			{
				merchantKey     = dbConn.ColString("Safekey");
				bureauURL       = dbConn.ColString("url");
				userID          = dbConn.ColString("MerchantUserId",0,0);
				userPassword    = dbConn.ColString("MerchantUserPassword",0,0);
				bureauType      = "PAYMENT";
				bureauName      = "";
//				bureauCode      = dbConn.ColString("BureauCode",0);
			}
		}

      public override void CleanUp()
		{
			transaction = null;
		}

		public Provider() : base()
		{
			bureauType   = "PAYMENT";
			cardCount    = 0;
			paymentCount = 0;
			transaction  = null;
		}
	}
}
