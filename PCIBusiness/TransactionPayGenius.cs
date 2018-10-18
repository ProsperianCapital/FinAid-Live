using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
//	using System.Web.Script.Serialization;

namespace PCIBusiness
{
	public class TransactionPayGenius : Transaction
	{
		public  bool Successful
		{
			get { return Tools.JSONValue(strResult,"success").ToUpper() == "TRUE"; }
		}

		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
				Tools.LogInfo("TransactionPayGenius.GetToken/10","Merchant Ref=" + payment.MerchantReference,199);

				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMM,11)
				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,11,"","}")
				         + "}";
				ret      = 20;
//				ret      = TestService(0); // Dev
//				ret      = TestService(1); // Live
				ret      = CallWebService(payment,"/pg/api/v2/card/register");
				ret      = 30;
				payToken = Tools.JSONValue(XMLResult,"token");
				ret      = 40;
				if ( Successful && payToken.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("TransactionPayGenius.GetToken/50","JSON Sent="+xmlSent+", JSON Received="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGenius.GetToken/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGenius.GetToken/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int ProcessPayment(Payment payment)
		{
			int ret = 10;
			payRef  = "";

			Tools.LogInfo("TransactionPayGenius.ProcessPayment/10","Merchant Ref=" + payment.MerchantReference,199);

			try
			{
				xmlSent = "{ \"creditCard\" : "  + Tools.JSONPair("token"    ,payment.CardToken,1,"{","}")
				        + ", \"transaction\" : " + Tools.JSONPair("reference",payment.MerchantReference,1,"{")
				                                 + Tools.JSONPair("currency" ,payment.CurrencyCode,1)
				                                 + Tools.JSONPair("amount"   ,payment.PaymentAmount.ToString(),11,"","}")
				        + ", "                   + Tools.JSONPair("threeDSecure","false",12,"","")
				        + "}";

				ret     = 20;
				ret     = CallWebService(payment,"/pg/api/v2/payment/create");
				ret     = 30;
				payRef  = Tools.JSONValue(XMLResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;
//				else
//					Tools.LogInfo("TransactionPayGenius.ProcessPayment/50","JSON Sent="+xmlSent+", JSON Received="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGenius.ProcessPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGenius.ProcessPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex);
			}
			return ret;
		}

		private string SetUpXML(Payment payment,byte section)
		{
			return "";
		}

		private int CallWebService(Payment payment,string urlDetail)
      {
			int    ret = 10;
			string url = payment.ProviderURL;

			if ( Tools.NullToString(url).Length == 0 )
				if ( Tools.LiveTestOrDev() == Constants.SystemMode.Live )
					url = "https://www.paygenius.co.za";
				else
					url = "https://developer.paygenius.co.za";

			ret = 20;

			if ( Tools.NullToString(urlDetail).Length > 0 )
			{
				ret = 30;
				if ( url.EndsWith("/") )
					url = url.Substring(0,url.Length-1);
				ret = 40;
				if ( urlDetail.StartsWith("/") )
					urlDetail = urlDetail.Substring(1);
				ret = 50;
				url = url + "/" + urlDetail;
			}

			ret        = 60;
			strResult  = "";
			resultCode = "99";
			resultMsg  = "Internal error connecting to " + url;
			ret        = 70;

			try
			{
				string         sig;
				byte[]         page               = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest         = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType            = "application/json";
				webRequest.Accept                 = "application/json";
				webRequest.Method                 = "POST";
				ret                               = 60;
				webRequest.Headers["X-Token"]     = payment.ProviderKey;
				ret                               = 90;
				sig                               = GetSignature(payment.ProviderPassword,url,xmlSent);
				webRequest.Headers["X-Signature"] = sig;
				ret                               = 100;

				Tools.LogInfo("TransactionPayGenius.CallWebService/10",
				              "URL=" + url +
				            ", Token=" + payment.ProviderKey +
				            ", Key=" + payment.ProviderPassword +
				            ", Signature=" + sig, 220);

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
						ret        = 140;
						strResult  = rd.ReadToEnd();
					}
					if ( strResult.Length == 0 )
					{
						ret        = 150;
						resultMsg  = "No data returned from " + url;
					}
					else
					{
						Tools.LogInfo("TransactionPayGenius.CallWebService/20","JSON received=" + strResult,220);

						ret        = 160;
						resultCode = Tools.JSONValue(strResult,"code");
						resultMsg  = Tools.JSONValue(strResult,"message");

						if (Successful)
							resultCode = "00";
						else if ( Tools.StringToInt(resultCode) == 0 )
							resultCode = "99";
					}
				}
				ret = 0;
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGenius.CallWebService/298","ret="+ret.ToString(),220);
				Tools.LogException("TransactionPayGenius.CallWebService/299","ret="+ret.ToString(),ex);
			}
			return ret;
		}

		private int TestService(byte live=0)
      {
			try
			{
				string         url        = ( live == 0 ? "https://developer.paygenius.co.za/pg/api/v2/util/validate" : "https://www.paygenius.co.za/pg/api/v2/util/validate" );
				string         key        = ( live == 0 ? "f1a7d3b1-e90b-42c0-a304-459382a47aba" : "bb3a0012-74a5-4e74-bc46-03afa3c30850" );
				string         data       = "{\"data\":\"value\"}";
				byte[]         page       = Encoding.UTF8.GetBytes(data);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.ContentType    = "application/json";
				webRequest.Accept         = "application/json";
				webRequest.Method         = "POST";
				webRequest.Headers["X-Token"]     = ( live == 0 ? "60977662-6640-4701-96c8-ca6accbaac11" : "5403bd05-93da-49f7-8118-7a2713316dfe" );
				webRequest.Headers["X-Signature"] = GetSignature(key,url,data);

				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
						strResult = rd.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("TransactionPayGenius.CallWebService/99","",ex);
			}
			return 0;
		}

		private string GetSignature(string secretKey,string endPoint,string jsonData)
		{
			HMACSHA256 hmac = new HMACSHA256 (Encoding.Default.GetBytes(secretKey));
			byte[]     hash = hmac.ComputeHash (Encoding.Default.GetBytes(endPoint + "\n" + jsonData));
			string     sig  = "";

			for (int k = 0; k < hash.Length; k++)
				sig = sig + hash[k].ToString("X2"); // Hexadecimal

			return sig.ToLower();
		}

		public TransactionPayGenius() : base()
		{
			bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayGenius);
		}
	}
}
