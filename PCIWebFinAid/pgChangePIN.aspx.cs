using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgChangePIN : BasePageCRM
	{
		protected const int MIN_PIN_LENGTH = 4;
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
				StartOver(17010,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
			txtPIN0.Text = "";
			txtPIN1.Text = "";
			txtPIN2.Text = "";
			txtPIN0.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			int pin0 = Tools.StringToInt(txtPIN0.Text);
			int pin1 = Tools.StringToInt(txtPIN1.Text);
			int pin2 = Tools.StringToInt(txtPIN2.Text);

			if ( pin0 > 0 && pin1 > 0 && pin2 > 0 && pin1 == pin2 )
			{
				sqlProc = "sp_CRM_ChangeContractPINA";
				sql     = "exec " + sqlProc + " @ContractCode="       + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@ExistingPIN="        + pin0.ToString()
				                            + ",@NewPIN="             + pin1.ToString()
				                            + ",@NewPINConfirmation=" + pin2.ToString()
				                            + ",@Access="             + Tools.DBString(sessionGeneral.AccessType);
				UpdatePageData("btnOK_Click");
			}

//				using (MiscList mList = new MiscList())
//				{
//					sqlProc = "sp_CRM_ChangeContractPINA";
//					sql     = "exec " + sqlProc + " @ContractCode="       + Tools.DBString(sessionGeneral.ContractCode)
//					                            + ",@ExistingPIN="        + pin0.ToString()
//					                            + ",@NewPIN="             + pin1.ToString()
//					                            + ",@NewPINConfirmation=" + pin2.ToString()
//					                            + ",@Access="             + Tools.DBString(sessionGeneral.AccessType);
//					if ( mList.ExecQuery(sql,0) != 0 )
//						SetErrorDetail("btnOK_Click",17100,"Internal database error (" + sqlProc + ")",sql,102,1);
//					else if ( ! mList.EOF )
//					{
//						SetErrorDetail("btnOK_Click",17110,mList.GetColumn("ResultMessage"),"",102,0);
//						txtPIN0.Text = "";
//						txtPIN1.Text = "";
//						txtPIN2.Text = "";
//					}
//				}

		}
	}
}