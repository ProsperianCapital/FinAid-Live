using System;
using System.Net;
using Stripe;

namespace PCIBusiness
{
	public class TransactionStripe : Transaction
	{
		private string err;
		private byte   logPriority;

	//	Moved to Transaction.cs
	//	private string customerId;
	//	private string paymentMethodId;

		public override int GetToken(Payment payment)
		{
			int ret         = 10;
			err             = "";
			payToken        = "";
			customerId      = "";
			paymentMethodId = "";
			strResult       = "";
			resultCode      = "991";
			resultMsg       = "Fail";

//			Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
//	Testing
//				StripeConfiguration.ApiKey = "sk_test_51It78gGmZVKtO2iKBZF7DA5JisJzRqvibQdXSfBj9eQh4f5UDvgCShZIjznOWCxu8MtcJG5acVkDcd8K184gIegx001uXlHI5g";
//				StripeConfiguration.ApiKey = "pk_test_51O5sdDGIk82LjXJ5xtnw1BnY6As7pI9hZGZHfvmQjO9fVWGqVd1lj0snqTSZm6Wye015y6KBI3eaP4NphgNksuR3003upVu7Nw";
//	Testing
				ret                        = 20;
				StripeConfiguration.ApiKey = payment.ProviderPassword; // Secret key

//				Tools.LogInfo("GetToken/30","About to create Stripe.TokenCreateOptions",logPriority,this);
				ret                        = 30;
				var tokenOptions           = new TokenCreateOptions
				{
					Card = new TokenCardOptions
					{
						Number         = payment.CardNumber,
						ExpMonth       = payment.CardExpiryMonth,
						ExpYear        = payment.CardExpiryYear,
						Cvc            = payment.CardCVV,  
						Name           = payment.CardName,
						AddressLine1   = payment.Address1(0),
					//	AddressLine2   = payment.Address2(0),
						AddressCity    = payment.Address2(0),
						AddressState   = payment.ProvinceCode,
						AddressZip     = payment.PostalCode(0),
						AddressCountry = payment.CountryCode(83)
					}
				};

//				Tools.LogInfo("GetToken/40","About to create Stripe.TokenService",logPriority,this);
				ret              = 40;
				var tokenService = new TokenService();
				var token        = tokenService.Create(tokenOptions);
				payToken         = token.Id;
//				token.ClientIp   = payment.MandateIPAddress;
				err              = err + ", cardNumber=" + Tools.MaskedValue(payment.CardNumber) + ", tokenId="+Tools.NullToString(payToken);
				Tools.LogInfo("GetToken/40","Stripe.TokenService.Create(), cardNumber=" + Tools.MaskedValue(payment.CardNumber) + " ,token="+Tools.NullToString(payToken),logPriority,this);

//				Tools.LogInfo("GetToken/50","About to create Stripe.PaymentMethodCardOptions",logPriority,this);
				ret                      = 50;
				var paymentMethodOptions = new PaymentMethodCreateOptions
				{ 
					Type = "card",
					Card = new PaymentMethodCardOptions
					{  
						Token = token.Id
					},
					BillingDetails = new PaymentMethodBillingDetailsOptions
					{
						Name    = payment.CardName,
						Email   = payment.EMail,
						Phone   = payment.PhoneCell,
						Address = new AddressOptions
						{
							Line1      = payment.Address1(0),
					//		Line2      = payment.Address2(0),
							City       = payment.Address2(0),
							State      = payment.ProvinceCode,
							PostalCode = payment.PostalCode(0),
							Country    = payment.CountryCode(83)
						}
					}
//					RadarOptions = new PaymentMethodRadarOptions
//					{
//						Session = X
//					}
				};

//				Tools.LogInfo("GetToken/60","About to create Stripe.PaymentMethodService",logPriority,this);
				ret                      = 60;
				var paymentMethodService = new PaymentMethodService();
				var paymentMethod        = paymentMethodService.Create(paymentMethodOptions);
				paymentMethodId          = paymentMethod.Id;
				err                      = err + ", paymentMethodId="+Tools.NullToString(paymentMethodId);
				Tools.LogInfo("GetToken/60","Stripe.PaymentMethodService.Create(), paymentMethodId="+Tools.NullToString(paymentMethodId),logPriority,this);

//				Tools.LogInfo("GetToken/70","About to create Stripe.CustomerCreateOptions",logPriority,this);
				ret              = 70;
				string tmp       = (payment.FirstName + " " + payment.LastName).Trim();
				if ( tmp.Length  < 2 )
					tmp           = payment.CardName;
				var customerOptions = new CustomerCreateOptions
				{
				//	Name          = payment.CardName, // (payment.FirstName + " " + payment.LastName).Trim(),
					Name          = tmp,
					Email         = payment.EMail,
					Phone         = payment.PhoneCell,
					Description   = payment.MerchantReference + ( tmp.Length > 0 ? " (" + tmp + ")" : "" ),
					PaymentMethod = paymentMethod.Id
				};

				ret         = 75;
//				string addr = "[Empty]";
				if ( payment.Address1(0).Length > 0 || payment.Address2(0).Length > 0 || payment.ProvinceCode.Length > 0 || payment.CountryCode(0).Length > 0 )
				{
//					Tools.LogInfo("GetToken/80","About to create Stripe.AddressOptions",logPriority,this);
					ret                     = 80;
					customerOptions.Address = new AddressOptions()
					{
						Line1      = payment.Address1(0),
					//	Line2      = payment.Address2(0),
						City       = payment.Address2(0),
						State      = payment.ProvinceCode,
						PostalCode = payment.PostalCode(0)
					};
//					addr = payment.Address1(0)+"/"+payment.Address2(0)+"/"+payment.ProvinceCode+"/"+payment.PostalCode(0);
					ret  = 85;
					if ( payment.CountryCode(0).Length == 2 )
					{
						customerOptions.Address.Country = payment.CountryCode(83);
//						addr = addr + "/" + payment.CountryCode(0).ToUpper();
					}
				}
//				Tools.LogInfo("GetToken/89","Address="+addr,logPriority,this);

				ret                 = 90;
				var customerService = new CustomerService();
				var customer        = customerService.Create(customerOptions);
//				customer.Currency   = payment.CurrencyCode.ToLower();
				customerId          = customer.Id;
				err                 = err + ", customerId="+Tools.NullToString(customerId);
				Tools.LogInfo("GetToken/90","Stripe.CustomerService.Create(), customerId="+Tools.NullToString(customerId),logPriority,this);

				ret                 = 100;
				strResult           = customer.StripeResponse.Content;
//				resultMsg           = Tools.JSONValue(strResult,"status");
				resultCode          = customer.StripeResponse.ToString();
				int k               = resultCode.ToUpper().IndexOf(" STATUS=");
				ret                 = 110;
				err                 = err + ", StripeResponse="+Tools.NullToString(resultCode);
				Tools.LogInfo("GetToken/110","strResult="+strResult+", err="+err,logPriority,this);

//	customer.StripeResponse.ToString() is as follows:
//	<Stripe.StripeResponse status=200 Request-Id=req_bI0B5glG6r6DNe Date=2021-05-28T09:35:23>

				if ( k > 0 )
				{
					resultCode = resultCode.Substring(k+8).Trim();
					k          = resultCode.IndexOf(" ");
					if ( k > 0 )
						resultCode = resultCode.Substring(0,k);
				}
				else
					resultCode = "999";

				ret                  = 120;
				err                  = err + ", strResult=" +Tools.NullToString(strResult)
				                           + ", resultCode="+Tools.NullToString(resultCode);
				customer             = null;
				customerService      = null;
				customerOptions      = null;
				paymentMethod        = null;
				paymentMethodService = null;
				paymentMethodOptions = null;
				token                = null;
				tokenService         = null;
				tokenOptions         = null;

				if ( resultCode.StartsWith("2") && payToken.Length > 0 && paymentMethodId.Length > 0 && customerId.Length > 0 )
				{
					resultMsg = "Success";
					ret       = 0;
					Tools.LogInfo ("GetToken/189","Ret=0"                 + err,logPriority,this);
				}
				else
					Tools.LogInfo ("GetToken/190","Ret=" + ret.ToString() + err,231,this);
			}
			catch (WebException ex1)
			{
				string exData = Tools.DecodeWebException(ex1,ClassName+".GetToken/191","ret="+ret.ToString()+err);
				Tools.LogInfo     ("GetToken/192",exData,231,this);
				Tools.LogException("GetToken/193",err,ex1,this);
			}
			catch (Exception ex2)
			{
				err = "Ret=" + ret.ToString() + err;
				Tools.LogInfo     ("GetToken/194",err,231,this);
				Tools.LogException("GetToken/195",err,ex2,this);

				err   = "";
				int k = 0;
				foreach (System.Collections.DictionaryEntry exPair in ex2.Data)
				{
					k++;
					err = err + "[" + k.ToString() + "] " + exPair.Key.ToString() + " = " + exPair.Value.ToString() + Environment.NewLine;
				}
				if ( err.Length > 0 )
				{
					Tools.LogInfo     ("GetToken/196",err,231,this);
					Tools.LogException("GetToken/197",err,ex2,this);
				}
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret     = 610;
			payRef      = "";
			strResult   = "";
			err         = "";
			resultMsg   = "Fail";
			resultCode  = "981";

//	Testing
			logPriority = 238;
//	Testing

/*
var chgOptions = new ChargeCreateOptions
{
    Amount = 2000,
    Currency = "usd",
    Source = "tok_visa",
};
chgOptions.AddExtraParam("ip", "62.132.141.1");
chgOptions.AddExtraParam(    "user_agent",    "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
chgOptions.AddExtraParam("referrer", "https://example.com/payment-page");
chgOptions.AddExtraParam("payment_user_agent", "Stripe Button");
var chgService = new ChargeService();
var chg        = chgService.Create(chgOptions);
*/

//			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
				ret                        = 620;
				StripeConfiguration.ApiKey = payment.ProviderPassword; // Secret key

//				Tools.LogInfo("TokenPayment/624","About to create Stripe.PaymentIntentCreateOptions",logPriority,this);
				ret                        = 624;
				err                        = err + ", customerId="      + Tools.NullToString(payment.CustomerID)
				                                 + ", paymentMethodId=" + Tools.NullToString(payment.PaymentMethodID)
				                                 + ", tokenId="         + Tools.NullToString(payment.CardToken);
				ret                        = 630;
				var paymentIntentOptions   = new PaymentIntentCreateOptions
				{
					Amount              = payment.PaymentAmount,
					Currency            = payment.CurrencyCode.ToLower(), // Must be "usd" not "USD"
					StatementDescriptor = payment.PaymentDescriptionLeft(22),
					Customer            = payment.CustomerID,
					PaymentMethod       = payment.PaymentMethodID,
					Description         = payment.MerchantReference,
					ConfirmationMethod  = "manual",
					Confirm             = true,
//					SetupFutureUsage    = "off_session",
//					PaymentMethodData   = new PaymentIntentPaymentMethodDataOptions
//					{
//						Type = "card"
//					},
				};

				if ( payment.MandateIPAddress.Length > 0 )
				{
					Tools.LogInfo("TokenPayment/633","Set up PaymentIntentMandateData (IP " + payment.MandateIPAddress + ", Browser " + payment.MandateBrowser + ")",222,this);
					ret                              = 633;
					paymentIntentOptions.MandateData = new PaymentIntentMandateDataOptions
					{
						CustomerAcceptance = new PaymentIntentMandateDataCustomerAcceptanceOptions
						{
							AcceptedAt = ( payment.MandateDateTime > Constants.DateNull ? payment.MandateDateTime : System.DateTime.Now.AddMinutes(-15) ),
							Type       = "online",
							Online     = new PaymentIntentMandateDataCustomerAcceptanceOnlineOptions
							{
								IpAddress = payment.MandateIPAddress,
								UserAgent = payment.MandateBrowser
							}
						}
					};
				}
				else
					Tools.LogInfo("TokenPayment/634","PaymentIntentMandateData not set up (no IP address)",222,this);

//	Create a separate mandate
//	Mandate created further below
//				string mandateId = "";
//				if ( payment.MandateDateTime > Constants.DateNull && payment.MandateIPAddress.Length > 2 )
//				{
//					ret         = 640;
//					var mandate = new Mandate
//					{
//						CustomerAcceptance = new MandateCustomerAcceptance
//						{
//							AcceptedAt = payment.MandateDateTime,
//							Online     = new MandateCustomerAcceptanceOnline
//							{
//								IpAddress = payment.MandateIPAddress,
//								UserAgent = payment.MandateBrowser
//							}
//						}
//					};
//					mandateId = mandate.Id;
//					err       = err + ", mandateId="+Tools.NullToString(mandateId);
//				}
//				else
//					err       = err + ", No mandate";

//				Tools.LogInfo("TokenPayment/690","About to create Stripe.PaymentIntentService",logPriority,this);
				ret                      = 690;
				var paymentIntentService = new PaymentIntentService();
				var paymentIntent        = paymentIntentService.Create(paymentIntentOptions);
				err                      = err + ", paymentIntentId="+Tools.NullToString(paymentIntent.Id);
				Tools.LogInfo("TokenPayment/690","Stripe.PaymentIntentService.Create(): "+err.Substring(2),logPriority,this);

//				Tools.LogInfo("TokenPayment/700","About to create Stripe.PaymentIntentConfirmOptions",logPriority,this);
				ret                = 700;
				var confirmOptions = new PaymentIntentConfirmOptions
				{
					PaymentMethod = payment.PaymentMethodID,
					OffSession    = true
//					Mandate       = mandateId
				};

//	Moved to PaymentIntentOptions (above)
//				if ( payment.MandateDateTime > Constants.DateNull && payment.MandateIPAddress.Length > 2 )
//				if ( payment.MandateIPAddress.Length > 0 || payment.MandateBrowser.Length > 0 )
//				{
//					Tools.LogInfo("TokenPayment/710","Set up PaymentIntentMandateData (IP " + payment.MandateIPAddress + ", Browser " + payment.MandateBrowser + ")",222,this);
//					ret                        = 710;
//					confirmOptions.MandateData = new PaymentIntentMandateDataOptions
//					{
//						CustomerAcceptance = new PaymentIntentMandateDataCustomerAcceptanceOptions
//						{
//							AcceptedAt = ( payment.MandateDateTime > Constants.DateNull ? payment.MandateDateTime : System.DateTime.Now.AddMinutes(-15) ),
//							Type       = "online",
//							Online     = new PaymentIntentMandateDataCustomerAcceptanceOnlineOptions
//							{
//								IpAddress = payment.MandateIPAddress,
//								UserAgent = payment.MandateBrowser
//							}
//						}
//					};
//				}
//				else
//					Tools.LogInfo("TokenPayment/711","PaymentIntentMandateData not set up (no IP address or browser info)",222,this);

				ret                = 720;
				var paymentConfirm = paymentIntentService.Confirm(paymentIntent.Id,confirmOptions);
				payRef             = paymentConfirm.Id;
				err                = err + ", paymentConfirmId="+Tools.NullToString(payRef);
				Tools.LogInfo("TokenPayment/720","Stripe.PaymentIntentService.Confirm(): "+err.Substring(2),logPriority,this);

				ret                = 730;
				strResult          = paymentConfirm.StripeResponse.Content;
				resultMsg          = Tools.JSONValue(strResult,"status");
				resultCode         = paymentConfirm.StripeResponse.ToString();
				ret                = 740;
				err                = err + ", StripeResponse="+Tools.NullToString(resultCode);

//	2023/12/06 : Don't look at the Stripe response. If there is a PayRef just assume it worked

				if ( payRef.Length > 0 )
				{
					ret             = 0;
					resultMsg       = "Succeeded";
					resultCode      = "00";
					Tools.LogInfo ("TokenPayment/744","Ret=0, StripeResponse=" + paymentConfirm.StripeResponse.ToString(),logPriority,this);
				}
				else
				{
					int k = resultCode.ToUpper().IndexOf(" STATUS=");
					if ( k > 0 )
					{
						ret        = 750;
						resultCode = resultCode.Substring(k+8).Trim();
						k          = resultCode.IndexOf(" ");
						if ( k > 0 )
							resultCode = resultCode.Substring(0,k);
					}
					else
						resultCode = "989";

					ret = 760;
					err = err + ", strResult=" +Tools.NullToString(strResult)
				             + ", resultCode="+Tools.NullToString(resultCode);

					if ( ! resultCode.StartsWith("2") )
					{
						ret        = 762;
						resultCode = ( resultMsg.Length > 0 ? resultMsg : "Fail" );
						Tools.LogInfo ("TokenPayment/762","Ret=" + ret.ToString() + err,231,this);
					}
//					else if ( resultMsg.ToUpper().StartsWith("SUCCE") || resultMsg.Length == 0 )
//					{
//						ret        = 0;
//						resultCode = "Success";
//						Tools.LogInfo ("TokenPayment/189","Ret=0" + err,logPriority,this);
//					}
					else
					{
						ret        = 765;
						resultCode = resultMsg;
						Tools.LogInfo ("TokenPayment/765","Ret=" + ret.ToString() + err,231,this);
					}
				}

//				mandate              = null;
				paymentIntentService = null;
				paymentIntent        = null;
				confirmOptions       = null;
				paymentConfirm       = null;
			}
			catch (StripeException ex1)
			{
				resultCode = ex1.Message;
				resultMsg  = "Fail (Stripe error)";
				err        = "Ret=" + ret.ToString() + err;
				Tools.LogInfo     ("TokenPayment/194","[StripeException] " + err,231,this);
				Tools.LogException("TokenPayment/195","[StripeException] " + err,ex1,this);

				err                = "Internal error with StripeException.StripeError";
				StripeError stpErr = ex1.StripeError;

				if ( stpErr == null )
					err = "StripeException.StripeError=NULL";
				else
					try
					{
						resultCode = stpErr.Code + " | " + stpErr.DeclineCode + " | " + stpErr.Type;
						resultMsg  = Tools.NullToString(stpErr.Error);
						if ( Tools.NullToString(stpErr.ErrorDescription).Length > 0 )
							resultMsg = resultMsg + ( resultMsg.Length > 0 ? " : " : "" ) + stpErr.ErrorDescription;
						if ( Tools.NullToString(stpErr.Message).Length > 0 )
							resultMsg = resultMsg + ( resultMsg.Length > 0 ? " (" : "" ) + stpErr.Message + ( resultMsg.Length > 0 ? ")" : "" );
						err = "StripeError.Type="             + Tools.NullToString(stpErr.Type)
					    + " | StripeError.Code="             + Tools.NullToString(stpErr.Code)
					    + " | StripeError.DeclineCode="      + Tools.NullToString(stpErr.DeclineCode)
					    + " | StripeError.Error="            + Tools.NullToString(stpErr.Error)
					    + " | StripeError.ErrorDescription=" + Tools.NullToString(stpErr.ErrorDescription)
					    + " | StripeError.Message="          + Tools.NullToString(stpErr.Message);
						if ( stpErr.StripeResponse != null )
							err = err + " | StripeError.StripeResponse=" + stpErr.StripeResponse.ToString();
						err = err + " | StripeError.ToString()="        + stpErr.ToString();
						ChargeOutcome stpOutcome = stpErr.PaymentIntent.Charges.Data[0].Outcome;
						err = err + " | StripeError.PaymentIntent.Charges.Data[0].Outcome.Type=" + Tools.NullToString(stpOutcome.Type);
						err = err + " | ...Reason="                                              + Tools.NullToString(stpOutcome.Reason);
						err = err + " | ...RiskLevel="                                           + Tools.NullToString(stpOutcome.RiskLevel);
						err = err + " | ...RiskScore="                                           + stpOutcome.RiskScore.ToString();
						err = err + " | ...SellerMessage="                                       + Tools.NullToString(stpOutcome.SellerMessage);
					}
					catch
					{
						err = err + " | StripeError.PaymentIntent.Charges.Data[0].Outcome=NULL";
					}

			//	From https://stripe.com/docs/error-handling#payment-errors
			//	if (e.StripeError.Type == "card_error")
			//	{
			//		if(e.StripeError.PaymentIntent.Charges.Data[0].Outcome.Type == "blocked")
			//			Console.WriteLine("Payment blocked for suspected fraud");
			//		else if (e.StripeError.Code == "card_declined")
			//			Console.WriteLine("Declined by the issuer");
			//		else if (e.StripeError.Code == "expired_card")
			//			Console.WriteLine("Card expired");
			//		else
			//			Console.WriteLine("Other card error");
			//	}

				Tools.LogInfo     ("TokenPayment/196",err,231,this);
				Tools.LogException("TokenPayment/197",err,ex1,this);
			}
			catch (Exception ex2)
			{
				if ( resultMsg.Length < 1 )
					resultMsg = "Fail (internal exception)";
				resultCode   = ex2.Message;
				err          = "Ret=" + ret.ToString() + err;
				Tools.LogInfo     ("TokenPayment/198",err,231,this);
				Tools.LogException("TokenPayment/199",err,ex2,this);
			}
			return ret;
		}

		public override int CardPayment(Payment payment)
		{
			int ret = 10;
			err     = "";

			try
			{
				Tools.LogInfo("CardPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("CardPayment/198","Ret="+ret.ToString(),255,this);
				Tools.LogException("CardPayment1/99","Ret="+ret.ToString(),ex ,this);
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
				Tools.LogException("TestService/199","",ex,this);
			}
			return 99040;
		}
		public override int ThreeDSecureCheck(string providerRef,string merchantRef="",string data1="",string data2="",string data3="")
		{
//	Return
//	   0     : Payment succeeded
//	   1-999 : Payment processed but declined or rejected
// 1001-    : Internal error

			int    ret = 10010;
			resultCode = "XX";
			resultMsg  = "Internal failure";
			err        = "";

			try
			{
				ret                        = 10020;
				StripeConfiguration.ApiKey = Tools.ProviderCredentials("Stripe","SecretKey");
				ret                        = 10030;
				var paymentIntentService   = new PaymentIntentService();
				ret                        = 10040;
			//	var paymentIntent          = paymentIntentService.Get(clientSecretId);
				var paymentIntent          = paymentIntentService.Get(providerRef);
				ret                        = 10050;
				resultCode                 = paymentIntent.Status;
				ret                        = 10060;
				if ( resultCode.ToUpper().StartsWith("SUCCE") )
				{
					resultMsg = "Payment successful";
					return 0;
				}
				resultMsg = "Payment NOT successful; further action required";
				return 37;
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecureCheck/198","Ret="+ret.ToString(),222,this);
				Tools.LogException("ThreeDSecureCheck/199","Ret="+ret.ToString(), ex,this);
			}
			return ret;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret = 10;
			string url = "";
			err        = "";

			try
			{
				StripeConfiguration.ApiKey = payment.ProviderPassword; // Secret key

				if ( postBackURL == null )
					url = Tools.ConfigValue("SystemURL");
				else
					url = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! url.EndsWith("/") )
					url = url + "/";
				d3Form = "";
				ret    = 20;
				url    = url + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				             +                    "&TransRef="+Tools.XMLSafe(payment.MerchantReference);

				ret                      = 50;
				var paymentMethodOptions = new PaymentMethodCreateOptions
				{
					Type = "card",
					Card = new PaymentMethodCardOptions
					{
						Number   = payment.CardNumber,
						ExpMonth = payment.CardExpiryMonth,
						ExpYear  = payment.CardExpiryYear,
						Cvc      = payment.CardCVV
					}
				};
				ret                      = 60;
				var paymentMethodService = new PaymentMethodService();
				var paymentMethod        = paymentMethodService.Create(paymentMethodOptions);
				err                      = err + ", paymentMethodId="+Tools.NullToString(paymentMethod.Id);

				ret                 = 70;
				var customerOptions = new CustomerCreateOptions
				{
					Name          = payment.CardName, // (payment.FirstName + " " + payment.LastName).Trim(),
					Email         = payment.EMail,
					Phone         = payment.PhoneCell,
					PaymentMethod = paymentMethod.Id
				};
				ret                  = 80;
				var customerService  = new CustomerService();
				var customer         = customerService.Create(customerOptions);
				err                  = err + ", customerId="+Tools.NullToString(customer.Id);

//				if ( payment.PaymentDescription.Length < 1 )
//					payment.PaymentDescription = "CareAssist";
//				else if ( payment.PaymentDescription.Length > 22 )
//					payment.PaymentDescription = payment.PaymentDescription.Substring(0,22);

//	Stripe needs a minimum payment of 50 US cents
				var paymentIntentOptions = new PaymentIntentCreateOptions
				{
					Amount                = 050,   // payment.PaymentAmount,
				//	Currency              = "usd", // payment.CurrencyCode.ToLower(), // Must be "usd" not "USD"
					Currency              = payment.CurrencyCode.ToLower(),
					StatementDescriptor   = payment.PaymentDescriptionLeft(22),
					Customer              = customer.Id,
					PaymentMethod         = paymentMethod.Id,
					Description           = payment.MerchantReference,
					ConfirmationMethod    = "manual"
				};
				ret                      = 40;
				var paymentIntentService = new PaymentIntentService();
				var paymentIntent        = paymentIntentService.Create(paymentIntentOptions);	
				err                      = err + ", paymentIntentId="+Tools.NullToString(paymentIntent.Id);

				ret                = 50;
				var confirmOptions = new PaymentIntentConfirmOptions
				{
					PaymentMethod   = paymentMethod.Id,
					ReturnUrl       = url
				};
				ret                = 60;
				var paymentConfirm = paymentIntentService.Confirm(paymentIntent.Id,confirmOptions);
				payRef             = paymentConfirm.Id;
				err                = err + ", paymentConfirmId="+Tools.NullToString(payRef);

				ret                = 70;
				strResult          = paymentConfirm.StripeResponse.Content;
				d3Form             = Tools.JSONValue(strResult,"url","next_action");
				d3Form             = Tools.JSONRaw(d3Form);
				resultMsg          = Tools.JSONValue(strResult,"status");
				ret                = 80;
				resultCode         = paymentConfirm.StripeResponse.ToString();
				int k              = resultCode.ToUpper().IndexOf(" STATUS=");
				err                = err + ", StripeResponse="+Tools.NullToString(resultCode);
				ret                = 90;

				Tools.LogInfo("ThreeDSecurePayment/60","strResult="+strResult,logPriority,this);

				string sql = "exec sp_WP_PaymentRegister3DSecA @ContractCode="    + Tools.DBString(payment.MerchantReference)
				           +                                 ",@ReferenceNumber=" + Tools.DBString(payRef)
				           +                                 ",@Status='77'"; // Means payment pending
				if ( languageCode.Length > 0 )
					sql = sql + ",@LanguageCode="        + Tools.DBString(languageCode);
				if ( languageDialectCode.Length > 0 )
					sql = sql + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
				using (MiscList mList = new MiscList())
					mList.ExecQuery(sql,0,"",false,true);

				Tools.LogInfo("ThreeDSecurePayment/80","PayRef=" + payRef + "; SQL=" + sql + "; " + d3Form,logPriority,this);
				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("ThreeDSecurePayment/99","Ret="+ret.ToString(),ex,this);
			}
			return ret;
		}
		public TransactionStripe(Constants.PaymentProvider provider) : base(provider)
		{
			base.LoadBureauDetails(provider);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			logPriority                           = 10;  // For production, when all is stable
//			logPriority                           = 222; // For testing/development, to log very detailed errors
			err                                   = "";
		}

//		public TransactionStripe() : base(Constants.PaymentProvider.StripeUSA)
//		{
//			err = "";
//		}
//		public TransactionStripe(string provider) : base()
//		{
//			base.LoadBureauDetails((Constants.PaymentProvider)Tools.StringToInt(provider));
//			err = "";
//		}
//		public TransactionStripe(Constants.PaymentProvider provider) : base()
//		{
//			base.LoadBureauDetails(provider);
//			err = "";
//		}
	}
}
