using System;
using System.Net;
using System.Net.Mail;

namespace PCIBusiness
{
	public class Mail : Message
	{
		private SmtpClient  smtp;
		private MailMessage msg;

		private void AddMail(MailAddressCollection mailList,string mail)
		{
			try
			{
				mail = Tools.NullToString(mail).Replace(" ","").Replace(">","").Replace("<","");
				if ( mail.Length < 5 || ! mail.Contains("@") )
				{ }
				else if ( mail.Contains(",") || mail.Contains(";") )
				{
					mail          = mail.Replace(";",",");
					string[] mArr = mail.Split(',');
					for ( int k = 0; k < mArr.Length ; k++ )
						if ( mArr[k].Length > 5 )
							mailList.Add(mArr[k].Trim());
				}
				else // if ( Tools.CheckEMail(mail) )
					mailList.Add(mail);
			}
			catch
			{ }
		}

//		public  string Provider
//		{
//			get { return Tools.NullToString(provider); }
//			set { provider = value.Trim(); }
//		}

		public override string Recipient
		{
			get { return msg.To.ToString(); }
		}

		public  string BCC
		{
			set { AddMail(msg.Bcc,value); }
		}
		public  string CC
		{
			set { AddMail(msg.CC,value); }
		}
		public  string To
		{
			get
			{
				string x = "";
				if ( msg.To != null )
					foreach ( MailAddress add in msg.To )
						x = x + add.Address + ", ";
				if ( x.EndsWith(", ") )
					x = x.Substring(0,x.Length-2).Trim();
				return x;
			}
			set { AddMail(msg.To,value); }
		}
		public  string ReplyTo
		{
			set { AddMail(msg.ReplyToList,value); }
		}
		public  string From
		{
			set {	msg.From = new MailAddress(value); }
		}
//		public  string Sender
//		{
//			set {	msg.Sender = new MailAddress(value); }
//		}
		public  string Heading
		{
			get {	return Tools.NullToString(msg.Subject); }
			set {	msg.Subject = value.Trim(); }
		}
		public  string Body
		{
			set {	msg.Body = value.Trim(); }
		}

		public override int Send(byte mode=0)
		{
//	Mode  = 67 means load from Web.config
//	Mode != 67 means load from Provider on SQL

			int err    = 0;
			resultCode = "0";

			while ( err == 0 )
			{
				err = 10;
				if ( mode ==  0 && provider == null && LoadProvider() != 0 )
					break;

				err = 15;
				if ( mode == 67 && smtp == null && LoadConfig() != 0 )
					break;

				err = 20;
				if ( msg == null )
					break;

				err = 30;
				if ( mode == 0  && smtp == null )
					LoadProvider();

				err = 35;
				if ( mode == 67 && smtp == null )
					LoadConfig();

				err = 40;
				if ( smtp == null )
					break;

				err = 50;
				if ( msg.Body.Length < 10 )
					break;

				err = 60;
				if ( msg.To.Count < 1 )
					break;

//				if ( msg.Sender == null )
//					try
//					{
//						msg.Sender = new MailAddress(smtpSender);
//					//	msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-From"));
//					}
//					catch
//					{ }

//				if ( msg.Sender == null )
//					try
//					{
//						msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-User"));
//					}
//					catch
//					{ }

				err = 70;
				if ( msg.From == null )
					msg.From = msg.Sender;

				err = 80;
				msg.IsBodyHtml = msg.Body.ToUpper().Contains("<HTML");

				for ( int q = 1 ; q < 6 ; q++ ) // Try 5 times before quitting with an error
					try
					{
						err = 999;
						smtp.Send(msg);
						Tools.LogInfo("Send/5","(Success) Mail to "+msg.To.ToString(),Constants.LogSeverity,this);
						return 0;
					}
					catch (Exception ex)
					{
						if ( q > 3 ) // Log an error on the 4'th failed attempt
							Tools.LogException("Send/10","EMail failure (try " + q.ToString() + "), to " + msg.To.ToString(), ex, this);
					}

				break;
			}

			if ( err > 0 && err < 999 )
				Tools.LogException("Send/20","EMail failure (err=" + err.ToString() + "), to " + msg.To.ToString(), null, this);

			resultCode = err.ToString();
//			Tools.LogInfo("Send/25","(Failure (" + resultCode + ")) Mail to "+To,Constants.LogSeverity,this);
			return err;
		}

		public override void Clear()
		{
			base.Clear();
			msg.CC.Clear();
			msg.Bcc.Clear();
			msg.To.Clear();
			msg.ReplyToList.Clear();
//			msg.From    = null;
			msg.Subject = "";
			msg.Body    = "";
		}

		public byte LoadConfig()
		{
			byte ret = 10;

			try
			{
				int    ePort    = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));
				string eServer  = Tools.ConfigValue("SMTP-Server");
				string eUser    = Tools.ConfigValue("SMTP-User");
				string ePwd     = Tools.ConfigValue("SMTP-Password");
				string eFrom    = Tools.ConfigValue("SMTP-From");
				string eBCC     = Tools.ConfigValue("SMTP-BCC");
				string smtpData = eServer
				        + " / " + eUser
				        + " / " + ePwd
				        + " / " + ePort.ToString()
				        + " / " + eFrom
				        + " / " + eBCC;
				Tools.LogInfo("LoadConfig/10","SMTP Config ... " + smtpData,222,this);

				ret     = 20;
				smtp    = new SmtpClient(eServer);
				ret     = 30;
				if ( eFrom.Length > 5 )
					From = eFrom;
				else
					From = eUser;
				ret     = 40;
				BCC     = eBCC;
				ret     = 50;
				if ( ePort > 0 )
					smtp.Port = ePort;
				ret                        = 60;
				smtp.UseDefaultCredentials = false;
			//	smtp.EnableSsl             = false;
				smtp.Credentials           = new NetworkCredential(eUser,ePwd);
				ret                        = 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("LoadConfig/90","ret="+ret.ToString(),ex,this);
			}
			return ret;
		}

		public override byte LoadProvider()
		{
			byte ret = base.LoadProvider();

			if ( ret > 0 )
				return ret;

//			string smtpData = provider.BureauCode + " / " + provider.BureauType
//			                                      + " / " + provider.BureauURL
//			                                      + " / " + provider.MerchantUserID
//			                                      + " / " + provider.MerchantPassword
//			                                      + " / " + provider.Sender
//			                                      + " / " + provider.Port.ToString();

			if ( provider.BureauURL.Length > 5 && provider.MerchantUserID.Length > 5 )
				try
				{
//					Tools.LogInfo("LoadProvider/10","Provider ... " + smtpData,Constants.LogSeverity,this);
					msg.Sender = new MailAddress(provider.Sender);
					smtp       = new SmtpClient (provider.BureauURL);
					if ( provider.Port > 0 )
						smtp.Port = provider.Port;
					smtp.UseDefaultCredentials = false;
				//	smtp.EnableSsl             = false;
					smtp.Credentials           = new NetworkCredential(provider.MerchantUserID,provider.MerchantPassword);
				//	SendGrid testing
				//	For SendGrid, the NetworkCredential userName must equal "apikey" (the actual string "apikey")
					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("LoadProvider/30","Bureau "+provider.BureauCode,ex,this);
					return 30;
				}

			return 40;
		}

		public override void Close()
		{
			Clear();
			msg  = null;
			smtp = null;
		}

		public Mail()
		{
			msg = new MailMessage();

//			if ( Tools.ConfigValue("SMTP-From").Length > 5 )
//				msg.From   = new MailAddress  (Tools.ConfigValue("SMTP-From"));
//			if ( Tools.ConfigValue("SMTP-User").Length > 5 )
//				msg.Sender = new MailAddress  (Tools.ConfigValue("SMTP-User"));
//			if ( Tools.ConfigValue("SMTP-BCC").Length  > 5 )
//				BCC = Tools.ConfigValue("SMTP-BCC");
		}
	}
}