using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace PCIBusiness
{
	public class TransactionEcentric : Transaction
	{
	//	private X509Certificate            cert1;
	//	private X509Certificate            cert2;
		private X509Certificate2Collection certs;
		private string                     xmlHeader;
		private string                     tranStatus;
		private string                     nsURL;
		private string                     funcName;

		public  bool Successful
		{
			get { return Tools.NullToString(tranStatus) == "SUCCESS"; }
		}

//		private int GetTokenV1(Payment payment,int doNotUse) // Version 1, not used
//		{
//			int ret  = 300;
//			payToken = "";
//
//			try
//			{
//				Tools.LogInfo("TransactionEcentric.GetToken/10","Merchant Ref=" + payment.MerchantReference,199);
//
//				xmlSent = xmlHeader.Replace("#TransRef#",Tools.XMLSafe(payment.MerchantReference))
//				        + "<s:Body>"
//				        + "<AddCardRequest xmlns='" + nsURL + "'>"
//				        +	"<MerchantID>" + Tools.XMLSafe(payment.ProviderKey) + "</MerchantID>"
//				        +	"<MerchantUserID>" + Tools.XMLSafe(payment.ProviderUserID) + "</MerchantUserID>"
//				        +	"<Card>"
//				        +		"<CardholderName>" + Tools.XMLSafe(payment.CardName) + "</CardholderName>"
//				        +		"<CardNumber>" + Tools.XMLSafe(payment.CardNumber) + "</CardNumber>"
//				        +		"<ExpiryMonth>" + Tools.XMLSafe(payment.CardExpiryMM) + "</ExpiryMonth>"
//				        +		"<ExpiryYear>" + Tools.XMLSafe(payment.CardExpiryYY) + "</ExpiryYear>"
//				        +	"</Card>"
//				        + "</AddCardRequest>"
//				        + "</s:Body>"
//				        + "</s:Envelope>";
//
//				ret      = CallWebService(payment.ProviderURL,"AddCard");
//				payRef   = "";
//				payToken = Tools.XMLNode(xmlResult,"Token");
//
//				if ( Successful && payToken.Length > 0 )
//				{
//				//	Do an initial transaction
//					Tools.LogInfo("TransactionEcentric.GetToken/20","Token=" + payToken,199);
//					ret      = 400;
//					xmlSent  = xmlHeader.Replace("#TransRef#",Tools.XMLSafe(payment.MerchantReference))
//					         + "<s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>"
//					         + "<PaymentRequest xmlns='" + nsURL + "'>"
//					         +   "<MerchantID>" + Tools.XMLSafe(payment.ProviderKey) + "</MerchantID>"
//					         +   "<TransactionID>" + Tools.XMLSafe(payment.TransactionID) + "</TransactionID>"
//					         +   "<OrderNumber>" + Tools.XMLSafe(payment.MerchantReference) + "</OrderNumber>"
//				            +   "<TransactionDateTime>" + Tools.DateToString(System.DateTime.Now,7,0)
//				                                        + "T"
//				                                        + Tools.DateToString(System.DateTime.Now,0,5) + "</TransactionDateTime>"
//					         +   "<Amount>1</Amount>"
//				            +   "<CurrencyCode>" + Tools.XMLSafe(payment.CurrencyCode) + "</CurrencyCode>"
//	//	//			         +   "<PaymentService>CardNotPresentMotoRecurring</PaymentService>"
//					         +   "<PaymentService>CardNotPresentMotoRecurring</PaymentService>"
//					         +   "<Card>"
//					         +     "<Token>" + Tools.XMLSafe(payToken) + "</Token>"
//					         +     "<SecurityCode>" + Tools.XMLSafe(payment.CardCVV) + "</SecurityCode>"
//					         +   "</Card>"
//	//	//			         +   "<BankAccount>Credit</BankAccount>"
//					         +   "<FirstName>" + Tools.XMLSafe(payment.FirstName) + "</FirstName>"
//					         +   "<LastName>" + Tools.XMLSafe(payment.LastName) + "</LastName>"
//					         +   "<Email>" + Tools.XMLSafe(payment.EMail) + "</Email>"
//					         +   "<MobilePhone>" + Tools.XMLSafe(payment.PhoneCell) + "</MobilePhone>"
//					         + "</PaymentRequest>"
//					         + "</s:Body>"
//					         + "</s:Envelope>";
//					ret      = CallWebService(payment.ProviderURL,"Payment");
//					payRef   = Tools.XMLNode(xmlResult,"ReconID");
//	//	//			authCode = Tools.XMLNode(xmlResult,"AuthCode");
//	//	//			if ( ! Successful || authCode.Length < 1 )
//					if ( ! Successful || payRef.Length < 1 )
//					{
//						ret = ( ret > 0 ? ret : 430 );
//						Tools.LogInfo("TransactionEcentric.GetToken/30","XML Received="+XMLResult,199);
//					}
//				}
//				else
//					Tools.LogInfo("TransactionEcentric.GetToken/40","XML Received="+XMLResult,199);
//			}
//			catch (Exception ex)
//			{
//				Tools.LogInfo("TransactionEcentric.GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
//				Tools.LogException("TransactionEcentric.GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
//			}
//			return ret;
//		}

		public override int GetToken(Payment payment) // Version 2
		{
			int ret  = 300;
			payToken = "";
			funcName = ClassName + ".GetToken/";

			try
			{
				Tools.LogInfo(funcName+"10","Merchant Ref=" + payment.MerchantReference,10);

				xmlSent  = xmlHeader.Replace("#TransRef#",Tools.XMLSafe(payment.MerchantReference))
				         + "<s:Body>"
//				         + "<AddCardRequest xmlns='http://www.ecentricswitch.co.za/paymentgateway/v1'>"
				         + "<AddCardRequest xmlns='" + nsURL + "'>"
				         +   "<MerchantID>" + Tools.XMLSafe(payment.ProviderKey) + "</MerchantID>"
				         +   "<MerchantUserID>" + Tools.XMLSafe(payment.ProviderUserID) + "</MerchantUserID>"
				         +   "<Card>"
				         +		"<CardholderName>" + Tools.XMLSafe(payment.CardName) + "</CardholderName>"
				         +		"<CardNumber>" + Tools.XMLSafe(payment.CardNumber) + "</CardNumber>"
				         +		"<ExpiryMonth>" + Tools.XMLSafe(payment.CardExpiryMM) + "</ExpiryMonth>"
				         +		"<ExpiryYear>" + Tools.XMLSafe(payment.CardExpiryYY) + "</ExpiryYear>"
				         +   "</Card>"
				         + "</AddCardRequest>"
				         + "</s:Body>"
				         + "</s:Envelope>";

				ret      = 310;
				Tools.LogInfo(funcName+"20","XML Sent=" + xmlSent,10);
				ret      = CallWebService(payment.ProviderURL,"AddCard");
				payToken = Tools.XMLNode(xmlResult,"Token");
				Tools.LogInfo(funcName+"30","XML Rec=" + XMLResult,10);

				if ( ! Successful || payToken.Length < 1 )
				{
					ret = ( ret > 0 ? ret : 330 );
					Tools.LogInfo(funcName+"60","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
				}
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,funcName+"97",xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     (funcName+"98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException(funcName+"99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex2);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret  = 600;
			payRef   = "";
//			authCode = "";
			funcName = ClassName + ".TokenPayment/";

			Tools.LogInfo(funcName+"10","Merchant Ref=" + payment.MerchantReference,10);

			try
			{
				xmlSent = xmlHeader.Replace("#TransRef#",Tools.XMLSafe(payment.MerchantReference))
				        + "<s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>"
				        + "<PaymentRequest xmlns='" + nsURL + "'>"
				        +   "<MerchantID>" + Tools.XMLSafe(payment.ProviderKey) + "</MerchantID>"
				        +   "<TransactionID>" + Tools.XMLSafe(payment.TransactionID) + "</TransactionID>"
				        +   "<OrderNumber>" + Tools.XMLSafe(payment.MerchantReference) + "</OrderNumber>"
			           +   "<TransactionDateTime>" + Tools.DateToString(System.DateTime.Now,7,0)
			                                       + "T"
			                                       + Tools.DateToString(System.DateTime.Now,0,5) + "</TransactionDateTime>"
				        +   "<Amount>" + payment.PaymentAmount.ToString() + "</Amount>"
				        +   "<CurrencyCode>" + Tools.XMLSafe(payment.CurrencyCode) + "</CurrencyCode>"
				        +   "<PaymentService>CardNotPresentMotoRecurring</PaymentService>"
				        +   "<Card>"
				        +     "<Token>" + Tools.XMLSafe(payment.CardToken) + "</Token>"
				        +     "<SecurityCode>" + Tools.XMLSafe(payment.CardCVV) + "</SecurityCode>"
				        +   "</Card>"
				        +   "<FirstName>" + Tools.XMLSafe(payment.FirstName) + "</FirstName>"
				        +   "<LastName>" + Tools.XMLSafe(payment.LastName) + "</LastName>"
				        +   "<Email>" + Tools.XMLSafe(payment.EMail) + "</Email>"
				        +   "<MobilePhone>" + Tools.XMLSafe(payment.PhoneCell) + "</MobilePhone>"
				        + "</PaymentRequest>"
				        + "</s:Body>"
				        + "</s:Envelope>";

				Tools.LogInfo(funcName+"20","XML Sent=" + xmlSent,10);
				ret     = CallWebService(payment.ProviderURL,"Payment");
				payRef  = Tools.XMLNode(xmlResult,"ReconID");
				Tools.LogInfo(funcName+"30","XML Rec=" + XMLResult,10);

				if ( ! Successful || payRef.Length < 1 )
				{
					ret = ( ret > 0 ? ret : 630 );
					Tools.LogInfo(funcName+"60","XML Sent="+xmlSent+", XML Rec="+XMLResult,199);
				}
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,funcName+"97",xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     (funcName+"98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException(funcName+"99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex2);
			}
			return ret;
		}

		private int CallWebService(string url,string soapAction)
      {
			int ret  = 10;
			funcName = ClassName + ".CallWebService/";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;
			//	url = "https://sandbox.ecentricswitch.co.za:8443/paymentgateway/v1";

			try
			{
				byte[]         page       = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.Headers.Add(@"SOAP:Action");
				webRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
//				webRequest.Headers.Add("SOAPAction","\""+soapAction+"\"");
				webRequest.Headers.Add("SOAPAction",soapAction);
				webRequest.ContentType = "text/xml;charset=\"utf-8\"";
				webRequest.Accept      = "text/xml";
				webRequest.Method      = "POST";

				Tools.LogInfo(funcName+"10","URL=" + url,10);
				ret                           = 20;
            webRequest.ClientCertificates = certs;
				Tools.LogInfo(funcName+"20","Cert count=" + certs.Count.ToString(),10);

				using (Stream stream = webRequest.GetRequestStream())
				{
					ret = 110;
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				ret = 120;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 130;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 140;
						strResult = rd.ReadToEnd();
					}
				}

				Tools.LogInfo(funcName+"50","XML Rec="+strResult,50);

				ret        = 150;
				xmlResult  = new XmlDocument();
				xmlResult.LoadXml(strResult);
				ret        = 160;
				tranStatus = Tools.XMLNode(xmlResult,"TransactionStatus").ToUpper();
				ret        = 170;
				resultCode = Tools.XMLNode(xmlResult,"Code").ToUpper();
				resultMsg  = Tools.XMLNode(xmlResult,"Description");
				ret        = 0;
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,funcName+"97","ret="+ret.ToString());
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     (funcName+"98","ret="+ret.ToString(),220);
				Tools.LogException(funcName+"99","ret="+ret.ToString(),ex2);
			}
			return ret;
		}

		public TransactionEcentric() : base()
		{
		//	Version 1
		//	cert1 = X509Certificate.CreateFromCertFile("C:\\Dev\\Prosperian\\Application\\PCIWebRTR\\Certificates\\ECentricRoot.cer");
		//	cert2 = X509Certificate.CreateFromCertFile("C:\\Dev\\Prosperian\\Application\\PCIWebRTR\\Certificates\\ECentricClient.cer");

			byte   err      = 10;
			string certName = "";
			string certPwd  = "";
			nsURL           = "http://www.ecentricswitch.co.za/paymentgateway/v1";

			base.LoadBureauDetails(Constants.PaymentProvider.Ecentric);

			try
			{
				xmlHeader  = "<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>"
				           + "<s:Header>"
				           +	"<MessageHeader xmlns='" + nsURL + "'>"
				           +		"<MessageDateTime>" + Tools.DateToString(System.DateTime.Now,7,0)
				                                      + "T"
				                                      + Tools.DateToString(System.DateTime.Now,0,5) + "</MessageDateTime>"
				           +		"<MessageID>#TransRef#</MessageID>"
				           + "</MessageHeader>"
				           + "</s:Header>";
				err        = 20;
				tranStatus = "";
				certName   = Tools.SystemFolder("Certificates") + Tools.ConfigValue("ECentric/CertName");
				err        = 30;
				certPwd    = Tools.ConfigValue("ECentric/CertPassword");
//				err        = 40;
//				bureauCode = Tools.BureauCode(Constants.PaymentProvider.Ecentric);
				err        = 50;
				certs      = new X509Certificate2Collection();

				certs.Import(certName, certPwd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

			//	Force TLS 1.2
				err                                   = 60;
				ServicePointManager.Expect100Continue = true;
				err                                   = 70;
				ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			}
			catch (Exception ex)
			{
				string msg = "Failed to import certificate, certName="+certName+", certPwd="+certPwd+", err="+err.ToString();
				Tools.LogInfo     (ClassName+".Base",msg,244);
				Tools.LogException(ClassName+".Base",msg,ex);
			}
		}

      public override void Close()
		{
			certs = null;
			base.Close();
		}
	}
}
