<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LView.aspx.cs" Inherits="PCIWebFinAid.LView" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>View</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>

<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />

<!--#include file="IncludeLogin1.htm" -->

<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;font-weight:bold;font-size:24px;padding:10px">
<asp:Literal runat="server" ID="X104090"></asp:Literal><br /><br />
<asp:Button runat="server" ToolTip="btn00" id="X104002" style="width:160px;background-color:black" OnClientClick="JavaScript:location.href='LWelcome.aspx';return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn01" id="X104091" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(1);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn02" id="X104093" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(2);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn03" id="X104094" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(3);return false" />
</td>
<td style="vertical-align:top">

<div id="div01">
<asp:Label runat="server" ID="X104095" CssClass="Header4"></asp:Label>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104096" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtRegFee"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104106" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtMonthlyFee"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104098" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtGrantLimit"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104102" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtGrantAvail"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104104" /></td>
	<td><asp:TextBox runat="server" Width="480px" ReadOnly="true" ID="txtGrantStatus"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104108" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtFeeDate"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Label runat="server" ID="X104110" CssClass="Error"></asp:Label>
</div>

<div id="div02">
<asp:Label runat="server" ID="X104117" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Table runat="server" ID="tblStatement" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104118" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104119" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104120" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104121" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br />
<asp:Literal runat="server" ID="X104360" />
<asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtBalance"></asp:TextBox>
<br /><br />
<asp:Label runat="server" ID="X104126" CssClass="Error"></asp:Label>
</div>

<div id="div03">
<asp:Label runat="server" ID="X104128" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Table runat="server" ID="tblActivity" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104134" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104136" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br /><br />
<asp:Label runat="server" ID="X104140" CssClass="Error"></asp:Label>
</div>

</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>