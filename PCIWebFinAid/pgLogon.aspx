<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgLogon.aspx.cs" Inherits="PCIWebFinAid.pgLogon" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<form id="frmLogin" runat="server">
	<div class="Header3" style="margin-top:2px">
	BackOffice
	</div>
	<p style="text-align:center">
	<asp:TextBox runat="server" ID="txtID" placeholder="User Name" width="300px" Height="25px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtPW" placeholder="Password" TextMode="Password" Width="300px" Height="25px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="btnLogin" Text="LOGIN" OnClick="btnLogin_Click" Width="310px" />
	</p>

	<asp:PlaceHolder runat="server" ID="pnlSecure" Visible="false">
	<p style="text-align:center">
	<br /><br />
	Please capture the one-time security code that was sent to your mobile device
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtS1" width="12px" Height="20px" MaxLength="1" onfocus="this.select()" onkeyup="JavaScript:GetElt('txtS2').focus()"></asp:TextBox>&nbsp;
	<asp:TextBox runat="server" ID="txtS2" width="12px" Height="20px" MaxLength="1" onfocus="this.select()" onkeyup="JavaScript:GetElt('txtS3').focus()"></asp:TextBox>&nbsp;
	<asp:TextBox runat="server" ID="txtS3" width="12px" Height="20px" MaxLength="1" onfocus="this.select()" onkeyup="JavaScript:GetElt('txtS4').focus()"></asp:TextBox>&nbsp;
	<asp:TextBox runat="server" ID="txtS4" width="12px" Height="20px" MaxLength="1" onfocus="this.select()" onkeyup="JavaScript:GetElt('txtS5').focus()"></asp:TextBox>&nbsp;
	<asp:TextBox runat="server" ID="txtS5" width="12px" Height="20px" MaxLength="1" onfocus="this.select()" onkeyup="JavaScript:GetElt('txtS6').focus()"></asp:TextBox>&nbsp;
	<asp:TextBox runat="server" ID="txtS6" width="12px" Height="20px" MaxLength="1" onfocus="this.select()"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="btnSecure" Text="OK" OnClick="btnOK_Click" Width="310px" />
	<br /><br />
	<asp:Button runat="server" ID="btnCancel" Text="CANCEL" OnClick="btnCancel_Click" Width="310px" />
	</p>
	</asp:PlaceHolder>

	<p style="text-align:center">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
