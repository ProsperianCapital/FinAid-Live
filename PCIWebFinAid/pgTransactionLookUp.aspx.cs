// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgTransactionLookUp : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) == 0 )
				SetErrorDetail("",-777);
			else
				StartOver(10777,(int)Constants.ErrorType.InvalidMenu);
		}

		private int ValidateData()
		{
			lblTransactions.Text = "";
			txtCard1.Text        = txtCard1.Text.Trim().Replace(" ","");
			txtCard3.Text        = txtCard3.Text.Trim().Replace(" ","");
			string cardNo        = txtCard1.Text + txtCard3.Text;
			string err           = "";
			if ( cardNo.Length  != 10 )
				err = "Invalid card number (6 and 4 digits needed)<br />";
			else
				for ( int k = 0 ; k < cardNo.Length ; k++ )
					if ( ! "0123456789".Contains(cardNo.Substring(k,1)) )
					{
						err = "Invalid card number (only digits allowed)<br />";
						break;
					}
			DateTime d1 = Tools.StringToDate(txtDate1.Text,7);
			DateTime d2 = Tools.StringToDate(txtDate2.Text,7);
			if ( d1 <= Constants.DateNull )
				err = err + "Invalid from date (it must be in yyyy-mm-dd format)<br />";
			if ( d2 <= Constants.DateNull )
				err = err + "Invalid to date (it must be in yyyy-mm-dd format)<br />";
			if ( d1 > d2 && d2 > Constants.DateNull )
				err = err + "From date cannot be after to date<br />";
			if ( err.Length > 0 )
				SetErrorDetail("ValidateData",30010,err,err);
			return err.Length;
		}


		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			if ( ValidateData() > 0 )
				return;

			sql = "exec sp_Fin_GetPayments"
			    + " @MaskedCardNumber = " + Tools.DBString(txtCard1.Text+"******"+txtCard3.Text)
			    + ",@BeginDate = "        + Tools.DateToSQL(Tools.StringToDate(txtDate1.Text,1),11)  // Time = 00:00:00
			    + ",@EndDate = "          + Tools.DateToSQL(Tools.StringToDate(txtDate2.Text,1),12); // Time = 23:59:59
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("btnSearch_Click",30060,"Internal database error (sp_Fin_GetPayments)",sql,2,2);
				else if ( miscList.EOF )
					SetErrorDetail("btnSearch_Click",30070,"No transactions found. Refine your criteria and try again",sql,2,0);
				else
				{
					StringBuilder data = new StringBuilder();
					bool          odd  = false;
					data.Append("<table border='1'><tr class='tRowHead'><td>Card Number</td><td>Client Code</td><td>Contract Code</td><td>Date/Time</td><td>Transaction Number</td><td>Currency</td><td style='text-align:right'>Amount</td></tr>");
					while ( ! miscList.EOF )
					{
						odd = ! odd;
						data.Append("<tr class='");
						if (odd)
							data.Append("tRow'>");
						else
							data.Append("tRowAlt'>");
						data.Append("<td>" + miscList.GetColumn("MaskedCardNumber") + "</td>");
						data.Append("<td>" + miscList.GetColumn("ClientCode") + "</td>");
						data.Append("<td>" + miscList.GetColumn("ContractCode") + "</td>");
						data.Append("<td>" + miscList.GetColumn("ProcessDateTime") + "</td>");
						data.Append("<td>" + miscList.GetColumn("TransactionNumber") + "</td>");
						data.Append("<td>" + miscList.GetColumn("TransactionCurrencyCode") + "</td>");
						data.Append("<td style='text-align:right'>" + miscList.GetColumn("TransactionAmount") + "</td>");
						data.Append("</tr>");
						miscList.NextRow();
					}
					data.Append("</table>");
					lblTransactions.Text = data.ToString();
					data                 = null;
				}
		}
	}
}