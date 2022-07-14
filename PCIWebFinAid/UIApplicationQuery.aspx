<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="UIApplicationQuery.aspx.cs" Inherits="PCIWebFinAid.UIApplicationQuery" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html>
<head runat="server">
	<title>Prosperian API</title>
	<link rel="stylesheet" href="CSS/BackOffice.css" type="text/css" />
	<!--#include file="IncludeMainSimple.htm" -->
</head>
<body>
<form id="frmUI" runat="server">
<ascx:XHeader runat="server" ID="ascxXHeader" />
<br />
<p style="color:red;font-size:16px">
Oops, something seems to have gone badly wrong. Please click the link below to continue.
</p>
<br />
<p>
<a href="pgLogon.aspx">Prosperian BackOffice Login</a>
</p>
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>