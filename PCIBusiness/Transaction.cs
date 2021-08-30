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
//		protected string      authCode;
		protected string      resultCode;
		protected string      resultStatus;
		protected string      resultMsg;
		protected string      xmlSent;
		protected string      bureauCode;
		protected string      bureauCodeTokenizer;
		protected string      bureauURL;
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
		public  string      ResultStatus
		{
			get { return     Tools.NullToString(resultStatus); }
		}
		public  string      ResultMessage
		{
			get { return     Tools.NullToString(resultMsg); }
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

		public virtual int Refund(Payment payment)
		{
			return 14050;
		}

		public virtual int Reversal(Payment payment)
		{
			return 14060;
		}

		public virtual int Lookup(Payment payment)
		{
			return 14065;
		}

		public virtual int CardTest(Payment payment)
		{
			return 14070;
		}

		public virtual int CardPayment3rdParty(Payment payment)
		{
			return 14080;
		}

		public virtual int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			return 14510;
		}
		public virtual int ThreeDSecureCheck(string transID)
		{
			return 14610;
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

			if ( bureauURL.Length > 0 )
				return;

//	Providers where live and test are the same URL

			if ( bureau == Constants.PaymentProvider.PayGate )
				bureauURL = "https://secure.paygate.co.za/payhost/process.trans";

			else if ( bureau == Constants.PaymentProvider.Stripe_USA ||
			          bureau == Constants.PaymentProvider.Stripe_EU  ||
			          bureau == Constants.PaymentProvider.Stripe_Asia )
				bureauURL = "https://api.stripe.com";

			else if ( Tools.SystemIsLive() )
			{
				if ( bureau == Constants.PaymentProvider.Peach )
					bureauURL = "https://oppwa.com/v1";
				else if ( bureau == Constants.PaymentProvider.PayU )
					bureauURL = "https://secure.payu.co.za";
				else if ( bureau == Constants.PaymentProvider.TokenEx )
					bureauURL = "https://api.tokenex.com";
				else if ( bureau == Constants.PaymentProvider.FNB )
					bureauURL = "https://pay.ms.fnb.co.za";
				else if ( bureau == Constants.PaymentProvider.PaymentsOS )
					bureauURL = "https://api.paymentsos.com";
			}
			else // Testing
			{
				if ( bureau == Constants.PaymentProvider.Peach )
					bureauURL = "https://test.oppwa.com/v1";
				else if ( bureau == Constants.PaymentProvider.Ecentric )
					bureauURL = "https://sandbox.ecentricswitch.co.za:8443/paymentgateway/v1";
				else if ( bureau == Constants.PaymentProvider.eNETS )
					bureauURL = "https://uat-api.nets.com.sg:9065/GW2/TxnReqListener";
				else if ( bureau == Constants.PaymentProvider.PayGenius )
					bureauURL = "https://developer.paygenius.co.za";
				else if ( bureau == Constants.PaymentProvider.PayU )
					bureauURL = "https://staging.payu.co.za";
				else if ( bureau == Constants.PaymentProvider.T24 )
					bureauURL = "https://payment.ccp.transact24.com";
				else if ( bureau == Constants.PaymentProvider.TokenEx )
					bureauURL = "https://test-api.tokenex.com";
				else if ( bureau == Constants.PaymentProvider.FNB )
				//	bureauURL = "https://sandbox.ms.fnb.co.za/eCommerce/v2";
					bureauURL = "https://sandbox.ms.fnb.co.za";
				else if ( bureau == Constants.PaymentProvider.CyberSource )
					bureauURL = "https://apitest.cybersource.com";
				else if ( bureau == Constants.PaymentProvider.CyberSource_Moto )
					bureauURL = "https://apitest.cybersource.com";
			}
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
