using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionPayU : Transaction
	{
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
					<soap:XXXX xmlns:ns2='http://soap.api.controller.web.payjar.com/'>
						<Api>ONE_ZERO</Api>
					</soap:XXXX>
				</soapenv:Body>
				</soapenv:Envelope>";

		private string resultSuccessful;

		public  bool   Successful
		{
			get { return Tools.NullToString(resultSuccessful).ToUpper() == "TRUE"; }
		}

		private int SendXML(string url,string userID,string password,string payUType="")
		{
			int ret   = 10;
			strResult = "";

			try
			{
				if ( payUType.Length < 1 )
					payUType = "doTransaction";
				if ( userID.Length   < 1 )
					userID   = Tools.ProviderCredentials("PayU","Id");
				if ( password.Length < 1 )
					password = Tools.ProviderCredentials("PayU","Password");
				if ( url.Length      < 1 )
					url      = BureauURL;
				if ( ! url.ToUpper().EndsWith("WSDL") )
					url = url + "/service/PayUAPI?wsdl";

				Tools.LogInfo("SendXML/10","URL=" + url + ", XML Sent=" + xmlSent,10,this);

			// Construct soap object
				ret                       = 20;
				XmlDocument soapXml       = CreateSoapEnvelope(xmlSent,payUType);

			// Create username and password namespace
				ret                       = 30;
				XmlNamespaceManager mgr   = new XmlNamespaceManager(soapXml.NameTable);
				mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				XmlNode userName          = soapXml.SelectSingleNode("//wsse:Username",mgr);
				userName.InnerText        = userID;
				XmlNode userPassword      = soapXml.SelectSingleNode("//wsse:Password",mgr);
				userPassword.InnerText    = password;

			// Construct web request object
				ret                       = 40;
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				ret                       = 45;
				webRequest.Headers.Add(@"SOAP:Action");
				webRequest.ContentType    = "text/xml;charset=\"utf-8\"";
				webRequest.Accept         = "text/xml";
				webRequest.Method         = "POST";

			// Insert soap envelope into web request
				ret = 50;
				using (Stream stream = webRequest.GetRequestStream())
					soapXml.Save(stream);

			// Get the completed web request XML
				ret = 60;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 65;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 70;
						strResult = rd.ReadToEnd();
					}
				}

				Tools.LogInfo("SendXML/50","XML Rec=" + strResult,10,this);

			// Create an empty soap result object
				ret       = 75;
				xmlResult = new XmlDocument();
				xmlResult.LoadXml(strResult.ToString());

//			//	Get data from result XML
				ret              = 80;
				resultSuccessful = Tools.XMLNode(xmlResult,"successful");
				resultCode       = Tools.XMLNode(xmlResult,"resultCode");
				resultMsg        = Tools.XMLNode(xmlResult,"resultMessage");

				if ( Successful )
					return 0;

				Tools.LogInfo("SendXML/80","URL=" + url + ", XML Sent=" + xmlSent+", XML Rec="+strResult,231,this);
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".SendXML/97",xmlSent);
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

//	Testing
	//		payment.ProviderURL      = "https://staging.payu.co.za";
//	//		payment.ProviderUserID   = "800060";
//	//		payment.ProviderPassword = "qDRLeKI9";
//	//		payment.ProviderKey      = "{FBEF85FC-F395-4DE2-B17F-F53098D8F978}";
	//		payment.ProviderUserID   = "200208";
	//		payment.ProviderPassword = "g1Kzk8GY";
	//		payment.ProviderKey      = "{A580B3C7-3EF3-47F1-9B90-4047CE0EC54C}";
	//		payment.RegionalId       = "8212115010081";
	//		payment.CurrencyCode     = "USD";
	//		payment.CardNumber       = "4000015372250142"; // This card works!
	//		payment.CardExpiryMM     = "07";
	//		payment.CardExpiryYYYY   = "2025";
	//		payment.CardCVV          = "123";
//	Testing

			try
			{
				string tmp = Tools.ConfigValue("SystemURL")
				           + "/Succeed.aspx?TransRef="
				           + Tools.XMLSafe(payment.MerchantReference)
				           + "&#38;Mode=";
				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				        + "<TransactionType>RESERVE</TransactionType>"
				        + "<Customfield>"
				        +   "<key>processingType</key>"
				        +   "<value>REAL_TIME_RECURRING</value>"
				        + "</Customfield>"
				        + "<AdditionalInformation>"
				        +   "<storePaymentMethod>true</storePaymentMethod>"
				        +   "<secure3d>false</secure3d>"
				        +   "<showBudget>false</showBudget>"
				        +   "<demoMode>false</demoMode>"
				        +   "<redirectChannel>responsive</redirectChannel>"
				        +   "<supportedPaymentMethods>CREDITCARD_TOKEN</supportedPaymentMethods>"
				        +   "<returnUrl>"         + tmp + "1</returnUrl>"
				        +   "<cancelUrl>"         + tmp + "2</cancelUrl>"
				        +   "<notificationUrl>"   + tmp + "3</notificationUrl>"
				        +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				        + "</AdditionalInformation>"
				        + "<Customer>"
				        +   "<merchantUserId>" + payment.ContractCode  + "</merchantUserId>" // This must be unique per customer, it is NOT the payment provider user id
				        +   "<countryCode>"    + payment.CountryCode() + "</countryCode>"
				        +   "<regionalId>"     + payment.RegionalId    + "</regionalId>"
				        +   "<email>"          + payment.EMail         + "</email>"
				        +   "<firstName>"      + payment.FirstName     + "</firstName>"
				        +   "<lastName>"       + payment.LastName      + "</lastName>"
				        +   "<mobile>"         + payment.PhoneCell     + "</mobile>"
				        + "</Customer>"
				        + "<Basket>"
				        +   "<amountInCents>"  + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<currencyCode>"   + payment.CurrencyCode             + "</currencyCode>"
				        +   "<description>"    + payment.PaymentDescription       + "</description>"
				        + "</Basket>"
				        + "<Creditcard>"
				        +   "<nameOnCard>"     + payment.CardName                              + "</nameOnCard>"
				        +   "<amountInCents>"  + payment.PaymentAmount.ToString()              + "</amountInCents>"
				        +   "<cardNumber>"     + payment.CardNumber                            + "</cardNumber>"
				        +   "<cardExpiry>"     + payment.CardExpiryMM + payment.CardExpiryYYYY + "</cardExpiry>"
				        +   "<cvv>"            + payment.CardCVV                               + "</cvv>"
				        + "</Creditcard>";

				ret      = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword,"setTransaction");
//				tmp      = Tools.XMLNode(xmlResult,"gatewayReference");
//				payToken = Tools.XMLNode(xmlResult,"pmId");
				payRef   = Tools.XMLNode(xmlResult,"payUReference");

				if ( ret != 0 )
					return ret;

				if ( payRef.Length < 1 ) // || payToken.Length < 1 )
					return 320;

//	Now complete the transaction

				ret                       = 330;
				string          url       = payment.ProviderURL + "/rpp.do?PayUReference=" + Tools.HTMLSafe(payRef);
				HttpWebRequest  wRequest  = (HttpWebRequest)WebRequest.Create(url);
				ret                       = 350;
				HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
				ret                       = 370;
				using (StreamReader rdr   = new StreamReader(wResponse.GetResponseStream()))
					strResult = rdr.ReadToEnd().Trim();

//	Get the token (pmId)

				ret      = 390;
				xmlSent  = "<Safekey>" + payment.ProviderKey + "</Safekey>"
			            + "<AdditionalInformation>"
			            +   "<payUReference>" + payRef + "</payUReference>"
			            + "</AdditionalInformation>";
				ret      = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword,"getTransaction");
				payToken = Tools.XMLNode(xmlResult,"pmId");

				if ( ret != 0 )
					return ret;

				if ( payToken.Length < 1 )
					return 410;

//	Now reverse the initial RESERVE transaction

				ret     = 430;
				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
			           + "<TransactionType>RESERVE_CANCEL</TransactionType>"
				        + "<AuthenticationType>TOKEN</AuthenticationType>"
			           + "<AdditionalInformation>"
			           +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
			           +   "<payUReference>"     + payRef + "</payUReference>"
			           + "</AdditionalInformation>"
			           + "<Basket>"
			           +	"<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
			           +	"<currencyCode>"  + payment.CurrencyCode + "</currencyCode>"
			           + "</Basket>"
				        + "<Creditcard>"
			           +	"<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        + "</Creditcard>";
				ret = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int ThreeDSecureCheck(string providerRef,string merchantRef="")
		{
			int ret = 800;
			xmlSent = "";

			try
			{
				xmlSent = "<Safekey>" + Tools.ProviderCredentials("PayU","Key") + "</Safekey>"
				        + "<AdditionalInformation>"
				        +	"<payUReference>" + providerRef + "</payUReference>"
				        + "</AdditionalInformation>";
				ret     = SendXML("","","","getTransaction");
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecureCheck/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("ThreeDSecureCheck/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 600;
			xmlSent = "";

//	Testing
//			payment.ProviderURL      = "https://staging.payu.co.za";
//			payment.ProviderUserID   = "800060";
//			payment.ProviderPassword = "qDRLeKI9";
//			payment.ProviderKey      = "{FBEF85FC-F395-4DE2-B17F-F53098D8F978}";
//			payment.RegionalId       = "8212115010081";
//			payment.CurrencyCode     = "ZAR";
//	//		payment.CardToken        = "E13648542276F54C44C754843840821D";
//			payment.CardToken        = "74BF0BC8AE9987B423AA94A4BC894A2A"; // This token WORKS!
//	Testing

//	Not needed
//		   +   "<secure3d>false</secure3d>"
//       +   "<storePaymentMethod>true</storePaymentMethod>"

//       +   "<email>" + payment.EMail + "</email>"
//       +   "<firstName>" + payment.FirstName + "</firstName>"
//       +   "<lastName>" + payment.LastName + "</lastName>"
//       +   "<mobile>" + payment.PhoneCell + "</mobile>"

//		   +   "<cvv></cvv>"

			try
			{
				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				        + "<TransactionType>PAYMENT</TransactionType>"
				        + "<AuthenticationType>TOKEN</AuthenticationType>"
				        + "<Customfield>"
				        +   "<key>processingType</key>"
				        +   "<value>REAL_TIME_RECURRING</value>"
				        + "</Customfield>"
				        + "<AdditionalInformation>"
				        +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				        +   "<notificationUrl>" + Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef=" + Tools.XMLSafe(payment.MerchantReference) + "</notificationUrl>"
				        + "</AdditionalInformation>"
				        + "<Customer>"
				        +   "<merchantUserId>" + payment.ContractCode  + "</merchantUserId>"
				        +   "<countryCode>"    + payment.CountryCode() + "</countryCode>"
				        +   "<regionalId>"     + payment.RegionalId    + "</regionalId>"
				        + "</Customer>"
				        + "<Basket>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<currencyCode>"  + payment.CurrencyCode             + "</currencyCode>"
				        +   "<description>"   + payment.PaymentDescription       + "</description>"
				        + "</Basket>"
				        + "<Creditcard>"
				        +   "<pmId>"          + payment.CardToken                + "</pmId>"
				        +   "<amountInCents>" + payment.PaymentAmount.ToString() + "</amountInCents>"
				        + "</Creditcard>";

				ret    = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
				payRef = Tools.XMLNode(xmlResult,"payUReference");
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret = 10;
			string url = "";
			d3Form     = "";

			try
			{
				if ( postBackURL == null )
					url = Tools.ConfigValue("SystemURL");
				else
					url = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! url.EndsWith("/") )
					url = url + "/";
				url = url + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				          +                    "&amp;TransRef="+Tools.XMLSafe(payment.MerchantReference);
//				          +                    "&amp;PayUReference="+Tools.XMLSafe(payment.TransactionID);

				if ( payment.TokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
					xmlSent = "{{{" + Tools.URLString(payment.CardToken) + "}}}";
				else
					xmlSent = Tools.URLString(payment.CardNumber);

				xmlSent = "<Safekey>" + payment.ProviderKey + "</Safekey>"
				        + "<TransactionType>RESERVE</TransactionType>"
				        + "<AdditionalInformation>"
				        +   "<supportedPaymentMethods>CREDITCARD</supportedPaymentMethods>"
				        +   "<secure3d>true</secure3d>"
				        +   "<returnUrl>"         + url                       + "</returnUrl>"
				        +   "<cancelUrl>"         + url                       + "</cancelUrl>"
				        +   "<notificationUrl>"   + url                       + "</notificationUrl>"
				        +   "<merchantReference>" + payment.MerchantReference + "</merchantReference>"
				        + "</AdditionalInformation>"
				        + "<Customer>"
				        +   "<merchantUserId>" + payment.ContractCode  + "</merchantUserId>"
				        +   "<countryCode>"    + payment.CountryCode() + "</countryCode>"
				        +   "<email>"          + payment.EMail         + "</email>"
				        +   "<firstName>"      + payment.FirstName     + "</firstName>"
				        +   "<lastName>"       + payment.LastName      + "</lastName>"
				        +   "<mobile>"         + payment.PhoneCell     + "</mobile>"
				        +   "<regionalId>"     + payment.RegionalId    + "</regionalId>"
				        + "</Customer>"
				        + "<Basket>"
				        +   "<amountInCents>"  + payment.PaymentAmount.ToString() + "</amountInCents>"
				        +   "<currencyCode>"   + payment.CurrencyCode             + "</currencyCode>"
				        +   "<description>"    + payment.PaymentDescription       + "</description>"
				        + "</Basket>"
				        + "<Creditcard>"
				        +   "<nameOnCard>"     + payment.CardName                              + "</nameOnCard>"
				        +   "<amountInCents>"  + payment.PaymentAmount.ToString()              + "</amountInCents>"
				        +   "<cardNumber>"     + payment.CardNumber                            + "</cardNumber>"
				        +   "<cardExpiry>"     + payment.CardExpiryMM + payment.CardExpiryYYYY + "</cardExpiry>"
				        +   "<cvv>"            + payment.CardCVV                               + "</cvv>"
				        + "</Creditcard>";

				ret     = SendXML(payment.ProviderURL,payment.ProviderUserID,payment.ProviderPassword);
				payRef  = Tools.XMLNode(xmlResult,"payUReference");
				d3Form  = Tools.XMLNode(xmlResult,"url","","","redirect");

				if ( ret == 0 && payRef.Length > 0 && d3Form.Length > 0 )
				{
					string sql = "exec sp_WP_PaymentRegister3DSecA @ContractCode="    + Tools.DBString(payment.MerchantReference)
				              +                                 ",@ReferenceNumber=" + Tools.DBString(payRef)
				              +                                 ",@Status='77'"; // Means payment pending
					if ( languageCode.Length > 0 )
						sql = sql + ",@LanguageCode="        + Tools.DBString(languageCode);
					if ( languageDialectCode.Length > 0 )
						sql = sql + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					using (MiscList mList = new MiscList())
						mList.ExecQuery(sql,0,"",false,true);
					Tools.LogInfo("ThreeDSecurePayment/50","PayRef=" + payRef + "; SQL=" + sql + "; " + d3Form,10,this);
					return 0;
				}
//				Tools.LogInfo("ThreeDSecurePayment/60","ResultCode="+ResultCode + ", payRef=" + payRef,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("ThreeDSecurePayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		private static XmlDocument CreateSoapEnvelope(string content,string payUType)
		{
			StringBuilder str = new StringBuilder(soapEnvelope);
			str.Insert(str.ToString().IndexOf("</soap:XXXX>"), content);
			str.Replace("soap:XXXX","soap:"+payUType);

		//	Create an empty soap envelope
			XmlDocument soapEnvelopeXml = new XmlDocument();
			soapEnvelopeXml.LoadXml(str.ToString());
			return soapEnvelopeXml;
		}

		public TransactionPayU() : base()
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(Constants.PaymentProvider.PayU);
		}
	}
}
