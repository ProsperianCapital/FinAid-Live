using System;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class TransLookup : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			SetErrorDetail(-88,0,"","");
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
			DateTime d1 = Tools.StringToDate(txtDate1.Text,1);
			DateTime d2 = Tools.StringToDate(txtDate2.Text,1);
			if ( d1 <= Constants.C_NULLDATE() )
				err = err + "Invalid from date (it must be in dd/mm/yyyy format)<br />";
			if ( d2 <= Constants.C_NULLDATE() )
				err = err + "Invalid to date (it must be in dd/mm/yyyy format)<br />";
			if ( d1 > d2 && d2 > Constants.C_NULLDATE() )
				err = err + "From date cannot be after to date<br />";
			SetErrorDetail(err.Length,100,err,err);
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
					SetErrorDetail(30060,30060,"Internal database error (sp_Fin_GetPayments)",sql,2,2);
				else if ( miscList.EOF )
					SetErrorDetail(30061,30061,"No transactions found. Refine your criteria and try again","",2,0);
				else
				{
					StringBuilder data = new StringBuilder();
					bool          odd  = false;
					data.Append("<table><tr class='Header5'><td>Card Number</td><td>Client Code</td><td>Contract Code</td><td>Date/Time</td><td>Transaction Number</td><td>Currency</td><td style='text-align:right'>Amount</td></tr>");
					while ( ! miscList.EOF )
					{
						odd = ! odd;
						data.Append("<tr onmouseover='JavaScript:this.style.backgroundColor=\"aqua\"' onmouseout='JavaScript:this.style.backgroundColor=");
						if (odd)
							data.Append("\"\"'>");
						else
							data.Append("\"#E0D0C0\"' style='background-color:#E0D0C0'>");
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

//			if ( lblError.Text.Length == 0 && errNo > 0 )
//				SetErrorDetail(99,30330,"Internal error ; please try again later","errNo=" + errNo.ToString());
		}

		private void SetErrorDetail(int errCode,int logNo,string errBrief,string errDetail,byte briefMode=2,byte detailMode=1)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblTransactions.Text = "";
				lblError.Text        = "";
				lblErrorDtl.Text     = "";
				lblError.Visible     = false;
				btnError.Visible     = false;
				return;
			}

			Tools.LogInfo("Register.SetErrorDetail","(errCode="+errCode.ToString()+", logNo="+logNo.ToString()+") "+errDetail,244);

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else
				lblError.Text = errBrief;
			lblError.Visible = ( lblError.Text.Length > 0 );

			if ( detailMode > 0 )
			{
				errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
				if ( detailMode == 2 ) // Append
					errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
				lblErrorDtl.Text = errDetail;
				if ( ! lblErrorDtl.Text.StartsWith("<div") )
					lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;
			}
			btnError.Visible = ( lblErrorDtl.Text.Length > 0 ) && lblError.Visible && ! Tools.SystemIsLive();
		}
	}
}