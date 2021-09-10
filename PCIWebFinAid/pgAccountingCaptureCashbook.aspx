<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgAccountingCaptureCashbook.aspx.cs" Inherits="PCIWebFinAid.pgAccountingCaptureCashbook" EnableEventValidation="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<script async type="text/javascript" src="JS/AJAXUtils.js"></script>
<script type="text/javascript">
function AJAXFinalize(typeCode)
{
	var codes;
	var names;
	var sel;
	var searchEdit;
	var lstCB1;
	var lstCB2;
	var k;

	if ( typeCode == 1 ) // Cashbook lookup
	{
		codes      = xmlDOM.getElementsByTagName("CBCode");
		names      = xmlDOM.getElementsByTagName("CBName");
		sel        = xmlDOM.getElementsByTagName("CBSel");
		searchEdit = XMLValue('SearchEdit');
		lstCB1     = GetElt("lst"+searchEdit+"CashBook");
		lstCB2     = null;
		k;

		if ( searchEdit == 'E' )
			var lstCB2   = GetElt("lstSCashBook");

		while (lstCB1.length > 0)
			lstCB1.remove(0);

		if ( lstCB2 != null )
		{
			while (lstCB2.length > 0)
				lstCB2.remove(0);
			ListAdd(lstCB2,GetEltValue('hdnSCashBook'),GetEltValue('hdnSCashBookName'),true);
		}

		SetEltValue('hdn'+searchEdit+'CashBook','');

		for (k = 0; k < codes.length; k++)
		{
			ListAdd(lstCB1,codes[k].firstChild.nodeValue,names[k].firstChild.nodeValue,(sel[k].firstChild.nodeValue=="Y"));
			if ( k == 0 || sel[k].firstChild.nodeValue=="Y" )
				SetEltValue('hdn'+searchEdit+'CashBook',codes[k].firstChild.nodeValue);
		}
	}

	else if ( typeCode == 2 ) // GL code lookup
	{
		codes      = xmlDOM.getElementsByTagName("GLCode");
		names      = xmlDOM.getElementsByTagName("GLName");
		sel        = xmlDOM.getElementsByTagName("GLSel");
		searchEdit = XMLValue('SearchEdit');
		lstCB1     = GetElt("lst"+searchEdit+"GLCode");
		k;

		while (lstCB1.length > 0)
			lstCB1.remove(0);

		SetEltValue('hdn'+searchEdit+'GLCode','');

		for (k = 0; k < codes.length; k++)
		{
			ListAdd(lstCB1,codes[k].firstChild.nodeValue,names[k].firstChild.nodeValue,(sel[k].firstChild.nodeValue=="Y"));
			if ( k == 0 || sel[k].firstChild.nodeValue=="Y" )
				SetEltValue('hdn'+searchEdit+'GLCode',codes[k].firstChild.nodeValue);
		}
	}
	HideBusy();
}
function LoadCashBooks(selCode,searchEdit)
{
	SetEltValue('hdn'+searchEdit+'CashBook','');
	var coy = GetListValue('lst'+searchEdit+'Company');
	if ( coy.length > 0 && coy != "0" )
	{
		ShowBusy("Loading ...");
		AJAXInitialize(1,"CompanyCode="+coy+"&SearchEdit="+searchEdit+(selCode==null?'':'&Selected='+selCode));
		return;
	}
	var lstCB = GetElt('lst'+searchEdit+'CashBook');
	while (lstCB.length > 0)
		lstCB.remove(0);
}
function LoadGLCodes(selCode,searchEdit)
{
	SetEltValue('hdn'+searchEdit+'GLCode','');
	try
	{
		ShowBusy("Loading ...");
		var tty = GetListValue('lst'+searchEdit+'TType').split('/');
		AJAXInitialize(2,"GLCode="+tty[1]+"&SearchEdit="+searchEdit+(selCode==null?'':'&Selected='+selCode));
		return;
	}
	catch (x)
	{ }
}
function HidePopups()
{
	hideCalendar();
	ShowMessages(0);
	ShowElt('pnlDelete',false);
//	ShowElt('pnlEdit',false);
}
function EditMode(editInsert)
{
//	editInsert values:
//	0 : Hide
//	1 : Show, edit
//	2 : Show, insert
	HidePopups();
	if ( editInsert == 1 )
		SetEltValue('lblEditHead','Edit/Delete Transaction (ID '+GetEltValue('hdnETranID')+')');
	else
		SetEltValue('lblEditHead','New Transaction');
	ShowElt('pnlEdit',(editInsert> 0));
	ShowElt('btnDel1',(editInsert==1));
	SetEltValue('hdnEditInsert',editInsert);
}
function DeleteMode(show)
{
	HidePopups();
	SetEltValue('lblDTranID',GetEltValue('hdnETranID'));
	ShowElt('pnlDelete',(show>0));
}
</script>
<form id="frmCashBook" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	CashBook
	</div>

	<div id="pnlFilter" style="border:1px solid #000000">
	<div class="Header6">Search/Filter
		<span style="float:right">
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',true)">Show</a>&nbsp;&nbsp;
		<a href="#" onclick="JavaScript:ShowElt('tblFilter',false)">Hide</a></span>
	</div>
	<table id="tblFilter">
	<tr>
		<td>Company</td><td><asp:DropDownList runat="server" ID="lstSCompany" onchange="JavaScript:LoadCashBooks(null,'S')"></asp:DropDownList></td>
		<td colspan="2">Cashbook</td>
		<td>
			<asp:DropDownList runat="server" ID="lstSCashBook" onchange="JavaScript:SetEltValue('hdnSCashBook',GetListValue(this));SetEltValue('hdnSCashBookName',this.options[this.selectedIndex].text)"></asp:DropDownList>
			<asp:HiddenField runat="server"  ID="hdnSCashBook" value="" />
			<asp:HiddenField runat="server"  ID="hdnSCashBookName" value="" /></td></tr>
	<tr>
		<td>Receipt / Payment</td><td><asp:DropDownList runat="server" ID="lstSReceipt"></asp:DropDownList></td>
		<td colspan="2">Transaction Type</td>
		<td><asp:DropDownList runat="server" ID="lstSTType" onchange="JavaScript:LoadGLCodes(null,'S')"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company</td><td><asp:DropDownList runat="server" ID="lstSOBOCompany"></asp:DropDownList></td>
		<td colspan="2">Transaction Description</td><td><asp:TextBox runat="server" ID="txtSDesc" Width="234px"></asp:TextBox></td></tr>
	<tr>
		<td>GL Account Code</td>
		<td>
			<asp:DropDownList runat="server" ID="lstSGLCode" onchange="JavaScript:SetEltValue('hdnSGLCode',GetListValue(this))"></asp:DropDownList>
			<asp:HiddenField runat="server"  ID="hdnSGLCode" value="" /></td>
		<td colspan="2">GL Account Dimension</td><td><asp:DropDownList runat="server" ID="lstSGLDimension"></asp:DropDownList></td></tr>
	<tr>
		<td>Transaction Start Date</td><td><asp:TextBox runat="server" ID="txtSDate1" MaxLength="10" Width="100px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate1)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td>
		<td colspan="2">Transaction End Date</td><td><asp:TextBox runat="server" ID="txtSDate2" MaxLength="10" Width="100px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtSDate2)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>Tax Rate</td><td><asp:DropDownList runat="server" ID="lstSTaxRate"></asp:DropDownList></td>
		<td style="white-space:nowrap">Amount</td><td>&gt;<br /><br />&lt;</td><td>
		<asp:TextBox runat="server" ID="txtSAmt1" Width="100px"></asp:TextBox><br />
		<asp:TextBox runat="server" ID="txtSAmt2" Width="100px"></asp:TextBox></td>
		<td rowspan="99" style="text-align:right">&nbsp;
			<asp:Button runat="server" ID="btnSearch" Text="Filter" OnClientClick="JavaScript:ShowBusy('Searching ... Please be patient',null,0)" OnClick="btnSearch_Click" />
		</td></tr>
	</table>
	</div>

	<p>
	<asp:Button runat="server" ID="btnNew1" Text="New (Inline)" ToolTip="Capture a new transaction" OnClick="btnNew_Click" />&nbsp;
	<asp:Button runat="server" ID="btnNew2" Text="New (New)"    ToolTip="Capture a new transaction" OnClientClick="JavaScript:window.open('pgAccountingCaptureCashbook.aspx?Mode=213','New');return false" />&nbsp;
	<asp:PlaceHolder runat="server" ID="pnlGridBtn" Visible="false">
	<asp:Button runat="server" ID="btnPDF"  Text="PDF" ToolTip="Download in PDF format" OnClick="btnPDF_Click" />&nbsp;
	<asp:Button runat="server" ID="btnCSV"  Text="CSV" ToolTip="Download in CSV (Excel) format" OnClick="btnCSV_Click" />
	</asp:PlaceHolder>
	</p>

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<asp:DataGrid id="grdData" runat="server" AutoGenerateColumns="False" CellSpacing="5"
			OnItemCommand="grdData_Click" OnSortCommand="grdData_Sort" AllowSorting="true">
		<HeaderStyle CssClass="tRowHead"></HeaderStyle>
		<ItemStyle CssClass="tRow"></ItemStyle>
		<AlternatingItemStyle CssClass="tRowAlt"></AlternatingItemStyle>
		<Columns>
			<asp:BoundColumn DataField="RowNumber" ItemStyle-BackColor="GreenYellow" ItemStyle-BorderStyle="Solid" ItemStyle-BorderWidth="1" ItemStyle-BorderColor="Red" DataFormatString="&nbsp{0}&nbsp;"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Transaction<br />ID" SortExpression="TransactionID">
				<ItemTemplate>
					<asp:LinkButton runat="server" CommandName='Edit' CommandArgument='<%# ((PCIBusiness.MiscData)Container.DataItem).GetColumn(0) %>'><%# ((PCIBusiness.MiscData)Container.DataItem).NextColumn %></asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Date" DataField="NextColumn" SortExpression="TransactionDate"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Receipt or<br />Payment" DataField="NextColumn" SortExpression="RP"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Transaction<br />Type" DataField="NextColumn" SortExpression="TransactionType"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Main<br />Account" DataField="NextColumn" SortExpression="GLAccountCode"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="GL Account<br />Dimension" DataField="NextColumn" SortExpression="GLAccountDimension"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Transaction<br />Description">
				<ItemTemplate>
				<%# ((PCIBusiness.MiscData)Container.DataItem).NextColumnSubstring(50,"...") %>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn HeaderText="Tax<br />Rate" DataField="NextColumn" SortExpression="TaxRate"></asp:BoundColumn>
			<asp:BoundColumn HeaderText="Amount<br />(Inclusive)" DataField="NextColumn" SortExpression="TransactionAmountInclusive" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
		</Columns>
	</asp:DataGrid>

	<div class="Popup2" id="pnlEdit" style="visibility:hidden;display:none">
	<asp:HiddenField runat="server" ID="hdnEditInsert" Value="0" />
	<asp:HiddenField runat="server" ID="hdnETranID" Value="0" />
	<!-- asp:TextBox run@t="server" ID="txtETranID" visible="false" readOnly="true" -->
	<div class="Header6" id="lblEditHead"></div>
	<table>
	<tr>
		<td>Company<br /><asp:DropDownList runat="server" ID="lstECompany" onchange="JavaScript:LoadCashBooks(null,'E')"></asp:DropDownList></td>
		<td>
			Cashbook<br /><asp:DropDownList runat="server" ID="lstECashBook" onchange="JavaScript:SetEltValue('hdnECashBook',GetListValue(this))"></asp:DropDownList>
			<asp:HiddenField runat="server" ID="hdnECashBook" value="" /></td></tr>
	<tr>
		<td>Receipt / Payment<br /><asp:DropDownList runat="server" ID="lstEReceipt"></asp:DropDownList></td>
		<td>Transaction Type<br /><asp:DropDownList runat="server" ID="lstETType" onchange="JavaScript:LoadGLCodes(null,'E')"></asp:DropDownList></td></tr>
	<tr>
		<td>OBO Company<br /><asp:DropDownList runat="server" ID="lstEOBOCompany"></asp:DropDownList></td>
		<td>Transaction Date<br /><asp:TextBox runat="server" ID="txtEDate" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtEDate)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td>GL Account Code<br /><asp:DropDownList runat="server" ID="lstEGLCode" onchange="JavaScript:SetEltValue('hdnEGLCode',GetListValue(this))"></asp:DropDownList>
			<asp:HiddenField runat="server" ID="hdnEGLCode" value="" /></td>
		<td>GL Account Dimension<br /><asp:DropDownList runat="server" ID="lstEGLDimension"></asp:DropDownList></td></tr>
	<tr>
		<td>Amount<br /><asp:TextBox runat="server" ID="txtEAmt" MaxLength="10" Width="80px"></asp:TextBox></td>
		<td>Tax Rate<br /><asp:TextBox runat="server" ID="txtETaxRate" Width="40px"></asp:TextBox></td></tr>
	<tr>
		<td>Currency<br />
			<asp:DropDownList runat="server" ID="lstECurr"></asp:DropDownList></td>
		<td>Recon Date<br /><asp:TextBox runat="server" ID="txtERecon" MaxLength="10" Width="80px"></asp:TextBox>
			<a href="JavaScript:showCalendar(frmCashBook.txtERecon)"><img src="<%=PCIBusiness.Tools.ImageFolder() %>Calendar.gif" title="Pop-up calendar" style="vertical-align:middle" /></a></td></tr>
	<tr>
		<td colspan="2">Transaction Description<br /><asp:TextBox runat="server" ID="txtEDesc" Width="485px"></asp:TextBox></td></tr>
	</table>
	<asp:Button runat="server" ID="btnSave" Text="Save" title="Save this transaction" OnClick="btnUpdate_Click" />&nbsp;
	<input type="button" value="Delete" id="btnDel1" title="Delete this transaction" onclick="JavaScript:DeleteMode(1)" />
	<input type="button" value="Cancel" id="btnCancel" onclick="JavaScript:EditMode(0)" />
	<asp:Label runat="server" id="lblErr2" cssclass="Error"></asp:Label>
	</div>

	<div id="pnlDelete" class="PopupConfirm" style="visibility:hidden;display:none;width:320px">
	<div class="HelpHead">Please confirm ...</div>
	<p>
	You are about to DELETE transaction id <span id="lblDTranID" style="font-weight:bold"></span>.<br />
	This action cannot be reversed.
	</p><p class="Error"><b>
	Are you sure you want to do this?
   </b></p>
	<asp:Button runat="server" ID="btnDel2" Text="Delete" title="Delete this transaction" OnClick="btnDelete_Click" />&nbsp;
	<input type="button" value="Cancel" onclick="JavaScript:DeleteMode(0)" />
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>