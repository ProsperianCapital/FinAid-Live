<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LWelcome.aspx.cs" Inherits="PCIWebFinAid.LWelcome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Welcome</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>
<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />
<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;padding:10px;white-space:nowrap">
<asp:Label   runat="server" ID="X104002" Font-Bold="true" Font-Size="24px"></asp:Label><br /><br />
<asp:Label   runat="server" ID="X104004" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblAddress"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104006" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblEMail"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104008" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblCellNo"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104010" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblOption"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104012" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblFee"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104014" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblCredit"></asp:Literal><br /><br />
<asp:Label   runat="server" ID="X104016" Font-Bold="true"></asp:Label><br />
<asp:Literal runat="server" ID="lblDueDate"></asp:Literal><br />
<asp:Literal runat="server" ID="lblDate"></asp:Literal>
</td>
<td>
<asp:Label runat="server" ID="X104018" cssclass="Header4">Welcome to Log On</asp:Label>
<br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104019"></asp:Literal></td>
	<td><asp:Literal runat="server" ID="lblUserName"></asp:Literal></td></tr>
<tr>
	<td></td>
	<td><asp:Literal runat="server" ID="lblLastLogon"></asp:Literal></td></tr>
</table>
<asp:Label runat="server" ID="X104023"></asp:Label>
<br /><br />
<asp:Table runat="server" ID="tblHistory" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104367" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104368" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br />
<asp:Label runat="server" ID="X104027" CssClass="Error"></asp:Label>
</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>