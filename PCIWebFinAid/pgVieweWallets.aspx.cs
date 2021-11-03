// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;

// Error codes 80000-80099

namespace PCIWebFinAid
{
	public partial class pgVieweWallets : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) == 0 )
				LoadPageData();
			else
				StartOver(17666,(int)PCIBusiness.Constants.ErrorType.InvalidMenu);
		}

		protected override void LoadPageData()
		{
//		Called once in the beginning

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
			LoadLabelText(ascxXMenu);
		}
	}
}