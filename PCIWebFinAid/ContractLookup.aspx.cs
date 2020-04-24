using System;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ContractLookup : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			SetErrorDetail(-88,0,"","");
		}

		private int ValidateData()
		{
			txtContractCode.Text = txtContractCode.Text.Trim();
			string err           = "";
			if ( txtContractCode.Text.Length < 2 )
				err = "Invalid contract code<br />";
			SetErrorDetail(err.Length,100,err,err);
			return err.Length;
		}


		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			if ( ValidateData() > 0 )
				return;

			string sql = "exec sp_Fin_GetPayments"
			           + " @ContractCode = " + Tools.DBString(txtContractCode.Text);
	
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