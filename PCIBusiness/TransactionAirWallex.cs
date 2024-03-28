using System;
using System.Text;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionAirWallex : Transaction
	{
		private byte     logPriority;
		private string   err;

//		protected string paymentMethodId;
		protected string paymentConsentId;
		protected string paymentIntentId;

//		static  string   clientSecret;
		static  string   accessToken;
		static  DateTime accessDate;

//		public  bool Successful
//		{
//			get { return Tools.JSONValue(strResult,"success").ToUpper() == "TRUE"; }
//		}

//		See Transaction.cs
//		public  string      PaymentMethodId
//		{
//			get { return     Tools.NullToString(paymentMethodId); }
//		}
		public  string      PaymentConsentId
		{
			get { return     Tools.NullToString(paymentConsentId); }
		}
		public  string      PaymentIntentId
		{
			get { return     Tools.NullToString(paymentIntentId); }
		}

		public override int GetToken(Payment payment)
		{
			return CallWebService(payment,(byte)Constants.TransactionType.GetToken);
		}
		public override int GetToken3rdParty(Payment payment)
		{
			return CallWebService(payment,(byte)Constants.TransactionType.GetTokenThirdParty);
		}

		public override int TokenPayment(Payment payment)
		{
			if ( EnabledFor3d(payment.TransactionType) )
				return CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
			return 500;
		}
		public override int CardPayment3rdParty(Payment payment)
		{
			if ( EnabledFor3d(payment.TransactionType) )
				return CallWebService(payment,(byte)Constants.TransactionType.CardPaymentThirdParty);
			return 500;
		}
		public override int CardValidation(Payment payment)
		{
			return CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecureCheck);
		}

		private int ExtractErrorAndReturn(int err)
		{
			if ( ! String.IsNullOrWhiteSpace(strResult) )
			{
				string tmp = Tools.JSONValue(strResult,"code");
				if ( tmp.Length > 0 )
					resultCode = tmp;
				tmp = Tools.JSONValue(strResult,"message");
				if ( tmp.Length < 1 )
					tmp = Tools.JSONValue(strResult,"status");
				if ( tmp.Length > 0 )
					resultMsg = tmp;
			}
			return err;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			HttpWebRequest webRequest;
			int            wCall;
			int            ret       = 10;
			string         txURL     = payment.TokenizerURL;
			string         url       = Tools.ProviderCredentials("AirWallex","Url");
			string         awID      = Tools.ProviderCredentials("AirWallex","Id");
			string         awKey     = Tools.ProviderCredentials("AirWallex","Key");

//			string         url       = payment.ProviderURL;
//			string         txURL     = payment.TokenizerURL;
//			string         awID      = payment.ProviderUserID;
//			string         awKey     = payment.ProviderKey;
//			string         returnURL = "https://pcipaymentgateway1.azurewebsites.net/Succeed.aspx";
//			string         returnURL = "https://lifestyledirectglobal.com";
//			string         returnURL = "https://www.eservsecure.com";

			if ( awID.Length  < 1 )
				awID  = payment.ProviderUserID;
			if ( awKey.Length < 1 )
				awKey = payment.ProviderKey;
			if ( url.Length   < 1 )
				url = payment.ProviderURL;
			if ( url.Length   < 1 )
				url = BureauURL;
			if ( url.Length   < 1 )
				url = Tools.BureauURL(BureauCode);
			if ( ! url.EndsWith("/") )
				url = url + "/";
			if ( txURL.Length > 0 && ! txURL.ToUpper().EndsWith("DETOKENIZE") )
				txURL = txURL + "/TransparentGatewayAPI/Detokenize";
			if ( Tools.NullToString(returnURL).Length < 1 )
			//	returnURL = "https://www.eservsecure.com";
				returnURL = "https://lifestyledirectglobal.com";

			resultCode = "11";
			resultMsg  = "(11) Web service call failed";

//			Tools.LogInfo("CallWebService/10","tranType="+transactionType.ToString()
//			                              + ", url="+url
//			                              + ", awID="+awID
//			                              + ", awKey="+awKey,logPriority,this);

			try
			{
			// First get an access token

				wCall = GetAccessToken(1,url,awID,awKey);
				if ( wCall != 0 )
				{
					resultCode = wCall.ToString();
					resultMsg  = "Error retrieving/creating/storing access token '" + accessToken + "' to SQL";
					return wCall;
				}

				if ( transactionType == (byte)Constants.TransactionType.GetToken ||
			        transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck ||
			        transactionType == (byte)Constants.TransactionType.GetTokenThirdParty )
				{
				// Create a customer

					ret        = 100;
					strResult  = "";
					resultCode = "31";
					resultMsg  = "(31) Internal error creating a customer";
					xmlSent    = "{ \"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" }," // 2 characters, eg. ZA, US or CN
					           +   "\"first_name\" : \"" + payment.FirstName + "\","
					           +   "\"last_name\" : \"" + payment.LastName + "\","
					           +   "\"email\" : \"" + payment.EMail + "\","
					           +   "\"phone_number\" : \"" + payment.PhoneCell + "\","
					           +   "\"merchant_customer_id\" : \"" + payment.ContractCode + "\","
					           +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";

					ret        = 110;
					webRequest = (HttpWebRequest)WebRequest.Create(url+"pa/customers/create");
					ret        = 120;
					wCall      = WebCall(webRequest, "Create customer", xmlSent, ref strResult);
					customerId = Tools.JSONValue(strResult,"id");
					payRef     = Tools.JSONValue(strResult,"client_secret");

					if ( wCall > 0 || strResult.Length < 1 || customerId.Length < 1 )
						return ExtractErrorAndReturn(130);

					if ( transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck )
						return 0;

				// Now create a payment method

					ret        = 200;
					strResult  = "";
					resultCode = "32";
					resultMsg  = "(32) Internal error creating a payment method";
					xmlSent    = "{ \"card\" :"
					           +   "{ \"billing\" :"
					           +     "{ \"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" },"
					           +       "\"first_name\" : \""   + payment.FirstName + "\","
					           +       "\"last_name\" : \""    + payment.LastName + "\","
					           +       "\"email\" : \""        + payment.EMail + "\","
					           +       "\"phone_number\" : \"" + payment.PhoneCell + "\" },"
					           +     "\"cvc\" : \""            + payment.CardCVV + "\","
					           +     "\"expiry_month\" : \""   + payment.CardExpiryMM + "\","
					           +     "\"expiry_year\" : \""    + payment.CardExpiryYYYY + "\","
					           +     "\"name\" : \""           + payment.CardName + "\","
					           +     "\"number\" : \""         + payment.CardNumber + "\","
					           +     "\"number_type\" : \"PAN\" },"
					           +   "\"customer_id\" : \""      + customerId + "\","
					           +   "\"request_id\" : \""       + (Guid.NewGuid()).ToString() + "\","
					           +   "\"type\" : \"card\" }";

					ret        = 210;
					if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty && txURL.Length > 0 )
					{
						ret                                = 220;
						webRequest                         = (HttpWebRequest)WebRequest.Create(txURL);
						webRequest.Headers["TX_URL"]       = url+"pa/customers/create";
						webRequest.Headers["TX_TokenExID"] = payment.TokenizerID;  // "4311038889209736";
						webRequest.Headers["TX_APIKey"]    = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
					}
					else
						webRequest                         = (HttpWebRequest)WebRequest.Create(url+"pa/payment_methods/create");

					ret             = 230;
					wCall           = WebCall(webRequest, "Create payment method", xmlSent, ref strResult);
					paymentMethodId = Tools.JSONValue(strResult,"id");

					if ( wCall > 0 || strResult.Length < 1 || paymentMethodId.Length < 1 )
						return ExtractErrorAndReturn(230);

				// Now create a payment consent

					ret        = 300;
					strResult  = "";
					resultCode = "33";
					resultMsg  = "(33) Internal error creating a payment consent";
					xmlSent    = "{ \"customer_id\" : \"" + customerId + "\","
					           +   "\"next_triggered_by\" : \"merchant\","
					           +   "\"request_id\" : \""  + (Guid.NewGuid()).ToString() + "\" }";

					ret              = 310;
					webRequest       = (HttpWebRequest)WebRequest.Create(url+"pa/payment_consents/create");
					ret              = 320;
					wCall            = WebCall(webRequest, "Create payment consent", xmlSent, ref strResult);
					paymentConsentId = Tools.JSONValue(strResult,"id");

					if ( wCall > 0 || strResult.Length < 1 || paymentConsentId.Length < 1 )
						return ExtractErrorAndReturn(330);

				// Verify the payment consent
				//	See https://www.airwallex.com/docs/api#/Payment_Acceptance/Payment_Consents/_api_v1_pa_payment_consents__id__verify/post

					ret        = 400;
					strResult  = "";
					resultCode = "34";
					resultMsg  = "(34) Internal error verifying a payment consent";
					xmlSent    = "{ \"descriptor\" : \"" + payment.PaymentDescription + "\","
					           +   "\"payment_method\" :"
					           +     "{ \"type\" : \"card\","
					           +       "\"id\" : \"" + paymentMethodId + "\" },"
					           +   "\"device_data\" :"
					           +     "{ \"ip_address\" : \"" + payment.MandateIPAddress + "\","
					           +       "\"accept_header\" : \"*/*\","
					           +       "\"browser\" :"
					           +         "{ \"user_agent\" : \"" + payment.MandateBrowser + "\","
					           +           "\"java_enabled\" : false,"
					           +           "\"javascript_enabled\" : true } },"
					           +   "\"risk_control_options\" :"
					           +     "{ \"skip_risk_processing\" : \"true\" },"
					           +   "\"verification_options\" :"
					           +     "{ \"card\" :"
					           +       "{ \"amount\" : 1.00,"
					           +         "\"currency\" : \"USD\","
					           +         "\"cvc\" : \"" + payment.CardCVV + "\" } },"
					           +   "\"return_url\" : \"" + ReturnURL + "\","
					           +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";

					ret              = 410;
					webRequest       = (HttpWebRequest)WebRequest.Create(url+"pa/payment_consents/" + paymentConsentId + "/verify");
					ret              = 420;
					wCall            = WebCall(webRequest, "Verify payment consent", xmlSent, ref strResult);
//					payRef           = Tools.JSONValue(strResult,"client_secret");
//					paymentConsentId = Tools.JSONValue(strResult,"id");
					customerId       = Tools.JSONValue(strResult,"customer_id");
					payToken         = Tools.JSONValue(strResult,"id");

					if ( wCall > 0 || strResult.Length < 1 || paymentConsentId.Length < 1 || customerId.Length < 1 || payToken.Length < 1 )
						return ExtractErrorAndReturn(430);

					ret = 0;
				}

				else if ( transactionType == (byte)Constants.TransactionType.TokenPayment ||
				          transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				{
				//	Create a Payment Intent
				//	https://www.airwallex.com/docs/api#/Payment_Acceptance/Payment_Intents/_api_v1_pa_payment_intents_create/post

					ret        = 500;
					strResult  = "";
					resultCode = "41";
					resultMsg  = "(41) Internal error creating a payment intent";

					if ( payment.CustomerID.Length > 0 )
					//	Ver 1: Use a previously created customer
						xmlSent = "{ \"amount\" : \"" + payment.PaymentAmountDecimal + "\","
						        +   "\"currency\" : \"" + payment.CurrencyCode + "\","
						        +   "\"customer_id\" : \"" + payment.CustomerID + "\","
						        +   "\"descriptor\" : \"" + payment.PaymentDescription + "\","
						        +   "\"merchant_order_id\" : \"" + payment.MerchantReference + "\","
						        +   "\"risk_control_options\" : { \"skip_risk_processing\" : \"true\" },"
						        +   "\"return_url\" : \"" + ReturnURL + "\","
						        +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";
					else
					//	Ver 2: Provide customer details
						xmlSent = "{ \"amount\" : \"" + payment.PaymentAmountDecimal + "\","
						        +   "\"currency\" : \"" + payment.CurrencyCode + "\","
						        +   "\"customer\" : {"
						        +     "\"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" }," // 2 characters, eg. ZA, US or CN
						        +     "\"first_name\" : \"" + payment.FirstName + "\","
						        +     "\"last_name\" : \"" + payment.LastName + "\","
						        +     "\"email\" : \"" + payment.EMail + "\","
						        +     "\"phone_number\" : \"" + payment.PhoneCell + "\","
						        +     "\"merchant_customer_id\" : \"" + payment.ContractCode + "\" },"
//						        +   "\"payment_method_options\" : {"
//						        +     "\"card\" : {"
//						        +       "\"risk_control\" : {"
//						        +         "\"skip_risk_processing\" : true,"
//	//					        +         "\"three_domain_secure_action\" : \"SKIP_3DS\","
//						        +         "\"three_ds_action\" : \"SKIP_3DS\" },"
//						        +       "\"three_ds_action\" : \"SKIP_3DS\" } },"
						        +   "\"descriptor\" : \"" + payment.PaymentDescription + "\","
						        +   "\"merchant_order_id\" : \"" + payment.MerchantReference + "\","
						        +   "\"risk_control_options\" : { \"skip_risk_processing\" : true },"
						        +   "\"return_url\" : \"" + ReturnURL + "\","
						        +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";

					ret             = 510;
					webRequest      = (HttpWebRequest)WebRequest.Create(url+"pa/payment_intents/create");
					ret             = 520;
					wCall           = WebCall(webRequest, "Create payment intent", xmlSent, ref strResult);
					payRef          = Tools.JSONValue(strResult,"client_secret");
					customerId      = Tools.JSONValue(strResult,"customer_id");
					paymentIntentId = Tools.JSONValue(strResult,"id");

					if ( wCall > 0 || strResult.Length < 1 || paymentIntentId.Length < 1 || payRef.Length < 1 )
						return ExtractErrorAndReturn(530);

					if ( transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck )
						return 0;

				//	Confirm the Payment Intent
				//	https://www.airwallex.com/docs/api#/Payment_Acceptance/Payment_Intents/_api_v1_pa_payment_intents__id__confirm/post

					ret        = 600;
					strResult  = "";
					resultCode = "42";
					resultMsg  = "(42) Internal error confirming a payment intent";

				//	Ver 1: Reference a previous Payment Consent (does not work - it asks for customer intervention)
				//
				//	xmlSent    = "{ \"customer_id\" : \"" + payment.CustomerID + "\","
				//	           +   "\"payment_consent_reference\" :"
				//	           +     "{ \"cvc\" : \"" + payment.CardCVV + "\","
				//	           +       "\"id\" : \"" + payment.CardToken + "\" },"
				//	           +   "\"payment_method\" :"
				//	           +     "{ \"type\" : \"card\","
				//	           +       "\"card\" :"
				//	           +         "{ \"billing\" :"
				//	           +             "{ \"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" },"
				//	           +               "\"first_name\" : \""   + payment.FirstName + "\","
				//	           +               "\"last_name\" : \""    + payment.LastName + "\","
				//	           +               "\"email\" : \""        + payment.EMail + "\","
				//	           +               "\"phone_number\" : \"" + payment.PhoneCell + "\" },"
				//	           +          "\"cvc\" : \""               + payment.CardCVV + "\","
				//	           +          "\"expiry_month\" : \""      + payment.CardExpiryMM + "\","
				//	           +          "\"expiry_year\" : \""       + payment.CardExpiryYYYY + "\","
				//	           +          "\"name\" : \""              + payment.CardName + "\","
				//	           +          "\"number\" : \""            + payment.CardNumber + "\","
				//	           +          "\"number_type\" : \"PAN\" } },"
				//	           +   "\"device_data\" :"
				//	           +     "{ \"ip_address\" : \"" + payment.MandateIPAddress + "\","
				//	           +       "\"accept_header\" : \"*/*\","
				//	           +       "\"browser\" :"
				//	           +         "{ \"user_agent\" : \"" + (payment.MandateBrowser.Length > 0 ? payment.MandateBrowser : "Mozilla Firefox 121.0.1") + "\","
				//	           +           "\"java_enabled\" : false,"
				//	           +           "\"javascript_enabled\" : true } },"
				//	           +   "\"descriptor\" : \"" + payment.PaymentDescription + "\","
				//	           +   "\"merchant_order_id\" : \"" + payment.MerchantReference + "\","
				//	           +   "\"risk_control_options\" : { \"skip_risk_processing\" : true },"
				//	           +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";

				//	Ver 2: Provide full card details. Works
				//
				//	xmlSent    = "{ \"payment_method\" :"
				//	           +     "{ \"type\" : \"card\","
				//	           +       "\"card\" :"
				//	           +         "{ \"billing\" :"
				//	           +             "{ \"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" },"
				//	           +               "\"first_name\" : \""   + payment.FirstName + "\","
				//	           +               "\"last_name\" : \""    + payment.LastName + "\","
				//	           +               "\"email\" : \""        + payment.EMail + "\","
				//	           +               "\"phone_number\" : \"" + payment.PhoneCell + "\" },"
				//	           +          "\"cvc\" : \""               + payment.CardCVV + "\","
				//	           +          "\"expiry_month\" : \""      + payment.CardExpiryMM + "\","
				//	           +          "\"expiry_year\" : \""       + payment.CardExpiryYYYY + "\","
				//	           +          "\"name\" : \""              + payment.CardName + "\","
				//	           +          "\"number\" : \""            + payment.CardNumber + "\","
				//	           +          "\"number_type\" : \"PAN\" } },"
				//	           +   "\"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\" }";

					if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty ) // TokenEx
					//	Ver 3: Provide full card details but using a TokenEx token for the card number
						xmlSent = "{ \"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\","
						        +   "\"payment_method\" :"
						        +     "{ \"type\" : \"card\","
						        +       "\"card\" : {"
						        +          "\"number\" : \"##CARDNUM##\","
						        +          "\"cvc\" : \"##CVV##\","
						        +          "\"number_type\" : \"PAN\","
						        +          "\"expiry_month\" : \"" + payment.CardExpiryMM + "\","
						        +          "\"expiry_year\" : \""  + payment.CardExpiryYYYY + "\","
						        +          "\"name\" : \""         + payment.CardName + "\"";
					else if ( payment.CustomerID.Length > 0 )
					//	Ver 4: Use an existing customer id
						xmlSent = "{ \"request_id\" : \"" + (Guid.NewGuid()).ToString() + "\","
						        +   "\"customer_id\" : \"" + payment.CustomerID + "\","
						        +   "\"payment_consent_reference\" : { \"id\" : \"" + payment.CardToken + "\" }";

					ret        = 607;
					if ( payment.CountryCode(65).Length > 0 && payment.FirstName.Length > 0 && payment.LastName.Length > 0 )
						xmlSent = xmlSent 
					           + ",\"billing\" :"
					           +    "{ \"address\" : { \"country_code\" : \"" + payment.CountryCode(65) + "\" },"
					           +      "\"first_name\" : \""   + payment.FirstName + "\","
					           +      "\"last_name\" : \""    + payment.LastName + "\","
					           +      "\"email\" : \""        + payment.EMail + "\","
					           +      "\"phone_number\" : \"" + payment.PhoneCell + "\" }";

					xmlSent    = xmlSent + "} }";
					ret        = 610;
					if ( payment.MandateBrowser.Length > 0 && payment.MandateIPAddress.Length > 0 )
						xmlSent = xmlSent
					           +  ",\"device_data\" :"
					           +     "{ \"ip_address\" : \"" + payment.MandateIPAddress + "\","
					           +       "\"accept_header\" : \"*/*\","
					           +       "\"browser\" :"
					           +         "{ \"user_agent\" : \"" + payment.MandateBrowser + "\","
					           +           "\"java_enabled\" : false,"
					           +           "\"javascript_enabled\" : true } }";
					xmlSent    = xmlSent + " }";

//	Replace the CVV if we have it
//					if ( payment.CardCVV.Length >= 3 && Tools.StringToInt(payment.CardCVV) > 0 )
//				 		xmlSent = xmlSent.Replace("##CVV##",payment.CardCVV);

					if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty && txURL.Length > 0 )
					{
					//	First tokenize the CVV via TokenEx
						using (TransactionTokenEx tranTx = new TransactionTokenEx())
						{
							ret                 = 615;
							Payment pmnt        = new Payment();
							pmnt.BureauCode     = Tools.BureauCode(Constants.PaymentProvider.TokenEx);
							pmnt.ProviderKey    = payment.TokenizerKey;
							pmnt.ProviderUserID = payment.TokenizerID;
							pmnt.CardNumber     = payment.CardNumber;
							pmnt.CardToken      = payment.CardToken;
							pmnt.CardCVV        = payment.CardCVV;
						//	pmnt.ProviderURL    = txURL;                // NO! This is different
						//	pmnt.ProviderURL    = payment.TokenizerURL; // NO!
							if ( pmnt.CardToken.Length > 0 )
								ret = tranTx.AssociateCVV(pmnt);
							else if ( pmnt.CardNumber.Length > 0 )
								ret = tranTx.GetToken(pmnt);
							else
								ret = 620;
							if ( ret == 0 && tranTx.PaymentToken.Length > 0 )
								payment.CardToken = tranTx.PaymentToken;
							else
								Tools.LogInfo("CallWebService/33","Associating CVV with token via TokenEx failed (ret="+ret.ToString()+")",222,this);
							pmnt = null;
						}
					//	Now do the AirWallex payment intent
						ret                                = 625;
					 	xmlSent                            = xmlSent.Replace("##CARDNUM##","{{{" + payment.CardToken + "}}}");
					 	xmlSent                            = xmlSent.Replace("##CVV##"    ,"{{{cvv}}}");
						webRequest                         = (HttpWebRequest)WebRequest.Create(txURL);
						webRequest.Headers["TX_URL"]       = url + "pa/payment_intents/" + paymentIntentId + "/confirm";
						webRequest.Headers["TX_TokenExID"] = payment.TokenizerID;  // "4311038889209736";
						webRequest.Headers["TX_APIKey"]    = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
						webRequest.Headers["TX_CacheCvv"]  = "true";
					}
					else
					{
						ret          = 630;
					//	xmlSent      = xmlSent + "\"" + payment.CardNumber + "\" } } }";
					 	xmlSent      = xmlSent.Replace("##CARDNUM##",payment.CardNumber);
				 		xmlSent      = xmlSent.Replace("##CVV##",payment.CardCVV);
						webRequest   = (HttpWebRequest)WebRequest.Create(url+"pa/payment_intents/" + paymentIntentId + "/confirm");
					}

					ret             = 640;
					wCall           = WebCall(webRequest, "Confirm payment intent", xmlSent, ref strResult);
					payRef          = Tools.JSONValue(strResult,"id");
					customerId      = Tools.JSONValue(strResult,"customer_id");
					paymentIntentId = Tools.JSONValue(strResult,"payment_intent_id");

					if ( wCall > 0 || strResult.Length < 1 || paymentIntentId.Length < 1 || payRef.Length < 1 )
						return ExtractErrorAndReturn(630);

					resultCode = "00";
					resultMsg  = "(00) Successful";
					ret        = 0;
				}

			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".CallWebService/297","ret="+ret.ToString());
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("CallWebService/298","ret="+ret.ToString(),220,this);
				Tools.LogException("CallWebService/299","ret="+ret.ToString(),ex2,this);
			}
			return ret;
		}

		private int TestService(byte live=0)
      {
//			Testing only!
			try
			{
			}
			catch (Exception ex)
			{
				Tools.LogException("TestService/99","",ex,this);
			}
			return 0;
		}

		private int WebCall(HttpWebRequest webRequest, string action, string dataSent, ref string dataReceived)
		{
			int wRet     = 10;
			dataReceived = "";

			try
			{
				Tools.LogInfo("WebCall/10","URL="+webRequest.RequestUri+", action="+action+", JSON Sent="+dataSent,logPriority,this);

				wRet                   = 20;
				byte[] page            = Encoding.UTF8.GetBytes(dataSent);
				webRequest.ContentType = "application/json";
				webRequest.Accept      = "application/json";
				webRequest.Method      = "POST";
				if ( accessToken.Length > 0 )
					webRequest.Headers["Authorization"] = "Bearer " + accessToken;

				using (Stream stream = webRequest.GetRequestStream())
				{
					wRet = 30;
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				wRet = 40;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					wRet = 50;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						wRet         = 60;
						dataReceived = rd.ReadToEnd();
						rd.Close();
					}
					webResponse.Close();
				}

				Tools.LogInfo("WebCall/20","JSON Rec="+dataReceived,logPriority,this);
				wRet = 0;
			}
			catch (WebException ex1)
			{
			//	To Do
			//	Check for access token error here
			//	To Do
				dataReceived = Tools.DecodeWebException(ex1,ClassName+".WebCall/80","wRet="+wRet.ToString());
			//	Tools.LogException("WebCall/81","wRet="+wRet.ToString()+", action="+action+", dataReceived="+dataReceived,ex1,this);
			//	Tools.LogInfo     ("WebCall/82","wRet="+wRet.ToString()+", action="+action+", dataReceived="+dataReceived,222,this);
			}
			catch (Exception ex2)
			{
				Tools.LogException("WebCall/91","wRet="+wRet.ToString()+", action="+action+", dataReceived="+dataReceived,ex2,this);
				Tools.LogInfo     ("WebCall/92","wRet="+wRet.ToString()+", action="+action+", dataReceived="+dataReceived,222,this);
			}
			finally
			{
				webRequest = null;
			}
			return wRet;
		}

		private int GetAccessToken(byte mode,string url,string userID,string pwd)
		{
//			Mode = 1 means look for a key in the SQL DB. If one is not found or it has expired, get a new one
//			Mode = 2 means get a new key, don't even look in the SQL DB	

			int errCode = 0;
			Tools.LogInfo("GetAccessToken/10","Mode="+mode.ToString()+", "+AccessTokenSummary(),logPriority,this);

			if ( ( string.IsNullOrWhiteSpace(accessToken) || accessDate <= System.DateTime.UtcNow ) && mode != 2 )
				using ( MiscList mList = new MiscList() )
					try
					{
						accessToken = "";
						errCode     = 20;
						string sql  = "exec sp_GET_PaymentBureauAccessToken @PaymentBureauCode = " + Tools.DBString(bureauCode)
						            +                                     ",@PaymentBureauUserCode = '001'";
						Tools.LogInfo("GetAccessToken/20","Mode="+mode.ToString()+", No token, look for one ("+sql+")",logPriority,this);

						if ( mList.ExecQuery(sql,0) == 0 && ! mList.EOF )
						{
							errCode     = 30;
							accessToken = mList.GetColumn    ("AccessTokenID");
							accessDate  = mList.GetColumnDate("TokenExpiryDate");
							if ( accessToken.Length < 1 || accessDate <= System.DateTime.UtcNow )
								errCode  = 40;
							else
								errCode  = 0;

						//	accessDate  = mList.GetColumnDate("AccessTokenDate",0);
						//	if ( accessToken.Length > 0 && accessDate.AddMinutes(20) > System.DateTime.Now )
						//		errCode  = 0;
						//	else
						//		errCode  = 40;
						}
						else
							errCode     = 50;

						Tools.LogInfo("GetAccessToken/30","Mode="+mode.ToString()+", "+AccessTokenSummary()+", errCode="+errCode.ToString(),logPriority,this);
					}
					catch (Exception ex)
					{
						Tools.LogInfo("GetAccessToken/40",ex.ToString(),222,this);
					}

			else
				Tools.LogInfo("GetAccessToken/50","Mode="+mode.ToString()+", Existing "+AccessTokenSummary(),logPriority,this);

			if ( string.IsNullOrWhiteSpace(accessToken) || errCode > 0 || mode == 2 )
			{
				accessToken                       = "";
				strResult                         = "";
				HttpWebRequest webRequest         = (HttpWebRequest)WebRequest.Create(url+"authentication/login");
				webRequest.Headers["x-client-id"] = userID;
				webRequest.Headers["x-api-key"]   = pwd;
				errCode                           = WebCall(webRequest, "Access Token", "", ref strResult);
				accessToken                       = Tools.JSONValue(strResult,"token");
				accessDate                        = System.DateTime.UtcNow.AddMinutes(25);

				Tools.LogInfo("GetAccessToken/60","Mode="+mode.ToString()+", New "+AccessTokenSummary()+", result="+strResult,logPriority,this);

				if ( errCode == 0 && accessToken.Length > 0 )
					errCode = SaveAccessToken();
			}
			else
				Tools.LogInfo("GetAccessToken/70","Mode="+mode.ToString()+", Existing "+AccessTokenSummary(),logPriority,this);

			return errCode;
		}

		private string AccessTokenSummary()
		{
			if ( string.IsNullOrWhiteSpace(accessToken) )
				return "Token=NULL";
			string tk = accessToken;
			int    ln = accessToken.Length;
			if ( ln > 20 )
				tk = tk.Substring(0,8) + "...." + tk.Substring(ln-8);
			tk = "Token=" + tk + " (" + ln.ToString() + ") / Expiry=";
			if ( accessDate < System.Convert.ToDateTime("1999/12/31") )
				return tk + "NULL";
			return tk + Tools.DateToString(accessDate,2,1);
		}

		private int SaveAccessToken()
		{
			if ( string.IsNullOrWhiteSpace(accessToken) )
				return 10;

			int    errCode = 10;
			string sql     = "";

			using ( MiscList mList = new MiscList() )
				try
				{
					errCode = 20;
					sql     = "exec sp_UPD_PaymentBureauAccessToken @PaymentBureauCode = " + Tools.DBString(bureauCode)
				           +                                     ",@PaymentBureauUserCode = '001'"
				           +                                     ",@AccessTokenID = "     + Tools.DBString(accessToken)
				           +                                     ",@AccessTokenDate = "   + Tools.DateToSQL(System.DateTime.UtcNow,1);
					Tools.LogInfo("SaveAccessToken/10",sql,logPriority,this);
					if ( mList.ExecQuery(sql,0,"",false,true) == 0 )
						errCode = 0;
				}
				catch (Exception ex)
				{
					Tools.LogInfo     ("SaveAccessToken/20",ex.ToString(),logPriority,this);
					Tools.LogException("SaveAccessToken/30",sql,ex,this);
				}

			return errCode;
		}

		public TransactionAirWallex() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.AirWallex);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
//			logPriority                           = 10;  // For production, when all is stable
			logPriority                           = 222; // For testing/development, to log very detailed errors
			err                                   = "";
		}
	}
}
