using System;
using System.Text;

namespace PCIWebFinAid
{
	public partial class XTransLookup : BasePageAdmin
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			Response.Redirect("TransLookup.aspx");
		}
	}
}