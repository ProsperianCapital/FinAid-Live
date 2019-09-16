using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionPayU : Transaction
	{
//		static string url      = "https://staging.payu.co.za";
//		static string userID   = "Staging Enterprise Integration Store 1";
//		static string password = "j3w8swi5";

//		static string url      = "https://staging.payu.co.za";
//		static string userID   = "200239";
//		static string password = "5AlTRPoD";

//	Version 1 ... worked then mysteriously stopped working ...
//		static string soapEnvelopeOLD =
//			@"<SOAP-ENV:Envelope
//				xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'
//				xmlns:ns1='http://soap.api.controller.web.payjar.com/'
//				xmlns:ns2='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
//				<SOAP-ENV:Header>
//					<wsse:Security SOAP-ENV:mustUnderstand='1' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
//						<wsse:UsernameToken wsu:Id='UsernameToken-9' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
//							<wsse:Username></wsse:Username>
//							<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'></wsse:Password>
//						</wsse:UsernameToken>
//					</wsse:Security>
//				</SOAP-ENV:Header>
//				<SOAP-ENV:Body>
//				</SOAP-ENV:Body>
//			</SOAP-ENV:Envelope>";

//	Version 2
		static string soapEnvelope =
			@"<soapenv:Envelope
					xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'
					xmlns:soap='http://soap.api.controller.web.payjar.com/'
					xmlns:SOAP-ENV='SOAP-ENV'>
				<soapenv:Header>
				<wsse:Security
					SOAP-ENV:mustUnderstand='1'
					xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
					<wsse:UsernameToken
						wsu:Id='UsernameToken-9'
						xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
						<wsse:Username></wsse:Username>
						<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'></wsse:Password>
					</wsse:UsernameToken>
				</wsse:Security>
				</soapenv:Header>
				<soapenv:Body>
					<soap:doTransaction>
					</soap:doTransaction>
				</soapenv:Body>
				</soapenv:Envelope>";

		private string resultSuccessful;

		public  bool   Successful
		{
			get { return Tools.NullToString(resultSuccessful).ToUpper() == "TRUE"; }
		}

		private int SendXML(string url,string userID,string password)
		{
			int    ret         = 10;
			string xmlReceived = "";
			payRef             = "";

			try
			{
				if ( ! url.ToUpper().EndsWith("WSDL") )
					url = url + "/service/PayUAPI?wsdl";

				Tools.LogInfo("TransactionPayU.SendXML/10","URL=" + url + ", XML Sent=" + xmlSent,10);

			// Construct soap object
				ret = 20;
				XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlSent);

			// Create username and password namespace
				ret = 30;
				XmlNamespaceManager mgr = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
				mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				XmlNode userName        = soapEnvelopeXml.SelectSingleNode("//wsse:Username",mgr);
				userName.InnerText      = userID;
				XmlNode userPassword    = soapEnvelopeXml.SelectSingleNode("//wsse:Password",mgr);
				userPassword.InnerText  = password;

			// Construct web request object
				Tools.LogInfo("TransactionPayU.SendXML/30","Create/set up web request, URL=" + url,10);
				ret = 40;
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
				webRequest.Headers.Add(@"SOAP:Action");
				webRequest.ContentType = "text/xml;charset=\"utf-8\"";
				webRequest.Accept      = "text/xml";
				webRequest.Method      = "POST";

			// Insert soap envelope into web request
				Tools.LogInfo("TransactionPayU.SendXML/35","Save web request",10);
				ret = 50;
				using (Stream stream = webRequest.GetRequestStream())
					soapEnvelopeXml.Save(stream);

			// Get the completed web request XML
				Tools.LogInfo("TransactionPayU.SendXML/40","Get web response",10);
				ret = 60;

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 63;
					Tools.LogInfo("TransactionPayU.SendXML/45","Read web response stream",10);
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret         = 66;
						xmlReceived = rd.ReadToEnd();
					}
				}

				Tools.LogInfo("TransactionPayU.SendXML/50","XML Rec=" + xmlReceived,255);

			// Create an empty soap result object
				ret       = 70;
				xmlResult = new XmlDocument();
				xmlResult.LoadXml(xmlReceived.ToString());

//			//	Get data from result XML
				ret              = 80;
				resultSuccessful = Tools.XMLNode(xmlResult,"successful");
				resultCode       = Tools.XMLNode(xmlResult,"resultCode");
				resultMsg        = Tools.XMLNode(xmlResult,"resultMessage");
//				payRef           = Tools.XMLNode(xmlResult,"payUReference");
//				payToken         = Tools.XMLNode(xmlResult,"pmId");

				if ( Successful )
					return 0;

				Tools.LogInfo("TransactionPayU.SendXML/80","URL=" + url + ", XML Sent=" + xmlSent+", XML Rec="+xmlReceived,220);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayU.SendXML/85","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayU.SendXML/90","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int ret = 300;
			xmlSent = "";

			Tools.LogInfo("TransactionPayU.GetToken/10","RESERVE, Merchant Ref=" + payment.MerchantReference,199);

			try
			{
				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				        + "<Api>ONE_ZERO</Api>"
				        + "<TransactionType>RESERVE</TransactionType>"
				        + "<Customfield>"
				        +   "<key>processingType</key>"
				        +   "<value>REAL_TIME_RECURRING</value>"
				        + "</Customfield>"
				        + "<AdditionalInformation>"
				        +   "<storePaymentMethod>true</storePaymentMethod>"
				        +   "<secure3d>false</secure3d>"
				        +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				        + "</AdditionalInformation>"
				        + "<Customer>"
				        +   "<merchantUserId>" + payment.ProviderUserID + "</merchantUserId>"
				        +   "<countryCode>" + payment.CountryCode + "</countryCode>"
				        +   "<email>" + payment.EMail + "</email>"
				        +   "<firstName>" + payment.FirstName + "</firstName>"
				        +   "<lastName>" + payment.LastName + "</lastName>"
				        +   "<mobile>" + payment.PhoneCell + "</mobile>"
				        +   "<regionalId>" + payment.RegionalId + "</regionalId>"
				        + "</Customer>"
				        + "<Basket>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<currencyCode>" + payment.CurrencyCode + "</currencyCode>"
				        +   "<description>" + payment.PaymentDescription + "</description>"
				        + "</Basket>"
				        + "<Creditcard>"
				        +   "<nameOnCard>" + payment.CardName + "</nameOnCard>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +   "<cardExpiry>" + payment.CardExpiryMM + payment.CardExpiryYYYY + "</cardExpiry>"
				        +   "<cvv>" + payment.CardCVV + "</cvv>"
				        + "</Creditcard>";

				ret      = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
				payRef   = Tools.XMLNode(xmlResult,"payUReference");
				payToken = Tools.XMLNode(xmlResult,"pmId");

				if ( ret == 0 )
				{
					Tools.LogInfo("TransactionPayU.GetToken/20","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,30);
					Tools.LogInfo("TransactionPayU.GetToken/30","RESERVE_CANCEL, Merchant Ref=" + payment.MerchantReference,30);
					xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				           + "<Api>ONE_ZERO</Api>"
				           + "<TransactionType>RESERVE_CANCEL</TransactionType>"
				           + "<AdditionalInformation>"
				           +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				           +   "<payUReference>" + payRef + "</payUReference>"
				           + "</AdditionalInformation>"
				           + "<Basket>"
				           +	"<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				           +	"<currencyCode>" + payment.CurrencyCode + "</currencyCode>"
				           + "</Basket>";
					ret = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
					Tools.LogInfo("TransactionPayU.GetToken/40","ResultCode="+ResultCode,30);
				}
				else
					Tools.LogInfo("TransactionPayU.GetToken/50","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,220);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayU.GetToken/85","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayU.GetToken/90","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}


		public override int ProcessPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 600;
			xmlSent = "";

			Tools.LogInfo("TransactionPayU.ProcessPayment/10","PAYMENT, Merchant Ref=" + payment.MerchantReference,199);

//		   +   "<secure3d>false</secure3d>"
//       +   "<storePaymentMethod>true</storePaymentMethod>"

			try
			{
				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				        + "<Api>ONE_ZERO</Api>"
				        + "<TransactionType>PAYMENT</TransactionType>"
				        + "<AuthenticationType>TOKEN</AuthenticationType>"
				        + "<Customfield>"
				        +   "<key>processingType</key>"
				        +   "<value>REAL_TIME_RECURRING</value>"
				        + "</Customfield>"
				        + "<AdditionalInformation>"
				        +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				        + "</AdditionalInformation>"
				        + "<Customer>"
				        +   "<merchantUserId>" + payment.ProviderUserID + "</merchantUserId>"
				        +   "<countryCode>" + payment.CountryCode + "</countryCode>"
				        +   "<email>" + payment.EMail + "</email>"
				        +   "<firstName>" + payment.FirstName + "</firstName>"
				        +   "<lastName>" + payment.LastName + "</lastName>"
				        +   "<mobile>" + payment.PhoneCell + "</mobile>"
				        +   "<regionalId>" + payment.RegionalId + "</regionalId>"
				        + "</Customer>"
				        + "<Basket>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<currencyCode>" + payment.CurrencyCode + "</currencyCode>"
				        +   "<description>" + payment.PaymentDescription + "</description>"
				        + "</Basket>"
				        + "<Creditcard>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<pmId>" + payment.CardToken + "</pmId>"
				        + "</Creditcard>";

				ret    = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
				payRef = Tools.XMLNode(xmlResult,"payUReference");

				Tools.LogInfo("TransactionPayU.ProcessPayment/30","ResultCode="+ResultCode,30);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayU.ProcessPayment/85","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayU.ProcessPayment/90","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		private static XmlDocument CreateSoapEnvelope(string content)
		{
			StringBuilder str = new StringBuilder(soapEnvelope);
			str.Insert(str.ToString().IndexOf("</soap:doTransaction>"), content);

		//	Create an empty soap envelope
			XmlDocument soapEnvelopeXml = new XmlDocument();
			soapEnvelopeXml.LoadXml(str.ToString());
			return soapEnvelopeXml;
		}

		public TransactionPayU() : base()
		{
			bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayU);
		}
	}
}
