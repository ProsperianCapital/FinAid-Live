using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace PCIBusiness
{
	public class TransactionPayGenius : Transaction
	{
		private byte logPriority;

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
				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,1,"","}")
				         + "}";
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				ret      = 30;
				payToken = Tools.JSONValue(XMLResult,"token");
				ret      = 40;
				if ( Successful && payToken.Length > 0 )
					ret   = 0;

//				Tools.LogInfo("GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,(byte)(ret==0?logPriority:199),this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int    ret = 10;
			string url = Tools.ConfigValue("SystemURL")+"/Succeed.aspx?TransRef=" + Tools.XMLSafe(payment.MerchantReference);
		//	string url = "https://lifestyledirectglobal.com";
			payRef     = "";

			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
				xmlSent = "{ \"creditCard\" : "  + Tools.JSONPair("token"        ,payment.CardToken,1,"{")
				                                 + Tools.JSONPair("cvv"          ,payment.CardCVV,1,"","}")
				        + ", \"transaction\" : " + Tools.JSONPair("reference"    ,payment.MerchantReference,1,"{")
				                                 + Tools.JSONPair("currency"     ,payment.CurrencyCode,1)
				                                 + Tools.JSONPair("amount"       ,payment.PaymentAmount.ToString(),11,"","}")
				        + ", "                   + Tools.JSONPair("callbackUrl"  ,url,     1,"","")
				        + ", "                   + Tools.JSONPair("threeDSecure" ,"false",12,"","")
//				        + ", "                   + Tools.JSONPair("supports3dsV2","false",12,"","")
				        + ", "                   + Tools.JSONPair("autoExecute"  ,"true" ,12,"","")
				        + ", "                   + Tools.JSONPair("recurring"    ,"true" ,12,"","")
				        + "}";

				ret     = 20;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				ret     = 30;
				payRef  = Tools.JSONValue(XMLResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;

//				Tools.LogInfo("TokenPayment/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,(byte)(ret==0?logPriority:199),this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			int    ret = 10;
			string url = payment.ProviderURL;

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
				url = url + "/pg/api/v2/card/register";
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
				url = url + "/pg/api/v2/payment/create";
			else
			{ }

			ret        = 60;
			strResult  = "";
			resultCode = "98";
			resultMsg  = "(98) Internal error connecting to " + url;
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

				Tools.LogInfo("CallWebService/20",
				              "Transaction Type=" + Tools.TransactionTypeName(transactionType) +
				            ", URL="              + url +
				            ", Token="            + Tools.MaskedValue(payment.ProviderKey) +
				            ", Key="              + Tools.MaskedValue(payment.ProviderPassword) +
//				            ", Signature="        + sig +
				            ", JSON Sent="        + xmlSent, logPriority, this);

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
						rd.Close();
					}
					webResponse.Close();
				}

				if ( strResult.Length == 0 )
				{
					ret        = 150;
					resultMsg  = "No data returned from " + url;
					Tools.LogInfo("CallWebService/30","Failed, JSON Rec=(empty)",199,this);
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
						Tools.LogInfo("CallWebService/40","Successful, JSON Rec=" + strResult,logPriority,this);
					}
					else
					{
						ret = 180;
						Tools.LogInfo("CallWebService/50","Failed, JSON Rec=" + strResult,199,this);
						if ( Tools.StringToInt(resultCode) == 0 )
							resultCode = "99";
					}
				}
				ret = 0;
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
				string         url        = BureauURL + "/pg/api/v2/util/validate";
				string         key        = ( live == 0 ? "blah-blah-42c0-a304-459382a47aba" : "blah-blah-4e74-bc46-03afa3c30850" );
				string         data       = "{\"data\":\"value\"}";
				byte[]         page       = Encoding.UTF8.GetBytes(data);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.ContentType    = "application/json";
				webRequest.Accept         = "application/json";
				webRequest.Method         = "POST";
				webRequest.Headers["X-Token"]     = ( live == 0 ? "blah-blah-4701-96c8-ca6accbaac11" : "blah-blah-49f7-8118-7a2713316dfe" );
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
				Tools.LogException("TestService/99","",ex,this);
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
			base.LoadBureauDetails(Constants.PaymentProvider.PayGenius);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
//			logPriority                           = 10;  // For production, when all is stable
			logPriority                           = 234; // For testing/development, to log very detailed errors
		}
	}
}
