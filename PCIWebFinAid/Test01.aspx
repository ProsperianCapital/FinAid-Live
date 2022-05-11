<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test01.aspx.cs" Inherits="PCIWebFinAid.Test01" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Prosperian : Testing</title>
	<link rel="stylesheet" href="CSS/BackOffice.css" type="text/css" />
	<!--#include file="IncludeMainSimple.htm" -->
</head>
<body>
<script type="text/javascript">
</script>

<script>
alert('T01');
window.addEventListener('message', function(event) {
  alert('Event: '+event.origin);
  if (event.origin.startsWith('https://centinelapistag.cardinalcommerce.com'))
  {
    alert('(T02) WorldPay event: '+event.origin);
    var data = JSON.parse(event.data);
    alert('(T03) WorldPay event data: '+data.toString());
    if (data !== undefined && data !== null && data.Status)
    { alert('(T04) Data found'); }
  }
}, false);
alert('T05');

//window.onload = function()
//{ alert('Onload(2)'); }
</script>

<!--
<iframe name="iX" id="iX" height='480' width='640' style="border:1px solid #000000">
</iframe>
-->

<iframe height='1' width='1' style='display:none;visibility:hidden' onload="alert('T20');document.frm3D.submit();alert('T21')">
	<!-- <body onload="alert('T20');document.frm3D.submit();alert('T21')" -->
	<form id='frm3D' method="post" action='https://centinelapistag.cardinalcommerce.com/V1/Cruise/Collect'>
		<input type='hidden' name='Bin' id='Bin' value='4444333322221111' />
		<input type='hidden' name='JWT' id='JWT' value='X' />
	</form>
	<script>
	alert('T06');
//  window.onload = function()
//  { alert('Onload(1)'); document.getElementById('frm3D').submit(); }
	alert('T07');
	</script>
</iframe>

<form id="frmHome" runat="server">
	<div class="Header3">
	Prosperian Integration Testing
	</div>
</form>

</body>
</html>