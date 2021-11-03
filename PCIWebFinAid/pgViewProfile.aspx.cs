using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProfile : BasePageCRM
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
				StartOver(11010,(int)Constants.ErrorType.InvalidMenu);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
			lblDate.Text       = Tools.DateToString(System.DateTime.Now,7,1); // yyyy-mm-dd hh:mm:ss
		}

		protected override void LoadPageData()
		{
			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_Get_CRMClientWelcome";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",11100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
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

				bool rowEven = true;
				sqlProc      = "sp_CRM_GetContractContactLog";
				sql          = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);

				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadPageData",11110,"Internal database error (" + sqlProc + ")",sql,102,2);
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
						if (rowEven)
							row.CssClass = "tRow";
						else
							row.CssClass = "tRowAlt";
						rowEven = ! rowEven;
						tblHistory.Rows.Add(row);
						mList.NextRow();
					}
			}
		}
	}
}