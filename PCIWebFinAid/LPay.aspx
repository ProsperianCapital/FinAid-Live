<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LPay.aspx.cs" Inherits="PCIWebFinAid.LPay" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Pay</title>
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
<asp:Literal runat="server" ID="X104142"></asp:Literal><br /><br />
<asp:Button runat="server" ToolTip="btn00" id="X104002" style="width:160px;background-color:black" OnClientClick="JavaScript:location.href='LWelcome.aspx';return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn01" id="X104144" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(1);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn02" id="X104146" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(2);return false" />
</td>
<td style="vertical-align:top">

<div id="div01">
<asp:Label runat="server" ID="X104390" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104152" />
<asp:TextBox runat="server" Width="160px" ID="txtRegAmt"></asp:TextBox>
<br /><br />
<asp:Button runat="server" id="X104154" />
<br /><br />
<asp:Label runat="server" ID="X104354" CssClass="Error"></asp:Label>
</div>

<div id="div02">
<asp:Label runat="server" ID="X104179" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104184" />
<asp:TextBox runat="server" Width="160px" ID="txtArrearAmt"></asp:TextBox>
<br /><br />
<asp:Button runat="server" id="X104186" />
<br /><br />
<asp:Label runat="server" ID="X104188" CssClass="Error"></asp:Label>
</div>

</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>