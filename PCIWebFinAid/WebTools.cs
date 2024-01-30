using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public static class WebTools
	{
		static short debugMode = -888;

		public static string RequestValueString (HttpRequest req,string parmName,byte method=0)
		{
		//	Method = 0, Anything
		//	Method = 1, GET
		//	Method = 2, POST
			try
			{
				if ( method == (byte)PCIBusiness.Constants.HttpMethod.Post ) // Must be HTML form variables
					return PCIBusiness.Tools.ObjectToString(req.Form[parmName]);
				return PCIBusiness.Tools.ObjectToString(req[parmName]);
			}
			catch
			{ }
			return "";
		}

		public static int RequestValueInt (HttpRequest req,string parmName,int minValue=0,int maxValue=int.MaxValue)
		{
			try
			{
				return IntValue((req[parmName]).ToString().Trim(),minValue,maxValue);
			}
			catch
			{ }
			return 0;
		}

		public static string ViewStateString (System.Web.UI.StateBag viewState,string parmName)
		{
			try
			{
				return (viewState[parmName]).ToString().Trim();
			}
			catch
			{ }
			return "";
		}

		public static int ViewStateInt (System.Web.UI.StateBag viewState,string parmName,int minValue=0,int maxValue=int.MaxValue)
		{
			try
			{
				return IntValue((viewState[parmName]).ToString().Trim(),minValue,maxValue);
			}
			catch
			{ }
			return 0;
		}

		public static int IntValue(string value,int minValue,int maxValue)
		{
			try
			{
				int  tmp  = System.Convert.ToInt32(value);
				if ( tmp >= minValue && tmp <= maxValue )
					return tmp;
			}
			catch
			{ }
			return 0;
		}

		public static int ListAdd ( ListControl listBox,
		                            int         position,
                                  string      listValue,
                                  string      listText )
		{
			try
			{
				if ( position > listBox.Items.Count )
					position = listBox.Items.Count;
				else if ( position < 0 )
					position = 0;
				listBox.Items.Insert(position,listText);
				listBox.Items[position].Value = listValue;
				if ( listBox.GetType() == typeof(RadioButtonList) )
					listBox.Items[position].Attributes.Add("style","white-space:nowrap");
			}
			catch
			{
				return -1;
			}
			return position;
		}

		public static int ListValue ( ListControl listBox )
		{
			try
			{
				return System.Convert.ToInt32(ListValue(listBox,"0"));
			}
			catch
			{ }
			return 0;
		}

		public static string ListValue ( ListControl listBox,
		                                 string      defaultValue,
		                                 byte        codeColumn = 0 )
		{
			string sel = "";
			try
			{
				sel = listBox.SelectedValue;
				if ( sel == null || sel.Length < 1 )
					sel = defaultValue;
				else if ( codeColumn > 1 ) // Means the "code" contains more than 1 column and we we want the x'th one
				{
					string[] codes   = sel.Split('/');
					if ( codeColumn <= codes.Length )
						sel = codes[codeColumn-1];
				}
			}
			catch
			{
				sel = defaultValue;
			}
			return sel;
		}

		public static void ListSelect ( ListControl listBox,
		                                string      selectValue,
		                                string      defaultValue="0" )
		{
			bool   OK = false;
			string lValue;

			if ( ! selectValue.StartsWith("*/") ) // For lists with values like "3/120"
				try
				{
					listBox.SelectedValue = selectValue;
					OK = true;
				}
				catch { }

			if ( !OK )
				try
				{
					for ( int k = 0 ; k < listBox.Items.Count ; k++ )
					{
						lValue = listBox.Items[k].Value;
						if ( lValue == selectValue )
						{
							listBox.Items[k].Selected = true;
							OK = true;
							break;
						}
						else if ( selectValue.StartsWith("*/") &&
						          lValue.IndexOf("/") > 0 &&
						          selectValue.Substring(2) == lValue.Substring(lValue.IndexOf("/")+1) )
						{
							listBox.Items[k].Selected = true;
							OK = true;
							break;
						}
					}
				}
				catch { }

			if ( !OK && defaultValue.Length > 0 )
				try
				{
					listBox.SelectedValue = defaultValue;
				}
				catch { }
		}

		public static byte ListBind ( ListControl listBox,
		                              string      sql,
		                              object      dataSource,
		                              string      dataFieldKey,
		                              string      dataFieldShow,
		                              string      addZeroRow  = "",
		                              string      selectValue = "",
		                              short       selectIndex = -888 )
		{
			if ( PCIBusiness.Tools.NullToString(sql).Length > 0 )
				return ListBindMultiKey ( listBox,
				                          sql,
				                          new string[] {dataFieldKey},
				                          dataFieldShow,
				                          addZeroRow,
				                          selectValue,
				                          selectIndex );

			else if ( dataSource != null )
			{
				listBox.DataSource     = dataSource;
				listBox.DataValueField = dataFieldKey;
				listBox.DataTextField  = dataFieldShow;
				listBox.DataBind();
				return 0;
			}
			return 99;
		}

		public static byte ListBindMultiKey ( ListControl listBox,
		                                      string      sql,
		                                      string[]    dataFieldKey,
		                                      string      dataFieldShow,
		                                      string      addZeroRow  = "",
		                                      string      selectValue = "",
		                                      short       selectIndex = -888 )
		{
			listBox.Items.Clear();

			try
			{
				if ( PCIBusiness.Tools.NullToString(sql).Length < 5 )
					return 22;

				string dataValue;
				string keyValue;
				int    k;

				using (PCIBusiness.MiscList dList = new PCIBusiness.MiscList())
					if ( dList.ExecQuery(sql,0) == 0 )
						while ( ! dList.EOF )
						{
							if ( dataFieldKey.Length == 1 )
								keyValue = dList.GetColumn(dataFieldKey[0]);
							else
							{
								keyValue = "";
								for ( k = 0 ; k < dataFieldKey.Length ; k++ )
									keyValue = keyValue + "/" + dList.GetColumn(dataFieldKey[k]);
								if ( keyValue.StartsWith("/") )
									keyValue = keyValue.Substring(1);
							}
							dataValue = dList.GetColumn(dataFieldShow);
							listBox.Items.Add(new ListItem(dataValue,keyValue));
							dList.NextRow();
						}
					else
						return 5;

				if ( addZeroRow.Length > 0 )
					listBox.Items.Insert(0,(new ListItem(addZeroRow,"")));

				if ( listBox.Items.Count > 0 )
				{
					if ( selectValue.Length > 0 )
						ListSelect(listBox,selectValue,"0");
					else if ( selectIndex >= listBox.Items.Count )
						listBox.SelectedIndex = listBox.Items.Count - 1;
					else if ( selectIndex >= 0 )
						listBox.SelectedIndex = selectIndex;
				}
				return 0;
			}
			catch
			{ }

			return 99; 
		}

		public static void Redirect (HttpResponse response,string url)
		{
			try
			{
				if ( url.Length < 6 ) url = "Register.aspx";
				response.Redirect(url,false);
			}
			catch
			{ }
		}

		public static short DebugMode(System.Web.UI.Page webPage,bool load=false)
		{
			if ( load || debugMode < 0 )
				try
				{
					string g  = ((Literal)webPage.FindControl("DebugMode")).Text;
					debugMode = System.Convert.ToInt16(g);
					if ( debugMode < 0 )
						debugMode = 0;
				}
				catch
				{
					debugMode = 0;
				}

			return debugMode;
		}

		public static string JavaScriptSource(string newScript,string existingScript="",byte beforeOrAfter=2)
		{
			newScript = newScript.Trim();
			if ( newScript.Length < 1 && existingScript.Length < 1 )
				return "";
			else if ( ! existingScript.ToLower().Contains("<script") )
				existingScript = "<script type='text/javascript'>" + existingScript + "</script>";
//			if ( newScript.Length > 0 && beforeOrAfter == 2 ) // After
//				existingScript = existingScript.Replace("</script>",newScript + ( newScript.EndsWith(";") ? "" : ";" ) + "</script>");
			if ( newScript.Length > 0 && beforeOrAfter == 1 ) // Before
				existingScript = existingScript.Replace("script'>","script'>" + newScript + ( newScript.EndsWith(";") ? "" : ";"  ) );
			else if ( newScript.Length > 0 )
				existingScript = existingScript.Replace("</script>",newScript + ( newScript.EndsWith(";") ? "" : ";" ) + "</script>");
			return existingScript;
		}

//	Version 1
//		public static string ClientIPAddress(HttpRequest req)
//		{
//			string ipAddr = "";
//
//			for ( int k = 1 ; k < 5 ; k++ )
//				try
//				{
//					if      ( k == 1 )
//						ipAddr = req.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
//					else if ( k == 2 )
//						ipAddr = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
//					else if ( k == 3 )
//						ipAddr = req.ServerVariables["REMOTE_ADDR"];
//					else if ( k == 4 )
//						ipAddr = req.UserHostAddress;
//					if ( !string.IsNullOrEmpty(ipAddr) )
//						break;
//				}
//				catch
//				{
//					ipAddr = "";
//				}
//
//			if ( string.IsNullOrWhiteSpace(ipAddr) )
//				ipAddr = "";
//			else if ( ipAddr.Contains(",") )
//				ipAddr = ipAddr.Split(',')[0];
//
//			return ipAddr;
//		}

//	Version 2
		public static string ClientIPAddress(HttpRequest req,byte mode=0)
		{
			string ipList = req.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
			if ( string.IsNullOrWhiteSpace(ipList) )
			{
				ipList = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if ( string.IsNullOrWhiteSpace(ipList) )
				{
					ipList = req.ServerVariables["REMOTE_ADDR"];
					if ( string.IsNullOrWhiteSpace(ipList) )
						ipList = req.UserHostAddress;
				}
			}
			if ( string.IsNullOrWhiteSpace(ipList) )
				return "";

			if ( ipList.StartsWith("::") ) // Typically "::1" on a local machine
				return ( mode == 2 ? "127.0.0.1" : "localhost" );

			if ( ipList.Contains(",") )
				ipList = ipList.Split(',')[0];

			if ( mode == 1 && ipList.IndexOf(":") > 0 )
				return ipList.Substring(0,ipList.IndexOf(":"));

			if ( ipList.ToUpper() == "LOCALHOST" && mode == 2 )
				return "127.0.0.1";

			return ipList;
		}

		public static string ClientBrowser(HttpRequest req,string otherInfo="")
		{
			HttpBrowserCapabilities bc = req.Browser;
			string                  h  = bc.Browser + " " + bc.Version + " (" + bc.Platform + ")";
			otherInfo                  = otherInfo.Trim();
			if ( otherInfo.Length > 0 )
				h = h + " : " + otherInfo;
			return h;
		}

		public static string ClientReferringURL(HttpRequest req,byte logInfo=0,string logSource="")
		{
			string refer = PCIBusiness.Tools.ObjectToString(req.UrlReferrer);
			if ( refer.Length < 5 )
				refer = PCIBusiness.Tools.ObjectToString(req.Headers["Referer"]); // Yes, this is spelt CORRECTLY! Do not change

			if ( logInfo > 0 )
			{
				if ( logSource.Length < 1 )
					logSource = req.Url.AbsoluteUri;
				PCIBusiness.Tools.LogInfo ( "WebTools.ClientReferringURL", logSource + " (" + logInfo.ToString() + "), Referring URL=" + refer );
			}
			return refer;
		}

		public static string DecodeWebException(System.Net.WebException ex)
		{
			try
			{
				System.Net.HttpWebResponse errorResponse = ex.Response as System.Net.HttpWebResponse;
				if ( errorResponse == null )
					return "";

				string responseContent = "";
				int    k               = 0;

				using ( StreamReader sR = new StreamReader(errorResponse.GetResponseStream()) )
					responseContent = sR.ReadToEnd();

				responseContent = responseContent + Environment.NewLine + Environment.NewLine;
				foreach (string key in errorResponse.Headers.AllKeys )
					responseContent = responseContent + "[" + (k++).ToString() + "] " + key + " : " + errorResponse.Headers[key] + Environment.NewLine;

				return responseContent;
			}
			catch
			{ }
			return "";
		}

		public static byte CheckProductProvider(string productCode,string urlOld,HttpRequest req=null,HttpResponse resp=null)
		{
			if ( string.IsNullOrWhiteSpace(productCode) || PCIBusiness.Tools.SystemLiveTestOrDev() == PCIBusiness.Constants.SystemMode.Development )
				return 10;

			string urlNew     = "";
			string sql        = "exec sp_WP_Get_ProductTokenBureau @ProductCode=" + PCIBusiness.Tools.DBString(productCode);
			int    bureauCode = 0;

			using (PCIBusiness.MiscList mList = new PCIBusiness.MiscList())
				if ( mList.ExecQuery(sql,0) == 0 )
					bureauCode = PCIBusiness.Tools.StringToInt(mList.GetColumn("TokenBureauCode"));

			if ( bureauCode < 1 )
				return 20;
			else if ( bureauCode  == (int)PCIBusiness.Constants.PaymentProvider.TokenEx ||			
			          bureauCode  == (int)PCIBusiness.Constants.PaymentProvider.CyberSource )				
				urlNew = "RegisterEx3.aspx";
			else
				urlNew = "Register.aspx";

			if ( urlOld.ToUpper() == urlNew.ToUpper() )
				return 30;
			if ( req == null || resp == null )
				return 40;
				
			string parms = req.Url.Query;
			try
			{
				if ( parms.Length == 0 )
					resp.Redirect(urlNew);
				else if ( parms.StartsWith("?") )
					resp.Redirect(urlNew+parms);
				else
					resp.Redirect(urlNew+"?"+parms);
			}
			catch
			{ }
			return 0;
		}
		public static byte ReplaceControlText(Page webPage,string ctlID,string blocked,string fieldValue,string fieldURL,Control subCtl1=null,Control subCtl2=null)
		{
			Control ctl;

			fieldValue = fieldValue.Replace(Environment.NewLine,"<br />").Replace("\r\n","<br />").Replace("[BR]","<br />").Replace("<br>","<br />");
			if ( fieldValue.Contains("[") )
				fieldValue = fieldValue.Replace("[B]","<b>").Replace("[I]","<i>").Replace("[U]","<u>").Replace("[/B]","</b>").Replace("[/I]","</i>").Replace("[/U]","</u>");

			for ( int k = 1 ; k < 4 ; k++ )
			{
				ctl = null;
				if ( k == 1 && webPage != null )
					ctl = webPage.FindControl(ctlID);
				else if ( k == 2 && subCtl1 != null )
					ctl = subCtl1.FindControl(ctlID);
				else if ( k == 3 && subCtl2 != null )
					ctl = subCtl2.FindControl(ctlID);

				if ( ctl == null )
				{ }
				else if ( blocked == "1" || blocked == "B" )
				{
					ctl.Visible = false;
					if ( k == 1 && webPage != null && ctlID.StartsWith("X") )
					{
						ctl = webPage.FindControl("T"+ctlID.Substring(1));
						if ( ctl != null )
							ctl.Visible = false;
						ctl = webPage.FindControl("D"+ctlID.Substring(1));
						if ( ctl != null )
							ctl.Visible = false;
					}
				}
				else if (ctl.GetType()  == typeof(Literal))
					((Literal)ctl).Text   = fieldValue;
				else if (ctl.GetType()  == typeof(Label))
					((Label)ctl).Text     = fieldValue;
				else if (ctl.GetType()  == typeof(TableCell))
					((TableCell)ctl).Text = fieldValue;
				else if (ctl.GetType()  == typeof(CheckBox))
					((CheckBox)ctl).Text  = fieldValue;
				else if (ctl.GetType()  == typeof(Button))
				{
					Button btn            = (Button)ctl;
					btn.Text              = fieldValue;
					if ( fieldURL.Length  > 0 )
						btn.OnClientClick  = "JavaScript:location.href='" + fieldURL + "';return false";
				}
				else if (ctl.GetType()  == typeof(HyperLink))
				{
					HyperLink lnk         = (HyperLink)ctl;
					lnk.Text              = fieldValue;
					if ( fieldURL.Length  > 0 )
						lnk.NavigateUrl    = fieldURL;
				}
				else if (ctl.GetType()  == typeof(TextBox))
					((TextBox)ctl).Attributes.Add("placeholder",fieldValue);

				if ( k == 1 && subCtl1 == null ) // Main page check, first sub control is NULL
					break;
				if ( k == 2 && subCtl2 == null ) // First sub control check, second sub control is NULL
					break;
			}
			return 0;
		}

		public static byte LoadProductFromURL(HttpRequest req,
		                                  ref string      countryCode,
		                                  ref string      productCode,
		                                  ref string      languageCode,
		                                  ref string      languageDialectCode,
		                                      bool        checkURLParms = false)
		{
			byte   ret          = 10;
			string sql          = "";
			countryCode         = "";
			productCode         = "";
			languageCode        = "";
			languageDialectCode = "";

			using (PCIBusiness.MiscList mList = new PCIBusiness.MiscList())
				try
				{
					ret             = 20;
					string pageName = System.IO.Path.GetFileName(req.Url.AbsolutePath);
					string refer    = req.Url.AbsoluteUri.Trim();
					int    k        = refer.IndexOf("://");
					refer           = refer.Substring(k+3);

					ret = 30;
					k   = refer.IndexOf(".");
					if ( k > 0 )
						countryCode = refer.Substring(0,k).ToUpper();
					ret            = 40;
					if ( countryCode.Length < 2 || countryCode.StartsWith("WWW") || countryCode.StartsWith("LOCALHOST") )
						countryCode = "ZA";
					else if ( countryCode.Length > 2 )
						countryCode = countryCode.Substring(0,2);

					if ( ! pageName.StartsWith("/") )
						pageName = "/" + pageName;

					ret = 50;
					k   = refer.ToUpper().IndexOf(pageName.ToUpper());
					if ( k > 0 )
						refer = refer.Substring(0,k);

					ret = 60;
					sql = "exec sp_WP_Get_WebsiteInfoByURL " + PCIBusiness.Tools.DBString(refer);
					if ( mList.ExecQuery(sql,0) != 0 )
						PCIBusiness.Tools.LogInfo("WebTools.LoadProductFromURL/3","SQL failed: " + sql,229);
					else if ( mList.EOF )
						PCIBusiness.Tools.LogInfo("WebTools.LoadProductFromURL/6","SQL returned no data: " + sql,229);
					else
					{
						ret                 = 70;
						productCode         = mList.GetColumn("ProductCode");
						languageCode        = mList.GetColumn("LanguageCode");
						languageDialectCode = mList.GetColumn("LanguageDialectCode");
						ret                 = 0;
					}
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogInfo     ("WebTools.LoadProductFromURL/8","ret="+ret.ToString()+" ("+ex.Message+")",229);
					PCIBusiness.Tools.LogException("WebTools.LoadProductFromURL/9","ret="+ret.ToString(),ex);
				}

			if ( checkURLParms )
			{
				string h = RequestValueString(req,"PC");
				if ( h.Length > 0 ) productCode = h;
				h        = RequestValueString(req,"LC");
				if ( h.Length > 0 ) languageCode = h;
				h        = RequestValueString(req,"LDC");
				if ( h.Length > 0 ) languageDialectCode = h;
			}

			return ret;
		}

		public static byte ReplaceImage ( Page    webPage,
		                                  string  ctlID,
		                                  string  imgFileName,
		                                  string  imgTooltip,
		                                  string  imgHyperlink="",
		                                  int     imgHeight=0,
		                                  int     imgWidth=0,
		                                  Control subCtl1=null,
		                                  Control subCtl2=null,
		                                  string  blocked="")
		{
			try
			{
				Image ctl = (Image)webPage.FindControl("P"+ctlID);
				if ( ctl == null && subCtl1 != null )
					ctl    = (Image)subCtl1.FindControl("P"+ctlID);
				if ( ctl == null && subCtl2 != null )
					ctl    = (Image)subCtl2.FindControl("P"+ctlID);

				if ( ctl == null )
				{
					Literal lbl = (Literal)webPage.FindControl("F"+ctlID);
					if ( lbl   != null ) // Favicon
						lbl.Text = "<link rel='shortcut icon' href='" + PCIBusiness.Tools.ImageFolder("ImagesCA") + imgFileName + "' />";
					else
					{
						lbl = (Literal)webPage.FindControl("H"+ctlID);
						if ( lbl != null )
							lbl.Text = "<img src='" + PCIBusiness.Tools.ImageFolder("ImagesCA") + imgFileName + "' title='" + imgTooltip + "' />";
					}
					return 0;
				}

				if ( blocked == "1" || blocked == "B" )
					try
					{
					//	PCIBusiness.Tools.LogInfo("WebTools.ReplaceImage/1","ctlID="+ctlID+", blocked="+blocked+", ctl.Visible="+ctl.Visible.ToString(),244);
						ctl.Visible = false;
						Control cX  = ((Control)webPage.FindControl("D"+ctlID)); // Container around the image
						if ( cX    != null )
							cX.Visible = false;
						cX = ((Control)webPage.FindControl("H"+ctlID)); // Container around the image
						if ( cX != null )
							cX.Visible = false;
						return 0;
					}
					catch
					{
						return 0;
					}

				ctl.ToolTip   = imgTooltip;
				ctl.ImageUrl  = PCIBusiness.Tools.ImageFolder("ImagesCA") + imgFileName;
				if ( imgHeight > 0 )
					ctl.Height = imgHeight;
				else if ( imgWidth > 0 )
					ctl.Width  = imgWidth;

				if ( imgHyperlink.Length < 1 )
					return 0;

				HyperLink lnk = (HyperLink)webPage.FindControl("H"+ctlID);
				if ( lnk == null && subCtl1 != null )
					lnk    = (HyperLink)subCtl1.FindControl("H"+ctlID);
				if ( lnk == null && subCtl2 != null )
					lnk    = (HyperLink)subCtl2.FindControl("H"+ctlID);
				if ( lnk == null )
					return 30;
				if ( lnk.GetType() != typeof(HyperLink) )
					return 40;
				lnk.NavigateUrl = imgHyperlink;
				return 0;
			}
			catch (Exception ex)
			{
				PCIBusiness.Tools.LogException("WebTools.ReplaceImage","",ex);
			}
			return 90;
		}

		public static string VersionDetails(byte format=0)
		{
			string std = "App Version : " + PCIWebFinAid.SystemDetails.AppVersion + " (" + PCIWebFinAid.SystemDetails.AppDate + ")<br />"
			           + "DLL Version : " + PCIBusiness.SystemDetails.AppVersion  + " (" + PCIBusiness.SystemDetails.AppDate  + ")";

			if ( format == 2 )
				return std;

			return "<span style='color:red'><b>Prosperian BackOffice</b></span><hr />" + std;
		}

		public static byte CheckBackDoorLogins(string uId,string uPwd,ref string userCode,ref string userName)
		{
			userCode = "";
			userName = "";

			if ( string.IsNullOrWhiteSpace(uId) || string.IsNullOrWhiteSpace(uPwd) )
				return 0;

			uId  = uId.Trim().ToUpper();
			uPwd = uPwd.Trim().ToUpper();

			if ( uId == "XADMIN" && uPwd == "X8Y3Z7" )
			{
				userCode = "013";
				userName = "Johrika Burger";
				return 0;
			}
			if ( uId == "PK" && uPwd == "PK" && ! PCIBusiness.Tools.SystemIsLive() )
			{
				userCode = "013";
				userName = "Paul Kilfoil";
				return 0;
			}
			return 20;
		}

//		Moved to PCIBusiness.Tools
//		public static string ImageFolder(string defaultDir="")
//		{
//			string folder = PCIBusiness.Tools.ConfigValue("ImageFolder");
//			if ( folder.Length < 1 )
//				if ( defaultDir.Length < 1 )
//					return "Images/";
//				else
//					folder = defaultDir;
//			if ( folder.EndsWith("/") )
//				return folder;
//			return folder + "/";
//		}

/* Not complete

		public static int EMailContractConfirmation(string contractCode,string cardNumber,ControlCollection controls)
		{
			string mailText;
			int    errCode;

			try
			{
				errCode  = 20;
				mailText = File.ReadAllText(PCIBusiness.Tools.SystemFolder("Templates")+"ConfirmationMail.htm");
				errCode  = 30;

				if ( cardNumber.Length > 12 )
					cardNumber = cardNumber.Substring(0,6) + "******" + cardNumber.Substring(12);
				else if (  cardNumber.Length > 8 )
					cardNumber = cardNumber.Substring(0,4) + "******";
				else if (  cardNumber.Length > 4 )
					cardNumber = cardNumber.Substring(0,2) + "******";

				errCode  = 40;
				mailText = mailText.Replace("#lblp6CCNumber#", cardNumber);
				errCode  = 50;

				foreach (Control ctlOuter in controls)
					foreach (Control ctlInner in ctlOuter.Controls)
						if ( ctlInner.GetType() == typeof(Literal) && mailText.Contains("#"+ctlInner.ID+"#") )
							mailText = mailText.Replace("#"+ctlInner.ID+"#",PCIBusiness.Tools.HTMLSafe(((Literal)ctlInner).Text));
						else if ( ctlInner.GetType() == typeof(Label) && mailText.Contains("#"+ctlInner.ID+"#") )
							mailText = mailText.Replace("#"+ctlInner.ID+"#",PCIBusiness.Tools.HTMLSafe(((Label)ctlInner).Text));

				errCode  = 60;
				File.AppendAllText(PCIBusiness.Tools.SystemFolder("Contracts")+contractCode+".htm",mailText,Encoding.UTF8);

							}
							catch (Exception ex6)
							{
								SetErrorDetail(30105,30105,"Unable to create confirmation file ("+contractCode+".htm)",ex6.Message);
							}

			}
			catch (Exception ex1)
			{
				mailText = "";
				SetErrorDetail(30095,30095,"Unable to open mail template (Templates/ConfirmationMail.htm)",ex1.Message);
			}
		}
*/

	}
}