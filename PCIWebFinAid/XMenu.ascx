<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XMenu.ascx.cs" Inherits="PCIWebFinAid.XMenu" %>

<style>
.VBig {
	float: left;
	vertical-align: top;
	padding: 5px;
	margin-right: 8px;
	background-color: black;
	display: inline;
	visibility: visible;
}
.VSmall {
	display: inline-block;
	width: 98%;
}
.VHamburger {
	width: 60px;
	display: none;
	visibility: hidden;
	margin-left: 45%;
	margin-right: 55%;
}
@media screen and (max-width: 600px)
{
	.VBig {
		display: none;
		visibility: hidden;
	}
	.VHamburger {
		display: inline;
		visibility: visible;
	}
}
@media screen and (min-width: 600px)
{
	.VSmall {
		display: none;
		visibility: hidden;
	}
}
.VHead {
	font-weight: bold;
	background-color: orange;
	padding: 5px;
}
.VMenu {
	background-color: #C3C3C3;
	padding: 5px;
}
.VSubMenu {
	background-color: #E3E3E3;
	padding: 3px;
	margin: 0px;
}
.VMenu a {
	color: black;
	text-decoration: none;
}
.VMenu:hover {
	background-color: red;
}
.VMenu a:hover {
	color: white;
	text-decoration: none;
	vertical-align: middle;
}
.VMenuMobi {
	margin-bottom: 2px;
	width: 100%;
	font-size: 24px;
}
.VText {
	font-size: 20px;
	font-weight: bold;
	text-decoration: none;
	color: white;
	display: flex; /* inline-block; */
	height: 75px;
	width: 130px;
	align-items: center;
	justify-content: center;
}
</style>
<script type="text/javascript">
var xActive    = null;
var xSubActive = null;

function XMenu(mID,mShow)
{
	if ( mShow == 0 && xSubActive != null )
		return;

	ShowElt(xActive,false);
	if ( mShow > 0 )
	{
		ShowElt(mID,true);
		ShowElt(xSubActive,false);
	}
	xActive = mID;
}

function XSubMenu(mID,parent)
{
	ShowElt(xSubActive,false);
	xSubActive = null;
	if ( mID  == null )
		return;

	var p = GetElt(mID);
	try
	{
		if ( parent == null )
			ShowElt(p,false);
		else
		{
			var rectBody   = document.body.getBoundingClientRect();
			var rectParent = parent.getBoundingClientRect();
			p.style.top    = ( 8+rectParent.top-rectBody.top).toString() + "px";
			p.style.left   = (12+rectParent.right-rectBody.left).toString() + "px";
			xSubActive     = p;
			ShowElt(p,true);
		}
	}
	catch (x)
	{
		alert(x.message);
	}	
}

function TestPos(obj)
{
	var h = GetElt('divTest');
	if ( h == null || obj == null )
		return;
	var bodyX = document.body.getBoundingClientRect();
	var eltX  = obj.getBoundingClientRect();
	var p = "[offset] Left="+obj.offsetLeft.toString()
	    + ", Top="+obj.offsetTop.toString()
	    + ", Width="+obj.offsetWidth.toString()
	    + ", Height="+obj.offsetHeight.toString();
	var q = "[body] Left="+bodyX.left.toString()
	    + ", Top="+bodyX.top.toString()
	    + ", Right="+bodyX.right.toString()
	    + ", Bottom="+bodyX.bottom.toString();
	var r = "[td] Left="+eltX.left.toString()
	    + ", Top="+eltX.top.toString()
	    + ", Right="+eltX.right.toString()
	    + ", Bottom="+eltX.bottom.toString();
	alert(p);
	alert(q);
	alert(r);
//	h.style.top  = obj.offsetTop.toString() + "px";
//	h.style.left = (obj.offsetLeft + obj.offsetWidth).toString() + "px";
	h.style.top  = (eltX.top-bodyX.top).toString() + "px";
	h.style.left = (3+eltX.right-bodyX.left).toString() + "px";
	ShowElt(h,true);
}
function MobileMenu(mID)
{
	if ( mID == null )
		mID = 'menuSmall';
	var p = GetElt(mID);
	if ( p.style.display.length == 0 )
	{
		p.style.display    = 'none';
		p.style.visibility = "hidden";
	}
	else
	{
		p.style.display    = '';
		p.style.visibility = "visible";
	}
}
</script>

<a href="JavaScript:MobileMenu()"><img src="<%=PCIBusiness.Tools.ImageFolder() %>HamburgerMenu.png" class="VHamburger" title="Show menu" /></a>
<asp:Literal runat="server" ID="lblMenuL"></asp:Literal>
<asp:Literal runat="server" ID="lblMenuS"></asp:Literal>