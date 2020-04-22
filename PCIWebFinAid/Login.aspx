<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Login.aspx.cs" Inherits="PCIWebFinAid.Login" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>Login</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<form id="frmLogin" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />
	<div class="Header3" style="margin-top:2px">
	Login
	</div>
	<p style="text-align:center">
	<asp:TextBox runat="server" ID="txtID" placeholder="ID Number"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtPW" placeholder="PIN"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="btnLogin" Text="LOGIN" />
	</p><p style="text-align:center">
	<a href="X">Forgotten PIN?</a>
	</p>

	<!--
	<svg id="Layer_1" x="0" y="0" viewBox="0 0 15 16" xml:space="preserve" aria-hidden="true">
		<title></title>
		<path d="M1.3,16c-0.7,0-1.1-0.3-1.2-0.8c-0.3-0.8,0.5-1.3,0.8-1.5c0.6-0.4,0.9-0.7,1-1c0-0.2-0.1-0.4-0.3-0.7c0,0,0-0.1-0.1-0.1 C0.5,10.6,0,9,0,7.4C0,3.3,3.4,0,7.5,0C11.6,0,15,3.3,15,7.4s-3.4,7.4-7.5,7.4c-0.5,0-1-0.1-1.5-0.2C3.4,15.9,1.5,16,1.5,16 C1.4,16,1.4,16,1.3,16z M3.3,10.9c0.5,0.7,0.7,1.5,0.6,2.2c0,0.1-0.1,0.3-0.1,0.4c0.5-0.2,1-0.4,1.6-0.7c0.2-0.1,0.4-0.2,0.6-0.1 c0,0,0.1,0,0.1,0c0.4,0.1,0.9,0.2,1.4,0.2c3,0,5.5-2.4,5.5-5.4S10.5,2,7.5,2C4.5,2,2,4.4,2,7.4c0,1.2,0.4,2.4,1.2,3.3 C3.2,10.8,3.3,10.8,3.3,10.9z"></path>
	</svg>
	-->

	<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>
