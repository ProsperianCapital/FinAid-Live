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
		string contractPIN;
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
				contractPIN         = WebTools.ViewStateString(ViewState,"ContractPIN");
				lblError.Text       = "";
				lblRegConf.Text     = "";
			}
			else
			{
//				Tools.LogInfo("Register.PageLoad","Inital load",logDebug);
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
				ViewState["ContractPIN"]         = contractPIN;

				LoadLabels();

				lblVer.Text     = "Version " + SystemDetails.AppVersion;
				lblVer.Visible  = ! Tools.SystemIsLive();
				btnNext.Visible = ( lblError.Text.Length == 0 );
			}
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
					string      screenGuide;
					string      regPageNo;
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
							regPageNo    = miscList.GetColumn("RegistrationPageNumber");
							controlID    = "";

							if ( logNo <= 10 )
								Tools.LogInfo("Register.LoadLabels/15","Row 1, FieldCode="+fieldCode+", FieldValue="+fieldValue,logDebug);
							logNo = 15;

						//	Page 6
							if ( regPageNo == "6" ) // Confirmation page
							{
								ctlLiteral = (Literal)FindControl("lbl"+fieldCode);
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;
								if ( fieldValue.ToUpper().StartsWith("TITLE") )
								{
									lblp6TitleX.Text = "Title (fieldCode " + fieldCode + ")";
									Tools.LogInfo("Register.LoadLabels/12","fieldCode="+fieldCode + ", fieldValue="+fieldValue);
								}
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
//	Deferred to the load of page 4
//					logNo = 70;
//	//				sql   = "exec sp_WP_Get_ProductOption"
//					sql   = "exec sp_WP_Get_ProductOptionA"
//					      + " @ProductCode="         + Tools.DBString(productCode)
//					      + ",@LanguageCode="        + Tools.DBString(languageCode)
//					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
//					      + ",@Income="              + hdnIncomeError.ToString();
//					Tools.LogInfo("Register.LoadLabels/70",sql,logDebug);
//					WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");

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
			lblError.Text = "Error retrieving new contract details ; please try again later";
			contractCode  = "";
			contractPIN   = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationC"
					    +     " @RegistrationPage   = 'Z'"
					    +     ",@WebsiteCode        =" + Tools.DBString(WebTools.RequestValueString(Request,"WVC"))
					    +     ",@ProductCode        =" + Tools.DBString(productCode)
					    +     ",@LanguageCode       =" + Tools.DBString(languageCode)
					    +     ",@GoogleUtmSource    =" + Tools.DBString(WebTools.RequestValueString(Request,"GUS"))
					    +     ",@GoogleUtmMedium    =" + Tools.DBString(WebTools.RequestValueString(Request,"GUM"))
					    +     ",@GoogleUtmCampaign  =" + Tools.DBString(WebTools.RequestValueString(Request,"GUC"))
					    +     ",@GoogleUtmTerm      =" + Tools.DBString(WebTools.RequestValueString(Request,"GUT"))
					    +     ",@GoogleUtmContent   =" + Tools.DBString(WebTools.RequestValueString(Request,"GUN"))
					    +     ",@AdvertCode         =" + Tools.DBString(WebTools.RequestValueString(Request,"AC"))
					    +     ",@ClientIPAddress    =" + Tools.DBString(WebTools.RequestValueString(Request,"CIPA"))
					    +     ",@ClientDevice       =" + Tools.DBString(WebTools.RequestValueString(Request,"CD"))
					    +     ",@WebsiteVisitorCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVC"))
					    +     ",@WebsiteVisitorSessionCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVSC"));

					Tools.LogInfo("Register.GetContractCode/10",sql,logDebug);

					if ( miscList.ExecQuery(sql,0) == 0 )
					{
//						Tools.LogInfo("Register.GetContractCode/11","",logDebug);
						if ( ! miscList.EOF )
						{
//							Tools.LogInfo("Register.GetContractCode/12","",logDebug);
							contractCode = miscList.GetColumn("ContractCode");
							contractPIN  = miscList.GetColumn("PIN");
							string stat  = miscList.GetColumn("Status");
							if ( contractCode.Length > 0 && contractPIN.Length > 0 && ( stat == "0" || stat.Length == 0 ) )
								lblError.Text = "";
						}
						else
							Tools.LogInfo("Register.GetContractCode/13","",logDebug);
					}
					else
						Tools.LogInfo("Register.GetContractCode/14","",logDebug);

					Tools.LogInfo("Register.GetContractCode/20","ContractCode="+contractCode,logDebug);
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.GetContractCode",sql,ex);
					return false;
				}
				return ( lblError.Text.Length == 0 );
		}

		private string Validate(int pageNo)
		{
			string err = "";
			if ( pageNo == 1 )
			{
				txtSurname.Text = txtSurname.Text.Trim();
				if ( txtSurname.Text.Length < 2 )
					err = err + "Invalid surname (at least 2 characters required)<br />";
				txtCellNo.Text = txtCellNo.Text.Trim();
				if ( txtCellNo.Text.Replace(" ","").Length < 10 )
					err = err + "Invalid cell number (at least 10 digits required)<br />";
			}
			else if ( pageNo == 2 )
			{
				txtFirstName.Text = txtFirstName.Text.Trim();
				if ( txtFirstName.Text.Length < 1 )
					err = err + "Invalid first name (at least 1 character required)<br />";
			}
			return err;
		}

		protected void btnNext_Click(Object sender, EventArgs e)
		{
			int statusCode = 900;
			int pageNo     = Tools.StringToInt(hdnPageNo.Value);
			lblError.Text  = "Page numbering error ; please try again later";

			if ( pageNo < 1 )
				return;

			lblError.Text = Validate(pageNo);

			if ( lblError.Text.Length > 0 )
				return;

			lblError.Text = "Internal database error ; please try again later";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationB"
					    +     " @RegistrationPage =" + Tools.DBString((pageNo-1).ToString())
					    +     ",@ContractCode     =" + Tools.DBString(contractCode);

					if ( Tools.LiveTestOrDev() == Constants.SystemMode.Development )
					{ }

					else if ( pageNo == 1 )
						sql = sql + ",@TitleCode        =" + Tools.DBString(WebTools.ListValue(lstTitle,""))
					             + ",@Surname          =" + Tools.DBString(txtSurname.Text)
					             + ",@TelephoneNumberM =" + Tools.DBString(txtCellNo.Text);
					else if ( pageNo == 2 )
						sql = sql + ",@FirstName    =" + Tools.DBString(txtFirstName.Text)
					             + ",@EMailAddress =" + Tools.DBString(txtEMail.Text)
					             + ",@ClientCode   =" + Tools.DBString(txtID.Text);
					else if ( pageNo == 3 )
						sql = sql + ",@DisposableIncome           =" + Tools.DBString(txtIncome.Text)
					             + ",@ClientEmploymentStatusCode =" + Tools.DBString(WebTools.ListValue(lstStatus,""))
					             + ",@PayDateCode                =" + Tools.DBString(WebTools.ListValue(lstPayDay,""));
					else if ( pageNo == 4 )
						sql = sql + ",@ProductOptionCode =" + Tools.DBString(WebTools.ListValue(lstOptions,""))
					             + ",@TsCsRead          = '1'"
					             + ",@PaymentMethodCode =" + Tools.DBString(WebTools.ListValue(lstPayment,""));
					else if ( pageNo == 5 )
						sql = sql + ",@CardNumber      =" + Tools.DBString(txtCCNumber.Text)
					             + ",@AccountHolder   =" + Tools.DBString(txtCCName.Text)
					             + ",@CardExpiryMonth =" + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
					             + ",@CardExpiryYear  =" + Tools.DBString(WebTools.ListValue(lstCCYear).ToString())
					             + ",@CardCVVCode     =" + Tools.DBString(txtCCCVV.Text);

					Tools.LogInfo("Register.btnNext_Click/10",sql,logDebug);
					miscList.ExecQuery(sql,0);
//					statusCode = System.Convert.ToInt32(miscList.GetColumn("Status"));
					statusCode = 0;

					if ( pageNo == 5 && statusCode == 0 )
					{
						sql = "exec WP_ContractApplicationB"
						    +     " @RegistrationPage = '5'"
						    +     ",@ContractCode     =" + Tools.DBString(contractCode);
						Tools.LogInfo("Register.btnNext_Click/20",sql,logDebug);
						miscList.ExecQuery(sql,0);
//						statusCode = System.Convert.ToInt32(miscList.GetColumn("Status"));
						statusCode = 0;
					}			

					if ( statusCode == 0 )
					{
						pageNo++;
						hdnPageNo.Value = pageNo.ToString();
						lblError.Text   = "";

						if ( pageNo == 4 )
						{
							sql   = "exec sp_WP_Get_ProductOptionA"
							      + " @ProductCode="         +  Tools.DBString(productCode)
							      + ",@LanguageCode="        +  Tools.DBString(languageCode)
							      + ",@LanguageDialectCode=" +  Tools.DBString(languageDialectCode)
							      + ",@Income="              + (Tools.StringToInt(txtIncome.Text)).ToString();
							Tools.LogInfo("Register.btnNext_Click/40",sql,logDebug);
							WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");
						}
						else if ( pageNo == 5 )
						{
							string productOption  = WebTools.ListValue(lstOptions,"X");
							string payMethod      = WebTools.ListValue(lstPayment,"X");
							txtCCName.Text        = txtFirstName.Text.Substring(0,1).ToUpper()
							                      + " "
							                      + txtSurname.Text.Substring(0,1).ToUpper()
							                      + txtSurname.Text.Substring(1).ToLower();
							lblCCDueDate.Text     = lstPayDay.SelectedItem.Text;
							lblCCMandate.Text     = "";
							lblCCMandateHead.Text = "";
//							lblCCMandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2);
							sql                   = "exec sp_WP_Get_ProductOptionMandateA"
							                      + " @ProductCode="         +  Tools.DBString(productCode)
							                      + ",@LanguageCode="        +  Tools.DBString(languageCode)
							                      + ",@LanguageDialectCode=" +  Tools.DBString(languageDialectCode);

							Tools.LogInfo("Register.btnNext_Click/50",sql,logDebug);
							if ( miscList.ExecQuery(sql,0) == 0 )
								while ( ! miscList.EOF )
									if ( ( miscList.GetColumn("ProductOptionCode") == productOption &&
									       miscList.GetColumn("PaymentMethodCode") == payMethod )   ||
									       miscList.GetColumn("PaymentMethodCode") == "*" )
									{
										lblCCMandate.Text = miscList.GetColumn("CollectionMandateText",0);
										if ( lblCCMandate.Text.Length == 0 )
											lblCCMandate.Text = miscList.GetColumn("CollentionMandateText",0);
										int k = lblCCMandate.Text.IndexOf("\n");
										if ( k > 0 )
										{
											lblCCMandateHead.Text = lblCCMandate.Text.Substring(0,k);
											lblCCMandate.Text     = lblCCMandate.Text.Substring(k+1).Replace("\n","<br />");
										}
										lblp6Mandate.Text     = lblCCMandate.Text;
										lblp6MandateHead.Text = lblCCMandateHead.Text;
										break;
									}
									else
										miscList.NextRow();

							if ( lblCCMandate.Text.Length < 1 )
								lblError.Text = "Error retrieving collection mandate ; please try again later";
						}
						else if ( pageNo == 6 )
						{
							lblp6Agreement.Text = "";
							sql = "exec sp_WP_Get_ProductLegalDocumentDetail"
							    + " @ProductCode="         + Tools.DBString(productCode)
							    + ",@LanguageCode="        + Tools.DBString(languageCode)
							    + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
							    + ",@ProductLegalDocumentTypeCode='005'";
							Tools.LogInfo("Register.btnNext_Click/60",sql,logDebug);
							if ( miscList.ExecQuery(sql,0) == 0 )
								if ( ! miscList.EOF )
									lblp6Agreement.Text = "<u>" + miscList.GetColumn("ProductLegalDocumentParagraphHeader") + "</u><br />"
									                            + miscList.GetColumn("ProductLegalDocumentParagraphText").Replace("\n","<br />")
									                            + miscList.GetColumn("ProductLegalDocumentParagraphText2").Replace("\n","<br />");

							lblp6CCType.Text = "";
							sql = "exec WP_Get_CardAssociation"
							    + " @BIN=" + Tools.DBString(txtCCNumber.Text.Trim().Substring(0,6));
							Tools.LogInfo("Register.btnNext_Click/70",sql,logDebug);
							if ( miscList.ExecQuery(sql,0) == 0 )
								if ( ! miscList.EOF )
									lblp6CCType.Text = miscList.GetColumn("Brand");

							lblRegConf.Text     = " Confirmation";
							lblp6Ref.Text       = contractCode;
							lblp6Pin.Text       = contractPIN;
							lblp6Title.Text     = lstTitle.SelectedItem.Text;
							lblp6FirstName.Text = txtFirstName.Text;
							lblp6Surname.Text   = txtSurname.Text;
							lblp6EMail.Text     = txtEMail.Text;
							lblp6Cell.Text      = txtCellNo.Text;
							lblp6ID.Text        = txtID.Text;
							lblp6Income.Text    = txtIncome.Text;
							lblp6Status.Text    = lstStatus.SelectedItem.Text;
							lblp6PayDay.Text    = lstPayDay.SelectedItem.Text;
							lblp6Option.Text    = hdnOption.Value;
							lblp6PayMethod.Text = lstPayment.SelectedItem.Text;
							lbl100209.Text      = lbl100209.Text.Replace("[Title]",lstTitle.SelectedItem.Text).Replace("[Initials]",txtFirstName.Text.Substring(0,1).ToUpper()).Replace("[Surname]",txtSurname.Text+"<br />");
							lblp6CCName.Text    = txtCCName.Text;
							lblp6CCNumber.Text  = txtCCNumber.Text;
							lblp6CCExpiry.Text  = lstCCYear.SelectedValue + "/" + lstCCMonth.SelectedValue;
							lblp6Date.Text      = Tools.DateToString(System.DateTime.Now,2,1);
							lblp6IP.Text        = WebTools.RequestValueString(Request,"CIPA");
							lblp6Browser.Text   = WebTools.RequestValueString(Request,"CD");
						}
					}
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.btnNext_Click/90",sql,ex);
					lblError.Text = "Internal database error ; please try again later";
				}

			if ( statusCode != 0 || lblError.Text.Length > 0 )
			{
				if ( lblError.Text.Length == 0 )
					lblError.Text = "Internal error ; please try again later";
				return;
			}
		}
	}
}