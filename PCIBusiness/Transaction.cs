using System;
using System.Xml;


namespace PCIBusiness
{
	public abstract class Transaction : StdDisposable
	{
		protected string      otherRef;
		protected string      payRef;
		protected string      payToken;
		protected string      customerId;
		protected string      paymentMethodId;
		protected string      cardNumber;
		protected string      cardCVV;
//		protected string      authCode;
		protected string      resultCode;
		protected string      resultStatus;
		protected string      resultMsg;
		protected string      xmlSent;
		protected string      bureauCode;
		protected string      bureauCodeTokenizer;
		protected string      bureauURL;
		protected string      returnURL;
		protected string      strResult;
		protected XmlDocument xmlResult;

//	3d Stuff
		protected string      eci;
		protected string      paReq;
		protected string      termUrl;
		protected string      md;
		protected string      acsUrl;
		protected string      keyValuePairs;
		protected string      d3Form;

		public  string      OtherReference // For reversals, refunds, etc
		{
			get { return     Tools.NullToString(otherRef); }
		}
		public  string      PaymentReference
		{
			get { return     Tools.NullToString(payRef); }
		}
		public  string      PaymentToken
		{
			get { return     Tools.NullToString(payToken); }
		}
		public  string      CustomerId
		{
			get { return     Tools.NullToString(customerId); }
		}

		public  string      PaymentMethodId
		{
			get { return     Tools.NullToString(paymentMethodId); }
		}
		public  string      BureauCode
		{
			get { return     Tools.NullToString(bureauCode); }
		}
		public  virtual     string BureauURL
		{
			get { return     Tools.NullToString(bureauURL); }
		}
		public  string      CardNumber
		{
			get { return     Tools.NullToString(cardNumber); }
		}
		public  string      ResultCode
		{
			get { return     Tools.NullToString(resultCode); }
			set { resultCode = value.Trim(); }
		}
		public  string      ReturnURL
		{
			get { return     Tools.NullToString(returnURL); }
			set { returnURL = value.Trim(); }
		}
		public  string      ResultStatus
		{
			get { return     Tools.NullToString(resultStatus); }
		}
		public  string      ResultMessage
		{
			get { return     Tools.NullToString(resultMsg); }
		}
		public  string      ResultSummary
		{
			get
			{
				string tmp = ResultMessage + " / " + ResultStatus + " (" + ResultCode + ")";
				if ( tmp.StartsWith(" /") )
					tmp = tmp.Substring(3);
				if ( tmp.EndsWith("()") )
					tmp = tmp.Substring(0,tmp.Length-2);
				if ( tmp.StartsWith(" (") )
				{
					tmp = tmp.Substring(2);
					tmp = tmp.Substring(0,tmp.Length-1);
				}
				return tmp.Trim();
			}
		}
		public  string      XMLSent
		{
			get { return     Tools.NullToString(xmlSent); }
		}
		public  string      XMLResult
		{
			get
			{
				try
				{
					if ( xmlResult != null )
						return  xmlResult.InnerXml;
				}
				catch
				{ }
				try
				{
					if ( strResult != null )
						return  strResult.Trim();
				}
				catch
				{ }
				return "";
			}
		}

//		public  Constants.BureauStatus ProviderStatus
//		{
//			get
//			{
//				try
//				{
//				if ( Tools.NullToString(bureauCode).Length < 1 )
//					return Constants.BureauStatus.Unknown;
//				string status = Tools.ConfigValue("BureauStatus/"+bureauCode);
//				if ( status )
//			}
//		}

		public  bool      ThreeDRequired
		{
			get { return ( Tools.NullToString(acsUrl).Length > 0 ); }
		}
		public  string    ThreeDeci
		{
			get { return   Tools.NullToString(eci); }
		}
		public  string    ThreeDtermUrl
		{
			get { return   Tools.NullToString(termUrl); }
		}
		public  string    ThreeDpaReq
		{
			get { return   Tools.NullToString(paReq); }
		}
		public  string    ThreeDacsUrl
		{
			get { return   Tools.NullToString(acsUrl); }
		}
		public  string    ThreeDmd
		{
			get { return   Tools.NullToString(md); }
		}
		public  string    ThreeDKeyValuePairs
		{
			get { return   Tools.NullToString(keyValuePairs); }
		}
		public string     ThreeDSecureHTML
		{
			get { return   Tools.NullToString(d3Form); }
		}

		public virtual string WebForm
		{
			get { return ""; }
		}

		public virtual int GetToken(Payment payment)
		{
			return 14010;
		}

		public virtual int GetToken3rdParty(Payment payment)
		{
			return 14015;
		}

		public virtual int Detokenize(Payment payment)
		{
			return 14020;
		}

		public virtual int DeleteToken(Payment payment)
		{
			return 14025;
		}

		public virtual int TokenPayment(Payment payment)
		{
			return 14030;
		}

		public virtual int CardPayment(Payment payment)
		{
			return 14040;
		}

		public virtual int CardValidation(Payment payment)
		{
			return 14050;
		}

		public virtual int Refund(Payment payment)
		{
			return 14060;
		}

		public virtual int Reversal(Payment payment)
		{
			return 14070;
		}

		public virtual int Lookup(Payment payment)
		{
			return 14080;
		}

		public virtual int CardTest(Payment payment)
		{
			return 14090;
		}

		public virtual int CardPayment3rdParty(Payment payment)
		{
			return 14100;
		}

		public virtual int AccountUpdate(Payment payment)
		{
			return 14120;
		}

		public virtual int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			return 14130;
		}
		public virtual int ThreeDSecureCheck(string providerRef,string merchantRef="",string data1="",string data2="",string data3="")
		{
			return 14140;
		}

      public virtual bool EnabledFor3d(byte transactionType)
		{
			if ( transactionType != (byte)Constants.TransactionType.ManualPayment )
				return true;

			resultCode = "99999";
			resultMsg  = "3D Secure payments are not supported for this provider";
			return false;
		}

		protected void LoadBureauDetails(Constants.PaymentProvider bureau)
		{
			bureauCode = Tools.BureauCode(bureau);
			bureauURL  = Tools.ConfigValue(bureauCode+"/URL");

			if ( bureauURL.Length == 0 )
				bureauURL = Tools.BureauURL(Tools.BureauCode(bureau));
		}

      public override void Close()
		{
			xmlResult = null;
		}

//		protected string ClassName
//		{
//			get { return this.GetType().ToString(); }
//		}

		private void Clear()
		{
			bureauCodeTokenizer = Tools.BureauCode(Constants.PaymentProvider.TokenEx);
			bureauCode          = "";
			bureauURL           = "";
			payRef              = "";
			otherRef            = "";
			payToken            = "";
			customerId          = "";
			paymentMethodId     = "";
			resultCode          = "";
			resultMsg           = "";
			xmlSent             = "";
			strResult           = "";
			eci                 = "";
			paReq               = "";
			termUrl             = "";
			md                  = "";
			acsUrl              = "";
			keyValuePairs       = "";
			d3Form              = "";
			xmlResult           = null;
		}

		public Transaction()
		{
			Clear();
		}
		public Transaction(Constants.PaymentProvider provider)
		{
			Clear();
			LoadBureauDetails(provider);
		}
	}
}
