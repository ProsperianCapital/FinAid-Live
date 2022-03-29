using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace PCIBusiness
{
	public class TransactionPaymentCloud : Transaction
	{
//		This was originally called "TransactionAuthorizeNet"
//		It is actually the code for provider Authorize.Net, but Prosperian requested the name change
//		So the URLS are all "Authorize.net"

		byte   logPriority;
		string errorCode;
		string errorText;

		public  bool Successful
		{
			get { return (resultStatus.ToUpper() == "OK" && errorCode.Length == 0); }
		}

		public override int CardTest(Payment payment)
		{
			int ret     = 10;
			logPriority = 244; // Always log

			try
			{
				xmlSent = @"<createTransactionRequest xmlns='AnetApi/xml/v1/schema/AnetApiSchema.xsd'>
				              [AUTHENTICATION]
				              <refId>123456</refId>
				              <transactionRequest>
				                <transactionType>authCaptureTransaction</transactionType>
				                <amount>6.39</amount>
				                <payment>
				                  <creditCard>
				                    <cardNumber>5424000000000015</cardNumber>
				                    <expirationDate>2025-12</expirationDate>
				                    <cardCode>999</cardCode>
				                  </creditCard>
				                </payment>
				                <order>
				                  <invoiceNumber>INV-12345</invoiceNumber>
				                  <description>Product Description</description>
				                </order>
				                <lineItems>
				                  <lineItem>
				                    <itemId>1</itemId>
				                    <name>vase</name>
				                    <description>Bozo</description>
				                    <quantity>3</quantity>
				                    <unitPrice>2.13</unitPrice>
				                  </lineItem>
				                </lineItems>
				                <poNumber>456654</poNumber>
				                <customer>
				                  <id>99999456654</id>
				                </customer>
				                <billTo>
				                  <firstName>Ellen</firstName>
				                  <lastName>Johnson</lastName>
				                  <company>Souveniropolis</company>
				                  <address>14 Main Street</address>
				                  <city>Pecan Springs</city>
				                  <state>TX</state>
				                  <zip>44628</zip>
				                  <country>US</country>
				                </billTo>
				                <shipTo>
				                  <firstName>China</firstName>
				                  <lastName>Bayles</lastName>
				                  <company>Thyme for Tea</company>
				                  <address>12 Main Street</address>
				                  <city>Pecan Springs</city>
				                  <state>TX</state>
				                  <zip>44628</zip>
				                  <country>US</country>
				                </shipTo>
				                <customerIP>192.168.1.1</customerIP>
				                <processingOptions>
				                  <isSubsequentAuth>true</isSubsequentAuth>
				                </processingOptions>
				                <authorizationIndicatorType>
				                  <authorizationIndicator>final</authorizationIndicator>
				                </authorizationIndicatorType>
				              </transactionRequest>
				            </createTransactionRequest>";

				ret = 20;
				ret = CallWebService(payment,(byte)Constants.TransactionType.CardPayment);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("CardTest/98","Ret="+ret.ToString()+", XML Sent="+XMLSent,255,this);
				Tools.LogException("CardTest/99","Ret="+ret.ToString()+", XML Sent="+XMLSent,ex ,this);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int ret    = 10;
			payToken   = "";
			customerId = "";

			try
			{
				xmlSent  = "<createCustomerProfileRequest xmlns='AnetApi/xml/v1/schema/AnetApiSchema.xsd'>"
				         + "[AUTHENTICATION]" // replaced in CallWebService()
				         + "<profile>"
				         +   "<merchantCustomerId>" + payment.ContractCode + "</merchantCustomerId>"
				         +   "<description>" + (payment.FirstName + " " + payment.LastName).Trim() + "</description>"
				         +   "<email>" + payment.EMail + "</email>"
				         +   "<paymentProfiles>"
				         +     "<customerType>individual</customerType>"
				         +     "<payment>"
				         +       "<creditCard>"
				         +         "<cardNumber>" + payment.CardNumber + "</cardNumber>"
				         +         "<expirationDate>" + payment.CardExpiryYYYY + "-" + payment.CardExpiryMM + "</expirationDate>"
				         +       "</creditCard>"
				         +     "</payment>"
				         +   "</paymentProfiles>"
				         + "</profile>"
				         + "<validationMode>testMode</validationMode>"
				         + "</createCustomerProfileRequest>";
//	Always use "testMode"
//				         + "<validationMode>" + ( Tools.SystemIsLive() ? "liveMode" : "testMode" ) + "</validationMode>"
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				if ( ret == 0 )
				{
					ret        = 30;
					customerId = Tools.XMLNode(xmlResult,"customerProfileId");
					payToken   = Tools.XMLNode(xmlResult,"numericString","","","customerPaymentProfileIdList");
					Tools.LogInfo("GetToken/40","customerId=" + customerId + ", payToken=" + payToken,logPriority,this);
					if ( Successful && customerId.Length > 0 && payToken.Length > 0 )
						return 0;
				}
				if ( logPriority < 100 )
					Tools.LogInfo("GetToken/50","Ret=" + ret.ToString() + ", XML Sent="+xmlSent+", XML Rec="+XMLResult,201,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", XML Sent="+XMLSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", XML Sent="+XMLSent,ex ,this);
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

			try
			{
				xmlSent  = "<createTransactionRequest xmlns='AnetApi/xml/v1/schema/AnetApiSchema.xsd'>"
				         +   "[AUTHENTICATION]" // replaced in CallWebService()
				         +   "<refId>" + payment.MerchantReference + "</refId>"
				         +   "<transactionRequest>"
				         +     "<transactionType>authCaptureTransaction</transactionType>"
				         +     "<amount>" + payment.PaymentAmountDecimal + "</amount>"
				         +     "<currencyCode>" + payment.CurrencyCode + "</currencyCode>"
				         +     "<profile>"
				         +       "<customerProfileId>" + payment.CustomerID + "</customerProfileId>"
				         +       "<paymentProfile>"
				         +         "<paymentProfileId>" + payment.CardToken + "</paymentProfileId>"
				         +         "<cardCode>" + payment.CardCVV + "</cardCode>"
				         +       "</paymentProfile>"
				         +     "</profile>"
				         +     "<authorizationIndicatorType>"
				         +       "<authorizationIndicator>final</authorizationIndicator>"
				         +     "</authorizationIndicatorType>"
				         +   "</transactionRequest>"
				         + "</createTransactionRequest>";
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				if ( ret == 0 )
				{
					ret      = 30;
					otherRef = Tools.XMLNode(xmlResult,"authCode");
					payRef   = Tools.XMLNode(xmlResult,"transId");
					Tools.LogInfo("TokenPayment/40","payRef=" + payRef + ", otherRef=" + otherRef,logPriority,this);
					if ( Successful && otherRef.Length > 0 && payRef.Length > 0 )
						return 0;
				}
				if ( logPriority < 100 )
					Tools.LogInfo("TokenPayment/50","XML Sent="+XMLSent+", XML Rec="+XMLResult,201,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+XMLSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+XMLSent,ex ,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			int    ret     = 10;
			string url     = payment.ProviderURL;
			string authXML = "<merchantAuthentication>"
			               +   "<name>"           + payment.ProviderUserID + "</name>"
			               +   "<transactionKey>" + payment.ProviderKey    + "</transactionKey>"
			               + "</merchantAuthentication>";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret          = 60;
			xmlSent      = xmlSent.Replace("[AUTHENTICATION]",authXML);
			strResult    = "";
			resultStatus = "Error";
			resultCode   = "X98";
			resultMsg    = "(X98) Internal error connecting to " + url;
			errorCode    = resultCode;
			errorText    = resultMsg;
			ret          = 70;

			try
			{
				byte[]         page       = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType    = "text/xml"; // "application/json";
				webRequest.Accept         = "text/xml"; // "application/json";
				webRequest.Method         = "POST";
				ret                       = 90;

				Tools.LogInfo("CallWebService/20",
				              "Transaction Type=" + Tools.TransactionTypeName(transactionType) +
				            ", URL="              + url +
				            ", Auth/Key="         + payment.ProviderKey +
				            ", Auth/Name="        + payment.ProviderUserID +
				            ", XML Sent="         + xmlSent, logPriority, this);

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
						ret       = 140;
						strResult = rd.ReadToEnd();
					}
				}

				if ( strResult.Length == 0 )
				{
					ret       = 150;
					resultMsg = "No data returned from " + url;
					Tools.LogInfo("CallWebService/30","Failed, XML Rec=(empty)",199,this);
				}
				else
				{
					xmlResult = new XmlDocument();
					xmlResult.LoadXml(strResult.ToString());

					ret          = 160;
					resultStatus = Tools.XMLNode(xmlResult,"resultCode","","","messages");
					resultCode   = Tools.XMLNode(xmlResult,"code"      ,"","","message"); // NOT "messages"
					resultMsg    = Tools.XMLNode(xmlResult,"text"      ,"","","message"); // NOT "messages"
					errorCode    = Tools.XMLNode(xmlResult,"errorCode" ,"","","error");
					errorText    = Tools.XMLNode(xmlResult,"errorText" ,"","","error");
					ret          = 165;

					if ( strResult.ToUpper().Contains("<ERRORCODE>") && errorCode.Length < 1 )
					{
						ret   = 170;
						int k = strResult.ToUpper().IndexOf("<ERRORCODE>");
						int j = strResult.ToUpper().IndexOf("</ERRORCODE>");
						int h = ("<ERRORCODE>").Length;
						ret   = 175;
						if ( j > k + h )
							errorCode = strResult.Substring(k+h,j-(k+h));
					}

//					ret = 180;
//					if ( errorCode.Length < 1 )
//						errorCode = Tools.XMLNode(xmlResult,"code" ,"","","message");
//					ret = 185;
//					if ( errorText.Length < 1 )
//						errorText = Tools.XMLNode(xmlResult,"description" ,"","","message");
							
					ret = 0;

					Tools.LogInfo("CallWebService/60","Successful="   + Successful.ToString() +
			                                      ", resultStatus=" + resultStatus +
			                                      ", resultCode="   + resultCode +
			                                      ", resultMsg="    + resultMsg +
			                                      ", errorCode="    + errorCode +
			                                      ", errorText="    + errorText +
			                                      ", XML Rec="      + strResult,
					                                 ( Successful ? logPriority : (byte)244 ), this);
					if (Successful)
						resultCode = "1";

					else
					{
						ret        = 190;
						resultCode = resultStatus
						           + ( resultCode.Length == 0 ? "" : "/" + resultCode )
						           + ( errorCode.Length  == 0 ? "" : "/" + errorCode  );
					}
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

		public TransactionPaymentCloud() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.PaymentCloud);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
		//	logPriority                           = 233; // Testing, always log
			logPriority                           =  10; // Live, only log errors
		}
	}
}
