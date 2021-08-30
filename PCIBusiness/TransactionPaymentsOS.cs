using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace PCIBusiness
{
	public class TransactionPaymentsOS : Transaction
	{
		public  bool Successful
		{
			get { return ( resultCode == "000" ); }
		}

		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

				xmlSent  = Tools.JSONPair("token_type"      ,"credit_card",1,"{")
				         + Tools.JSONPair("holder_name"     ,payment.CardName,1)
				         + Tools.JSONPair("card_number"     ,payment.CardNumber,1)
				         + Tools.JSONPair("credit_card_cvv" ,payment.CardCVV,1)
				         + Tools.JSONPair("expiration_date" ,payment.CardExpiryMM + "/" + payment.CardExpiryYYYY,1,"","}");

				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				ret      = 30;
				payToken = Tools.JSONValue(XMLResult,"token");
				ret      = 40;
				if ( Successful && payToken.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199,this);
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

			int ret = 10;
			payRef  = "";

			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
				xmlSent = Tools.JSONPair("amount"                   ,payment.PaymentAmount.ToString(),11,"{") // Amount must be INT in CENTS
				        + Tools.JSONPair("currency"                 ,payment.CurrencyCode,1)
				        + Tools.JSONPair("statement_soft_descriptor",payment.PaymentDescription,1,"","}");
				ret     = 20;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment,1);
				ret     = 30;
				payRef  = Tools.JSONValue(XMLResult,"id");
				ret     = 40;

				if ( Successful && payRef.Length > 0 )
				{
					ret     = 50;
					xmlSent = Tools.JSONPair("reconciliation_id",payment.MerchantReference,1,"{")
					        + "\"payment_method\":"
					        + Tools.JSONPair("type"             ,"tokenized",1,"{")
					        + Tools.JSONPair("token"            ,payment.CardToken,1)
					        + Tools.JSONPair("credit_card_cvv"  ,payment.CardCVV,1,"","}") + "}";
					ret     = 60;
					ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment,2);
					ret     = 70;
					payRef  = Tools.JSONValue(XMLResult,"id");
					ret     = 80;
				}

				if ( Successful && payRef.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("TokenPayment/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType,byte subType=0)
      {
			int    ret      = 10;
			string url      = payment.ProviderURL;
			string tranDesc = "";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
			{
				url      = url + "/tokens";
				tranDesc = "Get Token";
			}
			else
			{
				url         = url + "/payments" + ( subType == 2 ? "/" + payRef + "/charges" : "" );
				if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
					tranDesc = "Token Payment";
				else if ( transactionType == (byte)Constants.TransactionType.CardPayment )
					tranDesc = "Card Payment";
			}

			ret        = 60;
			strResult  = "";
			resultCode = "990";
			resultMsg  = "Internal error connecting to " + url;
			ret        = 70;

			try
			{
				byte[]         page       = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType    = "application/json";
				webRequest.Accept         = "application/json";
				webRequest.Method         = "POST";
				ret                       = 60;

				if ( transactionType == (byte)Constants.TransactionType.GetToken )
					webRequest.Headers["public-key"]  = payment.ProviderKey;
				//	webRequest.Headers["public-key"]  = "daea1771-d849-4fa4-a648-230a54186964";

				else if ( transactionType == (byte)Constants.TransactionType.CardPayment ||
				          transactionType == (byte)Constants.TransactionType.TokenPayment )
				{
					webRequest.Headers["app-id"]      = payment.ProviderAccount;
					webRequest.Headers["private-key"] = payment.ProviderPassword;
				//	webRequest.Headers["private-key"] = "3790d1d5-4847-43e6-a29a-f22180cc9fda";
				}

				webRequest.Headers["x-payments-os-env"] = ( Tools.SystemIsLive() ? "live" : "test" );
				webRequest.Headers["api-version"]       = "1.3.0";

				Tools.LogInfo("CallWebService/20","Transaction Type=" + tranDesc +
				                                ", URL=" + url +
				                                ", App Id=" + payment.ProviderAccount +
				                                ", Public Key=" + payment.ProviderKey +
				                                ", Private Key=" + payment.ProviderPassword +
				                                ", JSON Sent=" + xmlSent, 210, this);

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
						resultCode = "992";
						resultMsg  = "No data returned from " + url;
						Tools.LogInfo("CallWebService/30","Failed, JSON Rec=(empty)",199,this);
					}
					else
					{
						Tools.LogInfo("CallWebService/40","Successful, JSON Rec=" + strResult,255,this);
						ret        = 160;
						resultCode = "994";
						resultMsg  = Tools.JSONValue(strResult,"state");
						if ( resultMsg.Length < 1 )
							resultMsg = Tools.JSONValue(strResult,"status");
						if ( resultMsg.Length > 0 )
							resultCode = "000"; // Success
					}
				}
				ret = 0;
			}
			catch (WebException ex1)
			{
//				Example:
//				The remote server returned an error: (400) Bad Request.

				strResult = Tools.DecodeWebException(ex1,ClassName+".CallWebService/297","ret="+ret.ToString());
				resultMsg = "";
				if ( strResult.Length > 0 )
				{
					string e1  = Tools.JSONValue(strResult,"category");
					string e2  = Tools.JSONValue(strResult,"more_info");
					string e3  = Tools.JSONValue(strResult,"description");
					if ( e1.Length > 0 ) resultMsg = e1;
					if ( e2.Length > 0 ) resultMsg = resultMsg + ", " + e2;
					if ( e3.Length > 0 ) resultMsg = resultMsg + " (" + e3 + ")";
				}
				resultCode = ex1.Message;
				int j      = resultCode.IndexOf("(");
				int k      = resultCode.IndexOf(")",j+1);
				if ( j > 0 && k > j+1 )
					resultCode = resultCode.Substring(j+1,k-j-1);
				else
					resultCode = "996";
				if ( resultMsg.Length == 0 )
					resultMsg = ex1.Message;
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("CallWebService/998","ret="+ret.ToString(),220,this);
				Tools.LogException("CallWebService/999","ret="+ret.ToString(),ex2,this);
			}
			return ret;
		}

//		private int TestService(byte live=0)
//    {
//	//		Testing only!
//			try
//			{
//	//			string         url        = ( live == 0 ? "https://developer.paygenius.co.za/pg/api/v2/util/validate" : "https://www.paygenius.co.za/pg/api/v2/util/validate" );
//				string         url        = BureauURL + "/pg/api/v2/util/validate";
//				string         key        = ( live == 0 ? "f1a7d3b1-e90b-42c0-a304-459382a47aba" : "bb3a0012-74a5-4e74-bc46-03afa3c30850" );
//				string         data       = "{\"data\":\"value\"}";
//				byte[]         page       = Encoding.UTF8.GetBytes(data);
//				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
//
//				webRequest.ContentType    = "application/json";
//				webRequest.Accept         = "application/json";
//				webRequest.Method         = "POST";
//				webRequest.Headers["X-Token"]     = ( live == 0 ? "60977662-6640-4701-96c8-ca6accbaac11" : "5403bd05-93da-49f7-8118-7a2713316dfe" );
//				webRequest.Headers["X-Signature"] = GetSignature(key,url,data);
//
//				using (Stream stream = webRequest.GetRequestStream())
//				{
//					stream.Write(page, 0, page.Length);
//					stream.Flush();
//					stream.Close();
//				}
//
//				using (WebResponse webResponse = webRequest.GetResponse())
//				{
//					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
//						strResult = rd.ReadToEnd();
//				}
//			}
//			catch (Exception ex)
//			{
//				Tools.LogException("TestService/99","",ex,this);
//			}
//			return 0;
//		}

		public TransactionPaymentsOS() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.PaymentsOS);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
		}
	}
}
