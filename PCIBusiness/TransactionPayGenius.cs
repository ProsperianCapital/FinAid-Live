using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

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
				Tools.LogInfo("TransactionPayGenius.GetToken/10","Merchant Ref=" + payment.MerchantReference,10);

				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,1,"","}") // Changed to STRING from NUMERIC
				         + "}";
				ret      = 20;
//				ret      = TestService(0); // Dev
//				ret      = TestService(1); // Live
//				ret      = CallWebService(payment,"/pg/api/v2/card/register");
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				ret      = 30;
				payToken = Tools.JSONValue(XMLResult,"token");
				ret      = 40;
				if ( Successful && payToken.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("TransactionPayGenius.GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199);
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
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 10;
			payRef  = "";

			Tools.LogInfo("TransactionPayGenius.ProcessPayment/10","Merchant Ref=" + payment.MerchantReference,10);

			try
			{
				xmlSent = "{ \"creditCard\" : "  + Tools.JSONPair("token"    ,payment.CardToken,1,"{","}")
				        + ", \"transaction\" : " + Tools.JSONPair("reference",payment.MerchantReference,1,"{")
				                                 + Tools.JSONPair("currency" ,payment.CurrencyCode,1)
				                                 + Tools.JSONPair("amount"   ,payment.PaymentAmount.ToString(),11,"","}")
				        + ", "                   + Tools.JSONPair("threeDSecure","false",12,"","")
				        + "}";

				ret     = 20;
//				ret     = CallWebService(payment,"/pg/api/v2/payment/create");
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				ret     = 30;
				payRef  = Tools.JSONValue(XMLResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;
//				else
//					Tools.LogInfo("TransactionPayGenius.ProcessPayment/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayGenius.ProcessPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255);
				Tools.LogException("TransactionPayGenius.ProcessPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			int    ret      = 10;
			string url      = payment.ProviderURL;
			string tranDesc = "";

			if ( Tools.NullToString(url).Length == 0 )
				if ( Tools.LiveTestOrDev() == Constants.SystemMode.Live )
					url = "https://www.paygenius.co.za";
				else
					url = "https://developer.paygenius.co.za";

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
			{
				url      = url + "/pg/api/v2/card/register";
				tranDesc = "Get Token";
			}
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
			{
				url      = url + "/pg/api/v2/payment/create";
				tranDesc = "Process Payment";
			}
			else
			{ }

//			if ( Tools.NullToString(urlDetail).Length > 0 )
//			{
//				ret = 30;
//				if ( url.EndsWith("/") )
//					url = url.Substring(0,url.Length-1);
//				ret = 40;
//				if ( urlDetail.StartsWith("/") )
//					urlDetail = urlDetail.Substring(1);
//				ret = 50;
//				url = url + "/" + urlDetail;
//			}

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

				Tools.LogInfo("TransactionPayGenius.CallWebService/20",
				              "Transaction Type=" + tranDesc +
				            ", URL=" + url +
				            ", Token=" + payment.ProviderKey +
				            ", Key=" + payment.ProviderPassword +
				            ", Signature=" + sig +
				            ", JSON Sent=" + xmlSent, 10);

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
						Tools.LogInfo("TransactionPayGenius.CallWebService/30","Failed, JSON Rec=(empty)",199);
					}
					else
					{
						ret        = 160;
						resultCode = Tools.JSONValue(strResult,"code");
						resultMsg  = Tools.JSONValue(strResult,"message");

						if (Successful)
						{
							ret        = 170;
							resultCode = "00";
							Tools.LogInfo("TransactionPayGenius.CallWebService/40","Successful, JSON Rec=" + strResult,255);
						}
						else
						{
							ret = 180;
							Tools.LogInfo("TransactionPayGenius.CallWebService/50","Failed, JSON Rec=" + strResult,199);
							if ( Tools.StringToInt(resultCode) == 0 )
								resultCode = "99";
						}
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
//			Testing only!
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
				Tools.LogException("TransactionPayGenius.TestService/99","",ex);
			}
			return 0;
		}

		private string GetSignature(string secretKey,string endPoint,string jsonData)
		{
			HMACSHA256 hmac = new HMACSHA256 (Encoding.Default.GetBytes(secretKey));
			byte[]     hash = hmac.ComputeHash (Encoding.Default.GetBytes(endPoint + "\n" + jsonData));
			string     sig  = "";
			hmac            = null;

			for (int k = 0; k < hash.Length; k++)
				sig = sig + hash[k].ToString("X2"); // Hexadecimal

			return sig.ToLower();
		}

		public TransactionPayGenius() : base()
		{
			bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayGenius);
			xmlResult  = null;
		}
	}
}
