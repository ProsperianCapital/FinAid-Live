<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LGet.aspx.cs" Inherits="PCIWebFinAid.LGet" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Get</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>

<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />

<!--#include file="IncludeLogin1.htm" -->

<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;font-weight:bold;font-size:24px;padding:10px">
<asp:Literal runat="server" ID="X104075"></asp:Literal><br /><br />
<asp:Button runat="server" ToolTip="btn00" id="X104002" style="width:160px;background-color:black" OnClientClick="JavaScript:location.href='LWelcome.aspx';return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn01" id="X104031" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(1);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn02" id="X104032" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(2);return false" />
</td>
<td style="vertical-align:top">

<div id="div01" title="Get Emergency Cash">
<div class="Header4"><asp:Literal runat="server" ID="X104033"></asp:Literal></div>
<br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104034"></asp:Literal></td><td><asp:TextBox runat="server" ID="txtAmount"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104036"></asp:Literal></td><td><asp:DropDownList runat="server" ID="lstPurpose"></asp:DropDownList></td></tr>
</table>
<br />
<asp:CheckBox runat="server" ID="optDire" />   <asp:Literal runat="server" ID="X104039"></asp:Literal><br />
<asp:CheckBox runat="server" ID="optSell" />   <asp:Literal runat="server" ID="X104041"></asp:Literal><br />
<asp:CheckBox runat="server" ID="optCancel" /> <asp:Literal runat="server" ID="X104043"></asp:Literal><br />
<asp:CheckBox runat="server" ID="optIncome" /> <asp:Literal runat="server" ID="X104045"></asp:Literal><br />
<asp:CheckBox runat="server" ID="optAlt" />    <asp:Literal runat="server" ID="X104047"></asp:Literal>
<br /><br />
<asp:Literal runat="server" ID="X104056"></asp:Literal>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104061" /></td><td><asp:TextBox runat="server" Width="320px" ID="txtBankName"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104063" /></td><td><asp:TextBox runat="server" Width="320px" ID="txtAccName"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104067" /></td><td><asp:TextBox runat="server" Width="160px" ID="txtAccNumber"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104069" /></td><td><asp:TextBox runat="server" Width="320px" ID="txtBranchName"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104377" /></td><td><asp:TextBox runat="server" Width="160px" ID="txtBranchCode"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104073" /></td><td><asp:TextBox runat="server" Width="160px" ID="txtSwift"></asp:TextBox></td></tr>
</table>
<br />
<asp:Button runat="server" id="X104048" />
<br /><br />
<asp:Label runat="server" ID="X104050" CssClass="Error"></asp:Label>
</div>

<div id="div02" title="Get Emergency Service">
<div class="Header4"><asp:Literal runat="server" ID="X104079"></asp:Literal></div>
<br />
<table> <!-- style="width:99%" -->
<tr>
	<td style="vertical-align:top;width:20%;white-space:nowrap"><asp:Literal runat="server" ID="X104080" /></td>
	<td style="vertical-align:top;width:10%"><img src="Images/USPIcon1.png" /></td>
	<td style="vertical-align:top"><asp:Literal runat="server" ID="X104082" /></td></tr>
<tr>
	<td style="vertical-align:top;width:20%;white-space:nowrap"><asp:Literal runat="server" ID="X104083" /></td>
	<td style="vertical-align:top;width:10%"><img src="Images/USPIcon2.png" /></td>
	<td style="vertical-align:top"><asp:Literal runat="server" ID="X104085" /></td></tr>
<tr>
	<td style="vertical-align:top;width:20%;white-space:nowrap"><asp:Literal runat="server" ID="X104086" /></td>
	<td style="vertical-align:top;width:10%"><img src="Images/USPIcon3.png" /></td>
	<td style="vertical-align:top"><asp:Literal runat="server" ID="X104088" /></td></tr>
</table>
<br /><br />
<asp:Label runat="server" ID="X104076" CssClass="Error"></asp:Label>
</div>

</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>