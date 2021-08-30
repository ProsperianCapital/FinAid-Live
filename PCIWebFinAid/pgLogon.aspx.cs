// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using PCIBusiness;

// Error codes 11000-11099

namespace PCIWebFinAid
{
	public partial class pgLogon : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = Tools.SystemCode(Constants.ApplicationCode.BackOffice); // 001
			ShowSecure(false);
			SetErrorDetail("",-888);

			if ( Page.IsPostBack )
				SessionCheck(5);
			else
			{
				SessionCheck(2);
				ascxXHeader.ShowUser(null,ApplicationCode);
				txtID.Focus();
//				if ( WebTools.RequestValueInt(Request,"ErrNo") == 0 )
//					WebTools.ClientReferringURL(Request,14);
			}
		}

		protected void btnCancel_Click(Object sender, EventArgs e)
		{
			ShowSecure(false);
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
//			pnlSecurity.Visible = true;
//			pnlSecure.Visible   = true;
			ShowSecure(true);
			SetErrorDetail("btnOK_Click",11010,"Invalid security code","The security code must be exactly 6 digits",2,2,null,true);

//			string xSecure = txtSecurity.Text.Trim();
			string xSecure = (txtS1.Text+txtS2.Text+txtS3.Text+txtS4.Text+txtS5.Text+txtS6.Text).Trim().Replace(" ","");

			if ( xSecure.Length != 6 )
				return;

//	Testing
			if ( xSecure == ((int)Constants.SystemPassword.Login).ToString() )
			{
				SessionSave(null,null,"A");
				WebTools.Redirect(Response,sessionGeneral.StartPage);
				return;
			}
//	Testing

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Verify_BackOfficeSecurityCode"
				    + " @UserCode = "     + Tools.DBString(sessionGeneral.UserCode)
				    + ",@SecurityCode = " + Tools.DBString(xSecure);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnOK_Click",11020,"Internal database error (sp_Verify_BackOfficeSecurityCode)",sql,1,1,null,true);
				else if ( mList.EOF )
					SetErrorDetail("btnOK_Click",11030,"Invalid security code",sql + " (no data returned)",1,1,null,true);
				else
				{
					string status  = mList.GetColumn("Status").ToUpper();
					string message = mList.GetColumn("Message");
					if ( status != "S" )
						SetErrorDetail("btnOK_Click",11040,message,sql + " (Status = '" + status + "')",1,1,null,true);
					else
					{
						SessionSave(null,null,"A");
						WebTools.Redirect(Response,sessionGeneral.StartPage);
					}
				}
			}
		}

		protected void btnLogin_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnLogin_Click",11050,"Invalid user name and/or password","One or both of user name/password is blank",2,2,null,true);
			txtID.Text = txtID.Text.Trim();
			txtPW.Text = txtPW.Text.Trim();

			if ( txtID.Text.Length < 1 || txtPW.Text.Length < 1 )
				return;

//	Testing
//			if ( txtID.Text.ToUpper() == "XADMIN" && txtPW.Text.ToUpper() == "X8Y3Z7" )
			if ( ! Tools.SystemIsLive() && txtID.Text.ToUpper() == "PK" && txtPW.Text.ToUpper() == "PK" )
			{
				SetErrorDetail("",-777);
				SessionSave("013","Paul Kilfoil","A");
				ShowSecure(true,true);
				return;
			}
//	Testing

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Check_BackOfficeUser"
				    + " @UserName = " + Tools.DBString(txtID.Text)
				    + ",@Password = " + Tools.DBString(txtPW.Text);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnLogin_Click",11060,"Internal database error (sp_Check_BackOfficeUser)",sql,1,1,null,true);
				else if ( mList.EOF )
					SetErrorDetail("btnLogin_Click",11065,"Invalid user name and/or password",sql + " (no data returned)",1,1,null,true);
				else
				{
					string userCode = mList.GetColumn("UserCode");
					string userName = mList.GetColumn("UserDisplayName");
					string status   = mList.GetColumn("Status").ToUpper();
					string message  = mList.GetColumn("Message");
					if ( status != "S" )
						SetErrorDetail("btnLogin_Click",11070,message,sql + " (Status = '" + status + "')",1,1,null,true);
					else if ( userCode.Length < 1 || userName.Length < 2 )
						SetErrorDetail("btnLogin_Click",11075,"User details corrupted",sql + " (UserCode/UserDisplayName empty/invalid)",1,1,null,true);
					else
					{
						SetErrorDetail("",-777);
						SessionSave(userCode,userName,"X");
						ShowSecure(true,true);
					}
				}
			}
		}

		private void ShowSecure(bool onOff,bool clearSecurity=false)
		{
			pnlSecure.Visible =   onOff;
			btnLogin.Visible  = ! onOff;
			txtID.Enabled     = ! onOff;
			txtPW.Enabled     = ! onOff;
			if ( onOff )
				txtS1.Focus();
			else
				txtID.Focus();
			if ( clearSecurity )
			{
				txtS1.Text = "";
				txtS2.Text = "";
				txtS3.Text = "";
				txtS4.Text = "";
				txtS5.Text = "";
				txtS6.Text = "";
			}
		}
	}
}