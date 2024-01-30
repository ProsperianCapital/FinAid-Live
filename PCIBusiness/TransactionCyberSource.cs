using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;

namespace PCIBusiness
{
	public class TransactionCyberSource : Transaction
	{
		private string webForm;
		private int    ret;

		public  bool Successful
		{
			get
			{
				string p = Tools.NullToString(resultCode).ToUpper();
				return ( p == "AUTHORIZED" || p.StartsWith("AUTHORIZED/") || p == "ACTIVE" || p.StartsWith("ACTIVE/") );
			}
		}

		public override string WebForm
		{
			get { return Tools.NullToString(webForm); }
		}

		public override int CardTest(Payment payment)
		{
//			TestTransactionV1();
//			TestTransactionV2();

			return CardPayment(null);
		}

		private string CardType(string cardNumber)
		{
			string issuer = Tools.CardIssuer(cardNumber).ToUpper();
			if ( issuer.StartsWith("VISA")   ) return "001";
			if ( issuer.StartsWith("MASTER") ) return "002";
			if ( issuer.StartsWith("AME") )    return "003";
			if ( issuer.StartsWith("DISC") )   return "004";
			if ( issuer.StartsWith("DINE") )   return "005";
			return "";
		}

		public override int CardPayment(Payment payment)
		{
			ret        = 10;
			resultCode = "ERROR/Internal (86)";
			resultMsg  = "(86) Internal error";

			try
			{
				if ( payment == null || payment.TransactionType == (byte)Constants.TransactionType.Test )
					xmlSent  = "{"
						+ "  \"clientReferenceInformation\": {"
						+ "    \"code\": \"TC50171_3\""
						+ "  },"
						+ "  \"processingInformation\": {"
						+ "    \"commerceIndicator\": \"internet\""
						+ "  },"
						+ "  \"paymentInformation\": {"
						+ "    \"card\": {"
						+ "      \"number\": \"4111111111111111\","
						+ "      \"expirationMonth\": \"12\","
						+ "      \"expirationYear\": \"2024\","
						+ "      \"securityCode\": \"123\""
						+ "    }"
						+ "  },"
						+ "  \"orderInformation\": {"
						+ "    \"amountDetails\": {"
						+ "      \"totalAmount\": \"93.21\","
						+ "      \"currency\": \"ZAR\""
						+ "    },"
						+ "    \"billTo\": {"
						+ "      \"firstName\": \"John\","
						+ "      \"lastName\": \"Doe\","
						+ "      \"company\": \"Visa\","
						+ "      \"address1\": \"1 Market Str\","
						+ "      \"address2\": \"Address 2\","
						+ "      \"locality\": \"san francisco\","
						+ "      \"administrativeArea\": \"CA\","
						+ "      \"postalCode\": \"94105\","
						+ "      \"country\": \"US\","
						+ "      \"email\": \"test@cybs.com\","
						+ "      \"phoneNumber\": \"4158880000\""
						+ "    }"
						+ "  }"
						+ "}";

				else
					xmlSent  = "{"
						+ "  \"clientReferenceInformation\": {"
						+ "    \"code\": \"" + Tools.JSONSafe(payment.MerchantReference) + "\""
						+ "  },"
						+ "  \"processingInformation\": {"
						+ "    \"commerceIndicator\": \"internet\""
						+ "  },"
						+ "  \"paymentInformation\": {"
						+ "    \"card\": {"
						+ "      \"number\": \""          + payment.CardNumber     + "\","
						+ "      \"expirationMonth\": \"" + payment.CardExpiryMM   + "\","
						+ "      \"expirationYear\": \""  + payment.CardExpiryYYYY + "\","
						+ "      \"securityCode\": \""    + payment.CardCVV        + "\""
						+ "    }"
						+ "  },"
						+ "  \"orderInformation\": {"
						+ "    \"amountDetails\": {"
						+ "      \"totalAmount\": \""  + payment.PaymentAmountDecimal + "\","
						+ "      \"currency\": \"ZAR\""
						+ "    },"
						+ "    \"billTo\": {"
						+ "      \"firstName\": \""    + Tools.JSONSafe(payment.FirstName)       + "\","
						+ "      \"lastName\": \""     + Tools.JSONSafe(payment.LastName)        + "\","
						+ "      \"address1\": \""     + Tools.JSONSafe(payment.Address1(65))    + "\","
						+ "      \"address2\": \""     + Tools.JSONSafe(payment.Address2(65))    + "\","
						+ "      \"locality\": \""     + Tools.JSONSafe(payment.Address3(65))    + "\","
						+ "      \"postalCode\": \""   + Tools.JSONSafe(payment.PostalCode(65))  + "\","
						+ "      \"country\": \""      + Tools.JSONSafe(payment.CountryCode(65)) + "\","
						+ "      \"email\": \""        + Tools.JSONSafe(payment.EMail,1)         + "\","
						+ "      \"phoneNumber\": \""  + Tools.JSONSafe(payment.PhoneCell)       + "\""
						+ "    }"
						+ "  }"
						+ "}";
				ret        = CallWebService(null,(byte)Constants.TransactionType.CardPayment);
				resultCode = Tools.JSONValue(strResult,"responseCode");
				resultMsg  = Tools.JSONValue(strResult,"status");
//	To Do
//				if ( resultCode.Length == 0 )
//					resultCode = "ERROR";

				return ret;
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("CardPayment/198",resultCode + " | " + resultMsg + " | " + ret.ToString() + " | " + xmlSent,255,this);
				Tools.LogException("CardPayment/199",resultCode + " | " + resultMsg + " | " + ret.ToString() + " | " + xmlSent,ex,this);
			}
			return 203;
		}


		public override int TokenPayment(Payment payment)
		{
			ret        = 10;
			resultCode = "ERROR/Internal (87)";
			resultMsg  = "(87) Internal error";

//	TESTING
//			payment.CardToken         = "BC2A907BB21E7C3DE0531D588D0AD440";
//			payment.CardCVV           = "324";
//			payment.PaymentAmount     = 179;
//			payment.MerchantReference = "CYBERSOURCE-TEST-01";
//	TESTING

			try
			{
				if ( payment == null || payment.TransactionType == (byte)Constants.TransactionType.Test )
					xmlSent  = "{"
						+ "  \"clientReferenceInformation\": {"
						+ "    \"code\": \"TC50171_3\""
						+ "  },"
						+ "  \"processingInformation\": {"
						+ "    \"capture\": \"true\""
						+ "  },"
					   + "  \"paymentInstrument\": {"
						+ "    \"id\": \"BC02545C464225DFE05341588E0AE7C9\""
						+ "  },"
						+ "  \"paymentInformation\": {"
						+ "    \"card\": {"
						+ "      \"securityCode\": \"123\""
						+ "    }"
						+ "  },"
						+ "  \"orderInformation\": {"
						+ "    \"amountDetails\": {"
						+ "      \"totalAmount\": \"14.21\","
						+ "      \"currency\": \"ZAR\""
						+ "    }"
						+ "  }"
						+ "}";

				else
					xmlSent  = "{ \"clientReferenceInformation\": { \"code\": \"" + Tools.JSONSafe(payment.MerchantReference) + "\" },"
						      +   "\"processingInformation\": { \"capture\": \"true\" },"
					         +   "\"paymentInformation\": {"
						      +      "\"card\": { \"securityCode\": \"" + payment.CardCVV + "\" },"
					         +      "\"paymentInstrument\": { \"id\": \"" + payment.CardToken + "\" }},"
						      +   "\"orderInformation\": {"
					         +      "\"amountDetails\": { \"totalAmount\": \"" + payment.PaymentAmountDecimal + "\","
						      +                           "\"currency\": \"ZAR\" }}}";

				ret           = CallWebService(payment,payment.TransactionType);
				payRef        = Tools.JSONValue(strResult,"transactionId");
				resultCode    = Tools.JSONValue(strResult,"status");
				string reason = Tools.JSONValue(strResult,"reason");
				resultMsg     = Tools.JSONValue(strResult,"message");
				string respCd = Tools.JSONValue(strResult,"responseCode");

				if ( resultCode.Length == 0 )
					resultCode = "ERROR";
				if ( reason.Length > 0 )
					resultCode = resultCode + ( resultCode.Length > 0 ? "/" : "" ) + reason;
				else if ( resultCode == "ERROR" )
					resultCode = resultCode + "/Internal (64)";
				if ( respCd.Length > 0 )
					resultMsg  = resultMsg  + ( resultMsg.Length  > 0 ? "/" : "" ) + respCd;
				if ( ret == 0 && Successful )
					return 0;
				else if ( ret == 0 )
					ret = 64;
			}
			catch (Exception ex)
			{
				if ( ret == 0 )
					ret = 93;
				Tools.LogInfo     ("TokenPayment/198",resultCode + " | " + resultMsg + " | " + ret.ToString() + " | " + xmlSent,255,this);
				Tools.LogException("TokenPayment/199",resultCode + " | " + resultMsg + " | " + ret.ToString() + " | " + xmlSent,ex,this);
			}
			return ret;
		}

		public override int GetToken3rdParty(Payment payment)
		{
			return CreateToken(payment,Constants.TransactionType.GetTokenThirdParty);
		}

		public override int GetToken(Payment payment)
		{
			return CreateToken(payment,Constants.TransactionType.GetToken);
		}

		private int CreateToken(Payment payment,Constants.TransactionType transactionType)
		{
			ret          = 10;
			resultCode   = "ERROR/Internal (85)";
			resultMsg    = "(85) Internal error";
			payToken     = "";
			string payId = "";

			try
			{
				Tools.LogInfo("CreateToken/10","Merchant Ref=" + payment.MerchantReference,10,this);
// v1
//				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
//				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
//				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
//				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
//				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
//				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,1,"","}") // Changed to STRING from NUMERIC

//				xmlSent  = "{ \"card\" : " + Tools.JSONPair("number"         ,payment.CardNumber,1,"{")
//				                           + Tools.JSONPair("expirationYear" ,payment.CardExpiryYYYY,11)
//				                           + Tools.JSONPair("expirationMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
//				                           + Tools.JSONPair("securityCode"   ,payment.CardCVV,1,"","}") // Changed to STRING from NUMERIC
//				         + "}";

//	LIVE testing [REMOVE], v1
//				payment.FirstName      = "Francois";
//				payment.LastName       = "Olivier";
//				payment.CardNumber     = "4787698041719016";
//				payment.CardExpiryMM   = "03";
//				payment.CardExpiryYYYY = "2023";
//				payment.CardCVV        = "407";
//				payment.CardType       = "visa";
// LIVE testing [REMOVE], v2
//				payment.FirstName      = "Paul";
//				payment.LastName       = "Kilfoil";
//				payment.CardNumber     = "4901360180458036";
//				payment.CardExpiryMM   = "01";
//				payment.CardExpiryYYYY = "2023";
//				payment.CardCVV        = "324";
//				payment.CardType       = "visa";
// LIVE testing [REMOVE], v3
//				payment.FirstName      = "JAF";
//				payment.LastName       = "Jenkins";
//				payment.CardNumber     = "5284973093146722";
//				payment.CardExpiryMM   = "02";
//				payment.CardExpiryYYYY = (System.DateTime.Now.Year+1).ToString();
//				payment.CardCVV        = "137";
//				payment.CardType       = "mastercard";
// LIVE testing [REMOVE]

//	First create an Instrument Identifier

				if ( transactionType == Constants.TransactionType.GetTokenThirdParty )
					xmlSent = "{{{" + payment.CardToken + "}}}"; // TokenEx
				else
					xmlSent = payment.CardNumber;

				xmlSent    = "{ \"card\" : " + Tools.JSONPair("number",xmlSent,1,"{","}") + "}";
				ret        = 20;
				ret        = CallWebService(payment,(byte)transactionType,1);
				payId      = Tools.JSONValue(strResult,"id"); // Instrument id
				resultCode = Tools.JSONValue(strResult,"state");

				if ( ret  != 0 || payId.Length < 1 || resultCode.Length == 0 )
				{
					resultCode = ( resultCode.Length == 0 ? "ERROR/Instrument Id" : resultCode );
					resultMsg  = "Unable to create instrument identifier";
					Tools.LogInfo("CreateToken/20","ret="+ret.ToString()+" | "+xmlSent+" | "+strResult,222,this);
					return ret;
				}

//	Now create a Payment Instrument

				ret        = 60;
				payToken   = "";
				xmlSent    = "{ \"card\": { \"expirationMonth\": \""      + payment.CardExpiryMM                    + "\""
				           +             ", \"expirationYear\": \""       + payment.CardExpiryYYYY                  + "\""
				           +             ", \"type\": \""                 + Tools.JSONSafe(payment.CardType)        + "\" }"
				           + ", \"billTo\": { \"firstName\": \""          + Tools.JSONSafe(payment.FirstName)       + "\""
				           +               ", \"lastName\": \""           + Tools.JSONSafe(payment.LastName)        + "\""
				           +               ", \"address1\": \""           + Tools.JSONSafe(payment.Address1(65))    + "\""
				           +               ", \"locality\": \""           + Tools.JSONSafe(payment.Address2(65))    + "\""
				           +               ", \"administrativeArea\": \"" + Tools.JSONSafe(payment.Address3(65))    + "\""
				           +               ", \"postalCode\": \""         + Tools.JSONSafe(payment.PostalCode(65))  + "\""
				           +               ", \"country\": \""            + Tools.JSONSafe(payment.CountryCode(65)) + "\""
				           +               ", \"email\": \""              + Tools.JSONSafe(payment.EMail,1)         + "\""
				           +               ", \"phoneNumber\": \""        + Tools.JSONSafe(payment.PhoneCell)       + "\" }"
				           + ", \"instrumentIdentifier\": { \"id\": \""   + payId                                   + "\" } }";
				ret        = 70;
				ret        = CallWebService(payment,(byte)Constants.TransactionType.GetToken,2);
				payToken   = Tools.JSONValue(strResult,"id"); // Payment instrument
				resultCode = Tools.JSONValue(strResult,"state");
				resultMsg  = "";
				if ( resultCode.Length == 0 )
					resultCode = "ERROR/Payment Instrument";

				if ( ret == 0 && payToken.Length > 0 && payId.Length > 0 && payToken != payId )
					return 0;
				else if ( ret == 0 )
					ret = 90;

				resultMsg = "Unable to create payment instrument";
				Tools.LogInfo("CreateToken/90","ret="+ret.ToString()+" | "+xmlSent+" | "+strResult,222,this);
			}
			catch (Exception ex)
			{
				payId = resultCode + " | " + resultMsg + " | " + ret.ToString() + " | " + payId + " | " + payToken + " | " + xmlSent;
				Tools.LogInfo     ("CreateToken/198",payId,255,this);
				Tools.LogException("CreateToken/199",payId,ex ,this);
			}
			return ret;
		}

		public int TokenPaymentV1(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			ret    = 10;
			payRef = "";

			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
				xmlSent = "{ \"creditCard\" : "  + Tools.JSONPair("token"    ,payment.CardToken,1,"{","}")
				        + ", \"transaction\" : " + Tools.JSONPair("reference",payment.MerchantReference,1,"{")
				                                 + Tools.JSONPair("currency" ,payment.CurrencyCode,1)
				                                 + Tools.JSONPair("amount"   ,payment.PaymentAmount.ToString(),11,"","}")
				        + ", "                   + Tools.JSONPair("threeDSecure","false",12,"","")
				        + "}";

				ret     = 20;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				ret     = 30;
				payRef  = Tools.JSONValue(strResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/198","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/199","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex,this);
			}
			return ret;
		}

		public override int ThreeDSecureCheck(string providerRef,string merchantRef="",string data1="",string data2="",string data3="")
		{
			return 0; // All ok
		}

		private int CallWebService(Payment payment,byte transactionType,byte subType=0)
      {
			strResult = Tools.JSONPair("status","ERROR",1,"{")
			          + Tools.JSONPair("state" ,"ERROR/Internal (71)")
			          + Tools.JSONPair("reason","Internal (71)",1,"","}");

			if ( payment == null )
			{
				payment              = new Payment();
				payment.BureauCode   = BureauCode;
				payment.ProviderURL  = BureauURL;
			//	payment.BureauCode   = Tools.BureauCode(Constants.PaymentProvider.CyberSource);
			}

//	TESTING
//			payment.ProviderAccount = "000000002744639";
//			payment.ProviderUserID  = "31c799cd-18da-47c3-be95-f93bd90748e0";
//			payment.ProviderKey     = "IcJSjbVloKPQsS5PJrCdGOz8W/pLOBjzO4QVqKG4Ai8=";
//			payment.ProviderURL     = "https://apitest.cybersource.com";
//	TESTING

			string tURL     = payment.TokenizerURL;
			string pURL     = payment.ProviderURL;
			string pURLPart = "/pts/v2/payments";
			string tranDesc = "";
			ret             = 10;

			if ( Tools.NullToString(pURL).Length == 0 )
				pURL = BureauURL;

			ret = 20;
			if ( pURL.EndsWith("/") )
				pURL = pURL.Substring(0,pURL.Length-1);

			ret = 30;

			if ( transactionType == (byte)Constants.TransactionType.GetToken ||
			     transactionType == (byte)Constants.TransactionType.GetTokenThirdParty )
				if ( subType == 1 )
				{
					pURLPart = "/tms/v1/instrumentidentifiers";
					tranDesc = "Create Instrument Id";
				}
				else
				{
					pURLPart = "/tms/v1/paymentinstruments";
					tranDesc = "Create Payment Instrument (Token)";
				}

			else if ( transactionType == (byte)Constants.TransactionType.CardPayment ||
			          transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				tranDesc = "Card Payment";

			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
				tranDesc = "Token Payment";

			else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
				tranDesc = "3d Secure Payment";

			else
			{ }

			ret  = 60;
			pURL = pURL + pURLPart;

			try
			{
				string         digest;
				string         sigCoded;
				string         sigSource;
				HttpWebRequest webReq;
				DateTime       theDate = System.DateTime.UtcNow;
				string         sep     = "\"";
				byte[]         page    = Encoding.UTF8.GetBytes(xmlSent);

				if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty   ||
				     transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				{
					ret = 80;
					if ( tURL.Length < 1 )
					{
						Tools.LogInfo("CallWebService/80","Unknown Third Party Tokenizer (" + bureauCodeTokenizer + "), data=" + xmlSent,221,this);
						return ret;
					}
					if ( ! tURL.ToUpper().EndsWith("DETOKENIZE") )
						tURL = tURL + "/TransparentGatewayAPI/Detokenize";

					ret                            = 83;
					webReq                         = (HttpWebRequest)HttpWebRequest.Create(tURL);
					webReq.Headers["TX_URL"]       = pURL;
					webReq.Headers["TX_TokenExID"] = payment.TokenizerID;  // "4311038889209736";
					webReq.Headers["TX_APIKey"]    = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
//					webReq.Headers["TX_Headers"]   = "Host";
				}
				else
//					webReq                         = (HttpWebRequest)HttpWebRequest.Create(pURL);
					webReq                         = (HttpWebRequest)WebRequest.Create(pURL);

				webReq.ContentType                = "application/json";
//				webReq.Accept                     = "application/json";
				webReq.Method                     = "POST";
				webReq.Date                       = theDate;
//				webReq.Host                       = payment.ProviderHost; // No need to set this
				ret                               = 160;
				digest                            = GenerateDigestV3(xmlSent);
				ret                               = 170;
//	Set headers
				webReq.Headers["v-c-merchant-id"] = payment.ProviderAccount;
//				webReq.Headers["Date"]            = theDate.ToString();        // "Fri, 11 Dec 2020 07:18:03 GMT";
//				webReq.Headers["Host"]            = "apitest.cybersource.com"; // NO! Cannot set this here
				webReq.Headers["Digest"]          = digest;
//	Not needed for REST, but include it if supplied
				if ( payment.ProviderProfileID.Length > 0 )
					webReq.Headers["profile-id"]   = payment.ProviderProfileID;

//				ret                               = 175;
//				sigSource                         = "host: "            + webRequest.Host            + "\n"
//				                                  + "date: "            + webRequest.Headers["Date"] + "\n"
//				                                  + "(request-target): post /pts/v2/payments/"       + "\n"
//				                                  + "digest: "          + digest                     + "\n"
//				                                  + "v-c-merchant-id: " + payment.ProviderAccount;

				ret                               = 180;
				sigSource                         = "v-c-merchant-id: " + payment.ProviderAccount    + "\n"
				                                  + "date: "            + webReq.Headers["Date"]     + "\n"
				                                  + "host: "            + webReq.Host                + "\n"
				                                  + "digest: "          + digest                     + "\n"
				                                  + "(request-target): post " + pURLPart;

				ret                               = 190;
				sigCoded                          = GenerateSignatureV3(sigSource,payment.ProviderKey);

				webReq.Headers["Signature"]       =   "keyid="     + sep + payment.ProviderUserID + sep
				                                  + ", algorithm=" + sep + "HmacSHA256" + sep
				                                  + ", headers="   + sep + "v-c-merchant-id date host digest (request-target)" + sep
//				                                  + ", headers="   + sep + "host date (request-target) digest v-c-merchant-id" + sep
				                                  + ", signature=" + sep + sigCoded + sep;
				ret                               = 200;

				if ( Tools.SystemIsLive() )
					Tools.LogInfo("CallWebService/201", "(In) Tran=" + tranDesc + Environment.NewLine + xmlSent, 220, this);
				else
					Tools.LogInfo("CallWebService/202", "Tran="             + tranDesc                  + Environment.NewLine
					                                  + "pURL="             + pURL                      + Environment.NewLine
					                                  + "tURL="             + tURL                      + Environment.NewLine
					                                  + "Merchant Id="      + payment.ProviderAccount   + Environment.NewLine
					                                  + "Profile Id="       + payment.ProviderProfileID + Environment.NewLine
					                                  + "Key Detail/Id="    + payment.ProviderUserID    + Environment.NewLine
					                                  + "Secret Key="       + payment.ProviderKey       + Environment.NewLine
					                                  + "JSON Sent="        + xmlSent                   + Environment.NewLine
					                                  + "Signature Input="  + sigSource                 + Environment.NewLine
					                                  + "Signature Output=" + sigCoded                  + Environment.NewLine
					                                  + "Request Header[v-c-merchant-id]=" + webReq.Headers["v-c-merchant-id"] + Environment.NewLine
					                                  + "Request Header[Date]="            + webReq.Headers["Date"]            + Environment.NewLine
					                                  + "Request Header[Host]="            + webReq.Host                       + Environment.NewLine
					                                  + "Request Header[Digest]="          + webReq.Headers["Digest"]          + Environment.NewLine
					                                  + "Request Header[Signature]="       + webReq.Headers["Signature"]       + Environment.NewLine
					                                  + "Request Header[profile-id]="      + payment.ProviderProfileID
					                                  , 231, this);

				using (Stream stream = webReq.GetRequestStream())
				{
					ret = 310;
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				ret = 320;

				using (HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse())
				{
					ret = 330;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret        = 340;
						strResult  = rd.ReadToEnd();
					}
				}
				if ( strResult.Length == 0 )
				{
					ret        = 350;
					strResult  = Tools.JSONPair("status","ERROR",1,"{")
								  + Tools.JSONPair("state" ,"ERROR/Empty response")
								  + Tools.JSONPair("reason","Empty response",1,"","}");
					resultMsg  = "No data returned from " + pURL + ( tURL.Length > 0 ? " (or " + tURL + ")" : "");
//					Tools.LogInfo("CallWebService/30","Failed, JSON Rec=(empty)",199,this);
				}
				else
					ret = 0;

				Tools.LogInfo("CallWebService/353", "(Out) Tran=" + tranDesc + Environment.NewLine + strResult, 220, this);
			}
			catch (WebException ex1)
			{
				tURL = Tools.DecodeWebException(ex1,ClassName+".CallWebService/597","ret="+ret.ToString());
				if ( tURL.Length > 0 )
					strResult = tURL;
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("CallWebService/598","ret="+ret.ToString(),220,this);
				Tools.LogException("CallWebService/599","ret="+ret.ToString(),ex2,this);
			}
			return ret;
		}

//	Code from CyberSource
//	Start
		private string GenerateDigestV3(string jsonData)
		{
			try
			{
				using (SHA256 sha256hash = SHA256.Create())
				{
					byte[] payloadBytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(jsonData));
					string digest       = Convert.ToBase64String(payloadBytes);
					return "SHA-256=" + digest;
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("GenerateDigest",jsonData,ex,this);
			}
			return "";
		}

		private string GenerateSignatureV3(string signatureParams, string secretKey)
		{
			var sigBytes      = Encoding.UTF8.GetBytes(signatureParams);
			var decodedSecret = Convert.FromBase64String(secretKey);
			var hmacSha256    = new HMACSHA256(decodedSecret);
			var messageHash   = hmacSha256.ComputeHash(sigBytes);
			return Convert.ToBase64String(messageHash);
		}
//	End

		private string GenerateSignatureV2(string data, string secretKey)
		{
			UTF8Encoding encoding     = new System.Text.UTF8Encoding();
			byte[]       keyByte      = encoding.GetBytes(secretKey);
			HMACSHA256   hmacsha256   = new HMACSHA256(keyByte);
			byte[]       messageBytes = encoding.GetBytes(data);
			return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
		}

		public void TestTransactionV3() // REST
		{
			CardPayment(null);
		}

		public int TestTransactionV2(byte mode=2)
		{
		//	Mode = 1 : Web form submit
		//	Mode = 2 : URL fields

		   ret                 = 10;
//			string    url       = "https://testsecureacceptance.cybersource.com/silent/token/create";
//			string    url       = "https://testsecureacceptance.cybersource.com/silent/embedded/pay";
			string    url       = "https://testsecureacceptance.cybersource.com/silent/pay";
//			string    profileId = "466381AB-5F66-4679-AFD4-5035EA9077A7";
//			string    accessKey = "faaf4d2bcc42365d90f853daa4096cdc";
//			string    secretKey = "73004ef0b7c041be93e03c995261fddb651b0d62b2e34ec093452c706da8c08bd8dd69ff8747423f9f27f05b01f3e9d2efb12af5a2834989b08b0ed461dfe55df4d43a7cb81942439fd3496724037cce5d62874a64fe450380f037603e120b5e9caebdf35d1d4fb98c2c52202ea0aae7fc640bd9b8f64709b2dd1598e6c5dd4f";
			string    profileId = "3C857FA4-ED86-4A08-A119-24170A74C760";
			string    accessKey = "8b031c20a1ad343c97afe1869e2e7994";
			string    secretKey = "2ea6c71fa7e04304a78f417c1e4d95677abb9673c7cd45ec803a08696041ea62b5eae10527704f4580ae8da223c295c0b42f97808adf4b6db1a2bf032eb74bd7376d9d1393f1443aaf8bcba7cd4d1148b3157119169c404fa74be9e4cd5cf9cacc34f76976f54bfa93136e4de6b1f53750a5e9d4b1cc4fcebd67a14bbcc156c3";
			DateTime  dt        = DateTime.Now.ToUniversalTime();
			string[,] fieldS;
			string[,] fieldU;

			try
			{
				string sigX    = "";
				string sigF    = "";
				string sigS    = "";
				string sigU    = "";
				string unsForm = "";
				string unsSent = "";

				fieldS = new string[,] { { "reference_number"                   , "123456" } // Merchant ref num
				                       , { "transaction_type"                   , "sale,create_payment_token" }
				                       , { "currency"                           , "ZAR" }
				                       , { "amount"                             , "19.37" }
				                       , { "locale"                             , "en" }
				                       , { "profile_id"                         , profileId }
				                       , { "access_key"                         , accessKey }
				                       , { "transaction_uuid"                   , System.Guid.NewGuid().ToString() } // MUST be unique
				                       , { "signed_date_time"                   , dt.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") } // "2021-01-10T03:16:54Z"
				                       , { "payment_method"                     , "card" }
				                       , { "card_cvn"                           , "005" }
				                       , { "bill_to_forename"                   , "Pete" }
				                       , { "bill_to_surname"                    , "Smith" }
				                       , { "bill_to_email"                      , "test@hotmail.com" }
				                       , { "bill_to_phone"                      , "0885556666" }
				                       , { "bill_to_address_line1"              , "133 Fiddlers Lane" }
				                       , { "bill_to_address_city"               , "Knysna" }
				                       , { "bill_to_address_postal_code"        , "4083" }
				                       , { "bill_to_address_country"            , "ZA" }
				                       , { "payer_authentication_challenge_code", "03" } // Force 3d Secure
				                       , { "payer_authentication_merchant_name" , "CareAssist" }
				                       , { "override_custom_cancel_page"        , "http://www.paulkilfoil.co.za/Leisure.aspx" }
				                       , { "override_custom_receipt_page"       , "http://www.paulkilfoil.co.za/Travelogues.aspx" } };
				fieldU = new string[,] { { "card_type"                          , "001" }
				                       , { "card_number"                        , "4456530000000007" }
				                       , { "card_expiry_date"                   , "12-" + (System.DateTime.Now.Year+1).ToString() } };
//				                       , { "card_number"                        , "4111111111111111" } // Visa
//				                       , { "card_number"                        , "5555555555554444" } // MasterCard

				ret     = 20;
				xmlSent = "";
				webForm = "<html><body onload='document.forms[\"frmX\"].submit()'>" + Environment.NewLine
				        + "<form name='frmX' method='POST' action='" + url + "'>" + Environment.NewLine;

				for ( int k = 0 ; k < fieldS.GetLength(0) ; k++ )
				{
					ret     = 30;
					xmlSent = xmlSent + "&" + fieldS[k,0] + "=" + Tools.URLString(fieldS[k,1]);
					webForm = webForm + "<input type='hidden' id='" + fieldS[k,0] + "' name='" + fieldS[k,0] + "' value='" + fieldS[k,1] + "' />" + Environment.NewLine;
					sigX    = sigX + "," + fieldS[k,0] + "=" + fieldS[k,1];
					sigS    = sigS + "," + fieldS[k,0];
				}

				for ( int k = 0 ; k < fieldU.GetLength(0) ; k++ )
				{
					ret     = 40;
					unsSent = unsSent + "&" + fieldU[k,0] + "=" + Tools.URLString(fieldU[k,1]);
					unsForm = unsForm + "<input type='hidden' id='" + fieldU[k,0] + "' name='" + fieldU[k,0] + "' value='" + fieldU[k,1] + "' />" + Environment.NewLine;
					sigU    = sigU + "," + fieldU[k,0];
				}

				ret     = 50;
				sigU    = sigU.Substring(1);
				sigS    = sigS.Substring(1) + ",signed_field_names,unsigned_field_names";
				sigX    = sigX.Substring(1) + ",signed_field_names=" + sigS + ",unsigned_field_names=" + sigU;
				xmlSent = xmlSent.Substring(1);
				sigF    = GenerateSignatureV2(sigX,secretKey);
				ret     = 60;
				webForm = webForm + "<input type='hidden' id='signed_field_names' name='signed_field_names' value='" + sigS + "' />" + Environment.NewLine
				                  + "<input type='hidden' id='unsigned_field_names' name='unsigned_field_names' value='" + sigU + "' />" + Environment.NewLine
				                  + unsForm
				                  + "<input type='hidden' id='signature' name='signature' value='" + sigF + "' />" + Environment.NewLine
				                  + "</form></body></html>";
				ret     = 70;
				xmlSent = xmlSent + "&signed_field_names="   + Tools.URLString(sigS)
				                  + "&unsigned_field_names=" + Tools.URLString(sigU)
				                  + unsSent
				                  + "&signature="            + Tools.URLString(sigF);

				Tools.LogInfo("TestTransactionV2/10","Profile Id="+profileId,222,this);
				Tools.LogInfo("TestTransactionV2/20","Access Key="+accessKey,222,this);
//				Tools.LogInfo("TestTransactionV2/30","Secret Key="+secretKey,222,this);
				Tools.LogInfo("TestTransactionV2/40","Signature Input="+sigX,222,this);
				Tools.LogInfo("TestTransactionV2/50","Signature Output="+sigF,222,this);
				Tools.LogInfo("TestTransactionV2/60","Web form="+webForm,222,this);
				Tools.LogInfo("TestTransactionV2/70","URL params="+xmlSent,222,this);

//				if ( mode == 1 ) //	Web form
//					return;

				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
				ret                       = 110;
				webRequest.Method         = "POST";
				webRequest.ContentType    = "application/x-www-form-urlencoded";
				webRequest.Accept         = "application/x-www-form-urlencoded";
				strResult                 = "";
				byte[] page               = Encoding.UTF8.GetBytes(xmlSent);

				ret = 120;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
//					stream.Flush();
					stream.Close();
				}

				ret = 130;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 140;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 150;
						strResult = rd.ReadToEnd();
					}
				}

				Tools.LogInfo("TestTransactionV2/160","XML Rec=" + strResult,222,this);
				ret = 160;
				if ( strResult.ToUpper().Contains("<HTML") && strResult.ToUpper().Contains("<FORM") )
				{
					d3Form = strResult;
					return 0;
				}
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".TestTransactionV2/297","ret="+ret.ToString());
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("TestTransactionV2/298","ret="+ret.ToString(),220,this);
				Tools.LogException("TestTransactionV2/299","ret="+ret.ToString(),ex2,this);
			}
			finally
			{
				fieldS = null;
				fieldU = null;
			}
			return ret;
		}

		public void TestTransactionV1() // SOAP
		{
			CyberSource.RequestMessage request = new CyberSource.RequestMessage();

			request.merchantID            = "2744639";
			request.merchantReferenceCode = "A1-TEST-82348687";

		//	Help with trouble-shooting
			request.clientLibrary        = ".NET WCF";
			request.clientLibraryVersion = Environment.Version.ToString();
			request.clientEnvironment    = Environment.OSVersion.Platform + Environment.OSVersion.Version.ToString();

			request.ccAuthService     = new CyberSource.CCAuthService();
			request.ccAuthService.run = "true";

			CyberSource.BillTo billTo = new CyberSource.BillTo();
			billTo.firstName          = "John";
			billTo.lastName           = "Doe";
			billTo.street1            = "1295 Charleston Road";
			billTo.city               = "Mountain View";
			billTo.state              = "CA";
			billTo.postalCode         = "94043";
			billTo.country            = "US";
			billTo.email              = "null@cybersource.com";
			billTo.ipAddress          = "10.7.111.111";
			request.billTo            = billTo;

			CyberSource.Card card     = new CyberSource.Card();
			card.accountNumber        = "4111111111111111";
			card.expirationMonth      = "12";
			card.expirationYear       = (System.DateTime.Now.Year+1).ToString();
			request.card              = card;

			CyberSource.PurchaseTotals purchaseTotals = new CyberSource.PurchaseTotals();
			purchaseTotals.currency   = "USD";
			request.purchaseTotals    = purchaseTotals;

			request.item              = new CyberSource.Item[2];

			CyberSource.Item item     = new CyberSource.Item();
			item.id                   = "0";
			item.unitPrice            = "12.34";
			request.item[0]           = item;

			item                      = new CyberSource.Item();
			item.id                   = "1";
			item.unitPrice            = "56.78";
			request.item[1]           = item;

			try
			{
				CyberSource.TransactionProcessor proc = new CyberSource.TransactionProcessor();

//	proc.ChannelFactory does not exist in the API.
//	How do I define the merchant id and key/password?

//				proc.ChannelFactory.Credentials.UserName.UserName = request.merchantID;
//				proc.ChannelFactory.Credentials.UserName.Password = "1b5e6b316fd0e4a9885a354523f958fd78ef9b8c";

				CyberSource.ReplyMessage reply = proc.runTransaction(request);

				Tools.LogInfo("TestTransactionV1/5","decision = " + reply.decision
				                                + ", reasonCode = " + reply.reasonCode
				                                + ", requestID = " + reply.requestID
				                                + ", requestToken = " + reply.requestToken
				                                + ", ccAuthReply.reasonCode = " + reply.ccAuthReply.reasonCode,244);
			}
			catch (TimeoutException ex)
			{
				Tools.LogException("TestTransactionV1/10","TimeoutException",ex,this);
			}
			catch (FaultException ex)
			{
				Tools.LogException("TestTransactionV1/15","FaultException",ex,this);
			}
			catch (CommunicationException ex)
			{
				Tools.LogException("TestTransactionV1/20","CommunicationException",ex,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("TestTransactionV1/25","Exception",ex,this);
			}
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			ret                 = 10;

//	Testing [Start]
//			string    url       = "https://testsecureacceptance.cybersource.com/silent/embedded/pay";
//			string    profileId = "3C857FA4-ED86-4A08-A119-24170A74C760";
//			string    accessKey = "8b031c20a1ad343c97afe1869e2e7994";
//			string    secretKey = "2ea6c71fa7e04304a78f417c1e4d95677abb9673c7cd45ec803a08696041ea62b5eae10527704f4580ae8da223c295c0b42f97808adf4b6db1a2bf032eb74bd7376d9d1393f1443aaf8bcba7cd4d1148b3157119169c404fa74be9e4cd5cf9cacc34f76976f54bfa93136e4de6b1f53750a5e9d4b1cc4fcebd67a14bbcc156c3";
//	Testing [End]

			string    url       = payment.ProviderURL;
			string    profileId = payment.ProviderAccount;
			string    accessKey = payment.ProviderUserID;
			string    secretKey = payment.ProviderKey;
			string    ccNo      = payment.CardNumber;
			string    ccType    = CardType(payment.CardNumber);
			DateTime  dt        = DateTime.Now.ToUniversalTime();
			string[,] fieldS;
			string[,] fieldU;

			try
			{
				string sigX      = "";
				string sigF      = "";
				string sigS      = "";
				string sigU      = "";
				string unsForm   = "";
				string unsSent   = "";
				string urlReturn = "";

				if ( postBackURL == null )
					urlReturn = Tools.ConfigValue("SystemURL");
				else
					urlReturn = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! urlReturn.EndsWith("/") )
					urlReturn = urlReturn + "/";
				ret       = 20;
				urlReturn = urlReturn + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				                      +                    "&TransRef="+Tools.XMLSafe(payment.MerchantReference);

				if ( payment.TokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
					ccNo = "{{{" + payment.CardToken + "}}}";

				d3Form = "";
				ret    = 30;
				fieldS = new string[,] { { "reference_number"                   , Tools.JSONSafe(payment.MerchantReference) }
				                       , { "transaction_type"                   , "sale,create_payment_token" }
				                       , { "currency"                           , "ZAR" }
				                       , { "amount"                             , "1.00" } // "1.49" }
				                       , { "locale"                             , "en" }
				                       , { "profile_id"                         , profileId }
				                       , { "access_key"                         , accessKey }
				                       , { "transaction_uuid"                   , System.Guid.NewGuid().ToString() } // MUST be unique
				                       , { "signed_date_time"                   , dt.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") } // "2021-01-10T03:16:54Z"
				                       , { "payment_method"                     , "card" }
				                       , { "card_cvn"                           , Tools.JSONSafe(payment.CardCVV) }
				                       , { "bill_to_forename"                   , Tools.JSONSafe(payment.CardNameSplit(1)) }
				                       , { "bill_to_surname"                    , Tools.JSONSafe(payment.CardNameSplit(2)) }
				                       , { "bill_to_email"                      , Tools.JSONSafe(payment.EMail,1) }
				                       , { "bill_to_phone"                      , Tools.JSONSafe(payment.PhoneCell) }
				                       , { "bill_to_address_line1"              , Tools.JSONSafe(payment.Address1(65)) }
				                       , { "bill_to_address_city"               , Tools.JSONSafe(payment.Address2(65)) }
				                       , { "bill_to_address_postal_code"        , Tools.JSONSafe(payment.PostalCode(65)) }
				                       , { "bill_to_address_country"            , "ZA" }
				                       , { "payer_authentication_challenge_code", "03" } // Force 3d Secure
				                       , { "payer_authentication_merchant_name" , "CareAssist" }
				                       , { "override_custom_cancel_page"        , urlReturn }
				                       , { "override_custom_receipt_page"       , urlReturn } };
				ret    = 40;
				fieldU = new string[,] { { "card_type"                          , ccType }
				                       , { "card_number"                        , ccNo }
				                       , { "card_expiry_date"                   , payment.CardExpiryMM+"-"+payment.CardExpiryYYYY } };

				ret     = 50;
				xmlSent = "";

				for ( int k = 0 ; k < fieldS.GetLength(0) ; k++ )
				{
					ret     = 60;
					xmlSent = xmlSent + "&" + fieldS[k,0] + "=" + Tools.URLString(fieldS[k,1]);
					sigX    = sigX + "," + fieldS[k,0] + "=" + fieldS[k,1];
					sigS    = sigS + "," + fieldS[k,0];
				}

				for ( int k = 0 ; k < fieldU.GetLength(0) ; k++ )
				{
					ret     = 70;
					unsSent = unsSent + "&" + fieldU[k,0] + "=" + Tools.URLString(fieldU[k,1]);
					unsForm = unsForm + "<input type='hidden' id='" + fieldU[k,0] + "' name='" + fieldU[k,0] + "' value='" + fieldU[k,1] + "' />" + Environment.NewLine;
					sigU    = sigU + "," + fieldU[k,0];
				}

				ret     = 80;
				sigU    = sigU.Substring(1);
				sigS    = sigS.Substring(1) + ",signed_field_names,unsigned_field_names";
				sigX    = sigX.Substring(1) + ",signed_field_names=" + sigS + ",unsigned_field_names=" + sigU;
				xmlSent = xmlSent.Substring(1);
				sigF    = GenerateSignatureV2(sigX,secretKey);
				ret     = 100;
				xmlSent = xmlSent + "&signed_field_names="   + Tools.URLString(sigS)
				                  + "&unsigned_field_names=" + Tools.URLString(sigU)
				                  + unsSent
				                  + "&signature="            + Tools.URLString(sigF);

				Tools.LogInfo("ThreeDSecurePayment/10","Profile Id="+profileId
//				                                   + ", Access Key="+accessKey
//				                                   + ", Secret Key="+secretKey
				                                   + ", Signature Input="+sigX
				                                   + ", Signature Output="+sigF
				                                   + ", URL params="+xmlSent,222,this);

				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
				ret                       = 110;
				webRequest.Method         = "POST";
				webRequest.ContentType    = "application/x-www-form-urlencoded";
				webRequest.Accept         = "application/x-www-form-urlencoded";
				strResult                 = "";
				byte[] page               = Encoding.UTF8.GetBytes(xmlSent);

				ret = 120;
				using (Stream stream = webRequest.GetRequestStream())
				{
					ret = 130;
					stream.Write(page, 0, page.Length);
//					stream.Flush();
					stream.Close();
				}

				ret = 140;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 150;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 160;
						strResult = rd.ReadToEnd();
					}
				}

				Tools.LogInfo("ThreeDSecurePayment/170","strResult=" + strResult,222,this);
				ret = 170;

				if ( strResult.ToUpper().Contains("<HTML") && strResult.ToUpper().Contains("<FORM") && strResult.ToUpper().Contains("NAME=\"TERMURL") )
				{
					ret        = 180;
					d3Form     = strResult;

//					string sql = "exec sp_WP_PaymentRegister3DSecA @ContractCode="    + Tools.DBString(payment.MerchantReference)
//				              +                                 ",@ReferenceNumber=" + Tools.DBString(WhatShouldIPutHere)
//				              +                                 ",@Status='77'"; // Means payment pending
//					using (MiscList mList = new MiscList())
//						mList.ExecQuery(sql,0,"",false,true);
//					Tools.LogInfo("ThreeDSecurePayment/180","PayRef=" + payRef + "; SQL=" + sql + "; " + d3Form,10,this);

					return 0;
				}

				ret        = 190;
				resultCode = Tools.HTMLValue(strResult,"reason_code");
				resultMsg  = Tools.HTMLValue(strResult,"invalid_fields");
				resultMsg  = Tools.HTMLValue(strResult,"message") + ( resultMsg.Length > 0 ? " (" + resultMsg + ")" : "" );
				ret        = 210;
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".ThreeDSecurePayment/298","ret="+ret.ToString());
			}
			catch (Exception ex2)
			{
				Tools.LogException("ThreeDSecurePayment/299","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex2,this);
			}
			return ret;
		}

		public TransactionCyberSource() : base()
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(Constants.PaymentProvider.CyberSource);
		}

		public TransactionCyberSource(Constants.PaymentProvider provider) : base()
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(provider);
		}

//		public TransactionCyberSource(string provider) : base()
//		{
//			ServicePointManager.Expect100Continue = true;
//			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
//			base.LoadBureauDetails((Constants.PaymentProvider)Tools.StringToInt(provider));
//		}
	}
}
