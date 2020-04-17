using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		byte   logDebug = 240;
		string productCode;
		string languageCode;
		string languageDialectCode;
		string contractCode;
		string contractPIN;
//		string pdfFileName;
		string sql;
		int    errNo;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
//	Browser info in JavaScript:

//	var h = "navigator.appCodeName : " + navigator.appCodeName + "<br />"
//		   + "navigator.appName : " + navigator.appName + "<br />"
//		   + "navigator.appVersion : " + navigator.appVersion + "<br />"
//		   + "navigator.platform : " + navigator.platform + "<br />"
//		   + "navigator.userAgent : " + navigator.userAgent;

			SetErrorDetail(-88,0,"","");
			SetPostBackURL();

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
				lblJS.Text          = WebTools.JavaScriptSource("NextPage(0)");
				productCode         = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				languageCode        = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				languageDialectCode = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");

//	Testing 1 (English)
				if ( productCode.Length         < 1 ) productCode         = "10278";
				if ( languageCode.Length        < 1 ) languageCode        = "ENG";
				if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";

//	Testing 2 (Thai)
//				if ( productCode.Length         < 1 ) productCode         = "10024";
//				if ( languageCode.Length        < 1 ) languageCode        = "THA";
//				if ( languageDialectCode.Length < 1 ) languageDialectCode = "0001";

				GetContractCode();

				ViewState["ProductCode"]         = productCode;
				ViewState["LanguageCode"]        = languageCode;
				ViewState["LanguageDialectCode"] = languageDialectCode;

				LoadLabels();

				lblVer.Text        = "Version " + SystemDetails.AppVersion;
				lblVer.Visible     = ! Tools.SystemIsLive();
				btnBack1.Visible   = ! Tools.SystemIsLive();
				btnNext.Visible    = ( lblError.Text.Length == 0 );
				lblReg.Visible     = true;
				lblRegConf.Visible = false;

//	Testing 2
				if ( hdn100137.Value.Length < 1 ) hdn100137.Value = "PRIME" + Environment.NewLine + "ASSIST";
				if ( hdn100002.Value.Length < 1 ) hdn100002.Value = "Emergency Mobile, Legal & Financial Assistance";
				if ( lblReg.Text.Length     < 1 ) lblReg.Text     = "Registration";
				if ( lblRegConf.Text.Length < 1 ) lblRegConf.Text = "Registration Confirmation";

//	Testing 3
				if ( WebTools.RequestValueInt(Request,"PageNoX") > 0 )
				{
					hdnPageNo.Value = WebTools.RequestValueString(Request,"PageNoX");
					btnNext_Click(null,null);
				}
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

		private void LoadLabels()
		{
			byte logNo = 5;

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
					sql   = "exec sp_WP_Get_ProductWebsiteRegContent @ProductCode="         + Tools.DBString(productCode)
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

						//	Page 5
							else if ( fieldCode == "100187" ) controlID = "CCNumber";
							else if ( fieldCode == "100186" ) controlID = "CCName";
							else if ( fieldCode == "100188" ) controlID = "CCExpiry";
							else if ( fieldCode == "100189" ) controlID = "CCCVV";
							else if ( fieldCode == "100190" ) controlID = "CCDueDay";

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

//							else if ( fieldValue.Length < 1 )
							else if ( fieldBlocked == "1" )
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
						lblJS.Text = WebTools.JavaScriptSource("TestSetup()",lblJS.Text,1);

					SetErrorDetail(errNo,logNo,"Unable to load registration page labels and data",sql);

					logNo = 37;

//	Note : btnBack1 ("Back") is only for DEV, not LIVE. So no label data exists

					if ( btnNext.Text.Length  < 1 || btnBack2.Text.Length < 1 || btnAgree.Text.Length < 1 )
						Tools.LogInfo("Register.LoadLabels/37","Unable to load some or all button labels ("
						             + btnNext.Text + "/" + btnBack2.Text + "/" + btnAgree.Text + ")");

					if ( btnNext.Text.Length  < 1 ) btnNext.Text  = "NEXT";
//					if ( btnBack1.Text.Length < 1 ) btnBack1.Text = "BACK";
					if ( btnBack2.Text.Length < 1 ) btnBack2.Text = "Change Payment Method";
					if ( btnAgree.Text.Length < 1 ) btnAgree.Text = "I AGREE";

//	Title
					sql   = "exec sp_WP_Get_Title"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 40;
					errNo =  WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");
					SetErrorDetail(errNo,logNo,"Unable to load titles",sql);

//	Employment Status
					sql   = "exec sp_WP_Get_EmploymentStatus"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 50;
					errNo = WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");
					SetErrorDetail(errNo,logNo,"Unable to load employment statuses",sql);

//	Pay Date
					sql   = "exec sp_WP_Get_PayDate"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 60;
					errNo = WebTools.ListBind(lstPayDay,sql,null,"PayDateCode","PayDateDescription");
					SetErrorDetail(errNo,logNo,"Unable to load pay dates",sql);

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
					sql = "exec sp_WP_Get_WebsiteProductOptionA"
					    + " @ProductOptionCode='0'" // Return ALL product options
					    + ",@ProductCode="         + Tools.DBString(productCode)
					    + ",@LanguageCode="        + Tools.DBString(languageCode)
					    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					int    opt;
					string F = "";
					logNo    = 70;
					errNo    = miscList.ExecQuery(sql,0);
					SetErrorDetail(errNo,logNo,"Unable to retrieve product option descriptions",sql);
					while ( ! miscList.EOF )
					{
						opt = Tools.StringToInt(miscList.GetColumn("ProductOptionCode"));
						if ( opt > 0 )
							F = F + "<input type='hidden' id='hdnOption" + opt.ToString() + "' value='" + miscList.GetColumn("FieldValue").Replace("'","`").Replace(Environment.NewLine,"<br />") + "' />";
						miscList.NextRow();
					}
					lblOptionDescriptions.Text = F;

//	Payment Method
					sql   = "exec sp_WP_Get_PaymentMethod"
					      + " @ProductCode="         + Tools.DBString(productCode)
					      + ",@LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 80;
					errNo = WebTools.ListBind(lstPayment,sql,null,"PaymentMethodCode","PaymentMethodDescription");
					SetErrorDetail(errNo,logNo,"Unable to load payment methods",sql);
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.LoadLabels","logNo=" + logNo.ToString(),ex);
					SetErrorDetail(99,logNo,"Internal error ; please try again later",ex.Message + " (" + sql + ")");
				}

			lstCCYear.Items.Clear();
			for ( int y = System.DateTime.Now.Year ; y < System.DateTime.Now.Year+15 ; y++ )
				lstCCYear.Items.Add(new ListItem(y.ToString(),y.ToString()));
			lstCCYear.SelectedIndex = 1;
		}

		private bool GetContractCode()
		{
			contractCode              = "";
			contractPIN               = "";
			ViewState["ContractCode"] = null;
			ViewState["ContractPIN"]  = null;

//			SetErrorDetail(-77,0,"","");

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationC"
					    +     " @RegistrationPage   = 'Z'"
					    +     ",@WebsiteCode        =" + Tools.DBString(WebTools.RequestValueString(Request,"WC"))
					    +     ",@ProductCode        =" + Tools.DBString(productCode)
					    +     ",@LanguageCode       =" + Tools.DBString(languageCode)
					    +     ",@GoogleUtmSource    =" + Tools.DBString(WebTools.RequestValueString(Request,"GUS"))
					    +     ",@GoogleUtmMedium    =" + Tools.DBString(WebTools.RequestValueString(Request,"GUM"))
					    +     ",@GoogleUtmCampaign  =" + Tools.DBString(WebTools.RequestValueString(Request,"GUC"))
					    +     ",@GoogleUtmTerm      =" + Tools.DBString(WebTools.RequestValueString(Request,"GUT"))
					    +     ",@GoogleUtmContent   =" + Tools.DBString(WebTools.RequestValueString(Request,"GUN"))
					    +     ",@AdvertCode         =" + Tools.DBString(WebTools.RequestValueString(Request,"AC"))
					    +     ",@ClientIPAddress    =" + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					    +     ",@ClientDevice       =" + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value))
					    +     ",@WebsiteVisitorCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVC"))
					    +     ",@WebsiteVisitorSessionCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVSC"));
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail(10013,10013,"Error retrieving new contract details ; please try again later",sql);

					else if ( miscList.EOF )
						SetErrorDetail(10023,10023,"Error retrieving new contract details ; please try again later",sql);

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

//					If this fails, continue - don't stop with an error
					lblGoogleUA.Text = "";
					sql              = "exec sp_WP_Get_GoogleACA @ProductCode =" + Tools.DBString(productCode);
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
						                 + "ga('create', '" + gaCode + "', 'auto', {'allowLinker': true});" + Environment.NewLine
						                 + "ga('require', 'linker');" + Environment.NewLine
						                 + "ga('linker:autoLink', ['" + url + "'] );" + Environment.NewLine
						                 + "</script>" + Environment.NewLine;
					}

//					if ( miscList.ExecQuery(sql,0) != 0 )
//						SetErrorDetail(10026,10026,"Error retrieving the Google analytics code ; please try again later",sql);
//					else if ( miscList.EOF )
//						SetErrorDetail(10028,10028,"Error retrieving the Google analytics code ; please try again later",sql);
//					else
//						lblGoogleUA.Text = "Blah, blah" + miscList.GetColumn(0);
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.GetContractCode/99",sql,ex);
					SetErrorDetail(10033,10033,"Error retrieving new contract details ; please try again later",ex.Message + " (" + sql + ")");
					return false;
				}
				return ( lblError.Text.Length == 0 );
		}

		private int Validate(int pageNo)
		{
			string err = "";
			if ( pageNo == 1 )
			{
				txtSurname.Text = txtSurname.Text.Trim();
				if ( txtSurname.Visible && txtSurname.Text.Length < 2 )
					err = err + "Invalid surname (at least 2 characters required)<br />";
				txtCellNo.Text = txtCellNo.Text.Trim();
				if ( txtCellNo.Visible && txtCellNo.Text.Replace(" ","").Length < 10 )
					err = err + "Invalid cell number (at least 10 digits required)<br />";
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
				if ( txtCCCVV.Visible && txtCCCVV.Text.Length < 3 )
					err = err + "Invalid credit/debit card CVV code<br />";
			}

			if ( err.Length > 0 )
				SetErrorDetail(20022,20022,err,err,1,1);
			return err.Length;
		}

//	NO!
//		protected override void Render(HtmlTextWriter writer)
//		{
//			if (renderPDF)
//				try
//				{
//					renderPDF                        = false;
//					System.IO.TextWriter  pdfWriter  = new System.IO.StringWriter();
//					HtmlTextWriter        htmlWriter = new HtmlTextWriter(pdfWriter);
//					base.Render(htmlWriter);
//
//					string page6 = pdfWriter.ToString();
//					int    k1    = page6.IndexOf("<!-- [PDF-Start] -->");
//					int    k2    = page6.IndexOf("<!-- [PDF-End] -->");
//					if ( k1 > 0 && k2 > k1 )
//					{
//						page6 = page6.Substring(k1+20,k2-k1-20);
//						k1    = PCIBusiness.Tools.CreatePDF(contractCode,page6,ref pdfFileName);
//						if ( k1 == 0 && pdfFileName.Length > 0 )
//							SendConfirmationEmail();
//					}
//				}
//				catch (Exception ex)
//				{
//					Tools.LogException("Register.Render","",ex);
//				}
//
//			else
//				base.Render(writer);
//		}


		protected void btnNext_Click(Object sender, EventArgs e)
		{
//	PDF test
//			string ff = "<style>h1 {font-size:12px;}</style><h1>Test</h1><p style='font-weight:bold'>Test Bold</p>";
//			PCIBusiness.Tools.CreatePDF(ff);
//	PDF test

			int pageNo = Tools.StringToInt(hdnPageNo.Value);
			int pdfErr = 0;
			errNo      = 0;

			if ( pageNo < 1 )
			{
				SetErrorDetail(99,30010,"Page numbering error","The internal page number is " + pageNo.ToString());
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

			if ( Validate(pageNo) > 0 )
				return;

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationC"
					    +     " @RegistrationPage =" + Tools.DBString((pageNo-1).ToString())
					    +     ",@ContractCode     =" + Tools.DBString(contractCode)
					    +     ",@ClientIPAddress  =" + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					    +     ",@ClientDevice     =" + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value));

					if ( Tools.LiveTestOrDev() == Constants.SystemMode.Development )
					{ }

					else if ( pageNo == 1 )
						sql = sql + ",@TitleCode        =" + Tools.DBString(WebTools.ListValue(lstTitle,""))
					             + ",@Surname          =" + Tools.DBString(txtSurname.Text,47) // Unicode
					             + ",@TelephoneNumberM =" + Tools.DBString(txtCellNo.Text,47);
					else if ( pageNo == 2 )
						sql = sql + ",@FirstName    =" + Tools.DBString(txtFirstName.Text,47)
					             + ",@EMailAddress =" + Tools.DBString(txtEMail.Text,47)
					             + ",@ClientCode   =" + Tools.DBString(txtID.Text);
					else if ( pageNo == 3 )
						sql = sql + ",@DisposableIncome           =" + Tools.DBString(txtIncome.Text,47)
					             + ",@ClientEmploymentStatusCode =" + Tools.DBString(WebTools.ListValue(lstStatus,""))
					             + ",@PayDateCode                =" + Tools.DBString(WebTools.ListValue(lstPayDay,""));
					else if ( pageNo == 4 )
						sql = sql + ",@ProductOptionCode =" + Tools.DBString(WebTools.ListValue(lstOptions,""))
					             + ",@TsCsRead          = '1'"
					             + ",@PaymentMethodCode =" + Tools.DBString(WebTools.ListValue(lstPayment,""));
					else if ( pageNo == 5 )
						sql = sql + ",@CardNumber      =" + Tools.DBString(txtCCNumber.Text,47)
					             + ",@AccountHolder   =" + Tools.DBString(txtCCName.Text,47)
					             + ",@CardExpiryMonth =" + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
					             + ",@CardExpiryYear  =" + Tools.DBString(WebTools.ListValue(lstCCYear).ToString())
					             + ",@CardCVVCode     =" + Tools.DBString(txtCCCVV.Text,47);

					errNo = miscList.ExecQuery(sql,0);
					SetErrorDetail(errNo,30020,"Unable to update information (pageNo="+pageNo.ToString()+")",sql);

//					if ( errNo == 0 && pageNo == 5 )
//					{
//						sql   = "exec WP_ContractApplicationC"
//						      +     " @RegistrationPage = '5'"
//						      +     ",@ContractCode     =" + Tools.DBString(contractCode);
//						errNo = miscList.ExecQuery(sql,0);
//						SetErrorDetail(errNo,30025,"Unable to update information",sql);
//					}			

					if ( errNo == 0 || pageNo > 180 )
					{
						pageNo++;

						if ( pageNo == 4 )
						{
							int h = Tools.StringToInt(txtIncome.Text,true);
							sql   = "exec sp_WP_Get_ProductOptionA"
							      + " @ProductCode="         + Tools.DBString(productCode)
							      + ",@LanguageCode="        + Tools.DBString(languageCode)
							      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
							      + ",@Income="              + h.ToString();
							errNo = WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");
							SetErrorDetail(errNo,30030,"Unable to obtain product options",sql);
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
							sql                   = "exec sp_WP_Get_ProductOptionMandateA"
							                      + " @ProductCode="         + Tools.DBString(productCode)
							                      + ",@LanguageCode="        + Tools.DBString(languageCode)
							                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

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
								SetErrorDetail(30040,30040,"Unable to retrieve collection mandate",sql+" (looking for ProductOption="+productOption+" and PaymentMethod="+payMethod+"). SQL failed or returned no data or<br />these CollectionMandateText column was missing/empty/NULL");
						}
						else if ( pageNo == 6 || pageNo > 180 )
						{
							sql   = "exec WP_ContractApplicationC"
							      +     " @RegistrationPage = '5'"
							      +     ",@ContractCode     =" + Tools.DBString(contractCode);
							errNo = miscList.ExecQuery(sql,0);
							SetErrorDetail(errNo,30050,"Unable to update information (pageNo=6)",sql);

							lbl100325.Text = "";
							sql = "exec sp_WP_Get_WebsiteProductOptionA"
							    + " @ProductCode="         + Tools.DBString(productCode)
							    + ",@ProductOptionCode="   + Tools.DBString(WebTools.ListValue(lstOptions,""))
							    + ",@LanguageCode="        + Tools.DBString(languageCode)
							    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) != 0 )
								SetErrorDetail(30060,30060,"Internal database error (sp_WP_Get_WebsiteProductOptionA)",sql,2,2);
							else if ( miscList.EOF )
								SetErrorDetail(30061,30061,"No product option data returned (sp_WP_Get_WebsiteProductOptionA)",sql,2,2);
							else
							{
								lbl100325.Text = miscList.GetColumn("FieldValue");
								if ( lbl100325.Text.Length > 0 )
									lbl100325.Text = lbl100325.Text.Replace(Environment.NewLine,"<br />");
								else
									SetErrorDetail(30062,30062,"Product option data is empty/blank (sp_WP_Get_WebsiteProductOptionA, column 'FieldValue')",sql,2,2);
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
//								SetErrorDetail(30070,30070,"Unable to retrieve legal documents",sql,2,2);

							string refundPolicy       = "";
							string moneyBackPolicy    = "";
							string cancellationPolicy = "";

							sql = "exec sp_WP_Get_ProductPolicy"
							    + " @ProductCode="         + Tools.DBString(productCode)
							    + ",@LanguageCode="        + Tools.DBString(languageCode)
							    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
							{
								refundPolicy                 = miscList.GetColumn("RefundPolicyText");
								moneyBackPolicy              = miscList.GetColumn("MoneyBackPolicyText");
								cancellationPolicy           = miscList.GetColumn("CancellationPolicyText");
								lblp6RefundPolicy.Text       = refundPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
								lblp6MoneyBackPolicy.Text    = moneyBackPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
								lblp6CancellationPolicy.Text = cancellationPolicy.Replace(Environment.NewLine,"<br />");
							}
							if ( refundPolicy.Length < 1 || moneyBackPolicy.Length < 1 || cancellationPolicy.Length < 1 )
								SetErrorDetail(30080,30080,"Unable to retrieve product policy text",sql,2,2);

							lblp6CCType.Text = "";
							sql              = txtCCNumber.Text.Trim();
							if ( sql.Length > 6 )
								sql = sql.Substring(0,6);
							sql = "exec WP_Get_CardAssociation @BIN=" + Tools.DBString(sql);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
								lblp6CCType.Text = miscList.GetColumn("Brand");
							if ( lblp6CCType.Text.Length < 1 )
								SetErrorDetail(30090,30090,"Unable to retrieve card association details",sql,2,2);

							if ( lblError.Text.Length > 0 )
								throw new Exception("XYZ");

//	Confirmation Page
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
							lblp6CCNumber.Text  = txtCCNumber.Text;
							lblp6CCExpiry.Text  = lstCCYear.SelectedValue + "/" + lstCCMonth.SelectedValue;
							lblp6Date.Text      = Tools.DateToString(System.DateTime.Now,2,1);
							lblp6IP.Text        = WebTools.ClientIPAddress(Request,1);
							lblp6Browser.Text   = WebTools.ClientBrowser(Request,hdnBrowser.Value);

//	Testing
//							if ( lblp6MandateHead.Text.Length < 1 )
//								lblp6MandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2,0);
//							if ( lblp6Mandate.Text.Length < 1 )
//								lblp6Mandate.Text     = "You hereby authorise and instruct us to collect all money due by you from your Card listed above or any other card that you may indicate from time to time";
//							if ( lblp6Billing.Text.Length < 1 )
//								lblp6Billing.Text     = "We confirm that we have received the above Billing Information as submitted by you";
//	Testing

//	Generate PDF, version 4 (IronPDF)
//		and also
//	Generate PDF, version 3 (SelectPDF)
//
//	PDF code removed (2020/04/08)
/*
							string h   = Request.Url.GetLeftPart(UriPartial.Authority);
							string f   = Tools.SystemFolder("")+"TemplatePDF.htm";
							string pdf = Tools.ReadFile(f);
							int    k1  = pdf.IndexOf("<!-- [PDF-Start] -->");
							int    k2  = pdf.IndexOf("<!-- [PDF-End] -->");
							if ( k1 > 0 && k2 > k1 )
								pdf = pdf.Substring(k1+20,k2-k1-20);
							if ( h.EndsWith("/") )
								h = h.Substring(0,h.Length-1);

							if ( h.Length < 7 )
								SetErrorDetail(30100,30100,"Unable to establish the URL to use",h);
							if ( pdf.Length < 100 )
								SetErrorDetail(30110,30110,"Unable to find/open the PDF template file",f);

							StringBuilder pdfText = new StringBuilder(pdf);

//	Heading
							pdfText = pdfText.Replace("#URL#"           ,h)
							                 .Replace("#PRODUCT-NAME#"  ,hdn100137.Value)
							                 .Replace("#PRODUCT-DETAIL#",hdn100002.Value)
							                 .Replace("#REGISTRATION#"  ,lblRegConf.Text)
							                 .Replace("#CONGRATS#"      ,lbl100400.Text)
							                 .Replace("#GREETING#"      ,lbl100209.Text);

//	Contract details
//	..	Labels
							pdfText = pdfText.Replace("#CONTRACT-HEAD#"    ,lbl100372.Text)
							                 .Replace("#CONTRACT-CODE-LBL#",lbl100210.Text)
							                 .Replace("#CONTRACT-PIN-LBL#" ,lbl100211.Text);
//	..	Data
							pdfText = pdfText.Replace("#CONTRACT-CODE#",lblp6Ref.Text)
							                 .Replace("#CONTRACT-PIN#" ,lblp6Pin.Text);

//	Personal info
//	..	Labels  
							pdfText = pdfText.Replace("#PERSONAL-HEAD#"   ,lbl100212.Text)
							                 .Replace("#TITLE-LBL#"       ,lbl100111.Text)
							                 .Replace("#FIRST-NAME-LBL#"  ,lbl100214.Text)
							                 .Replace("#SURNAME-LBL#"     ,lbl100216.Text)
							                 .Replace("#EMAIL-LBL#"       ,lbl100218.Text)
							                 .Replace("#CELL-NO-LBL#"     ,lbl100219.Text)
							                 .Replace("#ID-NUMBER-LBL#"   ,lbl100220.Text)
							                 .Replace("#PERSONAL-CONFIRM#",lbl100373.Text);
//	..	Data
							pdfText = pdfText.Replace("#TITLE#"     ,lblp6Title.Text)
							                 .Replace("#FIRST-NAME#",lblp6FirstName.Text)
							                 .Replace("#SURNAME#"   ,lblp6Surname.Text)
							                 .Replace("#EMAIL#"     ,lblp6EMail.Text)
							                 .Replace("#CELL-NO#"   ,lblp6Cell.Text)
							                 .Replace("#ID-NUMBER#" ,lblp6ID.Text);

//	Income
//	..	Labels
							pdfText = pdfText.Replace("#INCOME-HEAD#"   ,lbl100222.Text)
							                 .Replace("#INCOME-LBL#"    ,lbl100223.Text)
							                 .Replace("#EMPLOYMENT-LBL#",lbl100230.Text)
							                 .Replace("#PAY-DAY-LBL#"   ,lbl100231.Text)
							                 .Replace("#INCOME-CONFIRM#",lbl100374.Text);
//	..	Data
							pdfText = pdfText.Replace("#INCOME#"    ,lblp6Income.Text)
							                 .Replace("#EMPLOYMENT#",lblp6Status.Text)
							                 .Replace("#PAY-DAY#"   ,lblp6PayDay.Text);

//	Product
//	..	Labels
							pdfText = pdfText.Replace("#PRODUCT-HEAD#"   ,lbl100233.Text)
							                 .Replace("#PAY-METHOD-LBL#" ,lbl100236.Text)
							                 .Replace("#PRODUCT-CONFIRM#",lbl100237.Text);
//	..	Data
							pdfText = pdfText.Replace("#PRODUCT-OPTIONS#",lbl100325.Text)
							                 .Replace("#PAY-METHOD#"     ,lblp6PayMethod.Text);

//	Credit Card
//	..	Labels
							pdfText = pdfText.Replace("#CCARD-HEAD#"      ,lbl100184.Text)
							                 .Replace("#CCARD-TYPE-LBL#"  ,lbl100185.Text)
							                 .Replace("#CCARD-NAME-LBL#"  ,lbl100186.Text)
							                 .Replace("#CCARD-NUMBER-LBL#",lbl100187.Text)
							                 .Replace("#CCARD-EXPIRY-LBL#",lbl100188.Text)
							                 .Replace("#CCARD-CONFIRM#"   ,lblp6Billing.Text);
//	..	Data
							pdfText = pdfText.Replace("#CCARD-TYPE#"  ,lblp6CCType.Text)
							                 .Replace("#CCARD-NAME#"  ,lblp6CCName.Text)
							                 .Replace("#CCARD-NUMBER#",lblp6CCNumber.Text)
							                 .Replace("#CCARD-EXPIRY#",lblp6CCExpiry.Text);

//	Mandate
//	..	Labels
							pdfText = pdfText.Replace("#MANDATE-HEAD#",lblp6MandateHead.Text)
							                 .Replace("#MANDATE#"     ,lblp6Mandate.Text);

//	Policies
//	..	Labels
							pdfText = pdfText.Replace("#POLICY-HEAD#"     ,lbl100238.Text)
							                 .Replace("#POLICY-REFUND#"   ,lblp6RefundPolicy.Text)        // refundPolicy)
							                 .Replace("#POLICY-MONEYBACK#",lblp6MoneyBackPolicy.Text)     // moneyBackPolicy)
							                 .Replace("#POLICY-CANCEL#"   ,lblp6CancellationPolicy.Text); // cancellationPolicy);

//	Connection
//	..	Labels
							pdfText = pdfText.Replace("#CONN-HEAD#"     ,lbl100259.Text)
							                 .Replace("#DATE-TIME-LBL#" ,lbl100375.Text)
							                 .Replace("#IP-ADDRESS-LBL#",lbl100376.Text);
//	..	Data
							pdfText = pdfText.Replace("#DATE-TIME#" ,lblp6Date.Text)
							                 .Replace("#IP-ADDRESS#",lblp6IP.Text)
							                 .Replace("#BROWSER#"   ,lblp6Browser.Text);

//	Version
//	.. Do not include in live version
							if ( Tools.SystemIsLive() )
								pdfText = pdfText.Replace("#SYSTEM-DETAILS#","")
								                 .Replace("#SYSTEM-OWNER#","");
							else
								pdfText = pdfText.Replace("#SYSTEM-DETAILS#","FinAid, Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")")
								                 .Replace("#SYSTEM-OWNER#","(c) " + SystemDetails.Owner);

							errNo = Tools.CreatePDF(contractCode,pdfText.ToString(),ref pdfFileName);
							if ( errNo < 1 && pdfFileName.Length < 5 )
								errNo = 30120;
							SetErrorDetail(errNo,errNo,"Unable to create PDF file",pdfFileName);
*/

//	Generate PDF, version 1 (iTextSharp)
/*
							string pdfFileName = "";
 
							using (PdfFile pdf = new PdfFile())
							{
								pdfErr =          pdf.Create("FinAid",contractCode);
								pdfErr = pdfErr + pdf.AddParagraph(hdn100137.Value,(int)Constants.PdfFontSize.HugeHeading,1,(int)Constants.PdfAlign.Centre,"Orange"); // Prime Assist (product Name)
								pdfErr = pdfErr + pdf.AddParagraph(hdn100002.Value,(int)Constants.PdfFontSize.TableCell,2,(int)Constants.PdfAlign.Centre); // Emergency mobile, blah
								pdfErr = pdfErr + pdf.AddParagraph(lblRegConf.Text,(int)Constants.PdfFontSize.MajorHeading,2,(int)Constants.PdfAlign.Centre); // Registration confirmation
								pdfErr = pdfErr + pdf.AddParagraph(lbl100400.Text,(int)Constants.PdfFontSize.MinorHeading,2,(int)Constants.PdfAlign.Centre); // Congratulations
								pdfErr = pdfErr + pdf.AddParagraph(lbl100209.Text,(int)Constants.PdfFontSize.TableCell,2,(int)Constants.PdfAlign.Left); // Dear Mr Smith

								pdfErr = pdfErr + pdf.TableOpen(2);

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100372.Text);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100210.Text,lblp6Ref.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100211.Text,lblp6Pin.Text});
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100212.Text);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100111.Text,lblp6Title.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100214.Text,lblp6FirstName.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100216.Text,lblp6Surname.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100218.Text,lblp6EMail.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100219.Text,lblp6Cell.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100220.Text,lblp6ID.Text});
								pdfErr = pdfErr + pdf.TableWriteLine(lbl100373.Text,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100222.Text);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100223.Text,lblp6Income.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100230.Text,lblp6Status.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100231.Text,lblp6PayDay.Text});
								pdfErr = pdfErr + pdf.TableWriteLine(lbl100374.Text,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100233.Text);
								pdfErr = pdfErr + pdf.TableWriteLine(lbl100325.Text,2);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100236.Text,lblp6PayMethod.Text});
								pdfErr = pdfErr + pdf.TableWriteLine(lbl100237.Text,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100184.Text);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100185.Text,lblp6CCType.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100186.Text,lblp6CCName.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100187.Text,lblp6CCNumber.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100188.Text,lblp6CCExpiry.Text});
								pdfErr = pdfErr + pdf.TableWriteLine(lblp6Billing.Text,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lblp6MandateHead.Text);
								pdfErr = pdfErr + pdf.TableWriteLine(lblp6Mandate.Text,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100238.Text);
								pdfErr = pdfErr + pdf.TableWriteLine(refundPolicy,2,2);
								pdfErr = pdfErr + pdf.TableWriteLine(moneyBackPolicy,2,2);
								pdfErr = pdfErr + pdf.TableWriteLine(cancellationPolicy,2);
								pdfErr = pdfErr + pdf.TableWriteLine();

								pdfErr = pdfErr + pdf.TableWriteLine(lbl100259.Text);
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100375.Text,lblp6Date.Text});
								pdfErr = pdfErr + pdf.TableWriteRow(new string[] {lbl100376.Text,lblp6IP.Text});
								pdfErr = pdfErr + pdf.TableWriteLine(lblp6Browser.Text,2);

								pdfErr = pdfErr + pdf.TableClose();
								pdf.Close();

								if ( pdfErr == 0 )
									pdfFileName = pdf.SavedFileNameAndFolder;
								else
									SetErrorDetail(pdfErr,30150,"Unable to create PDF file","");
							}
//	Generate PDF, version 1
*/

//	Send email
//	Code removed, 2020/04/08
//	Instead ...
							errNo = 0;

/*
// Code removed 2020/04/08
							errNo = 30200;
							sql   = "exec sp_WP_Get_ProductEmail"
							      + " @ProductCode="  + Tools.DBString(productCode)
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
										string emailRoute1  = miscList.GetColumn("Route1EMailAddress");
										string emailRoute2  = miscList.GetColumn("Route2EMailAddress");
										string emailRoute3  = miscList.GetColumn("Route3EMailAddress",0);
										string emailRoute4  = miscList.GetColumn("Route4EMailAddress",0);
										string emailRoute5  = miscList.GetColumn("Route5EMailAddress",0);
										string emailHeader  = miscList.GetColumn("EMailHeaderText");
//										string emailHeader  = miscList.GetColumn("EMailHeaderTextENG");
										string emailBody    = miscList.GetColumn("EMailBodyText");
//										string emailBody    = miscList.GetColumn("EMailBodyTextENG");

										if ( ! Tools.CheckEMail(emailFrom) )
										{
											errNo = 30220;
											err   = "Invalid sender email (" + emailFrom + ")";
											throw new Exception(err);
										}

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

											errNo              = 30245;
											mailMsg.From       = new MailAddress(emailFrom);
											mailMsg.Subject    = emailHeader.Replace("[ContractCode]",contractCode);
											mailMsg.Body       = emailBody.Replace("[ContractCode]",contractCode);
											mailMsg.IsBodyHtml = emailBody.ToUpper().Contains("<HTML");
//											errNo              = 30250;
//	Do NOT send PDF					if ( pdfFileName.Length > 0 )
//												mailMsg.Attachments.Add(new Attachment(pdfFileName));
											errNo              = 30255;

											for ( int k = 0 ; k < 5 ; k++ )
												try
												{
													smtp.Send(mailMsg);
													errNo         = 0;
													lblError.Text = "";
													break;
												}
												catch (Exception ex1)
												{
													if ( k > 1 ) // After 2 failed attempts
														smtp.UseDefaultCredentials = false;
													if ( k > 2 ) // After 3 failed attempts
														Tools.LogException("Register.aspx/84","Mail send failure, errNo=" + errNo.ToString() + " (" + txtEMail.Text+")",ex1);
												}
										}
										SetErrorDetail(errNo,errNo,"Unable to send confirmation email (5 failed attempts)","");
										smtp  = null;
										errNo = 0;
									}
									catch (Exception ex2)
									{
										SetErrorDetail(99,errNo,"Unable to send confirmation email (SMTP error)",ex2.Message);
										errNo = 0;
									}

							SetErrorDetail(errNo,errNo,"Unable to send confirmation email (SQL error)",sql);
*/
//	Email code removed, 2020/04/08
						}
					}
				}
				catch (Exception ex3)
				{
					if ( ex3.Message != "XYZ" )
						SetErrorDetail(99,errNo,"Internal database error ; please try again later",ex3.Message);
				}

			if ( lblError.Text.Length == 0 && errNo + pdfErr > 0 )
				SetErrorDetail(99,30330,"Internal error ; please try again later","errNo=" + errNo.ToString() + "<br />pdfErr=" + pdfErr.ToString());

			if ( lblError.Text.Length == 0 ) // No errors, can continue
			{
				hdnPageNo.Value     = pageNo.ToString();
//				btnNext.PostBackUrl = "Register.aspx?PageNo=" + (pageNo+1).ToString();
				SetPostBackURL();
			}
		}

		private int SendConfirmationEmail()
		{
			if ( Tools.ConfigValue("SMTP-Mode") == "1" )
			{
				SetErrorDetail(99,30117,"Mail not sent (SMTP-Mode=1)","");
				return 30117;
			}

			string err         = "";
			string emailFrom   = "";
			string emailReply  = "";
			string emailRoute1 = "";
			string emailRoute2 = "";
			string emailRoute3 = "";
			string emailRoute4 = "";
			string emailRoute5 = "";
			string emailHeader = "";
//			string emailHeader = "";
			string emailBody   = "";
//			string emailBody   = "";

			errNo = 30200;
			sql   = "exec sp_WP_Get_ProductEmail"
			      + " @ProductCode="  + Tools.DBString(productCode)
			      + ",@LanguageCode=" + Tools.DBString(languageCode);

			using (MiscList mList = new MiscList())
				if ( mList.ExecQuery(sql,0) != 0 )
					errNo = 30210;
				else if ( mList.EOF )
					errNo = 30220;
				else
				{
					errNo       = 30230;
					err         = "";
					emailFrom   = mList.GetColumn("SenderEmailAddress");
					emailReply  = mList.GetColumn("ReplyEMailAddress");
					emailRoute1 = mList.GetColumn("Route1EMailAddress");
					emailRoute2 = mList.GetColumn("Route2EMailAddress");
					emailRoute3 = mList.GetColumn("Route3EMailAddress",0);
					emailRoute4 = mList.GetColumn("Route4EMailAddress",0);
					emailRoute5 = mList.GetColumn("Route5EMailAddress",0);
					emailHeader = mList.GetColumn("EMailHeaderText");
//					emailHeader = mList.GetColumn("EMailHeaderTextENG");
					emailBody   = mList.GetColumn("EMailBodyText");
//					emailBody   = mList.GetColumn("EMailBodyTextENG");

					if ( emailFrom.Length > 5 && emailHeader.Length > 0 && emailBody.Length > 0 )
						errNo = 0;
				}

			if ( errNo > 0 )
			{
				SetErrorDetail(99,errNo,"Unable to obtain email details from SQL",sql);
				return errNo;
			}

			try
			{
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

				errNo                      = 30235;
				SmtpClient smtp            = new SmtpClient(smtpServer);
				smtp.Credentials           = new System.Net.NetworkCredential(smtpUser,smtpPassword);
				if ( smtpPort > 0 )
					smtp.Port               = smtpPort;
//				smtp.UseDefaultCredentials = false;
//				smtp.EnableSsl             = true;

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

					errNo              = 30245;
					mailMsg.From       = new MailAddress(emailFrom);
					mailMsg.Subject    = emailHeader.Replace("[ContractCode]",contractCode);
					mailMsg.Body       = emailBody.Replace("[ContractCode]",contractCode);
					mailMsg.IsBodyHtml = emailBody.ToUpper().Contains("<HTML");
//					errNo              = 30250;
//					if ( pdfFileName.Length > 0 )
//						mailMsg.Attachments.Add(new Attachment(pdfFileName));
					errNo              = 30255;

					for ( int k = 0 ; k < 5 ; k++ )
						try
						{
							smtp.Send(mailMsg);
							errNo         = 0;
							lblError.Text = "";
							break;
						}
						catch (Exception ex1)
						{
							if ( k > 1 ) // After 2 failed attempts
								smtp.UseDefaultCredentials = false;
							if ( k > 2 ) // After 3 failed attempts
								Tools.LogException("Register.aspx/84","Mail send failure, errNo=" + errNo.ToString() + " (" + txtEMail.Text+")",ex1);
						}
				}
				SetErrorDetail(errNo,errNo,"Unable to send confirmation email (5 failed attempts)","");
				smtp  = null;
				errNo = 0;
			}
			catch (Exception ex2)
			{
				SetErrorDetail(99,errNo,"Unable to send confirmation email (SMTP error)",ex2.Message);
				return errNo;
			}

			SetErrorDetail(errNo,errNo,"Unable to send confirmation email (SQL error)",sql);
			return errNo;
		}

		private void SetErrorDetail(int errCode,int logNo,string errBrief,string errDetail,byte briefMode=2,byte detailMode=1)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblError.Text    = "";
				lblErrorDtl.Text = "";
				lblError.Visible = false;
				btnError.Visible = false;
				return;
			}

			Tools.LogInfo("Register.SetErrorDetail","(errCode="+errCode.ToString()+", logNo="+logNo.ToString()+") "+errDetail,244);

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else
				lblError.Text = errBrief;

			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
			if ( detailMode == 2 ) // Append
				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
			lblErrorDtl.Text = errDetail;
			if ( ! lblErrorDtl.Text.StartsWith("<div") )
				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white'>Error Details</div>" + lblErrorDtl.Text;

			lblError.Visible = ( lblError.Text.Length    > 0 );
			btnError.Visible = ( lblErrorDtl.Text.Length > 0 ) && lblError.Visible && ! Tools.SystemIsLive();
		}

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