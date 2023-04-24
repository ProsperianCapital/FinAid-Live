<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LGHome.aspx.cs" Inherits="PCIWebFinAid.LGHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="LGHeader.ascx" %>
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

	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div style="margin:0px;padding:0px;display:block;width:100%;min-height:375px">
	<asp:Image runat="server" ID="P12002" style="max-width:100%;max-height:346px;float:left;margin-right:5px" />
	<p style="color:#242424;font-family:Arial;font-size:35px;font-weight:600;line-height:1.5em; letter-spacing:0.8px;text-shadow:0px 0px 0px #000000;top:100px;left:0px;right:0px;word-break:break-word;word-wrap:break-word;text-align:center">
	<asp:Literal runat="server" ID="X100002">100002</asp:Literal>
	</p><p style="color:#54595F;font-family:Sans-serif;font-size:19px;font-weight:500;line-height:1.6em;letter-spacing:0.8px;text-shadow:0px 0px 51px #FFFFFF;top:210px">
	<asp:Literal runat="server" ID="X100004">100004</asp:Literal>
	</p>
	</div>

<!--
	<div style="position:relative">
	<img src="ImagesCA/CA-Assist2.jpg" style="width:100%" />
	<div style="color:#FFFFFF; font-family:Sans-serif; position:absolute; top:10px; left:5%; width:95%">
		<p style="font-size:35px; font-weight:400; line-height:1.4em; letter-spacing:0.8px">
		HOW IT WORKS
-->

<!--
	<div style="color:#FFFFFF;background-color:#F9CF0E;font-family:Sans-serif;width:99%;padding:0px;margin:0px;background-image:url('<%=PCIBusiness.Tools.ImageFolder() %>LG-Background.jpg');background-repeat:no-repeat;background-size:cover">
-->
	<div style="color:#FFFFFF;background-color:#F9CF0E;font-family:Sans-serif;width:99%;padding:0px;margin:0px">
		<br />
		<p style="font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;color:#333232;margin-left:50px">
		<asp:Literal runat="server" ID="X100045">100045</asp:Literal>
		</p><p style="font-size:20px;font-weight:400;line-height:1.6em;color:#272626;margin-left:50px">
		<asp:Literal runat="server" ID="X100046">100046</asp:Literal>
		</p>

		<div style="padding:10px;font-family:'Open Sans Hebrew',Sans-serif;font-size:20px;line-height:1.5em;letter-spacing:1.3px;margin:0px">
			<div style="margin-left:40px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100287">100287</asp:Literal>
			</div>
			<br />
			<div style="margin-left:40px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100288">100288</asp:Literal>
			</div>
			<br />
			<div style="margin-left:40px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100289">100289</asp:Literal>
			</div>
			<br />
			<div style="margin-left:40px">
			<asp:HyperLink runat="server" ID="X100009" class="TopButton" style="height:32px;width:120px;padding:3px;padding-top:4px;color:#FFFFFF;background-color:#54595F;text-decoration:none" onmouseover="JavaScript:this.style.backgroundColor='#FF7400'" onmouseout="JavaScript:this.style.backgroundColor='#54595F'" NavigateUrl="JavaScript:ShowElt('divNotAvailable',true,0,59)">100009</asp:HyperLink>
			</div>
			<br />&nbsp;
		</div>
	</div>

	<div style="margin:0 auto;padding:0px;display:inline-block;width:100%">
		<p style="color: #F88742;font-family:Sans-serif;font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;text-align:center">
		<asp:Literal runat="server" ID="X100051">100051</asp:Literal>
		</p>
		<!--
		<figure> images included here
		-->
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:10px;margin-right:10px">
	<asp:Literal runat="server" ID="xHIW"></asp:Literal>
	<b>
	<asp:Literal runat="server" ID="X105007">105007</asp:Literal>
	</b><br /><br />
	<asp:Literal runat="server" ID="X105008">105008</asp:Literal>
	</div>

	<!--
	<div style="text-align:center">
	<asp HyperLink run@t="server" ID="H12013"><asp Image run@t="server" ID="P12013" /></asp:HyperLink>
	<asp HyperLink run@t="server" ID="H12025"><asp Image run@t="server" ID="P12025" /></asp:HyperLink>
	</div>
	<div style="text-align:center">
	<asp HyperLink run@t="server" ID="H12026"><asp Image run@t="server" ID="P12026" /></asp:HyperLink>
	<asp HyperLink run@t="server" ID="H12027"><asp Image run@t="server" ID="P12027" /></asp:HyperLink>
	</div>
	-->

	<br />&nbsp;<hr />

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

	<div id="divNotAvailable" style="display:none;visibility:hidden" class="Popup2">
	<div class="PopupHead">
	<asp:Literal runat="server" id="X105045">Sorry ...</asp:Literal>
	<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('divNotAvailable',false,0,0)" title="Close" style="float:right" />
	</div>
	<p>
	<asp:Literal runat="server" id="X105046">
	Bookings for this service are not yet available.
	<br /><br />
	Please contact customer support for more information.
	</asp:Literal>
	</p>
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
