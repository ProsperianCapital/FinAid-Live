using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewMiniStatement : BasePageCRM
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
				StartOver(13010,(int)Constants.ErrorType.InvalidMenu);
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
				bool   rowEven = true;
				string balance = "";
				sqlProc        = "sp_Get_CRMClientMiniStatementB";
				sql            = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadPageData",13100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row   = new TableRow();
//						TableCell col2  = new TableCell();
//						TableCell col3  = new TableCell();
//						TableCell col4  = new TableCell();
//						col1.Text       = Tools.DateToString(mList.GetColumnDate("Date"),7,1); // yyyy-mm-dd hh:mm:ss
						TableCell col   = new TableCell();
						col.Text        = mList.GetColumn("Date");
						row.Cells.Add(col);
						col             = new TableCell();
						col.Text        = mList.GetColumn("Description");
						row.Cells.Add(col);
						col             = new TableCell();
						col.HorizontalAlign = HorizontalAlign.Right;
						col.Text        = mList.GetColumn("Amount");
						row.Cells.Add(col);
						col             = new TableCell();
						col.HorizontalAlign = HorizontalAlign.Right;
						col.Text        = mList.GetColumn("Balance");
						row.Cells.Add(col);
						balance         = col.Text;
						if (rowEven)
							row.CssClass = "tRow";
						else
							row.CssClass = "tRowAlt";
						rowEven = ! rowEven;
						tblStatement.Rows.Add(row);
						mList.NextRow();
					}
				lblBalance.Text = balance;
			}
		}
	}
}