using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewRewardTransactions : BasePageCRM
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
				StartOver(19010,(int)Constants.ErrorType.InvalidMenu);
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
//			for ( int j = 0; j < 3; j++ )
//			{
//				TableRow  rowX = new TableRow();
//				TableCell colX;
//				for ( int k = 0 ; k < 6 ; k++ )
//				{
//					colX   = new TableCell();
//					if ( k == 0 ) colX.Text = "2020/11/22 17:01:43";
//					if ( k == 1 ) colX.Text = "Blah";
//					if ( k == 2 ) colX.Text = "5 182 307.64";
//					if ( k == 3 ) colX.Text = "This is a very long comment that I hope will be wrapped. If not, it will become a problem. Wrap, wrap, WRAP, I Say!";
//					if ( k == 4 ) colX.Text = "Update";
//					if ( k == 5 ) colX.Text = "4901******5512";
//					if ( k == 0 || k == 2 )
//						colX.Wrap = false;
//					if ( k == 2 ) // Amount
//						colX.HorizontalAlign = HorizontalAlign.Right;
//					rowX.Cells.Add(colX);
//				}
//				if ( j == 0 || j == 2 || j == 4 || j == 6 || j == 8 )
//					rowX.CssClass = "tRow";
//				else
//					rowX.CssClass = "tRowAlt";
//				tblData.Rows.Add(rowX);
//			}
//			return;
//	Test

			using (MiscList mList = new MiscList())
			{
				bool rowEven = true;
				sqlProc      = "sp_CRM_GetContractEmergencyCash";
				sql          = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
				                                 + ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
				                                 + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
				                                 + ",@Access="              + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadPageData",19100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row = new TableRow();
						TableCell col;
						for ( int k = 0 ; k < 6 ; k++ )
						{
							col      = new TableCell();
							col.Text = mList.GetColumn(k);
							if ( k == 0 || k == 2 )
								col.Wrap = false;
							if ( k == 2 ) // Amount
								col.HorizontalAlign = HorizontalAlign.Right;
							row.Cells.Add(col);
						}
						if (rowEven)
							row.CssClass = "tRow";
						else
							row.CssClass = "tRowAlt";
						rowEven = ! rowEven;
						tblData.Rows.Add(row);
						mList.NextRow();
					}
			}
		}
	}
}