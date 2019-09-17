using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		byte logDebug = 240;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( ! Page.IsPostBack )
			{
				Tools.LogInfo("Register.PageLoad","Inital load",logDebug);
				lblAppDetails.Text = AppDetails.Summary();
				lblJS.Text         = WebTools.JavaScriptSource("NextPage(0)");
				lblVersion.Text    = "Version " + SystemDetails.AppVersion;
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
					Label       ctlLabel;
					string      fieldCode;
					string      fieldValue;
					string      controlID;
					string      sql;
					string      productCode     = Tools.ConfigValue("ProductCode");
					string      languageCode    = Tools.ConfigValue("LanguageCode");
					string      languageDialect = Tools.ConfigValue("LanguageDialectCode");

//	Static labels, help text, etc
					logNo = 10;
					sql   = "exec sp_WP_GetFieldData @ProductCode="         + Tools.DBString(productCode)
					                             + ",@LanguageCode="        + Tools.DBString(languageCode)
					                             + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);

					Tools.LogInfo("Register.LoadLabels/10",sql,logDebug);

					if ( miscList.ExecQuery(sql,0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode  = miscList.GetColumn("WebsiteFieldCode");
							fieldValue = miscList.GetColumn("FieldValue");
							controlID  = "";

							if ( logNo <= 10 )
								Tools.LogInfo("Register.LoadLabels/15","Row 1, FieldCode="+fieldCode+", FieldValue="+fieldValue,logDebug);
							logNo = 15;

						//	Page 1
							if      ( fieldCode == "100111" ) controlID = "Title";
							else if ( fieldCode == "100114" ) controlID = "Surname";
							else if ( fieldCode == "100117" ) controlID = "CellNo";

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
								ctlLiteral = (Literal)FindControl("lbl"+controlID+"Label");
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;

								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Help");
								if ( ctlHidden != null )
									ctlHidden.Value = miscList.GetColumn("FieldScreenGuide");

								ctlLabel   = (Label)FindControl("lbl"+controlID+"Error");
								if ( ctlLabel != null )
								{
									ctlLabel.ToolTip = miscList.GetColumn("FieldMessage");
									if ( ctlLabel.ToolTip.Length == 0 )
										ctlLabel.ToolTip = miscList.GetColumn("FieldScreenGuide");
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
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					Tools.LogInfo("Register.LoadLabels/40",sql,logDebug);
					WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");

//	Employment Status
					logNo = 50;
					sql   = "exec sp_WP_Get_EmploymentStatus"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					Tools.LogInfo("Register.LoadLabels/50",sql,logDebug);
					WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");

//	Pay Date
					logNo = 60;
					sql   = "exec sp_WP_Get_PayDate"
					      + " @LanguageCode="        + Tools.DBString(languageCode)
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
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
					      + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
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
	}
}