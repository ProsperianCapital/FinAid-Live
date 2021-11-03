using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductActivityLog : BasePageCRM
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
				StartOver(14010,(int)Constants.ErrorType.InvalidMenu);
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
				TableRow  row;
				TableCell col;
				bool      rowEven = true;
				sqlProc           = "sp_CRM_GetContractContactLog";
				sql               = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);

				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadPageData",14100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						row      = new TableRow();
						col      = new TableCell();
						col.Text = mList.GetColumn("ContactDate");
						row.Cells.Add(col);
						col      = new TableCell();
						col.Text = mList.GetColumn("ContactDescription");
						row.Cells.Add(col);
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