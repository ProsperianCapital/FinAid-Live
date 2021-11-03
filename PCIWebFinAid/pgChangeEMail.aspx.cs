using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgChangeEMail : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;

			ClearData();

			if ( Page.IsPostBack )
				return;

			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) == 0 )
			{
				LoadLabelText(ascxXMenu);
				LoadPageData();
			}
			else
				StartOver(16012,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_GetContractContactInfo";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@Access="       + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",16100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{
					lblEMail.Text = mList.GetColumn("EMailAddress");
					lblPhone.Text = mList.GetColumn("MobileNumber");
				}
			}
			txtEMail.Text = "";
			txtPhone.Text = "";
			txtEMail.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string email = txtEMail.Text.Trim();
			string phone = txtPhone.Text.Trim();

			if ( Tools.CheckEMail(email,1) && Tools.CheckPhone(phone) )
			{
				sqlProc = "sp_CRM_ChangeContractContactInfoA";
				sql     = "exec " + sqlProc + " @ContractCode="    + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@Access="          + Tools.DBString(sessionGeneral.AccessType)
				                            + ",@NewEMailAddress=" + Tools.DBString(email,47)
				                            + ",@NewMobileNumber=" + Tools.DBString(phone,47);
				UpdatePageData("btnOK_Click");

//				using (MiscList mList = new MiscList())
//				{
//					sqlProc = "sp_CRM_ChangeContractContactInfoA";
//					sql     = "exec " + sqlProc + " @ContractCode="    + Tools.DBString(sessionGeneral.ContractCode)
//					                            + ",@Access="          + Tools.DBString(sessionGeneral.AccessType)
//					                            + ",@NewEMailAddress=" + Tools.DBString(email,47)
//					                            + ",@NewMobileNumber=" + Tools.DBString(phone,47);
//	
//					if ( mList.ExecQuery(sql,0) != 0 )
//						SetErrorDetail("btnOK_Click",16200,"Internal database error (" + sqlProc + ")",sql,102,1);
//					else if ( mList.EOF )
//						SetErrorDetail("btnOK_Click",16210,"No data returned (" + sqlProc + ")",sql,102,1);
//					else
//					{
//						SetErrorDetail("btnOK_Click",16220,mList.GetColumn("ResultMessage"),"",102,0);
//						Tools.LogInfo("btnOK_Click","ResultCode="+mList.GetColumn("ResultCode"),222);
//						if ( mList.GetColumnInt("ResultCode") == 0 )
//							LoadPageData();
//					}
//				}

			}
		}
	}
}