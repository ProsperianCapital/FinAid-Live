using System;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class TransLookup : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck (19) != 0 ) // Admin only
				return;
//			if ( SecurityCheck(19) != 0 ) // Admin only
//				return;

			lblJS.Text = WebTools.JavaScriptSource("ActionMenu(0,0)");

			SetErrorDetail("",-888);

			if ( Page.IsPostBack )
				return;

			imgClose.ImageUrl = Tools.ImageFolder() + "Close1.png";
			imgClose.ToolTip  = "Close";
		}

		private int ValidateData()
		{
			txtCard1.Text = txtCard1.Text.Trim().Replace(" ","");
			txtCard3.Text = txtCard3.Text.Trim().Replace(" ","");
			string cardNo = txtCard1.Text + txtCard3.Text;
			string err    = "";
			if ( cardNo.Length != 10 )
				err = "Invalid card number (6 and 4 digits needed)<br />";
			else
				for ( int k = 0 ; k < cardNo.Length ; k++ )
					if ( ! "0123456789".Contains(cardNo.Substring(k,1)) )
					{
						err = "Invalid card number (only digits allowed)<br />";
						break;
					}
			DateTime d1 = Tools.StringToDate(txtDate1.Text,1); // dd/mm/yyyy
			DateTime d2 = Tools.StringToDate(txtDate2.Text,1); // dd/mm/yyyy
			if ( d1 <= Constants.DateNull )
				d1 = Tools.StringToDate(txtDate1.Text,2);       // yyyy/mm/dd
			if ( d2 <= Constants.DateNull )
				d2 = Tools.StringToDate(txtDate2.Text,2);       // yyyy/mm/dd
			if ( d1 <= Constants.DateNull )
				err = err + "Invalid from date (it must be in dd/mm/yyyy format)<br />";
			if ( d2 <= Constants.DateNull )
				err = err + "Invalid to date (it must be in dd/mm/yyyy format)<br />";
			if ( d1 > d2 && d2 > Constants.DateNull )
				err = err + "From date cannot be after to date<br />";
			SetErrorDetail("ValidateData",err.Length,err,err);
			return err.Length;
		}

		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			if ( ValidateData() > 0 )
				return;

			string sql = "exec sp_Fin_GetPayments"
			           + " @MaskedCardNumber = " + Tools.DBString(txtCard1.Text+"******"+txtCard3.Text)
			           + ",@BeginDate = "        + Tools.DateToSQL(Tools.StringToDate(txtDate1.Text,1),11)  // Time = 00:00:00
			           + ",@EndDate = "          + Tools.DateToSQL(Tools.StringToDate(txtDate2.Text,1),12); // Time = 23:59:59
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("btnSearch_Click",30060,"Internal database error (sp_Fin_GetPayments)",sql,2,2);
				else if ( miscList.EOF )
					SetErrorDetail("btnSearch_Click",30061,"No transactions found. Refine your criteria and try again","",2,0);
				else
				{
					StringBuilder data = new StringBuilder();
					bool          odd  = false;
					int           k    = 0;

					data.Append("<table><tr class='Header5'><td>Card Number</td><td>Client Code</td><td>Contract Code</td><td>Date/Time</td><td>Transaction Number</td><td>Currency</td><td style='text-align:right'>Amount</td><td>&nbsp;&nbsp;Action...</td></tr>");
					while ( ! miscList.EOF )
					{
						k++;
						odd = ! odd;
						data.Append("<tr onmouseover='JavaScript:this.style.backgroundColor=\"aqua\"' onmouseout='JavaScript:this.style.backgroundColor=");
						if (odd)
							data.Append("\"\"'>");
						else
							data.Append("\"#E0D0C0\"' style='background-color:#E0D0C0'>");
						data.Append("<td>" + miscList.GetColumn("MaskedCardNumber") + "</td>");
						data.Append("<td id='cl"+k.ToString()+"'>" + miscList.GetColumn("ClientCode") + "</td>");
						data.Append("<td id='cc"+k.ToString()+"'>" + miscList.GetColumn("ContractCode") + "</td>");
						data.Append("<td>" + miscList.GetColumn("ProcessDateTime") + "</td>");
						data.Append("<td id='tr"+k.ToString()+"'>" + miscList.GetColumn("TransactionNumber") + "</td>");
						data.Append("<td>" + miscList.GetColumn("TransactionCurrencyCode") + "</td>");
						data.Append("<td style='text-align:right'>" + miscList.GetColumn("TransactionAmount") + "</td>");
						data.Append("<td id='td"+k.ToString()+"'>&nbsp;&nbsp;&nbsp;&nbsp;<a href='JavaScript:ActionMenu(1,"+k.ToString()+")'>...</a></td>");
					//	data.Append("<td><input type='button' value='...' onclick='JavaScript:alert(\"Hello\")' /></td>");
						data.Append("</tr>");
						miscList.NextRow();
					}
					data.Append("</table>");
					lblTransactions.Text = data.ToString();
					data                 = null;
				}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				int    actionId = Tools.StringToInt(hdnActionId.Value);
				string xData    = hdnData.Value.Trim();

				if ( actionId < 1 || actionId > 3 || xData.Length < 1 )
				{
					lblFinish.Text = "Update failed (internal corruption)";
					return;
				}

				int    ret = 10;
				string sql = "";

				if ( actionId == 1 )
					sql = "sp_FILL_ChargebackAlert @InvoiceNumber = " + Tools.DBString(xData)
					    +                        ",@AlertDate = "     + Tools.DateToSQL(System.DateTime.Now,1)
					    +                        ",@AlertProviderCode = '001'"
					    +                        ",@AlertStatusCode = '01'";

				else if ( actionId == 2 )
					sql = "sp_FILL_Chargeback @InvoiceNumber = "  + Tools.DBString(xData)
					    +                   ",@ChargebackDate	= " + Tools.DateToSQL(System.DateTime.Now,1);

				else if ( actionId == 3 )
					sql = "sp_CRM_CancelContract @ContractCode = " + Tools.DBString(xData);

				using ( MiscList miscList = new MiscList() )
					ret = miscList.ExecQuery("exec "+sql,0,"",false,true);

				if ( ret == 0 )
				{
					lblFinish.Text = "Update successful";
					btnSearch_Click(null,null);
				}
				else
					lblFinish.Text = "Update failed (SQL error)";
			}
			catch (Exception ex)
			{
				lblFinish.Text = "Update failed (application error)";
			}
			lblJS.Text = WebTools.JavaScriptSource("ActionMenu(0,0);ShowElt('divFinish',true)");
		}
	}
}