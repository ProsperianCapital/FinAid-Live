using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class CALogin : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			SessionCheck(3);

			if ( ! Page.IsPostBack )
			{
				hdnVer.Value                       = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
				sessionGeneral.ProductCode         = WebTools.RequestValueString(Request,"PC");
				sessionGeneral.LanguageCode        = WebTools.RequestValueString(Request,"LC");
				sessionGeneral.LanguageDialectCode = WebTools.RequestValueString(Request,"LDC");

//	Testing 1 (English)
				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10278";
				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "ENG";
				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0002";

//	Testing 2 (Thai)
//				if ( sessionGeneral.ProductCode.Length         < 1 ) sessionGeneral.ProductCode         = "10024";
//				if ( sessionGeneral.LanguageCode.Length        < 1 ) sessionGeneral.LanguageCode        = "THA";
//				if ( sessionGeneral.LanguageDialectCode.Length < 1 ) sessionGeneral.LanguageDialectCode = "0001";

				SessionClearData();
				SessionSave();
			}
		}
	}
}