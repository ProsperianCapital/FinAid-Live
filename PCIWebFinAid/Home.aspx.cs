using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Home : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			byte   err  = 0;
			string url  = Request.Url.AbsoluteUri.Trim();
			string goTo = "";

		//	The above is the best one to use
		//	string urlT1 = Tools.ObjectToString(Request.UrlReferrer);
		//	string urlT2 = Tools.ObjectToString(Request.Headers["Referer"]);
		//	string urlT3 = Request.Url.AbsolutePath;
		//	string urlT4 = Request.Url.OriginalString;

			try
			{
				int    k         = url.IndexOf("://");
				string dName     = url.Substring(k+3);
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
						goTo               = mList.GetColumn("ApplicationLandingPage");

						if ( appStatus != "A" )
							err  = 10;
						else if ( goTo.Length > 0 )
							err  = 0;
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.Registration)     ) // 000
							goTo = "Register3.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.CRM)              ) // 002
							goTo = "pgLogonCRM.aspx";
//						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.Mobile)           ) // 006
//							goTo = "pgLogonCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.CareAssist)       ) // 100
							goTo = "CAHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.iSOS)             ) // 110
							goTo = "ISHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.LifeGuru)         ) // 120
							goTo = "LGHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.PayPayYa)         ) // 130
							goTo = "PYHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.AdvantageCard)    ) // 140
							goTo = "ADVHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.AdvantageCardCRM) ) // 145
							goTo = "ADVCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.SmartStox)        ) // 160
							goTo = "PCISSHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.SmartStoxCRM)     ) // 165
							goTo = "SSCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.RewardsVault)     ) // 170
							goTo = "RVHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.RewardsVaultCRM)  ) // 175
							goTo = "RVCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.TankwaCyber)      ) // 180
							goTo = "TCFHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.TankwaCyberCRM)   ) // 185
							goTo = "TCFCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.ProsperianSTO)    ) // 190
							goTo = "PCISTOHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.ProsperianSTOCRM) ) // 195
							goTo = "PCISTOCRM.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.SmartMoney)       ) // 200
							goTo = "SMHome.aspx";
						else if ( ApplicationCode == Tools.SystemCode(Constants.ApplicationCode.MoneyMaster)      ) // 210
							goTo = "MMHome.aspx";
						else
							err  = 20;
					}
					else
						err =  0;
					//	err = 30;

				if ( goTo.Length < 1 )
					goTo = "pgLogon.aspx";
				else if ( ! goTo.ToUpper().Contains(".ASPX") )
					goTo = goTo + ".aspx";

				if ( err > 0 )
					Tools.LogInfo("Home.PageLoad/1","err="       + err.ToString()
					                            + ", url="       + url
					                            + ", domain="    + dName
					                            + ", goTo="      + goTo
					                            + ", parms="     + parms
					                            + ", appCode="   + ApplicationCode
					                            + ", appStatus=" + appStatus, 222);
				err = 0;
				Response.Redirect(goTo+parms);
			}	
			catch (System.Threading.ThreadAbortException)
			{
			//	Ignore
			}
			catch (Exception ex)
			{
				if ( err < 1 )
					err = 90;
				Tools.LogException("Home.PageLoad/2","err="     + err.ToString()
				                                 + ", url="     + url
				                                 + ", appCode=" + ApplicationCode
				                                 + ", goTo="    + goTo,ex);
			}

			if ( err > 0 )
				Tools.LogInfo("Home.PageLoad/3","err="     + err.ToString()
				                            + ", url="     + url
				                            + ", appCode=" + ApplicationCode
				                            + ", goTo="    + goTo, 222);
		}
	}
}