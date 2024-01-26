<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="TransLookup.aspx.cs" Inherits="PCIWebFinAid.TransLookup" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Transaction Lookup</title>
	<link rel="preload" href="CSS/FinAid.css" as="style" />
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>
<form id="frmLookup" runat="server">

<style>
.ActionLink:link
{
	text-decoration: none;
}
.ActionLink:visited
{
	text-decoration: none;
}
.ActionLink:hover
{
	text-decoration: none;
}
.ActionLink:active
{
	text-decoration: none;
}
</style>

<script type="text/javascript" src="JS/Calendar.js"></script>

<script type="text/javascript">
function ActionMenu(mode,tranId)
{
	var mnu = GetElt('divMenu');
	var tId = "0";
	if ( mode > 0 )
	{
		tId     = tranId.toString();
		var td  = GetElt('td'+tId);
		var ctl = td.getBoundingClientRect();
		var scr = document.body.getBoundingClientRect();
		mnu.style.top  = ( ctl.top  - scr.top  + 10 ).toString() + "px";
		mnu.style.left = ( ctl.left - scr.left + 10 ).toString() + "px";
	}
	SetEltValue('hdnTranId',tId);
	ShowElt(mnu,(mode>0));
	ShowElt('divConfirm',false);
	ShowElt('divFinish',false);
}
function RUSure(actionId)
{
	var xData;
	var xLbl;
	var tranId = GetEltValueInt('hdnTranId');
	if ( tranId < 1 || actionId < 1 )
		return;
	tranId = tranId.toString();

	if ( actionId == 1 )
	{
		xData = GetElt('tr'+tranId).innerHTML;
		xLbl  = 'transaction number ';
		SetEltValue('lblLabel1','Create a <b>ChargeBack Alert</b>');
	}
	else if ( actionId == 2 )
	{
		xData = GetElt('tr'+tranId).innerHTML;
		xLbl  = 'transaction number ';
		SetEltValue('lblLabel1','Process a <b>ChargeBack</b>');
	}
	else if ( actionId == 3 )
	{
		xData = GetElt('cc'+tranId).innerHTML;
		xLbl  = 'contract code ';
		SetEltValue('lblLabel1','<b>Cancel a Contract</b>');
	}
	else
		return;

	SetEltValue('hdnActionId',actionId.toString());
	SetEltValue('hdnData',xData);
	SetEltValue('lblLabel2',xLbl+xData);
	ShowElt('divConfirm',true);
}
</script>

<div class="Header3">
	Transaction Lookup
</div>
<asp:HiddenField runat="server" ID="hdnTranId" />
<asp:HiddenField runat="server" ID="hdnActionId" />
<asp:HiddenField runat="server" ID="hdnData" />
<p>
Card Number (first 6 and last 4 digits only)<br />
<asp:TextBox runat="server" ID="txtCard1" Width="54px" MaxLength="6"></asp:TextBox>
<asp:TextBox runat="server" ID="txtCard2" Width="54px" ReadOnly="true" TabIndex="99">******</asp:TextBox>
<asp:TextBox runat="server" ID="txtCard3" Width="36px" MaxLength="4"></asp:TextBox>
</p><p>
From Date<br />
<asp:TextBox runat="server" ID="txtDate1" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
<a href="JavaScript:showCalendar(frmLookup.txtDate1)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
</p><p>
To Date<br />
<asp:TextBox runat="server" ID="txtDate2" Width="80px" MaxLength="10" placeholder="dd/mm/yyyy"></asp:TextBox>
<a href="JavaScript:showCalendar(frmLookup.txtDate2)"><img src="Images/Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a>
</p><p>
<input type="button" value="Home"   onclick="JavaScript:location.href='LAdmin.aspx'" />&nbsp;
<asp:Button runat="server" ID="btnSearch"   Text="Search" OnClick="btnSearch_Click" />&nbsp;
<asp:Button runat="server" ID="btnErrorDtl" Text="Error ...?" OnClientClick="JavaScript:ShowElt('lblErrorDtl',true);return false" />
</p>
<asp:Literal runat="server" ID="lblTransactions"></asp:Literal>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
<asp:Label runat="server" ID="lblErrorDtl" style="border:1px solid #000000;position:fixed;bottom:20px;right:5px;visibility:hidden;display:none;padding:5px;font-family:Verdana;background-color:pink"></asp:Label>

<div id="divMenu" class="PopupBox" style="visibility:hidden;display:none;width:180px">
<a href="JavaScript:RUSure(1)" class="ActionLink" onmouseover="JavaScript:this.style.backgroundColor='aqua'" onmouseout="JavaScript:this.style.backgroundColor='#E68A00'">&nbsp; ChargeBack Alert &nbsp;</a><span style="float:right"><asp:Image runat="server" ID="imgClose" onclick="JavaScript:ActionMenu(0,0)" /></span><br />
<a href="JavaScript:RUSure(2)" class="ActionLink" onmouseover="JavaScript:this.style.backgroundColor='aqua'" onmouseout="JavaScript:this.style.backgroundColor='#E68A00'">&nbsp; ChargeBack &nbsp;</a><br />
<a href="JavaScript:RUSure(3)" class="ActionLink" onmouseover="JavaScript:this.style.backgroundColor='aqua'" onmouseout="JavaScript:this.style.backgroundColor='#E68A00'">&nbsp; Cancel Contract &nbsp;</a>
</div>

<div id="divConfirm" class="PopupBox" style="visibility:hidden;display:none;background-color:aqua;color:black">
<div class="PopupHead">Are you sure?</div>
<div>
<br />
You are about to
<div id="lblLabel1"></div>
for <span id="lblLabel2"></span>.
<br /><br />
Please confirm that you want to do this.
</div>
<hr />
<asp:Button runat="server" ID="btnOK" Text="OK" OnClick="btnOK_Click" />
<input type="button" value="Cancel" onclick="JavaScript:ActionMenu(0,0)" />
</div>

<div id="divFinish" class="PopupBox" style="visibility:hidden;display:none;background-color:aqua;color:black">
<div class="PopupHead">Status ...</div>
<div>
<p>
<asp:Literal runat="server" ID="lblFinish"></asp:Literal>
</p>
<hr />
<input type="button" value="Close" onclick="JavaScript:ActionMenu(0,0)" />
</div>

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>
</body>
</html>