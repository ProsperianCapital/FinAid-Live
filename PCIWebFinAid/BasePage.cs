using System;
using System.Web.UI.WebControls;
using PCIBusiness;

// Developed by Paul Kilfoil
// http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePage : StdDisposable
	{
		protected Label  lblError;
		protected Label  lblErrorDtl;
		protected Button btnErrorDtl;

		override protected void OnInit(EventArgs e) // You must set AutoEventWireup="false" in the ASPX page
		{
			base.OnInit(e);
			this.Load += new System.EventHandler(this.PageLoad);
		}

		protected virtual void StartOver(int errNo,string pageName="")
		{
			if ( pageName.Length < 6 )
				pageName = "pgLogon.aspx"; // The LOGON ADMIN version
			Response.Redirect ( pageName + ( errNo > 0 ? "?ErrNo=" + errNo.ToString() : "" ) , true );
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

		public string ApplicationCode
		{
			get
			{
				try
				{
					if ( Session["ApplicationCode"] == null )
						return "";
					string appCode = Session["ApplicationCode"].ToString().Trim();
					return appCode;
				}
				catch
				{ }
				return "";
			}
			set
			{
				Session["ApplicationCode"] = value;
			}
		}

		protected void SetErrorDetail(string method,int errCode,string errBrief="",string errDetail="",byte briefMode=2,byte detailMode=2,Exception ex=null,bool alwaysShow=false,byte errPriority=0)
		{
			if ( errCode == 0 )
				return;

			if ( lblError == null && detailMode == 0 ) // No brief msg holder and no detailed msg
				return;

			if ( errCode < 0 )
			{
				if ( lblError != null )
				{
					lblError.Text    = "";
					lblError.Visible = false;
				}
				if ( lblErrorDtl != null ) lblErrorDtl.Text    = "";
				if ( btnErrorDtl != null ) btnErrorDtl.Visible = false;
				return;
			}

			if ( lblError == null )
				lblError = new Label();

			string pageName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
			if ( Tools.NullToString(pageName).Length < 1 )
				pageName = "BasePage";

//	This will put (eg) "PCIWebFinAid." in front of the page name
//			try
//			{
//				string x = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
//				pageName = x + "." + pageName;
//			}		
//			catch
//			{ }	

			method = ( Tools.NullToString(method).Length > 0 ? method+"[SetErrorDetail]." : "SetErrorDetail/" ) + errCode.ToString();
			if ( errPriority < 10 )
				errPriority = 10;
			Tools.LogInfo(pageName+"."+method,errBrief+" ("+errDetail+")",errPriority);

			if ( ex != null )
			{
				Tools.LogException(pageName+"."+method,errBrief,ex);
				if ( Tools.NullToString(errDetail).Length < 1 )
					errDetail = ex.Message;
			}

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else if ( briefMode == 101 ) // Overwrite, add in 1 <br /> BEFORE
				lblError.Text = "<br />" + errBrief;
			else if ( briefMode == 102 ) // Overwrite, add in 2 <br /> BEFORE
				lblError.Text = "<br /><br />" + errBrief;
			else if ( briefMode == 201 ) // Overwrite, add in 1 <br /> AFTER
				lblError.Text = errBrief + "<br />";
			else if ( briefMode == 202 ) // Overwrite, add in 2 <br /> AFTER
				lblError.Text = errBrief + "<br /><br />";

			else if ( briefMode == 23 ) // Use "lblErr2", <p></p>
				try
				{
					((Label)FindControl("lblErr2")).Text = "<p>" + errBrief + "</p>";
				}
				catch {}

			else if ( briefMode == 33 ) // Use "lblErr3", <br />
				try
				{
					((Label)FindControl("lblErr3")).Text = "<br />" + errBrief;
				}
				catch {}
			else
				lblError.Text = errBrief;

			lblError.Visible = ( lblError.Text.Length > 0 && lblError.Enabled );

			if ( lblErrorDtl == null || detailMode == 0 ) // Do not log a detailed message at all
				return;

			if ( errDetail.Length < 1 )
				errDetail = errBrief;
			errDetail = "[" + errCode.ToString() + "] " + errDetail;
			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
			if ( detailMode == 2 ) // Append
				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
			lblErrorDtl.Text = errDetail;
			if ( ! lblErrorDtl.Text.StartsWith("<div") )
				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='"
				                 + Tools.ImageFolder()
				                 + "Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;

			btnErrorDtl.Visible = ( lblErrorDtl.Text.Length > 0 ) && ( ! Tools.SystemIsLive() || alwaysShow );
		}
	}
}