using System;
using System.Text;
using System.Net;
using System.IO;
// using Stripe;

namespace PCIBusiness
{
	public class TransactionStripe : Transaction
	{
		public  bool Successful
		{
			get { return Tools.JSONValue(strResult,"success").ToUpper() == "TRUE"; }
		}

		public override int GetToken(Payment payment)
		{
			return 99010;

			int ret  = 10;
			payToken = "";

			try
			{
				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

//				var customerOptions = new CustomerCreateOptions
//				{
//					Name  = (payment.FirstName + " " + payment.LastName).Trim(),
//					Email = payment.EMail,
//					Phone = payment.PhoneCell
//				};
//				var customerservice = new CustomerService();
//				var customer        = customerservice.Create(customerOptions);
//
//				var paymentOptions  = new PaymentIntentCreateOptions
//				{
//					Amount   = payment.PaymentAmount,
//					Currency = payment.CurrencyCode,
//					Customer = customer.Id,
//				};
//				var paymentIntentservice = new PaymentIntentService();
//				var paymentIntent        = paymentIntentservice.Create(paymentOptions);	

				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,1,"","}") // Changed to STRING from NUMERIC
				         + "}";
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
			return 99020;

			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 10;
			payRef  = "";

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
				payRef  = Tools.JSONValue(XMLResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;
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

		public override int CardPayment(Payment payment)
		{
			return 99030;

			int ret = 10;

			try
			{
				xmlSent =  "merchant_id="   + Tools.URLString(payment.ProviderUserID)
				        + "&merchant_key="  + Tools.URLString(payment.ProviderKey)
				        + "&notify_url="    + Tools.URLString("")
				        + "&name_first="    + Tools.URLString(payment.FirstName)
				        + "&name_last="     + Tools.URLString(payment.LastName)
				        + "&email_address=" + Tools.URLString(payment.EMail)
				        + "&m_payment_id="  + Tools.URLString(payment.MerchantReference)
				        + "&amount="        + Tools.URLString(payment.PaymentAmountDecimal)
				        + "&item_name="     + Tools.URLString(payment.PaymentDescription)
				        + "&subscription_type=2";

				ret     = 40;
//				xmlSent = xmlSent + "&signature=" + HashMD5(xmlSent);
//				ret     = PostHTML(payment.ProviderURL);

				Tools.LogInfo("CardPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("CardPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("CardPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
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

			ret        = 60;
			strResult  = "";
			resultCode = "98";
			resultMsg  = "(98) Internal error connecting to " + url;
			ret        = 70;

			try
			{
//				string         sig;
				byte[]         page               = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest         = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType            = "application/json";
				webRequest.Accept                 = "application/json";
				webRequest.Method                 = "POST";
				ret                               = 60;
				webRequest.Headers["X-Token"]     = payment.ProviderKey;
				ret                               = 90;
//				sig                               = GetSignature(payment.ProviderPassword,url,xmlSent);
//				webRequest.Headers["X-Signature"] = sig;
				ret                               = 100;

				Tools.LogInfo("CallWebService/20",
				              "Transaction Type=" + tranDesc +
				            ", URL=" + url +
				            ", Token=" + payment.ProviderKey +
				            ", Key=" + payment.ProviderPassword +
				            ", JSON Sent=" + xmlSent, 10, this);

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
							Tools.LogInfo("CallWebService/40","Successful, JSON Rec=" + strResult,255,this);
						}
						else
						{
							ret = 180;
							Tools.LogInfo("CallWebService/50","Failed, JSON Rec=" + strResult,199,this);
							if ( Tools.StringToInt(resultCode) == 0 )
								resultCode = "99";
						}
					}
				}
				ret = 0;
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,"TransactionStripe.CallWebService/297","ret="+ret.ToString());
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
			try
			{
			}
			catch (Exception ex)
			{
				Tools.LogException("TestService/99","",ex,this);
			}
			return 99040;;
		}

		public TransactionStripe() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.Stripe);
			xmlResult = null;

//			if ( Tools.SystemIsLive() )
//				StripeConfiguration.ApiKey = "sk_live";
//			else
//				StripeConfiguration.ApiKey = "sk_test_51It78gGmZVKtO2iKwt179k2NOmHVUNab70RO7EcbRm7AZmvunvtgD4S0srMXQWIpvj3EAWq7QLJ4kcRIMRHPzPxq00n0dLN01U"; // Secret key
		}
	}
}
