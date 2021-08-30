<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LChange.aspx.cs" Inherits="PCIWebFinAid.LChange" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Change</title>
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
<asp:Button runat="server" ToolTip="btn01" id="X104192" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(1);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn02" id="X104193" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(2);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn03" id="X104194" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(3);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn04" id="X104195" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(4);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn05" id="X104196" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(5);return false" />
</td>
<td style="vertical-align:top">

<div id="div01">
<asp:Label runat="server" ID="X104197" CssClass="Header4"></asp:Label>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104198" /></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINOld"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104200" /></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINNew1"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104202" /></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINNew2"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Button runat="server" id="X104204" />
<br /><br />
<asp:Label runat="server" ID="X104355" CssClass="Error"></asp:Label>
</div>

<div id="div02">
<asp:Label runat="server" ID="X104214" CssClass="Header4"></asp:Label>
<br /><br />
<b><asp:Literal runat="server" ID="Y104215" /></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104216" /></td>
	<td><asp:Literal runat="server" ID="lblLine1">Addr Line 1</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104218" /></td>
	<td><asp:Literal runat="server" ID="lblLine2">Addr Line 2</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104220" /></td>
	<td><asp:Literal runat="server" ID="lblLine3">Addr Line 3</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104222" /></td>
	<td><asp:Literal runat="server" ID="lblLine4">Addr Line 4</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104224" /></td>
	<td><asp:Literal runat="server" ID="lblLine5">Addr Line 5</asp:Literal></td></tr>
</table>
<hr />
<b><asp:Literal runat="server" ID="Y104226" /></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="Y104216" /></td>
	<td><asp:TextBox runat="server" ID="txtLine1" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104218" /></td>
	<td><asp:TextBox runat="server" ID="txtLine2" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104220" /></td>
	<td><asp:TextBox runat="server" ID="txtLine3" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104222" /></td>
	<td><asp:TextBox runat="server" ID="txtLine4" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104224" /></td>
	<td><asp:TextBox runat="server" ID="txtLine5" Width="400px"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Button runat="server" id="X104237" />
<br /><br />
<asp:Label runat="server" ID="X104356" CssClass="Error"></asp:Label>
</div>

<div id="div03">
<asp:Label runat="server" ID="XYZ1" CssClass="Header4">Contact Info</asp:Label>
<br /><br />
Unspecified
<br /><br />
<asp:Label runat="server" ID="XYZ2" CssClass="Error">Deon, please help!</asp:Label>
</div>

<div id="div04">
<asp:Label runat="server" ID="X104268" CssClass="Header4"></asp:Label>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104269" /></td><td><asp:TextBox runat="server" Width="320px" ID="txtCCName"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104273" /></td><td><asp:TextBox runat="server" Width="320px" ID="txtCCNumber"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104275" /></td>
	<td>
		<asp:DropDownList runat="server" ID="lstCCMM"></asp:DropDownList>
		<asp:DropDownList runat="server" ID="lstCCYY"></asp:DropDownList></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104277" /></td><td><asp:TextBox runat="server" Width="80px" ID="txtCCCVV"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Button runat="server" id="X104279" />
<asp:Label runat="server" ID="X104281" CssClass="Error"></asp:Label>
<br /><br />
<asp:Label runat="server" ID="X104358" CssClass="Error"></asp:Label>
</div>

<div id="div05">
<asp:Label runat="server" ID="X104290" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104294"></asp:Literal>
<br /><br />
<asp:DropDownList runat="server" ID="lstOption"></asp:DropDownList>
<br /><br />
<asp:Literal runat="server" ID="X104297"></asp:Literal>
<br /><br />
<asp:DropDownList runat="server" ID="lstReason"></asp:DropDownList>
<br /><br />
<asp:Button runat="server" id="X104299" />
<br /><br />
<asp:Label runat="server" ID="X104301" CssClass="Error"></asp:Label>
</div>

</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>