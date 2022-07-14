using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LWelcome : BasePageCRMv1
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			maxTab                = 0;
//			ascxMenu.CurrentPage  = "Welcome";

			if ( SessionCheck()  != 0 )
				return;
//			if ( SecurityCheck() != 0 )
//				return;
			if ( PageCheck()     != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( LoadLabelText(ascxMenu) != 0 )
				return;

			lblDate.Text = Tools.DateToString(System.DateTime.Now,7,1); // yyyy-mm-dd hh:mm:ss

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Get_CRMClientWelcome @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("PageLoad",11040,"Internal database error (sp_Get_CRMClientWelcome failed)",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("PageLoad",11050,"Internal database error (sp_Get_CRMClientWelcome no data returned)",sql,1,1);
				else
				{
					lblAddress.Text   = mList.GetColumn("Address",1,6);
					lblEMail.Text     = mList.GetColumn("EmailAddress");
					lblCellNo.Text    = mList.GetColumn("TelephoneNumber");
					lblOption.Text    = mList.GetColumn("ProductOptionDescription");
					lblFee.Text       = mList.GetColumn("MonthlyFee");
					lblCredit.Text    = mList.GetColumn("CreditLimit");
					lblDueDate.Text   = mList.GetColumn("PaymentCycleDescription");
					lblUserName.Text  = mList.GetColumn("UserName");
					lblLastLogon.Text = mList.GetColumn("LastLogon");
				}

				sql = "exec sp_CRM_GetContractActivityLog @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				                                      + ",@Access="       + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("PageLoad",11060,"Internal database error (sp_CRM_GetContractActivityLog failed)",sql,1,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row  = new TableRow();
						TableCell col1 = new TableCell();
						TableCell col2 = new TableCell();
						col1.Text      = mList.GetColumn("ContactDate");
						col2.Text      = mList.GetColumn("ContactDescription");
						row.Cells.Add(col1);
						row.Cells.Add(col2);
						tblHistory.Rows.Add(row);
						mList.NextRow();
					}
			}
		}
	}
}