<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ISHome.aspx.cs" Inherits="PCIWebFinAid.ISHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="ISHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<title><asp:Literal runat="server" ID="X105040">105040</asp:Literal></title>
	<!--
	<style>
	body
	{
		font-family: var(--e-global-typography-text-font-family), Sans-serif;
	}
	</style>
	-->
</head>
<body>
<script type="text/javascript">
var showLegal = [0,0,0,0,0,0,0];
function TickOver(img,mode)
{
	img.src = '<%=PCIBusiness.Tools.ImageFolder() %>' + ( mode == 1 ? 'TickOrange' : 'TickWhite' ) + '.png';
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

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div style="margin:0px;padding:0px;display:block;width:100%;min-height:346px">
	<asp:Image runat="server" ID="P12002" style="max-width:100%;max-height:346px;float:left" />
	<p style="color:#242424;font-family:Arial;font-size:35px;font-weight:600;line-height:1.5em; letter-spacing:0.8px;text-shadow:0px 0px 0px #000000;top:100px;left:0px;right:0px;word-break:break-word;word-wrap:break-word;text-align:center">
	<asp:Literal runat="server" ID="X100002">100002</asp:Literal>
	</p>
	</div>

	<div style="color:#FFFFFF;background-color:#FF9900;font-family:Sans-serif;width:99%;padding:0px;margin:0px">
		<asp:Panel runat="server" ID="pnlTicks" style="padding:10px;font-family:'Open Sans Hebrew',Sans-serif;font-size:20px;line-height:1.5em;letter-spacing:1.3px;margin:0px">
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100287"></asp:Literal>
			</div>
			<br />
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100288"></asp:Literal>
			</div>
			<br />
			<div style="margin-left:20px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100289"></asp:Literal>
			</div>
		</asp:Panel>

		<p style="font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;color:#333232;text-align:center">
		<asp:Literal runat="server" ID="X100045">100045</asp:Literal>
		</p><p style="font-size:20px;font-weight:400;line-height:1.6em;color:#272626;padding-left:10px">
		<asp:Literal runat="server" ID="X100046">100046</asp:Literal>
		</p>

		<asp:Image runat="server" ID="P12036" style="padding-left:5px" class="ImageAdjust" />

		<div style="color:#54595F;font-size:19px;font-weight:600;padding-left:10px">
			<asp:Image runat="server" ID="PSignup" style="float:left" />
			<br /><asp:Label runat="server" ID="X105001">105001</asp:Label>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Label runat="server" ID="X105002">105002</asp:Label>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600;padding-left:10px">
			<asp:Image runat="server" ID="PConfigure" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105003">105003</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105004">105004</asp:Literal>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600;padding-left:10px">
			<asp:Image runat="server" ID="PInstall" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105005">105005</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105006">105006</asp:Literal>
			</div>
		</div>
		<br />
	</div>

	<div style="margin:0 auto;padding:0px;display:inline-block;width:100%">
		<p style="color:#F88742;font-family:Sans-serif;font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;text-align:center">
		<asp:Literal runat="server" ID="X100051">100051</asp:Literal>
		</p>
		<p style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:10px;margin-right:10px">
		<asp:Literal runat="server" ID="X105044">105044</asp:Literal>
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
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Image 12023 Caption</figcaption>
		</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12024" CssClass="HFig">
		<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
			<asp:Image runat="server" ID="P12024" style="width:100%" />
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Image 12024 Caption</figcaption>
		</figure> 
		</asp:Panel>
		<asp:Panel runat="server" ID="D12028" CssClass="HFig">
		<figure style="display:inline-block;box-shadow:0px 0px 50px 0px rgba(15,15,43,0.58);width:340px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:10px 10px 10px 10px;padding:20px 20px 10px 10px">
			<asp:Image runat="server" ID="P12028" style="width:100%" />
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Image 12028 Caption</figcaption>
		</figure> 
		</asp:Panel>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:10px;margin-right:10px">
	<asp:Literal runat="server" ID="xHIW"></asp:Literal>
	</div>

	<hr />

	<div style="color:#F9CF0E;font-family:Sans-serif;font-size:40px;font-weight:600;line-height:1.4em;letter-spacing:0.8px;margin-left:10px">
	<asp:Literal runat="server" ID="X105041">105041</asp:Literal>
	</div>
	<div style="margin-bottom:20px">
	<asp:Literal runat="server" ID="X105042">105042</asp:Literal>
	</div>

	<div style="text-align:center">
	<asp:HyperLink runat="server" ID="H12037"><asp:Image runat="server" ID="P12037" /></asp:HyperLink>&nbsp;
	<asp:HyperLink runat="server" ID="H12038"><asp:Image runat="server" ID="P12038" /></asp:HyperLink>
	</div>

	<div style="text-align:center">
	<asp:HyperLink runat="server" ID="X100009" CssClass="TopButton TopButtonD"></asp:HyperLink>&nbsp;
	<asp:HyperLink runat="server" ID="X105045" CssClass="TopButton TopButtonD"></asp:HyperLink>
	</div>

	<div style="margin-top:20px">
	<asp:Literal runat="server" ID="X105043">105043</asp:Literal>
	</div>

	<div style="text-align:center">
	<asp:HyperLink runat="server" ID="H12013"><asp:Image runat="server" ID="P12013" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12025"><asp:Image runat="server" ID="P12025" /></asp:HyperLink>
	</div>
	<div style="text-align:center">
	<asp:HyperLink runat="server" ID="H12026"><asp:Image runat="server" ID="P12026" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12027"><asp:Image runat="server" ID="P12027" /></asp:HyperLink>
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
	<!-- To centre
	<div style="text-align:center">
	-->

	<asp:HyperLink runat="server" ID="H12015"><asp:Image runat="server" ID="P12015" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12016"><asp:Image runat="server" ID="P12016" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12017"><asp:Image runat="server" ID="P12017" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12018"><asp:Image runat="server" ID="P12018" /></asp:HyperLink>
	<asp:HyperLink runat="server" ID="H12019"><asp:Image runat="server" ID="P12019" /></asp:HyperLink>

	<asp:PlaceHolder runat="server" ID="pnlBr1">
	<br /><br />
	</asp:PlaceHolder>

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
