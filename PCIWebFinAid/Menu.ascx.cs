using System;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class Menu : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public string CurrentPage
		{
			set
			{
				Button btn = (Button)FindControl("X"+value);
				if ( btn != null )
					lblMenuJS.Text = WebTools.JavaScriptSource("GetElt('" + btn.ClientID + "').style.backgroundColor = '#E68A00'");
				//	btn.BackColor = System.Drawing.Color.Orange;
			}
		}

		public void SetAdmin()
		{
			X103026.Text    = "Home";
			X103026.ToolTip = "LAdmin";
			X103028.Text    = "Log Off";
			X103028.ToolTip = "Login";
			X103027.Visible = false;
			X103029.Visible = false;
			X103030.Visible = false;
		}

		protected void MenuClick(Object sender, EventArgs e)
		{
			if ( sender.GetType() == typeof(Button) )
				Response.Redirect(((Button)sender).ToolTip+".aspx");
		}
	}
}