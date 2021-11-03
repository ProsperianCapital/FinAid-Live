using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XHome : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(ApplicationCode,sessionGeneral) != 0 )
				StartOver(10995,(int)Constants.ErrorType.InvalidMenu);
		}
	}
}