using System;
using System.Net;
using System.IO;
using System.Xml;

namespace PCIBusiness
{
	public class SMS : Message
	{
	//	Message details
		string phoneNumber;

	//	JSON template
	//	string json;

		public string PhoneNumber
		{
			get { return Tools.NullToString(phoneNumber); }
			set { phoneNumber = value.Trim().Replace(" ",""); }
		}
		public override string Recipient
		{
			get { return Tools.NullToString(phoneNumber); }
		}

		public override void Clear()
		{
			base.Clear();
			phoneNumber = "";
		}

		public override int Send(byte mode=0)
		{
			string resultData = "";
			int    ret        = 0;
			resultMsg         = "Missing/invalid phone number and/or message";

			if ( PhoneNumber.Length < 10 || MessageBody.Length < 3 )
				return 10;

			if ( provider == null )
				if ( mode == (byte)Constants.TransactionType.Test ) // Testing
				{
					provider             = new Provider();
					provider.BureauCode  = ((int)Constants.MessageProvider.ClickaTell).ToString().PadLeft(3,'0');
					provider.MerchantKey = "E8gSxksaQpmEDZ4OvabmlQ==";
					provider.BureauURL   = "https://platform.clickatell.com/messages/http/send";
				}
				else if ( LoadProvider() != 0 )
					return 15;

			try
			{
				ret            = 20;
				resultMsg      = "Missing/invalid message provider details";
				string sendURL = provider.BureauURL;

				Tools.LogInfo("Send/5","SMS to "+phoneNumber,Constants.LogSeverity,this);

				if ( ProviderCode == (int)Constants.MessageProvider.ClickaTell )
					sendURL = sendURL + "?apiKey="  + Tools.URLString(provider.MerchantKey)
				                     + "&to="      + Tools.URLString(phoneNumber)
				                     + "&content=" + Tools.URLString(messageBody);

				else if ( ProviderCode == (int)Constants.MessageProvider.GlobalSMS )
					sendURL = sendURL + "?key="      + Tools.URLString(provider.MerchantKey)
				                     + "&contacts=" + Tools.URLString(phoneNumber)
				                     + "&msg="      + Tools.URLString(messageBody)
				                     + "&senderid=" + Tools.URLString(provider.MerchantUserID)
				                     + "&type=text";
				else
					return 25;

				Tools.LogInfo("Send/88","Provider = " + ProviderCode.ToString() + ", sendURL = " + sendURL,Constants.LogSeverity,this);

				resultMsg                    = "";
				ret                          = 30;
				HttpWebRequest webReq        = (HttpWebRequest)WebRequest.Create(sendURL);
				ret                          = 60;
				HttpWebResponse webResponse  = (HttpWebResponse)webReq.GetResponse();
				ret                          = 70;
				using (StreamReader streamIn = new StreamReader(webResponse.GetResponseStream()))
					resultData = streamIn.ReadToEnd().Trim();

				string resultOK = "false";

				if ( ProviderCode == (int)Constants.MessageProvider.GlobalSMS )
				{
					resultOK     = "true";
					resultDetail = resultData;
					resultCode   = resultData;
					if ( resultCode.EndsWith("<br />") )
						resultCode = (" "+resultCode).Substring(0,resultCode.Length-5).Trim();
					if ( resultCode.StartsWith("<br />") )
						resultCode = (resultCode+" ").Substring(6).Trim();
				}
				else if ( ProviderCode == (int)Constants.MessageProvider.ClickaTell )
				{
					if ( resultData.Length > 0 && resultData.StartsWith("{") ) // JSON
					{
						resultID     = Tools.JSONValue(resultData,"apiMessageId");
						resultOK     = Tools.JSONValue(resultData,"accepted");
						resultCode   = Tools.JSONValue(resultData,"errorCode");
						resultDetail = Tools.JSONValue(resultData,"errorDescription");
						resultMsg    = Tools.JSONValue(resultData,"error");
					}
					else if ( resultData.Length > 0 && resultData.StartsWith("<") ) // XML
					{
						XmlDocument h = new XmlDocument();
						h.Load(resultData);
						resultID      = Tools.XMLNode(h,"apiMessageId");
						resultOK      = Tools.XMLNode(h,"accepted");
						resultCode    = Tools.XMLNode(h,"errorCode");
						resultDetail  = Tools.XMLNode(h,"errorDescription");
						resultMsg     = Tools.XMLNode(h,"error");
						h             = null;
					}
					if ( resultID.ToUpper()     == "NULL" ) resultID     = "";
					if ( resultCode.ToUpper()   == "NULL" ) resultCode   = "";
					if ( resultDetail.ToUpper() == "NULL" ) resultDetail = "";
					if ( resultMsg.ToUpper()    == "NULL" ) resultMsg    = "";
				}

				Tools.LogInfo("Send/199","SMS to "+phoneNumber + " ("+resultData+")",Constants.LogSeverity,this);

				if ( resultOK.ToUpper() == "TRUE" )
					return 0;

				return 199;
			}
			catch (Exception ex)
			{
				Tools.LogException("Send/301","ret="+ret.ToString()+", "+resultData+"",ex,this);
				resultMsg = ex.Message;
			}
			return ret;
		}

/*
		public int SendV1()
		{
			int ret = 10;
			resultMsg  = "Missing/invalid phone number and/or message";

			if ( PhoneNumber.Length < 10 || MessageBody.Length < 3 )
				return ret;

			try
			{
				HttpWebRequest webReq  = (HttpWebRequest)WebRequest.Create(providerAddress);
				ret                    = 20;
				resultMsg              = "(20) Internal error";
            webReq.ContentType     = "application/json";
            webReq.Method          = "POST";
            webReq.Accept          = "application/json";
            webReq.PreAuthenticate = true;
            webReq.Headers.Add("Authorization", providerPassword);
				ret                    = 30;
//				string data            = json.Replace("@PHONE@",PhoneNumber).Replace("@MSG@",MessageBody);
				string data            = "Not used";
				ret                    = 40;

				using (StreamWriter streamOut = new StreamWriter(webReq.GetRequestStream()))
				{
					ret = 50;
					streamOut.Write(data);
					streamOut.Flush();
					streamOut.Close();
				}

				ret                          = 60;
				HttpWebResponse webResponse  = (HttpWebResponse)webReq.GetResponse();
				ret                          = 70;

				using (StreamReader streamIn = new StreamReader(webResponse.GetResponseStream()))
					resultMsg = streamIn.ReadToEnd();

				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("SendV1","ret="+resultCode.ToString(),ex,this);
				resultMsg = ex.Message;
			}
			return ret;
		}
*/
//		public override byte LoadProvider()
//		{
//			providerAddress  = "";
//			providerUserName = "";
//			providerPassword = "";
//			
//			if ( ProviderCode == (int)Constants.SMSProvider.ClickaTell )
//			{
//				providerAddress  = "https://platform.clickatell.com/messages/http/send";
//				providerPassword = "E8gSxksaQpmEDZ4OvabmlQ==";
//			}
//			else if ( ProviderCode == (int)Constants.SMSProvider.GlobalSMS )
//			{
//			//	providerAddress  = "http://148.251.196.36/app/smsXmlApi";
//				providerAddress  = "http://148.251.196.36/app/smsapi/index.php";
//				providerUserName = "PCI";
//				providerPassword = "fuspQ2M6";
//			}
//			return 0;
//		}

		public SMS()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		//	providerAddress  = "https://platform.clickatell.com/messages/http/send";
		//	providerAddress  = "https://platform.clickatell.com/messages";
		//	providerAddress  = "https://platform.clickatell.com/v1/message";
		//	providerPassword = "-03qEod-S2KbSMLcooBm1w==";
		//	providerPassword = "E8gSxksaQpmEDZ4OvabmlQ==";
		//	json             = "{ #messages#: [{#channel#:#sms#,#to#:#@PHONE@#,#content#:#@MSG@#}] }";
		//	json             = json.Replace("#","\"");
		}
	}
}