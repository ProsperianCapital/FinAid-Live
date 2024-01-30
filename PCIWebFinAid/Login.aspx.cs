using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Login : BasePageCRMv1
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			SessionCheck(3);

			if ( ! Page.IsPostBack )
			{
			//	hdnVer.Value                       = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
				hdnVer.Value                       = WebTools.VersionDetails(2);
				lblVer.Text                        = hdnVer.Value;
				lblVer.Visible                     = ! Tools.SystemIsLive();
				sessionGeneral.ProductCode         = WebTools.RequestValueString(Request,"PC");
				sessionGeneral.LanguageCode        = WebTools.RequestValueString(Request,"LC");
				sessionGeneral.LanguageDialectCode = WebTools.RequestValueString(Request,"LDC");

//	Testing 1 (English)
				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10278";
				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "ENG";
				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0002";

//	Testing 2 (Thai)
//				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10024";
//				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "THA";
//				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0001";

				if ( LoadLabelText(null) != 0 )
					X103016.Enabled = false;
				SessionClearData();
				SessionSave();
				txtID.Focus();
			}
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			txtID.Text      = txtID.Text.Trim();
			txtPW.Text      = txtPW.Text.Trim();
			string userCode = "";
			string userName = "";
			byte   loginErr = WebTools.CheckBackDoorLogins(txtID.Text, txtPW.Text, ref userCode, ref userName);
			
			if ( loginErr == 0 && userCode.Length > 0 && userName.Length > 0 )
			{
				SessionSave(txtID.Text,"Prosperian Admin","A");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}

			if ( loginErr == 0 )
			{
				SetErrorDetail("btnLogin_Click",10010,"Invalid login and/or PIN","One or both of ID/PIN is blank");
				return;
			}

//			if ( txtID.Text.ToUpper() == "XADMIN" && txtPW.Text.ToUpper() == "X8Y3Z7" )
//			{
//				SessionSave(txtID.Text,"Prosperian Admin","A");
//				WebTools.Redirect(Response,sessionGeneral.StartPage);
//				return;
//			}
//
//			if ( ( txtID.Text.ToUpper() == "TEST" || txtID.Text.ToUpper() == "109337" ) && txtPW.Text.ToUpper() == "TEST" )
//			{
//				SessionSave("109337","Jack Ivanovich","N","20200304-1954");
//				WebTools.Redirect(Response,sessionGeneral.StartPage);
//				return;
//			}

			using (MiscList mList = new MiscList())
			{
				sql = "exec SP_ClientCRMValidateLoginD"
				    + " @IPAddress = "   + Tools.DBString(WebTools.ClientIPAddress(Request))
				    + ",@ClientCode = "  + Tools.DBString(txtID.Text,47)
				    + ",@ContractPin = " + Tools.DBString(txtPW.Text)
				    + ",@ProductCode = " + Tools.DBString(sessionGeneral.ProductCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",10020,"Internal database error (SP_ClientCRMValidateLoginD)",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",10030,"Invalid login and/or PIN","SP_ClientCRMValidateLoginD, no data returned",1,1);
				else if ( mList.GetColumn("Status") != "S" )
					SetErrorDetail("btnLogin_Click",10040,"Invalid login and/or PIN","SP_ClientCRMValidateLoginD, Status = '" + mList.GetColumn("Status") + "'",1,1);
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