using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgVerifyMobile : BasePageCRM
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
				LoadPageData();
			else
				StartOver(21012,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
//		Called once in the beginning
//
//			LoadLabelText(ascxXMenu);
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			int x = 0;

			if ( x == 871 )
				using (MiscList mList = new MiscList())
				{
					sqlProc = "sp_CRM_Blah";
					sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
					                            + ",@Access="       + Tools.DBString(sessionGeneral.AccessType);
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("btnOK_Click",21200,"Internal database error (" + sqlProc + ")",sql,102,1);
					else if ( ! mList.EOF )
						SetErrorDetail("btnOK_Click",21210,mList.GetColumn("ResultMessage"),"",102,0);
			}
		}
	}
}