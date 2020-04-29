using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace PCIBusiness
{
	public class Mail : StdDisposable
	{
		private SmtpClient   smtp;
//		private StreamReader msgFile;
		private MailMessage  msg;
		private string       mailSender;

		public  string BCC
		{
			set
			{
				if ( ! string.IsNullOrWhiteSpace(value) )
					msg.Bcc.Add(value);
			}
		}

		public  string CC
		{
			set
			{
				if ( ! string.IsNullOrWhiteSpace(value) )
					msg.CC.Add(value);
			}
		}
		public  string To
		{
			set
			{
				if ( ! string.IsNullOrWhiteSpace(value) )
					msg.To.Add(value);
			}
		}
		public  string ReplyTo
		{
			set
			{
				if ( ! string.IsNullOrWhiteSpace(value) )
					msg.ReplyToList.Add(value);
			}
		}
		public  string From
		{
			set {	msg.From = new MailAddress(value); }
		}
		public  string Heading
		{
			set {	msg.Subject = value.Trim(); }
		}
		public  string Body
		{
			set {	msg.Body = value.Trim(); }
		}

		public byte Send()
		{
			if ( msg == null )
				return 10;
			if ( msg.Body.Length < 10 )
				return 20;
			if ( msg.To.Count < 1 )
				return 30;

			msg.IsBodyHtml = msg.Body.ToUpper().Contains("<HTML");

			for ( int q = 1 ; q < 6 ; q++ ) // Try 5 times before quitting with an error
				try
				{
					smtp.Send(msg);
					return 0;
				}
				catch (Exception ex)
				{
					if ( q >= 3 )
						Tools.LogException("Mail.Send/1","EMail failure (try " + q.ToString() + "), to " + msg.To.ToString(), ex);
				}

			return 90;
		}

		public void Clear()
		{
			msg.CC.Clear();
			msg.Bcc.Clear();
			msg.To.Clear();
			msg.ReplyToList.Clear();
//			msg.From    = null;
			msg.Subject = "";
			msg.Body    = "";
		}

		public override void Close()
		{
			Clear();
			msg  = null;
			smtp = null;
		}

		public Mail()
		{
			string smtpServer   = Tools.ConfigValue("SMTP-Server");
			string smtpUser     = Tools.ConfigValue("SMTP-User");
			string smtpPassword = Tools.ConfigValue("SMTP-Password");
			string smtpBCC      = Tools.ConfigValue("SMTP-BCC");
			int    smtpPort     = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));
			mailSender          = smtpUser;
			msg                 = new MailMessage();
			smtp                = new SmtpClient(smtpServer);

			if ( smtpPort > 0 )
				smtp.Port  = smtpPort;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials           = new NetworkCredential(smtpUser,smtpPassword);
		}
	}
}