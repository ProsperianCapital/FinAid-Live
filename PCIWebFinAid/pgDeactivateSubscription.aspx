<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgDeactivateSubscription.aspx.cs" Inherits="PCIWebFinAid.pgDeactivateSubscription" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->

<script type="text/javascript">
function Validate(pic)
{
	try
	{
		var d = [null,null,null,null,null,null,null];
	}
	catch (x)
	{ }
	return false;
}
</script>

<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104308">104308</asp:Literal>
</div>

<p class="Note">
<asp:Literal runat="server" ID="X104318">104318</asp:Literal>
</p><p>
<asp:Literal runat="server" ID="X104309">104309</asp:Literal>&nbsp;
<span class="DataStatic">
<asp:Literal runat="server" ID="lblCurr"></asp:Literal>
<asp:Literal runat="server" ID="lblBalance"></asp:Literal>
</span>
</p><p>
<asp:Literal runat="server" ID="X104312">104312</asp:Literal><br />
<asp:DropDownList runat="server" ID="lstReason" style="margin-top:5px"></asp:DropDownList>
</p><p class="DataStatic">
<asp:Literal runat="server" ID="X104316">104316</asp:Literal>
</p>
<asp:Button runat="server" id="X104317" Text="104317" style="width:160px" OnClick="btnChange_Click" />
<asp:Button runat="server" id="X104314" Text="104314" style="width:160px" OnClick="btnConfirm_Click" />
<br /><br />
<asp:Label runat="server" ID="X104370" CssClass="Info">104370</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>