<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LFinnHub.aspx.cs" Inherits="PCIWebFinAid.LFinnHub" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>FinnHub Financial Data</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<asp:Literal runat="server" ID="lblRefresh"></asp:Literal>
</head>
<body>
<script type="text/javascript">
function ChooseTicker(p)
{
	DisableElt('txtStock'   ,(p!=21));
	DisableElt('txtBase'    ,(p!=23));
	DisableElt('txtForex'   ,(p!=29));
	DisableElt('lstCurr'    ,(p!=29));
	DisableElt('lstExchange',(p!=29));
	if ( p == 21 )
		GetElt('txtStock').focus();
	else if ( p == 23 )
		GetElt('txtBase').focus();
	else if ( p == 29 )
		GetElt('txtForex').focus();
}
</script>
<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />
<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;padding:10px;white-space:nowrap">
<asp:Label runat="server" ID="X104002" Font-Bold="true" Font-Size="24px">Admin</asp:Label>
<br /><br /><br />
<asp:Label runat="server" ID="X104004" Font-Bold="true">Prosperian Capital</asp:Label><br />
4 Cybercity<br />
4’th Floor<br />
Ebène Heights<br />
Ebène<br />
Mauritius<br />
72201
<br /><br /><br />
<asp:Literal runat="server" ID="lblDate"></asp:Literal>
</td>
<td style="vertical-align:top;padding-left:10px">

<div class="Header4">FinnHub Financial Tickers</div>
<br />

<table>
<tr>
	<td>FinnHub API Key</td>
	<td colspan="2">
		<asp:TextBox runat="server" ID="txtKey" Width="200px" value="bqvq9gnrh5rapls47e8g"></asp:TextBox></td></tr>
<tr>
	<td>Refresh every</td>
	<td colspan="2"><asp:TextBox runat="server" ID="txtRefresh" Width="30px" value="10"></asp:TextBox> seconds</td></tr>
<tr>
	<td>Status</td>
	<td colspan="2"><b><asp:Label runat="server" ID="lblStatus"></asp:Label></b></td></tr>
<tr>
	<td colspan="3"><hr /></td></tr>

<tr>
	<td>
		<asp:RadioButton runat="server" id="rdoTick21" GroupName="rdoTick" onclick="JavaScript:ChooseTicker(21)" />Stock values</td>
	<td>
		Stock symbol(s)<br />
		Separated by commas<br />
		Eg. AAPL,MSFT,GOOGL</td>
	<td>
		<asp:TextBox runat="server" ID="txtStock" Width="200px" TextMode="MultiLine" Rows="4"></asp:TextBox></td></tr>
<tr>
	<td colspan="3"><hr /></td></tr>

<tr>
	<td rowspan="3">
		<asp:RadioButton runat="server" id="rdoTick29" GroupName="rdoTick" onclick="JavaScript:ChooseTicker(29)" Enabled="false" />Currency exchange candles</td>
	<td>
		Exchange</td>
	<td>
		<asp:DropDownList runat="server" ID="lstExchange"></asp:DropDownList></td></tr>
<tr>
	<td>
		Base currency</td>
	<td>
		<asp:DropDownList runat="server" ID="lstCurr">
			<asp:ListItem Value="ZAR" Text="(ZAR) SA Rands"></asp:ListItem>
			<asp:ListItem Value="EUR" Text="(EUR) Euros"></asp:ListItem>
			<asp:ListItem Value="GBP" Text="(GBP) Pounds Sterling"></asp:ListItem>
			<asp:ListItem Value="USD" Text="(USD) US Dollars"></asp:ListItem>
			<asp:ListItem Value="CHF" Text="(CHF) Swiss Francs"></asp:ListItem>
			<asp:ListItem Value="AUD" Text="(AUD) Australian Dollar"></asp:ListItem>
			<asp:ListItem Value="THB" Text="(THB) Thai Baht"></asp:ListItem>
			<asp:ListItem Value="JPY" Text="(JPY) Japanese Yen"></asp:ListItem>
			<asp:ListItem Value="CNY" Text="(CNY) Chinese Yuan (Renminbi)"></asp:ListItem>
		</asp:DropDownList></td></tr>
<tr>
	<td>
		Currency symbol(s)<br />
		Separated by commas<br />
		Eg. GBP,USD,EUR,ZAR</td>
	<td>
		<asp:TextBox runat="server" ID="txtForex" Width="200px" TextMode="MultiLine" Rows="4"></asp:TextBox></td></tr>
<tr>
	<td colspan="3"><hr /></td></tr>

<tr>
	<td>
		<asp:RadioButton runat="server" id="rdoTick23" GroupName="rdoTick" onclick="JavaScript:ChooseTicker(23)" />Currency exchange rates</td>
	<td>
		Base currency (eg. USD,THB,ZAR)</td>
	<td>
		<asp:TextBox runat="server" ID="txtBase" MaxLength="3" Width="35px"></asp:TextBox></td></tr>

<tr>
	<td colspan="3"><br />
		<asp:Button runat="server" ID="btnStart" Text="Start" OnClick="btnStart_Click" OnClientClick="JavaScript:SetEltValue('lblStatus','<span style=\'color:orange\'>Starting</span>');return true" />
		<asp:Button runat="server" ID="btnStop"  Text="Stop"  OnClick="btnStop_Click" />
		<asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_Click" /></td></tr>
<tr>
	<td colspan="3" class="Error"><asp:Label ID="lblErr" runat="server"></asp:Label></td></tr>
</table>

</td><td style="vertical-align:top;padding-left:10px;border:1px solid #000000">
	<asp:Literal runat="server" id="lblData"></asp:Literal>
</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>