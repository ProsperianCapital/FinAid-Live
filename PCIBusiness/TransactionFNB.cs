using System;
using System.Text;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionFNB : Transaction
	{
		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
//				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

//	Testing
//				xmlSent  = Tools.JSONPair("cardHolderName","A N Other",1,"{")
//				         + Tools.JSONPair("pan"           ,"5413330089010483",1)
//				         + Tools.JSONPair("cvv"           ,"603",1)
//				         + Tools.JSONPair("expiryDate"    ,"milli seconds since 01/01/1970",1,"","}");
//				payment.CardExpiryYYYY = "2024";
//				payment.CardExpiryMM   = "03";
//	Testing

				if ( payment.CardName.Length > 0 )
					xmlSent = Tools.JSONPair("cardHolderName",payment.CardName,1,"{");
				else if ( payment.LastName.Length + payment.FirstName.Length > 0 )
					xmlSent = Tools.JSONPair("cardHolderName",(payment.FirstName+" "+payment.LastName).Trim(),1,"{");
				else
					xmlSent = "{";

				xmlSent  = xmlSent + Tools.JSONPair("pan"       ,payment.CardNumber,1)
				                   + Tools.JSONPair("cvv"       ,payment.CardCVV,1)
				                   + Tools.JSONPair("expiryDate",payment.CardExpiryMilliSeconds.ToString(),11,"","}");
//				                   + Tools.JSONPair("expiryDate",payment.CardExpiryYYYY + payment.CardExpiryMM + payment.CardExpiryDD,1,"","}");
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				payToken = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && payToken.Length > 0 )
					return 0;

				Tools.LogInfo("GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		public override int Reversal(Payment payment)
		{
			int ret  = 10;
			payRef   = "";
			otherRef = "";
			xmlSent  = "";

			try
			{
//	Testing
//				payment.TransactionID = "SGwHoKZ4lbYYVjDhIDKcewqsEpSr7Wkh5k8bCTPalVmjx74kzAmkGQw1gR8HOwwT";
//	Testing

				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.Reversal);
				otherRef = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && otherRef.Length > 0 )
					return 0;
				Tools.LogInfo("Reversal/50","JSON Rec="+strResult,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("Reversal/98","Ret="+ret.ToString(),255,this);
				Tools.LogException("Reversal/99","Ret="+ret.ToString(), ex,this);
			}
			return ret;
		}

		public override int Lookup(Payment payment)
		{
			int ret  = 10;
			otherRef = "";
			xmlSent  = "";

			if ( payment.TransactionID.Length < 1 )
			{
				resultCode = "ERROR/141";
				resultMsg  = "Missing TransactionId";
				return 20;
			}

			try
			{
//	Testing
//				payment.TransactionID = "SGwHoKZ4lbYYVjDhIDKcewqsEpSr7Wkh5k8bCTPalVmjx74kzAmkGQw1gR8HOwwT";
//				payment.TransactionID = "kkBOu59PBLkRvUKNXxBxsXSbhuADKFi5mOM5NVsEVHwWE7lJjAZFIlxoRmSS1nYn";
//	Testing

				ret      = 40;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.TransactionLookup);
				otherRef = Tools.JSONValue(strResult,"id");
				ret      = 50;
				if ( resultCode.ToUpper().StartsWith("BUSY") || resultCode.ToUpper().StartsWith("APPROV") )
					return ret;

				Tools.LogInfo("Lookup/50","Ret="+ret.ToString()
				                      + ", TransactionId="+payment.TransactionID
				                      + ", ResultCode="+resultCode
				                      + ", ResultMsg="+resultMsg
				                      + ", strResult="+strResult,10,this);

//	Ver 1, removed 2022/12/02
//				ret = 59;
//				if ( resultMsg.Length > 0 )
//					resultCode = resultMsg;
//				else
//					resultCode = "ERROR/142";

//	Ver 2, removed 2022/12/05
//				ret = 60;
//				if ( resultCode.Length < 1 )
//					if ( resultMsg.Length > 250 )
//						resultCode = resultMsg.Substring(0,250);
//					else if ( resultMsg.Length > 0 )
//						resultCode = resultMsg;

//	Ver 3, removed 2022/12/05
//				ret = 61;
//				if ( resultCode.Length < 1 )
//					resultCode = "ERROR/142";
//				if ( resultMsg.Length  < 1 )
//					resultMsg  = "No details supplied";
//
//				ret        = 62;
//				resultCode = resultCode + " (" + resultMsg;
//
//				if ( resultCode.Length > 249 )
//					resultCode = resultCode.Substring(0,249);
//
//				ret        = 63;
//				resultCode = resultCode + ")";

//	Ver 4, added 2022/12/05
				ret = 64;
				if ( resultMsg.Length > 249 )
					resultCode = resultMsg.Substring(0,249);
				else if ( resultMsg.Length > 0 )
					resultCode = resultMsg;
				else if ( resultCode.Length < 1 )
					resultCode = "ERROR/142";

//				ret = 70;
//				if ( resultCode.Length < 1 )
//					resultCode = "ERROR/142";

//				ret = 80;
//				if ( ret == 0 && otherRef.Length > 0 && resultCode.Length > 0 )
//					if ( resultCode.ToUpper().StartsWith("DECLINE") && resultMsg.Length > 0 )
//						resultCode = resultMsg;
//				if ( resultCode.Length == 0 )
//					resultCode = "ERROR";
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("Lookup/98","Ret="+ret.ToString(),255,this);
				Tools.LogException("Lookup/99","Ret="+ret.ToString(), ex,this);
			}
			return ret;
		}

		public override int Refund(Payment payment)
		{
			int ret  = 10;
			payRef   = "";
			otherRef = "";

			try
			{
//	Testing
//				payment.TransactionID = "ZlXzGHCFCxx5lrj9lwic80hdJ4P8PCKwd04fiAi7bGhErdAI6Dqyp2sGvhgZzXDe";
//	Testing

				ret      = 20;
				xmlSent  = Tools.JSONPair("amount"           ,payment.PaymentAmount.ToString(),11,"{")
				         + Tools.JSONPair("originalPaymentId",payment.TransactionID,1)
				         + Tools.JSONPair("refundType"       ,"CARD",1,"","}");
				ret      = 30;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.Refund);
				otherRef = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && otherRef.Length > 0 )
					return 0;
				Tools.LogInfo("Refund/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("Refund/98","Ret="+ret.ToString(),255,this);
				Tools.LogException("Refund/99","Ret="+ret.ToString(), ex,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret  = 10;
			payRef   = "";
			otherRef = "";

//	Testing
//			payment.CardToken = "HgYwCV8vUE0qAFvxGr902XM1J04cFHgDSlW7sq5KmldkWi5zRjX2zrq8psW826Al";
//			payment.CardToken = "IdYsxppCTuLbrXDW3shAFCOXP1vpdSxH62dMR25I99UkabHaHY0vmeOfzXvEWhrJ";
//			payment.CardToken = "u2SP9zdVP1Xda44E4gkjlfIkp64owKqN5ucKsvKFskSqdFihSiQD3HZi44fKUNQK";
//	Testing
/*
{ "paymentSplit" :
  [
   { "cardToken" : "IdYsxppCTuLbrXDW3shAFCOXP1vpdSxH62dMR25I99UkabHaHY0vmeOfzXvEWhrJ",
     "amount" : 899,
     "paymentType" : "MOTO"
   }
  ],
  "basket" :
  { 
    "usingBasketDetails" :
    {
      "items" :
      [
       { "description" : "Subscription",
         "amount" : 899
       }
      ],
      "merchantOrderNumber" : "A133208703"
    }
  }
}
*/
			try
			{
				ret     = 20;
				xmlSent = "{ \"paymentSplit\" : " + Tools.JSONPair("cardToken"  ,payment.CardToken,1,"[{")
				                                  + Tools.JSONPair("amount"     ,payment.PaymentAmount.ToString(),11)
				                                  + Tools.JSONPair("paymentType","MOTO",1,"","}],")
				        +    "\"basket\" : { \"usingBasketDetails\" :"
				        +                 "{ \"items\" : " + Tools.JSONPair("description","Subscription",1,"[{")
				                                           + Tools.JSONPair("amount",payment.PaymentAmount.ToString(),11,"","}],")
				        +                  Tools.JSONPair("merchantOrderNumber",payment.MerchantReference,1,"","")
				        + "}}}";
				ret     = 30;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				payRef  = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && payRef.Length > 0 )
				{
					resultCode = "BUSY";
					return 0;
				}
				if ( resultCode.Length < 1 )
					resultCode = "ERROR/703";

//	Check transaction status
//	NO! Always returns "Busy" ... must do this later, separately
//				if ( ret == 0 && payRef.Length > 0 )
//				{
//					ret                   = 40;
//					xmlSent               = "";
//					payment.TransactionID = payRef;
//					ret                   = CallWebService(payment,(byte)Constants.TransactionType.TransactionLookup);
//					otherRef              = Tools.JSONValue(strResult,"id");
//					if ( ret == 0 && otherRef.Length > 0 && resultCode.Length > 0 && resultCode.StartsWith("APPROVE") )
//						return 0;
//					ret = 50;
//				}

				Tools.LogInfo("TokenPayment/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			int    k;
			int    ret       = 10;
			string url       = payment.ProviderURL;
			string resultURL = "";
			string urlPart   = "";
			string webMethod = "POST";

			resultCode = "10";
			resultMsg  = "(10) Internal error";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
			{
				ret     = 40;
				url     = url + "/mtokenizer/api/card";
				urlPart = "API/CARD/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment ) // Spec section 3.3.3
			{
				ret     = 50;
				url     = url + "/ape/api/pay/simple";
				urlPart = "PAY/TRACK/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.TransactionLookup ) // Spec section 3.4.3
			{
				ret = 70;
				if ( payment.TransactionID.Length < 1 )
				{
					Tools.LogInfo("CallWebService/260","Lookup, empty TransactionID",220,this);
					resultCode = "70";
					resultMsg  = "(70) Empty TransactionID";
					return ret;
				}
				url       = url + "/ape/api/pay/track/" + payment.TransactionID;
				webMethod = "GET";
			}
			else if ( transactionType == (byte)Constants.TransactionType.Refund )
			{
				ret = 80;
				if ( payment.TransactionID.Length < 1 )
				{
					Tools.LogInfo("CallWebService/261","Refund, empty TransactionID",220,this);
					resultCode = "80";
					resultMsg  = "(80) Empty TransactionID";
					return ret;
				}
				url     = url + "/ape/api/refund";
				urlPart = "REFUND/TRACK/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.Reversal )
			{
				ret = 90;
				if ( payment.TransactionID.Length < 1 )
				{
					Tools.LogInfo("CallWebService/262","Reversal, empty TransactionID",220,this);
					resultCode = "90";
					resultMsg  = "(90) Empty TransactionID";
					return ret;
				}
				url       = url + "/ape/api/pay/reverse/" + payment.TransactionID;
				urlPart   = "TRACK/REVERSAL/";
				webMethod = "PUT";
			}
			else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
			{
				ret = 100;
				url = url + "/eCommerce/v2/prepareTransaction";
			}
			else
			{
				Tools.LogInfo("CallWebService/263","Unknown transaction type (transactionType="+transactionType.ToString()+")",220,this);
				ret        = 110;
				resultCode = "110";
				resultMsg  = "(110) Unknown transaction type";
				return ret;
			}

			ret         = 130;
			strResult   = "";
			resultCode  = "130";
			resultMsg   = "(130) Internal error connecting to " + url;
			ret         = 140;
			byte endLog = 1;

//	Testing
//			payment.ProviderKey       = "REVqzPb4PTiD4n7Fo3e1p1VyQUbvmy5YZuhxhUpqL0EcUTGWHPchIUd8m3LeixLf"; // API Key
//			payment.ProviderPassword  = "sbyq0CUAvUSPMifwRH0f68fByQ5ZgSjyEpbeKg77o1Cuh9BD30ucakuXtpCCUMJN"; // Instance Key
//			payment.ProviderKeyPublic = "Blah ?"                                                          ; // Shared secret key
//	Testing

			try
			{
				byte[]         page            = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest      = (HttpWebRequest)WebRequest.Create(url);
			//	webRequest.ContentType         = "application/json;charset=UTF-8";
				webRequest.ContentType         = "application/json";
				webRequest.Accept              = "application/json";
				webRequest.Method              = webMethod;
				ret                            = 170;
				webRequest.Headers["API-KEY"]  = payment.ProviderKey;
				ret                            = 180;
				webRequest.Headers["INST-KEY"] = payment.ProviderPassword;
				ret                            = 190;
				if ( payment.ProviderKeyPublic.Length > 0 )
					webRequest.Headers["SHARED-SECRET"] = payment.ProviderKeyPublic;
				ret                                    = 200;

//	Testing
//	Detailed logging
//				string h = "";
//				k        = 0;
//				foreach (string key in webRequest.Headers.AllKeys )
//				{
//					h = h + Environment.NewLine + "[" + (k++).ToString() + "] " + key + " : ";
//					if ( webRequest.Headers[key].ToUpper() == payment.ProviderKey.ToUpper() ||
//					     webRequest.Headers[key].ToUpper() == payment.ProviderPassword.ToUpper() )
//						h = h + Tools.MaskedValue(webRequest.Headers[key]);
//					else
//						h = h + webRequest.Headers[key];
//				}
//				Tools.LogInfo("CallWebService/20","Transaction Type=" + Tools.TransactionTypeName(transactionType) +
//				                                ", URL="              + url +
//				                                ", API Key="          + Tools.MaskedValue(payment.ProviderKey) +
//				                                ", Instance Key="     + Tools.MaskedValue(payment.ProviderPassword) +
//				                                ", Request Body="     + xmlSent +
//				                                ", Request Headers="  + h, 199, this);
//
//	Basic logging
//				Tools.LogInfo("CallWebService/21","Transaction Type=" + Tools.TransactionTypeName(transactionType) +
//				                                ", URL="              + url +
//				                                ", Request Body="     + xmlSent, 199, this);
//	Testing

				if ( xmlSent.Length > 0 && page.Length > 0 )
					using (Stream stream = webRequest.GetRequestStream())
					{
						ret = 210;
						stream.Write(page, 0, page.Length);
						stream.Flush();
						stream.Close();
					}

				ret = 220;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 230;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 240;
						strResult = rd.ReadToEnd().Trim();
					}
					if ( urlPart.Length > 0 && webResponse.Headers["Location"] != null )
					{
						ret       = 250;
						resultURL = webResponse.Headers["Location"].ToString();
						ret       = 260;
						endLog    = 2;
						k         = resultURL.ToUpper().IndexOf(urlPart);
						if ( resultURL.Length > 0 && k > 0 )
						{
							Tools.LogInfo("CallWebService/31","Success (" + urlPart + "), Location="+resultURL,10,this);
							ret        = 270;
							strResult  = Tools.JSONPair("transactionId",resultURL.Substring(urlPart.Length+k),1,"{",",")
							           + Tools.JSONPair("resultUrl",resultURL,1,"","}")
							           + strResult;
							ret        = 280;
							strResult  = strResult.Replace("}{",",");
							ret        = 0;
							endLog     = 0;
							resultCode = "00";
							resultMsg  = "";
						}
						else
							ret = 290;
					}
					else if ( strResult.Length > 0 )
					{
					//	Possible values for "status":
					//		Approved
					//		Declined
					//		Failed
					//		Reverse
					//		Busy

						ret        = 320;
						endLog     = 3;
						resultMsg  = "";
						resultCode = Tools.JSONValue(strResult,"status").ToUpper();
/*
3d JSON result
{ "url":"https://sandbox.ms.fnb.co.za/eCommerce/v2/getPaymentOptions?token=XYZ",
  "iframe":false,
  "txnToken":"XYZ"
}
*/
						if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
						{
							ret = 324;
							if ( Tools.JSONValue(strResult,"url").Length      > 0 &&
						        Tools.JSONValue(strResult,"txnToken").Length > 0 )
							{
								endLog     = 0;
								resultCode = "00";
							}
							else if ( resultCode.Length < 1 )
								resultCode = "ERROR/331";
						}
						else if ( resultCode.StartsWith("APPROV") )
							endLog = 0;

						else
						{
							ret           = 330;
							endLog        = 4;
							if ( resultCode.Length == 0 )
								resultCode = "ERROR/332";
							else if ( resultCode.StartsWith("BUSY") )
								resultMsg = Tools.JSONValue(strResult,"busyMessage");
							else if ( resultCode.StartsWith("DECLINE") )
								resultMsg = Tools.JSONValue(strResult,"declineMessage");
							else if ( resultCode.StartsWith("FAIL") )
								resultMsg = Tools.JSONValue(strResult,"failMessage");
							if ( resultMsg.Length == 0 )
								resultMsg = strResult;
						}
						ret = 0;
					}
					else
					{
						ret    = 370;
						endLog = 5;
					}
				}
			}
			catch (WebException ex1)
			{
				endLog        = 6;
				strResult     = Tools.DecodeWebException(ex1,ClassName+".CallWebService/291","ret="+ret.ToString());
				if ( strResult.Length == 0 )
				{
					strResult  = Tools.JSONPair("status" ,ex1.Status.ToString(),1,"{")
					           + Tools.JSONPair("error"  ,ex1.Message,1)
					           + Tools.JSONPair("message",ex1.ToString(),1,"","}");
					resultCode = ex1.Status.ToString();
				}
				else
				{
					resultCode = Tools.JSONValue(strResult,"statusCode").ToUpper();
					resultMsg  = Tools.JSONValue(strResult,"message");
					if ( resultMsg.Length == 0 )
						resultMsg = strResult;
				}
				if ( resultCode.Length == 0 )
					resultCode = "ERROR/291";
				if ( resultMsg.Length == 0 )
					resultMsg  = ex1.Message;
			}
			catch (Exception ex2)
			{
				endLog = 7;
				Tools.LogException("CallWebService/294","ret="+ret.ToString(),ex2,this);
				if ( strResult.Length == 0 )
					strResult = Tools.JSONPair("status" ,ex2.Source.ToString(),1,"{")
					          + Tools.JSONPair("error"  ,ex2.Message,1)
					          + Tools.JSONPair("message",ex2.ToString(),1,"","}");
				else
				{
					resultCode = Tools.JSONValue(strResult,"statusCode").ToUpper();
					resultMsg  = Tools.JSONValue(strResult,"message");
					if ( resultMsg.Length == 0 )
						resultMsg = strResult;
				}
				if ( resultCode.Length == 0 )
					resultCode = "ERROR/293";
				if ( resultMsg.Length == 0 )
					resultMsg  = ex2.Message;
			}
			if ( endLog > 0 )
				Tools.LogInfo("CallWebService/299","transactionType="+transactionType.ToString()
				                              + " | ret="            +ret.ToString()
				                              + " | endLog="         +endLog.ToString()
				                              + " | resultCode="     +resultCode
				                              + " | resultMsg="      +resultMsg
				                              + " | urlPart="        +urlPart
				                              + " | resultURL="      +resultURL
				                              + " | xmlSent="        +xmlSent
				                              + " | strResult="      +strResult,220,this);
			return ret;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret       = 10;
			string urlReturn = "";
			d3Form           = "";

			try
			{
//				Tools.LogInfo("ThreeDSecurePayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

				if ( postBackURL == null )
					urlReturn = Tools.ConfigValue("SystemURL");
				else
					urlReturn = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! urlReturn.EndsWith("/") )
					urlReturn = urlReturn + "/";
				ret       = 20;
				urlReturn = urlReturn + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				                      +                    "&TransRef="+Tools.XMLSafe(payment.MerchantReference)
				                      +                    "&ResultCode=";

//	Testing
//	{	"apiKey" : "f9bd07c6-a662-441c-8335-365a967cf1b3",
//		"merchantOrderNumber" : "MON123456",
//		"amount" : 12300,
//		"validationURL" : "http://test.co.za/validate",
//		"description" : "Test Transaction" }
//	Testing

				xmlSent  = Tools.JSONPair("apiKey"             ,payment.ProviderKey,1,"{")
				         + Tools.JSONPair("merchantOrderNumber",payment.MerchantReference,1)
				         + Tools.JSONPair("amount"             ,"100",11)
				         + Tools.JSONPair("successURL"         ,urlReturn+"00",1)
				         + Tools.JSONPair("failureURL"         ,urlReturn+"09",1)
				         + Tools.JSONPair("description"        ,payment.PaymentDescription,1,"","}");
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecurePayment);
				ret      = 30;
				payToken = Tools.JSONValue(strResult,"txnToken");
				d3Form   = Tools.JSONValue(strResult,"url");
				ret      = 40;
				if ( payToken.Length > 0 && d3Form.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("ThreeDSecurePayment/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecurePayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("ThreeDSecurePayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		public TransactionFNB() : base()
		{
			ServicePointManager.Expect100Continue = false; // Yes, this must be FALSE
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(Constants.PaymentProvider.FNB);
		}
	}
}
