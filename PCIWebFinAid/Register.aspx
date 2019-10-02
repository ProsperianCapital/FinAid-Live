<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Register.aspx.cs" Inherits="PCIWebFinAid.Register" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>FinAid : Register</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="gfx/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<form id="frmRegister" runat="server">

<script type="text/javascript">
var firstPage = 1;
var lastPage  = 5;
var pageNo;

function OptSelect()
{
	try
	{
		var p = GetListValue('lstOptions');
		var h = '';
		if ( p == 2 ) // Bronze
			h = 'PRODUCT NAME: BRONZE<br /><br />Up To $150 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $14.95';
		else if ( p == 3 ) // Silver
			h = 'PRODUCT NAME: SILVER<br /><br />Up To $200 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $19.95';
		else if ( p == 4 ) // Gold
			h = 'PRODUCT NAME: GOLD<br /><br />Up To $300 CA$HBack<br />Your annual registration fee is equal to 1<br />month’s subscription fee<br />Monthly Fee: $29.95';
		SetEltValue('lblInfo4',h);
	}
	catch (x)
	{ }
}

function NextPage(inc)
{
	try
	{
//		alert("pageNo="+pageNo.toString()+", inc="+inc.toString());

		if ( inc > 0 )
			return ValidatePage(0,2);
		else if ( inc < 0 && pageNo > firstPage )
			pageNo--;
		else if ( inc != 0 )
			return false;

//		if ( inc > 0 && pageNo < lastPage )
//			pageNo++;
//		else if ( inc < 0 && pageNo > firstPage )
//			pageNo--;
//		else if ( inc != 0 )
//			return false;

		SetEltValue('hdnPageNo',pageNo.toString());

		ShowElt('divP01'  ,pageNo==1);
		ShowElt('divP02'  ,pageNo==2);
		ShowElt('divP03'  ,pageNo==3);
		ShowElt('divP04'  ,pageNo==4);
		ShowElt('divP05'  ,pageNo==5);
		ShowElt('btnBack' ,pageNo> firstPage);
		ShowElt('btnNext' ,pageNo< lastPage);
		ShowElt('btnAgree',pageNo==lastPage);
		ShowElt('btnBack2',pageNo==lastPage);

		if ( pageNo == firstPage )
			ShowElt('btnNext',GetElt('chkAgree').checked);

		else if ( pageNo == lastPage )
		{
			var dt = new Date();
			var dd = dt.getDate();
			var mm = dt.getMonth()+1;
			var yy = dt.getFullYear();
			if ( dd > 9 )
				dd = dd.toString();
			else
				dd = '0'+dd.toString();
			if ( mm > 9 )
				mm = mm.toString();
			else
				mm = '0'+mm.toString();
			yy = yy.toString();
		//	SetEltValue('lblCCDueDate',yy+'/'+mm+'/'+dd);
		}
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
function ValidatePage(ctl,seq)
{
	var err = "";
	var p;

	try
	{
	//	Page 1
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100111 )
		{
			p   = Validate('lstTitle','lblInfo1',3,hdnTitleError.value,73,0);
			err = err + p;
			ShowTick(p,'Title',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100114 )
		{
			p   = Validate('txtSurname','lblInfo1',1,hdnSurnameError.value,2,2);
			err = err + p;
			ShowTick(p,'Surname',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100117 )
		{
			p   = Validate('txtCellNo','lblInfo1',7,hdnCellNoError.value,0,0);
			err = err + p;
			ShowTick(p,'CellNo',seq);
		}

	//	Page 2
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100112 )
		{
			p   = Validate('txtFirstName','lblInfo2',1,hdnFirstNameError.value,2,1);
			err = err + p;
			ShowTick(p,'FirstName',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100116 )
		{
			p   = Validate('txtEMail','lblInfo2',5,hdnEMailError.value);
			err = err + p;
			ShowTick(p,'EMail',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100118 )
		{
			p   = Validate('txtID','lblInfo2',6,hdnIDError.value,7,20);
			err = err + p;
			ShowTick(p,'ID',seq);
		}

	//	Page 3
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100123 )
		{
			p   = Validate('txtIncome','lblInfo3',6,hdnIncomeError.value,3,1000);
			err = err + p;
			ShowTick(p,'Income',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100131 )
		{
			p   = Validate('lstStatus','lblInfo3',3,hdnStatusError.value,73,0);
			err = err + p;
			ShowTick(p,'Status',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100132 )
		{
			p   = Validate('lstPayDay','lblInfo3',3,hdnPayDayError.value,73,0);
			err = err + p;
			ShowTick(p,'PayDay',seq);
		}

	//	Page 4
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100138 )
		{
			p   = Validate('lstOptions','lblInfo4',3,hdnOptionsError.value,73,0);
			err = err + p;
			ShowTick(p,'Options',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100144 )
		{
			p   = Validate('chkTerms','lblInfo4',8,hdnTermsError.value,2);
			err = err + p;
			ShowTick(p,'Terms',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100147 )
		{
			p   = Validate('lstPayment','lblInfo4',3,hdnPaymentError.value,73,0);
			err = err + p;
			ShowTick(p,'Payment',seq);
		}

	//	Page 5
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100187 )
		{
			p   = Validate('txtCCNumber','lblInfo5',6,hdnCCNumberError.value,8,14);
			err = err + p;
			ShowTick(p,'CCNumber',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100186 )
		{
			p   = Validate('txtCCName','lblInfo5',1,hdnCCNameError.value,2,2);
			err = err + p;
			ShowTick(p,'CCName',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100188 )
		{
			p    = Validate('lstCCMonth','lblInfo5',3,hdnCCExpiryError.value,73,0)
			     + Validate('lstCCYear' ,'lblInfo5',3,hdnCCExpiryError.value,73,0);
			err  = err + p;
			if ( p.length == 0 )
			{
				p = new Date();
				if ( p.getFullYear() > GetListValue('lstCCYear') )
					p = 'Invalid card expiry date';
				else if ( p.getFullYear() == GetListValue('lstCCYear') && p.getMonth()+1 > GetListValue('lstCCMonth') )
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
			p   = Validate('txtCCCVV','lblInfo5',6,hdnCCCVVError.value,7,4);
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
</script>

<asp:HiddenField runat="server" id="hdnPageNo" value="1" />

<div class="Header3">Registration</div>

<div id="divP01">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead1Label">Welcome</asp:Literal>
</p><p>
<asp:Literal runat="server" ID="lbl104397"></asp:Literal>
</p><p>
<asp:CheckBox runat="server" ID="chkAgree" onclick="JavaScript:ShowElt('btnNext',this.checked)" />
<asp:Literal runat="server" ID="lbl104398"></asp:Literal>
</p>
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblTitleLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstTitle" onfocus="JavaScript:ValidatePage(100111,1)" onblur="JavaScript:ValidatePage(100111,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Title')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgTitle" />
			<asp:HiddenField runat="server" ID="hdnTitleHelp" />
			<asp:HiddenField runat="server" ID="hdnTitleError" />
			<asp:HiddenField runat="server" ID="hdnTitleGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo1" style="text-align:center"></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblSurnameLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtSurname" onfocus="JavaScript:ValidatePage(100114,1)" onblur="JavaScript:ValidatePage(100114,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Surname')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgSurname" />
			<asp:HiddenField runat="server" ID="hdnSurnameHelp" />
			<asp:HiddenField runat="server" ID="hdnSurnameError" />
			<asp:HiddenField runat="server" ID="hdnSurnameGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCellNoLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCellNo" onfocus="JavaScript:ValidatePage(100117,1)" onblur="JavaScript:ValidatePage(100117,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CellNo')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCellNo" />
			<asp:HiddenField runat="server" ID="hdnCellNoHelp" />
			<asp:HiddenField runat="server" ID="hdnCellNoError" />
			<asp:HiddenField runat="server" ID="hdnCellNoGuide" /></td></tr>
</table>
</div>

<div id="divHelp" class="PopupBox" style="visibility:hidden"></div>

<div id="divP02">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead2Label"></asp:Literal>
</p>
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblFirstNameLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtFirstName" onfocus="JavaScript:ValidatePage(100112,1)" onblur="JavaScript:ValidatePage(100112,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'FirstName')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgFirstName" />
			<asp:HiddenField runat="server" ID="hdnFirstNameHelp" />
			<asp:HiddenField runat="server" ID="hdnFirstNameError" />
			<asp:HiddenField runat="server" ID="hdnFirstNameGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo2" style="text-align:center"></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblEMailLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtEMail" onfocus="JavaScript:ValidatePage(100116,1)" onblur="JavaScript:ValidatePage(100116,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'EMail')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgEMail" />
			<asp:HiddenField runat="server" ID="hdnEMailHelp" />
			<asp:HiddenField runat="server" ID="hdnEMailError" />
			<asp:HiddenField runat="server" ID="hdnEMailGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblIDLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtID" onfocus="JavaScript:ValidatePage(100118,1)" onblur="JavaScript:ValidatePage(100118,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'ID')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
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
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblIncomeLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtIncome" onfocus="JavaScript:ValidatePage(100123,1)" onblur="JavaScript:ValidatePage(100123,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Income')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgIncome" />
			<asp:HiddenField runat="server" ID="hdnIncomeHelp" />
			<asp:HiddenField runat="server" ID="hdnIncomeError" />
			<asp:HiddenField runat="server" ID="hdnIncomeGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo3" style="text-align:center"></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblStatusLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstStatus" onfocus="JavaScript:ValidatePage(100131,1)" onblur="JavaScript:ValidatePage(100131,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Status')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgStatus" />
			<asp:HiddenField runat="server" ID="hdnStatusHelp" />
			<asp:HiddenField runat="server" ID="hdnStatusError" />
			<asp:HiddenField runat="server" ID="hdnStatusGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblPayDayLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayDay" onfocus="JavaScript:ValidatePage(100132,1)" onblur="JavaScript:ValidatePage(100132,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'PayDay')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgPayDay" />
			<asp:HiddenField runat="server" ID="hdnPayDayHelp" />
			<asp:HiddenField runat="server" ID="hdnPayDayError" />
			<asp:HiddenField runat="server" ID="hdnPayDayGuide" /></td></tr>
</table>
</div>

<div id="divP04">
<p class="Header4">
<asp:Literal runat="server" ID="lblSubHead4aLabel"></asp:Literal> <!-- Congratulations! Your product options are: -->
</p><p class="Header5">
<asp:Literal runat="server" ID="lblSubHead4bLabel"></asp:Literal>
</p>
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblOptionsLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstOptions" onfocus="JavaScript:ValidatePage(100138,1)" onblur="JavaScript:ValidatePage(100138,2)" onchange="JavaScript:OptSelect()">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Options')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgOptions" />
			<asp:HiddenField runat="server" ID="hdnOptionsHelp" />
			<asp:HiddenField runat="server" ID="hdnOptionsError" />
			<asp:HiddenField runat="server" ID="hdnOptionsGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo4" style="text-align:center"></td></tr>
	<tr>
		<td colspan="4" class="Header4">
			<br /><asp:Literal runat="server" ID="lblSubHead4cLabel"></asp:Literal><br />&nbsp;</td></tr>
	<tr>
		<td style="white-space:nowrap" colspan="2">
			<asp:CheckBox runat="server" ID="chkTerms" onclick="JavaScript:ValidatePage(100144,2)" />
			<asp:Literal runat="server" ID="lblTermsLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Terms')" onmouseout="JavaScript:Help(0)">?</a><br />&nbsp;</td>
		<td>
			<img id="imgTerms" />
			<asp:HiddenField runat="server" ID="hdnTermsHelp" />
			<asp:HiddenField runat="server" ID="hdnTermsError" />
			<asp:HiddenField runat="server" ID="hdnTermsGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblPaymentLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayment" onfocus="JavaScript:ValidatePage(100147,1)" onblur="JavaScript:ValidatePage(100147,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Payment')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
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
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCNumberLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCNumber" MaxLength="20" onfocus="JavaScript:ValidatePage(100187,1)" onblur="JavaScript:ValidatePage(100187,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCNumber')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCNumber" />
			<asp:HiddenField runat="server" ID="hdnCCNumberHelp" />
			<asp:HiddenField runat="server" ID="hdnCCNumberError" />
			<asp:HiddenField runat="server" ID="hdnCCNumberGuide" /></td>
		<td class="Error" rowspan="3" id="lblInfo5" style="text-align:center"></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCNameLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCName" onfocus="JavaScript:ValidatePage(100186,1)" onblur="JavaScript:ValidatePage(100186,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCName')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCName" />
			<asp:HiddenField runat="server" ID="hdnCCNameHelp" />
			<asp:HiddenField runat="server" ID="hdnCCNameError" />
			<asp:HiddenField runat="server" ID="hdnCCNameGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCExpiryLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCMonth" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)">
				<asp:ListItem Value= "0" Text="(Select one)"></asp:ListItem>
				<asp:ListItem Value= "1" Text="01 (January)"></asp:ListItem>
				<asp:ListItem Value= "2" Text="02 (February)"></asp:ListItem>
				<asp:ListItem Value= "3" Text="03 (March)"></asp:ListItem>
				<asp:ListItem Value= "4" Text="04 (April)"></asp:ListItem>
				<asp:ListItem Value= "5" Text="05 (May)"></asp:ListItem>
				<asp:ListItem Value= "6" Text="06 (June)"></asp:ListItem>
				<asp:ListItem Value= "7" Text="07 (July)"></asp:ListItem>
				<asp:ListItem Value= "8" Text="08 (August)"></asp:ListItem>
				<asp:ListItem Value= "9" Text="09 (September)"></asp:ListItem>
				<asp:ListItem Value="10" Text="10 (October)"></asp:ListItem>
				<asp:ListItem Value="11" Text="11 (November)"></asp:ListItem>
				<asp:ListItem Value="12" Text="12 (December)"></asp:ListItem>
			</asp:DropDownList>
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCYear" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)"></asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCExpiry')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCExpiry" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryHelp" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryError" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCCVVLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCCVV" MaxLength="4" onfocus="JavaScript:ValidatePage(100189,1)" onblur="JavaScript:ValidatePage(100189,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCCVV')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCCVV" />
			<asp:HiddenField runat="server" ID="hdnCCCVVHelp" />
			<asp:HiddenField runat="server" ID="hdnCCCVVError" />
			<asp:HiddenField runat="server" ID="hdnCCCVVGuide" /></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCDueDayLabel"></asp:Literal></td>
		<td style="white-space:nowrap;font-weight:bold" colspan="3">
			<asp:Label runat="server" id="lblCCDueDate"></asp:Label></td></tr>
	<tr>
		<td colspan="2">
			</td>
	</tr>
</table>
<div style="background-color:lightgray">
	<div style="color:orange;font-weight:bold;font-size:18px;padding:10px">
	COLLECTION MANDATE: <asp:Literal runat="server" ID="lblCCMandateDate"></asp:Literal>
	</div>
	<asp:Literal runat="server" ID="lblCCMandate"></asp:Literal><br />&nbsp;
</div>
</div>

<br />
<input type="button" id="btnBack"  value="<< BACK" onclick="JavaScript:NextPage(-1)" />
<asp:Button runat="server" ID="btnNext"  OnClick="btnNext_Click" OnClientClick="JavaScript:return NextPage(1)" Text="NEXT >>" />
<asp:Button runat="server" ID="btnAgree" OnClick="btnNext_Click" OnClientClick="JavaScript:return NextPage(1)" Text="I Agree" />
<input type="button" id="btnBack2" value="Change Payment Method" onclick="JavaScript:NextPage(-1)" style="width:175px" />
<br /><br />

<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<asp:Label runat="server" ID="lblVer" style="position:fixed;bottom:3px;right:5px"></asp:Label>

<script type="text/javascript">
pageNo = GetEltValueInt('hdnPageNo');
</script>

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>
</body>
</html>
