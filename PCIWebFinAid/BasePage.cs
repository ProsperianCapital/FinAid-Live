using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Developed by Paul Kilfoil
// http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePage : StdDisposable
	{
//		protected SessionGeneral sessionGeneral;

//		private struct WebSetup
//		{
//			string productCode;
//			string languageCode;
//			string languageDialect;
//		}
//		private WebSetup webSetup;

		override protected void OnInit(EventArgs e) // You must set AutoEventWireup="false" in the ASPX page
		{
			base.OnInit(e);
			this.Load += new System.EventHandler(this.PageLoad);
		}

//		protected void SessionSave()
//		{
//			if ( sessionGeneral == null )
//				sessionGeneral = new SessionGeneral();
//			Session["SessionGeneral"] = sessionGeneral;
//		}

//		protected void SessionClear()
//		{
//			sessionGeneral            = null;
//			Session["SessionGeneral"] = null;
//		}

		protected int PageCheck(byte mode=0)
		{
			return 0;
		}

		protected int SessionCheck(byte sslMode, byte requireSession, byte requireUser)
		{
			return 0;
		}

		protected void CheckSSL(byte sslMode=0)
		{
			if ( sslMode < 1 )
				return;

			HttpContext context  = HttpContext.Current;
			bool        isSecure = context.Request.IsSecureConnection;
			string      url      = context.Request.Url.ToString();
			string      urlLive  = PCIBusiness.Tools.ConfigValue("HttpsURLString").ToString();

			if ( sslMode == 76 )
			{
				if ( isSecure ) // Force "http"
					context.Response.Redirect(url.Replace("https:","http:"),true);
			}
			else if ( ! isSecure && urlLive.Length > 0 && url.ToUpper().Contains(urlLive.ToUpper()) ) // Force "https"
				context.Response.Redirect(url.Replace("http:","https:"),true);
		}

		public override void Close()
		{
		// This will automatically be called by the base class destructor (StdDisposable).

		//	Clean up the derived class
			CleanUp();

		//	Clean up the base class
		//	sessionGeneral = null;
		}

		public virtual void CleanUp()
		{
		//	This method can be overridden in the derived class to CLEAN UP stuff - not to initialize in the beginning
		//	Nothing here, so can completely override it in the derived class
		}

		protected abstract void PageLoad(object sender, EventArgs e);

		//	This method MUST be overridden in the derived class - it is the standard "Page Load" event
		// You must also set AutoEventWireup="false" in the ASPX page

		//	Put this in the derived class:
		//	protected override void PageLoad(object sender, EventArgs e)

	}
}
