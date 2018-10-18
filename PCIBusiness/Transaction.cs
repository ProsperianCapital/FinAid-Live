using System;
using System.Xml;


namespace PCIBusiness
{
	public abstract class Transaction : StdDisposable
	{
		protected string      payRef;
		protected string      payToken;
		protected string      resultCode;
		protected string      resultMsg;
		protected string      xmlSent;
		protected string      bureauCode;
		protected string      strResult;
		protected XmlDocument xmlResult;

		public  string      PaymentReference
		{
			get { return     Tools.NullToString(payRef); }
		}
		public  string      BureauCode
		{
			get { return     Tools.NullToString(bureauCode); }
		}
		public  string      PaymentToken
		{
			get { return     Tools.NullToString(payToken); }
		}
		public  string      ResultCode
		{
			get { return     Tools.NullToString(resultCode); }
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
					return     xmlResult.InnerXml;
				}
				catch
				{ }
				try
				{
					return     Tools.NullToString(strResult);
				}
				catch
				{ }
				return "";
			}
		}
//		public  XmlDocument XMLResult
//		{
//			get { return     xmlResult; }
//		}

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

		public virtual int GetToken(Payment payment)
		{
			return 0;
		}

		public virtual int ProcessPayment(Payment payment)
		{
			return 0;
		}

      public override void Close()
		{
			xmlResult = null;
		}

		public Transaction()
		{
			payRef      = "";
			payToken    = "";
			resultCode  = "";
			resultMsg   = "";
			xmlSent     = "";
			bureauCode  = "";
			strResult   = "";
			xmlResult   = null;
		}
	}
}
