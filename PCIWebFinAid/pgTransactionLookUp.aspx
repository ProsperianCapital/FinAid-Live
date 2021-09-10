<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgTransactionLookUp.aspx.cs" Inherits="PCIWebFinAid.pgTransactionLookUp" ValidateRequest="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->

<form id="frmLookup" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	Transaction Lookup
	</div>
	<p>
	Card Number (first 6 and last 4 digits only)<br />
	<asp:TextBox runat="server" ID="txtCard1" Width="54px" MaxLength="6"></asp:TextBox>
	<asp:TextBox runat="server" ID="txtCard2" Width="54px" ReadOnly="true" TabIndex="99">******</asp:TextBox>
	<asp:TextBox runat="server" ID="txtCard3" Width="36px" MaxLength="4"></asp:TextBox>
	</p><p>
	From Date<br />
	<asp:TextBox runat="server" ID="txtDate1" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
	<a href="JavaScript:showCalendar(frmLookup.txtDate1)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
	</p><p>
	To Date<br />
	<asp:TextBox runat="server" ID="txtDate2" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
	<a href="JavaScript:showCalendar(frmLookup.txtDate2)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
	</p><p>
	<asp:Button runat="server" ID="btnSearch" Text="Search" OnClientClick="JavaScript:ShowBusy('Searching ... Please be patient',null,0)" OnClick="btnSearch_Click" />&nbsp;
	</p>

	<asp:Literal runat="server" ID="lblTransactions"></asp:Literal>
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>