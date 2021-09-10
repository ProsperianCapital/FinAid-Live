<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgVieweWallets.aspx.cs" Inherits="PCIWebFinAid.pgVieweWallets" EnableEventValidation="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<script type="text/javascript">
function EditMode(editInsert)
{
//	editInsert values:
//	0 : Hide
//	1 : Show, edit
//	2 : Show, insert
	HidePopups();
	var g = '&nbsp;<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" style="float:right" title="Close" onclick="JavaScript:EditMode(0)" />';
	if ( editInsert == 1 )
		SetEltValue('lblEditHead','Edit eWallet'+g);
	else
		SetEltValue('lblEditHead','Create eWallet'+g);
	ShowElt('pnlEdit',(editInsert> 0));
	SetEltValue('hdnEditInsert',editInsert);
}
function HidePopups()
{
}
</script>
<form id="frmMain" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	My eWallets
	</div>

	<style>
	.CardMenuBox
	{
		display: flex;
		flex-wrap: nowrap;
		justify-content: space-between;
		margin-left: 8px;
		margin-right: 0px;
		margin-top: 2px;
		margin-bottom: 4px;
	}
	.CardMenuItem
	{
		color: black;
		padding: 0px;
		width: 31%;
		padding-top: 2px;
		font-size: 13px;
	}
	.CardMenuSep
	{
		color: black;
		padding: 0px;
		width: 3%;
		font-size: 16px;
	}
	.CardFlag
	{
		height: 40px;
		width: 60px;
	}
	.Wallet1
	{
		font-family: atlanta;
		font-size: 9px;
		letter-spacing: 0.1em;
	}
	.Wallet2
	{
		font-family: atlanta;
		font-size: 16px;
		letter-spacing: 0.1em;
	}
	.Wallet3
	{
		font-family: atlanta;
		font-size: 20px;
		letter-spacing: 0.1em;
	}
	.Wallet4
	{
		font-family: atlanta;
		font-size: 24px;
		letter-spacing: 0.1em;
	}
	</style>

	<!-- Card 1 (start) -->
	<div style="width:350px;display:inline-block">
	<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:8px;width:338px;height:188px;border-radius:10px">
		<div style="display:inline-flex;justify-content:space-between;width:100%">
			<div>
				<img src="Images/Flag-USA.png" title="US Dollars" class="CardFlag" /></div>
			<div>
				<br />
				<span class="Wallet3">USD</span></div>
			<div>
				Balance<br />
				<span class="Wallet3">71,463.12</span></div>
			<div style="float:right;font-size:13px">
				<img src="Images/PayPayYa.png" style="width:45px" /><br />
				eWallet</div>
		</div>
		Account Number<br />
		<span class="Wallet4">1234 5678 9012 3456</span>
		<p>
		Account Description<br />
		<span class="Wallet2">My VISA USD eWallet</span>
		<span style="float:right"><img style="width:60px" src="Images/LogoVisa.jpg" title="VISA" />&nbsp;</span>
		</p><p>
		Account Name<br />
		<span class="Wallet2">Samual Briggs</span>
		</p>
	</div>

	<div class="CardMenuBox">
		<div class="CardMenuItem"><a href="#">New eWallet</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="JavaScript:EditMode(1)">Edit Description</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="#">Get Bank Details</a></div>
	</div>
	<div class="CardMenuBox">
		<div class="CardMenuItem"><a class="CardMenu" href="#">TopUp Now</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Auto TopUp</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Transactions</a></div>
	</div>

	</div>
	<!-- Card 1 (end) -->

	<br /><br />

	<!-- Card 2 (start) -->
	<div style="width:350px;display:inline-block">
	<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:8px;width:338px;height:188px;border-radius:10px">
		<div style="display:inline-flex;justify-content:space-between;width:100%">
			<div>
				<img src="Images/Flag-EUR.png" title="Euros" class="CardFlag" /></div>
			<div>
				<br />
				<span class="Wallet3">EUR</span></div>
			<div>
				Balance<br />
				<span class="Wallet3">1,433.29</span></div>
			<div style="float:right;font-size:13px">
				<img src="Images/PayPayYa.png" style="width:45px" /><br />
				eWallet</div>
		</div>
		Account Number<br />
		<span class="Wallet4">6702 3853 1066 4287</span>
		<p>
		Account Description<br />
		<span class="Wallet2">My PayPayYa EUR eWallet</span>
		<span style="float:right"><img style="width:50px" src="Images/LogoPlaNet.png" title="PayPayYa" />&nbsp;</span>
		</p><p>
		Account Name<br />
		<span class="Wallet2">Samual Briggs</span>
		</p>
	</div>

	<div class="CardMenuBox">
		<div class="CardMenuItem"><a href="#">New eWallet</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="JavaScript:EditMode(1)">Edit Description</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="#">Get Bank Details</a></div>
	</div>
	<div class="CardMenuBox">
		<div class="CardMenuItem"><a class="CardMenu" href="#">TopUp Now</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Auto TopUp</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Transactions</a></div>
	</div>

	</div>
	<!-- Card 2 (end) -->

	<!--
	<hr />

	<p style="color:red;font-weight:bold">OTHER TESTING</p>

	<hr />

	<div style="width:350px;display:inline-block">
	<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:5px;width:338px;height:188px;border-radius:10px">
		<table style="width:100%">
		<tr>
		<td>
			<img src="Images/Flag-USA.png" title="US Dollars" class="CardFlag" />
		</td>
		<td>
			<br />
			<span class="Wallet3">USD</span></td>
		<td>
			Balance<br />
			<span class="Wallet3">38,901.68</span></td>
		<td style="float:right;font-size:14px">
			<img src="Images/PayPayYa.png" style="width:50px" /><br />
			eWallet</td>
		</tr>
		</table>
		Account Number<br />
		<span class="Wallet4">1234 5678 9012 3456</span>
		<p>
			Account Description<br />
			<span class="Wallet2">My VISA USD eWallet</span><br />
		<span style="float:right"><img style="width:60px" src="Images/LogoVisa.jpg" title="VISA" />&nbsp;</span>
		</p><p>
			Account Name<br />
			<span class="Wallet2">Samual Briggs</span>
		</p>
	</div>

	<div class="CardMenuBox">
		<div class="CardMenuItem"><a href="#">New eWallet</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="JavaScript:EditMode(1)">Edit Description</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="#">Get Bank Details</a></div>
	</div>
	<div class="CardMenuBox">
		<div class="CardMenuItem"><a class="CardMenu" href="#">TopUp Now</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Auto TopUp</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Transactions</a></div>
	</div>

	</div>

	<br /><br />

	<div style="width:350px;display:inline-block">
		<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:10px;width:348px;height:200px;border-radius:10px">
			<table style="width:100%">
			<tr>
			<td>
				<img src="Images/Flag-EUR.png" title="Euros" class="CardFlag" /></td>
			<td>
				<br />
				<span class="Wallet3">EUR</span></td>
			<td>
				Balance<br />
				<span class="Wallet3">8,013.76</span></td>
			<td style="float:right;font-size:14px">
				<img src="Images/PayPayYa.png" style="width:50px" /><br />
				eWallet</td>
			</tr>
			</table>
			Account Number<br />
			<span class="Wallet4">8045 6723 0198 3755</span>
			<p>
			Account Description<br />
			<span class="Wallet2">My PayPayYa EUR eWallet</span>
			<span style="float:right"><img style="width:50px" src="Images/LogoPlaNet.png" title="PayPayYa" />&nbsp;</span>
			</p><p>
			Account Name<br />
			<span class="Wallet2">Samual Briggs</span>
			</p>
		</div>

	<div class="CardMenuBox">
		<div class="CardMenuItem"><a href="#">New<br />eWallet</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="JavaScript:EditMode(1)">Edit<br />Description</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a href="#">Get Bank<br />Details</a></div>
	</div>
	<div class="CardMenuBox">
		<div class="CardMenuItem"><a class="CardMenu" href="#">TopUp<br />Now</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Auto<br />TopUp</a></div>
		<div class="CardMenuSep">|</div>
		<div class="CardMenuItem"><a class="CardMenu" href="#">Trans-<br />actions</a></div>
	</div>

	</div>

	-->

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<div class="Popup2" id="pnlEdit" style="visibility:hidden;display:none">
	<asp:HiddenField runat="server" ID="hdnEditInsert" />
	<div class="Header6" id="lblEditHead"></div>
	<p>
	Wallet Account Number<br /><asp:TextBox runat="server" ID="txtAccNo" Width="200px" ReadOnly="true">1234 5678 9012 3456</asp:TextBox>
	</p><p>
	Wallet Currency<br />
	<asp:DropDownList runat="server" ID="lstCurrency" style="width:200px">
		<asp:ListItem Value="AUD" Text="Australian Dollar"></asp:ListItem>
		<asp:ListItem Value="NZD" Text="New Zealand Dollar"></asp:ListItem>
		<asp:ListItem Value="USD" Text="US Dollar"></asp:ListItem>
		<asp:ListItem Value="EUR" Text="Euro"></asp:ListItem>
		<asp:ListItem Value="GBP" Text="British Pound"></asp:ListItem>
		<asp:ListItem Value="SGD" Text="Singapore Dollar"></asp:ListItem>
	</asp:DropDownList>
	</p><p>
	Wallet Description<br /><asp:TextBox runat="server" ID="txtDescr" Width="400px">My USD eWallet blah</asp:TextBox>
	</p>
	<asp:Button runat="server" ID="btnSave" Text="Save" title="Save your changes" />&nbsp;
	<input type="button" value="Cancel" id="btnCancel" title="Exit without saving" onclick="JavaScript:EditMode(0)" />
	<asp:Label runat="server" id="lblErr2" cssclass="Error"></asp:Label>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>