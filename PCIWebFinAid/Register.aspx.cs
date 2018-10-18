using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( ! Page.IsPostBack )
			{
				lblAppDetails.Text = AppDetails.Summary();
				lblJS.Text         = WebTools.JavaScriptSource("NextPage(0)");
				LoadLabels();
			}
		}

		private void LoadLabels()
		{
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

					sql = "exec sp_WP_GetFieldData @ProductCode="         + Tools.DBString(productCode)
					                           + ",@LanguageCode="        + Tools.DBString(languageCode)
					                           + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);

					if ( miscList.ExecQuery(sql,0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode  = miscList.GetColumn("WebsiteFieldCode");
							fieldValue = miscList.GetColumn("FieldValue");
							controlID  = "";

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
									ctlLabel.ToolTip = miscList.GetColumn("FieldMessage");
							}
							miscList.NextRow();
						}
					else
						lblJS.Text = WebTools.JavaScriptSource("TestSetup()",lblJS.Text,1);

//	Title
					sql = "exec sp_WP_Get_Title"
					    + " @LanguageCode="        + Tools.DBString(languageCode)
					    + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");

//	Employment Status
					sql = "exec sp_WP_Get_EmploymentStatus"
					    + " @LanguageCode="        + Tools.DBString(languageCode)
					    + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");

//	Pay Date
					sql = "exec sp_WP_Get_PayDate"
					    + " @LanguageCode="        + Tools.DBString(languageCode)
					    + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					WebTools.ListBind(lstPayDay,sql,null,"PayDateCode","PayDateDescription");

//	Product Option
					sql = "exec sp_WP_Get_ProductOption"
					    + " @ProductCode="         + Tools.DBString(productCode);
					WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");

//	Payment Method
					sql = "exec sp_WP_Get_PaymentMethod"
					    + " @ProductCode="         + Tools.DBString(productCode)
					    + ",@LanguageCode="        + Tools.DBString(languageCode)
					    + ",@LanguageDialectCode=" + Tools.DBString(languageDialect);
					WebTools.ListBind(lstPayment,sql,null,"PaymentMethodCode","PaymentMethodDescription");
				}
				catch (Exception ex)
				{
					Tools.LogException("Register.LoadLabels","",ex);
				}
		}
	}
}