<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LAdmin.aspx.cs" Inherits="PCIWebFinAid.LAdmin" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Admin</title>
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
<asp:Label runat="server" ID="X104002" Font-Bold="true" Font-Size="24px">Welcome</asp:Label>
<br /><br /><br />
<asp:Label runat="server" ID="X104004" Font-Bold="true">Prosperian Capital</asp:Label><br />
4 Cybercity<br />
4’th Floor<br />
Ebène Heights<br />
Ebène<br />
Mauritius<br />
72201
<br /><br /><br />
<asp:Literal runat="server" ID="lblDate"></asp:Literal>
</td>
<td style="vertical-align:top;padding-left:10px">
<br />
<asp:Label runat="server" ID="X104018" cssclass="Header4">Admin</asp:Label>
<br /><br />
<a href="TransLookup.aspx">Transaction Lookup</a>
<br /><br />
<a href="ContractLookup.aspx">Contract Lookup</a>
<br /><br />
<a href="LFinnHub.aspx">FinnHub Financial Tickers</a>
<!--
<br /><br />
<a href="JavaScript:alert('Sorry, this is not yet available')">FinnHub Currency Exchange Rates</a>
-->
</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>