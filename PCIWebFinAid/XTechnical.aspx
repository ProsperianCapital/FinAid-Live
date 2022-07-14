<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XTechnical.aspx.cs" Inherits="PCIWebFinAid.XTechnical" ValidateRequest="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<script type="text/javascript">
function ShowData(err)
{
	try
	{
		var show = true;
		var p    = GetElt('txtPwd');
		var k    = GetListValue('lstAction');
		var w    = "960px";
		if ( k == 102 )
		{
			SetEltValue('tdData','SQL object');
			w = "400px";
		}
		else if ( k == 103 )
			SetEltValue('tdData','SQL statement');
		else if ( k == 7 || k == 8 )
		{
			SetEltValue('tdData','Log date (yyyy-mm-dd). Default is TODAY');
			w = "100px";
		}
		else if ( k == 111 )
			SetEltValue('tdData','To email address(es)');
		else if ( k == 109 || k == 110 )
			SetEltValue('tdData','Message');
		else if ( k == 13 )
			SetEltValue('tdData','URL');
		else
			show = false;

		ShowElt('txtPwd',(k>99),8);
		ShowElt('spnPwd',(k<99),8);
		ShowElt('trData',show,8);

		if (show)
		{
			var d = GetElt('txtData');
			d.style.width = w;
			if ( err == 0 )
				SetEltValue(d,'');
			d.focus();
		}
		else if ( k > 99 )
			p.focus();
		if ( err == 9 ) // Password err
			p.focus();
	}
	catch (x)
	{ }
}
</script>
<ascx:XHeader runat="server" ID="ascxXHeader" />

<form id="frmHome" runat="server">

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	Technical Diagnostics
	</div>

	<asp:HiddenField runat="server" ID="hdnBrowser" />

	<table style="width:800px">
		<tr>
			<td>What would you like to do?</td>
			<td>
				<asp:DropDownList runat="server" ID="lstAction" Width="400" onchange="JavaScript:ShowData(0)">
					<asp:ListItem Value= "00" Text="(Select one)"></asp:ListItem>
					<asp:ListItem Value= "01" Text="Check SQL database status"></asp:ListItem>
					<asp:ListItem Value="102" Text="View SQL object properties"></asp:ListItem>
					<asp:ListItem Value="103" Text="Execute SQL"></asp:ListItem>
					<asp:ListItem Value= "04" Text="View ASP.NET setup (eg. Server.MapPath)"></asp:ListItem>
					<asp:ListItem Value= "05" Text="View hardware/software versions & setup"></asp:ListItem>
					<asp:ListItem Value="106" Text="View application configuration settings"></asp:ListItem>
					<asp:ListItem Value= "07" Text="View error log"></asp:ListItem>
					<asp:ListItem Value= "08" Text="View information log"></asp:ListItem>
					<asp:ListItem Value="109" Text="Write to error log"></asp:ListItem>
					<asp:ListItem Value="110" Text="Write to information log"></asp:ListItem>
					<asp:ListItem Value="111" Text="Send test email"></asp:ListItem>
					<asp:ListItem Value= "12" Text="View client computer details"></asp:ListItem>
					<asp:ListItem Value= "13" Text="View server certificate"></asp:ListItem>
					<asp:ListItem Value= "14" Text="View all IIS server variables"></asp:ListItem>
				</asp:DropDownList></td></tr>
		<tr id="trData" style="visibility:hidden;display:none">
			<td id="tdData"></td>
			<td><asp:TextBox runat="server" ID="txtData" Width="800"></asp:TextBox></td></tr>
		<tr>
			<td>System password</td>
			<td>
				<asp:TextBox runat="server" ID="txtPwd" TextMode="Password" style="visibility:hidden;display:none"></asp:TextBox>
				<span id="spnPwd"><b>Not required</b></span></td></tr>
	</table>

	<hr />

	<div class="ButtonBox">
		<asp:Button runat="server" ID="btnOK" Text="OK" OnClick="btnOK_Click" />&nbsp;
	</div>
	<p class="error">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	<asp:Literal runat="server" ID="lblResult"></asp:Literal>

	<script type="text/javascript">
	SetEltValue('hdnBrowser',navigator.userAgent.toString());
	</script>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>