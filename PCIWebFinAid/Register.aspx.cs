using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
		string sql;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( Page.IsPostBack )
			{
//				lblJS.Text          = WebTools.JavaScriptSource("NextPage(0)");
				productCode         = WebTools.ViewStateString(ViewState,"ProductCode");
				languageCode        = WebTools.ViewStateString(ViewState,"LanguageCode");
				languageDialectCode = WebTools.ViewStateString(ViewState,"LanguageDialectCode");
				contractCode        = WebTools.ViewStateString(ViewState,"ContractCode");
			}
			else
			{
				Tools.LogInfo("Register.PageLoad","Inital load",logDebug);
//				lblJS.Text          = WebTools.JavaScriptSource("SetEltValue('hdnPageNo','1');pageNo=1;NextPage(0)");
				lblJS.Text          = WebTools.JavaScriptSource("NextPage(0)");
				productCode         = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				languageCode        = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				languageDialectCode = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");

//	Testing
				if ( productCode.Length         < 1 ) productCode         = "10278";
				if ( languageCode.Length        < 1 ) languageCode        = "ENG";
				if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";

				GetContractCode();

				ViewState["ProductCode"]         = productCode;
				ViewState["LanguageCode"]        = languageCode;
				ViewState["LanguageDialectCode"] = languageDialectCode;
				ViewState["ContractCode"]        = contractCode;

				LoadLabels();
			}
		}

		private void LoadLabels()
		{
			byte logNo = 5;
			Tools.LogInfo("Register.LoadLabels/5","",logDebug);

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
					string      screenGuide;
					string      controlID;
					int         k;

//	Static labels, help text, etc
					logNo = 10;
					sql   = "exec sp_WP_Get_ProductWebsiteRegContent @ProductCode="         + Tools.DBString(productCode)
					                                             + ",@LanguageCode="        + Tools.DBString(languageCode)
					                                             + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

					Tools.LogInfo("Register.LoadLabels/10",sql,logDebug);

					if ( miscList.ExecQuery(sql,0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode    = miscList.GetColumn("WebsiteFieldCode");
							fieldValue   = miscList.GetColumn("WebsiteFieldValue");
							fieldMessage = miscList.GetColumn("WebsiteFieldiMessage"); // Yes, this is spelt correctly
							screenGuide  = miscList.GetColumn("WebsiteFieldScreenGuide");
							fieldFail    = miscList.GetColumn("FieldValidationFailureText");
							fieldPass    = miscList.GetColumn("FieldValidationPassText");
							controlID    = "";

							if ( logNo <= 10 )
								Tools.LogInfo("Register.LoadLabels/15","Row 1, FieldCode="+fieldCode+", FieldValue="+fieldValue,logDebug);
							logNo = 15;

						//	Page 1
							if      ( fieldCode == "100111" ) controlID      = "Title";
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
							else if ( fieldCode == "100191" )
								lblMandateHead.Text = fieldValue;
							else if ( fieldCode == "100192" )
								lblMandateDetail.Text = fieldValue;

							logNo = 20;

							if ( controlID.Length > 0 )
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

//	Title
					logNo = 40;
					sql   = "exec sp_WP_Get_Title"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					Tools.LogInfo("Register.LoadLabels/40",sql,logDebug);
					WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");

//	Employment Status
					logNo = 50;
					sql   = "exec sp_WP_Get_EmploymentStatus"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					Tools.LogInfo("Register.LoadLabels/50",sql,logDebug);
					WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");

//	Pay Date
					logNo = 60;
					sql   = "exec sp_WP_Get_PayDate"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					Tools.LogInfo("Register.LoadLabels/60",sql,logDebug);
					WebTools.ListBind(lstPayDay,sql,null,"PayDateCode","PayDateDescription");

//	Product Option
					logNo = 70;
					sql   = "exec sp_WP_Get_ProductOption"
					      + " @ProductCode="         + Tools.DBString(productCode);
					Tools.LogInfo("Register.LoadLabels/70",sql,logDebug);
					WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");

//	Payment Method
					logNo = 80;
					sql   = "exec sp_WP_Get_PaymentMethod"
					      + " @ProductCode="         + Tools.DBString(productCode)
					      + ",@LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					Tools.LogInfo("Register.LoadLabels/80",sql,logDebug);
					WebTools.ListBind(lstPayment,sql,null,"PaymentMethodCode","PaymentMethodDescription");
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.LoadLabels","logNo=" + logNo.ToString(),ex);
				}

			lstCCYear.Items.Clear();
			lstCCYear.Items.Add(new ListItem("(Select one)","0"));
			for ( int y = System.DateTime.Now.Year ; y < System.DateTime.Now.Year+15 ; y++ )
				lstCCYear.Items.Add(new ListItem(y.ToString(),y.ToString()));
		}

		private bool GetContractCode()
		{
			contractCode = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationA"
					    +     " @RegistrationPage   = 'Z'"
					    +     ",@WebsiteCode        =" + Tools.DBString(WebTools.RequestValueString(Request,"WebSiteCode"))
					    +     ",@ProductCode        =" + Tools.DBString(productCode)
					    +     ",@LanguageCode       =" + Tools.DBString(languageCode)
					    +     ",@GoogleUtmSource    =" + Tools.DBString(WebTools.RequestValueString(Request,"GoogleUtmSource"))
					    +     ",@GoogleUtmMedium    =" + Tools.DBString(WebTools.RequestValueString(Request,"GoogleUtmMedium"))
					    +     ",@GoogleUtmCampaign  =" + Tools.DBString(WebTools.RequestValueString(Request,"GoogleUtmCampaign"))
					    +     ",@GoogleUtmTerm      =" + Tools.DBString(WebTools.RequestValueString(Request,"GoogleUtmTerm"))
					    +     ",@GoogleUtmContent   =" + Tools.DBString(WebTools.RequestValueString(Request,"GoogleUtmContent"))
					    +     ",@AdvertCode         =" + Tools.DBString(WebTools.RequestValueString(Request,"AdvertCode"))
					    +     ",@ClientIPAddress    =" + Tools.DBString(WebTools.RequestValueString(Request,"ClientIPAddress"))
					    +     ",@ClientDevice       =" + Tools.DBString(WebTools.RequestValueString(Request,"ClientDevice"))
					    +     ",@WebsiteVisitorCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WebsiteVisitorCode"))
					    +     ",@WebsiteVisitorSessionCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WebsiteVisitorSessionCode"));

					Tools.LogInfo("Register.GetContractCode/10",sql,logDebug);

					if ( miscList.ExecQuery(sql,0) == 0 )
						if ( ! miscList.EOF )
							contractCode = miscList.GetColumn("ContractCode");
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.GetContractCode",sql,ex);
					return false;
				}
				return ( contractCode.Length > 0 );
		}

		protected void btnNext_Click(Object sender, EventArgs e)
		{
			int pageNo = Tools.StringToInt(hdnPageNo.Value);
			if ( pageNo < 1 )
				return;

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationA"
					    +     " @RegistrationPage =" + Tools.DBString((pageNo-1).ToString())
					    +     ",@ContractCode     =" + Tools.DBString(contractCode);

					if ( pageNo == 1 )
						sql = sql + ",@TitleCode        =" + Tools.DBString(WebTools.ListValue(lstTitle).ToString())
					             + ",@Surname          =" + Tools.DBString(txtSurname.Text)
					             + ",@TelephoneNumberM =" + Tools.DBString(txtCellNo.Text);
					else if ( pageNo == 2 )
						sql = sql + ",@FirstName    =" + Tools.DBString(txtFirstName.Text)
					             + ",@EMailAddress =" + Tools.DBString(txtEMail.Text)
					             + ",@ClientCode   =" + Tools.DBString(txtID.Text);
					else if ( pageNo == 3 )
						sql = sql + ",@DisposableIncome           =" + Tools.DBString(txtIncome.Text)
					             + ",@ClientEmploymentStatusCode =" + Tools.DBString(WebTools.ListValue(lstStatus).ToString())
					             + ",@PayDateCode                =" + Tools.DBString(WebTools.ListValue(lstPayDay).ToString());
					else if ( pageNo == 4 )
						sql = sql + ",@ProductOptionCode =" + Tools.DBString(WebTools.ListValue(lstOptions).ToString())
					             + ",@TsCsRead          = '1'"
					             + ",@PaymentMethodCode =" + Tools.DBString(WebTools.ListValue(lstPayment).ToString());
					else if ( pageNo == 5 )
						sql = sql + ",@CardNumber      =" + Tools.DBString(txtCCNumber.Text)
					             + ",@AccountHolder   =" + Tools.DBString(txtCCName.Text)
					             + ",@CardExpiryMonth =" + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
					             + ",@CardExpiryYear  =" + Tools.DBString(WebTools.ListValue(lstCCYear).ToString())
					             + ",@CardCVVCode     =" + Tools.DBString(txtCCCVV.Text);

					Tools.LogInfo("Register.btnNext_Click/10",sql,logDebug);

					miscList.ExecQuery(sql,0);

					if ( pageNo == 5 )
					{
						sql = "exec WP_ContractApplicationA"
						    +     " @RegistrationPage = '5'"
						    +     ",@ContractCode     =" + Tools.DBString(contractCode);
						Tools.LogInfo("Register.btnNext_Click/20",sql,logDebug);
						miscList.ExecQuery(sql,0);
					}			

				//	Check for error
				//	if error, show message and return
				//	if no error, do the following:

					pageNo++;
					hdnPageNo.Value = pageNo.ToString();
//					lblError.Text   = "";
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.GetContractCode",sql,ex);
				}
		}
	}
}