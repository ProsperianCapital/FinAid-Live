<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XHeader.ascx.cs" Inherits="PCIWebFinAid.XHeader" %>

<style>
.HImageBig {
	display: inline;
	visibility: visible;
	height: 60px;
}
.HImageSmall {
	display: none;
	visibility: hidden;
	height: 60px;
}
@media screen and (max-width: 600px) {
	.HImageBig {
		display: none;
		visibility: hidden;
	}
	.HImageSmall {
		display: inline;
		visibility: visible;
	}
}
</style>

<div class="Header1">
	<asp:PlaceHolder runat="server" ID="pnl001" Visible="false">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>PlaNetBig-Black.png"   title="PlaNet Tech Limited" class="HImageBig" />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>PlaNetSmall-Black.png" title="PlaNet Tech Limited" class="HImageSmall" />
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="pnl002" Visible="false">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductLogoCareAssist.png" title="CareAssist" class="HImageBig" />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductLogoCareAssist.png" title="CareAssist" class="HImageSmall" />
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="pnl003" Visible="false"></asp:PlaceHolder> <!--  Add as necessary -->

	<div style="float:right;margin-right:20px">
		<asp:Label runat="server" ID="lblUName" style="top:12px;position:relative"></asp:Label>
		&nbsp;&nbsp;&nbsp;&nbsp;
		<asp:HyperLink runat="server" ID="lnkMessages" ToolTip="Notifications are not available" NavigateUrl="#" style="top:18px;position:relative">
			<!-- (removed from above) onclick="JavaScript:ShowMessages(1)" -->
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Bell1.png" height="24" />
		</asp:HyperLink>
	</div>
</div>

<div id="pnlMessages" style="border:1px solid #000000;width:200px;float:right;visibility:hidden;display:none;padding:3px;margin-top:2px;background-color:yellow">
<div class="HelpHead">Notifications&nbsp;<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" style="float:right" onclick="JavaScript:ShowMessages(0)" title="Close" /></div>
<p>
You have to blah, blah, blah
</p><p>
Answer your phone!
</p><p>
That email from Francois needs attention ... NOW!
</p>
</div>

<script type="text/javascript">
function ShowMessages(show)
{
	show = 0;
//	ShowElt('pnlMessages',(show>0));
}
</script>
