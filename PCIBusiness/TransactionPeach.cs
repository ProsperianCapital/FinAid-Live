using System;
using System.Text;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionPeach : Transaction
	{
		string currency;
		string amount;
//		string cardNumber;
		string cardHolder;

		public string   Currency
		{
			get { return Tools.NullToString(currency); }
		}
		public string   Amount
		{
			get { return Tools.NullToString(amount); }
		}
//		public string   CardNumber
//		{
//			get { return Tools.NullToString(cardNumber); }
//		}
		public string   CardHolder
		{
			get { return Tools.NullToString(cardHolder); }
		}

//		public string   ThreeDSecureHTML
//		{
//			get { return Tools.NullToString(d3Form); }
//		}

		public  bool   Successful
		{
			get
			{
			//	Always in 999.999.999 format
				string okList = "|000.000.000|"
				              +  "000.100.110|"
				              +  "000.100.111|"
				              +  "000.100.112|"
				              +  "000.200.000|"
				              +  "000.300.000|"
				              +  "000.400.000|"
				              +  "000.400.010|"
				              +  "000.400.020|"
				              +  "000.400.040|"
				              +  "000.400.060|"
				              +  "000.400.090|";
				resultCode    = Tools.NullToString(resultCode).ToUpper();
				if ( resultCode.Length == 11 && okList.Contains("|"+resultCode+"|") )
					return true;

//				if ( ! resultCode.StartsWith("000") )
//					return false;
//				if ( ! resultCode.StartsWith("000.400") )
//					return true;
//				if ( resultCode.CompareTo("000.400.101") < 0 ) // 000.400.000 - 000.400.100 is OK, 101+ is an error
//					return true;

				return false;
			}
		}

		private int PostHTML(byte transactionType,Payment payment)
		{
			byte   err = 0;
			int    ret = 10;
			string url = BureauURL;
			strResult  = "";
			payRef     = "";
			resultCode = "999.999.888";
			resultMsg  = "(999.999.888) Internal error";

			try
			{
				if ( payment.ProviderURL.Length > 0 )
					url = payment.ProviderURL;
				if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
					url = url + "/" + payment.CardToken + "/payments";
				else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
					url = url + "/threeDSecure";
				//	url = url + "/payments";

//	Note:
// Use endpoint "threeDSecure" if you simply want to check a card via 3d secure and don't want the amount going through
// Use endpoint "payments" if the amount must actually go through
//	Only need "paymentType=DB" if it is an actual payment

				if ( url.Contains("/payments") && ! xmlSent.Contains("paymentType=") )
					xmlSent = xmlSent.Replace("&amount=","&paymentType=DB&amount=");

				Tools.LogInfo("PostHTML/10","URL=" + url + ", URL data=" + xmlSent,10,this);

				ret                   = 20;
				byte[]         buffer = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request;

				if ( payment.TokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
				{
					ret                             = 30;
					request                         = (HttpWebRequest)HttpWebRequest.Create(payment.TokenizerURL+"/TransparentGatewayAPI/Detokenize");
					request.Headers["TX_URL"]       = url;
					request.Headers["TX_TokenExID"] = payment.TokenizerID;
					request.Headers["TX_APIKey"]    = payment.TokenizerKey;
				}
				else
					request                         = (HttpWebRequest)HttpWebRequest.Create(url);

				ret                                = 50;
				request.Method                     = "POST";
				request.Headers["Authorization"]   = "Bearer " + payment.ProviderKey;
				request.ContentType                = "application/x-www-form-urlencoded";
				ret                                = 60;

				using (Stream postData = request.GetRequestStream())
				{
					ret = 70;
					postData.Write(buffer, 0, buffer.Length);
					postData.Close();
				}

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                     = 100;
					Stream       dataStream = response.GetResponseStream();
					ret                     = 110;
					StreamReader reader     = new StreamReader(dataStream);
					ret                     = 120;
					strResult               = reader.ReadToEnd();
					ret                     = 130;
					reader.Close();
					dataStream.Close();
				}
			}
			catch (WebException ex1)
			{
				err = 1;
				strResult = Tools.DecodeWebException(ex1,ClassName+".PostHTML/197",xmlSent);
			}
			catch (Exception ex2)
			{
				err = 2;
				if ( strResult == null )
					strResult = "";
				Tools.LogInfo     ("PostHTML/198","Ret="+ret.ToString()+", Result="+strResult,222,this);
				Tools.LogException("PostHTML/199","Ret="+ret.ToString()+", Result="+strResult,ex2,this);
			}

			ret        = 200;
			resultCode = Tools.JSONValue(strResult,"code");
			resultMsg  = Tools.JSONValue(strResult,"description");
			ret        = 210;
			if ( Successful && err == 0 )
				ret     = 0;
			else
				Tools.LogInfo("PostHTML/220","resultCode="+resultCode+", resultMsg="+resultMsg,221,this);

			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int ret = 10;

			try
			{
				xmlSent = "entityId="          + Tools.URLString(payment.ProviderUserID)
				        + "&paymentBrand="     + Tools.URLString(payment.CardType.ToUpper())
				        + "&card.number="      + Tools.URLString(payment.CardNumber)
				        + "&card.holder="      + Tools.URLString(payment.CardName)
				        + "&card.expiryMonth=" + Tools.URLString(payment.CardExpiryMM)
				        + "&card.expiryYear="  + Tools.URLString(payment.CardExpiryYYYY)
				        + "&card.cvv="         + Tools.URLString(payment.CardCVV);
				if ( payment.CardType.ToUpper().StartsWith("DINE") ) // Diners Club
					xmlSent = xmlSent + "&shopperResultUrl=https://peachpayments.docs.oppwa.com/server-to-server";

				Tools.LogInfo("GetToken/10","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret      = PostHTML((byte)Constants.TransactionType.GetToken,payment);
				payToken = Tools.JSONValue(strResult,"id");
				payRef   = Tools.JSONValue(strResult,"ndc");
				if ( payToken.Length < 1 && ret == 0 )
					ret = 247;

				if ( ret > 0 )
					Tools.LogInfo("GetToken/20","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("GetToken/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		private void SetUpPaymentXML(Payment payment,byte transactionType)
		{
			if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				xmlSent = "{{{" + Tools.URLString(payment.CardToken) + "}}}";
			else
				xmlSent = Tools.URLString(payment.CardNumber);

			xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
			        + "&paymentBrand="          + Tools.URLString(payment.CardType.ToUpper())
			        + "&paymentType=DB"         // DB = Instant debit, PA = Pre-authorize, CP =
			        + "&recurringType=REPEATED"
			        + "&card.number="           + xmlSent
			        + "&card.holder="           + Tools.URLString(payment.CardName)
			        + "&card.expiryMonth="      + Tools.URLString(payment.CardExpiryMM)
			        + "&card.expiryYear="       + Tools.URLString(payment.CardExpiryYYYY)
			        + "&card.cvv="              + Tools.URLString(payment.CardCVV)
			        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
			        + "&currency="              + Tools.URLString(payment.CurrencyCode)
			        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
			        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
			        + "&merchantInvoiceId="     + Tools.URLString(payment.MerchantReference)
			        + "&shopperResultUrl="      + Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef="+Tools.XMLSafe(payment.MerchantReference);
		}

		public override int CardPayment3rdParty(Payment payment)
		{
			byte   err  = 0;
			int    ret  = 10;
			string pURL = "";
			string tURL = "";
			strResult   = "";
			payRef      = "";
			resultCode  = "999.999.777";
			resultMsg   = "(999.999.777) Internal error";

			try
			{
				if ( payment.ProviderURL.Length > 0 ) // The PAYMENT provider (Peach)
					pURL = payment.ProviderURL;
				else
					pURL = BureauURL;

				if ( pURL.ToUpper().EndsWith("REGISTRATIONS") || pURL.ToUpper().EndsWith("PAYMENTS") )
					ret  = 15;
				else
					pURL = BureauURL + "/payments";
//					pURL = BureauURL + "/registrations";

				ret  = 20;
				tURL = payment.TokenizerURL; // The TOKENIZER/THIRD PARTY (TokenEx)

				if ( tURL.Length < 1 )
				{
					Tools.LogInfo("CardPayment3rdParty/20","Unknown Third Party Tokenizer (" + bureauCodeTokenizer + "), data=" + xmlSent,221,this);
					return ret;
				}
				if ( ! tURL.ToUpper().EndsWith("DETOKENIZE") )
					tURL = tURL + "/TransparentGatewayAPI/Detokenize";

				ret = 30;
				SetUpPaymentXML(payment,(byte)Constants.TransactionType.CardPaymentThirdParty);

				Tools.LogInfo("CardPayment3rdParty/30","pURL=" + pURL + ", tURL=" + tURL + ", data=" + xmlSent,10,this);

				ret                              = 40;
				byte[]         buffer            = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request           = (HttpWebRequest)HttpWebRequest.Create(tURL);
				ret                              = 50;
				request.Method                   = "POST";
				request.Headers["Authorization"] = "Bearer " + payment.ProviderKey;
				request.Headers["TX_URL"]        = pURL;
				request.Headers["TX_TokenExID"]  = payment.TokenizerID;  // "4311038889209736";
				request.Headers["TX_APIKey"]     = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
				request.ContentType              = "application/x-www-form-urlencoded";
				ret                              = 70;
				Stream postData                  = request.GetRequestStream();
				ret                              = 80;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                     = 100;
					Stream       dataStream = response.GetResponseStream();
					ret                     = 110;
					StreamReader reader     = new StreamReader(dataStream);
					ret                     = 120;
					strResult               = reader.ReadToEnd();
					ret                     = 130;
//					var s       = new JavaScriptSerializer();
//					xmlReceived = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
					reader.Close();
					dataStream.Close();
				}
			}

			catch (WebException ex1)
			{
				err       = 1;
				strResult = Tools.DecodeWebException(ex1,ClassName+".CardPayment3rdParty/997",xmlSent);
			}

			catch (Exception ex2)
			{
				err = 2;
				if ( strResult == null )
					strResult = "";
				Tools.LogInfo     ("CardPayment3rdParty/998","Ret="+ret.ToString()+", Result="+strResult,222,this);
				Tools.LogException("CardPayment3rdParty/999","Ret="+ret.ToString()+", Result="+strResult,ex2,this);
			}

			ret        = 200;
			resultCode = Tools.JSONValue(strResult,"code");
			resultMsg  = Tools.JSONValue(strResult,"description");
			payRef     = Tools.JSONValue(strResult,"id");
			ret        = 210;
			if ( Successful && err == 0 )
			{
				ret = 0;
				Tools.LogInfo("CardPayment3rdParty/220","(Succeed) Result="+strResult,10,this);
			}
			else
				Tools.LogInfo("CardPayment3rdParty/230","(Fail/" + err.ToString() + ") Result="+strResult,221,this);

			return ret;
		}

		public override int CardPayment(Payment payment)
		{
			int ret = 10;

			try
			{
// v1
//				xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
//				        + "&paymentBrand="          + Tools.URLString(payment.CardType.ToUpper())
//				        + "&card.number="           + Tools.URLString(payment.CardNumber)
//				        + "&card.holder="           + Tools.URLString(payment.CardName)
//				        + "&card.expiryMonth="      + Tools.URLString(payment.CardExpiryMM)
//				        + "&card.expiryYear="       + Tools.URLString(payment.CardExpiryYYYY)
//				        + "&card.cvv="              + Tools.URLString(payment.CardCVV)
//				        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
//				        + "&currency="              + Tools.URLString(payment.CurrencyCode)
//				        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
//				        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
//				        + "&merchantInvoiceId="     + Tools.URLString(payment.MerchantReference)
//				        + "&merchant.name=Prosperian"
//				        + "&paymentType=DB" // DB = Instant debit, PA = Pre-authorize, CP =
//				        + "&recurringType=REPEATED";
//	//			        + "&merchant.city=[merchant.city]abcdefghijklmnopqrstuvwxyz"

//	v2
//				xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
//				        + "&paymentBrand="          + Tools.URLString(payment.CardType.ToUpper())
//				        + "&card.number="           + Tools.URLString(payment.CardNumber)
//				        + "&card.holder="           + Tools.URLString(payment.CardName)
//				        + "&card.expiryMonth="      + Tools.URLString(payment.CardExpiryMM)
//				        + "&card.expiryYear="       + Tools.URLString(payment.CardExpiryYYYY)
//				        + "&card.cvv="              + Tools.URLString(payment.CardCVV)
//				        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
//				        + "&currency="              + Tools.URLString(payment.CurrencyCode)
//				        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
//				        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
//				        + "&merchantInvoiceId="     + Tools.URLString(payment.MerchantReference)
//				        + "&merchant.name=Prosperian"
//				        + "&recurringType=REPEATED"
//				        + "&paymentType=DB"
//				        + "&shopperResultUrl="      + Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef="+Tools.XMLSafe(payment.MerchantReference);

				SetUpPaymentXML(payment,(byte)Constants.TransactionType.CardPayment);

				Tools.LogInfo("CardPayment/10","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret      = PostHTML((byte)Constants.TransactionType.GetToken,payment);
//				payToken = Tools.JSONValue(strResult,"id");
//				payRef   = Tools.JSONValue(strResult,"ndc");
				payRef   = Tools.JSONValue(strResult,"id");
				if ( payRef.Length < 1 && ret == 0 )
					ret = 248;

				Tools.LogInfo("CardPayment/20","ResultCode="+ResultCode + ", payRef=" + payRef,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("CardPayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			int ret = 10;

			try
			{
				xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
				        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
				        + "&currency="              + Tools.URLString(payment.CurrencyCode)
				        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
				        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
				        + "&paymentType=DB" // DB = Instant debit, PA = Pre-authorize
				        + "&recurringType=REPEATED";

				Tools.LogInfo("TokenPayment/10","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret    = PostHTML((byte)Constants.TransactionType.TokenPayment,payment);
				payRef = Tools.JSONValue(strResult,"id");
				if ( payRef.Length < 1 && ret == 0 )
					ret = 249;

				if ( ret > 0 )
					Tools.LogInfo("TokenPayment/20","ResultCode="+ResultCode + ", payRef=" + payRef,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("TokenPayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		public override int ThreeDSecureCheck(string providerRef,string merchantRef="")
		{
//	Return
//	   0     : Payment succeeded
//	   1-999 : Payment processed but declined or rejected
// 1001-    : Internal error

			int    ret      = 20010;
			string key      = Tools.ProviderCredentials("Peach","Key");
			string entityId = Tools.ProviderCredentials("Peach","Id","3d");
			strResult       = "";
			resultCode      = "";
			resultMsg       = "Internal failure";

			if ( ! Tools.SystemIsLive() ) // Test
			{
				if ( entityId.Length < 1 )
					entityId = "8ac7a4c772b77ddf0172b7ed1cd206df";
				if ( key.Length < 1 )
					key      = "OGFjN2E0Yzc3MmI3N2RkZjAxNzJiN2VkMDFmODA2YTF8akE0aEVaOG5ZQQ==";
			}

//	Note:
// Use endpoint "threeDSecure" if you simply want to check a card via 3d secure and don't want the amount going through
// Use endpoint "payments" if the amount must actually go through

			try
			{
				string         url               = BureauURL + "/threeDSecure/" + providerRef + "?entityId=" + entityId;
			//	string         url               = BureauURL + "/payments/"     + providerRef + "?entityId=" + entityId;
				HttpWebRequest request           = (HttpWebRequest)HttpWebRequest.Create(url);
				request.Method                   = "GET";
				request.Headers["Authorization"] = "Bearer " + key;
				using ( HttpWebResponse response = (HttpWebResponse)request.GetResponse() )
				{
					ret                     = 20060;
					Stream       dataStream = response.GetResponseStream();
					ret                     = 20070;
					StreamReader reader     = new StreamReader(dataStream);
					ret                     = 20080;
					strResult               = reader.ReadToEnd();
					ret                     = 20090;
					reader.Close();
					dataStream.Close();
					ret                     = 0;
				}
			}
			catch (WebException ex1)
			{
				strResult = Tools.DecodeWebException(ex1,ClassName+".ThreeDSecureCheck/197",xmlSent);
			}
			catch (Exception ex2)
			{
				if ( strResult == null )
					strResult = "";
				Tools.LogInfo     ("ThreeDSecureCheck/198","Ret="+ret.ToString()+", Result="+strResult,222,this);
				Tools.LogException("ThreeDSecureCheck/199","Ret="+ret.ToString()+", Result="+strResult,ex2,this);
			}

			if ( strResult == null )
				strResult = "";

			Tools.LogInfo ("ThreeDSecureCheck/210","Ret="+ret.ToString()+", Result="+strResult,10,this);

			resultCode = Tools.JSONValue(strResult,"code");
			resultMsg  = Tools.JSONValue(strResult,"description");
			currency   = Tools.JSONValue(strResult,"currency");
			amount     = Tools.JSONValue(strResult,"amount");
			cardNumber = Tools.JSONValue(strResult,"bin") + "******" + Tools.JSONValue(strResult,"last4Digits");
			cardHolder = Tools.JSONValue(strResult,"holder");

			if ( ! Successful && ret == 0 )
				ret = 53;
			if ( ! Successful || ret != 0 )
				Tools.LogInfo("ThreeDSecureCheck/220","ret="+ret.ToString()+", resultCode="+resultCode+", resultMsg="+resultMsg,221,this);
			return ret;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret = 10;
			string url = "";

			try
			{
				if ( postBackURL == null )
					url = Tools.ConfigValue("SystemURL");
				else
					url = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! url.EndsWith("/") )
					url = url + "/";

				if ( payment.TokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
					xmlSent = "{{{" + Tools.URLString(payment.CardToken) + "}}}";
				else
					xmlSent = Tools.URLString(payment.CardNumber);

				d3Form  = "";
				xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
				        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
				        + "&currency="              + Tools.URLString(payment.CurrencyCode)
				        + "&card.number="           + xmlSent
				        + "&card.holder="           + Tools.URLString(payment.CardName)
				        + "&card.expiryMonth="      + Tools.URLString(payment.CardExpiryMM)
				        + "&card.expiryYear="       + Tools.URLString(payment.CardExpiryYYYY)
				        + "&card.cvv="              + Tools.URLString(payment.CardCVV)
				        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
				        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
				        + "&shopperResultUrl="      + Tools.URLString(url+"RegisterThreeD.aspx"
				                                    +                    "?ProviderCode="+Tools.BureauCode(Constants.PaymentProvider.Peach)
				                                    +                    "&TransRef="+Tools.XMLSafe(payment.MerchantReference));
//				        + "&paymentType=DB"
//				        + "&shopperResultUrl="      + Tools.URLString(Tools.ConfigValue("SystemURL")+"/RegisterThreeD.aspx?TransRef="+Tools.XMLSafe(payment.MerchantReference));

				Tools.LogInfo("ThreeDSecurePayment/10","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret    = PostHTML((byte)Constants.TransactionType.ThreeDSecurePayment,payment);
				payRef = Tools.JSONValue(strResult,"id");

				if ( ret > 0 )
					Tools.LogInfo("ThreeDSecurePayment/20","strResult="+strResult,222,this);

/*				
//	TESTING
				ret = 0;
				strResult = @"{
									|id|:|8ac7a4a173bdef630173be24d9710ad9|,
									|paymentBrand|:|VISA|,
									|amount|:|12.50|,
									|currency|:|EUR|,
									|result|:{
										|code|:|000.200.000|,
										|description|:|transaction pending|
									},
									|card|:{
										|bin|:|471110|,
										|last4Digits|:|0000|,
										|holder|:|John Smith|,
										|expiryMonth|:|12|,
										|expiryYear|:|2021|
									},
									|threeDSecure|:{
										|eci|:|06|
									},
									|redirect|:{
										|url|:|https://test.ppipe.net/connectors/demo/simulator.link?ndcid=8a8294174e735d0c014e78cf26461790_32b958682e7942e5a045c5b08bbd03ce&REMOTEADDRESS=10.71.36.33|,
										|parameters|:[
										{
											|name|:|PaReq|,
											|value|:|IT8ubu+5z4YupUCOEHKsbiPep8UzIAcPKJEjpwGlzD8#NDcxMTEwMDAwMDAwMDAwMCMxMi41MCBFVVIj|
										},
										{
											|name|:|TermUrl|,
											|value|:|https://test.ppipe.net/connectors/asyncresponse_simulator;jsessionid=12885DB09A3E49DB8B6214E20A7246D7.uat01-vm-con02?asyncsource=THREEDSECURE&ndcid=8a8294174e735d0c014e78cf26461790_32b958682e7942e5a045c5b08bbd03ce|
										},
										{
											|name|:|connector|,
											|value|:|THREEDSECURE|
										},
										{
											|name|:|MD|,
											|value|:|8ac7a4a173bdef630173be24d9710ad9|
										}
										]
									},
									|buildNumber|:|982467e36fd8bc9e74f536ba375c5d0be4fe48eb@2020-07-30 03:42:32 +0000|,
									|timestamp|:|2020-08-05 10:22:32+0000|,
									|ndc|:|8a8294174e735d0c014e78cf26461790_32b958682e7942e5a045c5b08bbd03ce|
									}";
				strResult = strResult.Replace("|","\"");
//	TESTING
*/

				short     k       = 1;
				string[,] d3Parms = new string[100,3];
				string    d3Parm  = Tools.JSONValue(strResult,"parameters","",k);
				string    d3URL   = Tools.JSONValue(strResult,"url");

				while ( d3Parm.Length > 0 && k < 100 )
				{
					d3Parms[k,1] = Tools.JSONValue(d3Parm,"name");
					d3Parms[k,2] = Tools.JSONValue(d3Parm,"value");
					d3Form       = d3Form + "<input type='hidden' name='" + d3Parms[k,1] + "' value='" + d3Parms[k,2] + "' />";
					k++;
					d3Parm       = Tools.JSONValue(strResult,"parameters","",k);
				}

				if ( d3Form.Length > 0 )
				{
				//	Ver 1
				//	d3Form = "<form name='frm3D' action='" + d3URL + "' class='paymentWidgets'>"
				//	       + d3Form
				//	       + "</form>"
				//	       + "<script type='text/javascript'>"
				//	       + "document.frm3D.submit();"
				//	       + "</script>";
				//	Ver 2
					d3Form     = "<html><body onload='document.forms[\"frm3D\"].submit()'>"
					           + "<form name='frm3D' method='POST' action='" + d3URL + "'>"
					           + d3Form
					           + "</form></body></html>";
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
				Tools.LogInfo("ThreeDSecurePayment/60","ResultCode="+ResultCode + ", payRef=" + payRef,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("ThreeDSecurePayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		public TransactionPeach() : base()
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(Constants.PaymentProvider.Peach);
		}
	}
}
