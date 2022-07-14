using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class CAFooter : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public string JSText
		{
			get { return lblJS.Text.Trim(); }
			set { lblJS.Text = PCIBusiness.Tools.NullToString(value); }
		}
	}
}