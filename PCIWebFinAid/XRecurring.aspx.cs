// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 12300-12399

namespace PCIWebFinAid
{
	public partial class XRecurring : BasePageAdmin
	{
		private int    timeOut;
		private int    maxRows;
		private string provider;

		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) == 0 )
				LoadData();
			else
				StartOver(10887,(int)Constants.ErrorType.InvalidMenu);
		}
		private void LoadData()
		{
			SetErrorDetail("",-888);
//			ascxXFooter.JSText = WebTools.JavaScriptSource("HidePopups()");
			lblErr2.Text       = "";
	
			foreach (int bureauCode in Enum.GetValues(typeof(Constants.PaymentProvider)))
				lstProvider.Items.Add(new ListItem(Enum.GetName(typeof(Constants.PaymentProvider),bureauCode),bureauCode.ToString().PadLeft(3,'0')));

			lstCCYear.Items.Clear();
			lstCCYear.Items.Add(new ListItem("(Select one)","0"));

			for ( int y = System.DateTime.Now.Year ; y < System.DateTime.Now.Year+15 ; y++ )
				lstCCYear.Items.Add(new ListItem(y.ToString(),y.ToString()));
			ProviderDetails();
		}

		private void ProviderDetails()
		{
			string   bureauCode  = lstProvider.SelectedValue.Trim();
			Provider provider    = new Provider();
			provider.BureauCode  = bureauCode;
			lblBureauCode.Text   = provider.BureauCode;
			lblBureauName.Text   = lstProvider.SelectedItem.Text;
			lblBureauStatus.Text = provider.BureauStatusName;
			rdoCard.Text         = "Single card payment";
			rdoCard.Enabled      = provider.ThreeDEnabled;
			btnPay.Visible       = provider.ThreeDEnabled;
			if ( ! provider.ThreeDEnabled )
				rdoCard.Checked   = false;

			if ( provider.BureauStatusCode == 2 ) // Disabled
			{
				provider             = null;
				btnProcess1.Text     = "Get Tokens (Disabled)";
				btnProcess2.Text     = "Do Payments (Disabled)";
				btnProcess3.Text     = "Delete Tokens (Disabled)";
				btnProcess1.Enabled  = false;
				btnProcess2.Enabled  = false;
				btnProcess3.Enabled  = false;
				lblBureauURL.Text    = "";
				lblMerchantKey.Text  = "";
				lblMerchantUser.Text = "";
				lblCards.Text        = "";
				lblPayments.Text     = "";
				return;
			}
			btnProcess1.Text    = "Get Tokens";
			btnProcess2.Text    = "Process Payments";
			btnProcess3.Text    = "Delete Tokens";
			btnProcess1.Enabled = true;
			btnProcess2.Enabled = true;
			btnProcess3.Enabled = true;
//			btnProcess1.CommandArgument = ((byte)Constants.TransactionType.GetToken).ToString();
//			btnProcess2.CommandArgument = ((byte)Constants.TransactionType.TokenPayment).ToString();
//			btnProcess3.CommandArgument = ((byte)Constants.TransactionType.DeleteToken).ToString();

			if ( bureauCode.Length > 0 )
				using (Payments payments = new Payments())
				{
					payments.Summary(provider);
					lblBureauURL.Text    = provider.BureauURL;
					lblMerchantKey.Text  = provider.MerchantKey;
					lblMerchantUser.Text = provider.MerchantUserID;
					lblCards.Text        = provider.CardsToBeTokenized.ToString()    + ( provider.CardsToBeTokenized    >= Constants.MaxRowsPayment ? "+" : "" );
					lblPayments.Text     = provider.PaymentsToBeProcessed.ToString() + ( provider.PaymentsToBeProcessed >= Constants.MaxRowsPayment ? "+" : "" );
					if ( provider.PaymentType == (byte)Constants.TransactionType.TokenPayment )
					{
						btnProcess1.Text    = "Get Tokens";
						btnProcess1.Enabled = true;
						btnProcess3.Text    = "Delete Tokens";
						btnProcess3.Enabled = true;
					}
					else if ( provider.PaymentType == (byte)Constants.TransactionType.CardPayment ) // Means no tokens, card payments only
					{
						btnProcess1.Text    = "N/A";
						btnProcess1.Enabled = false;
						btnProcess3.Text    = "N/A";
						btnProcess3.Enabled = false;
					}
				}
		}

		protected void lstProvider_Click(Object sender, EventArgs e)
		{
			ProviderDetails();
		}

		protected void btnPay_Click(Object sender, EventArgs e)
		{
			ProcessCards((byte)PCIBusiness.Constants.TransactionType.ManualPayment);
		}

		protected void btnProcess1_Click(Object sender, EventArgs e)
		{
			ProcessCards((byte)Constants.TransactionType.GetToken);
		}

		protected void btnProcess2_Click(Object sender, EventArgs e)
		{
			ProcessCards((byte)Constants.TransactionType.TokenPayment);
		}

		protected void btnProcess3_Click(Object sender, EventArgs e)
		{
			ProcessCards((byte)Constants.TransactionType.DeleteToken);
		}

		private void ProcessCards(byte transactionType)
		{
			if ( CheckData() > 0 || transactionType < 1 )
				SetErrorDetail("ProcessCards",12310,"Invalid/no payment provider selected or no data found");
			else if ( rdoWeb.Checked )
				ProcessWeb(transactionType);
			else if ( rdoAsynch.Checked )
				ProcessAsynch(transactionType);
			else if ( rdoCard.Checked )
				ProcessPayment();
			else
				SetErrorDetail("ProcessCards",12320,"Invalid options selected");
		}

		private void ProcessAsynch(byte transactionType)
		{
			if ( ! sessionGeneral.AdminUser )
			{
				SetErrorDetail("ProcessAsynch",12330,"You do not have sufficient rights to do this","User code " + sessionGeneral.UserCode + " is not an admin user");
				return;
			}
			string fName = Tools.SystemFolder("Bin") + "PCIUnattended.exe";
			if ( ! File.Exists(fName) )
			{
				SetErrorDetail("ProcessAsynch",12340,"PCIUnattended.EXE cannot be found","File missing : " + fName);
				return;
			}

			ProcessStartInfo app = new ProcessStartInfo();

			app.Arguments      =  "TransactionType=" + transactionType.ToString()
			                   + " Rows=" + maxRows.ToString()
			                   + " Provider=" + provider
			                   + " UserCode=" + sessionGeneral.UserCode;
			app.WindowStyle    = ProcessWindowStyle.Hidden;
		//	app.WindowStyle    = ProcessWindowStyle.Normal;
		//	app.FileName       = "PCIUnattended.exe";
			app.CreateNoWindow = false;
			app.FileName       = fName;

			try
			{
				Tools.LogInfo("XRecurring.ProcessAsynch/1",app.FileName + " " + app.Arguments,220);

//	(1) Run the external process asynchronously
				System.Diagnostics.Process.Start(app);
				Tools.LogInfo("XRecurring.ProcessAsynch/2","Launched",220);

// (2) Run the external process & wait for it to finish
//				using (Process proc = System.Diagnostics.Process.Start(app))
//				{
//					proc.WaitForExit();
//				// Retrieve the app's exit code
//					exitCode = proc.ExitCode;
//				}
//				Tools.LogInfo("XRecurring.ProcessAsynch/3","exitCode="+exitCode.ToString());
			}
			catch (Exception ex)
			{
				Tools.LogException("XRecurring.ProcessAsynch/9",app.FileName + " " + app.Arguments,ex);
			}
			app = null;
		}

		private void ProcessPayment()
		{
			int           ret  = 0;
			StringBuilder msg  = new StringBuilder();
			ascxXFooter.JSText = WebTools.JavaScriptSource("PaySingle(8)");

			try
			{
				Tools.LogInfo("XRecurring.ProcessPayment/1","Started, provider '" + provider + "'",10);

				using (Payment payment = new Payment(provider))
				{
					payment.CardNumber        = txtCCNumber.Text;
					payment.CardName          = txtFName.Text + " " + txtLName.Text;
					payment.CardCVV           = txtCCCVV.Text;
					payment.EMail             = txtEMail.Text;
					payment.CurrencyCode      = txtCurrency.Text;
					payment.MerchantReference = txtReference.Text;
					payment.CardExpiryMM      = lstCCMonth.SelectedValue;
					payment.CardExpiryYYYY    = lstCCYear.SelectedValue;
					payment.PaymentAmount     = Tools.StringToInt(txtAmount.Text);
					payment.TransactionType   = (byte)Constants.TransactionType.ManualPayment;
					payment.FirstName         = txtFName.Text;
					payment.LastName          = txtLName.Text;
//					int k = payment.CardName.IndexOf(" ");
//					if ( k > 0 )
//					{
//						payment.FirstName = payment.CardName.Substring(0,k).Trim();
//						payment.LastName  = payment.CardName.Substring(k).Trim();
//					}
//					else
//						payment.LastName  = payment.CardName;

					if ( payment.CardName.Length < 3 )
						msg.Append("Invalid first and/or last name<br />");
					if ( ! Tools.CheckEMail(payment.EMail,1) )
						msg.Append("Invalid email address<br />");
					if ( payment.CardNumber.Length < 10 )
						msg.Append("Invalid card number<br />");
					if ( WebTools.ListValue(lstCCMonth) < 1 || WebTools.ListValue(lstCCYear) < 1 )
						msg.Append("Invalid expiry date<br />");
					if ( payment.CardCVV.Length < 3 )
						msg.Append("Invalid CVV/CVC number<br />");
					if ( payment.CurrencyCode.Length < 1 )
						msg.Append("Invalid currency code<br />");
					if ( payment.PaymentAmount< 1 )
						msg.Append("Invalid payment amount<br />");
					if ( msg.ToString().Length > 0 )
					{
						SetErrorDetail("ProcessPayment",12350,msg.ToString(),"",23);
						return;
					}

					ret = payment.ProcessPayment();
					if ( ret == 0 && payment.WebForm.Length > 0 )
						try
						{
						//	Busy();
							ret = 987654;
						//	This always throws a "thread aborted" exception ... ignore it
							System.Web.HttpContext.Current.Response.Clear();
							System.Web.HttpContext.Current.Response.Write(payment.WebForm);
							System.Web.HttpContext.Current.Response.End();
						}
						catch
						{ }
					else
					{
						if ( payment.ReturnMessage.Length > 0 )
							SetErrorDetail("ProcessPayment",12360,payment.ReturnMessage,"",23);
						else
							SetErrorDetail("ProcessPayment",12370,"Transaction failed","",23);
					}
				}
				Tools.LogInfo("XRecurring.ProcessPayment/2","Finished",10);
			}
			catch (Exception ex)
			{
				if ( ret != 987654 )
					Tools.LogException("RTR.ProcessPayment/9","",ex);
			}
		}

		private void ProcessWeb(byte transactionType)
		{
			try
			{
				Tools.LogInfo("XRecurring.ProcessWeb/1","Started, provider '" + provider + "'",10);

				using (Payments payments = new Payments())
				{
					int k         = payments.ProcessCards(provider,transactionType,maxRows);
					lblError.Text = (payments.CountSucceeded+payments.CountFailed).ToString() + " token(s)/payment(s) processed : " + payments.CountSucceeded.ToString() + " succeeded, " + payments.CountFailed.ToString() + " failed<br />&nbsp;";
				}
				Tools.LogInfo("XRecurring.ProcessWeb/2","Finished",10);
			}
			catch (Exception ex)
			{
				Tools.LogException("XRecurring.ProcessWeb/9","",ex);
			}
		}

		private byte CheckData()
		{
			try
			{
				provider  = lstProvider.SelectedValue.Trim();
				string rw = txtRows.Text.Trim().ToUpper();
				if ( rw == "ALL" || rw.Length == 0 )
					maxRows = 0;
				else
					maxRows = Tools.StringToInt(rw);
			}
			catch
			{
				maxRows = -8;
			}
			if ( string.IsNullOrWhiteSpace(provider) )
				return 78; // Error
			if ( maxRows < 0 )
				return 79; // Error
			return 0;
		}

		public XRecurring() : base()
		{
			timeOut              = Server.ScriptTimeout;
			Server.ScriptTimeout = 1800; // 30 minutes
		}

		~XRecurring()
		{
			Server.ScriptTimeout = timeOut;
		}
	}
}
