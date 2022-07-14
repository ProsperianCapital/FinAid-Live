<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgVerifyIdentity.aspx.cs" Inherits="PCIWebFinAid.pgVerifyIdentity" %>
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
function Validate()
{
	try
	{
		return true;
	}
	catch (x)
	{
		alert(x.message);
	}
	return false;	
}
</script>

<div class="Header3">
We're sorry ...
</div>
<p class="Error">
This page is still under construction.
<br /><br />
Please check back later for updates.
</p>

<script type="text/javascript">
//	if (Validate())
//		DisableElt('X',false);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>