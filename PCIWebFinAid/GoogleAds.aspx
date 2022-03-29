<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="GoogleAds.aspx.cs" Inherits="PCIWebFinAid.GoogleAds" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Prosperian : GoogleAds API</title>
	<link rel="stylesheet" href="CSS/BackOffice.css" type="text/css" />
	<!--#include file="IncludeMainSimple.htm" -->
</head>
<body>
<script type="text/javascript">
function SetType()
{
	ShowElt('txtJSON',GetElt('rdoJSON').checked);
	ShowElt('txtXML' ,GetElt('rdoXML').checked);
	ShowElt('txtWeb' ,GetElt('rdoWeb').checked);
	ShowElt('txtForm',GetElt('rdoForm').checked);
}
</script>

<form id="frmHome" runat="server">

	<asp:Label runat="server" ID="lblVer" style="border:1px solid #000000;padding:10px;float:right;background-color:lightgreen"></asp:Label>

	<div class="Header3">
	Prosperian Google Ads API
	</div>

	Developer token<br />
	<asp:TextBox runat="server" ID="txtToken" Width="800px"></asp:TextBox>
	<br /><br />
	Client id<br />
	<asp:TextBox runat="server" ID="txtClient" Width="800px"></asp:TextBox>
	<br /><br />
	Client secret<br />
	<asp:TextBox runat="server" ID="txtSecret" Width="800px"></asp:TextBox>
	<br /><br />
	URL<br />
	<asp:TextBox runat="server" ID="txtURL" Width="800px"></asp:TextBox>

	<p class="ButtonBox">
		<asp:Button runat="server" ID="btnOK" Text="Go" OnClick="btnOK_Click" />&nbsp;
	</p>

	<hr />

	<p class="error">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	Output data<br />
	<asp:TextBox runat="server" ID="txtOut" TextMode="MultiLine" Height="140" Width="800px" Rows="8" ReadOnly="true"></asp:TextBox>
</form>

<script type="text/javascript">
SetType();
</script>
</body>
</html>