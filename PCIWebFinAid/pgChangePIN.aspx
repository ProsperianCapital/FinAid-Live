<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangePIN.aspx.cs" Inherits="PCIWebFinAid.pgChangePIN" %>
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
function CheckPIN(n)
{
	try
	{
		var p0 = GetEltValue('txtPIN0');
		var p1 = GetEltValue('txtPIN1');
		var p2 = GetEltValue('txtPIN2');

		if ( n < 3 )
		{
			ShowElt('imgY'+n,false);
			ShowElt('imgN'+n,false);
			var p = GetEltValue('txtPIN'+n);
			if ( ! ValidPIN(p,<%=MIN_PIN_LENGTH%>) )
				ShowElt('imgN'+n,true);
			else if ( n == 1 && p0 == p1 ) // New PIN = old PIN
				ShowElt('imgN'+n,true);
			else if ( n == 2 && p1 != p2 ) // New PINs not equal
				ShowElt('imgN'+n,true);
			else
			{
				ShowElt('imgY'+n,true);
				if ( n == 1 && p2.length > 0 )
				{
					ShowElt('imgY2',(p1==p2));
					ShowElt('imgN2',(p1!=p2));
				}
			}
		}
		var ok = ( ValidPIN(p0,<%=MIN_PIN_LENGTH%>) &&
		           ValidPIN(p1,<%=MIN_PIN_LENGTH%>) &&
		           ValidPIN(p2,<%=MIN_PIN_LENGTH%>) &&
		           p1 == p2 );
		DisableElt('X104204',!ok);
		return ok;
	}
	catch (x)
	{
		alert(x.message);
	}
	return false;	
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104197">104197</asp:Literal>
</div>

<div class="DataLabel" style="width:100%">

<asp:Literal runat="server" ID="X104198">104198</asp:Literal><br />
<asp:TextBox runat="server" ID="txtPIN0" CssClass="DataInput" OnKeyUp="JavaScript:CheckPIN(0)"></asp:TextBox>&nbsp;
	<img id="imgY0" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN0" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
<br /><br />

<asp:Literal runat="server" ID="X104200">104200</asp:Literal><br />
<asp:TextBox runat="server" ID="txtPIN1" CssClass="DataInput" OnKeyUp="JavaScript:CheckPIN(1)"></asp:TextBox>&nbsp;
	<img id="imgY1" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN1" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
<br /><br />

<asp:Literal runat="server" ID="X104202">104202</asp:Literal><br />
<asp:TextBox runat="server" ID="txtPIN2" CssClass="DataInput" OnKeyUp="JavaScript:CheckPIN(2)"></asp:TextBox>&nbsp;
	<img id="imgY2" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN2" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div>

<br />
<asp:Button runat="server" id="X104204" Text="104204" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104355" CssClass="Info">104355</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
CheckPIN(99);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>