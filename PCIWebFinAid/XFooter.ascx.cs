using System;

namespace PCIWebFinAid
{
	public partial class XFooter : System.Web.UI.UserControl
	{
		private byte clearJS;
		protected void Page_Load(object sender, EventArgs e)
		{
			if ( clearJS != 77 )
				lblJS.Text = "";

			if ( ! Page.IsPostBack )
			{
				if ( Session["ApplicationCode"] == null )
					hdnVer.Value = "";
				else
					hdnVer.Value = "Application " + Session["ApplicationCode"].ToString().Trim() + ", ";
				hdnVer.Value    = hdnVer.Value + "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
				lblVer.Text     = hdnVer.Value;
//	Temporarily taken out ...
//				lblVer.Visible = ! PCIBusiness.Tools.SystemIsLive();
			}
		}

		public string JSText
		{
			get { return lblJS.Text.Trim(); }
			set
			{
				lblJS.Text = PCIBusiness.Tools.NullToString(value);
				clearJS    = 77; // This is because the "Page_Load" event occurs AFTER this call, so the JS is overwritten
			}
		}
	}
}