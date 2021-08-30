<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PYHeader.ascx.cs" Inherits="PCIWebFinAid.PYHeader" %>

<div class="Header1 HeaderPY" style="background-color:#A89889">
	<div class="HCol1">
		<asp:Image runat="server" ID="P12001" />
	</div>
	<div class="HCol2">
		<asp:Literal runat="server" ID="X100003">100003</asp:Literal>
	</div>
	<div class="HCol3" style="visibility:hidden;display:none">
		<asp:HyperLink runat="server" ID="X100008" CssClass="TopButton TopButtonO"></asp:HyperLink>&nbsp;
		<asp:HyperLink runat="server" ID="X100009" CssClass="TopButton TopButtonP"></asp:HyperLink>&nbsp;
		<asp:DropDownList runat="server" ID="lstLang" CssClass="TopButton" AutoPostBack="true" style="padding:0px"></asp:DropDownList>
	</div>
</div>
