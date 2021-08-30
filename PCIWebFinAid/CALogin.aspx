<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CALogin.aspx.cs" Inherits="PCIWebFinAid.CALogin" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainSimple.htm" -->
	<title>CareAssist Africa - Instant Help When You Need It Most</title>
	<link rel="stylesheet" href="CSS/CAFinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<!--
	<style>
	body
	{
		background-image: url("ImagesCA/CareAssist3.jpg"); /* no-repeat center center fixed; */
		background-position: center;
		background-repeat: no-repeat;
		background-size: cover;
	}
	</style>
	-->
</head>
<body>
<form id="frmLogin" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<img src="ImagesCA/CA-Assist1.jpg" style="width:100%" />
	<p style="text-align:center; color:#FFFFFF; font-family:Arial; font-size:35px; font-weight:600; line-height:1.5em; letter-spacing:0.8px; text-shadow:0px 0px 11px #000000; position:absolute; top:100px; left:0px; right:0px">
	Emergency Mobile, Legal & Financial Assistance
	</p><p style="text-align:center; color:#54595F; font-family:Sans-serif; font-size:19px; font-weight:600; line-height:1.6em; letter-spacing:0.8px; text-shadow:0px 0px 51px #FFFFFF; position:absolute; top:210px; left:0px; right:0px">
	CareAssist provides Emergency Mobile Response and a Legal Access Service<br />
	on subscription with a FREE Emergency CASH Reward for loyal subscribers.
	</p>
	<div style="margin:0 auto;display:block;width:100%">
	<img src="ImagesCA/CA-Gold.png"   style="width:33%" />
	<img src="ImagesCA/CA-Silver.png" style="width:33%" />
	<img src="ImagesCA/CA-Bronze.png" style="width:33%" />
	</div>

	<div style="position:relative">
	<img src="ImagesCA/CA-Assist2.jpg" style="width:100%" />
	<div style="color:#FFFFFF; font-family:Sans-serif; position:absolute; top:10px; left:5%; width:95%">
		<p style="font-size:35px; font-weight:400; line-height:1.4em; letter-spacing:0.8px">
		HOW IT WORKS
		</p><p style="font-size:19px;font-weight:300;line-height:1.6em">
		CareAssist is an Emergency Mobile Response<br />
		Application & Legal Assistance Service with an<br />
		Emergency CASH Reward for loyal subscribers.
		</p>
		<div style="margin-right:120px;font-size:19px;font-weight:300;line-height:1.6em;float:right">
		Only 3 Minutes Easy<br />Online Application.<br />Select your option below.
		<p style="font-size:24px;font-weight:300;line-height:1.6em">
		GOLD<br />
		SILVER<br />
		BRONZE
		</p>
		<a href="#"><div class="TopButton TopButtonO" style="height:30px">REGISTER</div></a>
		</div>
		<p style="font-size:19px;font-weight:600;color:#54595F">
		Register
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		Online 24/7 via this website (Please have your<br />ID Number & Debit/Credit Card details handy).
		</p><p style="font-size:19px;font-weight:600;color:#54595F">
		Pay
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		Your Annual Registration & Monthly<br />Subscription Fees (Via Direct<br />Debit from your Master or VISA card).
		</p><p style="font-size:19px;font-weight:600;color:#54595F">
		Enjoy
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		All your Subscription Benefits<br />(Accessible Online 24/7).
		</p>
	</div>
	</div>

	<div>
	<p style="color: #F88742;font-family:Sans-serif;font-size: 35px;font-weight: 400;line-height: 1.4em;letter-spacing: 0.8px;text-align:center">
	YOUR SUBSCRIPTION BENEFITS
	</p>
	<div style="display:flex;margin:auto;width:100%">
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/isos.png" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Mobile Response</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/Legal.jpg" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Legal Access</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/Loyalty.png" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Cash Reward</figcaption>
	</figure> 
	</div>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height: 1.5;margin-left:50px;margin-right:50px">
	<p style="color:#FBDB00;font-family:Montserrat, Sans-serif;font-size:19px;font-weight:500;text-transform:capitalize;line-height:1.2em;letter-spacing:0.6px">
	1. iSOS Emergency Mobile Response Application
	</p><p style="margin-left:20px">
	Don’t PANIC! With our iSOS Emergency Mobile Response Application HELP from Friends, Family and Emergency Professionals is only a button press away!<br />
	Our iSOS Emergency Mobile Response Application is a Mobile Panic Button that notifies your predefined list of friends, family members or emergency professionals of your emergency via SMS to a maximum of 5 recipients.<br />
	All you must do is install the iSOS application from Google Play and press the appropriate button in case of a specific emergency.<br />
	You may personalise your iSOS Emergency Mobile Response Service from the login menu of this website.
	</p><p style="color:#FBDB00;font-family:Montserrat, Sans-serif;font-size:19px;font-weight:500;text-transform:capitalize;line-height:1.2em;letter-spacing:0.6px">
	2. Emergency Legal Assistance Service
	</p><p style="margin-left:20px">
	Never Stand ALONE! With our Emergency Legal Assistance Service, a Lawyer is only a phone call away!<br />
	Our Emergency Legal Assistance Service, available to Bronze subscribers, is a telephonic helpline affording you access to Legal Professionals for telephonic general legal advice or assistance when you need it most. The service is limited to 2 telephonic consultations of 30 minutes each per annum with a lawyer of your choice and approved by us.<br />
	Our Extended Legal Assistance Service, available to Silver subscribers, includes 4 telephonic consultations of 30 minutes each per annum with a lawyer of your choice and approved by us.<br />
	Our Advanced Legal Assistance Service, available to Gold subscribers, includes 6 telephonic consultations of 30 minutes each per annum with a lawyer of your choice and approved by us.
	</p><p style="color:#FBDB00;font-family:Montserrat, Sans-serif;font-size:19px;font-weight:500;text-transform:capitalize;line-height:1.2em;letter-spacing:0.6px">
	3. Emergency CASH Reward
	</p><p style="margin-left:20px">
	Quick and Instant Emergency CASH when you need it MOST! We all know just how much a little extra cash in our pocket can mean ...<br />
	Our annual Emergency Cash Reward is accessible online through this product website. You can access this reward immediately after paying your registration fee and 1st monthly subscription fee, without having to wait!<br />
	Simply request the Cash Reward you need from our website and we shall process transfer of your requested amount to your card expediently, usually within 24 Hours. (Dependant on information accuracy, banking restrictions and holidays.)<br />
	To qualify for your annual Emergency Cash Reward, you are required to be up to date with your monthly subscription fee payments in respect of your annual subscription.<br />
	Your initial Emergency Cash Reward is limited to R 750 for the first 3 months and thereafter to a maximum amount according to your chosen annual product subscription option: Bronze R 2,000, Silver R 2,500, Gold R 3,000.
	</p><p><b>
	PLEASE NOTE
	</b></p><p>
	You are liable for an annual registration fee equivalent to one month’s service subscription. All your product benefits are included as part of your annual service subscription fee, which is payable monthly. We may charge your subscription fee payments in Euros, Singaporean Dollars, US Dollars or SA Rands. Please note that these amounts are based on the Euro and are subject to changes weekly.<br />
	</p><p>
	During the first 30 days after signing up for this service, you may cancel immediately for a full refund of any amounts paid, less the value of any benefits received by you. Should you wish to cancel later than 30 days after signing up, you must give us at least one calendar month’s written notice and you would be required to repay any loyalty rewards that you have received.
	</p><p>
	It is important to note that, in the event where you elect to cancel your annual subscription with us, all your outstanding monthly fees will remain due and payable to us.
	</p><p>
	We reserve the right to amend any of the terms and conditions of our service offering, including price, at any time, and such changes will be reflected in the content of this website.
	</p>
	</div>

	<div>
	<img src="ImagesCA/CA-Assist3.jpg" style="width:99%" />
	</div>

	<p></p>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<img src="Images/LogoENG.png" style="height:30px" />
		<p>
		CareAssist is rendered by LifeStyle Direct Financial Services (Pty) Ltd, duly registered in South Africa (company registration no 2009/008897/07).
		We comply fully with applicable laws and regulations and with international marketing and consumer service best practise guidelines.
		</p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		CONTACT US
		</p><p><b>
		Customer Support
		</b></p><p><b>
		Corporate Office
		</b></p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		SITE MAP
		</p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>

	<asp:HiddenField runat="server" ID="hdnVer" />
</form>
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
