using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class SMHomeTest : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;
		string countryCode;
		string productCode;
		string languageCode;
		string languageDialectCode;
		string promoCode;

		protected Literal F12014; // Favicon

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = Tools.SystemCode(Constants.ApplicationCode.SmartMoney);
			errPriority     = 19;

			if ( Page.IsPostBack )
			{
				countryCode         = hdnCountryCode.Value;
				productCode         = hdnProductCode.Value;
				languageCode        = hdnLangCode.Value;
				languageDialectCode = hdnLangDialectCode.Value;
				promoCode           = hdnPromoCode.Value;
				ListItem lang       = ascxHeader.lstLanguage.SelectedItem;

				if ( lang != null && lang.Value != languageCode + "|" + languageDialectCode )
				{
					languageCode        = lang.Text;
					languageDialectCode = lang.Value;
					int k               = languageDialectCode.IndexOf("|");
					languageDialectCode = languageDialectCode.Substring(k+1);
					SaveHiddenVars();
					LoadDynamicDetails();
				}
			}
			else
			{
				LoadPromo();
				LoadProduct();
				LoadStaticDetails();
				LoadDynamicDetails();
				if ( Tools.SystemLiveTestOrDev() != Constants.SystemMode.Development )
				{
					LoadGoogleAnalytics();
					LoadChat();
				}
				SaveHiddenVars();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
			}
		}

		private void SaveHiddenVars()
		{
			hdnCountryCode.Value     = countryCode;
			hdnProductCode.Value     = productCode;
			hdnLangCode.Value        = languageCode;
			hdnLangDialectCode.Value = languageDialectCode;
		}

		private void LoadStaticDetails()
		{
			ret = 10003;

			if ( Tools.NullToString(Request["BackDoor"]) == ((int)Constants.SystemPassword.BackDoor).ToString() )
			{
				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("ENG","ENG|0002"));
				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("GRK","GRK|0001"));
				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("TUR","TUR|0001"));
//				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("THA","THA|0001"));
//				ascxHeader.lstLanguage.Items.Add(new System.Web.UI.WebControls.ListItem("GER","GER|0298"));
				Tools.LogInfo("LoadStaticDetails/10003","BackDoor, PC/LC/LDC/Promo="+productCode+"/"+languageCode+"/"+languageDialectCode+"/"+promoCode,222,this);
			}
			else
				using (MiscList mList = new MiscList())
					try
					{
						ret = 10010;
						spr = "sp_WP_Get_ProductLanguageInfo";
						sql = "exec " + spr + " @ProductCode=" + Tools.DBString(productCode);
						if ( mList.ExecQuery(sql,0) != 0 )
							SetErrorDetail("LoadStaticDetails", 10060, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
						else if ( mList.EOF )
							SetErrorDetail("LoadStaticDetails", 10070, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
						else
						{
							string       lCode;
							string       lDialectCode;
							DropDownList lstLang = ascxHeader.lstLanguage;

							while ( ! mList.EOF )
							{
								ret          = 10080;
								lCode        = mList.GetColumn("LanguageCode");
								lDialectCode = mList.GetColumn("LanguageDialectCode");
							//	blocked      = mList.GetColumn("Blocked");
								Tools.LogInfo("LoadStaticDetails/10080","Language="+lCode+"/"+lDialectCode,errPriority,this);
								lstLang.Items.Add(new System.Web.UI.WebControls.ListItem(lCode,lCode+"|"+lDialectCode));
								if ( mList.GetColumn("DefaultIndicator").ToUpper() == "Y" ||
								   ( lCode == languageCode && lDialectCode == languageDialectCode ) )
								{
									ret                   = 10090;
									languageCode          = lCode;
									languageDialectCode   = lDialectCode;
									lstLang.SelectedIndex = lstLang.Items.Count - 1;
								}
								mList.NextRow();
							}
							if ( languageCode.Length == 0 )
							{
								ret                   = 10100;
								languageCode          = lstLang.Items[0].Text;
								languageDialectCode   = lstLang.Items[0].Value;
								int k                 = languageDialectCode.IndexOf("|");
								languageDialectCode   = languageDialectCode.Substring(k+1);
								lstLang.SelectedIndex = 0;
							}
						}
					}
					catch (Exception ex)
					{
						PCIBusiness.Tools.LogException("LoadStaticDetails/99","ret="+ret.ToString(),ex,this);
					}

			hdnVer.Value = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
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
					sql = "exec " + spr + stdParms + ",@PromoCode=" + Tools.DBString(promoCode);
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
							blocked    = mList.GetColumn("Blocked",0);
							err        = WebTools.ReplaceImage(this.Page,fieldCode,fieldValue,
							                                   mList.GetColumn   ("ImageMouseHoverText"),
							                                   fieldURL,
							                                   mList.GetColumnInt("ImageHeight"),
							                                   mList.GetColumnInt("ImageWidth"),
							                                   ascxHeader,
							                                   ascxFooter,
							                                   blocked);
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10197, "Unrecognized Image code ("+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceImage('"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							Tools.LogInfo("LoadDynamicDetails/10201","ImageCode="+fieldCode+"/"+fieldValue,errPriority,this);
							mList.NextRow();
						}

//	Show/Hide with JavaScript
//					ret = 10205;
//					spr = "";
//					if ( P12010.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12010',false);";
//					if ( P12011.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12011',false);";
//					if ( P12012.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12012',false);";
//					if ( P12023.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12023',false);";
//					if ( P12024.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12024',false);";
//					if ( P12028.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12028',false);";
//					ascxFooter.JSText = WebTools.JavaScriptSource(spr);

//	Show/hide with server-side code
					ret            = 10207;
					D12010.Visible = ( P12010.ImageUrl.Length > 4 );
					D12011.Visible = ( P12011.ImageUrl.Length > 4 );
					D12012.Visible = ( P12012.ImageUrl.Length > 4 );
					D12023.Visible = ( P12023.ImageUrl.Length > 4 );
					D12024.Visible = ( P12024.ImageUrl.Length > 4 );
					D12028.Visible = ( P12028.ImageUrl.Length > 4 );

					ret       = 10210;
					xHIW.Text = "";
					spr       = "sp_WP_Get_ProductHIWInfo";
					sql       = "exec " + spr + stdParms + ",@PromoCode=" + Tools.DBString(promoCode);
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

					ret       = 10310;
					xFAQ.Text = "";
					spr       = "sp_WP_Get_ProductFAQInfo";
					sql       = "exec " + spr + stdParms;
					string hd = "*P*";

					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10320, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10330, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10340;
							fieldCode  = mList.GetColumn("FAQ"      ,0,6);
							fieldHead  = mList.GetColumn("FAQHeader",0,6);
							fieldValue = mList.GetColumn("FAQDetail",0,6);
							if ( fieldHead != hd )
							{
								ret = 10350;
								if ( hd == "*P*" )
									xFAQ.Text = xFAQ.Text + "<hr /><a href='JavaScript:FAQ()' title='Close' style='float:right;padding:4px'><img src='" + Tools.ImageFolder() + "Close1.png' /></a>";
								xFAQ.Text    = xFAQ.Text + "<p class='FAQHead'>" + fieldHead  + "</p>";
								hd           = fieldHead;
							}
							ret       = 10360;
							xFAQ.Text = xFAQ.Text + "<p class='FAQQuestion'>" + fieldCode  + "</p>"
								                   + "<p class='FAQAnswer'>"   + fieldValue + "</p>";
							Tools.LogInfo("LoadDynamicDetails/10240","FAQ="+fieldCode+"/"+fieldHead,errPriority,this);
							mList.NextRow();
						}

					X100063.Visible = ( xFAQ.Text.Length > 0 );
					ret             = 10410;
					spr             = "sp_WP_Get_ProductLegalDocumentInfo";
					sql             = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10420, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10430, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret       = 10440;
							fieldCode = mList.GetColumn("DocumentTypeCode");
							try
							{
								((Literal)FindControl("LH"+fieldCode)).Text = mList.GetColumn("DocumentHeader",1,6);
								((Literal)FindControl("LD"+fieldCode)).Text = mList.GetColumn("DocumentText",1,6);
							}
							catch
							{ }
							mList.NextRow();
						}
				}
				catch (Exception ex)
				{
					Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}

		private void LoadPromo()
		{
			promoCode          = WebTools.RequestValueString(Request,"PromoCode");
			if ( promoCode.Length < 1 )
				promoCode       = "10001"; // Default
			hdnPromoCode.Value = promoCode;
		}	

		private void LoadProduct()
		{
			byte ret  = WebTools.LoadProductFromURL(Request,ref countryCode,ref productCode,ref languageCode,ref languageDialectCode,true);
			if ( ret != 0 || productCode.Length < 1 || languageCode.Length < 1 || languageDialectCode.Length < 1 )
			{
				SetErrorDetail("LoadProduct", 10888, "Unable to load product/language details", "ret="+ret.ToString(), 2, 2, null, false, errPriority);
				productCode         = "10472";
				languageCode        = "ENG";
				languageDialectCode = "0002";
			}

//		See SaveHiddenVars()
//			hdnCountryCode.Value     = countryCode;
//			hdnProductCode.Value     = productCode;
//			hdnLangCode.Value        = languageCode;
//			hdnLangDialectCode.Value = languageDialectCode;

//			Tools.LogInfo("LoadProduct","PC/LC/LDC="+productCode+"/"+languageCode+"/"+languageDialectCode,10,this);
		}	

		private void LoadChat()
		{
			lblChat.Text = Tools.LoadChat(productCode);
		}	

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text       = Tools.LoadGoogleAnalytics(productCode);
			lblGoogleNoScript.Text = Tools.LoadGoogleAnalytics(productCode,0,"",1);
		}
	}
}