using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class Register : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( ! Page.IsPostBack )
			{
				lblJS.Text = WebTools.JavaScriptSource("NextPage(0)");
				LoadLabels();
			}
		}

		private void LoadLabels()
		{
			using (PCIBusiness.MiscList miscList = new PCIBusiness.MiscList())
				try
				{
					string       fieldCode;
					string       controlID;
					HiddenField  ctlHidden;
					Literal      ctlLiteral;
					Label        ctlLabel;
					DropDownList ctlList;

					if ( miscList.ExecQuery("exec sp_Get_PageLabels 1",0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode = miscList.GetColumn("FieldCode");
							controlID = "";
							if      ( fieldCode == "100111" ) controlID = "Title";
							else if ( fieldCode == "100114" ) controlID = "Surname";
							else if ( fieldCode == "100117" ) controlID = "CellNo";

							else if ( fieldCode == "10011x" ) controlID = "FirstName";
							else if ( fieldCode == "10011x" ) controlID = "EMail";
							else if ( fieldCode == "10011x" ) controlID = "ID";

							else if ( fieldCode == "10011x" ) controlID = "Income";
							else if ( fieldCode == "10011x" ) controlID = "Status";
							else if ( fieldCode == "10011x" ) controlID = "PayDay";

							else if ( fieldCode == "100107" ) controlID = "SubHead1";
							else if ( fieldCode == "100122" ) controlID = "SubHead3";
							else if ( fieldCode == "100136" ) controlID = "SubHead4";

							if ( controlID.Length > 0 )
							{
								ctlLiteral = (Literal)FindControl("lbl"+controlID+"Label");
								if ( ctlLiteral != null )
									ctlLiteral.Text = miscList.GetColumn("FieldLabel");

								ctlHidden   = (HiddenField)FindControl("hdn"+controlID+"Help");
								if ( ctlHidden != null )
									ctlHidden.Value = miscList.GetColumn("FieldHelp");

								ctlLabel    = (Label)FindControl("lbl"+controlID+"Error");
								if ( ctlLabel != null )
									ctlLabel.ToolTip = miscList.GetColumn("FieldError");

//								ctlText    = (TextBox)FindControl("txt"+controlID);
//								if ( ctlText != null )
//									ctlText.Text = miscList.GetColumn("FieldLabel");
//
//								ctlList    = (DropDownList)FindControl("lst"+controlID);
//								if ( ctlList != null )
//									ctlList.Text = miscList.GetColumn("FieldLabel");
							}
							miscList.NextRow();
						}
					else
						lblJS.Text = WebTools.JavaScriptSource("TestSetup()",lblJS.Text,1);
				}
				catch (Exception ex)
				{
					int k = 0;
				}
		}
	}
}