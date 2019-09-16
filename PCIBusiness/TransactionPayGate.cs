using System;
using System.Xml;
using System.Net;
using PCIBusiness.PayGateVault;

namespace PCIBusiness
{
	public class TransactionPayGate : Transaction
	{
		private string nsPrefix;
		private string nsURL;

		public  bool Successful
		{
			get
			{
				resultCode   = Tools.NullToString(resultCode);
				resultStatus = Tools.NullToString(resultStatus);
				if ( resultCode == "990017" || resultCode == "190988" ) // || resultStatus.ToUpper().StartsWith("COMPLETE") )
					return true;
				return false;
			}
		}
      public override bool EnabledFor3d(byte transactionType)
		{
			return true;
		}

		public int GetTokenV1(Payment payment) // NOT USED, left in to keep interface the same
		{
			return 0;
		}


		public int GetTokenV2(Payment payment) // NOT USED, left in to keep interface the same
		{
			return 0;
		}


		public override int GetToken(Payment payment)
		{
			int ret  = 300;
			payToken = "";

			try
			{
				Tools.LogInfo("TransactionPayGate.GetToken/10","Merchant Ref=" + payment.MerchantReference,199);

				xmlSent = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'"
				        +                  " xmlns:pay='http://www.paygate.co.za/PayHOST'>"
				        + "<soapenv:Header />"
				        + "<soapenv:Body>"
				        + "<pay:SingleVaultRequest>"
				        + "<pay:CardVaultRequest>"
				        +   "<pay:Account>"
				        +     "<pay:PayGateId>" + Tools.XMLSafe(payment.ProviderUserID) + "</pay:PayGateId>"
				        +     "<pay:Password>" + Tools.XMLSafe(payment.ProviderPassword) + "</pay:Password>"
				        +   "</pay:Account>"
				        +   "<pay:CardNumber>" + Tools.XMLSafe(payment.CardNumber) + "</pay:CardNumber>"
				        +   "<pay:CardExpiryDate>" + Tools.XMLSafe(payment.CardExpiryMM) + Tools.XMLSafe(payment.CardExpiryYYYY) + "</pay:CardExpiryDate>"
				        + "</pay:CardVaultRequest>"
				        + "</pay:SingleVaultRequest>"
				        + "</soapenv:Body>"
				        + "</soapenv:Envelope>"; 

				ret      = CallWebService(payment);
				payToken = Tools.XMLNode(xmlResult,"VaultId",nsPrefix,nsURL);

				if ( ! Successful || payToken.Length < 1 )
					Tools.LogInfo("TransactionPayGate.GetToken/20","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGate.GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGate.GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int DeleteToken(Payment payment)
		{
			int ret = 300;

			try
			{
				Tools.LogInfo("TransactionPayGate.DeleteToken/10","Merchant Ref=" + payment.MerchantReference,199);

				xmlSent = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'"
				        +                  " xmlns:pay='http://www.paygate.co.za/PayHOST'>"
				        + "<soapenv:Header />"
				        + "<soapenv:Body>"
				        + "<pay:SingleVaultRequest>"
				        +	"<pay:DeleteVaultRequest>"
				        +		"<pay:Account>"
				        +			"<pay:PayGateId>" + Tools.XMLSafe(payment.ProviderUserID) + "</pay:PayGateId>"
				        +			"<pay:Password>" + Tools.XMLSafe(payment.ProviderPassword) + "</pay:Password>"
				        +		"</pay:Account>"
				        +		"<pay:VaultId>" + Tools.XMLSafe(payment.CardToken) + "</pay:VaultId>"
				        +	"</pay:DeleteVaultRequest>"
				        + "</pay:SingleVaultRequest>"
				        + "</soapenv:Body>"
				        + "</soapenv:Envelope>"; 
				ret     = CallWebService(payment);

				if ( ! Successful )
					Tools.LogInfo("TransactionPayGate.DeleteToken/20","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGate.DeleteToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGate.DeleteToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int ProcessPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 600;
			payRef  = "";

			Tools.LogInfo("TransactionPayGate.ProcessPayment/10","Merchant Ref=" + payment.MerchantReference,10);

			try
			{
				if ( payment.TransactionType == (byte)Constants.TransactionType.ManualPayment ) // Manual card payment
					xmlSent = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'"
					        +                  " xmlns:pay='http://www.paygate.co.za/PayHOST'>"
					        + "<soapenv:Header />"
					        + "<soapenv:Body>"
					        + "<pay:SinglePaymentRequest>"
					        + "<pay:CardPaymentRequest>"
					        +   "<pay:Account>"
					        +     "<pay:PayGateId>" + Tools.XMLSafe(payment.ProviderUserID) + "</pay:PayGateId>"
					        +     "<pay:Password>" + Tools.XMLSafe(payment.ProviderPassword) + "</pay:Password>"
					        +   "</pay:Account>"
					        +   "<pay:Customer>"
					        +     "<pay:FirstName>" + Tools.XMLSafe(payment.FirstName) + "</pay:FirstName>"
					        +     "<pay:LastName>" + Tools.XMLSafe(payment.LastName) + "</pay:LastName>"
					        +     "<pay:Email>" + Tools.XMLSafe(payment.EMail) + "</pay:Email>"
					        +   "</pay:Customer>"
					        +   "<pay:CardNumber>" + Tools.XMLSafe(payment.CardNumber) + "</pay:CardNumber>"
					        +   "<pay:CardExpiryDate>" + Tools.XMLSafe(payment.CardExpiryMM) + Tools.XMLSafe(payment.CardExpiryYYYY) + "</pay:CardExpiryDate>"
					        +   "<pay:CVV>" + Tools.XMLSafe(payment.CardCVV) + "</pay:CVV>"
					        +   "<pay:Redirect>"
					        +     "<pay:NotifyUrl>" + Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef=" + Tools.XMLSafe(payment.MerchantReference) + "</pay:NotifyUrl>"
					        +     "<pay:ReturnUrl>" + Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef=" + Tools.XMLSafe(payment.MerchantReference) + "</pay:ReturnUrl>"
					        +   "</pay:Redirect>"
					        +   "<pay:Order>"
					        +     "<pay:MerchantOrderId>" + Tools.XMLSafe(payment.MerchantReference) + "</pay:MerchantOrderId>"
					        +     "<pay:Currency>" + Tools.XMLSafe(payment.CurrencyCode) + "</pay:Currency>"
					        +     "<pay:Amount>" + payment.PaymentAmount.ToString() + "</pay:Amount>"
					        +   "</pay:Order>"
					        + "</pay:CardPaymentRequest>"
					        + "</pay:SinglePaymentRequest>"
					        + "</soapenv:Body>"
					        + "</soapenv:Envelope>";

//	Not needed
//					        +   "<pay:ThreeDSecure>"
//					        +     "<pay:BillingDescriptor>Prosperian Capital</pay:BillingDescriptor>"
//					        +     "<pay:Enrolled>Y</pay:Enrolled>"
//					        +   "</pay:ThreeDSecure>"
//					        +   "<pay:Redirect>"
//					        +     "<pay:NotifyUrl>http://www.paulkilfoil.co.za</pay:NotifyUrl>"
//					        +     "<pay:ReturnUrl>http://www.paulkilfoil.co.za</pay:ReturnUrl>"
//					        +   "</pay:Redirect>"
				else
					xmlSent = SetUpXML(payment,1)
					        + "<pay:VaultId>" + Tools.XMLSafe(payment.CardToken) + "</pay:VaultId>"
					        + "<pay:CVV>" + Tools.XMLSafe(payment.CardCVV) + "</pay:CVV>"
					        + SetUpXML(payment,2);

				ret    = CallWebService(payment);
				payRef = Tools.XMLNode(xmlResult,"PayRequestId",nsPrefix,nsURL);

				if ( ! Successful )
					Tools.LogInfo("TransactionPayGate.ProcessPayment/21","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
				else if ( payment.TransactionType == (byte)Constants.TransactionType.CardPayment && payRef.Length < 1 )
					Tools.LogInfo("TransactionPayGate.ProcessPayment/22","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
				else if ( payment.TransactionType == (byte)Constants.TransactionType.TokenPayment && payRef.Length < 1 )
					Tools.LogInfo("TransactionPayGate.ProcessPayment/23","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
				else if ( payment.TransactionType == (byte)Constants.TransactionType.ManualPayment && keyValuePairs.Length < 1 )
					Tools.LogInfo("TransactionPayGate.ProcessPayment/24","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGate.ProcessPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGate.ProcessPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		private string SetUpXML(Payment payment,byte section)
		{
			if ( section == 1 )
				return "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'"
				     +                  " xmlns:pay='http://www.paygate.co.za/PayHOST'>"
				     + "<soapenv:Header />"
				     + "<soapenv:Body>"
				     + "<pay:SinglePaymentRequest>"
				     +   "<pay:CardPaymentRequest>"
				     +     "<pay:Account>"
				     +       "<pay:PayGateId>" + Tools.XMLSafe(payment.ProviderUserID) + "</pay:PayGateId>"
				     +       "<pay:Password>" + Tools.XMLSafe(payment.ProviderPassword) + "</pay:Password>"
				     +     "</pay:Account>"
				     +     "<pay:Customer>"
				     +       "<pay:FirstName>" + Tools.XMLSafe(payment.FirstName) + "</pay:FirstName>"
				     +       "<pay:LastName>" + Tools.XMLSafe(payment.LastName) + "</pay:LastName>"
				     +       "<pay:Mobile>" + Tools.XMLSafe(payment.PhoneCell) + "</pay:Mobile>"
				     +       "<pay:Email>" + Tools.XMLSafe(payment.EMail) + "</pay:Email>"
				     +     "</pay:Customer>";
			else
				return     "<pay:BudgetPeriod>0</pay:BudgetPeriod>"
				     +     "<pay:Order>"
				     +       "<pay:MerchantOrderId>" + Tools.XMLSafe(payment.MerchantReference) + "</pay:MerchantOrderId>"
				     +       "<pay:Currency>" + Tools.XMLSafe(payment.CurrencyCode) + "</pay:Currency>"
				     +       "<pay:Amount>" + payment.PaymentAmount.ToString() + "</pay:Amount>"
				     +     "</pay:Order>"
				     +   "</pay:CardPaymentRequest>"
				     + "</pay:SinglePaymentRequest>"
				     + "</soapenv:Body>"
				     + "</soapenv:Envelope>";
		}

		private int CallWebService(Payment payment) // string url)
      {
			int    ret    = 10;
			string url    = payment.ProviderURL;
			keyValuePairs = "";

			if ( Tools.NullToString(url).Length == 0 )
//				PayGate use the same URL for live and test
				url = "https://secure.paygate.co.za/payhost/process.trans";

			try
			{
				using ( WebClient wc = new WebClient() )
				{
					Tools.LogInfo("TransactionPayGate.CallWebService/10",payment.TransactionTypeName+", XML Sent="+xmlSent,10);

					ret           = 20;
					wc.Encoding   = System.Text.Encoding.UTF8;
					string xmlOut = wc.UploadString(url,xmlSent);

				//	VB sample code
				//	Dim _byteOrderMarkUtf8 As String = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble())
				//	If retString.StartsWith(_byteOrderMarkUtf8) Then
				//		retString = retString.Remove(0, _byteOrderMarkUtf8.Length)
				//	End If

					ret          = 30;
					xmlResult    = new XmlDocument();
					xmlResult.LoadXml(xmlOut);
					ret          = 40;
					resultCode   = Tools.XMLNode(xmlResult,"ResultCode"       ,nsPrefix,nsURL);
					resultMsg    = Tools.XMLNode(xmlResult,"ResultDescription",nsPrefix,nsURL);
					resultStatus = Tools.XMLNode(xmlResult,"StatusName"       ,nsPrefix,nsURL);
					ret          = 50;

					Tools.LogInfo("TransactionPayGate.CallWebService/50",payment.TransactionTypeName+", XML Rec="+xmlOut,255);

					if ( payment.TransactionType == (byte)Constants.TransactionType.ManualPayment &&
					     resultStatus.ToUpper() == ("ThreeDSecureRedirectRequired").ToUpper() )
					{
						string              key;
						string              value;
						XmlNodeList         keyValues;
						XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlResult.NameTable);
						ret                       = 60;
						nsmgr.AddNamespace(nsPrefix,nsURL);
						keyValues                 = xmlResult.SelectNodes("//"+nsPrefix+":UrlParams",nsmgr);
						acsUrl                    = Tools.XMLNode(xmlResult,"RedirectUrl",nsPrefix,nsURL);

						foreach ( XmlNode keyValue in keyValues )
						{
							ret           = 70;
							key           = keyValue.SelectSingleNode(nsPrefix+":key"  ,nsmgr).InnerText;
							value         = keyValue.SelectSingleNode(nsPrefix+":value",nsmgr).InnerText;
// Template ...		keyValuePairs = keyValuePairs + "<input type='hidden' name='key' value='value' />";
							keyValuePairs = keyValuePairs + "<input type='hidden' name='" + Tools.XMLSafe(key) + "'"
							                              +                    " value='" + Tools.XMLSafe(value) + "' />";
						}
						nsmgr      = null;
						ret        = 0;
						resultCode = "190988";
					}

					else if ( Successful )
					{
						if ( resultMsg.Length < 1 )
							resultMsg = Tools.XMLNode(xmlResult,"StatusDetail",nsPrefix,nsURL);
						resultCode   = "990017";
						ret          = 0;
					}

					else if ( resultCode.Length == 0 && resultMsg.Length == 0 )
					{
						ret        = 80;
						resultCode = Tools.XMLNode(xmlResult,"faultcode"); // Namespace not needed
						resultMsg  = Tools.XMLNode(xmlResult,"faultstring");
						if ( resultCode.Length == 0 && resultMsg.Length == 0 )
						{
							ret        = 90;
							resultCode = Tools.XMLNode(xmlResult,"StatusName"  ,nsPrefix,nsURL);
							resultMsg  = Tools.XMLNode(xmlResult,"StatusDetail",nsPrefix,nsURL);
							if ( resultCode.ToUpper().StartsWith("COMPLETE") )
							{
								ret        = 0;
								resultCode = "990017";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGate.CallWebService/98","ret="+ret.ToString(),220);
				Tools.LogException("TransactionPayGate.CallWebService/99","ret="+ret.ToString(),ex);
			}
			return ret;
		}

		public TransactionPayGate() : base()
		{
			bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayGate);

		//	Force TLS 1.2
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;

		//	Namespace for result XML
			nsPrefix = "ns2";
			nsURL    = "http://www.paygate.co.za/PayHOST";
		}
	}
}
