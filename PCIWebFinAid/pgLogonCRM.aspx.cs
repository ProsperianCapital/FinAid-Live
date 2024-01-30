// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using PCIBusiness;

// Error codes 11000-11099

namespace PCIWebFinAid
{
	public partial class pgLogonCRM : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = "002"; // CareAssist CRM
			SetErrorDetail("",-888);

			if ( Page.IsPostBack )
				SessionCheck(5);

			else
			{
				SessionCheck(3);

				string pc  = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				string lc  = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				string ldc = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");

				if ( pc.Length  > 0 ) sessionGeneral.ProductCode         = pc;
				if ( lc.Length  > 0 ) sessionGeneral.LanguageCode        = lc;
				if ( ldc.Length > 0 ) sessionGeneral.LanguageDialectCode = ldc;

//	Testing [Start]
				if ( ! Tools.SystemIsLive() )
				{
//	English
					if ( sessionGeneral.ProductCode.Length           < 1 ) sessionGeneral.ProductCode         = "10387";
					if ( sessionGeneral.LanguageCode.Length          < 1 ) sessionGeneral.LanguageCode        = "ENG";
					if ( sessionGeneral.LanguageDialectCode.Length   < 1 ) sessionGeneral.LanguageDialectCode = "0002";
//	Thai
//					if ( sessionGeneral.ProductCode.Length           < 1 ) sessionGeneral.ProductCode         = "10024";
//					if ( sessionGeneral.LanguageCode.Length          < 1 ) sessionGeneral.LanguageCode        = "THA";
//					if ( sessionGeneral.LanguageDialectCode.Length   < 1 ) sessionGeneral.LanguageDialectCode = "0001";
				}
//	Testing [End]

				if ( sessionGeneral.ProductCode.Length         < 1 ||
				     sessionGeneral.LanguageCode.Length        < 1 ||
				     sessionGeneral.LanguageDialectCode.Length < 1 )
				{
					SetErrorDetail("PageLoad",10200,"Invalid startup values ... system cannot continue","");
					X103016.Text    = "Disabled"; // Login button
					X103016.Enabled = false;
					X103014.Enabled = false; // User name
					X103015.Enabled = false; // Password
				}
				else
				{
					LoadLabelText(null);
					LoadPageData();
					X103014.Focus(); // User name
					SessionSave();
//					SessionSave(null,null,null,null,productCode,languageCode,languageDialectCode);
				}
				ascxXHeader.ShowUser(null,ApplicationCode);

				string h = Tools.ErrorTypeName(WebTools.RequestValueInt(Request,"ErrType"));
				if ( h.Length > 0 )
					SetErrorDetail("PageLoad",10201,h,"");
			}
		}

		protected override void LoadPageData()
		{
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			X103014.Text    = X103014.Text.Trim(); // User name
			X103015.Text    = X103015.Text.Trim(); // Password
			string userCode = "";
			string userName = "";
			byte   loginErr = WebTools.CheckBackDoorLogins(X103014.Text, X103015.Text, ref userCode, ref userName);
			
			if ( loginErr == 0 && userCode.Length > 0 && userName.Length > 0 )
			{
				SetErrorDetail("",-777);
				SessionSave(userCode,userName,"N","20200304-1911");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}

			if ( loginErr == 0 )
			{
				SetErrorDetail("btnLogin_Click",10100,"Invalid user name and/or password","One or both of user name/password is blank",2,2,null,true);
				return;
			}

			using (MiscList mList = new MiscList())
			{
				sqlProc = "SP_ClientCRMValidateLoginD";
				sql     = "exec " + sqlProc + " @IPAddress = "   + Tools.DBString(WebTools.ClientIPAddress(Request))
				                            + ",@ClientCode = "  + Tools.DBString(X103014.Text,47)
				                            + ",@ContractPin = " + Tools.DBString(X103015.Text)
				                            + ",@ProductCode = " + Tools.DBString(sessionGeneral.ProductCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",10110,"Internal database error (" + sqlProc + ")",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",10120,"Invalid login and/or PIN",sqlProc + ", no data returned",1,1);
				else if ( mList.GetColumn("Status") != "S" )
				{
					string err = mList.GetColumn("ActionResultMessage");
					if ( err.Length < 1 )
						err = "Invalid login and/or PIN";
					SetErrorDetail("btnLogin_Click",10130,err,sqlProc + ", Status = '" + mList.GetColumn("Status") + "'",1,1);
				}
				else
				{
					userCode            = mList.GetColumn("ClientCode");
					string contractCode = mList.GetColumn("ContractCode");
					string access       = mList.GetColumn("Access");
					SessionSave(userCode,"",access,contractCode);
					WebTools.Redirect(Response,sessionGeneral.StartPage);
				}
			}
		}
	}
}