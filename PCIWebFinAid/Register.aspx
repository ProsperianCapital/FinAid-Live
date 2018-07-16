<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Register.aspx.cs" Inherits="PCIWebFinAid.Register" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>FinAid : Register</title>
	<link rel="stylesheet" href="FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="gfx/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<form id="frmRegister" runat="server">
<ascx:Header runat="server" ID="ctlHeader" />

<script type="text/javascript">
var firstPage = 1;
var lastPage  = 5;
var pageNo    = firstPage;

function NextPage(inc)
{
	try
	{
		if ( inc > 0 && ! ValidatePage() )
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
function ValidatePage()
{
	var err = "X";
	try
	{
		if ( pageNo == 1 )
			err = Validate('lstTitle','errTitle',3,errTitle.title,73,0)
			    + Validate('txtSurname','errSurname',1,errSurname.title,2,2)
			    + Validate('txtCellNo','errCellNo',7,errCellNo.title);
		else if ( pageNo == 2 )
			err = Validate('txtFirstName','errFirstName',1,errFirstName.title,2,2)
			    + Validate('txtEMail','errEMail',5,errEMail.title)
			    + Validate('txtID','errID',1,errID.title,2,4);
		else if ( pageNo == 3 )
			err = Validate('txtIncome','errIncome',6,errIncome.title,3,100)
			    + Validate('lstStatus','errStatus',3,errStatus.title,73,0)
			    + Validate('lstPayDay','errPayDay',3,errPayDay.title,73,0);
		else if ( pageNo == 4 )
			err = Validate('lstOptions','errOptions',3,errOptions.title,73,0)
			    + Validate('chkTerms','errTerms',8,errTerms.title,2)
			    + Validate('lstPayment','errPayment',3,errPayment.title,73,0);
		else if ( pageNo == 5 )
			err = Validate('txtCCNumber','errCCNumber',6,errCCNumber.title,71)
			    + Validate('txtCCName','errCCName',1,errCCName.title,2,2)
			    + Validate('lstCCMonth','errCCExpiry',3,errCCExpiry.title,73,0)
			    + Validate('lstCCYear','errCCExpiry',3,errCCExpiry.title,73,0)
			    + Validate('txtCCCVV','errCCCVV',6,errCCCVV.title,2,9999);
	}
	catch (x)
	{
		err = "Y";
	}
	return ( err.length == 0 );
}
</script>

<input type="hidden" id="hdnPageNo" value="1" />

<div id="divP01">
<p class="Header4">
(1) Welcome
</p>
<table>
	<tr>
		<td>Title</td>
		<td>
			<asp:DropDownList runat="server" ID="lstTitle" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
				<asp:ListItem Value="0" Text="Select Title"></asp:ListItem>
				<asp:ListItem Value="1" Text="Mr"></asp:ListItem>
				<asp:ListItem Value="2" Text="Mrs"></asp:ListItem>
				<asp:ListItem Value="3" Text="Miss"></asp:ListItem>
				<asp:ListItem Value="4" Text="Dr"></asp:ListItem>
				<asp:ListItem Value="5" Text="Prof"></asp:ListItem>
			</asp:DropDownList> ?</td>
		<td id="errTitle" title="Please choose a title"></td></tr>
	<tr>
		<td>Surname</td>
		<td><asp:TextBox runat="server" ID="txtSurname" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?&nbsp;&nbsp;</td>
		<td id="errSurname" title="Please enter your Surname/Last Name"></td></tr>
	<tr>
		<td>Mobile Number</td>
		<td><asp:TextBox runat="server" ID="txtCellNo" Width="240px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?</td>
		<td id="errCellNo" title="Please enter a valid mobile/cellular phone number"></td></tr>
</table>
</div>

<div id="divP02">
<p class="Header4">
(2) Welcome
</p>
<table>
	<tr>
		<td>First Name</td>
		<td><asp:TextBox runat="server" ID="txtFirstName" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?&nbsp;&nbsp;</td>
		<td id="errFirstName" title="Please enter your First Name"></td></tr>
	<tr>
		<td>EMail</td>
		<td><asp:TextBox runat="server" ID="txtEMail" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?</td>
		<td id="errEMail" title="Please enter a valid email address"></td></tr>
	<tr>
		<td>Identity Number</td>
		<td><asp:TextBox runat="server" ID="txtID" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?</td>
		<td id="errID" title="Please enter your identity number"></td></tr>
</table>
</div>

<div id="divP03">
<p class="Header4">
(3) Income
</p>
<table>
	<tr>
		<td>Net Income</td>
		<td><asp:TextBox runat="server" ID="txtIncome" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?</td>
		<td id="errIncome" title="Your total monthly income after statutory deductions"></td></tr>
	<tr>
		<td>Employment Status</td>
		<td>
			<asp:DropDownList runat="server" ID="lstStatus" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
				<asp:ListItem Value="0" Text="Select Employment Status"></asp:ListItem>
				<asp:ListItem Value="1" Text="Fixed Term"></asp:ListItem>
				<asp:ListItem Value="2" Text="Permanent"></asp:ListItem>
				<asp:ListItem Value="3" Text="Project Based"></asp:ListItem>
				<asp:ListItem Value="4" Text="Casual"></asp:ListItem>
				<asp:ListItem Value="5" Text="No Contract"></asp:ListItem>
				<asp:ListItem Value="6" Text="Training Agreement"></asp:ListItem>
			</asp:DropDownList> ?</td>
		<td id="errStatus" title="Please choose an employment status"></td></tr>
	<tr>
		<td>Pay Day</td>
		<td>
			<asp:DropDownList runat="server" ID="lstPayDay" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
				<asp:ListItem Value="0" Text="Select Pay Day"></asp:ListItem>
				<asp:ListItem Value="1" Text="Monthly on the 1'st"></asp:ListItem>
				<asp:ListItem Value="2" Text="Monthly on the 8'th"></asp:ListItem>
				<asp:ListItem Value="3" Text="Monthly on the 15'th"></asp:ListItem>
				<asp:ListItem Value="4" Text="Monthly on the 21'st"></asp:ListItem>
				<asp:ListItem Value="5" Text="Monthly on the 25'th"></asp:ListItem>
				<asp:ListItem Value="6" Text="Monthly on the last day"></asp:ListItem>
			</asp:DropDownList> ?</td>
		<td id="errPayDay" title="Please choose your pay day"></td></tr>
</table>
</div>

<div id="divP04">
<p class="Header4">
(4) Congratulations! Your product options are:
</p>
Product Options&nbsp;&nbsp;
<asp:DropDownList runat="server" ID="lstOptions" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
	<asp:ListItem Value="0" Text="Select Product Option"></asp:ListItem>
	<asp:ListItem Value="1" Text="Bronze"></asp:ListItem>
	<asp:ListItem Value="2" Text="Silver"></asp:ListItem>
	<asp:ListItem Value="3" Text="Gold"></asp:ListItem>
</asp:DropDownList> ?&nbsp;&nbsp;&nbsp;&nbsp;
<span id="errOptions" title="Please choose one of the product options"></span>
<p class="Header4">
I confirm having read and understood each of ...
</p>
<asp:CheckBox runat="server" ID="chkTerms" onclick="JavaScript:ValidatePage()" /> Terms & Conditions ?&nbsp;&nbsp;&nbsp;&nbsp;
<span id="errTerms" title="Please confirm that you have read and understood all the terms and conditions"></span><br /><br />
Payment Method&nbsp;&nbsp;
<asp:DropDownList runat="server" ID="lstPayment" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
	<asp:ListItem Value="0" Text="Select Payment Method"></asp:ListItem>
	<asp:ListItem Value="1" Text="Debit my Visa or MasterCard"></asp:ListItem>
</asp:DropDownList> ?&nbsp;&nbsp;&nbsp;&nbsp;
<span id="errPayment" title="Please choose a payment method"></span><br /><br />
PLEASE NOTE: We are accepting your application based on the information you provided.
</div>

<div id="divP05">
<p class="Header4">
(5) Card Information
</p>
<table>
	<tr>
		<td>Card Number</td>
		<td><asp:TextBox runat="server" ID="txtCCNumber" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?&nbsp;&nbsp;</td>
		<td id="errCCNumber" title="Please enter your credit/debit card number"></td></tr>
	<tr>
		<td>Name on Card</td>
		<td><asp:TextBox runat="server" ID="txtCCName" Width="320px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?</td>
		<td id="errCCName" title="Please enter the name on your credit/debit card"></td></tr>
	<tr>
		<td>Expiry Date</td>
		<td>
			<asp:DropDownList runat="server" ID="lstCCMonth" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
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
			</asp:DropDownList>&nbsp;
			<asp:DropDownList runat="server" ID="lstCCYear" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()">
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
			</asp:DropDownList> ?</td>
		<td id="errCCExpiry" title="Please select your card's date (month and year)"></td></tr>
	<tr>
		<td>CVV Code</td>
		<td><asp:TextBox runat="server" ID="txtCCCVV" Width="40px" onfocus="JavaScript:ValidatePage()" onblur="JavaScript:ValidatePage()"></asp:TextBox> ?&nbsp;&nbsp;</td>
		<td id="errCCCVV" title="Please enter your card CVV number"></td></tr>
</table>
<div style="background-color:lightgray">
<p style="color:orange;font-weight:bold;font-size:20px">
COLLECTION MANDATE : <span id="spnDate"></span>
</p><p>
You hereby authorise and instruct us to collect all money due by you from your Card listed above or any other card that you may indicate from time to time.
</p><p>
You confirm that you are the account holder or have authority to give us this mandate.
</p><p>
You hereby authorise and instruct us to collect your registration fee immediately or as soon as possible.
</p><p>
You hereby authorise and instruct us to deduct your monthly subscription fees on, or as close as possible to your indicated Pay Day.
</p><p>
You will ensure that sufficient funds are available in your account to cover these deductions and we may track your account and re-present the deductions should the transactions fail.
</p><p>
This collection mandate will stay in force until you cancel it by giving us at least 30 days prior written notice to the client care email address listed in the Contact Us section of this website.
</p>
</div>
</div>

<br />
<input type="button" id="btnBack"  value="<< Back" onclick="JavaScript:NextPage(-1)" />
<input type="button" id="btnNext"  value="Next >>" onclick="JavaScript:NextPage(1)"  />
<input type="button" id="btnAgree" value="I Agree" />
<br /><br />

</form>
<ascx:Footer runat="server" ID="ctlFooter" />
</body>
</html>
