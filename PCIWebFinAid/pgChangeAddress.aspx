<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeAddress.aspx.cs" Inherits="PCIWebFinAid.pgChangeAddress" %>
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
<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<script type="text/javascript">
function Validate(n)
{
	try
	{
		if ( n > 0 )
		{
			var img = GetElt('img'+n.toString());
			var adr = GetEltValue('txtLine'+n.toString());
			var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
			if ( adr.length < 3 && ( adr.length > 0 || n < 3 ) )
				img.src = dir + 'Cross.png';
			else
				img.src = dir + 'Tick.png';
		}
		var ok = ( GetEltValue('txtLine1').length > 2 &&
		           GetEltValue('txtLine2').length > 2 &&
		         ( GetEltValue('txtLine3').length > 2 || GetEltValue('txtLine3').length == 0 ) &&
		         ( GetEltValue('txtLine4').length > 2 || GetEltValue('txtLine4').length == 0 ) &&
		         ( GetEltValue('txtLine5').length > 2 || GetEltValue('txtLine5').length == 0 ) );
		DisableElt('X104237',!ok);
		return ok;
	}
	catch (x)
	{ }
	return false;
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104214">104214</asp:Literal>
</div>

<p class="Note">
<asp:Literal runat="server" ID="X104215">104215</asp:Literal>
</p>

<div class="DataLabel" style="width:100%">
<asp:Literal runat="server" ID="X104216">104216</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblLine1" CssClass="DataStatic">Current Addr Line 1</asp:Label><br />
<asp:Literal runat="server" ID="X104218">104218</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblLine2" CssClass="DataStatic">Current Addr Line 2</asp:Label><br />
<asp:Literal runat="server" ID="X104220">104220</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblLine3" CssClass="DataStatic">Current Addr Line 3</asp:Label><br />
<asp:Literal runat="server" ID="X104222">104222</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblLine4" CssClass="DataStatic">Current Addr Line 4</asp:Label><br />
<asp:Literal runat="server" ID="X104224">104224</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblLine5" CssClass="DataStatic">Current Addr Line 5</asp:Label>
</div>

<hr />

<p class="Note">
<asp:Literal runat="server" ID="X104226">104226</asp:Literal>
</p>

<div class="DataLabel" style="width:100%">
<div>
<asp:Literal runat="server" ID="Y104216">104216</asp:Literal><br />
<asp:TextBox runat="server" ID="txtLine1" CssClass="DataInput" OnKeyUp="JavaScript:Validate(1)"></asp:TextBox>&nbsp;<img id="img1" />
</div><div>
<asp:Literal runat="server" ID="Y104218">104218</asp:Literal><br />
<asp:TextBox runat="server" ID="txtLine2" CssClass="DataInput" OnKeyUp="JavaScript:Validate(2)"></asp:TextBox>&nbsp;<img id="img2" />
</div><div>
<asp:Literal runat="server" ID="Y104220">104220</asp:Literal><br />
<asp:TextBox runat="server" ID="txtLine3" CssClass="DataInput" OnKeyUp="JavaScript:Validate(3)"></asp:TextBox>&nbsp;<img id="img3" />
</div><div>
<asp:Literal runat="server" ID="Y104222">104222</asp:Literal><br />
<asp:TextBox runat="server" ID="txtLine4" CssClass="DataInput" OnKeyUp="JavaScript:Validate(4)"></asp:TextBox>&nbsp;<img id="img4" />
</div><div>
<asp:Literal runat="server" ID="Y104224">104224</asp:Literal><br />
<asp:TextBox runat="server" ID="txtLine5" CssClass="DataInput" OnKeyUp="JavaScript:Validate(5)"></asp:TextBox>&nbsp;<img id="img5" />
</div>
</div>

<br />
<asp:Button runat="server" id="X104237" Text="104237" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104356" CssClass="Info">104356</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
Validate(0);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>