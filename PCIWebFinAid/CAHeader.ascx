<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CAHeader.ascx.cs" Inherits="PCIWebFinAid.CAHeader" %>

<div class="Header1 HeaderCA" id="HRow">
	<div class="HCol1">
		<asp:Image runat="server" ID="P12001" Height="75px" />
	</div>
	<div class="HCol2">
		<asp:Literal runat="server" ID="X100003">100003</asp:Literal>
	</div>
	<div class="HCol3">
		<asp:HyperLink runat="server" ID="X100008" CssClass="TopButton TopButtonO"></asp:HyperLink>&nbsp;
		<asp:HyperLink runat="server" ID="X100009" CssClass="TopButton TopButtonY"></asp:HyperLink>&nbsp;
		<asp:DropDownList runat="server" ID="lstLang" CssClass="TopButton" AutoPostBack="true" style="padding:0px"></asp:DropDownList>
	</div>
</div>
<asp:Literal runat="server" ID="HJs"></asp:Literal>