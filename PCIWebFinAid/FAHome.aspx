﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FAHome.aspx.cs" Inherits="PCIWebFinAid.FAHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<title><asp:Literal runat="server" ID="X105040">105040</asp:Literal></title>
</head>
<body>
<asp:Literal runat="server" ID="lblGoogleNoScript"></asp:Literal>
<script type="text/javascript">
var showFAQ   = 0;
var showLegal = [0,0,0,0,0,0,0];
function TickOver(img,mode)
{
	img.src = '<%=PCIBusiness.Tools.ImageFolder() %>' + ( mode == 1 ? 'TickOrange' : 'TickWhite' ) + '.png';
}
function FAQ()
{
	showFAQ = ( showFAQ > 0 ? 0 : 1 );
	ShowElt('divFAQ',(showFAQ>0));
}
function Legal(code)
{
	for ( var k = 1 ; k < showLegal.length ; k++ )
		try
		{
			if ( k == code && showLegal[k] == 0 )
				showLegal[k] = 1;
			else
				showLegal[k] = 0;
			ShowElt('LV00'+k.toString(),(showLegal[k]>0));
		}
		catch (x)
		{ }
}
</script>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" ID="hdnCountryCode" />
	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />
	<asp:HiddenField runat="server" ID="hdnPromoCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div style="margin:0px;padding:0px;display:block;width:100%">
	<asp:Image runat="server" ID="P12002" style="max-width:100%;max-height:346px;float:left" />
	<p style="color:#242424;font-family:Arial;font-size:35px;font-weight:600;line-height:1.5em; letter-spacing:0.8px;text-shadow:0px 0px 0px #000000;top:100px;left:0px;right:0px;word-break:break-word;word-wrap:break-word;text-align:center">
	<asp:Literal runat="server" ID="X100002">100002</asp:Literal>
	</p><p style="color:#54595F;font-family:Sans-serif;font-size:19px;font-weight:500;line-height:1.6em;letter-spacing:0.8px;text-shadow:0px 0px 51px #FFFFFF;top:210px;left:0px;right:0px">
	<asp:Literal runat="server" ID="X100004">100004</asp:Literal>
	</p>
	</div>

	<div style="margin:0 auto;width:100%;display:inline-block">

	<asp:Panel runat="server" ID="X105141" style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:340px;height:210px">
		<div style="background-color:#FFCC00;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105012">105012</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105013">105013</asp:Literal>
			</div>
		</div>
		<div style="float:right">
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105017">105017</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105018">105018</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105019">105019</asp:Literal></div>
		</div>
		<asp:Panel runat="server" ID="X105144" style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-top:10px;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105014">105014</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105015">105015</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105016">105016</asp:Literal>
			</div>
		</asp:Panel>
	</asp:Panel>

	<asp:Panel runat="server" ID="X105142" style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:340px;height:210px">
		<div style="background-color:#A6A6A6;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105020">105020</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105021">105021</asp:Literal>
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105025">105025</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105026">105026</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105027">105027</asp:Literal></div>
		</div>
		<asp:Panel runat="server" ID="X105145" style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-top:10px;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105022">105022</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105023">105023</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105024">105024</asp:Literal>
			</div>
		</asp:Panel>
	</asp:Panel>

	<asp:Panel runat="server" ID="X105143" style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:340px;height:210px">
		<div style="background-color:#B18135;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105028">105028</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105029">105029</asp:Literal>
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105033">105033</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105034">105034</asp:Literal></div>
			<div style="line-height:30px"><img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" style="float:left" />&nbsp;<asp:Literal runat="server" ID="X105035">105035</asp:Literal></div>
		</div>
		<asp:Panel runat="server" ID="X105146" style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-top:10px;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105030">105030</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105031">105031</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105032">105032</asp:Literal>
			</div>
		</asp:Panel>
	</asp:Panel>
	</div> 

	<div style="color:#FFFFFF;background-color:#F9CF0E;font-family:Sans-serif;width:99%;padding:0px;margin:0px">
		<div style="padding:10px;font-family:'Open Sans Hebrew',Sans-serif;font-size:20px;line-height:1.5em;letter-spacing:1.3px;margin:0px">
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100287">100287</asp:Literal>
			</div>
			<br />
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100288">100288</asp:Literal>
			</div>
			<br />
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100289">100289</asp:Literal>
			</div>
			<br />
			<div style="text-align:center">
			<asp:HyperLink runat="server" ID="X100009" class="TopButton" style="height:32px;width:120px;padding:3px;padding-top:4px;color:#FFFFFF;background-color:#54595F;text-decoration:none" onmouseover="JavaScript:this.style.backgroundColor='#FF7400'" onmouseout="JavaScript:this.style.backgroundColor='#54595F'">REGISTER</asp:HyperLink>
			</div>
		</div>

		<p style="font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;color:#333232;text-align:center">
		<asp:Literal runat="server" ID="X100045">100045</asp:Literal>
		</p><p style="font-size:20px;font-weight:400;line-height:1.6em;color:#272626">
		<asp:Literal runat="server" ID="X100046">100046</asp:Literal>
		</p>

		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Register.png" style="float:left" />
			<br /><asp:Label runat="server" ID="X105001">105001</asp:Label>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Label runat="server" ID="X105002">105002</asp:Label>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Pay.png" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105003">105003</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105004">105004</asp:Literal>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Enjoy.png" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105005">105005</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105006">105006</asp:Literal>
			</div>
		</div>
		<br />
	</div>

	<div style="margin:0 auto;padding:0px;display:inline-block;width:100%">
		<p style="color: #F88742;font-family:Sans-serif;font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;text-align:center">
		<asp:Literal runat="server" ID="X100051">100051</asp:Literal>
		</p>
		<asp:Panel runat="server" ID="D12010" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12010" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105036">105036</asp:Literal></figcaption>
			</figure>
		</asp:Panel>
		<asp:Panel runat="server" ID="D12011" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12011" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105037">105037</asp:Literal></figcaption>
			</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12012" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12012" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105038">105038</asp:Literal></figcaption>
			</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12023" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12023" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105151">105151</asp:Literal></figcaption>
			</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12024" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12024" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105152">105152</asp:Literal></figcaption>
			</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12028" CssClass="HFig">
			<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
				<asp:Image runat="server" ID="P12028" style="width:100%" />
				<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center"><asp:Literal runat="server" ID="X105153">105153</asp:Literal></figcaption>
			</figure> 
		</asp:Panel>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:10px;margin-right:10px">
	<asp:Literal runat="server" ID="xHIW"></asp:Literal>
	<b>
	<asp:Literal runat="server" ID="X105007">105007</asp:Literal>
	</b><br /><br />
	<asp:Literal runat="server" ID="X105008">105008</asp:Literal>
	</div>

	<div style="background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;text-align:center">
		<a href="JavaScript:FAQ()"><div class="TopButton" style="width:120px;color:#FFFFFF;background-color:#54595F;margin:4px 0px 0px 4px" onmouseover="JavaScript:this.style.backgroundColor='#FF7400'" onmouseout="JavaScript:this.style.backgroundColor='#54595F'"><asp:Literal runat="server" ID="X100063">100063</asp:Literal></div></a>
		<div id="divFAQ" style="color:#FFFFFF;font-family:Sans-serif;visibility:hidden;display:none">
		<asp:Literal runat="server" ID="xFAQ"></asp:Literal>
		</div>
	</div>

	<div style="color:#F9CF0E;font-family:Sans-serif;font-size:40px;font-weight:600;line-height:1.4em;letter-spacing:0.8px;margin-left:10px">
	<asp:Literal runat="server" ID="X105009">105009</asp:Literal>
	</div>

	<div style="text-align:center"> <!-- margin:0px;margin-left:10px -->
	<asp:HyperLink runat="server" ID="H12013"><asp:Image runat="server" ID="P12013" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12025"><asp:Image runat="server" ID="P12025" /></asp:HyperLink>
	</div>
	<div style="text-align:center">
	<asp:HyperLink runat="server" ID="H12026"><asp:Image runat="server" ID="P12026" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12027"><asp:Image runat="server" ID="P12027" /></asp:HyperLink>
	</div>
	<hr />

	<div>
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092"></asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100093"></asp:Literal>
		</b></p><p>
		<asp:Literal runat="server" ID="X104402"></asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100095"></asp:Literal>
		</b></p><p style="display:flex">
		<asp:Image runat="server" ID="P12031" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100096">100096</asp:Label>
		</p><p><b>
		<asp:Literal runat="server" ID="X100101">100101</asp:Literal>
		</b></p><p style="display:flex">
		<asp:Image runat="server" ID="P12032" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X104404" style="padding-top:4px"></asp:Label>
		</p><p style="display:flex">
		<asp:Image runat="server" ID="P12033" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100102" style="padding-top:4px"></asp:Label>
		</p><p><b>
		<asp:Literal runat="server" ID="X104418">104418</asp:Literal>
		</b></p>
		<div style="display:flex">
		<div style="vertical-align:top">
		<asp:Image runat="server" ID="P12034" style="object-fit:contain" />
		</div>
		<asp:Label runat="server" ID="X100105">100105</asp:Label>
		</div>
	</div>

	<!--
	<asp Image run@t="server" ID="P12001" style="height:30px" />
	-->
	<p style="line-height:1.5;margin: 0 0 1em 0;padding-top:10px;font-size:11px">
	<asp:Literal runat="server" ID="X100040">100040</asp:Literal>
	</p>
	<!-- To centre
	<div style="text-align:center">
	-->

	<asp:HyperLink runat="server" ID="H12015"><asp:Image runat="server" ID="P12015" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12016"><asp:Image runat="server" ID="P12016" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12017"><asp:Image runat="server" ID="P12017" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12018"><asp:Image runat="server" ID="P12018" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12019"><asp:Image runat="server" ID="P12019" /></asp:HyperLink>

	<p style="font-size:13px">
	<asp:Literal runat="server" ID="X105010">105010</asp:Literal>
	</p>

	<!--
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		SITE MAP
		</p>
		<asp HyperLink run@t="server" ID="X100008" CssClass="TopButton TopButtonO"></asp:HyperLink>&nbsp;
		<asp HyperLink run@t="server" ID="X100009" CssClass="TopButton TopButtonY"></asp:HyperLink>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	-->
<!--
	<asp HyperLink run@t="server" ID="X100041" ForeColor="Orange" NavigateUrl="CALegal.aspx?DT=001" Target="Legal">100041</asp:HyperLink> |
-->
	<asp:HyperLink runat="server" ID="X100041" ForeColor="Orange" NavigateUrl="JavaScript:Legal(1)"></asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100042" ForeColor="Orange" NavigateUrl="JavaScript:Legal(3)"></asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100043" ForeColor="Orange" NavigateUrl="JavaScript:Legal(5)"></asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100044" ForeColor="Orange" NavigateUrl="JavaScript:Legal(4)"></asp:HyperLink>

	<div id="LV001" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV001',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH001">Header 001</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD001">Detail 001</asp:Literal></p>
	</div>

	<div id="LV003" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV003',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH003">Header 003</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD003">Detail 003</asp:Literal></p>
	</div>

	<div id="LV005" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV005',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH005">Header 005</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD005">Detail 005</asp:Literal></p>
	</div>

	<div id="LV004" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV004',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH004">Header 004</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD004">Detail 004</asp:Literal></p>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
	<asp:Literal runat="server" id="lblChat"></asp:Literal>
</form>
<br />
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
