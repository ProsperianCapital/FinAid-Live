<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewProfile.aspx.cs" Inherits="PCIWebFinAid.pgViewProfile" %>
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
<form id="frmMain" runat="server">
	<div style="display:inline-flex">
	<ascx:XMenu runat="server" ID="ascxXMenu"></ascx:XMenu>
	<table>
	<tr>
	<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;padding:10px;white-space:nowrap">
		<asp:Label   runat="server" ID="X104002" Font-Bold="true" Font-Size="24px">104002</asp:Label><br /><br />
		<asp:Label   runat="server" ID="X104004" Font-Bold="true">104004</asp:Label><br />
		<asp:Literal runat="server" ID="lblAddress"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104006" Font-Bold="true">104006</asp:Label><br />
		<asp:Literal runat="server" ID="lblEMail"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104008" Font-Bold="true">104008</asp:Label><br />
		<asp:Literal runat="server" ID="lblCellNo"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104010" Font-Bold="true">104010</asp:Label><br />
		<asp:Literal runat="server" ID="lblOption"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104012" Font-Bold="true">104012</asp:Label><br />
		<asp:Literal runat="server" ID="lblFee"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104014" Font-Bold="true">104014</asp:Label><br />
		<asp:Literal runat="server" ID="lblCredit"></asp:Literal><br /><br />
		<asp:Label   runat="server" ID="X104016" Font-Bold="true">104016</asp:Label><br />
		<asp:Literal runat="server" ID="lblDueDate"></asp:Literal>
		<hr />
		<asp:Literal runat="server" ID="lblDate"></asp:Literal>
	</td><td style="white-space:normal">
		<asp:Label runat="server" ID="X104018" cssclass="Header3">104018</asp:Label>
		<table>
		<tr>
			<td><asp:Label runat="server" ID="X104019" CssClass="DataLabel">104019</asp:Label></td>
			<td><asp:Literal runat="server" ID="lblUserName"></asp:Literal></td></tr>
		<tr>
			<td></td>
			<td><asp:Literal runat="server" ID="lblLastLogon"></asp:Literal></td></tr>
		</table>
		<br />
		<asp:Literal runat="server" ID="X104023">104023</asp:Literal>
		<br /><br />
		<asp:Table runat="server" ID="tblHistory" style="border:1px solid #000000">
		<asp:TableRow CssClass="tRowHead">
			<asp:TableCell ID="X104367">104367</asp:TableCell>
			<asp:TableCell ID="X104368">104368</asp:TableCell></asp:TableRow>
		</asp:Table>
		<br />
		<asp:Label runat="server" ID="X104027" CssClass="Info">104027</asp:Label>
	</td></tr>
	</table>
	</div>

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
