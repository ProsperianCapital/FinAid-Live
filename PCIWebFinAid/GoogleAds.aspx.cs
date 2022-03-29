using System;
using System.Text;
using System.Net;
using System.IO;
using PCIBusiness;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class GoogleAds : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( ! Page.IsPostBack )
			{
				txtToken.Text  = "V03y8kx1jCNb38bHQuBxbw"; // Test
			//	txtToken.Text  = "yVSGRK6SaKNv386UnNnDXA"; // Live
				txtClient.Text = "519447257225-k9hh8hvihtgrvaebt6hp6rl2891gr0fe.apps.googleusercontent.com";
				txtSecret.Text = "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";
				txtURL.Text    = "https://www.googleapis.com/auth/adwords";
				lblVer.Text    = WebTools.VersionDetails(1);
			}
			lblError.Text = "";
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			lblError.Visible = true;
			txtOut.Text      = "";
			string msg       = "";

			try
			{
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

//	Get OAuth token
				string oauthToken = "Blah";
				if ( oauthToken.Length < 1 )
				{
					lblError.Text = "(OAuth) Unable to obtain an OAuth token";
					return;
				}

//	Initiate connection
				string dataOut = "";
				string dataIn  = Tools.JSONPair("messageId","blah");
				string url     = txtURL.Text;
				byte[] page                         = Encoding.UTF8.GetBytes(dataIn);
				System.Net.HttpWebRequest webReq    = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
				webReq.ContentType                  = "application/json;charset=\"utf-8\"";
				webReq.Accept                       = "application/json";
				webReq.Method                       = "POST";
				webReq.Headers["Authorization"]     = "Bearer " + oauthToken;
				webReq.Headers["developer-token"]   = txtToken.Text;
//				webReq.Headers["login-customer-id"] = "Blah";

				Tools.LogInfo("btnOK_Click/30","URL="+url+", dataIn="+dataIn,222,this);

				using (System.IO.Stream stream = webReq.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}
				using (System.Net.WebResponse webResponse = webReq.GetResponse())
					using (System.IO.StreamReader rd = new System.IO.StreamReader(webResponse.GetResponseStream()))
						dataOut = rd.ReadToEnd();
				webReq = null;

				txtOut.Text = dataOut;
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,"btnOK_Click/70","GoogleAds");
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("btnOK_Click/80",msg,220,this);
				Tools.LogException("btnOK_Click/90",msg,ex2,this);
			}
		}
	}
}