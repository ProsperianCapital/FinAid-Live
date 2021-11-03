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
			             || txtAccNumber.Text.Length  < 5 )
				return;

			sqlProc = "sp_CRM_ApplyForEmergencyCash";
			sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
			                            + ",@BankName="            + Tools.DBString(txtBank.Text)
			                            + ",@AccountHolderName="   + Tools.DBString(txtAccName.Text)
			                            + ",@AccountNumber="       + Tools.DBString(txtAccNumber.Text)
			                            + ",@BranchName="          + Tools.DBString(txtBranchName.Text)
			                            + ",@SWIFTorIBAN="         + Tools.DBString(txtSwift.Text)
			                            + ",@BranchCode="          + Tools.DBString(txtBranchCode.Text)
			                            + ",@Amount='"             + amt.ToString() + "'"
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

//			using (MiscList mList = new MiscList())
//			{
//				sqlProc = "sp_CRM_ApplyForEmergencyCash";
//				sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
//				                            + ",@BankName="            + Tools.DBString(txtBank.Text)
//				                            + ",@AccountHolderName="   + Tools.DBString(txtAccName.Text)
//				                            + ",@AccountNumber="       + Tools.DBString(txtAccNumber.Text)
//				                            + ",@BranchName="          + Tools.DBString(txtBranchName.Text)
//				                            + ",@SWIFTorIBAN="         + Tools.DBString(txtSwift.Text)
//				                            + ",@BranchCode="          + Tools.DBString(txtBranchCode.Text)
//				                            + ",@Amount='"             + amt.ToString() + "'"
//				                            + ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
//				                            + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
//				                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType)
//				                            + ",@ProductBenefitPurposeCode='000'"
//				                            + ",@CB1=''"
//				                            + ",@CB2=''"
//				                            + ",@CB3=''"
//				                            + ",@CB4=''"
//				                            + ",@CB5=''";
//				if ( mList.ExecQuery(sql,0) != 0 )
//					SetErrorDetail("btnOK_Click",24100,"Internal database error (" + sqlProc + ")",sql,102,1);
//				else if ( ! mList.EOF )
//					SetErrorDetail("btnOK_Click",24110,mList.GetColumn("ActionResultMessage"),"",102,0);
//			}
		}
	}
}