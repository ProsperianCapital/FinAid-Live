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
				if ( url.Length < 1 )
					url = BureauURL;
				if ( ! url.ToUpper().EndsWith("WSDL") )
					url = url + "/service/PayUAPI?wsdl";

				Tools.LogInfo("SendXML/10","URL=" + url + ", XML Sent=" + xmlSent,10,this);

			// Construct soap object
				ret                         = 20;
				XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlSent);

			// Create username and password namespace
				ret                     = 30;
				XmlNamespaceManager mgr = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
				mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				XmlNode userName        = soapEnvelopeXml.SelectSingleNode("//wsse:Username",mgr);
				userName.InnerText      = userID;
				XmlNode userPassword    = soapEnvelopeXml.SelectSingleNode("//wsse:Password",mgr);
				userPassword.InnerText  = password;

			// Construct web request object
				Tools.LogInfo("SendXML/30","Create/set up web request, URL=" + url,10,this);
				ret                       = 40;
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				ret                    = 45;
				webRequest.Headers.Add(@"SOAP:Action");
				webRequest.ContentType = "text/xml;charset=\"utf-8\"";
				webRequest.Accept      = "text/xml";
				webRequest.Method      = "POST";

			// Insert soap envelope into web request
				Tools.LogInfo("SendXML/35","Save web request",10,this);
				ret = 50;
				using (Stream stream = webRequest.GetRequestStream())
					soapEnvelopeXml.Save(stream);

			// Get the completed web request XML
				Tools.LogInfo("SendXML/40","Get web response",10,this);
				ret = 60;

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 65;
					Tools.LogInfo("SendXML/45","Read web response stream",10,this);
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret         = 70;
						xmlReceived = rd.ReadToEnd();
					}
				}

				Tools.LogInfo("SendXML/50","XML Rec=" + xmlReceived,255,this);

			// Create an empty soap result object
				ret       = 75;
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

				Tools.LogInfo("SendXML/80","URL=" + url + ", XML Sent=" + xmlSent+", XML Rec="+xmlReceived,220,this);
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,"TransactionPayU.SendXML/97",xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("SendXML/98","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,255,this);
				Tools.LogException("SendXML/99","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,ex2,this);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int ret = 300;
			xmlSent = "";

			Tools.LogInfo("GetToken/10","RESERVE, Merchant Ref=" + payment.MerchantReference,10,this);

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
				        +   "<countryCode>" + payment.CountryCode() + "</countryCode>"
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
					Tools.LogInfo("GetToken/20","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,30,this);
					Tools.LogInfo("GetToken/30","RESERVE_CANCEL, Merchant Ref=" + payment.MerchantReference,30,this);
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
					Tools.LogInfo("GetToken/40","ResultCode="+ResultCode,30,this);
				}
				else
					Tools.LogInfo("GetToken/50","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,220,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}


		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 600;
			xmlSent = "";

			Tools.LogInfo("TokenPayment/10","PAYMENT, Merchant Ref=" + payment.MerchantReference,10,this);

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
				        +   "<countryCode>" + payment.CountryCode() + "</countryCode>"
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

				Tools.LogInfo("TokenPayment/30","ResultCode="+ResultCode,30,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
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
			base.LoadBureauDetails(Constants.PaymentProvider.PayU);
		//	bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayU);
		}
	}
}
