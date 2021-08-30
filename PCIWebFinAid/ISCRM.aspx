<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ISCRM.aspx.cs" Inherits="PCIWebFinAid.ISCRM" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="ISHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<title><asp:Literal runat="server" ID="X105040">105040</asp:Literal></title>
	<style>
	.InputRow
	{
		display: flex;
		flex-wrap: wrap;
		margin: 0 0 10px 0;
	}
	.InputCol33
	{
		flex: 33%;
	}
	.InputCol66
	{
		flex: 66%;
	}
	.InputCol99
	{
		flex: 99%;
	}
	.SpaceAbove1
	{
		margin-top: 6px;
	}
	.SpaceAbove2
	{
		margin-top: 22px;
	}
	.SpaceAbove3
	{
		margin-top: 24px;
	}
	.SpaceAbove4
	{
		margin-top: 4px;
	}
	@media screen and (max-width: 800px)
	{
		.InputCol33
		{
			flex: 100%;
		}
		.InputCol66
		{
			flex: 100%;
		}
		.InputCol99
		{
			flex: 100%;
		}
		.SpaceAbove2
		{
			margin-top: 4px;
		}
		.SpaceAbove3
		{
			margin-top: 8px;
			margin-left: 24%;
			margin-right: 20%;
		}
		.SpaceAbove4
		{
			margin-top: 6px;
			margin-left: 25%;
			margin-right: 25%;
		}
	}
	.InputLabel
	{
		color: #EAA62E;
		cursor: pointer;
		font-weight: bold;
	}
	.InputBox
	{
		-webkit-box-flex: 1;
		-moz-appearance: none;
		background-color: #ECECEC;
		border-color: #D8CDCD;
		border-width: 0px 0px 0px 0px;
		border-radius: 20px 20px 20px 20px;
		border: 1px solid #818a91;
		box-shadow: none;
		box-sizing: border-box;
		color: #54595F;
		display: block;
		flex-basis: 100%;
		flex-grow: 1;
		font-family: var( --e-global-typography-text-font-family ), Sans-serif;
		font-size: 13px;
		line-height: 0.1em;
		letter-spacing: 1px;
		max-width: 100%;
		min-height: 33px;
		text-align: inherit;
		padding: 4px 12px;
		width: 90%;
	}
	.InputBox:focus
	{
		outline: none;
		box-shadow: 0px 0px 4px blue;
	}
	</style>
</head>
<body>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" ID="hdnCountryCode" />
	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<p></p>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105102" AssociatedControlID="X105103" class="InputLabel">105102</asp:Label>
			<asp:TextBox runat="server" ID="X105103" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66 SpaceAbove3">
			<asp:Button runat="server" ID="X105133" CssClass="TopButton TopButtonO" Width="180px" OnClick="btnGet_Click" Text="Verify Number" />
			&nbsp;&nbsp;&nbsp;
			<asp:Image runat="server" ID="imgOK" />
		</div>
	</div>

	<hr />

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105104" AssociatedControlID="X105105" class="InputLabel">105104</asp:Label>
			<asp:TextBox runat="server" ID="X105105" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105106" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105107" AssociatedControlID="X105108" class="InputLabel">105107</asp:Label>
			<asp:TextBox runat="server" ID="X105108" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105109" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105110" AssociatedControlID="X105111" class="InputLabel">105110</asp:Label>
			<asp:TextBox runat="server" ID="X105111" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105112" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105113" AssociatedControlID="X105114" class="InputLabel">105113</asp:Label>
			<asp:TextBox runat="server" ID="X105114" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105115" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105116" AssociatedControlID="X105117" class="InputLabel">105116</asp:Label>
			<asp:TextBox runat="server" ID="X105117" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105118" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105119" AssociatedControlID="X105120" class="InputLabel">105119</asp:Label>
			<asp:TextBox runat="server" ID="X105120" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105121" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105122" AssociatedControlID="X105123" class="InputLabel">105122</asp:Label>
			<asp:TextBox runat="server" ID="X105123" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105124" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105125" AssociatedControlID="X105126" class="InputLabel">105125</asp:Label>
			<asp:TextBox runat="server" ID="X105126" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105127" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<asp:Label runat="server" ID="X105128" AssociatedControlID="X105129" class="InputLabel">105128</asp:Label>
			<asp:TextBox runat="server" ID="X105129" CssClass="InputBox SpaceAbove1"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<asp:TextBox runat="server" ID="X105130" CssClass="InputBox SpaceAbove2"></asp:TextBox>
		</div>
	</div>

	<!--
	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl9n" for="txt9n" class="InputLabel">9. General/Other</label>
			<asp:TextBox runat="server" ID="txt9n" CssClass="InputBox SpaceAbove1" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl9d" for="txt9d" class="InputLabel">Message 9</label>
			<asp:TextBox runat="server" ID="txt9d" CssClass="InputBox SpaceAbove2" PlaceHolder="Type your message"></asp:TextBox>
		</div>
	</div>
	-->

	<asp:Button runat="server" ID="X105131" CssClass="TopButton TopButtonO SpaceAbove4" Width="180px" OnClick="btnSave_Click"></asp:Button>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
</form>
<br />
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
