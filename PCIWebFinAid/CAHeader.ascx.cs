using System;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class CAHeader : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
		public DropDownList lstLanguage
		{
			get { return lstLang; }
		}
		public void Exception(byte excCode)
		{
			if ( excCode == 129 ) // PCISSHome.aspx
				HJs.Text = WebTools.JavaScriptSource("GetElt('HRow').style.backgroundColor='#000000'");
		}
	}
}