<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="PCIWebFinAid.Menu" %>

<div style="font-size:2px">&nbsp;</div>
<div class="MenuRow">
	<asp:Button runat="server" style="background-color:black" CssClass="MenuButton" id="X103026" onclick="MenuClick" ToolTip="LGet" />
	<asp:Button runat="server" style="background-color:black" CssClass="MenuButton" id="X103027" onclick="MenuClick" ToolTip="LView" />
	<asp:Button runat="server" style="background-color:black" CssClass="MenuButton" id="X103028" onclick="MenuClick" ToolTip="LPay" />
	<asp:Button runat="server" style="background-color:black" CssClass="MenuButton" id="X103029" onclick="MenuClick" ToolTip="LChange" />
	<asp:Button runat="server" style="background-color:black" CssClass="MenuButton" id="X103030" onclick="MenuClick" ToolTip="LOther" />
</div>
<asp:Literal runat="server" ID="lblMenuJS"></asp:Literal>