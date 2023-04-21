using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		private   byte   logDebug = 40; // 240;
		private   string productCode;
		private   string languageCode;
		private   string languageDialectCode;
		private   string contractCode;
		private   string contractPIN;
		private   string spr;
		private   string sql;
		private   int    errNo;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
//	Browser info in JavaScript:
//	var h = "navigator.appCodeName : " + navigator.appCodeName + "<br />"
//		   + "navigator.appName : " + navigator.appName + "<br />"
//		   + "navigator.appVersion : " + navigator.appVersion + "<br />"
//		   + "navigator.platform : " + navigator.platform + "<br />"
//		   + "navigator.userAgent : " + navigator.userAgent;

			SetErrorDetail("",-888);
			SetPostBackURL();
			SetWarning("");

			if ( Page.IsPostBack )
			{
				productCode         = WebTools.ViewStateString(ViewState,"ProductCode");
				languageCode        = WebTools.ViewStateString(ViewState,"LanguageCode");
				languageDialectCode = WebTools.ViewStateString(ViewState,"LanguageDialectCode");
				contractCode        = WebTools.ViewStateString(ViewState,"ContractCode");
				contractPIN         = WebTools.ViewStateString(ViewState,"ContractPIN");
			}
			else
			{
				lblJS.Text          = WebTools.JavaScriptSource("NextPage(0,null)");
				productCode         = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				languageCode        = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				languageDialectCode = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");
//				hdnReferURL.Value   = WebTools.ClientReferringURL(Request,11);

				if ( ! Tools.SystemIsLive() )
				{
//	Testing 1 (English)
					if ( productCode.Length         < 1 ) productCode         = "10278";
					if ( languageCode.Length        < 1 ) languageCode        = "ENG";
					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
//	Testing 2 (Thai)
//					if ( productCode.Length         < 1 ) productCode         = "10024";
//					if ( languageCode.Length        < 1 ) languageCode        = "THA";
//					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0001";
				}
				else if ( productCode.Length         < 1 ||
				          languageCode.Length        < 1 ||
					       languageDialectCode.Length < 1 )
				{
					SetErrorDetail("PageLoad",33088,"Invalid startup values ... system cannot continue","");
					SetWarning("B","Invalid startup values.");
					return;
				}
 
				if ( WebTools.CheckProductProvider(productCode,"Register.aspx",Request,Response) == 0 )
					return;

				ViewState["ProductCode"]         = productCode;
				ViewState["LanguageCode"]        = languageCode;
				ViewState["LanguageDialectCode"] = languageDialectCode;

				hdnVer.Value       = "[Register.aspx] DLL Version " + PCIBusiness.SystemDetails.AppVersion + " (" + PCIBusiness.SystemDetails.AppDate + "), "
				                   +                 "Web Version " + SystemDetails.AppVersion             + " (" + SystemDetails.AppDate             + ")";
				lblVer.Text        = "[Register] Versions " + PCIBusiness.SystemDetails.AppVersion + "/" + SystemDetails.AppVersion;
				lblVer.Visible     = ! Tools.SystemIsLive();
				btnBack1.Visible   = ! Tools.SystemIsLive();
				lblReg.Visible     = true;
				lblRegConf.Visible = false;

				if ( Tools.SystemLiveTestOrDev() != Constants.SystemMode.Development )
				{
					LoadGoogleAnalytics();
					LoadChat();
				}
				LoadLabels();

				if ( CheckIP() != "B" ) // Blocked
				{
					LoadContractCode();
					btnNext.Visible = ( lblError.Text.Length == 0 );
				}

//	Testing 2
//				if ( hdn100137.Value.Length < 1 ) hdn100137.Value = "PRIME" + Environment.NewLine + "ASSIST";
//				if ( hdn100002.Value.Length < 1 ) hdn100002.Value = "Emergency Mobile, Legal & Financial Assistance";
//				if ( lblReg.Text.Length     < 1 ) lblReg.Text     = "Registration";
//				if ( lblRegConf.Text.Length < 1 ) lblRegConf.Text = "Registration Confirmation";

//	Testing 3
//				if ( WebTools.RequestValueInt(Request,"PageNoX") > 0 )
//				{
//					hdnPageNo.Value = WebTools.RequestValueString(Request,"PageNoX");
//					btnNext_Click(null,null);
//				}

			}
		}

		private void SetWarning(string action="",string msg="")
		{
			action                = Tools.NullToString(action).ToUpper();
			pnlWarning.Visible    = ( action.Length > 0 );
			lblWarnP.Visible      = ( action == "P" );
			lblWarnB.Visible      = ( action == "B" );
			pnlDisabled.Visible   = ( action == "B" );

			if ( msg.Length > 0 )
				lblWarnX.Text      = msg;
			else
				lblWarnX.Text      = "Your IP address is not listed as from the country this product is sold.";

			if ( action == "B" )
			{
				btnBack1.Visible   = false;
				btnBack2.Visible   = false;
				btnNext.Visible    = false;
				lstTitle.Enabled   = false;
				txtSurname.Enabled = false;
				txtCellNo.Enabled  = false;
				chkAgree.Enabled   = false;
			}
		}

		private void SetPostBackURL()
		{
			int    pNo = Tools.StringToInt(hdnPageNo.Value);
			string url = "Register.aspx";
			if ( pNo > 0 )
				url = url + "?PageNo=" + (pNo+1).ToString();
			btnNext.PostBackUrl  = url;
			btnAgree.PostBackUrl = url;
		}

		private void HideControls(string controlID)
		{
			try
			{
				Control ctlToHide;
				ctlToHide       = FindControl("lbl"+controlID+"Label");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("txt"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("lst"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("opt"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("chk"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("img"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Help");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Error");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Guide");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("lblp6"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
			}
			catch (Exception ex)
			{
				Tools.LogException("Register.HideControls","",ex);
			}
			lblJS.Text = WebTools.JavaScriptSource("ShowElt('tr"+controlID+"',false);ShowElt('trp6"+controlID+"',false)",lblJS.Text,1);
		}

		private void LoadGoogleAnalytics(byte version=3)
		{
//	V3 ... from Johrika Burger via Anton Koekemoer at Open Circle Solutions, 2023/04/21

			if ( version == 3 )
			{
				lblGoogleUA.Text       = Tools.LoadGoogleAnalytics(productCode,version);
//				lblGoogleNoScript.Text = Tools.LoadGoogleAnalytics(productCode,version,"",1);
				return;
			}

			lblGoogleUA.Text = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec sp_WP_Get_GoogleACA @ProductCode=" + Tools.DBString(productCode);

					if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
					{
						string gaCode    = miscList.GetColumn("GoogleAnalyticCode");
						string url       = miscList.GetColumn("URL");
//	V1 ... gtag.js
//						lblGoogleUA.Text = Environment.NewLine
//						                 + "<!-- Global site tag (gtag.js) - Google Analytics -->" + Environment.NewLine
//						                 + "<script async src=\"https://www.googletagmanager.com/gtag/js?id=" + gaCode + "\"></script>" + Environment.NewLine
//						                 + "<script>" + Environment.NewLine
//						                 + "window.dataLayer = window.dataLayer || [];" + Environment.NewLine
//						                 + "function gtag(){dataLayer.push(arguments);}" + Environment.NewLine
//						                 + "gtag('js', new Date());" + Environment.NewLine
//						                 + "gtag('config', '" + gaCode + "', { 'linker': { 'accept_incoming': true } });" + Environment.NewLine
// //					                 + "gtag('config', '" + gaCode + "');" + Environment.NewLine
//						                 + "</script>" + Environment.NewLine;

// V2 ... Analytics.js
						lblGoogleUA.Text = Environment.NewLine
						                 + "<script>" + Environment.NewLine
						                 + "(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){"
						                 + "(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),"
						                 + "m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)"
						                 + "})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');" + Environment.NewLine
						                 + "ga('create', '" + gaCode + "', 'auto', {'allowLinker': true});" + Environment.NewLine
						                 + "ga('require', 'linker');" + Environment.NewLine
						                 + "ga('linker:autoLink', ['" + url + "'] );" + Environment.NewLine
						                 + "ga('send', 'pageview');" + Environment.NewLine
						                 + "</script>" + Environment.NewLine;
					}
					else
					{
						Tools.LogInfo     ("Register.LoadGoogleAnalytics/1","Failed to load Google UA code",logDebug);
						Tools.LogException("Register.LoadGoogleAnalytics/2",sql,null);
					}
				}
				catch (Exception ex)
				{
					Tools.LogInfo     ("Register.LoadGoogleAnalytics/8","Failed to load Google UA code",logDebug);
					Tools.LogException("Register.LoadGoogleAnalytics/9",sql,ex);
				}
		}

		private void LoadChat()
		{
			lblChat.Text = "";

			using (MiscList miscList = new MiscList())
				try
				{
					spr = "sp_WP_Get_ChatSnip";
					sql = "exec " + spr + " @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadChat",90010,"Internal database error (" + spr + ")",sql);
					else if ( miscList.EOF )
						SetErrorDetail("LoadChat",90011,"Chat widget code cannot be found (" + spr + ")",sql);
					else
						lblChat.Text = miscList.GetColumn("ChatSnippet");
				}
				catch (Exception ex)
				{
				//	Tools.LogException("Register.LoadChat",sql,ex);
					SetErrorDetail("LoadChat",90099,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}
		}	

		private string CheckIP()
		{
			string action = "";

			using (MiscList miscList = new MiscList())
				try
				{
					spr = "sp_Check_IPLocation";
					sql = "exec " + spr + " @ProductCode=" + Tools.DBString(productCode)
					                    + ",@IPAddress="   + Tools.DBString(WebTools.ClientIPAddress(Request,1));
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("CheckIP",91010,"Internal database error (" + spr + ")",sql);
					else if ( miscList.EOF )
						SetErrorDetail("CheckIP",91011,"Country/IP address check failed (" + spr + ")",sql);
					else if ( miscList.GetColumn("Result") == "1" )
						action = miscList.GetColumn("Action");
				}
				catch (Exception ex)
				{
				//	Tools.LogException("RegisterCheckIP",sql,ex);
					SetErrorDetail("CheckIP",91099,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}

			SetWarning(action);
			return action;
		}	

		private void LoadLabels()
		{
			byte logNo      = 5;
			hdn100187.Value = "X";

			using (MiscList miscList = new MiscList())
				try
				{
					HiddenField ctlHidden;
					Literal     ctlLiteral;
					string      fieldFail;
					string      fieldPass;
					string      fieldCode;
					string      fieldValue;
					string      fieldMessage;
					string      fieldBlocked;
					string      screenGuide;
					string      regPageNo;
					string      controlID;
					int         k;

//	Static labels, help text, etc
					errNo = 10;
					logNo = 10;
					spr   = "sp_WP_Get_ProductWebsiteRegContent";
					sql   = "exec " + spr + " @ProductCode="         + Tools.DBString(productCode)
					                      + ",@LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					if ( miscList.ExecQuery(sql,0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode    = miscList.GetColumn("WebsiteFieldCode");
							fieldValue   = miscList.GetColumn("WebsiteFieldValue");
							fieldMessage = miscList.GetColumn("WebsiteFieldiMessage"); // Yes, this is spelt correctly
							screenGuide  = miscList.GetColumn("WebsiteFieldScreenGuide");
							fieldFail    = miscList.GetColumn("FieldValidationFailureText");
							fieldPass    = miscList.GetColumn("FieldValidationPassText");
							regPageNo    = miscList.GetColumn("RegistrationPageNumber");
							fieldBlocked = miscList.GetColumn("Blocked");
							controlID    = "";
							logNo        = 15;
							errNo        = 0;
 
//							if ( logNo <= 10 )
//								Tools.LogInfo("Register.LoadLabels/15","Row 1, FieldCode="+fieldCode+", FieldValue="+fieldValue,logDebug);

						//	Common
							if ( fieldCode == "100119" )      // Next
								btnNext.Text = fieldValue;
//							else if ( fieldCode == "100xxx" ) // Back ... this button only exists in DEV
//								btnBack1.Text = fieldValue;
							else if ( fieldCode == "100195" ) // I agree
								btnAgree.Text = fieldValue;
							else if ( fieldCode == "100194" ) // Change payment method
								btnBack2.Text = fieldValue;
							else if ( fieldCode == "100135" ) // Registration
								lblReg.Text = fieldValue;
							else if ( fieldCode == "100207" ) // Registration Confirmation
								lblRegConf.Text = fieldValue;

						//	PDF Stuff
							else if ( fieldCode == "100002" ) // Emergency mobile assistance, blah
								hdn100002.Value  = fieldValue;
							else if ( fieldCode == "100137" ) // Product name
								hdn100137.Value  = fieldValue;

						//	Page 6
							if ( regPageNo == "6" ) // Confirmation page
							{
								ctlLiteral = (Literal)FindControl("lbl"+fieldCode);
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;
							}

						//	Page 1
							else if ( fieldCode == "100111" ) controlID      = "Title";
							else if ( fieldCode == "100114" ) controlID      = "Surname";
							else if ( fieldCode == "100117" ) controlID      = "CellNo";
							else if ( fieldCode == "104397" ) lbl104397.Text = fieldValue;
							else if ( fieldCode == "104398" ) lbl104398.Text = fieldValue;

						//	Page 2
							else if ( fieldCode == "100112" ) controlID = "FirstName";
							else if ( fieldCode == "100116" ) controlID = "EMail";
							else if ( fieldCode == "100118" ) controlID = "ID";

						//	Page 3
							else if ( fieldCode == "100123" ) controlID = "Income";
							else if ( fieldCode == "100131" ) controlID = "Status";
							else if ( fieldCode == "100132" ) controlID = "PayDay";

						//	Page 4
							else if ( fieldCode == "100138" ) controlID = "Options";
							else if ( fieldCode == "100147" ) controlID = "Payment";
							else if ( fieldCode == "100144" ) controlID = "Terms";
							else if ( fieldCode == "104429" ) controlID = "Rewards";

						//	Page 5
							else if ( fieldCode == "100186" ) controlID = "CCName";
							else if ( fieldCode == "100188" ) controlID = "CCExpiry";
							else if ( fieldCode == "100189" ) controlID = "CCCVV";
							else if ( fieldCode == "100190" ) controlID = "CCDueDay";
							else if ( fieldCode == "100187" ) // Credit card number
							{
								controlID        = "CCNumber";
								hdn100187.Value  = miscList.GetColumn("ValidationLuhnTest").ToUpper();
							}
							else if ( fieldCode == "100107" )
							{
								lblSubHead1Label.Text = fieldValue;
								lblSubHead2Label.Text = fieldValue;
							}
							else if ( fieldCode == "100122" )
								lblSubHead3Label.Text = fieldValue;
							else if ( fieldCode == "100136" )
								lblSubHead4aLabel.Text = fieldValue;
							else if ( fieldCode == "100137" )
								lblSubHead4bLabel.Text = fieldValue;
							else if ( fieldCode == "100143" )
								lblSubHead4cLabel.Text = fieldValue;
							else if ( fieldCode == "100148" )
								lblSubHead4dLabel.Text = fieldValue;
							else if ( fieldCode == "100084" )
								lblSubHead5Label.Text = fieldValue;
//							else if ( fieldCode == "100207" )
//								lblSubHead6Label.Text = fieldValue;
//							else if ( fieldCode == "100191" )
//								lblMandateHead.Text = fieldValue;
//							else if ( fieldCode == "100192" )
//								lblMandateDetail.Text = fieldValue;

							logNo = 18;

							if ( controlID.Length < 1 )
								logNo = 20;

							else if ( fieldBlocked == "1" ) // Don't show this field
							{
								logNo = 21;
								HideControls(controlID);
							}
							else
							{
								logNo      = 23;
								ctlLiteral = (Literal)FindControl("lbl"+controlID+"Label");
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;

								logNo      = 26;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Help");
								if ( ctlHidden != null )
									ctlHidden.Value = fieldMessage;

								logNo      = 29;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Error");
								if ( ctlHidden != null )
								{
									k = fieldFail.IndexOf("  ");
									if ( k > 0 )
										fieldFail    = fieldFail.Substring(0,k) + "<br /><br />" + fieldFail.Substring(k+2);
									ctlHidden.Value = fieldFail.Replace("  "," ");
								}

								logNo      = 32;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Guide");
								if ( ctlHidden != null )
								{
									k = screenGuide.IndexOf("  ");
									if ( k > 0 )
										screenGuide  = screenGuide.Substring(0,k) + "<br /><br />" + screenGuide.Substring(k+2);
									ctlHidden.Value = screenGuide.Replace("  "," ");
								}
							}
							miscList.NextRow();
						}
					else
						lblJS.Text = WebTools.JavaScriptSource("NoDataError()",lblJS.Text,1);

					SetErrorDetail("LoadLabels",errNo,"Unable to load registration page labels and data ("+spr+")",sql);

					logNo = 37;

//	Note : btnBack1 ("Back") is only for DEV, not LIVE. So no label data exists

					if ( btnNext.Text.Length  < 1 || btnBack2.Text.Length < 1 || btnAgree.Text.Length < 1 )
						Tools.LogInfo("Register.LoadLabels/37","Unable to load some or all button labels ("
						             + btnNext.Text + "/" + btnBack2.Text + "/" + btnAgree.Text + ")",logDebug);

					if ( btnNext.Text.Length  < 1 ) btnNext.Text  = "NEXT";
//					if ( btnBack1.Text.Length < 1 ) btnBack1.Text = "BACK";
					if ( btnBack2.Text.Length < 1 ) btnBack2.Text = "Change Payment Method";
					if ( btnAgree.Text.Length < 1 ) btnAgree.Text = "I AGREE";

//	Title
					spr   = "sp_WP_Get_Title";
					sql   = "exec " + spr + " @LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 40;
					errNo =  WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");
					SetErrorDetail("LoadLabels/40",errNo,"Unable to load titles ("+spr+")",sql);

//	Employment Status
					spr   = "sp_WP_Get_EmploymentStatus";
					sql   = "exec " + spr + " @LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 50;
					errNo = WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");
					SetErrorDetail("LoadLabels/50",errNo,"Unable to load employment statuses ("+spr+")",sql);

//	Pay Date
					spr   = "sp_WP_Get_PayDate";
					sql   = "exec " + spr + " @LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 60;
					errNo = WebTools.ListBind(lstPayDay,sql,null,"PayDateCode","PayDateDescription");
					SetErrorDetail("LoadLabels/60",errNo,"Unable to load pay dates ("+spr+")",sql);

//	Product Option
//	Deferred to the load of page 4
//	//				sql   = "exec sp_WP_Get_ProductOption"
//					sql   = "exec sp_WP_Get_ProductOptionA"
//					      + " @ProductCode="         + Tools.DBString(productCode)
//					      + ",@LanguageCode="        + Tools.DBString(languageCode)
//					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
//					      + ",@Income="              + hdnIncomeError.ToString();
//					logNo = 65;
//					errNo = errNo + WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");

//	But we need the details
					spr = "sp_WP_Get_WebsiteProductOptionA";
					sql = "exec " + spr + " @ProductOptionCode='0'" // Return ALL product options
					                    + ",@ProductCode="         + Tools.DBString(productCode)
					                    + ",@LanguageCode="        + Tools.DBString(languageCode)
					                    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					int    opt;
					string F = "";
					logNo    = 70;
					errNo    = miscList.ExecQuery(sql,0);
					SetErrorDetail("LoadLabels/70",errNo,"Unable to retrieve product option descriptions ("+spr+")",sql);
					while ( ! miscList.EOF )
					{
						opt = Tools.StringToInt(miscList.GetColumn("ProductOptionCode"));
						if ( opt > 0 )
							F = F + "<input type='hidden' id='hdnOption" + opt.ToString() + "' value='" + miscList.GetColumn("FieldValue").Replace("'","`").Replace(Environment.NewLine,"<br />") + "' />";
						miscList.NextRow();
					}
					lblOptionDescriptions.Text = F;

//	Payment Method
					spr   = "sp_WP_Get_PaymentMethod";
					sql   = "exec " + spr + " @ProductCode="         + Tools.DBString(productCode)
					                      + ",@LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 80;
					errNo = WebTools.ListBind(lstPayment,sql,null,"PaymentMethodCode","PaymentMethodDescription");
					SetErrorDetail("LoadLabels/80",errNo,"Unable to load payment methods ("+spr+")",sql);
				}
				catch (Exception ex)
				{
				//	Tools.LogException("Register.LoadLabels","logNo=" + logNo.ToString(),ex);
					SetErrorDetail("LoadLabels",logNo,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}

			lstCCYear.Items.Clear();
			for ( int y = System.DateTime.Now.Year ; y < System.DateTime.Now.Year+15 ; y++ )
				lstCCYear.Items.Add(new ListItem(y.ToString(),y.ToString()));
			lstCCYear.SelectedIndex = 1;
		}

		private bool LoadContractCode()
		{
			contractCode              = "";
			contractPIN               = "";
			ViewState["ContractCode"] = null;
			ViewState["ContractPIN"]  = null;

			using (MiscList miscList = new MiscList())
				try
				{
					spr = "WP_ContractApplicationC";
					sql = "exec " + spr
					    + " @RegistrationPage = 'Z'"
					    + ",@WebsiteCode ="               + Tools.DBString(WebTools.RequestValueString(Request,"WC"))
					    + ",@ProductCode ="               + Tools.DBString(productCode)
					    + ",@LanguageCode ="              + Tools.DBString(languageCode)
					    + ",@GoogleUtmSource ="           + Tools.DBString(WebTools.RequestValueString(Request,"GUS"))
					    + ",@GoogleUtmMedium ="           + Tools.DBString(WebTools.RequestValueString(Request,"GUM"))
					    + ",@GoogleUtmCampaign ="         + Tools.DBString(WebTools.RequestValueString(Request,"GUC"))
					    + ",@GoogleUtmTerm ="             + Tools.DBString(WebTools.RequestValueString(Request,"GUT"))
					    + ",@GoogleUtmContent ="          + Tools.DBString(WebTools.RequestValueString(Request,"GUN"))
					    + ",@AdvertCode ="                + Tools.DBString(WebTools.RequestValueString(Request,"AC"))
					    + ",@ClientIPAddress ="           + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					    + ",@ClientDevice ="              + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value))
//					    + ",@ReferringURL ="              + Tools.DBString(hdnReferURL.Value)
					    + ",@WebsiteVisitorCode ="        + Tools.DBString(WebTools.RequestValueString(Request,"WVC"))
					    + ",@WebsiteVisitorSessionCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVSC"));
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadContractCode",10013,"Error retrieving new contract details ("+spr+")",sql);

					else if ( miscList.EOF )
						SetErrorDetail("LoadContractCode",10023,"Error retrieving new contract details ("+spr+")",sql);

					else
					{
						contractCode = miscList.GetColumn("ContractCode");
						contractPIN  = miscList.GetColumn("PIN");
						string stat  = miscList.GetColumn("Status");
						if ( contractCode.Length > 0 && contractPIN.Length > 0 && ( stat == "0" || stat.Length == 0 ) )
						{
							lblError.Text             = "";
							ViewState["ContractCode"] = contractCode;
							ViewState["ContractPIN"]  = contractPIN;
						}
					}
				}
				catch (Exception ex)
				{
				//	Tools.LogException("Register.LoadContractCode/99",sql,ex);
					SetErrorDetail("LoadContractCode",10033,"Error retrieving new contract details ("+spr+")",ex.Message + " (" + sql + ")",2,2,ex);
					return false;
				}
				return ( lblError.Text.Length == 0 );
		}

		private int ValidateData(int pageNo)
		{
			string err = "";
			if ( pageNo == 1 )
			{
				txtSurname.Text = txtSurname.Text.Trim();
				if ( txtSurname.Visible && txtSurname.Text.Length < 2 )
					err = err + "Invalid surname (at least 2 characters required)<br />";
				txtCellNo.Text = txtCellNo.Text.Trim();
				if ( txtCellNo.Visible && txtCellNo.Text.Length < 5 )
					err = err + "Invalid cell number (at least 5 digits required)<br />";
			}
			else if ( pageNo == 2 )
			{
				txtFirstName.Text = txtFirstName.Text.Trim();
				if ( txtFirstName.Visible && txtFirstName.Text.Length < 1 )
					err = err + "Invalid first name (at least 1 character required)<br />";
				txtEMail.Text = txtEMail.Text.Trim();
				if ( txtEMail.Visible && ! Tools.CheckEMail(txtEMail.Text) )
					err = err + "Invalid email address<br />";
				txtID.Text = txtID.Text.Trim();
				if ( txtID.Visible && txtID.Text.Length < 3 )
					err = err + "Invalid id number<br />";
			}
			else if ( pageNo == 3 )
			{
				int income = Tools.StringToInt(txtIncome.Text,true);
				if ( txtIncome.Visible && income < 100 )
					err = err + "Invalid income (it must be numeric and more than 100)<br />";
				else
					txtIncome.Text = income.ToString();
			}
			else if ( pageNo == 4 )
			{ }
			else if ( pageNo == 5 )
			{
				txtCCNumber.Text = txtCCNumber.Text.Trim();
				if ( txtCCNumber.Visible && txtCCNumber.Text.Length < 12 )
					err = err + "Invalid credit/debit card number<br />";
				txtCCName.Text = txtCCName.Text.Trim();
				if ( txtCCName.Visible && txtCCName.Text.Length < 3 )
					err = err + "Invalid credit/debit card name<br />";
				txtCCCVV.Text = txtCCCVV.Text.Trim();
				if ( txtCCCVV.Visible && ( txtCCCVV.Text.Length < 3 || txtCCCVV.Text.Length > 6 ) )
					err = err + "Invalid credit/debit card CVV code<br />";
			}

			if ( err.Length > 0 )
				SetErrorDetail("ValidateData",20022,err,err,1,1,null,false,50);
			return err.Length;
		}

		protected void btnNext_Click(Object sender, EventArgs e)
		{
			int pageNo = Tools.StringToInt(hdnPageNo.Value);
			int pdfErr = 0;
			errNo      = 0;

			if ( pageNo < 1 )
			{
				SetErrorDetail("btnNext_Click",30010,"Page numbering error","The internal page number is " + pageNo.ToString());
				return;
			}
			if ( pageNo > 180 ) // Testing
			{
				contractCode      = "20191101/0014";
				txtSurname.Text   = "Smith";
				txtFirstName.Text = "Peter";
				txtEMail.Text     = "PaulKilfoil@gmail.com";
				txtIncome.Text    = "125000";
				txtCCNumber.Text  = "4901888877776666";
				txtCCCVV.Text     = "789";
			}

			if ( ValidateData(pageNo) > 0 )
				return;

			string mailText = "";

			using (MiscList miscList = new MiscList())
				try
				{
					spr = "WP_ContractApplicationC";
					sql = "exec " + spr + " @RegistrationPage =" + Tools.DBString((pageNo-1).ToString())
					                    + ",@ContractCode ="     + Tools.DBString(contractCode)
					                    + ",@ClientIPAddress ="  + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					                    + ",@ClientDevice ="     + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value));
//					                    + ",@ReferringURL ="     + Tools.DBString(hdnReferURL.Value)

//					if ( Tools.LiveTestOrDev() == Constants.SystemMode.Development )
					if ( Tools.SystemViaBackDoor() )
					{ }

					else if ( pageNo == 1 )
						sql = sql + ",@TitleCode ="        + Tools.DBString(WebTools.ListValue(lstTitle,""))
					             + ",@Surname ="          + Tools.DBString(txtSurname.Text,47) // Unicode
					             + ",@TelephoneNumberM =" + Tools.DBString(txtCellNo.Text,47);
					else if ( pageNo == 2 )
						sql = sql + ",@FirstName ="    + Tools.DBString(txtFirstName.Text,47)
					             + ",@EMailAddress =" + Tools.DBString(txtEMail.Text,47)
					             + ",@ClientCode ="   + Tools.DBString(txtID.Text,47);
					else if ( pageNo == 3 )
						sql = sql + ",@DisposableIncome ="           + Tools.DBString(txtIncome.Text,47)
					             + ",@ClientEmploymentStatusCode =" + Tools.DBString(WebTools.ListValue(lstStatus,""))
					             + ",@PayDateCode ="                + Tools.DBString(WebTools.ListValue(lstPayDay,""));
					else if ( pageNo == 4 )
						sql = sql + ",@ProductOptionCode =" + Tools.DBString(WebTools.ListValue(lstOptions,""))
					             + ",@TsCsRead = '1'"
					             + ",@PaymentMethodCode =" + Tools.DBString(WebTools.ListValue(lstPayment,""))
					             + ",@RewardsBoost = "     + ( chkRewards.Checked ? "'1'" : "'0'" );
					else if ( pageNo == 5 )
						sql = sql + ",@CardNumber ="        + Tools.DBString(txtCCNumber.Text,47)
					             + ",@AccountHolder ="     + Tools.DBString(txtCCName.Text,47)
					             + ",@CardExpiryMonth ="   + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
					             + ",@CardExpiryYear ="    + Tools.DBString(WebTools.ListValue(lstCCYear).ToString())
					             + ",@CardCVVCode ="       + Tools.DBString(txtCCCVV.Text,47);

					errNo = miscList.ExecQuery(sql,0);

					SetErrorDetail("btnNext_Click/30020",errNo,"Unable to update information (pageNo="+pageNo.ToString()+", "+spr+")",sql);

					if ( errNo == 0 && pageNo == 5 )
					{
						spr   = "WP_ContractApplicationC";
						sql   = "exec " + spr + " @RegistrationPage = '5'"
						                      + ",@ContractCode =" + Tools.DBString(contractCode);
						errNo = miscList.ExecQuery(sql,0);
						SetErrorDetail("btnNext_Click/30025",errNo,"Unable to update final information (pageNo="+pageNo.ToString()+", "+spr+")",sql);
					}			

					if ( errNo == 0 || pageNo > 180 )
					{
						pageNo++;

						if ( pageNo == 4 )
						{
							int h = Tools.StringToInt(txtIncome.Text,true);
							spr   = "sp_WP_Get_ProductOptionA";
							sql   = "exec " + spr + " @ProductCode ="         + Tools.DBString(productCode)
							                      + ",@LanguageCode ="        + Tools.DBString(languageCode)
							                      + ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode)
							                      + ",@Income ="              + h.ToString();
							errNo = WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");
							SetErrorDetail("btnNext_Click/30030",errNo,"Unable to load product options ("+spr+")",sql);
						}
						else if ( pageNo == 5 )
						{
							string productOption  = WebTools.ListValue(lstOptions,"X");
							string payMethod      = WebTools.ListValue(lstPayment,"X");
							txtCCName.Text        = "";

							if ( txtFirstName.Text.Length > 0 && txtSurname.Text.Length > 1 )
								txtCCName.Text     = txtFirstName.Text.Substring(0,1).ToUpper()
								                   + " "
								                   + txtSurname.Text.Substring(0,1).ToUpper()
								                   + txtSurname.Text.Substring(1).ToLower();
							else if ( txtSurname.Text.Length > 1 )
								txtCCName.Text     = txtSurname.Text.Substring(0,1).ToUpper()
								                   + txtSurname.Text.Substring(1).ToLower();
							else if ( txtFirstName.Text.Length > 1 )
								txtCCName.Text     = txtFirstName.Text.Substring(0,1).ToUpper()
								                   + txtFirstName.Text.Substring(1).ToLower();

							lblCCDueDate.Text     = lstPayDay.SelectedItem.Text;
							lblCCMandate.Text     = "";
							lblCCMandateHead.Text = "";
//							lblCCMandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2);
							spr                   = "sp_WP_Get_ProductOptionMandateA";
							sql                   = "exec " + spr + " @ProductCode ="         + Tools.DBString(productCode)
							                                      + ",@LanguageCode ="        + Tools.DBString(languageCode)
							                                      + ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);

							string w = " (looking for ProductOption="+productOption+" and PaymentMethod="+payMethod+")";
							Tools.LogInfo("Register.btnNext_Click/50",sql+w,logDebug);

							if ( miscList.ExecQuery(sql,0) == 0 )
								while ( ! miscList.EOF )
								{
									w = "ProductOptionCode=" + miscList.GetColumn("ProductOptionCode") +
									  ", PaymentMethodCode=" + miscList.GetColumn("PaymentMethodCode");
									if ( ( miscList.GetColumn("ProductOptionCode").ToUpper() == productOption.ToUpper()  &&
									       miscList.GetColumn("PaymentMethodCode").ToUpper() == payMethod.ToUpper()  )   ||
									       miscList.GetColumn("PaymentMethodCode") == "*" )
									{
										Tools.LogInfo("Register.btnNext_Click/51",w+" (match)",logDebug);
										lblCCMandate.Text = miscList.GetColumn("CollectionMandateText",0);
										int k             = lblCCMandate.Text.IndexOf("\n"); // Do NOT use Environment.NewLine here!
										if ( k > 0 && lblCCMandate.Text.Length > k+1 )
										{
											lblCCMandateHead.Text = lblCCMandate.Text.Substring(0,k);
											lblCCMandate.Text     = lblCCMandate.Text.Substring(k+1).Replace(Environment.NewLine,"<br />");
										}
										lblp6MandateHead.Text = lblCCMandateHead.Text;
										lblp6Mandate.Text     = lblCCMandate.Text;
										break;
									}
									Tools.LogInfo("Register.btnNext_Click/52",w,logDebug);
									miscList.NextRow();
								}

							if ( lblCCMandate.Text.Length < 1 )
								SetErrorDetail("btnNext_Click/30040",30040,"Unable to retrieve collection mandate ("+spr+")",sql+" (looking for ProductOption="+productOption+" and PaymentMethod="+payMethod+"). SQL failed or returned no data or<br />the CollectionMandateText column was missing/empty/NULL");
						}
						else if ( pageNo == 6 || pageNo > 180 )
						{
						//	sql   = "exec WP_ContractApplicationC"
						//	      +     " @RegistrationPage = '5'"
						//	      +     ",@ContractCode     =" + Tools.DBString(contractCode);
						//	errNo = miscList.ExecQuery(sql,0);
						//	SetErrorDetail("btnNext_Click/30050",errNo,"Unable to update information (pageNo=6)",sql);

							lbl100325.Text = "";
							spr = "sp_WP_Get_WebsiteProductOptionA";
							sql = "exec " + spr + " @ProductCode ="         + Tools.DBString(productCode)
							                    + ",@ProductOptionCode ="   + Tools.DBString(WebTools.ListValue(lstOptions,""))
							                    + ",@LanguageCode ="        + Tools.DBString(languageCode)
							                    + ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) != 0 )
								SetErrorDetail("btnNext_Click/30060",30060,"Internal database error (" + spr + ")",sql);
							else if ( miscList.EOF )
								SetErrorDetail("btnNext_Click/30061",30061,"No product option data returned (" + spr + ")",sql);
							else
							{
								lbl100325.Text = miscList.GetColumn("FieldValue");
								if ( lbl100325.Text.Length > 0 )
									lbl100325.Text = lbl100325.Text.Replace(Environment.NewLine,"<br />");
								else
									SetErrorDetail("btnNext_Click/30062",30062,"Product option data is empty/blank (" + spr + ", column 'FieldValue')",sql,2,2);
							}

//	Not needed any more
//							string legalAgreementHead = "";
//							string legalAgreementText = "";
//							lblp6Agreement.Text       = "";
//
//							sql = "exec sp_WP_Get_ProductLegalDocumentDetail"
//							    + " @ProductCode="         + Tools.DBString(productCode)
//							    + ",@LanguageCode="        + Tools.DBString(languageCode)
//							    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
//							    + ",@ProductLegalDocumentTypeCode='004'";
//							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
//							{
//								legalAgreementHead  = miscList.GetColumn("ProductLegalDocumentParagraphHeader");
//								legalAgreementText  = miscList.GetColumn("ProductLegalDocumentParagraphText")
//								                    + miscList.GetColumn("ProductLegalDocumentParagraphText2");
//								lblp6Agreement.Text = "<u>"
//								                    + miscList.GetColumn("ProductLegalDocumentParagraphHeader") + "</u><br />"
//								                    + miscList.GetColumn("ProductLegalDocumentParagraphText").Replace(Environment.NewLine,"<br />")
//								                    + miscList.GetColumn("ProductLegalDocumentParagraphText2").Replace(Environment.NewLine,"<br />");
//							}
//							if ( legalAgreementHead.Length + legalAgreementText.Length < 1 )
//								SetErrorDetail("btnNext_Click/30070",,30070,"Unable to retrieve legal documents",sql);

							string refundPolicy       = "";
							string moneyBackPolicy    = "";
							string cancellationPolicy = "";
//							byte   p                  = 31;

							spr = "sp_WP_Get_ProductPolicy";
							sql = "exec " + spr + " @ProductCode ="         + Tools.DBString(productCode)
							                    + ",@LanguageCode ="        + Tools.DBString(languageCode)
							                    + ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
							{
//								p                  = 46;
								refundPolicy       = miscList.GetColumn("RefundPolicyText",1,6);
								moneyBackPolicy    = miscList.GetColumn("MoneyBackPolicyText",1,6);
								cancellationPolicy = miscList.GetColumn("CancellationPolicyText",1,6);
							}
//							Tools.LogInfo("btnNext_Click/30079",sql+" (p="+p.ToString()+")",231,this);

							if ( refundPolicy.Length < 1 || moneyBackPolicy.Length < 1 || cancellationPolicy.Length < 1 )
								SetErrorDetail("btnNext_Click/30080",30080,"Unable to retrieve product policy text ("+spr+")",sql);
//	Testing
//								SetErrorDetail("btnNext_Click/30081",30081,"Unable to retrieve product policy text",sql,2,2,null,false,231);
//	Testing
							else
							{
								lblp6RefundPolicy.Text       = refundPolicy + "<br />&nbsp;";
								lblp6MoneyBackPolicy.Text    = moneyBackPolicy + "<br />&nbsp;";
								lblp6CancellationPolicy.Text = cancellationPolicy;
							}

							lblp6CCType.Text = "";
							sql              = txtCCNumber.Text.Trim();
							if ( sql.Length > 6 )
								sql = sql.Substring(0,6);
							spr = "WP_Get_CardAssociation";
							sql = "exec " + spr + " @BIN=" + Tools.DBString(sql);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
								lblp6CCType.Text = miscList.GetColumn("Brand");
							if ( lblp6CCType.Text.Length < 1 )
								SetErrorDetail("btnNext_Click/30090",30090,"Unable to retrieve card association details ("+spr+")",sql);

							if ( lblError.Text.Length > 0 )
								throw new Exception("XYZ");

//	Confirmation Page

						//	Insert Google "Conversion" code here
						//	... Get code via SP call
						//	... Put JavaScript into a literal				

							try
							{
								mailText = File.ReadAllText(Tools.SystemFolder("Templates")+"ConfirmationMail.htm");
							}
							catch (Exception ex1)
							{
								mailText = "";
								SetErrorDetail("btnNext_Click/30095",30095,"Unable to open mail template (Templates/ConfirmationMail.htm)",ex1.Message);
							}

							lblReg.Visible      = false;
							lblRegConf.Visible  = true;
							lblp6Ref.Text       = contractCode;
							lblp6Pin.Text       = contractPIN;
							lblp6Title.Text     = lstTitle.SelectedItem.Text;
							lblp6FirstName.Text = txtFirstName.Text;
							lblp6Surname.Text   = txtSurname.Text;
							lblp6EMail.Text     = txtEMail.Text;
							lblp6CellNo.Text    = txtCellNo.Text;
							lblp6ID.Text        = txtID.Text;
							lblp6Income.Text    = txtIncome.Text;
							lblp6Status.Text    = lstStatus.SelectedItem.Text;
							lblp6PayDay.Text    = lstPayDay.SelectedItem.Text;
//							lblp6Option.Text    = hdnOption.Value;
							lblp6Payment.Text   = lstPayment.SelectedItem.Text;
							lbl100209.Text      = lbl100209.Text.Replace("[Title]",lstTitle.SelectedItem.Text).Replace("[Surname]",txtSurname.Text);
							if ( txtFirstName.Text.Length > 0 )
								lbl100209.Text   = lbl100209.Text.Replace("[Initials]",txtFirstName.Text.Substring(0,1).ToUpper());
							else
								lbl100209.Text   = lbl100209.Text.Replace("[Initials]","");
							lblp6CCName.Text    = txtCCName.Text;
							lblp6CCNumber.Text  = Tools.MaskedValue(txtCCNumber.Text);
							lblp6CCExpiry.Text  = lstCCYear.SelectedValue + "/" + lstCCMonth.SelectedValue;
							lblp6Date.Text      = Tools.DateToString(System.DateTime.Now,2,1);
							lblp6IP.Text        = WebTools.ClientIPAddress(Request,1);
							lblp6Browser.Text   = WebTools.ClientBrowser(Request,hdnBrowser.Value);

//	Can't store or email the actual card number
//							sql      = "******";
//							if ( lblp6CCNumber.Text.Length > 12 )
//								sql   = lblp6CCNumber.Text.Substring(0,6) + sql + lblp6CCNumber.Text.Substring(12);
//							else if (  lblp6CCNumber.Text.Length > 8 )
//								sql   = lblp6CCNumber.Text.Substring(0,4) + sql;
//							else if (  lblp6CCNumber.Text.Length > 4 )
//								sql   = lblp6CCNumber.Text.Substring(0,2) + sql;
//							mailText = mailText.Replace("#lblp6CCNumber#", sql);

							foreach (Control ctlOuter in Page.Controls)
								if ( ctlOuter.GetType() == typeof(Literal) && mailText.Contains("#"+ctlOuter.ID+"#") )
									mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Literal)ctlOuter).Text);
								else if ( ctlOuter.GetType() == typeof(Label) && mailText.Contains("#"+ctlOuter.ID+"#") )
									mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Label)ctlOuter).Text);
								else
									foreach (Control ctlInner in ctlOuter.Controls)
										if ( ctlInner.GetType() == typeof(Literal) && mailText.Contains("#"+ctlInner.ID+"#") )
											mailText = mailText.Replace("#"+ctlInner.ID+"#",((Literal)ctlInner).Text);
										else if ( ctlInner.GetType() == typeof(Label) && mailText.Contains("#"+ctlInner.ID+"#") )
											mailText = mailText.Replace("#"+ctlInner.ID+"#",((Label)ctlInner).Text);
										else
											foreach (Control ctlDeep in ctlInner.Controls)
												if ( ctlDeep.GetType() == typeof(Literal) && mailText.Contains("#"+ctlDeep.ID+"#") )
													mailText = mailText.Replace("#"+ctlDeep.ID+"#",((Literal)ctlDeep).Text);
												else if ( ctlDeep.GetType() == typeof(Label) && mailText.Contains("#"+ctlDeep.ID+"#") )
													mailText = mailText.Replace("#"+ctlDeep.ID+"#",((Label)ctlDeep).Text);

							try
							{
								File.AppendAllText(Tools.SystemFolder("Contracts")+contractCode+".htm",mailText,Encoding.UTF8);
							}
							catch (Exception ex6)
							{
								SetErrorDetail("btnNext_Click/30105",30105,"Unable to create confirmation file ("+contractCode+".htm)",ex6.Message);
							}

//	Testing
//							if ( lblp6MandateHead.Text.Length < 1 )
//								lblp6MandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2,0);
//							if ( lblp6Mandate.Text.Length < 1 )
//								lblp6Mandate.Text     = "You hereby authorise and instruct us to collect all money due by you from your Card listed above or any other card that you may indicate from time to time";
//							if ( lblp6Billing.Text.Length < 1 )
//								lblp6Billing.Text     = "We confirm that we have received the above Billing Information as submitted by you";
//	Testing

							errNo = 30200;
							spr   = "sp_WP_Get_ProductEmail";
							sql   = "exec " + spr + " @ProductCode="  + Tools.DBString(productCode)
							                      + ",@LanguageCode=" + Tools.DBString(languageCode);
							if ( miscList.ExecQuery(sql,0) == 0 )
								if ( miscList.EOF )
									errNo = 30210;
								else
									try								
									{
										errNo               = 30215;
										string err          = "";
										string emailFrom    = miscList.GetColumn("SenderEmailAddress");
										string emailReply   = miscList.GetColumn("ReplyEMailAddress");
										string emailRoute1  = miscList.GetColumn("Route1EMailAddress",0);
										string emailRoute2  = miscList.GetColumn("Route2EMailAddress",0);
										string emailRoute3  = miscList.GetColumn("Route3EMailAddress",0);
										string emailRoute4  = miscList.GetColumn("Route4EMailAddress",0);
										string emailRoute5  = miscList.GetColumn("Route5EMailAddress",0);
//										string emailHeader  = miscList.GetColumn("EMailHeaderText");
// Testing							string emailHeader  = miscList.GetColumn("EMailHeaderTextENG");
//										string emailBody    = miscList.GetColumn("EMailBodyText");
//	Testing							string emailBody    = miscList.GetColumn("EMailBodyTextENG");

//	NO! Allow sender to be invalid
//										if ( ! Tools.CheckEMail(emailFrom) )
//										{
//											errNo = 30220;
//											err   = "Invalid sender email (" + emailFrom + ")";
//											throw new Exception(err);
//										}

										errNo               = 30225;
										string smtpServer   = Tools.ConfigValue("SMTP-Server");
										string smtpUser     = Tools.ConfigValue("SMTP-User");
										string smtpPassword = Tools.ConfigValue("SMTP-Password");
										string smtpBCC      = Tools.ConfigValue("SMTP-BCC");
										int    smtpPort     = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));

										if ( smtpServer.Length < 3 || smtpUser.Length < 3 || smtpPassword.Length < 3 )
										{
											errNo = 30230;
											err   = "Invalid SMTP details, server=" + smtpServer + ", user=" + smtpUser + ", pwd=" + smtpPassword + ", port=" + smtpPort.ToString();
											throw new Exception(err);
										}

//	If sender is invalid, use the SMTP address
										if ( ! Tools.CheckEMail(emailFrom) )
											emailFrom = smtpUser;

										errNo                      = 30235;
										SmtpClient smtp            = new SmtpClient(smtpServer);
										smtp.Credentials           = new System.Net.NetworkCredential(smtpUser,smtpPassword);
										if ( smtpPort > 0 )
											smtp.Port               = smtpPort;
//										smtp.UseDefaultCredentials = false;
//										smtp.EnableSsl             = true;

										using (MailMessage mailMsg = new MailMessage())
										{
											errNo = 30240;
											mailMsg.To.Add(txtEMail.Text);
											if ( Tools.CheckEMail(emailReply) )
												mailMsg.ReplyToList.Add(emailReply);
											if ( Tools.CheckEMail(emailRoute1) )
												mailMsg.Bcc.Add(emailRoute1);
											if ( Tools.CheckEMail(emailRoute2) )
												mailMsg.Bcc.Add(emailRoute2);
											if ( Tools.CheckEMail(emailRoute3) )
												mailMsg.Bcc.Add(emailRoute3);
											if ( Tools.CheckEMail(emailRoute4) )
												mailMsg.Bcc.Add(emailRoute4);
											if ( Tools.CheckEMail(emailRoute5) )
												mailMsg.Bcc.Add(emailRoute5);
											if ( Tools.CheckEMail(smtpBCC) )
												mailMsg.Bcc.Add(smtpBCC);

											errNo                = 30245;
											if ( Tools.CheckEMail(smtpUser,1) )
												mailMsg.Sender    = new MailAddress(smtpUser);
											mailMsg.From         = new MailAddress(emailFrom);
											mailMsg.Subject      = "Contract " + contractCode;
											mailMsg.BodyEncoding = Encoding.UTF8;
											mailMsg.Body         = mailText;
											mailMsg.IsBodyHtml   = mailText.ToUpper().Contains("<HTML");
//											errNo                = 30250;
//	Do NOT send PDF					if ( pdfFileName.Length > 0 )
//												mailMsg.Attachments.Add(new Attachment(pdfFileName));
											errNo                = 30255;

											for ( int k = 0 ; k < 5 ; k++ )
												try
												{
													if ( Tools.ConfigValue("SMTP-Mode") != "29" ) // Do not send email
														smtp.Send(mailMsg);
													errNo = 0;
													break;
												}
												catch (Exception ex3)
												{
													if ( Tools.SystemViaBackDoor() ) // Email fails, no problem
													{
														errNo = 0;
														break;
													}
													if ( k > 1 ) // After 2 failed attempts
														smtp.UseDefaultCredentials = false;
													if ( k > 2 ) // After 3 failed attempts
														Tools.LogException("Register.aspx/84","Mail send failure, errNo=" + errNo.ToString() + " (" + txtEMail.Text+")",ex3);
												}
										}
										SetErrorDetail("btnNext_Click",errNo,"Unable to send confirmation email (5 failed attempts)","");
										smtp  = null;
										errNo = 0;
									}
									catch (Exception ex4)
									{
										SetErrorDetail("btnNext_Click",errNo,"Unable to send confirmation email (SMTP error)",ex4.Message,2,2,ex4);
										errNo = 0;
									}

							SetErrorDetail("btnNext_Click",errNo,"Unable to send confirmation email ("+spr+")",sql);
						}
					}
				}
				catch (Exception ex5)
				{
					if ( ex5.Message != "XYZ" )
						SetErrorDetail("btnNext_Click",errNo,"Internal database error ; please try again later",ex5.Message,2,2,ex5);
				}

			if ( lblError.Text.Length == 0 && errNo + pdfErr > 0 )
				SetErrorDetail("btnNext_Click",30330,"Internal error ; please try again later","errNo=" + errNo.ToString() + "<br />pdfErr=" + pdfErr.ToString());

			if ( lblError.Text.Length == 0 ) // No errors, can continue
			{
				hdnPageNo.Value = pageNo.ToString();
				SetPostBackURL();
			}
		}

//		Moved to BasePage.cs
//
//		private void SetErrorDetail(int errCode,int logNo,string errBrief,string errDetail,byte briefMode=2,byte detailMode=1,byte errPriority=248)
//		{
//			if ( errCode == 0 )
//				return;
//
//			if ( errCode <  0 )
//			{
//				lblError.Text       = "";
//				lblErrorDtl.Text    = "";
//				lblError.Visible    = false;
//				btnErrorDtl.Visible = false;
//				return;
//			}
//
//			Tools.LogInfo("Register.SetErrorDetail","(errCode="+errCode.ToString()+", logNo="+logNo.ToString()+") "+errDetail,errPriority);
//
//			if ( briefMode == 2 ) // Append
//				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
//			else
//				lblError.Text = errBrief;
//
//			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
//			if ( detailMode == 2 ) // Append
//				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
//			lblErrorDtl.Text = errDetail;
//			if ( ! lblErrorDtl.Text.StartsWith("<div") )
//			//	lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white'>Error Details</div>" + lblErrorDtl.Text;
//				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;
//
//			lblError.Visible    = ( lblError.Text.Length    > 0 );
//			btnErrorDtl.Visible = ( lblErrorDtl.Text.Length > 0 ) && lblError.Visible && ! Tools.SystemIsLive();
//		}


//	Script timeout is set to 230 seconds in MS Azure and can't be changed

//		public Register() : base()
//		{
//			timeOut              = Server.ScriptTimeout;
//			Server.ScriptTimeout = 600; // 10 minutes
//		}
//
//		public override void CleanUp()
//		{
//			Server.ScriptTimeout = timeOut;
//		}
	}
}