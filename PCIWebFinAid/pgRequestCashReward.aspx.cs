using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgRequestCashReward : BasePageCRM
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
				StartOver(24010,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
			txtAmount.Text = "";
			txtAmount.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
		//	decimal amt = Tools.StringToDecimal(txtAmount.Text);
			int amt     = Tools.StringToInt(txtAmount.Text);

			if ( amt < 1 || txtBank.Text.Length       < 2
			             || txtBranchName.Text.Length < 2 
			             || txtBranchCode.Text.Length < 4
			             || txtAccName.Text.Length    < 2 
			             || txtAccNumber.Text.Length  < 5
			             || txtAddr1.Text.Length      < 3
			             || txtAddr2.Text.Length      < 3
			             || txtAddr5.Text.Length      < 4 )
				return;

			sqlProc = "sp_CRM_ApplyForEmergencyCash";
			sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
			                            + ",@BankName="            + Tools.DBString(txtBank.Text,47)
			                            + ",@AccountHolderName="   + Tools.DBString(txtAccName.Text,47)
			                            + ",@AccountNumber="       + Tools.DBString(txtAccNumber.Text,47)
			                            + ",@BranchName="          + Tools.DBString(txtBranchName.Text,47)
			                            + ",@SWIFTorIBAN="         + Tools.DBString(txtSwift.Text,47)
			                            + ",@BranchCode="          + Tools.DBString(txtBranchCode.Text,47)
			                            + ",@Amount='"             + amt.ToString() + "'"
			                            + ",@Address1="            + Tools.DBString(txtAddr1.Text,47)
			                            + ",@Address2="            + Tools.DBString(txtAddr2.Text,47)
			                            + ",@Address3="            + Tools.DBString(txtAddr3.Text,47)
			                            + ",@Address4="            + Tools.DBString(txtAddr4.Text,47)
			                            + ",@Address5="            + Tools.DBString(txtAddr5.Text,47)
			                            + ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
			                            + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
			                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType)
			                            + ",@ProductBenefitPurposeCode='000'"
			                            + ",@CB1=''"
			                            + ",@CB2=''"
			                            + ",@CB3=''"
			                            + ",@CB4=''"
			                            + ",@CB5=''";
			UpdatePageData("btnOK_Click");
		}
	}
}