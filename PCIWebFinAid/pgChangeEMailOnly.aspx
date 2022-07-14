<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeEMailOnly.aspx.cs" Inherits="PCIWebFinAid.pgChangeEMailOnly" %>
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
function CheckMail(n)
{
	try
	{
		ShowElt('imgY'+n,false);
		ShowElt('imgN'+n,false);
		var p = GetEltValue('txtEMailNew'+n).toUpperCase();
		if ( p.length < 1 )
			return;
		if ( ! ValidEmail(p) )
			ShowElt('imgN'+n,true);
		else if ( n == 2 && GetEltValue('txtEMailNew1').toUpperCase() != p )
			ShowElt('imgN'+n,true);
		else
		{
			ShowElt('imgY'+n,true);
			var h = GetEltValue('txtEMailNew2').toUpperCase();
			if ( n == 1 && h.length > 0 )
			{
				ShowElt('imgY2',(h==p));
				ShowElt('imgN2',(h!=p));
			}
		}
	}
	catch (x)
	{
		alert(x.message);
	}			
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104197">104197</asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104198">104198</asp:Literal></td>
	<td><asp:TextBox runat="server" Width="480px" ID="txtEMailOld" ReadOnly="true"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104200">104200</asp:Literal></td>
	<td>
		<asp:TextBox runat="server" Width="480px" ID="txtEMailNew1" OnChange="JavaScript:CheckMail(1)"></asp:TextBox>&nbsp;
		<img id="imgY1" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
		<img id="imgN1" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
	</td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104202">104202</asp:Literal></td>
	<td>
		<asp:TextBox runat="server" Width="480px" ID="txtEMailNew2" OnChange="JavaScript:CheckMail(2)"></asp:TextBox>&nbsp;
		<img id="imgY2" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
		<img id="imgN2" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
	</td></tr>
</table>
<br /><br />
<asp:Button runat="server" id="X104204" Text="104204" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104355" CssClass="Info">104355</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
CheckMail(1);
CheckMail(2);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>