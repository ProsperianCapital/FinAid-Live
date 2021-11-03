// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Text;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 80000-80099

namespace PCIWebFinAid
{
	public partial class pgAccountingCaptureCashbook : BasePageAdmin
	{
		string cashBook;
		int    errNo;
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
			{
			//	ascxXFooter.JSText = WebTools.JavaScriptSource("LoadCashBooks(GetEltValue('hdnSCashBook'),'S')");
				LoadDataAJAX("S");
				return;
			}
			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) == 0 )
			{
				LoadDataInitial();
				if ( WebTools.RequestValueInt(Request,"Mode") == 213 )
					ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(2)");
			}
			else
				StartOver(10888,(int)Constants.ErrorType.InvalidMenu);
		}

		private void LoadDataAJAX(string dType)
		{
//		Called after every postback to load data that was loaded via AJAX
//		Each AJAX list is based on the value of another list. So
//		(1)	lstXCashBook uses the value from lstXCompany
//				hdnXCashBook is the previously selected cash book
//		(2)	lstXGLCode uses the value from lstXTType
//				hdnXGLCode is the previously selected GL code

			errNo = 0;

			try
			{
				DropDownList lstX     = (DropDownList)FindControl("lst"+dType+"Company");
				DropDownList lstY     = (DropDownList)FindControl("lst"+dType+"CashBook");
				HiddenField  hdnX     = (HiddenField) FindControl("hdn"+dType+"CashBook");
				string       codeMain = WebTools.ListValue(lstX,"");
				string       codeSel  = Tools.NullToString(hdnX.Value);
				if ( codeMain.Length > 0 && codeMain != "0" )
				{
					sql   = "exec sp_Audit_Get_CompanyCashbook @CompanyCode=" + Tools.DBString(codeMain);
					errNo = WebTools.ListBind(lstY,sql,null,"CashBookCode","CashBookDescription","(All/any cashbook)",codeSel);
					SetErrorDetail("LoadData",errNo,"Unable to load cash book list",sql);
				}

				lstX     = (DropDownList)FindControl("lst"+dType+"TType");
				lstY     = (DropDownList)FindControl("lst"+dType+"GLCode");
				hdnX     = (HiddenField) FindControl("hdn"+dType+"GLCode");
				codeMain = WebTools.ListValue(lstX,"",2);
				codeSel  = Tools.NullToString(hdnX.Value);
				if ( codeMain.Length == 0 || codeMain == "0" )
					codeMain = "00000";
				sql   = "exec sp_Audit_Get_GLAccount @GLAccount=" + Tools.DBString(codeMain);
				errNo = WebTools.ListBind(lstY,sql,null,"GLAccount","GLAccountDescription","",codeSel);
				SetErrorDetail("LoadData",errNo,"Unable to load GL account list",sql);
			}
			catch (Exception ex)
			{
				SetErrorDetail("LoadData",80080,"Internal error",sql,2,2,ex);
			}
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			SetErrorDetail("",-888);
			ascxXFooter.JSText = WebTools.JavaScriptSource("HidePopups()");
			errNo              = 0;

			try
			{
				sql   = "exec sp_Audit_Get_Company";
				errNo = WebTools.ListBind(lstSCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				errNo = WebTools.ListBind(lstECompany,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);
				errNo = WebTools.ListBind(lstSOBOCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				errNo = WebTools.ListBind(lstEOBOCompany,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);

//				sql   = "exec sp_Audit_Get_CompanyCashbook";
//				This is done via an AJAX call

				sql   = "exec sp_Audit_Get_RPA";
				errNo = WebTools.ListBind(lstSReceipt,sql,null,"RPCode","Description","(All/any receipt)","");
				errNo = WebTools.ListBind(lstEReceipt,sql,null,"RPCode","Description","","");
				SetErrorDetail("LoadData",errNo,"Unable to load receipt/payment list",sql);

				sql   = "exec sp_Audit_Get_TransactionType";
				errNo = WebTools.ListBindMultiKey(lstSTType,sql,new string[]{"TransactionTypeCode","GLAccount"},"TransactionTypeDescription","(All/any type)","");
				errNo = WebTools.ListBindMultiKey(lstETType,sql,new string[]{"TransactionTypeCode","GLAccount"},"TransactionTypeDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load transaction type list",sql);

//				sql   = "exec sp_Audit_Get_GLAccount";
//				This is done via an AJAX call
//				errNo = WebTools.ListBind(lstSGLCode,sql,null,"GLAccount","GLAccountDescription","(All/any account)","");
//				errNo = WebTools.ListBind(lstEGLCode,sql,null,"GLAccount","GLAccountDescription","","");
//				SetErrorDetail("LoadData",errNo,"Unable to load GL account codes",sql);

				sql   = "exec sp_Audit_Get_GLAccountDimension";
				errNo = WebTools.ListBind(lstSGLDimension,sql,null,"CompanyCode","CompanyDescription","(All/any dimension)","");
				errNo = WebTools.ListBind(lstEGLDimension,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load GL account dimensions",sql);

				sql   = "exec sp_Audit_Get_TaxRate";
				errNo = WebTools.ListBind(lstSTaxRate,sql,null,"VatRateCode","VatRateDescription","(All/any tax rate)","");
//				errNo = WebTools.ListBind(lstETaxRate,sql,null,"VatRateCode","VatRateDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load tax rate list",sql);

				sql   = "exec sp_Audit_Get_CUR";
				errNo = WebTools.ListBind(lstECurr,sql,null,"CUR","CUR","","");
				SetErrorDetail("LoadData",errNo,"Unable to load currency list",sql);
			}
			catch (Exception ex)
			{
				SetErrorDetail("LoadData",80010,"Internal error",sql,2,2,ex);
			}
		}

		protected void grdData_Sort(Object sender, DataGridSortCommandEventArgs e)
		{
			string orderBy = e.SortExpression;
			RetrieveData(0,orderBy);
		}

		protected void grdData_Click(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				string       cmdName = e.CommandName.Trim().ToUpper();
				int          tranID  = Tools.StringToInt(e.CommandArgument.ToString());
				DataGridItem row     = e.Item;
				sql                  = "exec sp_Audit_Get_CashbookExtractAllFields @TransactionID=" + tranID.ToString();
				lblErr2.Text         = "";

				if ( cmdName == "EDIT" && tranID > 0 )
					using (MiscList cbTran = new MiscList())
						if ( cbTran.ExecQuery(sql,0) == 0 )
						{
						//	txtETranID.Text    = tranID.ToString();
							hdnETranID.Value   = tranID.ToString();
							txtEDate.Text      = Tools.DateToString(cbTran.GetColumnDate("TransactionDate"),7);
							txtERecon.Text     = Tools.DateToString(cbTran.GetColumnDate("ReconDate"),7);
							txtEAmt.Text       = cbTran.GetColumnCurrency("TransactionAmountInclusive");
							txtEDesc.Text      = cbTran.GetColumn("TransactionDescription");
							txtETaxRate.Text   = cbTran.GetColumn("TaxRate");
							cashBook           = cbTran.GetColumn("CashbookCode");
							hdnECashBook.Value = cashBook;
							ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(1);LoadCashBooks('"+cashBook+"','E')");
							lstECashBook.Items.Clear();
//	AJAX calls
//							WebTools.ListAdd(lstECashBook,0,hdnECashBook.Value,hdnECashBook.Value);
//							WebTools.ListSelect(lstEGLCode     ,cbTran.GetColumn("GLAccountCode"));
							WebTools.ListSelect(lstECompany    ,cbTran.GetColumn("CompanyCode"));
							WebTools.ListSelect(lstEOBOCompany ,cbTran.GetColumn("OBOCompanyCode"));
							WebTools.ListSelect(lstEReceipt    ,cbTran.GetColumn("RP"));
							WebTools.ListSelect(lstECurr       ,cbTran.GetColumn("CUR"));
							WebTools.ListSelect(lstETType      ,cbTran.GetColumn("TransactionDescription"));
							WebTools.ListSelect(lstEGLDimension,cbTran.GetColumn("GLAccountDimension"));
//							WebTools.ListSelect(lstETaxRate    ,cbTran.GetColumn("TaxRate"));
							LoadDataAJAX("E");
							lstECompany.Focus();
						}
			}
			catch
			{ }
		}

		protected void btnNew_Click(Object sender, EventArgs e)
		{
			cashBook           = Tools.NullToString(hdnSCashBook.Value);
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(2);LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'S')");
//			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(2)");
			hdnETranID.Value   = "";
//			txtETranID.Text    = "";
			txtEDate.Text      = "";
			txtERecon.Text     = "";
			txtEAmt.Text       = "";
			txtEDesc.Text      = "";
			txtETaxRate.Text   = "";
			hdnECashBook.Value = "";
			lblErr2.Text       = "";
//			WebTools.ListSelect(lstECurr       ,"");
//			WebTools.ListSelect(lstEGLCode     ,"");
//			WebTools.ListSelect(lstEGLDimension,"");
//			WebTools.ListSelect(lstETType      ,"");
//			WebTools.ListSelect(lstEReceipt    ,"");
//			WebTools.ListSelect(lstEOBOCompany ,"");
		//	lstECashBook.Items.Clear();
			lstECompany.Focus();
		}

		protected void btnDelete_Click(Object sender, EventArgs e)
		{
			string msg = "Failed to delete cashbook transaction";
			try
			{
				cashBook = Tools.NullToString(hdnECashBook.Value);
				sql      = "exec sp_Audit_Del_Cashbook @TransactionID=" + hdnETranID.Value;
				if ( Tools.StringToInt(hdnETranID.Value) > 0 )
					using ( MiscList miscList = new MiscList() )
						if ( miscList.UpdateQuery(sql) == 0 )
							return;
						else
							msg = miscList.ReturnMessage;
			}
			catch (Exception ex)
			{
				msg = "Internal error deleting cashbook transaction";
				Tools.LogException("pgAccountingCaptureCashbook.btnDelete_Click",sql,ex);
			}	
			SetErrorDetail("btnDelete_Click",80020,msg,sql,23,2);
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(1);LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'E')");
		}

		protected void btnUpdate_Click(Object sender, EventArgs e)
		{
			int      editInsert = Tools.StringToInt(hdnEditInsert.Value);
			int      taxRate    = Tools.StringToInt(txtETaxRate.Text);
			decimal  amt        = Tools.StringToDecimal(txtEAmt.Text);
			decimal  amtX       = amt;
			DateTime d1         = Tools.StringToDate(txtEDate.Text,7);
			DateTime d2         = Tools.StringToDate(txtERecon.Text,7);
			string   action     = ( editInsert == 1 ? "update" : "insert" );
			string   msg        = "Failed to " + action + " cashbook transaction";
			cashBook            = Tools.NullToString(hdnECashBook.Value);

			if ( taxRate < 1 )
				taxRate = 0;
			else if ( amt > 0 )
				amtX = amt / ( 1 + ( (decimal)taxRate / (decimal)100.00 ) );

			try
			{
				sql =  "@CompanyCode="                + Tools.DBString(WebTools.ListValue(lstECompany,""))
				    + ",@OBOCompanyCode="             + Tools.DBString(WebTools.ListValue(lstEOBOCompany,""))
				    + ",@CashbookCode="               + Tools.DBString(cashBook)
				    + ",@RP="                         + Tools.DBString(WebTools.ListValue(lstEReceipt,""))
				    + ",@TransactionDate="            + Tools.DateToSQL(d1,0)
				    + ",@ReconDate="                  + Tools.DateToSQL(d2,0)
				    + ",@GLAccountCode="              + Tools.DBString(WebTools.ListValue(lstEGLCode,""))
				    + ",@GLAccountDimension="         + Tools.DBString(WebTools.ListValue(lstEGLDimension,""))
				    + ",@TransactionDescription="     + Tools.DBString(txtEDesc.Text)
				    + ",@CUR="                        + Tools.DBString(WebTools.ListValue(lstECurr,""))
				    + ",@TaxRate="                    + taxRate.ToString()
					 + ",@TransactionAmountExclusive=" + amtX.ToString();

				if ( editInsert == 1 && Tools.StringToInt(hdnETranID.Value) > 0 )
					sql = "exec sp_Audit_Upd_Cashbook @TransactionID=" + hdnETranID.Value
					                              + ",@TransactionAmountTax=" + ( amtX * (decimal)taxRate / (decimal)100.00 ).ToString()
					                              + ",@TransactionAmountInclusive=" + amt.ToString() + "," + sql;
				else if ( editInsert == 2 )
					sql = "exec sp_Audit_Ins_Cashbook " + sql;
				else
					return;
	
				using ( MiscList miscList = new MiscList() )
					if ( miscList.UpdateQuery(sql) != 0 ) // Failed
						msg = miscList.ReturnMessage;
					else if ( editInsert == 1 )           // Update, close window
						return;
					else                                  // Insert, keep window open
					{
						msg = "Transaction successfully inserted";
						sql = "";
						btnNew_Click(null,null);
					}
			}
			catch (Exception ex)
			{
				msg = "Internal error trying to " + action + " cashbook transaction";
				Tools.LogException("pgAccountingCaptureCashbook.btnUpdate_Click",sql,ex);
			}	
			SetErrorDetail("btnUpdate_Click",80030,msg,sql,23,(byte)(sql.Length==0?0:2));
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode("+editInsert.ToString()+");LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'E')");
		}

		private void RetrieveData(int mode,string orderBy="")
		{
			string   cashBook  = Tools.NullToString(hdnSCashBook.Value);
			DateTime d1        = Tools.StringToDate(txtSDate1.Text,7);
			DateTime d2        = Tools.StringToDate(txtSDate2.Text,7);
//			DateTime dR        = Tools.StringToDate(txtSRecon.Text,7);
			decimal  a1        = Tools.StringToDecimal(txtSAmt1.Text);
			decimal  a2        = Tools.StringToDecimal(txtSAmt2.Text);
			lblError.Text      = "";
			ascxXFooter.JSText = WebTools.JavaScriptSource("LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'S')");
			grdData.Visible    = false;
			pnlGridBtn.Visible = false;

			if ( d1 > Constants.DateNull && d2 <= Constants.DateNull )
				SetErrorDetail("ValidateData",80050,"If you specify a start date you must also specify an end date");
			else if ( d2 > Constants.DateNull && d1 <= Constants.DateNull )
				SetErrorDetail("ValidateData",80053,"If you specify an end date you must also specify a start date");
			else if ( d1 > Constants.DateNull && d2 > Constants.DateNull && d1 > d2 )
				SetErrorDetail("ValidateData",80056,"The start date cannot be after the end date");
//			if ( d1 > Constants.DateNull && dR > Constants.DateNull && dR < d1 )
//				SetErrorDetail("ValidateData",80059,"The recon date cannot be before the start date");
//			if ( d2 > Constants.DateNull && dR > Constants.DateNull && dR > d2 )
//				SetErrorDetail("ValidateData",80062,"The recon date cannot be after the end date");
			if ( a1 > 0 && a2 <= (decimal)0.01 )
				SetErrorDetail("ValidateData",80065,"If you specify a from amount you must also specify a to amount");
			else if ( a2 > 0 && a1 <= (decimal)0.01 )
				SetErrorDetail("ValidateData",80068,"If you specify a to amount you must also specify a from amount");
			else if ( a1 > 0 && a2 > 0 && a1 > a2 )
				SetErrorDetail("ValidateData",80071,"The from amount cannot be greater than the to amount");

			if ( lblError.Text.Length > 0 )
				return;

			string coy       = WebTools.ListValue(lstSCompany,"");
			string coyOBO    = WebTools.ListValue(lstSOBOCompany,"");
			string receipt   = WebTools.ListValue(lstSReceipt,"");
//			string transType = WebTools.ListValue(lstSTType,"");
			string glAcc     = WebTools.ListValue(lstSGLCode,"");
			string glDim     = WebTools.ListValue(lstSGLDimension,"");
			string taxRate   = WebTools.ListValue(lstSTaxRate,"");

//			sql = "exec sp_Audit_Get_CashbookExtract @CompanyCode=" + Tools.DBString(coy);
			sql = "exec sp_Audit_Get_CashbookExtractA"
			    +     " @CompanyCode="            + Tools.DBString(coy)
			    +     ",@CashbookCode="           + Tools.DBString(cashBook)
			    +     ",@RP="                     + Tools.DBString(receipt)
			    +     ",@OBOCompanyCode="         + Tools.DBString(coyOBO)
			    +     ",@GLAccountCode="          + Tools.DBString(glAcc)
			    +     ",@GLAccountDimension="     + Tools.DBString(glDim)
			    +     ",@TransactionDescription=" + Tools.DBString(txtSDesc.Text)
			    +     ",@StartDate="              + Tools.DateToSQL(d1,0)
			    +     ",@EndDate="                + Tools.DateToSQL(d2,0)
			    +     ",@TaxRate="                + Tools.DBString(taxRate)
			    +     ",@MinAmount="              + a1.ToString()
			    +     ",@MaxAmount="              + a2.ToString();
			if ( orderBy.Length > 0 )
				sql = sql + ",@OrderBy="           + Tools.DBString(orderBy);
	
			using ( MiscList miscList = new MiscList() )
				if ( mode > 30 ) // Download
				{
					if ( miscList.ExecQuery(sql,0) > 0 ) // Do not load rows
						SetErrorDetail("RetrieveData",80080,"No transactions found. Refine your criteria and try again",sql,2,2);
					else if ( miscList.Download(mode, "CashBook", sessionGeneral.UserCode) == 0 )
						Response.Redirect("Download.ashx?File=" + miscList.FileName);
				}
				else if ( miscList.ExecQuery(sql,1,"",false,true) < 1 ) // Load rows into a list of "MiscData" objects
					SetErrorDetail("RetrieveData",80085,"No transactions found. Refine your criteria and try again",sql,2,2);
				else
				{
					grdData.Visible    = true;
					pnlGridBtn.Visible = true;
					grdData.DataSource = miscList;
					grdData.DataBind();
				}
		}

		protected void btnCSV_Click(Object sender, EventArgs e)
		{
			RetrieveData((int)Constants.DataFormat.CSV);
		}

		protected void btnPDF_Click(Object sender, EventArgs e)
		{
			RetrieveData((int)Constants.DataFormat.PDF);
		}

		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			RetrieveData(0);
		}
	}
}