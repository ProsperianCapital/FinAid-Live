using System;

namespace PCIWebFinAid
{
	public partial class AJAXProcess : System.Web.UI.Page
	{
		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new System.EventHandler(this.PageLoad);
		}

		protected void PageLoad(object sender, EventArgs e)
		{
			Response.Expires = -1 ;
			Response.Write ("<?xml version='1.0' encoding='UTF-8' standalone='yes'?>") ;

			int procType = 0;

			try
			{
				procType = PCIBusiness.Tools.StringToInt(Request.QueryString["Type"].ToString());

				if ( procType == 1 ) // Cashbook lookup
				{
					string companyCode = PCIBusiness.Tools.NullToString(Request.QueryString["CompanyCode"]);
					string selCode     = PCIBusiness.Tools.NullToString(Request.QueryString["Selected"]);
					string searchEdit  = PCIBusiness.Tools.NullToString(Request.QueryString["SearchEdit"]);
					string cCode;
					Response.Write ("<CashBooks><SearchEdit>"+searchEdit.ToUpper()+"</SearchEdit>");
					using (PCIBusiness.MiscList cBooks = new PCIBusiness.MiscList())
						if ( cBooks.ExecQuery("exec sp_Audit_Get_CompanyCashbook @CompanyCode="+PCIBusiness.Tools.DBString(companyCode),1,"",true,true) > 0 )
							foreach ( PCIBusiness.MiscData cashBook in cBooks )
							{
								cCode = cashBook.NextColumn;
								Response.Write(PCIBusiness.Tools.XMLCell("CBCode",cCode));
								Response.Write(PCIBusiness.Tools.XMLCell("CBName",cashBook.NextColumn));
								Response.Write(PCIBusiness.Tools.XMLCell("CBSel",( selCode.Length > 0 && selCode == cCode ? "Y" : "N" )));
							//	Response.Write(PCIBusiness.Tools.XMLCell("CBType",searchEdit));
							}
					Response.Write ("</CashBooks>");
				}

				else if ( procType == 2 ) // GL Codes
				{
					string glCode     = PCIBusiness.Tools.NullToString(Request.QueryString["GLCode"]);
					string selCode    = PCIBusiness.Tools.NullToString(Request.QueryString["Selected"]);
					string searchEdit = PCIBusiness.Tools.NullToString(Request.QueryString["SearchEdit"]);
					string cCode;
					Response.Write ("<GL><SearchEdit>"+searchEdit.ToUpper()+"</SearchEdit>");
					using (PCIBusiness.MiscList gls = new PCIBusiness.MiscList())
						if ( gls.ExecQuery("exec sp_Audit_Get_GLAccount @GLAccount="+PCIBusiness.Tools.DBString(glCode),1,"",true,true) > 0 )
							foreach ( PCIBusiness.MiscData gl in gls )
							{
								cCode = gl.NextColumn;
								Response.Write(PCIBusiness.Tools.XMLCell("GLCode",cCode));
								Response.Write(PCIBusiness.Tools.XMLCell("GLName",gl.NextColumn));
								Response.Write(PCIBusiness.Tools.XMLCell("GLSel",( selCode.Length > 0 && selCode == cCode ? "Y" : "N" )));
							}
					Response.Write ("</GL>");
				}

			}
			catch (Exception ex)
			{
				PCIBusiness.Tools.LogException("AJAXProcess.PageLoad","procType="+procType.ToString(),ex);
			}
		}
	}
}
