using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionWorldPay : Transaction
	{
		private byte   logPriority;

//		public  bool Successful
//		{
//			get { return Tools.JSONValue(strResult,"success").ToUpper() == "TRUE"; }
//		}

		public override int AccountUpdate(Payment payment)
		{
			int ret = 10;

//	Testing
//	//	payment.CardNumber = "4444333322221111";
//	Testing

			try
			{
				xmlSent = "<order orderCode='" + payment.TransactionID + "'>"
				        +   "<description>" + payment.PaymentDescription + "</description>"
				        +   "<amount currencyCode='" + payment.CurrencyCode + "'"
				        +          " exponent='2'"
				        +          " value='" + payment.PaymentAmount.ToString() + "' />"
				        +   "<paymentDetails action='ACCOUNTVERIFICATION'>"
				        +     "<CARD-SSL>"
				        +       "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +       "<expiryDate>"
				        +         "<date month='" + payment.CardExpiryMM + "' year='" + payment.CardExpiryYYYY + "' />"
				        +       "</expiryDate>"
				        +       "<cardHolderName>" + payment.CardName + "</cardHolderName>"
				        +       "<cvc>" + payment.CardCVV + "</cvc>"
				        +       CardAddress(payment)
				        +     "</CARD-SSL>"
				        +   "</paymentDetails>"
				        + "</order>";
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.ZeroValueCheck);
				if ( ret == 0 )
					return 0;

				Tools.LogInfo("AccountUpdate/50","ret="+ret.ToString()+", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("AccountUpdate/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("AccountUpdate/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
//				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

				if ( payment.SchemeTransactionID.Length < 1 )
				{
					ret = CardValidation(payment);
					if ( ret == 0 && payRef.Length > 0 )
						payment.SchemeTransactionID = payRef;
					else
					{
						if ( ret > 0 ) // XML gets logged in CardValidation()
							Tools.LogInfo("GetToken/20","ret="+ret.ToString()+", payRef=" + payRef,199,this);
						else           // XML not logged in CardValidation()
							Tools.LogInfo("GetToken/30","payRef=" + payRef + ", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
						return 20;
					}
				}

				ret          = 30;
				string descr = payment.PaymentDescription;
				if ( descr.Length < 1 )
					descr = "Recurring payment token";

				xmlSent = "<paymentTokenCreate>"
				        +   "<createToken tokenScope='merchant'>"
				        +     "<tokenEventReference>" + payment.MerchantReference + "</tokenEventReference>"
				        +     "<tokenReason>" + descr + "</tokenReason>"
				        +   "</createToken>"
				        +   "<paymentInstrument>"
				        +     "<cardDetails>"
				        +       "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +       "<expiryDate>"
				        +         "<date month='" + payment.CardExpiryMM + "' year='" + payment.CardExpiryYYYY + "' />"
				        +       "</expiryDate>"
				        +       "<cardHolderName>" + payment.CardName + "</cardHolderName>"
				        +     "</cardDetails>"
				        +   "</paymentInstrument>"
				        +   "<storedCredentials usage='FIRST'>"
				        +     "<schemeTransactionIdentifier>" + payment.SchemeTransactionID + "</schemeTransactionIdentifier>"
				        +   "</storedCredentials>"
				        + "</paymentTokenCreate>";

/*
<?xml version="1.0" encoding="UTF-8"?> 
<!DOCTYPE paymentService PUBLIC "-//WorldPay//DTD WorldPay PaymentService v1//EN"
 "http://dtd.worldpay.com/paymentService_v1.dtd">
<paymentService version="1.4" merchantCode="MYMERCHANT">
  <submit>
    <paymentTokenCreate> <!--used instead of order element-->
      <createToken tokenScope="merchant">
        <tokenEventReference>TOK7854321</tokenEventReference>
        <tokenReason>ClothesDepartment</tokenReason>
      </createToken>
      <paymentInstrument>
        <cardDetails>
          <cardNumber>4444333322221111</cardNumber>
          <expiryDate>
            <date month="06" year="2019" />
          </expiryDate>
          <cardHolderName>J. Shopper</cardHolderName>
          <cardAddress>
            <address>
              <address1>47A</address1>
              <address2>Queensbridge Road</address2>
              <address3>Suburbia</address3>
              <postalCode>CB94BQ</postalCode>
              <city>Cambridge</city>
              <state>Cambridgeshire</state>
              <countryCode>GB</countryCode>
            </address>
          </cardAddress>
        </cardDetails>
      </paymentInstrument>
    </paymentTokenCreate>
  </submit>
</paymentService>
*/
				ret      = 40;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				if ( ret == 0 && payToken.Length > 0 )
					return 0;

				Tools.LogInfo("GetToken/50","ret="+ret.ToString()+", payToken="+payToken+", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 10;
			payRef  = "";

//			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

			try
			{
				xmlSent = "<order orderCode='" + payment.TransactionID + "'>"
				        +   "<description>" + payment.PaymentDescription + "</description>"
				        +   "<amount currencyCode='" + payment.CurrencyCode + "'"
				        +          " exponent='2'"
				        +          " value='" + payment.PaymentAmount.ToString() + "' />"
				        +   "<paymentDetails>"
				        +     "<TOKEN-SSL tokenScope='merchant'>"
				        +       "<paymentTokenID>" + payment.CardToken + "</paymentTokenID>"
				        +       "<paymentInstrument>"
				        +         "<cardDetails>"
				        +           "<cvc>" + payment.CardCVV + "</cvc>"
				        +         "</cardDetails>"
				        +       "</paymentInstrument>"
				        +     "</TOKEN-SSL>"
				        +     "<storedCredentials usage='USED' merchantInitiatedReason='RECURRING'>"
				        +       "<schemeTransactionIdentifier>" + payment.SchemeTransactionID + "</schemeTransactionIdentifier>"
				        +     "</storedCredentials>"
				        +   "</paymentDetails>"
				        + "</order>";
/*
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE paymentService PUBLIC "-//WorldPay//DTD WorldPay PaymentService v1//EN" "http://dtd.worldpay.com/paymentService_v1.dtd">
<paymentService version="1.4" merchantCode="YOUR_MERCHANT_CODE"> 
  <submit>
    <order orderCode="YOUR_ORDER_CODE">
      <description>20 red roses from the MyMerchant webshop.</description>
      <amount currencyCode="GBP" exponent="2" value="5000"/>
      <paymentDetails>
        <TOKEN-SSL tokenScope="shopper"> 
          <paymentTokenID>efnhiuh7438rhf3hd9i3</paymentTokenID>
        </TOKEN-SSL>
        <session shopperIPAddress="123.123.123" id="0215ui8ib1" />
      </paymentDetails>
      <shopper>
        <shopperEmailAddress>jshopper@myprovider.int</shopperEmailAddress>
        <authenticatedShopperID>shopperID1234</authenticatedShopperID> <!--Mandatory for shopper tokens, don't send for merchant tokens-->
        <browser>
          <acceptHeader>text/html,application/xhtml+xml,application/xml ;q=0.9,* / *;q=0.8 </acceptHeader>
          <userAgentHeader>Mozilla/5.0 (Windows; U; Windows NT 5.1;en-GB; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729) </userAgentHeader>
        </browser>
      </shopper>
    </order>
  </submit>
</paymentService>
*/

				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				if ( ret == 0 && payRef.Length > 0 )
					return 0;

				Tools.LogInfo("TokenPayment/50","ret="+ret.ToString()+", payRef="+payRef+", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int CardValidation(Payment payment) // Also called Zero-Value Check
		{
			int ret = 10;

//	Testing
//	//	payment.CardNumber = "4444333322221111";
//	Testing

			try
			{
				xmlSent = "<order orderCode='" + payment.TransactionID + "'>"
				        +   "<description>" + payment.PaymentDescription + "</description>"
				        +   "<amount currencyCode='" + payment.CurrencyCode + "'"
				        +          " exponent='2'"
				        +          " value='" + payment.PaymentAmount.ToString() + "' />"
				        +   "<paymentDetails action='ACCOUNTVERIFICATION'>"
				        +     "<CARD-SSL>"
				        +       "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +       "<expiryDate>"
				        +         "<date month='" + payment.CardExpiryMM + "' year='" + payment.CardExpiryYYYY + "' />"
				        +       "</expiryDate>"
				        +       "<cardHolderName>" + payment.CardName + "</cardHolderName>"
				        +       "<cvc>" + payment.CardCVV + "</cvc>"
				        +       CardAddress(payment)
				        +     "</CARD-SSL>"
				        +   "</paymentDetails>"
				        + "</order>";
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.ZeroValueCheck);
				if ( ret == 0 )
					return 0;

				Tools.LogInfo("CardValidation/50","ret="+ret.ToString()+", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("CardValidation/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("CardValidation/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
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

//	Testing
//			url                      = "https://secure-test.worldpay.com/jsp/merchant/xml/paymentService.jsp";
//			string usr               = payment.ProviderUserID;
//			string pwd               = payment.ProviderPassword;
//			payment.ProviderUserID   = "2LHRK1HBEPDYVP9OKG8S";
//			payment.ProviderPassword = "st0nE#481";
//	Testing

			SetError ("99","Internal error connecting to " + url);

			string xmlOuter = "submit";
			if ( transactionType == (byte)Constants.TransactionType.DeleteToken )
				xmlOuter = "modify";

			ret         = 60;
			payToken    = "";
			payRef      = "";
			otherRef    = "";
			d3Form      = "";
			strResult   = "";
			resultCode  = "";
			resultMsg   = "";
			xmlSent     = "<?xml version='1.0' encoding='UTF-8'?>"
				         + "<!DOCTYPE paymentService PUBLIC"
				         +          " '-//WorldPay//DTD WorldPay PaymentService v1//EN'"
				         +          " 'http://dtd.worldpay.com/paymentService_v1.dtd'>"
				         + "<paymentService version='1.4' merchantCode='" + payment.ProviderAccount + "'>"
			            + "<"  + xmlOuter + ">"
			            + xmlSent
			            + "</" + xmlOuter + ">"
			            + "</paymentService>";

			try
			{
				byte[]         page                 = Encoding.UTF8.GetBytes(xmlSent);
				byte[]         authArray            = Encoding.ASCII.GetBytes(payment.ProviderUserID+":"+payment.ProviderPassword);
				string         auth64               = Convert.ToBase64String(authArray);
				HttpWebRequest webRequest           = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType              = "text/xml;charset=\"utf-8\"";
				webRequest.Accept                   = "text/xml";
				webRequest.Method                   = "POST";
				ret                                 = 80;
				webRequest.Headers["Authorization"] = "Basic " + auth64;
				ret                                 = 100;

				if ( transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck )
					webRequest.Headers["Cookie"]     = payment.Cookie;

				Tools.LogInfo("CallWebService/20",
				              "Transaction Type=" + Tools.TransactionTypeName(transactionType) +
				            ", URL=" + url +
				            ", Account=" + payment.ProviderAccount +
				            ", User=" + payment.ProviderUserID +
				            ", Pwd=" + payment.ProviderPassword +
				            ", Cookie=" + payment.Cookie +
				            ", Authorization=" + auth64 +
				            ", XML Sent=" + xmlSent, logPriority, this);

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
					ret = 125;
					if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
						foreach (string key in webResponse.Headers.AllKeys )
							if ( key.ToUpper().Contains("COOKIE") )
							{
								otherRef = webResponse.Headers[key];
								int  j   = otherRef.ToUpper().IndexOf("COOKIE:");
								if ( j  >= 0 )
									otherRef = otherRef.Substring(j+7).Trim();
								break;
							}

					ret = 130;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 140;
						strResult = rd.ReadToEnd();
						rd.Close();
					}
					webResponse.Close();
				}

				if ( strResult.Length == 0 )
				{
					ret = 150;
					SetError ("98","No data returned from " + url);
					Tools.LogInfo("CallWebService/30","XML Rec=(empty)",199,this);
				}

				else
					try
					{
						Tools.LogInfo("CallWebService/33", "XML Rec=" + strResult, logPriority, this);
						SetError ("97","Unable to read XML returned");

						ret       = 160;
						xmlResult = new XmlDocument();
						xmlResult.LoadXml(strResult);

						resultMsg           = "";
						resultCode          = "";
						string lastEventMsg = Tools.XMLNode(xmlResult,"lastEvent");

						if ( strResult.Contains("<error") )
						{
							ret        = 170;
							resultMsg  = xmlResult.SelectNodes("/paymentService/reply")[0].InnerText;
							resultCode = Tools.XMLNode(xmlResult,"error","","","","code");
							resultCode = "ERROR" + ( resultCode.Length > 0 ? "/" + resultCode : "" );
						}

						if ( strResult.Contains("<ISO8583ReturnCode") )
						{
							ret        = 180;
							resultCode = ( resultCode.Length > 0 ? resultCode + "/" : "" )
							           + Tools.XMLNode(xmlResult,"ISO8583ReturnCode","","","","code");
							resultMsg  = ( resultMsg.Length  > 0 ? resultMsg  + "/" : "" )
							           + Tools.XMLNode(xmlResult,"ISO8583ReturnCode","","","","description");
						}

						if ( resultMsg.Length == 0 && resultCode.Length == 0 )
						{
							ret        = 190;
							resultCode = lastEventMsg;
							resultMsg  = "";
						}
						else
						{
							ret = 200;
							if ( lastEventMsg.Length > 0 )
								resultMsg = ( resultMsg.Length > 0 ? resultMsg + " (" : "" )
							             + lastEventMsg
							             + ( resultMsg.Length > 0 ? ")" : "" );
							Tools.LogInfo("CallWebService/37", "resultCode=" + resultCode + ", resultMsg=" + resultMsg, logPriority, this);
							return ret;
						}
						
						if ( transactionType == (byte)Constants.TransactionType.GetToken )
						{
							ret      = 210;
							payToken = Tools.XMLNode(xmlResult,"paymentTokenID");
							payRef   = Tools.XMLNode(xmlResult,"transactionIdentifier","","","","",93);
							if ( payToken.Length > 0 )
								SetError ("00","");
							else
								SetError ("98","Unable to get token");
						}
						else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
						{
							ret    = 220;
							payRef = Tools.XMLNode(xmlResult,"transactionIdentifier","","","","",93); // Don't TRIM()
							if ( resultCode.ToUpper().StartsWith("AUTHORI") )
								SetError ("00",resultCode);
							else if ( resultCode.Length > 0 )
								SetError ("97","Payment failed (" + resultCode + ")");
							else
								SetError ("96","Payment failed");
						}
						else if ( transactionType == (byte)Constants.TransactionType.DeleteToken )
						{
							ret      = 225;
							payToken = Tools.XMLNode(xmlResult,"deleteTokenReceived","","","","paymentTokenID");
							if ( strResult.ToUpper().Contains("REPLY><OK") && payToken.Length > 0 )
								SetError ("Success","");
							else if ( resultCode.Length > 0 )
								SetError ("95","Delete token failed (" + resultCode + ")");
							else
								SetError ("94","Delete token failed");
						}
						else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
						{
							ret    = 230;
							payRef = Tools.XMLNode(xmlResult,"paRequest");
							d3Form = Tools.XMLNode(xmlResult,"issuerURL");
							if ( payRef.Length > 0 && otherRef.Length > 0 && d3Form.Length > 0 )
								SetError ("00",resultMsg);
							else if ( resultMsg.Length > 0 )
								SetError ("93","3d Secure Payment failed (" + resultMsg + ")");
							else
								SetError ("92","3d Secure Payment failed");
						}
						else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck )
						{
							ret       = 240;
							resultMsg = Tools.XMLNode(xmlResult,"ThreeDSecureResult","","","","description");
							if ( resultCode.ToUpper().StartsWith("AUTHORI") )
								if ( resultMsg.Length > 0 )
									SetError ("00",resultCode + " (" + resultMsg + ")");
								else
									SetError ("00",resultCode);
							else if ( resultMsg.Length > 0 )
								SetError ("91","3d Secure Payment failed : " + resultCode + " (" + resultMsg + ")");
							else
								SetError ("90","3d Secure Payment failed : " + resultCode);
						}
						else if ( transactionType == (byte)Constants.TransactionType.ZeroValueCheck )
						{
							ret    = 250;
							payRef = Tools.XMLNode(xmlResult,"transactionIdentifier","","","","",93); // Don't TRIM()
							if ( resultCode.ToUpper().StartsWith("AUTHORI") )
								SetError ("00",resultCode);
							else
							{
								Tools.LogInfo("CallWebService/39", "strResult="+strResult+", resultCode="+resultCode+", resultMsg="+resultMsg, logPriority, this);
								if ( resultMsg.Length < 1 )
									SetError ("89","Zero value validation failed : " + resultCode);
							}
						}
						else if ( transactionType == (byte)Constants.TransactionType.AccountUpdate )
						{
							ret    = 250;
						//	payRef = Tools.XMLNode(xmlResult,"transactionIdentifier","","","","",93); // Don't TRIM()
						//	if ( resultCode.ToUpper().StartsWith("AUTHORI") )
						//		SetError ("00",resultCode);
						//	else
						//	{
						//		Tools.LogInfo("CallWebService/39", "strResult="+strResult+", resultCode="+resultCode+", resultMsg="+resultMsg, logPriority, this);
						//		if ( resultMsg.Length < 1 )
						//			SetError ("89","Zero value validation failed : " + resultCode);
						//	}
						}

//	"payRef" is the WorldPay "schemeTransactionIdentifier", but it is saved in "paymentMethodId" because SP sp_Upd_CardTokenVault
//	has a parameter with this name and it expects it in this variable.
//	For WorldPay, "paymentMethodId" is eaxctly the same as "schemeTransactionID"
//	See Payment.GetToken()
						paymentMethodId     = payRef;
//						schemeTransactionId = payRef;
						if ( resultCode == "00" )
							ret = 0;
						else
							Tools.LogInfo("CallWebService/44", "resultCode=" + resultCode + ", resultMsg=" + resultMsg, logPriority, this);
					}
					catch (Exception ex3)
					{
						Tools.LogInfo     ("CallWebService/290","ret="+ret.ToString()+", "+strResult,220,this);
						Tools.LogException("CallWebService/291","ret="+ret.ToString()+", "+strResult,ex3,this);
						if ( resultMsg.Length == 0 )
							resultMsg  = "(81) Unable to read XML returned";
						if ( resultCode.Length == 0 )
							resultCode = "80";
						Tools.LogInfo("CallWebService/293", "resultCode=" + resultCode + ", resultMsg=" + resultMsg, logPriority, this);
					}
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

		public override int ThreeDSecureCheck(string providerRef,string merchantRef="",string data1="",string data2="",string data3="")
		{
			int ret = 10;
			try
			{
//				Tools.LogInfo("ThreeDSecureCheck/10","Merchant Ref=" + payment.MerchantReference,10,this);

				ret     = 40;
				xmlSent = "<order orderCode='" + merchantRef + "'>" // The order code from the first message 
				        +   "<info3DSecure>"
				        +     "<paResponse>" + providerRef + "</paResponse>" // PaRes
				        +   "</info3DSecure>"
				        +   "<session id='" + data1 + "' />" // The session id in the first message
				        + "</order>";

				using (Payment payment = new Payment())
				{
					ret                = 60;
					payment.BureauCode = Tools.BureauCode(Constants.PaymentProvider.WorldPay);
					payment.Cookie     = data2;
					ret                = CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecureCheck);
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("ThreeDSecureCheck/99","xmlSent=" + xmlSent,ex,this);
			}
			return ret;
		}

		public override int DeleteToken(Payment payment)
		{
			int ret = 10;

			try
			{
				string descr = payment.PaymentDescription;
				if ( descr.Length < 1 )
					descr = "Delete token";

				xmlSent  = "<paymentTokenDelete tokenScope='merchant'>"
				         +   "<paymentTokenID>" + payment.CardToken + "</paymentTokenID>"
				         +   "<tokenEventReference>" + payment.MerchantReference + "</tokenEventReference>"
				         +   "<tokenReason>" + descr + "</tokenReason>"
				         + "</paymentTokenDelete>";
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.DeleteToken);
				if ( ret == 0 && payToken.Length > 0 )
					return 0;

				Tools.LogInfo("DeleteToken/50","ret="+ret.ToString()+", payToken="+payToken+", XML Sent="+xmlSent+", XML Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("DeleteToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("DeleteToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex ,this);
			}
			return ret;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret       = 10;
			string urlReturn = "";

			try
			{
//				Tools.LogInfo("ThreeDSecurePayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

				if ( postBackURL == null )
					urlReturn = Tools.ConfigValue("SystemURL");
				else
					urlReturn = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! urlReturn.EndsWith("/") )
					urlReturn = urlReturn + "/";
				ret          = 20;
				urlReturn    = urlReturn + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				                         +                    "&TransRef="    +Tools.XMLSafe(payment.MerchantReference)
				                         +                    "&OrderCode="   +Tools.XMLSafe(payment.TransactionID)
				                         +                    "&SessionID="   +Tools.XMLSafe(payment.SessionIDClient);

				string descr = payment.PaymentDescription;
				if ( descr.Length < 1 )
					descr = "Initial 3d Secure payment";

				ret     = 40;
				xmlSent = "<order orderCode='" + payment.TransactionID + "'>"
				        +   "<description>" + descr + "</description>"
				        +   "<amount currencyCode='" + payment.CurrencyCode + "'"
				        +          " exponent='2'"
				        +          " value='" + payment.PaymentAmount.ToString() + "' />"
				        +   "<orderContent />"
				        +   "<paymentDetails>"
				        +     "<CARD-SSL>"
				        +       "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +       "<expiryDate>"
				        +         "<date month='" + payment.CardExpiryMM + "' year='" + payment.CardExpiryYYYY + "' />"
				        +       "</expiryDate>"
				        +       "<cardHolderName>3D</cardHolderName>" // + payment.CardName + "</cardHolderName>"
				        +       "<cvc>" + payment.CardCVV + "</cvc>"
				        +       CardAddress(payment)
				        +     "</CARD-SSL>"
				        +     "<session shopperIPAddress='" + payment.MandateIPAddress + "'"
				        +             " id='"               + payment.SessionIDClient + "' />" // Session id must be unique for each order
				        +   "</paymentDetails>"
				        +   "<shopper>"
				        +     "<shopperEmailAddress>" + payment.EMail + "</shopperEmailAddress>"
				        +     "<browser>"
				        +       "<acceptHeader>text/html</acceptHeader>"
				        +       "<userAgentHeader>" + payment.MandateBrowser + "</userAgentHeader>"
				        +     "</browser>"
				        +   "</shopper>"
				        +   "<additional3DSData dfReferenceId=\"" + payment.SessionIDProvider + "\" />"
				        + "</order>";

//				        +   "<dynamicInteractionType type='ECOMMERCE' />"
//				        +   "<dynamic3DS overrideAdvice='do3DS' />"

				ret     = 60;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecurePayment);

/* Sample
   <reply>
      <orderStatus orderCode="ExampleOrder1"> <!--The orderCode you supplied in the order-->
         <requestInfo>
            <request3DSecure> <!--PaRequest must be supplied as-is. Do not truncate-->
               <paRequest>eJxVUsFuwjAM/ZWK80aSUgpFJogNpHEo2hjTzlVr0Uq0KUkYsK+fUwpl72Q/2y/Jc2B2LvfeD2pTqGraE33e82YStrlGXHxietQoIUZjkh16RUYdQTge8DAII3846kl4n2/wIKFVkCTQ94HdUhrVaZ5UVkKSHl5Wayk6AGs5KFGvFo8lh+eu71qHOjHmpHQmhT8IhuFoDOxOQZWUKL+V3md1cvEWWCqTqxpYw0OqjpXVFzn0aeiWwFHvZW5tPWGsUtqqylilEZjjgXV3fz+6yJDOuchk/DsX8XZ3Wi+WPP6YP2IKzHVAlliUPhchHwnf4+FkGEz8CFjDQ1K6C8jl18YTT5yTD1cCanfO/JoIV3gkgJahsUovMnIvv2eA51pVSB1k/D2GDE0qLRrrkT2o6WxHAOve8vrmtpJasjYgDAg+4baapuDEC7JKRDxs1F0CzI2ydvWs/R4U/fs2f8B1wXg=</paRequest>
               <issuerURL><![CDATA[https://secure-test.worldpay.com/jsp/test/shopper/ThreeDResponseSimulator.jsp]]></issuerURL>
            </request3DSecure>
         </requestInfo>
         <echoData>1374244409987691395</echoData> <!--For compatibility with older integrations - can be ignored-->
      </orderStatus>
   </reply>
*/
				if ( ret == 0 && payRef.Length > 0 && otherRef.Length > 0 && d3Form.Length > 0 )
				{
					d3Form     = "<html><body onload='document.forms[\"frm3D\"].submit()'>"
					           + "<form name='frm3D' method='POST' action='" + d3Form + "'>"
					           +   "<input type='hidden' name='PaReq' value='" + payRef + "' />"
					           +   "<input type='hidden' name='TermUrl' value='" + urlReturn + "' />"
					           +   "<input type='hidden' name='MD' value='" + otherRef + "' />"
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
					Tools.LogInfo("ThreeDSecurePayment/50","PayReq=" + payRef + "; Cookie=" + otherRef + "; SQL=" + sql + "; " + d3Form,logPriority,this);
					return 0;
				}
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecurePayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("ThreeDSecurePayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent, ex,this);
			}
			return ret;
		}


		public int ThreeDSecurePaymentV1(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
//	No longer used
	
			int    ret       = 10;
			string urlReturn = "";

			try
			{
//				Tools.LogInfo("ThreeDSecurePayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

				if ( postBackURL == null )
					urlReturn = Tools.ConfigValue("SystemURL");
				else
					urlReturn = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! urlReturn.EndsWith("/") )
					urlReturn = urlReturn + "/";
				ret          = 20;
				urlReturn    = urlReturn + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				                         +                    "&TransRef="    +Tools.XMLSafe(payment.MerchantReference)
				                         +                    "&OrderCode="   +Tools.XMLSafe(payment.TransactionID)
				                         +                    "&SessionID="   +Tools.XMLSafe(payment.SessionIDClient);

				string descr = payment.PaymentDescription;
				if ( descr.Length < 1 )
					descr = "Initial 3d Secure payment";

				ret     = 40;
				xmlSent = "<order orderCode='" + payment.TransactionID + "'>"
				        +   "<description>" + descr + "</description>"
				        +   "<amount currencyCode='" + payment.CurrencyCode + "'"
				        +          " exponent='2'"
				        +          " value='" + payment.PaymentAmount.ToString() + "' />"
				        +   "<orderContent />"
				        +   "<paymentDetails>"
				        +     "<CARD-SSL>"
				        +       "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				        +       "<expiryDate>"
				        +         "<date month='" + payment.CardExpiryMM + "' year='" + payment.CardExpiryYYYY + "' />"
				        +       "</expiryDate>"
				        +       "<cardHolderName>3D</cardHolderName>" // + payment.CardName + "</cardHolderName>"
				        +       "<cvc>" + payment.CardCVV + "</cvc>"
				        +       CardAddress(payment)
				        +     "</CARD-SSL>"
				        +     "<session shopperIPAddress='" + payment.MandateIPAddress + "'"
				        +             " id='"               + payment.SessionIDClient + "' />" // Session id must be unique for each order
				        +   "</paymentDetails>"
				        +   "<shopper>"
				        +     "<shopperEmailAddress>" + payment.EMail + "</shopperEmailAddress>"
				        +     "<browser>"
				        +       "<acceptHeader>text/html</acceptHeader>"
				        +       "<userAgentHeader>" + payment.MandateBrowser + "</userAgentHeader>"
				        +     "</browser>"
				        +   "</shopper>"
				        + "</order>";

//				        +   "<dynamicInteractionType type='ECOMMERCE' />"
//				        +   "<dynamic3DS overrideAdvice='do3DS' />"

				ret     = 60;
				ret     = CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecurePayment);

/* Sample
   <reply>
      <orderStatus orderCode="ExampleOrder1"> <!--The orderCode you supplied in the order-->
         <requestInfo>
            <request3DSecure> <!--PaRequest must be supplied as-is. Do not truncate-->
               <paRequest>eJxVUsFuwjAM/ZWK80aSUgpFJogNpHEo2hjTzlVr0Uq0KUkYsK+fUwpl72Q/2y/Jc2B2LvfeD2pTqGraE33e82YStrlGXHxietQoIUZjkh16RUYdQTge8DAII3846kl4n2/wIKFVkCTQ94HdUhrVaZ5UVkKSHl5Wayk6AGs5KFGvFo8lh+eu71qHOjHmpHQmhT8IhuFoDOxOQZWUKL+V3md1cvEWWCqTqxpYw0OqjpXVFzn0aeiWwFHvZW5tPWGsUtqqylilEZjjgXV3fz+6yJDOuchk/DsX8XZ3Wi+WPP6YP2IKzHVAlliUPhchHwnf4+FkGEz8CFjDQ1K6C8jl18YTT5yTD1cCanfO/JoIV3gkgJahsUovMnIvv2eA51pVSB1k/D2GDE0qLRrrkT2o6WxHAOve8vrmtpJasjYgDAg+4baapuDEC7JKRDxs1F0CzI2ydvWs/R4U/fs2f8B1wXg=</paRequest>
               <issuerURL><![CDATA[https://secure-test.worldpay.com/jsp/test/shopper/ThreeDResponseSimulator.jsp]]></issuerURL>
            </request3DSecure>
         </requestInfo>
         <echoData>1374244409987691395</echoData> <!--For compatibility with older integrations - can be ignored-->
      </orderStatus>
   </reply>
*/
				if ( ret == 0 && payRef.Length > 0 && otherRef.Length > 0 && d3Form.Length > 0 )
				{
					d3Form     = "<html><body onload='document.forms[\"frm3D\"].submit()'>"
					           + "<form name='frm3D' method='POST' action='" + d3Form + "'>"
					           +   "<input type='hidden' name='PaReq' value='" + payRef + "' />"
					           +   "<input type='hidden' name='TermUrl' value='" + urlReturn + "' />"
					           +   "<input type='hidden' name='MD' value='" + otherRef + "' />"
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
					Tools.LogInfo("ThreeDSecurePayment/50","PayReq=" + payRef + "; Cookie=" + otherRef + "; SQL=" + sql + "; " + d3Form,logPriority,this);
					return 0;
				}
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecurePayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("ThreeDSecurePayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		private string CardAddress(Payment payment)
		{

/*
            <address>
              <address1>47A</address1>
              <address2>Queensbridge Road</address2>
              <address3>Suburbia</address3>
              <postalCode>CB94BQ</postalCode>
              <city>Cambridge</city>
              <state>Cambridgeshire</state>
              <countryCode>GB</countryCode>
            </address>
*/

			string addr = "";

			if ( payment.Address1(0).Length > 0 )
				addr = addr + "<address1>" + payment.Address1(0) + "</address1>";
			else
				addr = addr + "<address1>3A Bellpark Plaza, De Lange Street</address1>";

//			if ( payment.Address2(0).Length > 0 && payment.Address3(0).Length > 0 )
//				addr = addr + "<address2>" + payment.Address2(0) + "</address2>"
//				            + "<city>"     + payment.Address3(0) + "</city>";
//
//			else if ( payment.Address2(0).Length > 0 )
//				addr = addr + "<city>"     + payment.Address2(0) + "</city>";
//
//			else if ( payment.Address3(0).Length > 0 )
//				addr = addr + "<city>"     + payment.Address3(0) + "</city>";
//
//			else
//				addr = addr + "<city>Bellville</city>";

			if ( payment.PostalCode(0).Length > 0 )
				addr = addr + "<postalCode>" + payment.PostalCode(0) + "</postalCode>";
			else
				addr = addr + "<postalCode>7530</postalCode>";

			if ( payment.Address2(0).Length > 0 )
				addr = addr + "<city>" + payment.Address2(0) + "</city>";
			else
				addr = addr + "<city>Bellville</city>";

			if ( payment.State.Length > 0 )
				addr = addr + "<state>" + payment.State + "</state>";
			else
				addr = addr + "<state>Western Cape</state>";

			if ( payment.CountryCode(0).Length > 0 )
				addr = addr + "<countryCode>" + payment.CountryCode(0) + "</countryCode>";
			else
				addr = addr + "<countryCode>ZA</countryCode>";

			addr = "<cardAddress><address>" + addr + "</address></cardAddress>";

			return addr;
		}

		private void SetError(string eCode,string eMsg)
		{
			resultCode = eCode;
			if ( Tools.StringToInt(eCode) > 0 && eMsg.Length > 0 )
				resultMsg = "(" + eCode + ") " + eMsg;
			else
				resultMsg = eMsg;
		}

		public TransactionWorldPay() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.WorldPay);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			logPriority                          = 10;  // For production, when all is stable
//			logPriority                          = 222; // For testing/development, to log very detailed errors
		}
	}
}
