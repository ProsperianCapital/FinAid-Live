using System;

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
		public  string  BureauURL
		{
			get { return Tools.NullToString(providerURL); }
		}
		public  byte    BureauStatusCode
		{
			get
			{
				bureauStatus = 1; // Development
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU)      ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24)       ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate)    ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGenius) ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate)   ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS)     ||
				     bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ecentric) )
					bureauStatus = 3; // Live
//				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
//					bureauStatus = 2; // Disabled
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
			get { return Tools.NullToString(merchantKey); }
		}
		public  string  MerchantUserID
		{
			get { return Tools.NullToString(userID); }
		}

		public Transaction Transaction
		{
			get
			{
				if ( transaction == null )
					if      ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU)      ) transaction = new TransactionPayU();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24)       ) transaction = new TransactionT24();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate)    ) transaction = new TransactionMyGate();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGenius) ) transaction = new TransactionPayGenius();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate)   ) transaction = new TransactionPayGate();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS)     ) transaction = new TransactionENets();
				   else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ecentric)  ) transaction = new TransactionEcentric();
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
			merchantKey       = dbConn.ColString("Safekey");
			providerURL       = dbConn.ColString("url");
			userID            = dbConn.ColString("MerchantUserId",0);
			userPassword      = dbConn.ColString("MerchantUserPassword",0);
			bureauName        = "";
			bureauStatus      = 0;
		}
      public override void CleanUp()
		{
			transaction = null;
		}

		public Provider() : base()
		{
			cardCount    = 0;
			paymentCount = 0;
			transaction  = null;
		}
	}
}
