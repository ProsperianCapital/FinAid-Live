using System;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class PYHeader : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
		public DropDownList lstLanguage
		{
			get { return lstLang; }
		}
	}
}