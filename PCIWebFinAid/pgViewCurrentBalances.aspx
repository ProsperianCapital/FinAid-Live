<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewCurrentBalances.aspx.cs" Inherits="PCIWebFinAid.pgViewCurrentBalances" %>
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

<div class="Header3">
<asp:Literal runat="server" ID="X104095">104095</asp:Literal>
</div>

<div class="DataLabel" style="width:100%">
<p>
<asp:Literal runat="server" ID="X104096">104096</asp:Literal><br />
<asp:Label   runat="server" ID="lblRegFee" CssClass="DataStatic">RegFee</asp:Label>
</p><p>
<asp:Literal runat="server" ID="X104106">104106</asp:Literal><br />
<asp:Label   runat="server" ID="lblMonthlyFee" CssClass="DataStatic">MonthlyFee</asp:Label>
</p><p>
<asp:Literal runat="server" ID="X104098">104098</asp:Literal><br />
<asp:Label   runat="server" ID="lblGrantLimit" CssClass="DataStatic">GrantLimit</asp:Label>
</p><p>
<asp:Literal runat="server" ID="X104102">104102</asp:Literal><br />
<asp:Label   runat="server" ID="lblGrantAvail" CssClass="DataStatic">GrantAvail</asp:Label>
</p><p>
<asp:Literal runat="server" ID="X104104">104104</asp:Literal><br />
<asp:Label   runat="server" ID="lblGrantStatus" CssClass="DataStatic">GrantStatus</asp:Label>
</p><p>
<asp:Literal runat="server" ID="X104108">104108</asp:Literal><br />
<asp:Label   runat="server" ID="lblFeeDate" CssClass="DataStatic">FeeDate</asp:Label>
</p>
</div>
<br />
<asp:Label runat="server" ID="X104110" CssClass="Info">104110</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>