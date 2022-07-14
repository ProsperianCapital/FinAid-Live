using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LAdmin : BasePageCRMv1
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			maxTab                = 0;
//			ascxMenu.CurrentPage  = "Welcome";

			if ( SessionCheck(19)  != 0 )
				return;
//			if ( SecurityCheck(19) != 0 )
//				return;
//			if ( PageCheck()       != 0 )
//				return;
			if ( Page.IsPostBack )
				return;
//			if ( LoadLabelText(ascxMenu) != 0 )
//				return;

			lblDate.Text = Tools.DateToString(System.DateTime.Now,7,1); // yyyy-mm-dd hh:mm:ss
			ascxMenu.SetAdmin();
			SessionClearData();
		}
	}
}