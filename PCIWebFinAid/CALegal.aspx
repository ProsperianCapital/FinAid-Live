<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CALegal.aspx.cs" Inherits="PCIWebFinAid.CALegal" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<title><asp:Literal runat="server" ID="X100003">100003</asp:Literal></title>
</head>
<body>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />
	<asp:HiddenField runat="server" ID="hdnDocTypeCode" />

	<br />&nbsp;
	<div style="background-color:#898787;width:100%;padding:0px;padding-top:12px;padding-bottom:15px;font-size:32px">
		<div style="margin:20px">
		<asp:Literal runat="server" ID="xTitle"></asp:Literal>
		</div>
	</div>
	<div style="color:#E68A00;font-size:17px;font-weight:800;margin:20px">
		<asp:Literal runat="server" ID="xHeader"></asp:Literal>
	</div>
	<div style="font-size:14px;margin:20px">
		<asp:Literal runat="server" ID="xText"></asp:Literal>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
</form>
<br />
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
