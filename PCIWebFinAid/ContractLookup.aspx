<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ContractLookup.aspx.cs" Inherits="PCIWebFinAid.ContractLookup" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>Contract Lookup</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="stylesheet" href="CSS/Calendar.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
	<style>
	.DLabel
	{
		white-space: nowrap;
		color: blue;
		font-weight: bold;
	}
	.DData
	{
	}
	</style>
</head>
<body>
<form id="frmLookup" runat="server">

<script type="text/javascript" src="JS/Calendar.js"></script>

<div class="Header3">
	Contract Lookup
</div>
<p>
Contract Code<br />
<asp:TextBox runat="server" ID="txtContractCode"></asp:TextBox>
</p><p>
<asp:Button runat="server" ID="btnSearch" text="Search" OnClick="btnSearch_Click" />
<asp:Button runat="server" ID="btnError" Text="Error ...?" OnClientClick="JavaScript:ShowElt('lblErrorDtl',true);return false" />
</p>

<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<asp:PlaceHolder runat="server" ID="pnlData">
<hr />
<table style="width:99%">
	<tr><td class="DLabel">Contract Code</td><td class="DData"><asp:Literal runat="server" id="lblContractCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Website Code</td><td class="DData"><asp:Literal runat="server" id="lblWebsiteCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Contract Application Status Code</td><td class="DData"><asp:Literal runat="server" id="lblContractApplicationStatusCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Prospecting Status Code</td><td class="DData"><asp:Literal runat="server" id="lblProspectingStatusCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Client Code</td><td class="DData"><asp:Literal runat="server" id="lblClientCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Client Type Code</td><td class="DData"><asp:Literal runat="server" id="lblClientCodeTypeCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Product Code</td><td class="DData"><asp:Literal runat="server" id="lblProductCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Product Option Code</td><td class="DData"><asp:Literal runat="server" id="lblProductOptionCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Product Option Mandate Type Code</td><td class="DData"><asp:Literal runat="server" id="lblProductOptionMandateTypeCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Surname</td><td class="DData"><asp:Literal runat="server" id="lblSurname"></asp:Literal></td></tr>
	<tr><td class="DLabel">First Name</td><td class="DData"><asp:Literal runat="server" id="lblFirstName"></asp:Literal></td></tr>
	<tr><td class="DLabel">Middle Name(s)</td><td class="DData"><asp:Literal runat="server" id="lblMiddleNames"></asp:Literal></td></tr>
	<tr><td class="DLabel">Initials</td><td class="DData"><asp:Literal runat="server" id="lblInitials"></asp:Literal></td></tr>
	<tr><td class="DLabel">Title Code</td><td class="DData"><asp:Literal runat="server" id="lblTitleCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Date Of Birth</td><td class="DData"><asp:Literal runat="server" id="lblDateOfBirth"></asp:Literal></td></tr>
	<tr><td class="DLabel">Home Language Code</td><td class="DData"><asp:Literal runat="server" id="lblHomeLanguageCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Home Language Dialect Code</td><td class="DData"><asp:Literal runat="server" id="lblHomeLanguageDialectCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Correspondence Language Code</td><td class="DData"><asp:Literal runat="server" id="lblCorrespondenceLanguageCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Correspondence Language Dialect Code</td><td class="DData"><asp:Literal runat="server" id="lblCorrespondenceLanguageDialectCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Gender Code</td><td class="DData"><asp:Literal runat="server" id="lblGenderCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Telephone (Mobile)</td><td class="DData"><asp:Literal runat="server" id="lblTelephoneNumberM"></asp:Literal></td></tr>
	<tr><td class="DLabel">Telephone (Home)</td><td class="DData"><asp:Literal runat="server" id="lblTelephoneNumberH"></asp:Literal></td></tr>
	<tr><td class="DLabel">Telephone (Work)</td><td class="DData"><asp:Literal runat="server" id="lblTelephoneNumberW"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 1 P</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine1P"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 2 P</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine2P"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 3 P</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine3P"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 4 P</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine4P"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 5 P</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine5P"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 1 W</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine1W"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 2 W</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine2W"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 3 W</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine3W"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 4 W</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine4W"></asp:Literal></td></tr>
	<tr><td class="DLabel">Address Line 5 W</td><td class="DData"><asp:Literal runat="server" id="lblAddressLine5W"></asp:Literal></td></tr>
	<tr><td class="DLabel">Country Code</td><td class="DData"><asp:Literal runat="server" id="lblCountryCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">EMail Address</td><td class="DData"><asp:Literal runat="server" id="lblEMailAddress"></asp:Literal></td></tr>
	<tr><td class="DLabel">Pay Date Code</td><td class="DData"><asp:Literal runat="server" id="lblPayDateCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Payment Method Code</td><td class="DData"><asp:Literal runat="server" id="lblPaymentMethodCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Payment Cycle Code</td><td class="DData"><asp:Literal runat="server" id="lblPaymentCycleCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Bank Code</td><td class="DData"><asp:Literal runat="server" id="lblBankCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Bank Branch Code</td><td class="DData"><asp:Literal runat="server" id="lblBankBranchCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Account Holder</td><td class="DData"><asp:Literal runat="server" id="lblAccountHolder"></asp:Literal></td></tr>
	<tr><td class="DLabel">Bank Account Holder Relationship Code</td><td class="DData"><asp:Literal runat="server" id="lblBankAccountHolderRelationShipCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Bank Account Number</td><td class="DData"><asp:Literal runat="server" id="lblBankAccountNumber"></asp:Literal></td></tr>
	<tr><td class="DLabel">Bank Account Type Code</td><td class="DData"><asp:Literal runat="server" id="lblBankAccountTypeCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card Association Code</td><td class="DData"><asp:Literal runat="server" id="lblCardAssociationCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card TypeCode</td><td class="DData"><asp:Literal runat="server" id="lblCardTypeCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card Number</td><td class="DData"><asp:Literal runat="server" id="lblCardNumber"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card Expiry Month</td><td class="DData"><asp:Literal runat="server" id="lblCardExpiryMonth"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card Expiry Year</td><td class="DData"><asp:Literal runat="server" id="lblCardExpiryYear"></asp:Literal></td></tr>
	<tr><td class="DLabel">Card CVV</td><td class="DData"><asp:Literal runat="server" id="lblCardCVVCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Third Party Collector Code</td><td class="DData"><asp:Literal runat="server" id="lblThirdPartyCollectorCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Third Party Collector Reference</td><td class="DData"><asp:Literal runat="server" id="lblThirdPartyCollectorReference"></asp:Literal></td></tr>
	<tr><td class="DLabel">Gross Income</td><td class="DData"><asp:Literal runat="server" id="lblGrossIncome"></asp:Literal></td></tr>
	<tr><td class="DLabel">Net Income</td><td class="DData"><asp:Literal runat="server" id="lblNetIncome"></asp:Literal></td></tr>
	<tr><td class="DLabel">Expenditure (Cell Phone)</td><td class="DData"><asp:Literal runat="server" id="lblExpenditureCellPhone"></asp:Literal></td></tr>
	<tr><td class="DLabel">Expenditure (Groceries</td><td class="DData"><asp:Literal runat="server" id="lblExpenditureGroceries"></asp:Literal></td></tr>
	<tr><td class="DLabel">Expenditure (Housing)</td><td class="DData"><asp:Literal runat="server" id="lblExpenditureHousing"></asp:Literal></td></tr>
	<tr><td class="DLabel">Expenditure (Insurance)</td><td class="DData"><asp:Literal runat="server" id="lblExpenditureInsurance"></asp:Literal></td></tr>
	<tr><td class="DLabel">Expenditure (Other)</td><td class="DData"><asp:Literal runat="server" id="lblExpenditureOther"></asp:Literal></td></tr>
	<tr><td class="DLabel">Disposable Income</td><td class="DData"><asp:Literal runat="server" id="lblDisposableIncome"></asp:Literal></td></tr>
	<tr><td class="DLabel">Legal Restriction Code</td><td class="DData"><asp:Literal runat="server" id="lblLegalRestrictionCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Client Employment Status Code</td><td class="DData"><asp:Literal runat="server" id="lblClientEmploymentStatusCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">T & C's Read</td><td class="DData"><asp:Literal runat="server" id="lblTsCsRead"></asp:Literal></td></tr>
	<tr><td class="DLabel">Refund Policy Read</td><td class="DData"><asp:Literal runat="server" id="lblRefundPolicyRead"></asp:Literal></td></tr>
	<tr><td class="DLabel">Cancellation Policy Read</td><td class="DData"><asp:Literal runat="server" id="lblCancellationPolicyRead"></asp:Literal></td></tr>
	<tr><td class="DLabel">Client IP Address</td><td class="DData"><asp:Literal runat="server" id="lblClientIPAddress"></asp:Literal></td></tr>
	<tr><td class="DLabel">Client Device</td><td class="DData"><asp:Literal runat="server" id="lblClientDevice"></asp:Literal></td></tr>
	<tr><td class="DLabel">Contact Centre Code</td><td class="DData"><asp:Literal runat="server" id="lblContactCentreCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Contract Application Date</td><td class="DData"><asp:Literal runat="server" id="lblContractApplicationDate"></asp:Literal></td></tr>
	<tr><td class="DLabel">Sales Agent Code</td><td class="DData"><asp:Literal runat="server" id="lblSalesAgentCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Capturing Agent Code</td><td class="DData"><asp:Literal runat="server" id="lblCapturingAgentCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Marketing Mix Code</td><td class="DData"><asp:Literal runat="server" id="lblMarketingMixCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Contract Procurement Channel Code</td><td class="DData"><asp:Literal runat="server" id="lblContractProcurementChannelCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Contract PIN</td><td class="DData"><asp:Literal runat="server" id="lblContractPin"></asp:Literal></td></tr>
	<tr><td class="DLabel">Website Host Name</td><td class="DData"><asp:Literal runat="server" id="lblWebsiteHostName"></asp:Literal></td></tr>
	<tr><td class="DLabel">Website Visitor Code</td><td class="DData"><asp:Literal runat="server" id="lblWebsiteVisitorCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Website Visitor Session Code</td><td class="DData"><asp:Literal runat="server" id="lblWebsiteVisitorSessionCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Google UTM Source</td><td class="DData"><asp:Literal runat="server" id="lblGoogleUtmSource"></asp:Literal></td></tr>
	<tr><td class="DLabel">Google UTM Medium</td><td class="DData"><asp:Literal runat="server" id="lblGoogleUtmMedium"></asp:Literal></td></tr>
	<tr><td class="DLabel">Google UTM Campaign</td><td class="DData"><asp:Literal runat="server" id="lblGoogleUtmCampaign"></asp:Literal></td></tr>
	<tr><td class="DLabel">Google UTM Term</td><td class="DData"><asp:Literal runat="server" id="lblGoogleUtmTerm"></asp:Literal></td></tr>
	<tr><td class="DLabel">Google UTM Content</td><td class="DData"><asp:Literal runat="server" id="lblGoogleUtmContent"></asp:Literal></td></tr>
	<tr><td class="DLabel">Advert Code</td><td class="DData"><asp:Literal runat="server" id="lblAdvertCode"></asp:Literal></td></tr>
	<tr><td class="DLabel">Event Date</td><td class="DData"><asp:Literal runat="server" id="lblEventDate"></asp:Literal></td></tr>
	<tr><td class="DLabel">Event Trigger</td><td class="DData"><asp:Literal runat="server" id="lblEventTrigger"></asp:Literal></td></tr>
	<tr><td class="DLabel">Event User Code</td><td class="DData"><asp:Literal runat="server" id="lblEventUserCode"></asp:Literal></td></tr>
</table>
</asp:PlaceHolder>

<asp:Label runat="server" ID="lblErrorDtl" style="border:1px solid #000000;position:fixed;bottom:20px;right:5px;visibility:hidden;display:none;padding:5px;font-family:Verdana;background-color:pink"></asp:Label>

<asp:Literal runat="server" ID="lblJS"></asp:Literal>

</form>
</body>
</html>