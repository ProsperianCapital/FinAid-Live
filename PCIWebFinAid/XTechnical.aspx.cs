using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class XTechnical : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;

			int  x  = ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral);
			if ( x != 0 && ! sessionGeneral.AdminUser )
				StartOver(10993);

//			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) != 0 )
//				StartOver(10994);
		}

		private string ReplacePassword(string connStr)
		{
			int j;
			int k = connStr.ToUpper().IndexOf("PWD=");
			while ( k >= 0 )
			{
				j = connStr.ToUpper().IndexOf(";",k+1);
				if ( j > k )
					connStr = connStr.Substring(0,k+4) + "******" + connStr.Substring(j);
				else
					connStr = connStr.Substring(0,k+4) + "******";
				k = connStr.ToUpper().IndexOf("PWD=",k+3);
			}
			k = connStr.ToUpper().IndexOf("UID=");
			while ( k >= 0 )
			{
				j = connStr.ToUpper().IndexOf(";",k+1);
				if ( j > k )
					connStr = connStr.Substring(0,k+4) + "******" + connStr.Substring(j);
				else
					connStr = connStr.Substring(0,k+4) + "******";
				k = connStr.ToUpper().IndexOf("UID=",k+3);
			}
			return connStr;
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			lblError.Visible   = true;
			lblError.Text      = "";
			lblResult.Text     = "";
			ascxXFooter.JSText = WebTools.JavaScriptSource("ShowData(4)");
			int action         = WebTools.ListValue(lstAction);

			if ( action > 99 && txtPwd.Text != ((int)PCIBusiness.Constants.SystemPassword.Technical).ToString() )
			{
				lblError.Text       = "Invalid password";
				ascxXFooter.JSText  = WebTools.JavaScriptSource("ShowData(9)");
				return;
			}

			string x1     = "";
			string x2     = "";
			string x3     = "";
			string tdBlue = "<td style='color:blue'>";

			if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ConfigNet )
			{
				x1 = "Server.MachineName<br />"
				   + "Server.MapPath('')<br />"
				   + "Server.MapPath('.')<br />"
				   + "Server.MapPath('/')<br />"
				   + "Environment.SystemDirectory<br />"
				   + "Environment.CurrentDirectory<br />"
				   + "Request.Url.Scheme<br />"
				   + "Request.Url.Authority<br />"
				   + "Request.Url.DnsSafeHost<br />"
				   + "Request.Url.Host<br />"
				   + "Request.Url.AbsoluteUri<br />"
				   + "Request.Url.AbsolutePath<br />"
				   + "Request.Url.LocalPath<br />"
				   + "Request.Url.PathAndQuery<br />"
				   + "Request.Url.GetLeftPart(UriPartial.Authority)<br />"
				   + "Request.UrlReferrer<br />"
				   + "Request.UserHostName<br />"
				   + "Request.RawUrl<br />"
				   + "Request.PhysicalApplicationPath";
				x2 = Server.MachineName + "<br />"
				   + Server.MapPath("") + "<br />"
				   + Server.MapPath(".") + "<br />"
				   + Server.MapPath("/") + "<br />"
				   + Environment.SystemDirectory.ToString() + "<br />"
				   + Environment.CurrentDirectory.ToString() + "<br />"
				   + Request.Url.Scheme + "<br />"
				   + Request.Url.Authority + "<br />"
				   + Request.Url.DnsSafeHost + "<br />"
				   + Request.Url.Host + "<br />"
				   + Request.Url.AbsoluteUri + "<br />"
				   + Request.Url.AbsolutePath + "<br />"
				   + Request.Url.LocalPath + "<br />"
				   + Request.Url.PathAndQuery + "<br />"
				   + Request.Url.GetLeftPart(UriPartial.Authority) + "<br />"
				   + Request.UrlReferrer + "<br />"
				   + Request.UserHostName + "<br />"
				   + Request.RawUrl + "<br />"
				   + Request.PhysicalApplicationPath;
				lblResult.Text = "<table><tr><td><u>Setting</u><br />" + x1 + "</td>" + tdBlue +"<u>Value</u><br />" + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ConfigApp )
			{
				System.Configuration.ConnectionStringSettings db1 = System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"];
				System.Configuration.ConnectionStringSettings db2 = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnTrade"];
				x1 = "AppDescription<br />"
				   + "LogFileErrors<br />"
				   + "LogFileInfo<br />"
				   + "Tools.SystemFolder('')<br />"
				   + "SystemPath<br />"
				   + "SystemMode<br />"
				   + "SMTP-Mode<br />"
				   + "SMTP-Server<br />"
				   + "SMTP-User<br />"
				   + "SMTP-From<br />"
				   + "SMTP-Password<br />"
				   + "SMTP-Port<br />"
				   + "SMTP-BCC<br />"
				   + "DBConn<br />"
				   + "DBConnTrade";
				x2 = PCIBusiness.Tools.ConfigValue("AppDescription") + "<br />"
				   + PCIBusiness.Tools.ConfigValue("LogFileErrors")  + "<br />"
				   + PCIBusiness.Tools.ConfigValue("LogFileInfo")    + "<br />"
				   + PCIBusiness.Tools.SystemFolder("")              + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SystemPath")     + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SystemMode")     + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-Mode")      + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-Server")    + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-User")      + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-From")      + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-Password")  + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-Port")      + "<br />"
				   + PCIBusiness.Tools.ConfigValue("SMTP-BCC")       + "<br />"
				   + ReplacePassword(db1.ConnectionString)           + "<br />"
				   + ReplacePassword(db2.ConnectionString);
				lblResult.Text = "<table><tr><td><u>Setting</u><br />" + x1 + "</td>" + tdBlue + "<u>Value</u><br />" + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ServerVariables )
			{
				foreach ( string sV in Request.ServerVariables )
				{
					x1 = x1 + sV + "<br />";
					x2 = x2 + Request.ServerVariables[sV] + "<br />";
				}
				lblResult.Text = "<table><tr><td><u>Server Variable</u><br />" + x1 + "</td>" + tdBlue + "<u>Value</u><br />" + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ConfigSoftware )
			{
				x1 = "Application name<br />"
				   + "Application code<br />"
				   + "Application version<br />"
				   + "Application release date<br />"
				   + "DLL version<br />"
				   + "DLL release date<br /><hr />"
				   + "Logged on user code<br />"
				   + "Logged on user name<br />"
				   + "Logged on user access<br /><hr />"
				   + "Machine name<br />"
				   + "Number of processors<br />"
				   + "System memory<br />"
				   + "Operating system version<br />"
				   + "ASP.NET version<br />"
				   + "Script timeout<br />"
				   + "Environment.UserName<br />"
				   + "Environment.UserDomainName<br />"
				   + "Database version<br />&nbsp;";
				if ( sessionGeneral == null )
					x3 = "<br /><br />";
				else
					x3 = sessionGeneral.UserCode + "<br />"
					   + sessionGeneral.UserName + "<br />"
					   + sessionGeneral.AccessType;
				x2 = SystemDetails.AppName + "<br />"
				   + ApplicationCode + "<br />"
				   + SystemDetails.AppVersion + "<br />"
				   + SystemDetails.AppDate + "<br />"
				   + PCIBusiness.SystemDetails.AppVersion + "<br />"
				   + PCIBusiness.SystemDetails.AppDate + "<br /><br />"
					+ x3 + "<br /><br />"
				   + Environment.MachineName + "<br />"
				   + Environment.ProcessorCount.ToString() + "<br />"
				   + Environment.WorkingSet.ToString() + " bytes<br />"
				   + Environment.OSVersion.ToString() + "<br />"
				   + Environment.Version.ToString() + "<br />"
				   + Server.ScriptTimeout.ToString() + "<br />"
				   + Environment.UserName.ToString() + "<br />"
				   + Environment.UserDomainName.ToString() + "<br />";

				PCIBusiness.DBConn conn = null;
				try
				{
					if ( PCIBusiness.Tools.OpenDB(ref conn) )
						if ( conn.Execute("select @@VERSION as Ver",true) )
						{
							x3 = conn.ColString("Ver");
							while ( x3.Length > 79 )
							{
								x2 = x2 + x3.Substring(0,80) + "<br />";
								x3 = x3.Substring(80);
							}
							x2 = x2 + x3;
						}
				}
				catch
				{
					x2 = x2 + "Microsoft SQL Server";
				}
				finally
				{
					PCIBusiness.Tools.CloseDB(ref conn);
					conn = null;
				}

				lblResult.Text = "<table><tr><td><u>Setting</u><br />" + x1 + "</td>" + tdBlue + "<u>Value</u><br />" + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.EMailSend && txtData.Text.Length > 0 )
				using (PCIBusiness.Mail mail = new PCIBusiness.Mail())
				{
					mail.LoadConfig();
					x1           = Request.Url.GetLeftPart(UriPartial.Authority);
					if ( x1.EndsWith("/") )
						x1        = x1.Substring(0,x1.Length-1);
					mail.To      = txtData.Text.Trim();
					mail.Heading = PCIBusiness.Tools.ConfigValue("AppDescription");
					if ( mail.Heading.Length < 1 )
						mail.Heading = "Prosperian Capital International";
					mail.Heading = mail.Heading + " : Test";
					mail.Body    = "<html><head><title>Test</title></head>"
					             + "<body><img src='" + x1 + "/Images/PCapital.png' title='Prosperian' style='float:right' />"
					             + "<p>Good day, this is a test message from " + SystemDetails.AppName + " user " + sessionGeneral.UserName
					             + " (" + sessionGeneral.UserCode + ").</p><table>"
					             + "<tr><td> <b>SMTP Server</b></td><td>: " + PCIBusiness.Tools.ConfigValue("SMTP-Server") + "</td></tr>"
					             + "<tr><td> <b>SMTP User</b></td><td>: "   + PCIBusiness.Tools.ConfigValue("SMTP-User") + "</td></tr>"
					             + "<tr><td> <b>SMTP From</b></td><td>: "   + PCIBusiness.Tools.ConfigValue("SMTP-From") + "</td></tr>"
					             + "<tr><td> <b>SMTP Port</b></td><td>: "   + PCIBusiness.Tools.ConfigValue("SMTP-Port") + "</td></tr>"
					             + "<tr><td> <b>BCC Address</b></td><td>: " + PCIBusiness.Tools.ConfigValue("SMTP-BCC") + "</td></tr>"
					             + "<tr><td> <b>Date/Time</b></td><td>: "   + PCIBusiness.Tools.DateToString(System.DateTime.Now,7,1) + "</td></tr></table>"
					             + "<p>Best regards"
					             + "<br />- " + SystemDetails.AppName
					             + "<br />&nbsp;&nbsp;Version " + SystemDetails.AppVersion
					             + "<br />&nbsp;&nbsp;(c) " + PCIBusiness.SystemDetails.Owner
					             + "<br />&nbsp;&nbsp;<a href='" + x1 + "'>" + x1 + "</a></p></body></html>";
					int k = mail.Send(67);
					if ( k == 0 )
						lblError.Text = "Email successfully sent to " + txtData.Text.Trim();
					else
						lblError.Text = "Email to " + txtData.Text.Trim() + " failed (error code " + k.ToString() + ")";
					PCIBusiness.Tools.LogException("XTechnical.btnOK_Click", "Send email : " + txtData.Text.Trim() + ", error code = "  + k.ToString() );
				}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ErrorLogWrite && txtData.Text.Length > 0 )
			{
				PCIBusiness.Tools.LogException("XTechnical.btnOK_Click","Diagnostic message : " + txtData.Text.Trim());
				lblResult.Text = "Message written to error log";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.InfoLogWrite && txtData.Text.Length > 0 )
			{
				PCIBusiness.Tools.LogInfo("XTechnical.btnOK_Click",txtData.Text.Trim() + " (Diagnostic message)");
				lblResult.Text = "Message written to information log";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.SQLStatus )
			{
				System.Configuration.ConnectionStringSettings db;
				PCIBusiness.DBConn                            conn = null;
				string                                        x4   = "<span style='color:red'>Failed</span>";

				try
				{
					db = System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"];
					x1 = "Connection<br />Read connection string<br />";
					x2 = "OK<br />OK<br />";
					x3 = "DBConn<br />" + db.ConnectionString;
					x1 = x1 + "Try to open DB<br />";
					if ( PCIBusiness.Tools.OpenDB(ref conn) )
					{
						x2 = x2 + "OK<br />";
						x1 = x1 + "select getdate()<br />";
						if ( conn.Execute("select getdate() as X",false) )
						{
							x3 = x3 + "<br /><br />" + conn.ColDate("X").ToString();
							x2 = x2 + "OK<br />";
//							x1 = x1 + "select count(*) from Contract<br />";
//							if ( conn.Execute("select count(*) as X from Membership",false) )
//							{
//								x3 = x3 + "<br />" + conn.ColLong("X").ToString();
//								x2 = x2 + "OK<br />";
								x1 = x1 + "exec sp_Check_BackOfficeUser 'xyz','xyz'<br />";
								if ( conn.Execute("exec sp_Check_BackOfficeUser 'xyz','xyz'",true) )
								{
									x2 = x2 + "OK<br />";
									x1 = x1 + "<br />All checks successful, database OK<br />";
								}
//							}
//							else
//								x2 = x2 + x4;
						}
						else
							x2 = x2 + x4;
					}
					else
						x2 = x2 + x4;
				}
				catch (Exception ex1)
				{
					x2 = x2 + x4;
					x3 = x3 + "<br /><br />" + ex1.Message;
				}
				finally
				{
					PCIBusiness.Tools.CloseDB(ref conn);
					conn = null;
				}

				try
				{
					x1 = x1 + "<br />";
					x2 = x2 + "<br /><br /><br />";
					x3 = x3 + "<br /><br /><br /><br /><br />";

					db = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnTrade"];
					x1 = x1 + "Connection<br />Read connection string<br />";
					x2 = x2 + "OK<br />OK<br />";
					x3 = x3 + "DBConnTrade<br />" + db.ConnectionString;
					x1 = x1 + "Try to open DB<br />";
					if ( PCIBusiness.Tools.OpenDB(ref conn) )
					{
						x2 = x2 + "OK<br />";
						x1 = x1 + "select getdate()<br />";
						if ( conn.Execute("select getdate() as X",false) )
						{
							x3 = x3 + "<br /><br />" + conn.ColDate("X").ToString();
							x2 = x2 + "OK<br />";
							x1 = x1 + "<br />All checks successful, database OK<br />";
						}
						else
							x2 = x2 + x4;
					}
					else
						x2 = x2 + x4;
				}
				catch (Exception ex2)
				{
					x2 = x2 + x4;
					x3 = x3 + "<br /><br />" + ex2.Message;
				}
				finally
				{
					PCIBusiness.Tools.CloseDB(ref conn);
					conn = null;
				}
				lblResult.Text = "<table><tr>"
				               + "<td><u>Check</u><br />" + x1 + "</td>"
				               + tdBlue + "<u>Result</u><br />" + x2 + "</td>"
				               + "<td><u>Value</u><br />" + ReplacePassword(x3) + "</td>"
				               + "</tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ErrorLogView ||
			          action == (byte)PCIBusiness.Constants.TechnicalQuery.InfoLogView )
			{
				DateTime fDate = System.DateTime.Now;
				if ( txtData.Text.Length > 0 )
					fDate = PCIBusiness.Tools.StringToDate(txtData.Text,7); // yyyy-mm-dd
				if ( fDate <= PCIBusiness.Constants.DateNull )
				{
					lblError.Text = "Invalid date";
					return;
				}
					
				StreamReader fHandle = null;
				string       fName   = PCIBusiness.Tools.LogFileName((action==(byte)PCIBusiness.Constants.TechnicalQuery.ErrorLogView?"LogFileErrors":"LogFileInfo"),fDate);

				try
				{
					fHandle = File.OpenText(fName);
					x1      = fHandle.ReadToEnd().Trim().Replace("<","&lt;").Replace(">","&gt;");
					x1      = x1.Replace(Environment.NewLine,"</p><p>");
					lblError.Text  = "Log File : " + fName;
					lblResult.Text = "<p>" + x1 + "</p>";
				}
				catch
				{
					lblError.Text = "Log file " + fName + " could not be found";
				}
				finally
				{
					if ( fHandle != null )
						fHandle.Close();
				}
				fHandle = null;
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.SQLObject )
			{
				lblError.Text = "Internal error interrogating SQL";
				x3            = txtData.Text.Trim().ToUpper();
				if ( x3.Length < 3 )
					return;

				PCIBusiness.DBConn conn = null;
				string             x4   = "";

				try
				{
					if ( ! PCIBusiness.Tools.OpenDB(ref conn) )
						lblError.Text = "Cannot connect to SQL database";

					else if ( ! conn.Execute("sp_help " + PCIBusiness.Tools.DBString(x3),false) )
						lblError.Text = "Invalid SQL object name";
					
					else
					{
						int k;
						for ( k = 0 ; k < conn.ColumnCount ; k++ )
						{
							x1 = x1 + conn.ColName(k) + "<br />";
							if ( conn.ColStatus("",k) == PCIBusiness.Constants.DBColumnStatus.ValueIsNull )
								x2 = x2 + "NULL<br />";
							else
								x2 = x2 + conn.ColValue(k) + "<br />";
						}
						if ( conn.NextResultSet() )
						{
							for ( k = 0 ; k < conn.ColumnCount ; k++ )
								x4 = x4 + "<td><u>" + conn.ColName(k) + "</u></td>";
							x4 = "<hr /><table><tr>" + x4 + "</tr>";
							while ( ! conn.EOF )
							{
								x4 = x4 + "<tr>";
								for ( k = 0 ; k < conn.ColumnCount ; k++ )
									if ( conn.ColStatus("",k) == PCIBusiness.Constants.DBColumnStatus.ValueIsNull )
										x4 = x4 + "<td>NULL</td>";
									else
										x4 = x4 + "<td>" + conn.ColValue(k) + "</td>";
								x4 = x4 + "</tr>";
								conn.NextRow();
							}
							x4 = x4 + "</table>";
						}
						lblError.Text = "";
					}
				}
				catch (Exception ex)
				{
					lblError.Text = ex.Message;
				}
				finally
				{
					PCIBusiness.Tools.CloseDB(ref conn);
					conn = null;
				}
				if ( x1.Length > 0 )
					lblResult.Text = "<table><tr>"
					               + "<td><u>Object</u><br />" + x1 + "</td>"
					               + "<td><u>Value</u><br />" + x2 + "</td>"
					               + "</tr></table>" + x4;
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.ClientDetails )
			{
				System.Web.HttpBrowserCapabilities bw   = Request.Browser;
				System.Web.HttpClientCertificate   cert = Request.ClientCertificate;
				x1 = "<u>Item</u><br />IP Address [HTTP_X_CLUSTER_CLIENT_IP]"
				              + "<br />IP Address [HTTP_X_FORWARDED_FOR]"
				              + "<br />IP Address [REMOTE_ADDR]"
				              + "<br />IP Address [Request.UserHostAddress]"
				              + "<br />IP Address ... best choice"
				              + "<br />Browser<br />Platform<br />Mobile Device?<br />- Make?<br />- Model?";
				x2 = "<u>Value</u><br />" + WebTools.ClientIPAddress(Request,1) + "<br />"
				                          + WebTools.ClientIPAddress(Request,2) + "<br />"
				                          + WebTools.ClientIPAddress(Request,3) + "<br />"
				                          + WebTools.ClientIPAddress(Request,4) + "<br />"
				                          + WebTools.ClientIPAddress(Request)   + "<br />"
				                          + bw.Browser + " " + bw.Version + " : " + hdnBrowser.Value.Trim() + "<br />"
				                          + bw.Platform + "<br />"
				                          + ( bw.IsMobileDevice ? "Yes" : "No" ) + "<br />"
				                          + ( bw.MobileDeviceManufacturer.ToUpper() == "UNKNOWN" ? "" : bw.MobileDeviceManufacturer ) + "<br />"
				                          + ( bw.MobileDeviceModel.ToUpper() == "UNKNOWN" ? "" : bw.MobileDeviceModel );
				lblResult.Text = "<table><tr><td>" + x1 + "</td>" + tdBlue + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.CertDetails )
			{
				int k;
				x2 = "OK<br />No";
				x3 = Request.Url.GetLeftPart(UriPartial.Authority).ToUpper();
				if ( x3.StartsWith("HTTP") )
				{
					k  = x3.IndexOf("://");
					x3 = x3.Substring(k+3);
				}
			
				txtData.Text = txtData.Text.Trim().Replace(" ","");

				if ( txtData.Text.Length < 1 )
					txtData.Text = Request.Url.GetLeftPart(UriPartial.Authority);

				if ( txtData.Text.ToUpper().Contains(x3) ) // Local URL
				{
					x1 = "<u>Item</u><br />URL<br />Status<br />Client Cert Name<br />Client Cert Issuer<br />Server Cert Name<br />Server Cert Issuer<br />HTTPS Cert Name<br />HTTPS Cert Issuer";
					x2 = "OK<br />" + Request.ServerVariables["CERT_SUBJECT"] + "<br />"
				                   + Request.ServerVariables["CERT_ISSUER"] + "<br />"
				                   + Request.ServerVariables["CERT_SERVER_SUBJECT"]  + "<br />"
				                   + Request.ServerVariables["CERT_SERVER_ISSUER"] + "<br />"
				                   + Request.ServerVariables["HTTPS_SERVER_SUBJECT"]  + "<br />"
				                   + Request.ServerVariables["HTTPS_SERVER_ISSUER"];
				}

				else
				{
					HttpWebRequest  req  = null;
					HttpWebResponse resp = null;
					k  = txtData.Text.ToUpper().IndexOf("://");
					x1 = "<u>Item</u><br />URL<br />Status<br />Certificate?<br />Name<br />Issuer<br />Expiry Date";

					if ( k >= 0 )
						txtData.Text = txtData.Text.Substring(k+3);

					try
					{
						req  = (HttpWebRequest)WebRequest.Create("https://" + txtData.Text);
						resp = (HttpWebResponse)req.GetResponse();
					}
					catch { }

					if ( req == null || resp == null )
						try
						{
							req  = (HttpWebRequest)WebRequest.Create("http://" + txtData.Text);
							resp = (HttpWebResponse)req.GetResponse();
						}
						catch { }

					if ( req != null && resp != null )
					{
						resp.Close();
						X509Certificate cert = req.ServicePoint.Certificate;

						if ( cert != null )
						{
							X509Certificate2 cert2 = new X509Certificate2(cert);
							x2 = "OK<br />Yes<br />"
							   + cert2.Subject + "<br />"
						      + cert2.Issuer + "<br />"
						      + cert2.GetExpirationDateString();
							cert2 = null;
						}
					}
					else
						x2 = "<span style='color:red'>Invalid URL</span>";
				}

				x2             = "<u>Value</u><br />" + txtData.Text + "<br />" + x2;
				lblResult.Text = "<table><tr><td>" + x1 + "</td>" + tdBlue + x2 + "</td></tr></table>";
			}

			else if ( action == (byte)PCIBusiness.Constants.TechnicalQuery.SQLExecute )
			{
				lblError.Text = "This SQL is not permitted";
				x3            = txtData.Text.Trim().ToUpper();

				if ( x3.Length < 3            ||
				     x3.Contains("TRUNCATE ") ||
				     x3.Contains("DELETE ")   ||
				     x3.Contains("INSERT ")   ||
				     x3.Contains("UPDATE ")   ||
				     x3.Contains("CREATE ")   ||
				     x3.Contains("ALTER ")    ||
				     x3.Contains("DROP ") )
					return;

				lblError.Text           = "";
				x1                      = "Internal SQL error";
				x3                      = txtData.Text.Trim();
				PCIBusiness.DBConn conn = null;

				try
				{
					if ( PCIBusiness.Tools.OpenDB(ref conn) )
						if ( conn.Execute(x3,false) )
						{
							x1 = "SQL execution successful. First row data:";
							for ( int k = 0 ; k < conn.ColumnCount ; k++ )
							{
								x1 = x1 + "<br />(Col " + k.ToString()
						         + ") Name = " + conn.ColName(k)
						         + ", Type = " + conn.ColDataType("",k)
						         + ", Value = ";
								if ( conn.ColStatus("",k) == PCIBusiness.Constants.DBColumnStatus.ValueIsNull )
									x1 = x1 + "NULL";
								else
									x1 = x1 + conn.ColValue(k);
							}
						}
						else
							x1 = "SQL execution failed";
					else
						x1 = "Cannot connect to SQL database";
				}
				catch (Exception ex)
				{
					x1 = x1 + "<br /><br />" + ex.Message;
				}
				finally
				{
					PCIBusiness.Tools.CloseDB(ref conn);
					conn = null;
				}
				lblResult.Text = x1;
			}
		}
	}
}