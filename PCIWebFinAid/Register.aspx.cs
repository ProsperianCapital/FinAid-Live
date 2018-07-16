using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ctlFooter.JSText = WebTools.JavaScriptSource("NextPage(0)");
		}
	}
}