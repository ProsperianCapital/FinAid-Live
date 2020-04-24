<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ContractLookup.aspx.cs" Inherits="PCIWebFinAid.ContractLookup" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>Contract Lookup</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<form id="frmLookup" runat="server">

<script type="text/javascript" src="JS/Calendar.js"></script>

<div class="Header3">
	Contract Lookup
</div>

<!--
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead1Label">Contract Lookup</asp:Literal>
</p>
-->
<p>
Contract Code<br />
<asp:TextBox runat="server" ID="txtContractCode"></asp:TextBox>
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