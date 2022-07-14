<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewDashboardProsperianWealth.aspx.cs" Inherits="PCIWebFinAid.pgViewDashboardProsperianWealth" ValidateRequest="false" %>
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

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	Trading Gateway
	</div>
	<table>
	<tr>
		<td><asp:Image runat="server" ID="img1" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName1"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus1" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart1" ToolTip="1" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop1"  ToolTip="1" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img2" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName2"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus2" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart2" ToolTip="2" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop2"  ToolTip="2" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img3" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName3"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus3" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart3" ToolTip="3" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop3"  ToolTip="3" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img21" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName21"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus21" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart21" ToolTip="21" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop21"  ToolTip="21" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img22" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName22"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus22" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart22" ToolTip="22" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop22"  ToolTip="22" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img23" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName23"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus23" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart23" ToolTip="23" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop23"  ToolTip="23" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	<tr>
		<td><asp:Image runat="server" ID="img24" /></td>
		<td style="vertical-align:middle"><asp:Literal runat="server" ID="lblName24"></asp:Literal></td>
		<td style="vertical-align:middle"><asp:Label   runat="server" ID="lblStatus24" CssClass="Red"></asp:Label></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStart24" ToolTip="24" Text="Start" OnClick="btnStart_Click"></asp:Button></td>
		<td style="vertical-align:middle"><asp:Button  runat="server" ID="btnStop24"  ToolTip="24" Text="Stop" OnClick="btnStop_Click"></asp:Button></td></tr>
	</table>

	<div style="border-top:1px solid #000000">
	<br />
	<asp:Button runat="server" ID="btnRefresh" Text="Refresh" ToolTip="Refresh the page" OnClick="btnRefresh_Click" />&nbsp;
	<asp:Button runat="server" ID="btnStopAll" Text="Stop All" ToolTip="Stop all tickers" OnClick="btnStop_Click" />&nbsp;
	<asp:Button runat="server" ID="btnShutDown" Text="Shut Down" ToolTip="Shut down the application" OnClick="btnShutDown_Click" />&nbsp;&nbsp;
	<asp:Label  runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>