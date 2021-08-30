using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionTokenEx : Transaction
	{
		TransactionPeach tranPeach;

		public bool Successful
		{
			get
			{
				return ( Tools.NullToString(resultCode).ToUpper() == "TRUE" );
			}
		}

		private int PeachHTML(byte transactionType,Payment payment)
		{
			int    ret  = 10;
			string pURL = "https://test.oppwa.com/v1/registrations";
//			string tURL = "https://test-api.tokenex.com/TransparentGatewayAPI/Detokenize";
			string tURL = BureauURL;
			strResult   = "";
			payRef      = "";
			resultCode  = "999.999.999";
			resultMsg   = "(999.999.999) Internal error";

			try
			{
				if ( payment.ProviderURL.Length > 0 )  // The PAYMENT provider (Peach)
					pURL = payment.ProviderURL;
				if ( payment.TokenizerURL.Length > 0 ) // The TOKENIZER (TokenEx)
					tURL = payment.TokenizerURL;
				if ( tranPeach == null )
					tranPeach = new TransactionPeach();

				tURL = tURL + "/TransparentGatewayAPI/Detokenize";

				Tools.LogInfo("PeachHTML/10","URL=" + pURL + ", URL data=" + xmlSent,221,this);

				ret                              = 20;
				byte[]         buffer            = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request           = (HttpWebRequest)HttpWebRequest.Create(tURL);
				ret                              = 30;
				request.Method                   = "POST";
				request.Headers["Authorization"] = "Bearer " + payment.ProviderKey;
				request.Headers["TX_URL"]        = pURL;
				request.Headers["TX_TokenExID"]  = payment.TokenizerID;  // "4311038889209736";
				request.Headers["TX_APIKey"]     = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
				request.ContentType              = "application/x-www-form-urlencoded";
				ret                              = 40;
				Stream postData                  = request.GetRequestStream();
				ret                              = 50;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                     = 60;
					Stream       dataStream = response.GetResponseStream();
					ret                     = 70;
					StreamReader reader     = new StreamReader(dataStream);
					ret                     = 80;
					strResult               = reader.ReadToEnd();
					ret                     = 90;
//					var s       = new JavaScriptSerializer();
//					xmlReceived = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
					reader.Close();
					dataStream.Close();
					ret                  = 100;
					resultCode           = Tools.JSONValue(strResult,"code");
					resultMsg            = Tools.JSONValue(strResult,"description");
					ret                  = 110;
					tranPeach.ResultCode = resultCode;
					if ( tranPeach.Successful )
						ret = 0;
					else
						Tools.LogInfo("PeachHTML/110","resultCode="+resultCode+", resultMsg="+resultMsg,221,this);
				}
			}
			catch (WebException ex1)
			{
				resultCode = ex1.Response.Headers["tx_code"];
				resultMsg  = ex1.Response.Headers["tx_message"];
				Tools.DecodeWebException(ex1,ClassName+".PeachHTML/197",xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("PeachHTML/198","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,255,this);
				Tools.LogException("PeachHTML/199","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,ex2,this);
			}
			return ret;
		}

		private int PostJSON(string url,Payment payment)
		{
			int ret = 10;

			try
			{
				if ( url.Length < 1 && payment.ProviderURL.Length > 0 )
					url = payment.ProviderURL;

//				Tools.LogInfo("PostJSON/10","Post="+xmlSent+", Key="+payment.ProviderKey+", URL="+url,10,this);

				ret                     = 20;
				byte[]         buffer   = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request  = (HttpWebRequest)HttpWebRequest.Create(url);
				ret                     = 30;
				request.Method          = "POST";
				request.ContentType     = "application/json";
				ret                     = 40;
				Stream postData         = request.GetRequestStream();
				ret                     = 50;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                  = 60;
					Stream       dStream = response.GetResponseStream();
					ret                  = 70;
					StreamReader sReader = new StreamReader(dStream);
					ret                  = 80;
					strResult            = sReader.ReadToEnd();
					ret                  = 90;
					sReader.Close();
					dStream.Close();
					sReader              = null;
					ret                  = 100;
					resultCode           = Tools.JSONValue(strResult,"Success");
					ret                  = 130;
					resultMsg            = Tools.JSONValue(strResult,"Error");
					ret                  = 150;
					payRef               = Tools.JSONValue(strResult,"ReferenceNumber");

					if ( Successful && strResult.Length > 0 )
						return 0;

					ret = 170;
					Tools.LogInfo("PostJSON/170","URL=" + url + " | XML Sent=" + xmlSent + " | Result=" + strResult,221,this);
//					Tools.LogInfo("PostJSON/170","resultCode="+resultCode+", resultMsg="+resultMsg,221,this);
				}
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".PostJSON/197","Ret="+ret.ToString()+" | " + xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogException("PostJSON/199","Ret="+ret.ToString()+" | " + xmlSent,ex2,this);
			}
			return ret;
		}

		private int PostXML(string url,Payment payment)
		{
			int ret = 10;

			try
			{
				xmlSent = "<TokenAction>"
				        + Tools.XMLCell("APIKey",payment.ProviderKey)
				        + Tools.XMLCell("TokenExID",payment.ProviderUserID)
				        + xmlSent
				        + "</TokenAction>";

				if ( payment.ProviderURL.Length > 0 )
					url = payment.ProviderURL;

				Tools.LogInfo("PostXML/10","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret                     = 20;
				byte[]         buffer   = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request  = (HttpWebRequest)HttpWebRequest.Create(url);
				ret                     = 30;
				request.Method          = "POST";
				request.ContentType     = "text/xml"; // ;charset=\"utf-8\"";
				ret                     = 40;
				Stream postData         = request.GetRequestStream();
				ret                     = 50;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                  = 60;
					Stream       dStream = response.GetResponseStream();
					ret                  = 70;
					StreamReader sReader = new StreamReader(dStream);
					ret                  = 80;
					strResult            = sReader.ReadToEnd();
					ret                  = 90;
					sReader.Close();
					dStream.Close();
					sReader              = null;
					ret                  = 100;
					if ( xmlResult == null )
						xmlResult         = new XmlDocument();
					else
						xmlResult.RemoveAll();
					ret                  = 110;
					xmlResult.LoadXml(strResult);
					ret                  = 120;
					resultCode           = Tools.XMLNode(xmlResult,"Success");
					ret                  = 130;
					resultMsg            = Tools.XMLNode(xmlResult,"Error");
					ret                  = 140;
					payRef               = Tools.XMLNode(xmlResult,"ReferenceNumber");
					if ( Successful )
						ret = 0;
					else
						Tools.LogInfo("PostXML/110","resultCode="+resultCode+", resultMsg="+resultMsg,221,this);
				}
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".PostXML/197",xmlSent);
			}
			catch (Exception ex2)
			{
				Tools.LogException("PostXML/199","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex2,this);
			}
			return ret;
		}

		public override int DeleteToken(Payment payment)
		{
			xmlSent = Tools.XMLCell("Token",payment.CardToken);
			return PostXML(BureauURL + "/TokenServices.svc/REST/DeleteToken",payment);
		//	return PostXML("https://test-api.tokenex.com/TokenServices.svc/REST/DeleteToken",payment);
		}

		public int GetTokenCardAndCVV(Payment payment)
		{
			string txScheme  = "sixTOKENfour";
			string timeStamp = Tools.DateToString(System.DateTime.Now,5,2).Replace(" ","");
			string payLoad   = payment.ProviderUserID + "|" + timeStamp + "|" + txScheme;
			string authKey   = Tools.GenerateHMAC(payLoad,payment.ProviderKey);
			string url       = "https://" + ( Tools.SystemIsLive() ? "" : "test-" ) + "htp.tokenex.com/api/sdk/TokenizeWithCVV";
			xmlSent          = Tools.JSONPair("tokenexid",        payment.ProviderUserID,1,"{")
			                 + Tools.JSONPair("timestamp",        timeStamp)
			                 + Tools.JSONPair("authenticationKey",authKey)
			                 + Tools.JSONPair("data",             payment.CardNumber)
			                 + Tools.JSONPair("tokenScheme",      txScheme)
			                 + Tools.JSONPair("cvv",              payment.CardCVV,1,"","}");
			payToken         = "";
			int ret          = PostJSON(url,payment);
			if ( ret == 0 )
				payToken = Tools.JSONValue(strResult,"Token");
			return ret;
		}

		public override int Detokenize(Payment payment)
		{
//	Live
			xmlSent    = Tools.JSONPair("APIKey"   , payment.ProviderKey,    0, "{")
			           + Tools.JSONPair("TokenExID", payment.ProviderUserID, 0)
			           + Tools.JSONPair("Token"    , payment.CardToken,      0, "", "}");

// Test, v1
//			xmlSent    = Tools.JSONPair("APIKey"   , "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi" ,0, "{")
//			           + Tools.JSONPair("TokenExID", "4311038889209736", 0)
//			           + Tools.JSONPair("Token"    , payment.CardToken, 0, "", "}");

			cardNumber = "";
			int  ret   = PostJSON(BureauURL + "/TokenServices.svc/REST/Detokenize",payment);
			if ( ret  == 0 && strResult.ToUpper().Contains("VALUE") ) // Success
				cardNumber = Tools.JSONValue(strResult,"Value");
			else if ( ret == 0 )
				ret = 817;
			return ret;
		}

		public int DetokenizeV2(Payment payment)
		{
			int ret = 10;
			xmlSent = "BureauCode="     + Tools.URLString(payment.BureauCode)
			        + "&ContractCode="  + Tools.URLString(payment.MerchantReference)
			        + "&Token="         + Tools.URLString(payment.CardToken)
			        + "&CardNumber={{{" + Tools.URLString(payment.CardToken) + "}}}";

			try
			{
				string         tURL            = "https://test-api.tokenex.com/TransparentGatewayAPI/Detokenize";
//				string         tURL            = payment.TokenizerURL + "/TransparentGatewayAPI/Detokenize";
				byte[]         buffer          = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webReq          = (HttpWebRequest)HttpWebRequest.Create(tURL);
//				webReq.Headers["TX_URL"]       = "https://www.eservsecureafrica.com/Detokenize.aspx";
//				webReq.Headers["TX_URL"]       = "https://www.eservsecure.com/UIApplicationQuery.aspx?QueryName=Detokenize";
				webReq.Headers["TX_URL"]       = "https://www.eservsecureafrica.com/RTR.aspx";
//				webReq.Headers["TX_TokenExID"] = payment.TokenizerID;  // "4311038889209736";
//				webReq.Headers["TX_APIKey"]    = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
				webReq.Headers["TX_TokenExID"] = "4311038889209736";
				webReq.Headers["TX_APIKey"]    = "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
				webReq.Method                  = "POST";
				webReq.ContentType             = "application/x-www-form-urlencoded";
				ret                            = 50;

				using (Stream postData = webReq.GetRequestStream())
				{
					ret = 70;
					postData.Write(buffer, 0, buffer.Length);
					postData.Close();
				}

				ret = 80;

				using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
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
				strResult = Tools.DecodeWebException(ex1,ClassName+".DetokenizeV2/197",xmlSent);
			}
			catch (Exception ex2)
			{
				if ( strResult == null )
					strResult = "";
				Tools.LogInfo     ("DetokenizeV2/198","Ret="+ret.ToString()+", Result="+strResult,222,this);
				Tools.LogException("DetokenizeV2/199","Ret="+ret.ToString()+", Result="+strResult,ex2,this);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			payToken = "";
			xmlSent  = Tools.XMLCell("Data",payment.CardNumber)
			         + Tools.XMLCell("TokenScheme","sixTOKENfour");
			int ret  = PostXML(BureauURL + "/TokenServices.svc/REST/Tokenize",payment);
			if ( ret == 0 )
				payToken = Tools.XMLNode(xmlResult,"Token");
			return ret;
		}

		public override int CardPayment(Payment payment)
		{
//			For Peach Payments

			int ret = 10;

			if ( payment.CardToken.Length < 6 )
			{
				Tools.LogInfo("CardPayment/10","Invalid token (" + payment.CardToken.Length + "), reference " + payment.MerchantReference,222,this);
				return ret;
			}

			try
			{
				xmlSent = "entityId="               + Tools.URLString(payment.ProviderUserID)
				        + "&paymentBrand="          + Tools.URLString(payment.CardType.ToUpper())
				        + "&card.number={{{"        + Tools.URLString(payment.CardToken) + "}}}"
				        + "&card.holder="           + Tools.URLString(payment.CardName)
				        + "&card.expiryMonth="      + Tools.URLString(payment.CardExpiryMM)
				        + "&card.expiryYear="       + Tools.URLString(payment.CardExpiryYYYY)
				        + "&card.cvv="              + Tools.URLString(payment.CardCVV)
				        + "&amount="                + Tools.URLString(payment.PaymentAmountDecimal)
				        + "&currency="              + Tools.URLString(payment.CurrencyCode)
				        + "&merchantTransactionId=" + Tools.URLString(payment.MerchantReference)
				        + "&descriptor="            + Tools.URLString(payment.PaymentDescription)
				        + "&paymentType=DB" // DB = Instant, PA = Pre-authorize
				        + "&recurringType=REPEATED";
//				        + "&card.cvv={{{cvv}}}"

				Tools.LogInfo("CardPayment/20","Post="+xmlSent+", Key="+payment.ProviderKey,10,this);

				ret    = PeachHTML((byte)Constants.TransactionType.CardPayment,payment);
				payRef = Tools.JSONValue(strResult,"id");
//				payRef = Tools.JSONValue(strResult,"ndc");
				if ( payRef.Length < 1 && ret == 0 )
					ret = 248;

				Tools.LogInfo("CardPayment/30","ResultCode="+ResultCode + ", payRef=" + payRef,221,this);
			}
			catch (Exception ex)
			{
				Tools.LogException("CardPayment/99","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			return CardPayment(payment);
		}

      public override void Close()
		{
			tranPeach = null;
			base.Close();
		}

		public TransactionTokenEx() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.TokenEx);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
		}
	}
}
