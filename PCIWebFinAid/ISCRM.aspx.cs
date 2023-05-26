using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ISCRM : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;
		string countryCode;
		string productCode;
		string languageCode;
		string languageDialectCode;

		protected Literal F12014; // Favicon

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = "210";
			errPriority     = 19;

			if ( Page.IsPostBack )
			{
				countryCode         = hdnCountryCode.Value;
				productCode         = hdnProductCode.Value;
				languageCode        = hdnLangCode.Value;
				languageDialectCode = hdnLangDialectCode.Value;
			}
			else
			{
				LoadProduct();
//				LoadStaticDetails();
				LoadDynamicDetails();
				LoadGoogleAnalytics();
//				LoadChat();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				hdnVer.Value        = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";

				EnableControls(1);
			}
		}

		private void EnableControls(byte seq)
		{
		//	Phone number
			X105103.Enabled = ( seq == 1 );
			X105133.Enabled = ( seq == 1 );

		//	Emregency data
			X105105.Enabled = ( seq == 2 );
			X105106.Enabled = ( seq == 2 );
			X105108.Enabled = ( seq == 2 );
			X105109.Enabled = ( seq == 2 );
			X105111.Enabled = ( seq == 2 );
			X105112.Enabled = ( seq == 2 );
			X105114.Enabled = ( seq == 2 );
			X105115.Enabled = ( seq == 2 );
			X105117.Enabled = ( seq == 2 );
			X105118.Enabled = ( seq == 2 );
			X105120.Enabled = ( seq == 2 );
			X105121.Enabled = ( seq == 2 );
			X105123.Enabled = ( seq == 2 );
			X105124.Enabled = ( seq == 2 );
			X105126.Enabled = ( seq == 2 );
			X105127.Enabled = ( seq == 2 );
			X105129.Enabled = ( seq == 2 );
			X105130.Enabled = ( seq == 2 );
			X105131.Enabled = ( seq == 2 ); // Button "Save"

			if ( seq == 1 )
			{
				imgOK.ImageUrl = "";
			//	imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Question.png";
				X105103.Text   = "";
				X105103.Focus();
			}
			else
			{
				imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Tick.png";
				X105105.Focus();
			}
		}

		private void LoadDynamicDetails()
		{
			byte   err;
			string fieldCode;
		//	string fieldHead;
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

						//	Tools.LogInfo("LoadDynamicDetails/10140","FieldCode="+fieldCode,errPriority,this);
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
						//	Tools.LogInfo("LoadDynamicDetails/10190","ImageCode="+fieldCode+"/"+fieldValue,errPriority,this);
							err        = WebTools.ReplaceImage(this.Page,fieldCode,fieldValue,
							                                   mList.GetColumn   ("ImageMouseHoverText"),
							                                   fieldURL,
							                                   mList.GetColumnInt("ImageHeight"),
							                                   mList.GetColumnInt("ImageWidth"),
							                                   ascxHeader,
							                                   ascxFooter,
							                                   blocked);
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10200, "Unrecognized Image code ("+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceImage('"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10205;
					spr = "";

//					if ( P12010.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12010',false);";
//					if ( P12011.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12011',false);";
//					if ( P12012.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12012',false);";
//					if ( P12023.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12023',false);";
//					if ( P12024.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12024',false);";
//					if ( P12028.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12028',false);";
//					ascxFooter.JSText = WebTools.JavaScriptSource(spr);

//	Removed
/*
					ret = 10410;
					spr = "sp_WP_Get_ProductLegalDocumentInfo";
					sql = "exec " + spr + stdParms;
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
*/

//	Testing
//					WebTools.ReplaceImage(this.Page,"12002","isos1.png","isos1");
//					WebTools.ReplaceImage(this.Page,"12036","isos2.png","isos2");
//					WebTools.ReplaceControlText(this.Page,"X105104","Label 105104","");
//					WebTools.ReplaceControlText(this.Page,"X105105","PlaceHolder 105105","");
//					WebTools.ReplaceControlText(this.Page,"X105106","PlaceHolder 105106","");
//					X105106.Text = "X105106 Default";
//					X105109.Text = "X105109 Default";
//					X105112.Text = "X105112 Default";
//					X105115.Text = "X105115 Default";
//					X105118.Text = "X105118 Default";
//					X105121.Text = "X105121 Default";
//					X105124.Text = "X105124 Default";
//					X105127.Text = "X105127 Default";
//					X105130.Text = "X105130 Default";
//	Testing
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}

		protected void btnGet_Click(Object sender, EventArgs e)
		{
			X105103.Focus();
			imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Cross.png";
			string phone   = X105103.Text.Trim().Replace(" ","").Replace("-","").Replace("(","").Replace(")","");
			
			if ( phone.Length < 6 )
				return;

			using (MiscList mList = new MiscList())
				try
				{
					ret = 14110;
					spr = "sp_WP_Get_iSOSUser";
					sql = "exec " + spr + " @MobileNumber=" + Tools.DBString(phone,47);
//	Testing
					if ( phone == "0844385400" )
						sql = "select '011 222 3333' as Button1Number,'Button 1 Msg' as Button1Message,"
					       +        "'066 888 1111' as Button6Number,'Button 6 Msg' as Button6Message,"
					       +        "'077 444 3333' as Button7Number,'Button 7 Msg' as Button7Message";
//	Testing
					ret = mList.ExecQuery(sql,0,"",false);
				//	Tools.LogInfo("btnGet_Click/1",sql + " (ret=" + ret.ToString() + ")",10,this);
					if ( ret == 0 )
					{
						EnableControls(2);
						if ( mList.EOF)
						{
							Tools.LogInfo("btnGet_Click/2",sql + " (ret=0, No data)",222,this);
						//	Show message "New customer"
						}
						else
						{
							X105105.Text = mList.GetColumn("Button1Number");
						//	X105106.Text = mList.GetColumn("Button1Message");
							X105108.Text = mList.GetColumn("Button2Number");
						//	X105109.Text = mList.GetColumn("Button2Message");
							X105111.Text = mList.GetColumn("Button3Number");
						//	X105112.Text = mList.GetColumn("Button3Message");
							X105114.Text = mList.GetColumn("Button4Number");
						//	X105115.Text = mList.GetColumn("Button4Message");
							X105117.Text = mList.GetColumn("Button5Number");
						//	X105118.Text = mList.GetColumn("Button5Message");
							X105120.Text = mList.GetColumn("Button6Number");
						//	X105121.Text = mList.GetColumn("Button6Message");
							X105123.Text = mList.GetColumn("Button7Number");
						//	X105124.Text = mList.GetColumn("Button7Message");
							X105126.Text = mList.GetColumn("Button8Number");
						//	X105127.Text = mList.GetColumn("Button8Message");
							X105129.Text = mList.GetColumn("Button9Number");
						//	X105130.Text = mList.GetColumn("Button9Message");

							string h     = mList.GetColumn("Button1Message");
							if ( h.Length > 0 ) X105106.Text = h;
							h            = mList.GetColumn("Button2Message");
							if ( h.Length > 0 ) X105109.Text = h;
							h            = mList.GetColumn("Button3Message");
							if ( h.Length > 0 ) X105112.Text = h;
							h            = mList.GetColumn("Button4Message");
							if ( h.Length > 0 ) X105115.Text = h;
							h            = mList.GetColumn("Button5Message");
							if ( h.Length > 0 ) X105118.Text = h;
							h            = mList.GetColumn("Button6Message");
							if ( h.Length > 0 ) X105121.Text = h;
							h            = mList.GetColumn("Button7Message");
							if ( h.Length > 0 ) X105124.Text = h;
							h            = mList.GetColumn("Button8Message");
							if ( h.Length > 0 ) X105127.Text = h;
							h            = mList.GetColumn("Button9Message");
							if ( h.Length > 0 ) X105130.Text = h;

							Tools.LogInfo("btnGet_Click/3","(1) " + X105105.Text + "/" + X105106.Text
							                           + ", (2) " + X105108.Text + "/" + X105109.Text  
							                           + ", (3) " + X105111.Text + "/" + X105112.Text  
							                           + ", (9) " + X105129.Text + "/" + X105130.Text,10,this);  
						}
					}
					else
						SetErrorDetail("btnGet_Click", 14120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
				}
				catch (Exception ex)
				{
					SetErrorDetail("btnGet_Click", 14130, "Internal database error (" + spr + " failed)", sql, 2, 2, ex, false, errPriority);
				}
		}

		protected void btnSave_Click(Object sender, EventArgs e)
		{
			using (MiscList mList = new MiscList())
				try
				{
					ret = 13110;
					spr = "sp_WP_Upd_iSOSUser";
					sql = "exec " + spr + " @MobileNumber="   + Tools.DBString(X105103.Text,47)
					                    + ",@Button1Number="  + Tools.DBString(X105105.Text,47)
					                    + ",@Button1Message=" + Tools.DBString(X105106.Text,47)
					                    + ",@Button2Number="  + Tools.DBString(X105108.Text,47)
					                    + ",@Button2Message=" + Tools.DBString(X105109.Text,47)
					                    + ",@Button3Number="  + Tools.DBString(X105111.Text,47)
					                    + ",@Button3Message=" + Tools.DBString(X105112.Text,47)
					                    + ",@Button4Number="  + Tools.DBString(X105114.Text,47)
					                    + ",@Button4Message=" + Tools.DBString(X105115.Text,47)
					                    + ",@Button5Number="  + Tools.DBString(X105117.Text,47)
					                    + ",@Button5Message=" + Tools.DBString(X105118.Text,47)
					                    + ",@Button6Number="  + Tools.DBString(X105120.Text,47)
					                    + ",@Button6Message=" + Tools.DBString(X105121.Text,47)
					                    + ",@Button7Number="  + Tools.DBString(X105123.Text,47)
					                    + ",@Button7Message=" + Tools.DBString(X105124.Text,47)
					                    + ",@Button8Number="  + Tools.DBString(X105126.Text,47)
					                    + ",@Button8Message=" + Tools.DBString(X105127.Text,47)
					                    + ",@Button9Number="  + Tools.DBString(X105129.Text,47)
					                    + ",@Button9Message=" + Tools.DBString(X105130.Text,47)
					                    + ",@LanguageCode="   + Tools.DBString(languageCode,47)
					                    + ",@CountryCode="    + Tools.DBString(countryCode ,47);
					ret = mList.ExecQuery(sql,0,"",false,true);
					if ( ret != 0 )
						SetErrorDetail("btnSave_Click", 13120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					Tools.LogInfo("btnSave_Click","Save iSOS config (ret="+ret.ToString() + ") : " +sql,10,this);
				}
				catch (Exception ex)
				{
					SetErrorDetail("btnSave_Click", 13130, "Internal database error (" + spr + " failed)", sql, 2, 2, ex, false, errPriority);
				}
		}

		private void LoadProduct()
		{
			byte ret = WebTools.LoadProductFromURL(Request,ref countryCode,ref productCode,ref languageCode,ref languageDialectCode,true);

			if ( ! Tools.SystemIsLive() )
			{
				if ( countryCode.Length         < 1 ) countryCode         = "ZA";
				if ( productCode.Length         < 1 ) productCode         = "10387";
				if ( languageCode.Length        < 1 ) languageCode        = "ENG";
				if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
			}	

			if ( ret != 0 || productCode.Length < 1 || languageCode.Length < 1 || languageDialectCode.Length < 1 )
				SetErrorDetail("LoadProduct", 10777, "Unable to load product/language details", "ret="+ret.ToString(), 2, 2, null, false, errPriority);

			hdnCountryCode.Value     = countryCode;
			hdnProductCode.Value     = productCode;
			hdnLangCode.Value        = languageCode;
			hdnLangDialectCode.Value = languageDialectCode;
		}	

//		private void LoadChat()
//		{
//			lblChat.Text = Tools.LoadChat(productCode);
//		}	

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text       = Tools.LoadGoogleAnalytics(productCode);
			lblGoogleNoScript.Text = Tools.LoadGoogleAnalytics(productCode,0,"",1);
		}
	}
}