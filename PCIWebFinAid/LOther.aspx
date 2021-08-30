<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LOther.aspx.cs" Inherits="PCIWebFinAid.LOther" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>Other</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>

<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />

<!--#include file="IncludeLogin1.htm" -->

<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;font-weight:bold;font-size:24px;padding:10px">
<asp:Literal runat="server" ID="X104303"></asp:Literal><br /><br />
<asp:Button runat="server" ToolTip="btn00" id="X104002" style="width:160px;background-color:black" OnClientClick="JavaScript:location.href='LWelcome.aspx';return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn01" id="X104305" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(1);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn02" id="X104306" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(2);return false" /><br /><br />
<asp:Button runat="server" ToolTip="btn03" id="X104307" style="width:160px;background-color:black" OnClientClick="JavaScript:SetTab(3);return false" />
</td>
<td style="vertical-align:top">

<div id="div01">
<asp:Label runat="server" ID="X104308" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104318"></asp:Literal>
<br /><br />
<asp:Literal runat="server" ID="X104309"></asp:Literal><br />
<asp:TextBox runat="server" ID="txtOutstanding" Width="160px" ReadOnly="true"></asp:TextBox>
<br /><br />
<asp:Literal runat="server" ID="X104312"></asp:Literal><br />
<asp:DropDownList runat="server" ID="lstReason"></asp:DropDownList>
<br /><br />
<asp:Literal runat="server" ID="X104316"></asp:Literal>
<br /><br />
<asp:Button runat="server" id="X104317" />&nbsp;<asp:Button runat="server" id="X104314" />
<br /><br />
<asp:Label runat="server" ID="X104370" CssClass="Error"></asp:Label>
</div>

<div id="div02">
<asp:Label runat="server" ID="X104325" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104326"></asp:Literal>
<br /><br />
<asp:Literal runat="server" ID="X104327"></asp:Literal><br />
<asp:TextBox runat="server" ID="txtQuery" TextMode="MultiLine" Rows="8" Width="480px"></asp:TextBox>
<br /><br />
<asp:Button runat="server" id="X104329" />
<br /><br />
<asp:Label runat="server" ID="X104331" CssClass="Error"></asp:Label>
</div>

<div id="div03">
<asp:Label runat="server" ID="X104338" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Literal runat="server" ID="X104339"></asp:Literal>
<br /><br />
<asp:RadioButton runat="server" ID="rdoRate05" GroupName="rdoRate" />
<img src="Images/Rate05.png" onclick="JavaScript:GetElt('rdoRate05').checked=true" style="vertical-align:middle" />
<asp:Literal runat="server" ID="X104348" /><br />
<asp:RadioButton runat="server" ID="rdoRate04" GroupName="rdoRate" />
<img src="Images/Rate04.png" onclick="JavaScript:GetElt('rdoRate04').checked=true" style="vertical-align:middle" />
<asp:Literal runat="server" ID="X104340" /><br />
<asp:RadioButton runat="server" ID="rdoRate03" GroupName="rdoRate" />
<img src="Images/Rate03.png" onclick="JavaScript:GetElt('rdoRate03').checked=true" style="vertical-align:middle" />
<asp:Literal runat="server" ID="X104342" /><br />
<asp:RadioButton runat="server" ID="rdoRate02" GroupName="rdoRate" />
<img src="Images/Rate02.png" onclick="JavaScript:GetElt('rdoRate02').checked=true" style="vertical-align:middle" />
<asp:Literal runat="server" ID="X104344" /><br />
<asp:RadioButton runat="server" ID="rdoRate01" GroupName="rdoRate" />
<img src="Images/Rate01.png" onclick="JavaScript:GetElt('rdoRate01').checked=true" style="vertical-align:middle" />
<asp:Literal runat="server" ID="X104346" />
<br /><br />
<asp:Literal runat="server" ID="X104385"></asp:Literal><br />
<asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" Rows="8" Width="480px"></asp:TextBox>
<br /><br />
<asp:Button runat="server" id="X104352" />
<br /><br />
<asp:Label runat="server" ID="X104353" CssClass="Error"></asp:Label>
</div>

</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>