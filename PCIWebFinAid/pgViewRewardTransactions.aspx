<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewRewardTransactions.aspx.cs" Inherits="PCIWebFinAid.pgViewRewardTransactions" %>
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
<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104505">104505</asp:Literal>
</div>

<asp:Table runat="server" ID="tblData" style="border:1px solid #000000;white-space:initial">
<asp:TableRow CssClass="tRowHead">
	<asp:TableCell ID="X104506">104506</asp:TableCell>
	<asp:TableCell ID="X104507">104507</asp:TableCell>
	<asp:TableCell ID="X104508">104508</asp:TableCell>
	<asp:TableCell ID="X104509">104509</asp:TableCell>
	<asp:TableCell ID="X104510">104510</asp:TableCell>
	<asp:TableCell ID="X104511">104511</asp:TableCell></asp:TableRow>
</asp:Table>
<br />
<asp:Label runat="server" ID="X104050" CssClass="Info">104050</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>