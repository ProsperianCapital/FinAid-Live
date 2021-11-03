using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductDashboard : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
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
				StartOver(12012,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_WP_Get_DashboardInfo";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",12100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( mList.EOF )
					SetErrorDetail("LoadPageData",12110,"Internal database error (" + sqlProc + ", no data returned)",sql,102,1);
				else
				{
					lblName.Text         = mList.GetColumn("ClientName");
					lblStatus.Text       = mList.GetColumn("ContractStatusDescription");
					lblContractCode.Text = mList.GetColumn("ContractCode");
					lblClientCode.Text   = mList.GetColumn("ClientCode");
				}
			}
		}
	}
}