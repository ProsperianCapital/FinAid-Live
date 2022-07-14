using System;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.IO;

namespace PCIBusiness
{
	public class TransactionPayFast : Transaction
	{
		static string providerVersion = "2";

		public  bool   Successful
		{
			get
			{
				resultCode = Tools.NullToString(resultCode).ToUpper();
				return ( resultCode.Length > 1 && resultCode.Substring(0,1) == "A" );
			}
		}

		private int PostHTML(string url)
		{
			int    ret         = 10;
			string xmlReceived = "";
			payRef             = "";

			try
			{
				Tools.LogInfo("TransactionPayFast.PostHTML/10","URL=" + url + ", XML Sent=" + xmlSent,10);

			// Construct web request object
				ret = 20;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
//				webRequest.Headers.Add(@"SOAP:Action");
//				webRequest.ContentType    = "text/xml;charset=\"utf-8\"";
//				webRequest.Accept         = "text/xml";
				webRequest.ContentType    = "application/x-www-form-urlencoded;charset=\"utf-8\"";
				webRequest.Method         = "POST";
				webRequest.KeepAlive      = false;

				ret = 30;
//				byte[] page = Encoding.ASCII.GetBytes(xmlSent);
				byte[] page = Encoding.UTF8.GetBytes(xmlSent); // To handle unicode

			// Insert encoded HTML into web request
				ret = 40;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
//					stream.Flush();
					stream.Close();
				}

			// Get the XML response
				ret = 50;

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 60;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret         = 70;
						xmlReceived = rd.ReadToEnd();
						rd.Close();
					}
					webResponse.Close();
				}

				Tools.LogInfo("TransactionPayFast.PostHTML/70","XML Received=" + xmlReceived.ToString(),10);
				ret       = 80;
				xmlResult = new XmlDocument();
				xmlResult.LoadXml(xmlReceived.ToString());

//			//	Get data from result XML
				ret        = 90;
				resultCode = Tools.XMLNode(xmlResult,"status");
				resultMsg  = Tools.XMLNode(xmlResult,"message");

				if ( Successful )
					return 0;

				Tools.LogInfo("TransactionPayFast.SendXML/80","XML Sent=" + xmlSent+", XML Received=" + xmlReceived,220);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionPayFast.PostHTML/85","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionPayFast.PostHTML/90","Ret="+ret.ToString()+", URL=" + url + ", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
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

			//	Checksum (MD5)
				ret     = 40;
				xmlSent = xmlSent + "&signature=" + HashMD5(xmlSent);

				Tools.LogInfo("TransactionPayFast.GetToken/40","Post="+xmlSent+", Key="+payment.ProviderKey,30);

				ret      = PostHTML(payment.ProviderURL);
				payToken = Tools.XMLNode(xmlResult,"merchant_card_number");
				payRef   = Tools.XMLNode(xmlResult,"transaction_id");

				Tools.LogInfo("TransactionPayFast.GetToken/20","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,10);
			}
			catch (Exception ex)
			{
				Tools.LogException("TransactionPayFast.GetToken/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex);
			}
			return ret;
		}

		public override int CardPayment(Payment payment)
		{
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

			//	Checksum (MD5)
				ret     = 40;
				xmlSent = xmlSent + "&signature=" + HashMD5(xmlSent);

				Tools.LogInfo("TransactionPayFast.CardPayment/20","Post="+xmlSent+", Key="+payment.ProviderKey,30);

				ret     = PostHTML(payment.ProviderURL);
			}
			catch (Exception ex)
			{
				Tools.LogException("TransactionPayFast.CardPayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex);
			}
			return ret;
		}

		private string HashMD5(string data)
		{
			int           k;
			byte[]        bytes;
			StringBuilder hash = new StringBuilder();

			using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
				bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(data));

			for (k = 0; k < bytes.Length; k++)
				hash.Append(bytes[k].ToString("x2"));
	
			return hash.ToString();
		}

		public TransactionPayFast() : base()
		{
			bureauCode = Tools.BureauCode(Constants.PaymentProvider.PayFast);
		}
	}
}
