using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewCurrentBalances : BasePageCRM
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
				StartOver(12011,(int)Constants.ErrorType.InvalidMenu);
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
				sqlProc = "sp_Get_CRMClientBalancesA";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",12100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{
					lblRegFee.Text      = mList.GetColumn("txtRegistrationFeeDue");
					lblGrantLimit.Text  = mList.GetColumn("txtEmergencyCashLimit");
					lblGrantAvail.Text  = mList.GetColumn("txtEmergencyCashAvailable");
					lblGrantStatus.Text = mList.GetColumn("txtEmergencyCashBenefitStatus");
					lblMonthlyFee.Text  = mList.GetColumn("txtMonthlyFee");
					lblFeeDate.Text     = mList.GetColumn("txtLastFeePaymentDate");
				}
			}
		}
	}
}