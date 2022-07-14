<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewProductDashboard.aspx.cs" Inherits="PCIWebFinAid.pgViewProductDashboard" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<form id="frmMain" runat="server">
	<ascx:XMenu runat="server" ID="ascxXMenu"></ascx:XMenu>
	<div class="Header3">
	<asp:Literal runat="server" ID="X104504">104504</asp:Literal>
	</div>
	<table style="overflow-x:auto">
	<tr>
		<td class="DataLabel"><asp:Literal runat="server" ID="X104500">104500</asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblClientCode"></asp:Literal></td></tr>
	<tr>
		<td class="DataLabel"><asp:Literal runat="server" ID="X104501">104501</asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblName"></asp:Literal></td></tr>
	<tr>
		<td class="DataLabel"><asp:Literal runat="server" ID="X104502">104502</asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblContractCode"></asp:Literal></td></tr>
	<tr>
		<td class="DataLabel"><asp:Literal runat="server" ID="X104503">104503</asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblStatus"></asp:Literal></td></tr>
	</table>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
