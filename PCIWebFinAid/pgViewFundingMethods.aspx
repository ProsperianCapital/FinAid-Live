<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewFundingMethods.aspx.cs" Inherits="PCIWebFinAid.pgViewFundingMethods" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->

<script type="text/javascript">
function Validate(eltID,len,eltType)
{
	try
	{
		var img = GetElt('img'+eltID);
		var val = GetEltValue('txt'+eltID);
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		var ok  = true;
		if ( val.length < len )
			ok = false;
		else if ( eltType == 1 ) // Card number
			ok = ValidCardNumber(val);
		else if ( eltType == 2 ) // CVV
			ok = ValidPIN(val,len);
		if (ok)
			img.src = dir + 'Tick.png';
		else
		{
			img.src = dir + 'Cross.png';
			DisableElt('X104279',true);
		}
		return ok;
	}
	catch (x)
	{ }
	return false;
}
</script>

<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104268">104268</asp:Literal>
</div>

<div class="DataLabel" style="width:100%">
<div>
<asp:Literal runat="server" ID="X104269">104269</asp:Literal><br />
<asp:Label   runat="server" ID="lblName" CssClass="DataStatic">Name</asp:Label>
</div><div>
<asp:Literal runat="server" ID="X104273">X104273</asp:Literal><br />
<asp:Label   runat="server" ID="lblNumber" CssClass="DataStatic">Number</asp:Label>
</div><div>
<asp:Literal runat="server" ID="X104275">X104275</asp:Literal><br />
<asp:Label   runat="server" ID="lblDate" CssClass="DataStatic">MM / YYYY</asp:Label>
</div><div>
<asp:Literal runat="server" ID="X104277">X104277</asp:Literal><br />
<asp:Label   runat="server" ID="lblCVV" CssClass="DataStatic">CVV</asp:Label>
</div>
</div>

<hr />

<div class="DataLabel" style="width:100%">
<div>
	<asp:Literal runat="server" ID="Y104269">104269</asp:Literal><br />
	<asp:TextBox runat="server" ID="txtName" CssClass="DataInput" OnKeyUp="JavaScript:Validate('Name',3,0)"></asp:TextBox>&nbsp;<img id="imgName" />
</div><div>
	<asp:Literal runat="server" ID="Y104273">104273</asp:Literal><br />
	<asp:TextBox runat="server" ID="txtNumber" CssClass="DataInput" OnKeyUp="JavaScript:Validate('Number',14,1)" MaxLength="20"></asp:TextBox>&nbsp;<img id="imgNumber" />
</div><div>
	<asp:Literal runat="server" ID="Y104275">104275</asp:Literal><br />
	<asp:DropDownList runat="server" ID="lstMM" CssClass="DataInput"></asp:DropDownList>
	<asp:DropDownList runat="server" ID="lstYY" CssClass="DataInput"></asp:DropDownList>
</div><div>
	<asp:Literal runat="server" ID="Y104277">104277</asp:Literal><br />
	<asp:TextBox runat="server" ID="txtCVV" CssClass="DataInput" OnKeyUp="JavaScript:Validate('CVV',3,2)" MaxLength="4"></asp:TextBox>&nbsp;<img id="imgCVV" />
</div>
</div>

<br />
<asp:Button runat="server" id="X104279" Text="104279" OnClick="btnOK_Click" />&nbsp;
<asp:Label runat="server" ID="X104281"  CssClass="Note">104281</asp:Label>
<br /><br />
<asp:Label runat="server" ID="X104358"  CssClass="Info">104358</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
if ( Validate('Name',3,0) && Validate('Number',14,1) && Validate('CVV',3,2) )
	DisableElt('X104279',false);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>