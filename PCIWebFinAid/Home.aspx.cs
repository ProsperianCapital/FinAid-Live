using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Home : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			byte   err = 0;
			string url = Request.Url.AbsoluteUri.Trim();

		//	The above is the best one to use
		//	string urlT1 = Tools.ObjectToString(Request.UrlReferrer);
		//	string urlT2 = Tools.ObjectToString(Request.Headers["Referer"]);
		//	string urlT3 = Request.Url.AbsolutePath;
		//	string urlT4 = Request.Url.OriginalString;

			try
			{
				int    k         = url.IndexOf("://");
				string dName     = url.Substring(k+3);
				string goTo      = "pgLogon.aspx";
				string parms     = "";
				string appStatus = "*";
				ApplicationCode  = "";

				k = dName.ToUpper().IndexOf("/HOME.ASPX");
				if ( k > 0 )
					dName = dName.Substring(0,k);
				k = url.IndexOf("?");
				if ( k > 0 )
					parms = url.Substring(k);
				string sql = "exec sp_Get_BackOfficeApplication " + Tools.DBString(dName);

				using (MiscList mList = new MiscList())
					if ( mList.ExecQuery(sql,0) == 0 )
					{
						string appSecurity = mList.GetColumn("EnforceMenuItemSecurity");
						appStatus          = mList.GetColumn("ApplicationStatus").ToUpper();
						ApplicationCode    = mList.GetColumn("ApplicationCode");

						if ( appStatus != "A" )
							err  = 10;
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.Registration) ) // 000
							goTo = "Register3.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.CRM)          ) // 002
							goTo = "pgLogonCRM.aspx";
//						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.Mobile)       ) // 006
//							goTo = "pgLogonCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.CareAssist)   ) // 100
							goTo = "CAHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.iSOS)         ) // 110
							goTo = "ISHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.LifeGuru)     ) // 120
							goTo = "LGHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.PayPayYa)     ) // 170
							goTo = "PYHome.aspx";
						else
							err  = 20;
					}
					else
						err =  0;
					//	err = 30;

				if ( err > 0 )
					Tools.LogInfo("Home.PageLoad/1","err="       + err.ToString()
					                            + ", url="       + url
					                            + ", domain="    + dName
					                            + ", goTo="      + goTo
					                            + ", parms="     + parms
					                            + ", appCode="   + ApplicationCode
					                            + ", appStatus=" + appStatus, 222);

				Response.Redirect(goTo+parms);
			}	
			catch (System.Threading.ThreadAbortException)
			{
			//	Ignore
			}
			catch (Exception ex)
			{
				err = 90;
				Tools.LogException("Home.PageLoad/2","url="+url,ex);
			}

			if ( err > 0 )
				Tools.LogInfo("Home.PageLoad/3","err="     + err.ToString()
				                            + ", url="     + url
				                            + ", appCode=" + ApplicationCode, 222);
		}
	}
}