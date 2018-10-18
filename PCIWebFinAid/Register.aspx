<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Register.aspx.cs" Inherits="PCIWebFinAid.Register" %>

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
<asp:Literal runat="server" ID="lblAppDetails"></asp:Literal>
<form id="frmRegister" runat="server">

<script type="text/javascript">
var firstPage = 1;
var lastPage  = 5;
var pageNo    = firstPage;

function NextPage(inc)
{
	try
	{
		if ( inc > 0 && ! ValidatePage(0) )
			return;

		if ( inc > 0 && pageNo < lastPage )
			pageNo++;
		else if ( inc < 0 && pageNo > firstPage )
			pageNo--;
		else if ( inc != 0 )
			return;

		ShowElt('divP01'  ,pageNo==1);
		ShowElt('divP02'  ,pageNo==2);
		ShowElt('divP03'  ,pageNo==3);
		ShowElt('divP04'  ,pageNo==4);
		ShowElt('divP05'  ,pageNo==5);
		ShowElt('btnBack' ,pageNo> firstPage);
		ShowElt('btnNext' ,pageNo< lastPage);
		ShowElt('btnAgree',pageNo==lastPage);

		if ( pageNo == lastPage )
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
			SetEltValue('spnDate',yy+'/'+mm+'/'+dd);
		}
	}
	catch (x)
	{
		alert(x.message);
	}
}
function ShowTick(err,ctl,seq)
{
	if ( seq == 2 )
		GetElt('img'+ctl).src = 'Images/' + ( err.length > 0 ? 'Cross' : 'Tick' ) + '.png';
}
function ValidatePage(ctl,seq)
{
	var err = "";
	var p;

	try
	{
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100111 )
		{
			p   = Validate('lstTitle','lblTitleError',3,lblTitleError.title,73,0);
			err = err + p;
			ShowTick(p,'Title',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100114 )
		{
			p   = Validate('txtSurname','lblSurnameError',1,lblSurnameError.title,2,2);
			err = err + p;
			ShowTick(p,'Surname',seq);
		}
		if ( ( pageNo == 1 && ctl == 0 ) || ctl == 100117 )
		{
			p   = Validate('txtCellNo','lblCellNoError',7,lblCellNoError.title);
			err = err + p;
			ShowTick(p,'CellNo',seq);
		}

		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100112 )
		{
			p   = Validate('txtFirstName','lblFirstNameError',1,lblFirstNameError.title,2,1);
			err = err + p;
			ShowTick(p,'FirstName',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100116 )
		{
			p   = Validate('txtEMail','lblEMailError',5,lblEMailError.title);
			err = err + p;
			ShowTick(p,'EMail',seq);
		}
		if ( ( pageNo == 2 && ctl == 0 ) || ctl == 100118 )
		{
			p   = Validate('txtID','lblIDError',6,lblIDError.title,7,20);
			err = err + p;
			ShowTick(p,'ID',seq);
		}

		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100123 )
		{
			p   = Validate('txtIncome','lblIncomeError',6,lblIncomeError.title,3,1000);
			err = err + p;
			ShowTick(p,'Income',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100131 )
		{
			p   = Validate('lstStatus','lblStatusError',3,lblStatusError.title,73,0);
			err = err + p;
			ShowTick(p,'Status',seq);
		}
		if ( ( pageNo == 3 && ctl == 0 ) || ctl == 100132 )
		{
			p   = Validate('lstPayDay','lblPayDayError',3,lblPayDayError.title,73,0);
			err = err + p;
			ShowTick(p,'PayDay',seq);
		}

		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100138 )
		{
			p   = Validate('lstOptions','lblOptionsError',3,lblOptionsError.title,73,0);
			err = err + p;
			ShowTick(p,'Options',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100144 )
		{
			p   = Validate('chkTerms','lblTermsError',8,lblTermsError.title,2);
			err = err + p;
			ShowTick(p,'Terms',seq);
		}
		if ( ( pageNo == 4 && ctl == 0 ) || ctl == 100147 )
		{
			p   = Validate('lstPayment','lblPaymentError',3,lblPaymentError.title,73,0);
			err = err + p;
			ShowTick(p,'Payment',seq);
		}

		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100187 )
		{
			p   = Validate('txtCCNumber','lblCCNumberError',6,lblCCNumberError.title,8,14);
			err = err + p;
			ShowTick(p,'CCNumber',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100186 )
		{
			p   = Validate('txtCCName','lblCCNameError',1,lblCCNameError.title,2,2);
			err = err + p;
			ShowTick(p,'CCName',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100188 )
		{
			p    = Validate('lstCCMonth','lblCCExpiryError',3,lblCCExpiryError.title,73,0)
			     + Validate('lstCCYear' ,'lblCCExpiryError',3,lblCCExpiryError.title,73,0);
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
				SetErrorLabel('lblCCExpiryError',p.length,p);
			}
			ShowTick(p,'CCExpiry',seq);
		}
		if ( ( pageNo == 5 && ctl == 0 ) || ctl == 100189 )
		{
			p   = Validate('txtCCCVV','lblCCCVVError',6,lblCCCVVError.title,7,4);
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

function TestSetup()
{
	SetEltValue("hdnTitleHelp"  ,"Please choose a title");
	SetEltValue("hdnSurnameHelp","Please enter your surname");
	SetEltValue("hdnCellNoHelp" ,"Please enter your mobile phone number");

	GetElt("lblTitleError").title   = "Choose a title from the list";
	GetElt("lblSurnameError").title = "Please enter a valid surname";
	GetElt("lblCellNoError").title  = "Please enter a valid mobile phone number";
}
</script>

<input type="hidden" id="hdnPageNo" value="1" />

<div id="divP01">
<p class="Header4">
(1) <asp:Literal runat="server" ID="lblSubHead1Label">Welcome</asp:Literal>
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
			<asp:HiddenField runat="server" ID="hdnTitleHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblTitleError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblSurnameLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtSurname" onfocus="JavaScript:ValidatePage(100114,1)" onblur="JavaScript:ValidatePage(100114,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Surname')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgSurname" />
			<asp:HiddenField runat="server" ID="hdnSurnameHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblSurnameError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCellNoLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCellNo" onfocus="JavaScript:ValidatePage(100117,1)" onblur="JavaScript:ValidatePage(100117,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CellNo')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCellNo" />
			<asp:HiddenField runat="server" ID="hdnCellNoHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblCellNoError"></asp:Label></td></tr>
</table>
</div>

<div id="divHelp" class="PopupBox" style="visibility:hidden"></div>

<div id="divP02">
<p class="Header4">
(2) <asp:Literal runat="server" ID="lblSubHead2Label"></asp:Literal>
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
			<asp:HiddenField runat="server" ID="hdnFirstNameHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblFirstNameError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblEMailLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtEMail" onfocus="JavaScript:ValidatePage(100116,1)" onblur="JavaScript:ValidatePage(100116,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'EMail')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgEMail" />
			<asp:HiddenField runat="server" ID="hdnEMailHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblEMailError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblIDLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtID" onfocus="JavaScript:ValidatePage(100118,1)" onblur="JavaScript:ValidatePage(100118,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'ID')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgID" />
			<asp:HiddenField runat="server" ID="hdnIDHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblIDError"></asp:Label></td></tr>
</table>
</div>

<div id="divP03">
<p class="Header4">
(3) <asp:Literal runat="server" ID="lblSubHead3Label"></asp:Literal>
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
			<asp:HiddenField runat="server" ID="hdnIncomeHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblIncomeError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblStatusLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstStatus" onfocus="JavaScript:ValidatePage(100131,1)" onblur="JavaScript:ValidatePage(100131,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Status')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgStatus" />
			<asp:HiddenField runat="server" ID="hdnStatusHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblStatusError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblPayDayLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayDay" onfocus="JavaScript:ValidatePage(100132,1)" onblur="JavaScript:ValidatePage(100132,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'PayDay')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgPayDay" />
			<asp:HiddenField runat="server" ID="hdnPayDayHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblPayDayError"></asp:Label></td></tr>
</table>
</div>

<div id="divP04">
<p class="Header4">
(4) <asp:Literal runat="server" ID="lblSubHead4aLabel"></asp:Literal> <!-- Congratulations! Your product options are: -->
</p><p class="Header5">
<asp:Literal runat="server" ID="lblSubHead4bLabel"></asp:Literal>
</p>
<table>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblOptionsLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstOptions" onfocus="JavaScript:ValidatePage(100138,1)" onblur="JavaScript:ValidatePage(100138,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Options')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgOptions" />
			<asp:HiddenField runat="server" ID="hdnOptionsHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblOptionsError"></asp:Label></td></tr>
	<tr>
		<td colspan="4" class="Header4">
			<br /><asp:Literal runat="server" ID="lblSubHead4cLabel"></asp:Literal><br />&nbsp;</td></tr>
	<tr>
		<td style="white-space:nowrap" colspan="2">
			<asp:CheckBox runat="server" ID="chkTerms" onclick="JavaScript:ValidatePage(100144,1)" />
			<asp:Literal runat="server" ID="lblTermsLabel"></asp:Literal>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Terms')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgTerms" />
			<asp:HiddenField runat="server" ID="hdnTermsHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblTermsError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblPaymentLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstPayment" onfocus="JavaScript:ValidatePage(100144,1);ValidatePage(100147,1)" onblur="JavaScript:ValidatePage(100147,2)">
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'Payment')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgPayment" />
			<asp:HiddenField runat="server" ID="hdnPaymentHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblPaymentError"></asp:Label></td></tr>
	<tr>
		<td colspan="4">
			<asp:Literal runat="server" ID="lblSubHead4dLabel"></asp:Literal></td></tr>
</table>
</div>

<div id="divP05">
<p class="Header4">
(5) <asp:Literal runat="server" ID="lblSubHead5Label"></asp:Literal>
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
			<asp:HiddenField runat="server" ID="hdnCCNumberHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblCCNumberError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCNameLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCName" onfocus="JavaScript:ValidatePage(100186,1)" onblur="JavaScript:ValidatePage(100186,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCName')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCName" />
			<asp:HiddenField runat="server" ID="hdnCCNameHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblCCNameError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCExpiryLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCMonth" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)">
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
			<asp:DropDownList runat="server" CssClass="DataInput" ID="lstCCYear" onfocus="JavaScript:ValidatePage(100188,1)" onblur="JavaScript:ValidatePage(100188,2)">
				<asp:ListItem Value= "2018" Text="2018"></asp:ListItem>
				<asp:ListItem Value= "2019" Text="2019"></asp:ListItem>
				<asp:ListItem Value= "2020" Text="2020"></asp:ListItem>
				<asp:ListItem Value= "2021" Text="2021"></asp:ListItem>
				<asp:ListItem Value= "2022" Text="2022"></asp:ListItem>
				<asp:ListItem Value= "2023" Text="2023"></asp:ListItem>
				<asp:ListItem Value= "2024" Text="2024"></asp:ListItem>
				<asp:ListItem Value= "2025" Text="2025"></asp:ListItem>
				<asp:ListItem Value= "2026" Text="2026"></asp:ListItem>
				<asp:ListItem Value= "2027" Text="2027"></asp:ListItem>
				<asp:ListItem Value= "2028" Text="2028"></asp:ListItem>
				<asp:ListItem Value= "2029" Text="2029"></asp:ListItem>
			</asp:DropDownList>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCExpiry')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCExpiry" />
			<asp:HiddenField runat="server" ID="hdnCCExpiryHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblCCExpiryError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Literal runat="server" ID="lblCCCVVLabel"></asp:Literal></td>
		<td style="white-space:nowrap">
			<asp:TextBox runat="server" CssClass="DataInput" ID="txtCCCVV" MaxLength="4" onfocus="JavaScript:ValidatePage(100189,1)" onblur="JavaScript:ValidatePage(100189,2)"></asp:TextBox>
			<a href="#" onmouseover="JavaScript:Help(1,this,'CCCVV')" onmouseout="JavaScript:Help(0)">?</a></td>
		<td>
			<img id="imgCCCVV" />
			<asp:HiddenField runat="server" ID="hdnCCCVVHelp" /></td>
		<td class="Error">
			<asp:Label runat="server" id="lblCCCVVError"></asp:Label></td></tr>
	<tr>
		<td class="DataLabel">
			<asp:Label runat="server" ID="lblCCDueDayLabel"></asp:Label></td>
		<td style="white-space:nowrap" colspan="3">
			<asp:Label runat="server" id="txtCCDueDay"></asp:Label></td></tr>
</table>

<div style="background-color:lightgray">
<div style="color:orange;font-weight:bold;font-size:20px">
<asp:Literal runat="server" ID="lblMandateHead"></asp:Literal> : <span id="spnDate"></span>
</div>
<asp:Literal runat="server" ID="lblMandateDetail"></asp:Literal>
</div>
</div>

<br />
<input type="button" id="btnBack"  value="<< BACK" onclick="JavaScript:NextPage(-1)" />
<input type="button" id="btnNext"  value="NEXT >>" onclick="JavaScript:NextPage(1)"  />
<input type="button" id="btnAgree" value="I Agree" />
<br /><br />

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>
</body>
</html>
