<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Login.aspx.cs" Inherits="PCIWebFinAid.Login" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Login</title>
	<link rel="preload" href="CSS/FinAid.css" as="style" />
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>
<form id="frmLogin" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />
	<div class="Header3" style="margin-top:2px">
	<asp:Literal runat="server" ID="X103013"></asp:Literal>
	</div>
	<p style="text-align:center">
	<asp:TextBox runat="server" ID="txtID" placeholder="ID Number"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtPW" placeholder="PIN" TextMode="Password"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="X103016" Text="LOGIN" OnClick="btnLogin_Click" />
	<asp:Button runat="server" ID="btnErrorDtl" Text="ERROR ...?" OnClientClick="JavaScript:ShowElt('lblErrorDtl',true);return false" />
	</p><p style="text-align:center">
	<a href="JavaScript:alert('This functionality is not yet available')"><asp:Literal runat="server" ID="X103018"></asp:Literal></a>
	<br /><br />
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	<asp:Label runat="server" ID="lblErrorDtl" style="border:1px solid #000000;position:fixed;bottom:20px;right:5px;visibility:hidden;display:none;padding:5px;font-family:Verdana;background-color:pink"></asp:Label>
	<asp:Label runat="server" ID="lblVer" style="position:fixed;bottom:3px;right:5px"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
	<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>
