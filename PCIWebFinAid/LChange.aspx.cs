using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class LChange : BasePageCRMv1
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			maxTab               = 5;
			ascxMenu.CurrentPage = "103029";

			if ( SessionCheck()  != 0 )
				return;
//			if ( SecurityCheck() != 0 )
//				return;
			if ( PageCheck()     != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( LoadLabelText(ascxMenu) != 0 )
				return;
		}
	}
}