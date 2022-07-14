<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgPayOutstandingFees.aspx.cs" Inherits="PCIWebFinAid.pgPayOutstandingFees" %>
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
function Validate()
{
	try
	{
		DisableElt('X104888',true);
		var img = GetElt('imgValidate');
		var amt = GetEltValue('txtAmt');
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		if ( amt.length < 1 )
			img.src = '';
		else if ( isNaN(amt) )
			img.src = dir + 'Cross.png';
		else
		{
			img.src = dir + 'Tick.png';
			DisableElt('X104888',false);
		}
	}
	catch (x)
	{ }
	return false;
}
</script>

<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104390">104390</asp:Literal>
</div>

<div class="DataLabel" style="width:100%">
<asp:Literal runat="server" ID="X104152">104152</asp:Literal>&nbsp;
<asp:Label runat="server" CssClass="DataStatic" ID="lblCurr">Curr Symbol</asp:Label>
<asp:Label runat="server" CssClass="DataStatic" ID="lblBalance">Outstanding Amount</asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104999">104999</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAmt" CssClass="DataInput" OnKeyUp="JavaScript:Validate()"></asp:TextBox>&nbsp;<img id="imgValidate" />
</div>

<br />
<asp:Button runat="server" id="X104888" Text="104888" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104777"  CssClass="Info">104777</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
Validate();
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>