using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LGHome : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;
		string productCode;
		string languageCode;
		string languageDialectCode;

		protected Literal F12014; // Favicon

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = "120";
			errPriority     = 19;

			if ( Page.IsPostBack )
			{
				productCode         = hdnProductCode.Value;
				languageCode        = hdnLangCode.Value;
				languageDialectCode = hdnLangDialectCode.Value;
			}
			else
			{
				LoadStaticDetails();
				LoadDynamicDetails();
				LoadGoogleAnalytics();
				LoadChat();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
			}
		}

		private void LoadStaticDetails()
		{
		//	Defaults
			productCode         = "10387";
			languageCode        = "ENG";
			languageDialectCode = "0002";
			ret                 = 10003;

//			if ( Tools.NullToString(Request["BackDoor"]) == ((int)Constants.SystemPassword.BackDoor).ToString() )
//			{
//				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("ENG","0002"));
//				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("THA","0001"));
//				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("GER","0298"));
//				Tools.LogInfo("LoadStaticDetails/10003","BackDoor, PC/LC/LDC="+productCode+"/"+languageCode+"/"+languageDialectCode,222,this);
//			}
//			else

			using (MiscList mList = new MiscList())
				try
				{
					ret             = 10008;
//					string pageName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
//					string pageName = Request.Url.LocalPath;
					string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
					string refer    = Request.Url.AbsoluteUri.Trim();
					int    k        = refer.IndexOf("://");
					refer           = refer.Substring(k+3);

					if ( ! pageName.StartsWith("/") )
						pageName = "/" + pageName;

					k = refer.ToUpper().IndexOf(pageName.ToUpper());
					if ( k > 0 )
						refer = refer.Substring(0,k);

					ret = 10010;
					spr = "sp_WP_Get_WebsiteInfoByURL";
					sql = "exec " + spr + " " + Tools.DBString(refer);
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10020, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10030, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
					{
						ret                 = 10020;
						productCode         = mList.GetColumn("ProductCode");
						languageCode        = mList.GetColumn("LanguageCode");
						languageDialectCode = mList.GetColumn("LanguageDialectCode");
						ret                 = 10030;
						if ( productCode.Length         < 1 ) productCode         = "10387";
						if ( languageCode.Length        < 1 ) languageCode        = "ENG";
						if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
					}

					Tools.LogInfo("LoadStaticDetails/10040",sql+" ... PC/LC/LDC="+productCode+"/"+languageCode+"/"+languageDialectCode,10,this);
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadStaticDetails/10050","ret="+ret.ToString(),ex,this);
				}

//	Override if passed via URL
			ret      = 10044;
			string h = WebTools.RequestValueString(Request,"PC");
			if ( h.Length > 0 ) productCode = h;
			h        = WebTools.RequestValueString(Request,"LC");
			if ( h.Length > 0 ) languageCode = h;
			h        = WebTools.RequestValueString(Request,"LDC");
			if ( h.Length > 0 ) languageDialectCode = h;

			hdnProductCode.Value     = productCode;
			hdnLangCode.Value        = languageCode;
			hdnLangDialectCode.Value = languageDialectCode;
			hdnVer.Value             = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
		}

		private void LoadDynamicDetails()
		{
			byte   err;
			string fieldCode;
			string fieldHead;
			string fieldValue;
			string fieldURL;
			string blocked;
			string stdParms = " @ProductCode="         + Tools.DBString(productCode)
					          + ",@LanguageCode="        + Tools.DBString(languageCode)
					          + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

			using (MiscList mList = new MiscList())
				try
				{
					ret = 10110;
					spr = "sp_WP_Get_ProductContent";
					sql = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10130, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret         = 10140;
							fieldCode   = mList.GetColumn("WebsiteFieldCode");
						//	fieldName   = mList.GetColumn("WebsiteFieldName");
							blocked     = mList.GetColumn("Blocked");
							fieldValue  = mList.GetColumn("WebsiteFieldValue");
							fieldURL    = mList.GetColumn("FieldHyperlinkTarget");
							if ( fieldURL.Length > 0 && fieldURL.Contains("[") )
								fieldURL = fieldURL.Replace("[PC]",Tools.URLString(productCode)).Replace("[LC]",Tools.URLString(languageCode)).Replace("[LDC]",Tools.URLString(languageDialectCode));

							Tools.LogInfo("LoadDynamicDetails/10140","FieldCode="+fieldCode,errPriority,this);
							err         = WebTools.ReplaceControlText(this.Page,"X"+fieldCode,blocked,fieldValue,fieldURL,ascxHeader,ascxFooter);
							if ( err   != 0 )
								SetErrorDetail("LoadDynamicDetails", 10150, "Unrecognized HTML control (X"+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceControlText('X"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10160;
					spr = "sp_WP_Get_ProductImageInfo";
					sql = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10170, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10180, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10190;
							fieldCode  = mList.GetColumn("ImageCode");
							fieldValue = mList.GetColumn("ImageFileName");
							fieldURL   = mList.GetColumn("ImageHyperLink");
							Tools.LogInfo("LoadDynamicDetails/10190","ImageCode="+fieldCode+"/"+fieldValue,errPriority,this);
							err        = WebTools.ReplaceImage(this.Page,fieldCode,fieldValue,
							                                   mList.GetColumn   ("ImageMouseHoverText"),
							                                   fieldURL,
							                                   mList.GetColumnInt("ImageHeight"),
							                                   mList.GetColumnInt("ImageWidth"),
							                                   ascxHeader,
							                                   ascxFooter);
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10200, "Unrecognized Image code ("+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceImage('"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10205;
					spr = "";

//	Test					
//					WebTools.ReplaceImage(this.Page,"12014","favicon.ico","","",0,0);
//	Test

					ret       = 10210;
					xHIW.Text = "";
					spr       = "sp_WP_Get_ProductHIWInfo";
					sql       = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10220, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10230, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10240;
							fieldCode  = mList.GetColumn("Serial");
							fieldHead  = mList.GetColumn("HIWHeader",0,6);
							fieldValue = mList.GetColumn("HIWDetail",0,6);
							Tools.LogInfo("LoadDynamicDetails/10240","HIW="+fieldCode+"/"+fieldHead,errPriority,this);
							if ( "0123456789".Contains(fieldHead.Substring(0,1)) )
								xHIW.Text = xHIW.Text + "<p class='HIWHead1'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail1'>" + fieldValue + "</p>";
							else
								xHIW.Text = xHIW.Text + "<p class='HIWHead2'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail2'>" + fieldValue + "</p>";
							mList.NextRow();
						}

//					ret       = 10310;
//					xFAQ.Text = "";
//					spr       = "sp_WP_Get_ProductFAQInfo";
//					sql       = "exec " + spr + stdParms;
//					string hd = "*P*";

//					if ( mList.ExecQuery(sql,0) != 0 )
//						SetErrorDetail("LoadDynamicDetails", 10320, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
//					else if ( mList.EOF )
//						SetErrorDetail("LoadDynamicDetails", 10330, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
//					else
//						while ( ! mList.EOF )
//						{
//							ret        = 10340;
//							fieldCode  = mList.GetColumn("FAQ"      ,0,6);
//							fieldHead  = mList.GetColumn("FAQHeader",0,6);
//							fieldValue = mList.GetColumn("FAQDetail",0,6);
//							if ( fieldHead != hd )
//							{
//								ret = 10350;
//								if ( hd == "*P*" )
//									xFAQ.Text = xFAQ.Text + "<hr /><a href='JavaScript:FAQ()' title='Close' style='float:right;padding:4px'><img src='" + Tools.ImageFolder() + "Close1.png' /></a>";
//								xFAQ.Text    = xFAQ.Text + "<p class='FAQHead'>" + fieldHead  + "</p>";
//								hd           = fieldHead;
//							}
//							ret       = 10360;
//							xFAQ.Text = xFAQ.Text + "<p class='FAQQuestion'>" + fieldCode  + "</p>"
//								                   + "<p class='FAQAnswer'>"   + fieldValue + "</p>";
//							Tools.LogInfo("LoadDynamicDetails/10240","FAQ="+fieldCode+"/"+fieldHead,errPriority,this);
//							mList.NextRow();
//						}

//					X100063.Visible = ( xFAQ.Text.Length > 0 );
//					ret             = 10410;
//					spr             = "sp_WP_Get_ProductLegalDocumentInfo";
//					sql             = "exec " + spr + stdParms;
//					if ( mList.ExecQuery(sql,0) != 0 )
//						SetErrorDetail("LoadDynamicDetails", 10420, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
//					else if ( mList.EOF )
//						SetErrorDetail("LoadDynamicDetails", 10430, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
//					else
//						while ( ! mList.EOF )
//						{
//							ret       = 10440;
//							fieldCode = mList.GetColumn("DocumentTypeCode");
//							try
//							{
//								((Literal)FindControl("LH"+fieldCode)).Text = mList.GetColumn("DocumentHeader",1,6);
//								((Literal)FindControl("LD"+fieldCode)).Text = mList.GetColumn("DocumentText",1,6);
//							}
//							catch
//							{ }
//							mList.NextRow();
//						}

					pnlContact01.Visible = ( X100093.Text.Length > 0 );
					pnlContact02.Visible = ( X104402.Text.Length > 0 );
					pnlContact03.Visible = ( X100095.Text.Length > 0 );
					pnlContact04.Visible = ( X100096.Text.Length > 0 || P12031.ImageUrl.Length > 0 );
					pnlContact05.Visible = ( X100101.Text.Length > 0 );
					pnlContact06.Visible = ( X104404.Text.Length > 0 || P12032.ImageUrl.Length > 0 );
					pnlContact07.Visible = ( X100102.Text.Length > 0 || P12033.ImageUrl.Length > 0 );
					pnlContact08.Visible = ( X104418.Text.Length > 0 );
					pnlContact09.Visible = ( X100105.Text.Length > 0 || P12034.ImageUrl.Length > 0 );
					pnlBr1.Visible       = ( P12015.ImageUrl.Length > 0 ||
					                         P12016.ImageUrl.Length > 0 ||
					                         P12017.ImageUrl.Length > 0 ||
					                         P12018.ImageUrl.Length > 0 ||
					                         P12019.ImageUrl.Length > 0 );
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}

		private void LoadChat()
		{
			lblChat.Text = Tools.LoadChat(productCode);
		}	

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text = Tools.LoadGoogleAnalytics(productCode);
		}
	}
}