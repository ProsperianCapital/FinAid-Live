<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="RegisterEx3.aspx.cs" Inherits="PCIWebFinAid.RegisterEx3" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Secure Registration</title>
	<link rel="preload" href="CSS/FinAid.css" as="style" />
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<!--#include file="IncludeMainSimple.htm" -->
</head>
<body>

<script type="text/javascript">
var firstPage = 1;
var lastPage  = 5;
var confPage  = 6;
var pageNo;

function NoDataError()
{
// Add code as necessary
}

function NextPage(inc,btn)
{
	try
	{
		DisableElt(btn,true); // Disable button immediately
		var ret = "X";

		if ( inc > 0 )
			ret = ( ValidatePage(0,2) ? 'T' : 'F' );
		else if ( inc < 0 && pageNo > firstPage )
			pageNo--;
		else if ( inc != 0 )
			ret = 'F';

		if ( ret == 'T' ) // Return true, leave the button DISABLED
			return true;

		DisableElt(btn,false);

		if ( ret == 'F' ) // Return false, ENABLE the button
			return false;

		SetEltValue('hdnPageNo',pageNo.toString());

		ShowElt('divP01'  ,pageNo==1);
		ShowElt('divP02'  ,pageNo==2);
		ShowElt('divP03'  ,pageNo==3);
		ShowElt('divP04'  ,pageNo==4);
		ShowElt('divP05'  ,pageNo==5);
		ShowElt('divP06'  ,pageNo==6);
		ShowElt('btnAgree',pageNo==lastPage  && pageNo!=confPage);
		ShowElt('btnBack2',pageNo==lastPage  && pageNo!=confPage);
		ShowElt('btnBack1',pageNo> firstPage && pageNo!=confPage);
		ShowElt('btnNext' ,pageNo< lastPage  && pageNo!=confPage);

		if ( pageNo == firstPage )
			ShowElt('btnNext',GetElt('chkAgree').checked);
	}
	catch (x)
	{
		alert(x.message);
		return false;
	}
	return true;
}
function ShowTick(err,ctl,seq)
{
	var h = '';
	if ( seq == 1 )
		h = GetEltValue('hdn'+ctl+'Guide');
	else if ( seq == 2 )
	{
		GetElt('img'+ctl).src = 'Images/' + ( err.length > 0 ? 'Cross' : 'Tick' ) + '.png';
		if ( err.length > 0 )
			h = GetEltValue('hdn'+ctl+'Error');
	}
	SetEltValue('lblInfo'+pageNo.toString(),h);
}
function ValidatePage(ctl,seq,misc)
{
	var err = "";
	var p;

	try
	{
	//	Page 1
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100111 )
		{
			p   = Validate('lstTitle','lblInfo1',3,GetEltValue('hdnTitleError'),73,0);
			err = err + p;
			ShowTick(p,'Title',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100114 )
		{
			p   = Validate('txtSurname','lblInfo1',1,GetEltValue('hdnSurnameError'),2,2);
			err = err + p;
			ShowTick(p,'Surname',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100117 )
		{
			p   = Validate('txtCellNo','lblInfo1',7,GetEltValue('hdnCellNoError'),0,0);
			err = err + p;
			ShowTick(p,'CellNo',seq);
		}

	//	Page 2
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100112 )
		{
			p   = Validate('txtFirstName','lblInfo2',1,GetEltValue('hdnFirstNameError'),2,1);
			err = err + p;
			ShowTick(p,'FirstName',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100116 )
		{
			p   = Validate('txtEMail','lblInfo2',5,GetEltValue('hdnEMailError'));
			err = err + p;
			ShowTick(p,'EMail',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100118 )
		{
		//	p   = Validate('txtID','lblInfo2',6,GetEltValue('hdnIDError'),7,20);
			p   = Validate('txtID','lblInfo2',1,GetEltValue('hdnIDError'),2,3);
			err = err + p;
			ShowTick(p,'ID',seq);
		}

	//	Page 3
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100123 )
		{
			p   = Validate('txtIncome','lblInfo3',6,GetEltValue('hdnIncomeError'),3,100);
			err = err + p;
			ShowTick(p,'Income',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100131 )
		{
			p   = Validate('lstStatus','lblInfo3',3,GetEltValue('hdnStatusError'),73,0);
			err = err + p;
			ShowTick(p,'Status',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100132 )
		{
			p   = Validate('lstPayDay','lblInfo3',3,GetEltValue('hdnPayDayError'),73,0);
			err = err + p;
			ShowTick(p,'PayDay',seq);
		}

	//	Page 4
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100138 )
		{
			p   = Validate('lstOptions','lblInfo4',3,GetEltValue('hdnOptionsError'),73,0);
			err = err + p;
			ShowTick(p,'Options',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100144 )
		{
			p   = Validate('chkTerms','lblInfo4',8,GetEltValue('hdnTermsError'),2);
			err = err + p;
			ShowTick(p,'Terms',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 104429 )
		{
			p   = Validate('chkRewards','lblInfo4',8,GetEltValue('hdnRewardsError'),2);
			err = err + p;
			ShowTick(p,'Rewards',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100147 )
		{
			p   = Validate('lstPayment','lblInfo4',3,GetEltValue('hdnPaymentError'),73,0);
			err = err + p;
			ShowTick(p,'Payment',seq);
		}

	//	Page 5

		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100187 )
			if ( GetElt('txtCCNumber') != null )
			{
				if ( GetEltValue('hdn100187') == 'Y' ) // Validate card number using Luhn check digit
					p = Validate('txtCCNumber','lblInfo5',9,GetEltValue('hdnCCNumberError'));
				else
					p = Validate('txtCCNumber','lblInfo5',6,GetEltValue('hdnCCNumberError'),8,14);
				err  = err + p;
				ShowTick(p,'CCNumber',seq);
			}

		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100186 )
		{
			p   = Validate('txtCCName','lblInfo5',1,GetEltValue('hdnCCNameError'),2,2);
			err = err + p;
			ShowTick(p,'CCName',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100188 )
		{
			p    = Validate('lstCCMonth','lblInfo5',3,GetEltValue('hdnCCExpiryError'),73,0)
			     + Validate('lstCCYear' ,'lblInfo5',3,GetEltValue('hdnCCExpiryError'),73,0);
			err  = err + p;
			if ( p.length == 0 )
			{
				p = new Date();
				if ( p.getFullYear() > GetListValueInt('lstCCYear') )
					p = 'Invalid card expiry date';
				else if ( p.getFullYear() == GetListValueInt('lstCCYear') && p.getMonth()+1 > GetListValueInt('lstCCMonth') )
					p = 'Invalid card expiry date';
				else
					p = '';
				err  = err + p;
				SetErrorLabel('lblInfo5',p.length,p);
			}
			ShowTick(p,'CCExpiry',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100189 )
		{
			p   = Validate('txtCCCVV','lblInfo5',6,GetEltValue('hdnCCCVVError'),44,0);
			err = err + p;
			ShowTick(p,'CCCVV',seq);
		}
	}
	catch (x)
	{
		alert(x.message);
	}
	return ( err.length == 0 );
}
function Help(onOrOff,ctl,item)
{
	try
	{
		if ( onOrOff > 0 )
		{
			var h = GetEltValue('hdn'+item+'Help');
			ShowPopup('divHelp',h,null,null,ctl);
		}
		else
			ShowElt('divHelp',false);
	}
	catch (x)
	{ }
}
function OptSelect(p)
{
	var h = '';
	if ( p == null || p < 2 || p > 4 )
		p = GetListValueInt('lstOptions');
	h = GetEltValue('hdnOption'+p.toString());
	if ( h.length < 1 )
		if ( p == 2 )
			h = 'PRODUCT NAME: BRONZE<br /><br />Up To $150 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $14.95';
		else if ( p == 3 )
			h = 'PRODUCT NAME: SILVER<br /><br />Up To $200 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $19.95';
		else if ( p == 4 )
			h = 'PRODUCT NAME: GOLD<br /><br />Up To $300 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $29.95';
	SetEltValue('lblOption',h);
}
function CheckWorldPay()
{
	try
	{
	//	var cc = GetEltValue('txtCCNumber');
	//	var fr = GetElt('ifr3D'); // .contentWindow.document; alert(fr);
//	//	fr = fr.body; alert(fr);

	//	var ifrDoc = fr.contentDocument || fr.contentWindow.document; alert('C01: '+ifrDoc.toString());
	//	var fx = ifrDoc.getElementById('frm3D'); alert('C02: '+fx);

//	//	var frDoc = (fr.contentWindow || fr.contentDocument); alert(frDoc);
//	//	if (frDoc.document) frDoc = frDoc.document; alert(frDoc);
	//	alert(fr.toString());
	//	var fm = fr.getElementsByTagName("form"); alert('C03: '+fm.length);
	//	fm = fm[0]; alert('C04: '+fm);
//	//	fm = fr.getElementById("frm3D"); alert(fm);
	//	var b5 = fm.getElementsByName("Bin")[0]; alert(b5);
	//	var b6 = fm.getElementById("Bin"); alert(b6);

//	//	var bi = frDoc.getElementById('Bin'); alert(bi);
//	//	var fm = frDoc.getElementById('frm3D'); alert(fm);
	//	alert('Card number: '+cc);
	//	alert('Bin1: '+bi.value);
	//	bi.value = cc;
	//	alert('Bin2: '+bi.value);
	//	fm.submit();
	//	alert('frm3D submitted');
	}
	catch (x)
	{ }
}
function WorldPay3DS(url,binValue,jwtValue)
{
	try
	{
		alert ('url: '+url+', bin: '+binValue+', jwt: '+jwtValue);
		var frm = document.createElement('form');
		frm.id = 'frmX';
		frm.name = 'frmX';
		frm.method = 'POST';
		frm.target = '_blank';
		frm.action = url;
		alert(frm);

		var bin = document.createElement('input');
		bin.type = 'text';
		bin.id = 'Bin';
		bin.name = 'Bin';
		bin.value = binValue;
		alert(bin);

		var jwt = document.createElement('input');
		jwt.type = 'text';
		jwt.id = 'JWT';
		jwt.name = 'JWT';
		jwt.value = jwtValue;
		alert(jwt);

		frm.appendChild(bin);
		frm.appendChild(jwt);
		frm.submit();
	}
	catch (x)
	{
		alert(x.message);
	}
}
</script>

<form id="frmRegister" runat="server">

<asp:HiddenField runat="server" id="hdnPageNo" value="1" />
<asp:HiddenField runat="server" id="hdn3dTries" />
<asp:HiddenField runat="server" id="hdnBrowser" />
<asp:HiddenField runat="server" id="hdn100002" />
<asp:HiddenField runat="server" id="hdn100137" />
<asp:HiddenField runat="server" id="hdn100187" />
<asp:HiddenField runat="server" ID="hdnJwtToken" />
<asp:HiddenField runat="server" ID="hdnSessionId" />

<div class="Header3">
	<asp:Literal runat="server" ID="lblReg"></asp:Literal><asp:Literal runat="server" ID="lblRegConf"></asp:Literal>
</div>

<div id="divP01">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead1Label">Welcome</asp:Literal>
</p><p>
<asp:Literal runat="server" ID="lbl104397"></asp:Literal>
</p><p>
<asp:CheckBox runat="server" ID="chkAgree" onclick="JavaScript:ShowElt('btnNext',this.checked)" />
<asp:Literal runat="server" ID="lbl104398"></asp:Literal>
</p>
<table style="width:99%">
	<tr id="trTitle">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblTitleLabel"></asp:Literal></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstTitle" onfocus="JavaScript:ValidatePage(100111,1)" onblur="JavaScript:ValidatePage(100111,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Title')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgTitle" />
			<asp:HiddenField runat="server" ID="hdnTitleHelp" />
			<asp:HiddenField runat="server" ID="hdnTitleError" />
			<asp:HiddenField runat="server" ID="hdnTitleGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo1" style="text-align:center;visibility:hidden;display:none"></td></tr>
	<tr id="trSurname">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblSurnameLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtSurname" onfocus="JavaScript:ValidatePage(100114,1)" onblur="JavaScript:ValidatePage(100114,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Surname')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgSurname" />
			<asp:HiddenField runat="server" ID="hdnSurnameHelp" />
			<asp:HiddenField runat="server" ID="hdnSurnameError" />
			<asp:HiddenField runat="server" ID="hdnSurnameGuide" /></td></tr>
	<tr id="trCellNo">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblCellNoLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCellNo" onfocus="JavaScript:ValidatePage(100117,1)" onblur="JavaScript:ValidatePage(100117,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CellNo')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgCellNo" />
			<asp:HiddenField runat="server" ID="hdnCellNoHelp" />
			<asp:HiddenField runat="server" ID="hdnCellNoError" />
			<asp:HiddenField runat="server" ID="hdnCellNoGuide" /></td></tr>
</table>
</div>

<div id="divHelp" class="PopupBox" style="visibility:hidden;width:300px"></div>

<div id="divP02">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead2Label"></asp:Literal>
</p>
<table style="width:99%">
	<tr id="trFirstName">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblFirstNameLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtFirstName" onfocus="JavaScript:ValidatePage(100112,1)" onblur="JavaScript:ValidatePage(100112,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'FirstName')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgFirstName" />
			<asp:HiddenField runat="server" ID="hdnFirstNameHelp" />
			<asp:HiddenField runat="server" ID="hdnFirstNameError" />
			<asp:HiddenField runat="server" ID="hdnFirstNameGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo2" style="text-align:center;visibility:hidden;display:none"></td></tr>
	<tr id="trEMail">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblEMailLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtEMail" onfocus="JavaScript:ValidatePage(100116,1)" onblur="JavaScript:ValidatePage(100116,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'EMail')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgEMail" />
			<asp:HiddenField runat="server" ID="hdnEMailHelp" />
			<asp:HiddenField runat="server" ID="hdnEMailError" />
			<asp:HiddenField runat="server" ID="hdnEMailGuide" /></td></tr>
	<tr id="trID">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblIDLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtID" onfocus="JavaScript:ValidatePage(100118,1)" onblur="JavaScript:ValidatePage(100118,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'ID')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgID" />
			<asp:HiddenField runat="server" ID="hdnIDHelp" />
			<asp:HiddenField runat="server" ID="hdnIDError" />
			<asp:HiddenField runat="server" ID="hdnIDGuide" /></td></tr>
</table>
</div>

<div id="divP03">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead3Label"></asp:Literal>
</p>
<table style="width:99%">
	<tr id="trIncome">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblIncomeLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtIncome" onfocus="JavaScript:ValidatePage(100123,1)" onblur="JavaScript:ValidatePage(100123,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Income')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgIncome" />
			<asp:HiddenField runat="server" ID="hdnIncomeHelp" />
			<asp:HiddenField runat="server" ID="hdnIncomeError" />
			<asp:HiddenField runat="server" ID="hdnIncomeGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo3" style="text-align:center;visibility:hidden;display:none"></td></tr>
	<tr id="trStatus">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblStatusLabel"></asp:Literal></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstStatus" onfocus="JavaScript:ValidatePage(100131,1)" onblur="JavaScript:ValidatePage(100131,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Status')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgStatus" />
			<asp:HiddenField runat="server" ID="hdnStatusHelp" />
			<asp:HiddenField runat="server" ID="hdnStatusError" />
			<asp:HiddenField runat="server" ID="hdnStatusGuide" /></td></tr>
	<tr id="trPayDay">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblPayDayLabel"></asp:Literal></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayDay" onfocus="JavaScript:ValidatePage(100132,1)" onblur="JavaScript:ValidatePage(100132,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'PayDay')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgPayDay" />
			<asp:HiddenField runat="server" ID="hdnPayDayHelp" />
			<asp:HiddenField runat="server" ID="hdnPayDayError" />
			<asp:HiddenField runat="server" ID="hdnPayDayGuide" /></td></tr>
</table>
</div>

<div id="divP04">
<asp:Literal runat="server" ID="lblOptionDescriptions"></asp:Literal>
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead4aLabel"></asp:Literal> <!-- Congratulations! Your product options are: -->
</p><p class="Header5">
<asp:Literal runat="server" ID="lblSubHead4bLabel"></asp:Literal>
</p>
<table style="width:99%">
	<tr id="trOptions">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblOptionsLabel"></asp:Literal></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstOptions" onfocus="JavaScript:ValidatePage(100138,1)" onblur="JavaScript:ValidatePage(100138,2)" onchange="JavaScript:OptSelect()">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Options')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgOptions" />
			<asp:HiddenField runat="server" ID="hdnOptionsHelp" />
			<asp:HiddenField runat="server" ID="hdnOptionsError" />
			<asp:HiddenField runat="server" ID="hdnOptionsGuide" /></td>
		<td class="Error" rowspan="5" id="lblInfo4" style="text-align:center;visibility:hidden;display:none"></td></tr>
	<tr>
		<td id="lblOption" colspan="2"></td></tr>
	<tr>
		<td colspan="2" class="Header4">
			<br /><asp:Literal runat="server" ID="lblSubHead4cLabel"></asp:Literal></td></tr>
	<tr id="trTerms">
		<td style="white-space:nowrap" colspan="2">
			<asp:CheckBox runat="server" ID="chkTerms" onclick="JavaScript:ValidatePage(100144,2)" />
			<asp:Literal runat="server" ID="lblTermsLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Terms')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgTerms" />
			<asp:HiddenField runat="server" ID="hdnTermsHelp" />
			<asp:HiddenField runat="server" ID="hdnTermsError" />
			<asp:HiddenField runat="server" ID="hdnTermsGuide" /></td></tr>
	<tr id="trRewards">
		<td style="white-space:nowrap" colspan="2">
			<asp:CheckBox runat="server" ID="chkRewards" onclick="JavaScript:ValidatePage(104429,2)" />
			<asp:Literal runat="server" ID="lblRewardsLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Rewards')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgRewards" />
			<asp:HiddenField runat="server" ID="hdnRewardsHelp" />
			<asp:HiddenField runat="server" ID="hdnRewardsError" />
			<asp:HiddenField runat="server" ID="hdnRewardsGuide" /></td></tr>
	<tr id="trPayment">
		<td style="white-space:nowrap">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblPaymentLabel"></asp:Literal></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayment" onfocus="JavaScript:ValidatePage(100147,1)" onblur="JavaScript:ValidatePage(100147,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Payment')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgPayment" />
			<asp:HiddenField runat="server" ID="hdnPaymentHelp" />
			<asp:HiddenField runat="server" ID="hdnPaymentError" />
			<asp:HiddenField runat="server" ID="hdnPaymentGuide" /></td></tr>
	<tr>
		<td colspan="4">
			<asp:Literal runat="server" ID="lblSubHead4dLabel"></asp:Literal></td></tr>
</table>
</div>

<div id="divP05">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead5Label"></asp:Literal>
</p>

<table style="width:99%">
	<tr id="trCCNumber">
		<td style="white-space:nowrap">
			<div class="DataLabel">
			<asp:Literal runat="server" ID="lblCCNumberLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCNumber')" onmouseout="JavaScript:Help(0)" style="float:right">?</a></div>
			<asp:PlaceHolder runat="server" ID="pnlTokenNot" Visible="false">
				<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCNumber" MaxLength="20" onfocus="JavaScript:ValidatePage(100187,1)" onblur="JavaScript:ValidatePage(100187,2)"></asp:TextBox>
			</asp:PlaceHolder>
			<img id="imgCCNumber" />
			<asp:PlaceHolder runat="server" ID="pnlTokenEx" Visible="false">
				<span id="txIFrameCC"></span>
			</asp:PlaceHolder>
			<asp:HiddenField runat="server" ID="hdnCCNumberHelp" />
			<asp:HiddenField runat="server" ID="hdnCCNumberError" />
			<asp:HiddenField runat="server" ID="hdnCCNumberGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo5" style="text-align:center;visibility:hidden;display:none"></td></tr>
	<tr id="trCCName">
		<td style="white-space:nowrap">
			<div class="DataLabel">
			<asp:Literal runat="server" ID="lblCCNameLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCName')" onmouseout="JavaScript:Help(0)" style="float:right">?</a></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCName" onfocus="JavaScript:ValidatePage(100186,1)" onblur="JavaScript:ValidatePage(100186,2)"></asp:TextBox>
			<img id="imgCCName" />
			<asp:HiddenField runat="server" ID="hdnCCNameHelp" />
			<asp:HiddenField runat="server" ID="hdnCCNameError" />
			<asp:HiddenField runat="server" ID="hdnCCNameGuide" /></td></tr>
	<tr id="trCCExpiry">
		<td style="white-space:nowrap">
			<div class="DataLabel">
			<asp:Literal runat="server" ID="lblCCExpiryLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCExpiry')" onmouseout="JavaScript:Help(0)" style="float:right">?</a></div>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCMonth" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)">
				<asp:ListItem Value="01" Text="01"></asp:ListItem>
				<asp:ListItem Value="02" Text="02"></asp:ListItem>
				<asp:ListItem Value="03" Text="03"></asp:ListItem>
				<asp:ListItem Value="04" Text="04"></asp:ListItem>
				<asp:ListItem Value="05" Text="05"></asp:ListItem>
				<asp:ListItem Value="06" Text="06"></asp:ListItem>
				<asp:ListItem Value="07" Text="07"></asp:ListItem>
				<asp:ListItem Value="08" Text="08"></asp:ListItem>
				<asp:ListItem Value="09" Text="09"></asp:ListItem>
				<asp:ListItem Value="10" Text="10"></asp:ListItem>
				<asp:ListItem Value="11" Text="11"></asp:ListItem>
				<asp:ListItem Value="12" Text="12"></asp:ListItem>
			</asp:DropDownList>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCYear" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)"></asp:DropDownList>
			<img id="imgCCExpiry" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryHelp" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryError" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryGuide" /></td></tr>
	<tr id="trCCCVV">
		<td style="white-space:nowrap">
			<div class="DataLabel">
			<asp:Literal runat="server" ID="lblCCCVVLabel"></asp:Literal></div>
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCCVV" MaxLength="4" onfocus="JavaScript:ValidatePage(100189,1)" onblur="JavaScript:ValidatePage(100189,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCCVV')" onmouseout="JavaScript:Help(0)">?</a>
			<img id="imgCCCVV" />
			<asp:HiddenField runat="server" ID="hdnCCCVVHelp" />
			<asp:HiddenField runat="server" ID="hdnCCCVVError" />
			<asp:HiddenField runat="server" ID="hdnCCCVVGuide" /></td></tr>
	<tr id="trCCDueDay">
		<td style="white-space:nowrap" colspan="2">
			<div class="DataLabel">
				<asp:Literal runat="server" ID="lblCCDueDayLabel"></asp:Literal></div>
			<div style="font-weight:bold">
				<asp:Label runat="server" id="lblCCDueDate"></asp:Label></div></td></tr>
	<tr>
		<td colspan="2"></td></tr>
</table>
<div style="background-color:lightgray">
	<div style="color:orange;font-weight:bold;font-size:18px;padding:10px">
	<asp:Literal runat="server" ID="lblCCMandateHead"></asp:Literal>
	</div>
	<asp:Literal runat="server" ID="lblCCMandate"></asp:Literal><br />&nbsp;
</div>
</div>

<div id="divP06">
<p class="Header4">
<asp:Literal runat="server" ID="lbl100400"></asp:Literal>
</p><p>
<asp:Literal runat="server" ID="lbl100209"></asp:Literal>
</p>

<!-- 3d Secure -->
<asp:Panel runat="server" id="pnl3d" style="border:1px solid red;background-color:aqua;color:black;padding:10px">

<asp:PlaceHolder runat="server" ID="pnl3d1">
<!-- Providers other than WorldPay -->
<script type='text/javascript'>
var tOut = setTimeout(function(){GetElt('btn3d').click()},10000);
//	Disable "Back"
history.pushState(null, document.title, location.href);
window.addEventListener('popstate', function (event)
{
	history.pushState(null, document.title, location.href);
});
</script>
<asp:Literal runat="server" ID="lbl100500">
PLEASE NOTE: You will shortly be re-directed to your bank's secure payment page to pay your
once-off Card Verification Fee of $0.10 (10 US Cents).
</asp:Literal>
<br /><br />
<asp:Literal runat="server" ID="lbl100501">
If you are not re-directed within 10 seconds, please click the button below to pay the
Card Verification Fee manually.
</asp:Literal>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="pnl3d2">
<!-- WorldPay only -->
<script type='text/javascript'>
var tOut = null;
function ThreeD2(stepNo,sId)
{
//	alert('ThreeD2('+stepNo.toString()+',"'+sId+'")');
	ShowElt('pnl3d2Step1',(stepNo==1));
	ShowElt('pnl3d2Step2',(stepNo==2));
	ShowElt('btn3dWait',  (stepNo==1));
	ShowElt('btn3d',      (stepNo==2));
	SetEltValue('hdnSessionId',sId);
}
ThreeD2(1,'');
</script>
<div id="pnl3d2Step1">
Please wait while we connect to our payment provider to verify your card ...
<br /><br />
<img src="<%=PCIBusiness.Tools.ImageFolder() %>Busy.gif" />
</div>
<div id="pnl3d2Step2">
<b>Card successfully verified.</b>
<br /><br />
Please click the button below to pay the Card Verification Fee.
</div>
</asp:PlaceHolder>

<br />
<input type="button" id="btn3dWait" value="Wait ..." disabled="disabled" style="visibility:hidden;display:none" />
<asp:Button runat="server" ID="btn3d" Text="Pay Now" UseSubmitBehavior="false" OnClick="btn3d_Click" OnClientClick="JavaScript:clearTimeout(tOut);DisableElt(this,true);" />
</asp:Panel>
<br />
<!-- 3d Secure -->

<table class="Confirmation" style="width:99%">
	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100372"></asp:Literal></td></tr>
	<tr>
		<td><asp:Literal runat="server" ID="lbl100210"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Ref"></asp:Literal></td></tr>
	<tr>
		<td><asp:Literal runat="server" ID="lbl100211"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Pin"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100212"></asp:Literal></td></tr>
	<tr id="trp6Title">
		<td><asp:Literal runat="server" ID="lbl100111"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Title"></asp:Literal></td></tr>
	<tr id="trp6FirstName">
		<td><asp:Literal runat="server" ID="lbl100214"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6FirstName"></asp:Literal></td></tr>
	<tr id="trp6Surname">
		<td><asp:Literal runat="server" ID="lbl100216"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Surname"></asp:Literal></td></tr>
	<tr id="trp6EMail">
		<td><asp:Literal runat="server" ID="lbl100218"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6EMail"></asp:Literal></td></tr>
	<tr id="trp6CellNo">
		<td><asp:Literal runat="server" ID="lbl100219"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6CellNo"></asp:Literal></td></tr>
	<tr id="trp6ID">
		<td><asp:Literal runat="server" ID="lbl100220"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6ID"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lbl100373"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100222"></asp:Literal></td></tr>
	<tr id="trp6Income">
		<td><asp:Literal runat="server" ID="lbl100223"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Income"></asp:Literal></td></tr>
	<tr id="trp6Status">
		<td><asp:Literal runat="server" ID="lbl100230"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Status"></asp:Literal></td></tr>
	<tr id="trp6PayDay">
		<td><asp:Literal runat="server" ID="lbl100231"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6PayDay"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lbl100374"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100233"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Label runat="server" ID="lbl100325"></asp:Label></td></tr>
	<tr id="trp6Payment">
		<td><asp:Literal runat="server" ID="lbl100236"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Payment"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lbl100237"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

<!-- OLD
	<tr>
		<td colspan="2" class="Header5"><asp Literal run@t="server" ID="lbl100238"></asp Literal></td></tr>
	<tr>
		<td colspan="2"><asp Literal run@t="server" ID="lblp6Agreement"></asp Literal></td></tr>
	<tr><td>&nbsp;</td></tr>
-->

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100238"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6RefundPolicy"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6MoneyBackPolicy"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6CancellationPolicy"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100184"></asp:Literal></td></tr>
	<tr id="trp6CCType">
		<td><asp:Literal runat="server" ID="lbl100185"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6CCType"></asp:Literal></td></tr>
	<tr id="trp6CCName">
		<td><asp:Literal runat="server" ID="lbl100186"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6CCName"></asp:Literal></td></tr>
	<tr id="trp6CCNumber">
		<td><asp:Literal runat="server" ID="lbl100187"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6CCNumber"></asp:Literal></td></tr>
	<tr id="trp6CCExpiry">
		<td><asp:Literal runat="server" ID="lbl100188"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6CCExpiry"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6Billing"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lblp6MandateHead"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6Mandate"></asp:Literal></td></tr>
	<tr><td>&nbsp;</td></tr>

	<tr>
		<td colspan="2" class="Header5"><asp:Literal runat="server" ID="lbl100259"></asp:Literal></td></tr>
	<tr>
		<td><asp:Literal runat="server" ID="lbl100375"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6Date"></asp:Literal></td></tr>
	<tr>
		<td><asp:Literal runat="server" ID="lbl100376"></asp:Literal></td>
		<td><asp:Literal runat="server" ID="lblp6IP"></asp:Literal></td></tr>
	<tr>
		<td colspan="2"><asp:Literal runat="server" ID="lblp6Browser"></asp:Literal></td></tr>
</table>
</div>
<br />

<asp:Panel runat="server" ID="pnlDisabled" CssClass="PopupHead">
Disabled
</asp:Panel>

<asp:Button runat="server" ID="btnBack1" UseSubmitBehavior="false" OnClientClick="JavaScript:if (!NextPage(-1,this)) return false;" Text="BACK" />
<asp:Button runat="server" ID="btnNext"  UseSubmitBehavior="false" OnClientClick="JavaScript:if (!NextPage( 1,this)) return false;" OnClick="btnNext_Click" />
<asp:Button runat="server" ID="btnAgree" UseSubmitBehavior="false" OnClientClick="JavaScript:if (!NextPage( 1,this)) return false;if (!TokenFinish()) return false;" OnClick="btnNext_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button runat="server" ID="btnBack2" UseSubmitBehavior="false" OnClientClick="JavaScript:if (!NextPage(-1,this)) return false" Width="200px" />
&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button runat="server" ID="btnErrorDtl" Text="Error ...?" OnClientClick="JavaScript:ShowElt('lblErrorDtl',true);return false" />
<br /><br />

<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
<asp:Label runat="server" ID="lblErrorDtl" style="border:1px solid #000000;position:fixed;bottom:20px;right:5px;visibility:hidden;display:none;padding:5px;font-family:Verdana;background-color:pink"></asp:Label>
<asp:Label runat="server" ID="lblVer" style="position:fixed;bottom:3px;right:5px"></asp:Label>

<asp:Panel runat="server" ID="pnlWarning" Visible="false" CssClass="Popup2">
<div class="PopupHead">Warning</div>
<p>
<asp:Literal runat="server" id="lblWarnX"></asp:Literal>
</p><p>
<asp:Literal runat="server" ID="lblWarnP">But you may continue anyway. Press "OK" to proceed.</asp:Literal>
<asp:Literal runat="server" ID="lblWarnB">We're sorry, but you cannot continue.</asp:Literal>
</p>
<hr />
<input type="button" value="OK" onclick="JavaScript:ShowElt('pnlWarning',false);return false" />
</asp:Panel>

<asp:HiddenField runat="server" ID="hdnVer" />

<asp:Literal runat="server" id="lblChat"></asp:Literal>

<script type="text/javascript">
pageNo = GetEltValueInt('hdnPageNo');
SetEltValue('hdnBrowser',navigator.userAgent.toString());
</script>

<!-- TokenEx start -->

<asp:Literal runat="server" ID="txScript"></asp:Literal>

<!-- Above is a place holder for the TokenEx iFrame JavaScript source. Test version would look like
[lt]script src="https://test-htp.tokenex.com/iframe/iframe-v3.min.js"
-->

<asp:HiddenField runat="server" ID="txHMAC" />
<asp:HiddenField runat="server" ID="txToken" />
<asp:HiddenField runat="server" ID="txReference" />
<asp:HiddenField runat="server" ID="txTimestamp" />
<asp:HiddenField runat="server" ID="txOrigin" />
<asp:HiddenField runat="server" ID="txID" />
<asp:HiddenField runat="server" ID="txTokenScheme" Value="sixTOKENfour" />

<script type="text/javascript">
var txFrame;
var txCC;

frmRegister.action = 'RegisterEx3.aspx';

function TokenFinish()
{
	if ( GetElt('txIFrameCC') == null )
		return true;

//	ALL returns other then the one above must be FALSE
//	Only return TRUE if TokenEx is turned OFF (ie. the iFrame object is NULL)

	var err = 'Invalid card number and/or CVV';

	try
	{
		var v = GetEltValue('txtCCCVV');
		if ( txCC.isValid && v.length >= 3 && ToInteger(v) > 0 )
		{
			txFrame.tokenize();
			return false; // This MUST be false!
		}
	}
	catch (x)
	{
		err = err + ' (exception: ' + x.message + ')';
	}

	DisableElt('btnAgree',false);
	alert(err);
	return false;
}
function TokenSetup()
{
	var txConfig = {
		styles: {
			base: "background-color:#898787;width:209px;border:0;padding:2px",
			focus: "",
			error: "border-color:#ce0a0a"
		},
		pci: true,
		inputType: "text",
		enablePrettyFormat: true,
		debug: false,
		placeholder: "Card Number",
//	Required
		origin: document.getElementById("txOrigin").value,
		timestamp: document.getElementById("txTimestamp").value,
		tokenExID: document.getElementById("txID").value,
		tokenScheme: document.getElementById("txTokenScheme").value,
		authenticationKey: document.getElementById("txHMAC").value,
		enableValidateOnBlur: true
	};

	try
	{
//		alert('TX/1');
		txFrame = new TokenEx.Iframe("txIFrameCC", txConfig);
//		alert(txFrame);
//		txFrame.on("load", function() { alert('TX/2'); });
//		txFrame.on("focus", function() { util.log("Iframe focus") });
//		txFrame.on("blur", function() { util.log("Iframe blur") });
//		txFrame.on("change", function() { txCC = null; });
		txFrame.on("validate", function (data) { txCC = data; });
//		txFrame.on("cardTypeChange", function (data) { util.log("Iframe cardTypeChange:" + JSON.stringify(data)); });
		txFrame.on("tokenize", function (data) { if (data.token) { SetEltValue('lblError','');
																					  SetEltValue('txToken',data.token);
																					  SetEltValue('txReference',data.referenceNumber);
																					  frmRegister.action = 'RegisterEx3.aspx?PageNo=6'
																					  frmRegister.submit(); } });
		txFrame.on("error", function (data) { SetEltValue('txToken','');SetEltValue('txReference','');SetEltValue('lblError',data.error) }); //major error occured
//		alert('TX/4');
		txFrame.load();
//		alert('TX/5');
	}
	catch (x)
	{
		alert(x.message);
	}
}
</script>
<asp:Literal runat="server" ID="lblTx"></asp:Literal>
<!-- TokenEx end -->

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>

<asp:Literal runat="server" ID="lblJwtIframe"></asp:Literal>

<!-- Testing -->
<!--
<div id="t01" style="border:1px solid #000000;margin:5px"></div>
<br />
<div id="t02" style="border:1px solid #000000;margin:5px"></div>
<br />
<div id="t03" style="border:1px solid #000000;margin:5px"></div>
-->
<!-- Testing -->

</body>
</html>