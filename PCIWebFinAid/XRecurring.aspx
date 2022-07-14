<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XRecurring.aspx.cs" Inherits="PCIWebFinAid.XRecurring" %>
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
<script type="text/javascript">
function PaySingle(mode)
{
	if ( mode > 0 )
	{
		ShowElt('divCard',1);
		GetElt('txtFName').focus();
		if ( mode == 33 )
			SetEltValue('lblErr2','');
	}
	else
		ShowElt('divCard',0);
}
</script>
<form runat="server" id="frmRTR">
	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	Recurring Payments
	</div>

	<table style="border:1px solid #000000">
	<tr>
		<td colspan="2" class="Header6">Select Payment Provider and Method</td></tr>
	<tr>
		<td>Payment Provider</td>
		<td>
			<asp:DropDownList runat="server" id="lstProvider" AutoPostBack="true" OnSelectedIndexChanged="lstProvider_Click"></asp:DropDownList></td></tr>
	<tr>
		<td>Process only the top (or "ALL")</td>
		<td><asp:TextBox runat="server" ID="txtRows" Width="50px">ALL</asp:TextBox> row(s)</td></tr>
	<tr>
		<td>Process via</td>
		<td>
			<asp:RadioButton runat="server" GroupName="rdoP" ID="rdoWeb" Text="Synchronous (this web page)" /><br />
			<asp:RadioButton runat="server" GroupName="rdoP" ID="rdoAsynch" Text="Asynchronous (a separate EXE)" /><br />
			<asp:RadioButton runat="server" GroupName="rdoP" ID="rdoCard" onclick="JavaScript:PaySingle(33)" Text="Single card payment" /></td></tr>
	<tr>
		<td colspan="2"><hr style="color:red"/></td></tr>
	<tr>
		<td>Bureau Code</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblBureauCode"></asp:Literal></td></tr>
	<tr>
		<td>Payment URL</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblBureauURL"></asp:Literal></td></tr>
	<tr>
		<td>Prosperian Account/Key</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblMerchantKey"></asp:Literal></td></tr>
	<tr>
		<td>Prosperian User ID</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblMerchantUser"></asp:Literal></td></tr>
	<tr>
		<td>Cards waiting to be tokenized</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblCards"></asp:Literal></td></tr>
	<tr>
		<td>Payments waiting to be processed</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblPayments"></asp:Literal></td></tr>
	<tr>
		<td>Status</td>
		<td class="DataStatic"> : <asp:Literal runat="server" ID="lblBureauStatus"></asp:Literal></td></tr>
	</table>

	<p>
	<asp:Button  runat="server" ID="btnProcess1" Width="155px" OnClientClick="JavaScript:ShowBusy('Getting tokens ... please be patient')" onclick="btnProcess1_Click" Text="Get Tokens" />
	<asp:Button  runat="server" ID="btnProcess2" Width="155px" OnClientClick="JavaScript:ShowBusy('Processing payments ... please be patient')" onclick="btnProcess2_Click" Text="Process Payments" />
	<asp:Button  runat="server" ID="btnProcess3" Width="155px" OnClientClick="JavaScript:ShowBusy('Deleting tokens ... please be patient')" onclick="btnProcess3_Click" Text="Delete Tokens" />
	</p>
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<div id="divCard" class="Popup2" style="visibility:hidden;display:none">
	<div class="PopupHead">Single Card Payment : <asp:Literal runat="server" ID="lblBureauName" /></div>
	<table>
	<tr>
		<td>First Name</td>
		<td colspan="2">
			<asp:TextBox runat="server" id="txtFName" Width="300px"></asp:TextBox></td></tr>
	<tr>
		<td>Last Name</td>
		<td colspan="2">
			<asp:TextBox runat="server" id="txtLName" Width="300px"></asp:TextBox></td></tr>
	<tr>
		<td>EMail</td>
		<td colspan="2">
			<asp:TextBox runat="server" id="txtEMail" Width="300px"></asp:TextBox></td></tr>
	<tr>
		<td>Card Number</td>
		<td colspan="2">
			<asp:TextBox runat="server" id="txtCCNumber" Width="160px" MaxLength="20"></asp:TextBox></td></tr>
	<tr>
		<td>Expiry Date</td>
		<td colspan="2">
			<asp:DropDownList runat="server" id="lstCCMonth">
				<asp:ListItem Value="00" Text="(Select one)"></asp:ListItem>
				<asp:ListItem Value="01" Text="01 (January)"></asp:ListItem>
				<asp:ListItem Value="02" Text="02 (February)"></asp:ListItem>
				<asp:ListItem Value="03" Text="03 (March)"></asp:ListItem>
				<asp:ListItem Value="04" Text="04 (April)"></asp:ListItem>
				<asp:ListItem Value="05" Text="05 (May)"></asp:ListItem>
				<asp:ListItem Value="06" Text="06 (June)"></asp:ListItem>
				<asp:ListItem Value="07" Text="07 (July)"></asp:ListItem>
				<asp:ListItem Value="08" Text="08 (August)"></asp:ListItem>
				<asp:ListItem Value="09" Text="09 (September)"></asp:ListItem>
				<asp:ListItem Value="10" Text="10 (September)"></asp:ListItem>
				<asp:ListItem Value="11" Text="11 (November)"></asp:ListItem>
				<asp:ListItem Value="12" Text="12 (December)"></asp:ListItem>
			</asp:DropDownList>
			<asp:DropDownList runat="server" id="lstCCYear"></asp:DropDownList></td></tr>
	<tr>
		<td>CVV/CVC</td>
		<td>
			<asp:TextBox runat="server" id="txtCCCVV" Width="50px" MaxLength="4"></asp:TextBox></td>
		<td>
			3/4 digit number on back of card</td></tr>
	<tr>
		<td>Currency</td>
		<td>
			<asp:TextBox runat="server" id="txtCurrency" Width="50px" MaxLength="3"></asp:TextBox></td>
		<td>
			Currency code as per the provider's<br />
			requirements (eg. ZAR, USD, GBP)</td></tr>
	<tr>
		<td>Amount</td>
		<td>
			<asp:TextBox runat="server" id="txtAmount" Width="75px"></asp:TextBox></td>
		<td>
			Cents, so ZAR 987 means R9.87</td></tr>
	<tr>
		<td>Prosperian Reference</td>
		<td colspan="2">
			<asp:TextBox runat="server" id="txtReference" Width="300px"></asp:TextBox></td></tr>
	</table>
	<hr />
	<asp:Button runat="server" ID="btnPay" Text="Pay" OnClick="btnPay_Click" />
	<input type="button" value="Cancel" onclick="JavaScript:PaySingle(0)" />
	<asp:Label runat="server" ID="lblErr2" CssClass="Error"></asp:Label>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>