<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeEMail.aspx.cs" Inherits="PCIWebFinAid.pgChangeEMail" %>
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
		var eX  = GetEltValue('txtEMail');
		var pX  = GetEltValue('txtPhone');
		var eOK = ValidEmail(eX);
		var pOK = ValidPhone(pX,0);
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		if ( n == 1 || ( n == 0 && eX.length > 0 ) )
			GetElt('imgEMail').src = dir + ( eOK ? 'Tick' : 'Cross' ) + '.png';
		if ( n == 2 || ( n == 0 && eP.length > 0 ) )
			GetElt('imgPhone').src = dir + ( pOK ? 'Tick' : 'Cross' ) + '.png';
		DisableElt('X104258',!(eOK&pOK));
		return eOK & pOK;
	}
	catch (x)
	{ }
	return false;
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104247">104247</asp:Literal>
</div>

<p class="Note">
<asp:Literal runat="server" ID="X104248">104248</asp:Literal>
</p>

<div class="DataLabel" style="width:100%">
<asp:Literal runat="server" ID="X104249">104249</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblEMail" CssClass="DataStatic">EMail</asp:Label><br />
<asp:Literal runat="server" ID="X104251">104251</asp:Literal>&nbsp;
<asp:Label   runat="server" ID="lblPhone" CssClass="DataStatic">Phone</asp:Label>
</div>

<hr />

<p class="Note">
<asp:Literal runat="server" ID="X104253">104253</asp:Literal>
</p>

<div class="DataLabel" style="width:100%">
<p>
<asp:Literal runat="server" ID="X104254">104254</asp:Literal><br />
<asp:TextBox runat="server" ID="txtEMail" CssClass="DataInput" OnKeyUp="JavaScript:Validate(1)"></asp:TextBox>&nbsp;<img id="imgEMail" />
</p><p>
<asp:Literal runat="server" ID="X104256">104256</asp:Literal><br />
<asp:TextBox runat="server" ID="txtPhone" CssClass="DataInput" OnKeyUp="JavaScript:Validate(2)"></asp:TextBox>&nbsp;<img id="imgPhone" />
</p>
</div>

<asp:Button runat="server" id="X104258" Text="104258" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104357" CssClass="Info">104357</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
Validate(0);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>