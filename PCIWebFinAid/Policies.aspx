<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Policies.aspx.cs" Inherits="PCIWebFinAid.Policies" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="ISHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<title><asp:Literal runat="server" ID="X105040">105040</asp:Literal></title>
	<style>
	</style>
</head>
<body>
<form id="frmHome" runat="server">
	<asp:Literal runat="server" ID="lblGoogleNoScript"></asp:Literal>
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" ID="hdnCountryCode" />
	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px">
		<p class='FAQQuestion'><asp:Literal runat="server" ID="polHead">Policy Header</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="polDtl">Policy Detail</asp:Literal></p>
	</div>

	<div>
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092"></asp:Literal>
		</p>

		<asp:PlaceHolder runat="server" ID="pnlContact01">
		<p><b>
		<asp:Literal runat="server" ID="X100093"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact02">
		<p>
		<asp:Literal runat="server" ID="X104402"></asp:Literal>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact03">
		<p><b>
		<asp:Literal runat="server" ID="X100095"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact04">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12031" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100096"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact05">
		<p><b>
		<asp:Literal runat="server" ID="X100101"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact06">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12032" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X104404" style="padding-top:4px"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact07">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12033" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100102" style="padding-top:4px"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact08">
		<p><b>
		<asp:Literal runat="server" ID="X104418"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" ID="pnlContact09">
		<div style="display:flex">
		<div style="vertical-align:top">
		<asp:Image runat="server" ID="P12034" style="object-fit:contain" />
		</div>
		<asp:Label runat="server" ID="X100105"></asp:Label>
		</div>
		</asp:PlaceHolder>
	</div>

	<!--
	<asp Image run@t="server" ID="P12001" style="height:30px" />
	-->
	<p style="line-height:1.5;margin: 0 0 1em 0;padding-top:10px;font-size:11px">
	<asp:Literal runat="server" ID="X100040">100040</asp:Literal>
	</p>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
	<!--
	<asp Literal run@t="server" id="lblChat"></asp>
	-->
</form>
<br />
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
