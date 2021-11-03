using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgDeactivateSubscription : BasePageCRM
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
				LoadPageData();
			else
				StartOver(23010,(int)Constants.ErrorType.InvalidMenu);
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

			LoadLabelText(ascxXMenu);

//	Test
//			lblCurr.Text    = "R";
//			lblBalance.Text = "8913.76";
//			lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("123","Lost Interest"));
//			lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("443","Gave up"));
//			lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("123","Bugger off"));
//	Test

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_GetContractSettlementBalance";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",23100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{
					lblBalance.Text = mList.GetColumn("SettlementBalance");
					lblCurr.Text    = mList.GetColumn("CurrencySymbol");
				}

				sqlProc = "sp_CRM_GetContractCancellationReasonList";
				sql     = "exec " + sqlProc + " @LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
				                            + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
				                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType);

				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",23110,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						System.Web.UI.WebControls.ListItem lItem = new System.Web.UI.WebControls.ListItem();
						lItem.Value = mList.GetColumn("CancellationReasonCode");
						lItem.Text  = mList.GetColumn("CancellationReasonDescription");
						lstReason.Items.Add(lItem);
						mList.NextRow();
					}
			}

			lstReason.Focus();
		}

		protected void btnChange_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnChange_Click",23200,"Not yet implemented ...","",102,0);
		}

		protected void btnConfirm_Click(Object sender, EventArgs e)
		{
			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_LogClientCancellationRequestA";
				sql     = "exec " + sqlProc + " @ContractCode="           + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@CancellationReasonCode=" + Tools.DBString(WebTools.ListValue(lstReason,"0"))
				                            + ",@LanguageCode="           + Tools.DBString(sessionGeneral.LanguageCode)
				                            + ",@LanguageDialectCode="    + Tools.DBString(sessionGeneral.LanguageDialectCode)
				                            + ",@Access="                 + Tools.DBString(sessionGeneral.AccessType);

//				Tools.LogInfo("btnConfirm_Click",sql,203,this);

				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("btnConfirm_Click",23300,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( mList.EOF )
					SetErrorDetail("btnConfirm_Click",23310,"No data returned (" + sqlProc + ")",sql,102,1);
				else
					SetErrorDetail("btnConfirm_Click",23320,mList.GetColumn("ResultMessage"),"",102,0);
			}
		}
	}
}