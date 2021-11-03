// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 77000-77099

namespace PCIWebFinAid
{
	public partial class pgViewDashboardProsperianWealth : BasePageAdmin
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
			{
				SetErrorDetail("",-888);
				LoadTickers();
			}
			else
				StartOver(77088,(int)Constants.ErrorType.InvalidMenu);
		}

		private void LoadTickers(int chgCode=0,int chgStatus=0)
		{
			Image   tImg;
			Literal tName;
			Label   tStatus;
			Button  tButton;
			string  tickerName;

//	Set them all to STOPPED

			for ( int k = 1; k < 100; k++ )
			{
				tickerName = Tools.TickerName(k);
				if ( tickerName.Length > 0 )
				{
					tImg          = (Image)  FindControl("img"+k.ToString());
					if ( tImg    != null )   tImg.ImageUrl = "Images/LightR.png";
					tName         = (Literal)FindControl("lblName"+k.ToString());
					if ( tName   != null )   tName.Text = tickerName;
					tStatus       = (Label)  FindControl("lblStatus"+k.ToString());
					if ( tStatus != null )   tStatus.Text = "&nbsp;";
					tButton       = (Button) FindControl("btnStart"+k.ToString());
					if ( tButton != null )   tButton.Enabled = true;
					tButton       = (Button) FindControl("btnStop"+k.ToString());
					if ( tButton != null )   tButton.Enabled = false;
				}
			}

//	Now see if one is RUNNING

			using (TickerState tickerState = new TickerState())
				try
				{
					int ret                  = 0;
					tickerState.UserCode     = sessionGeneral.UserCode;
					tickerState.Origin       = "BackOffice.pgViewDashboardProsperianWealth.aspx";
					tickerState.DBConnection = "DBConnTrade";
	
					if ( chgStatus > 0 )
					{
						tickerState.TickerStatus = chgStatus.ToString();
						tickerState.TickerCode   = chgCode.ToString().PadLeft(3,'0');
						ret = tickerState.Update();
					}
					else
						ret = tickerState.Enquire();

					if ( ret != 0 )
						SetErrorDetail("LoadTickers", 77010, "Unable to load ticker status", tickerState.SQL);
					else if ( tickerState.TickerCode.Length > 0 && Tools.StringToInt(tickerState.TickerStatus) == (int)Constants.TickerAction.Run )
					{
						int tCode    = Tools.StringToInt(tickerState.TickerCode);
						tImg         = (Image)FindControl("img"       + tCode.ToString());
						if (tImg    != null) tImg.ImageUrl   = "Images/LightG.png";
						tStatus      = (Label)FindControl("lblStatus" + tCode.ToString());
						if (tStatus != null) tStatus.Text    = "Running&nbsp;";
						tButton      = (Button)FindControl("btnStart" + tCode.ToString());
						if (tButton != null) tButton.Enabled = false;
						tButton      = (Button)FindControl("btnStop"  + tCode.ToString());
						if (tButton != null) tButton.Enabled = true;
					}
				}
				catch (Exception ex)
				{
					SetErrorDetail("LoadTickers",77030,"Unable to load ticker status (internal error)",sql,2,2,ex);
				}
		}

		protected void btnRefresh_Click(Object sender, EventArgs e)
		{
			LoadTickers();
		}

		protected void btnStop_Click(Object sender, EventArgs e)
		{
			int ticker = Tools.StringToInt(((Button)sender).ToolTip);
			LoadTickers(ticker,(int)Constants.TickerAction.Stop);
		}

		protected void btnStart_Click(Object sender, EventArgs e)
		{
			int ticker = Tools.StringToInt(((Button)sender).ToolTip);
			if ( ticker > 0 )
				LoadTickers(ticker,(int)Constants.TickerAction.Run);
		}

		protected void btnShutDown_Click(Object sender, EventArgs e)
		{
			LoadTickers(0,(int)Constants.TickerAction.ShutDown);
		}
	}
}