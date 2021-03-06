﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="TransLookup.aspx.cs" Inherits="PCIWebFinAid.TransLookup" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>Transaction Lookup</title>
	<link rel="stylesheet" href="CSS/FinAid.css?v=6" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<form id="frmLookup" runat="server">

<script type="text/javascript" src="JS/Calendar.js"></script>

<div class="Header3">
	Transaction Lookup
</div>

<!--
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead1Label">Transaction Lookup</asp:Literal>
</p>
-->
<p>
Card Number (first 6 and last 4 digits only)<br />
<asp:TextBox runat="server" ID="txtCard1" Width="54px" MaxLength="6"></asp:TextBox>
<asp:TextBox runat="server" ID="txtCard2" Width="54px" ReadOnly="true" TabIndex="99">******</asp:TextBox>
<asp:TextBox runat="server" ID="txtCard3" Width="36px" MaxLength="4"></asp:TextBox>
</p><p>
From Date<br />
<asp:TextBox runat="server" ID="txtDate1" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
<a href="JavaScript:showCalendar(frmLookup.txtDate1)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
</p><p>
To Date<br />
<asp:TextBox runat="server" ID="txtDate2" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
<a href="JavaScript:showCalendar(frmLookup.txtDate2)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
</p><p>
<asp:Button runat="server" ID="btnSearch" text="Search" OnClick="btnSearch_Click" />
<asp:Button runat="server" ID="btnError" Text="Error ...?" OnClientClick="JavaScript:ShowElt('lblErrorDtl',true);return false" />
</p>
<asp:Literal runat="server" ID="lblTransactions"></asp:Literal>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
<asp:Label runat="server" ID="lblErrorDtl" style="border:1px solid #000000;position:fixed;bottom:20px;right:5px;visibility:hidden;display:none;padding:5px;font-family:Verdana;background-color:pink"></asp:Label>

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>
</body>
</html>