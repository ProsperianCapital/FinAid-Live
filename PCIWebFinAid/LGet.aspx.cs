using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class LGet : BasePageCRMv1
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			maxTab               = 2;
			ascxMenu.CurrentPage = "103026";

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