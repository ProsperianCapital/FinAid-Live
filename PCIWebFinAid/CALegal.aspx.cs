using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class CALegal : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;

		string productCode;
		string languageCode;
		string languageDialectCode;
		string docTypeCode;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			errPriority = 241; // Log everything

			if ( Page.IsPostBack )
			{
//				productCode         = hdnProductCode.Value;
//				languageCode        = hdnLangCode.Value;
//				languageDialectCode = hdnLangDialectCode.Value;
//				docTypeCode         = hdnDocTypeCode.Value;
			}
			else
			{
				ret                            = 10010;
				productCode                    = WebTools.RequestValueString(Request,"PC");
				languageCode                   = WebTools.RequestValueString(Request,"LC");
				languageDialectCode            = WebTools.RequestValueString(Request,"LDC");
				docTypeCode                    = WebTools.RequestValueString(Request,"DT");
				ascxHeader.lstLanguage.Visible = false;
				btnErrorDtl.Visible            = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );

//				hdnProductCode.Value     = productCode;
//				hdnLangCode.Value        = languageCode;
//				hdnLangDialectCode.Value = languageDialectCode;
//				hdnDocTypeCode.Value     = docTypeCode;

				using (MiscList mList = new MiscList())
					try
					{
						int    err;
						string spr;
						string fieldCode;
						string fieldValue;
						string fieldURL;
						string blocked;
						string stdParms = " @ProductCode="         + Tools.DBString(productCode)
					                   + ",@LanguageCode="        + Tools.DBString(languageCode)
					                   + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
						ret = 10030;
						spr = "sp_WP_Get_ProductContent";
						sql = "exec " + spr + stdParms;
						if ( mList.ExecQuery(sql,0) != 0 )
							SetErrorDetail("PageLoad", 10120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
						else if ( mList.EOF )
							SetErrorDetail("PageLoad", 10130, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
						else
							while ( ! mList.EOF )
							{
								ret        = 10040;
								fieldCode  = mList.GetColumn("WebsiteFieldCode");
								fieldValue = mList.GetColumn("WebsiteFieldValue");
								fieldURL   = mList.GetColumn("FieldHyperlinkTarget");
								blocked    = mList.GetColumn("Blocked");
								err        = WebTools.ReplaceControlText(this.Page,"X"+fieldCode,blocked,fieldValue,fieldURL,ascxHeader,ascxFooter);
								if ( err != 0 )
									SetErrorDetail("LoadDynamicDetails", 10050, "Unrecognized HTML control (X"+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceControlText('X"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
								mList.NextRow();
							}

						ret = 10030;
						spr = "sp_WP_Get_ProductLegalDocumentInfo";
						sql = "exec " + spr + stdParms + ",@LegalDocumentTypeCode=" + Tools.DBString(docTypeCode);
						if ( mList.ExecQuery(sql,0) != 0 )
							SetErrorDetail("PageLoad", 10040, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
						else if ( mList.EOF )
							SetErrorDetail("PageLoad", 10050, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
						else
						{
							ret          = 10060;
							xTitle.Text  = mList.GetColumn("DocumentTitle");
							xHeader.Text = mList.GetColumn("DocumentHeader");
							xText.Text   = mList.GetColumn("DocumentText",1,6);
						}
					}
					catch (Exception ex)
					{
						Tools.LogException("PageLoad/10099","ret="+ret.ToString(),ex,this);
					}
			}
		}
	}
}