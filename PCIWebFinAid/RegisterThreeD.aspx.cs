using System;
using System.Web.UI.WebControls;
using System.IO;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterThreeD : BasePage
	{
		private string providerRef;
		private string providerCode;
		private string transRef;
		private string resultCode;
		private string resultMsg;
		private int    provRet;
		private int    sqlRet;
		private int    ret;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			provRet      = 990;
			sqlRet       = 770;
			ret          = 10;
			providerRef  = "";
			providerCode = WebTools.RequestValueString(Request,"ProviderCode");
			transRef     = WebTools.RequestValueString(Request,"TransRef");
			resultCode   = WebTools.RequestValueString(Request,"ResultCode");
			resultMsg    = WebTools.RequestValueString(Request,"ResultMessage");

//	Testing ...
//
//			try
//			{
//				using (StreamReader reader = new StreamReader(Request.InputStream))
//				{
//					ret           = 20;
//					string webStr = reader.ReadToEnd();
//					Tools.LogInfo("PageLoad/14",webStr,222,this);
//				}
//			}
//			catch
//			{ }

			try
			{
				Tools.LogInfo("PageLoad/16",Request.Url.AbsoluteUri,222,this);

			//	if ( resultCode.Length > 0 || resultMsg.Length > 0 || transRef.Length < 1 )
			//	{
			//		ret = 30;
			//		SetMessage("Error ...","Your payment failed.");
			//		return;
			//	}

				ret               = 40;
				Transaction trans = null;
				string      token = "";
				string      sql   = "exec sp_WP_PaymentRegister3DSecA @ContractCode=" + Tools.DBString(transRef);

				if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) ||
				     providerCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
				{
					ret         = 50;
					provRet     = 0;
				//	trans       = new TransactionCyberSource(providerCode);
					resultCode  = WebTools.RequestValueString(Request,"auth_response");
					providerRef = WebTools.RequestValueString(Request,"transaction_id");
					token       = WebTools.RequestValueString(Request,"payment_token");
					resultMsg   = WebTools.RequestValueString(Request,"decision");
					string xCd  = WebTools.RequestValueString(Request,"decision_return_code");
					string xMsg = WebTools.RequestValueString(Request,"message");
					if ( resultMsg.Length < 1 ) resultMsg = "FAIL";
					if ( xCd.Length       > 0 ) resultMsg = resultMsg + "/" + xCd;
					if ( xMsg.Length      > 0 ) resultMsg = resultMsg + " (" + xMsg + ")";

					if ( resultCode.Length > 0 && resultCode != "0" && resultCode != "00" && resultCode != "000" && resultCode != "0000" )
					{
						SetMessage("Error ...","Your payment was rejected.");
						provRet = 210;
					}
				}
				else if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_EU)  ||
				          providerCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA) ||
				          providerCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_Asia) )
				{
					ret         = 50;
					trans       = new TransactionStripe();
					providerRef = WebTools.RequestValueString(Request,"payment_intent");
				//	string id   = WebTools.RequestValueString(Request,"payment_intent_client_secret");
					provRet     = trans.ThreeDSecureCheck(providerRef);
					resultCode  = trans.ResultCode;
					resultMsg   = trans.ResultMessage;
				}
				else if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
				{
					if ( resultCode == "0" || resultCode == "00" || resultCode == "000" )
						provRet = 0;
					else
					{
						SetMessage("Error ...","Your payment was rejected.");
						provRet = 440;
					}
				}
				else if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
				{
					ret         = 50;
					trans       = new TransactionPayU();
					providerRef = WebTools.RequestValueString(Request,"PayUReference");
					provRet     = trans.ThreeDSecureCheck(providerRef,transRef);
					token       = trans.PaymentToken;
					resultCode  = trans.ResultCode;
					resultMsg   = trans.ResultMessage;
				}
				else
				{
					ret         = 80;
					trans       = new TransactionPeach();
				//	resultCode  = "XXX-XXX-XXX";
				//	providerURL = WebTools.RequestValueString(Request,"resourcePath");
					providerRef = WebTools.RequestValueString(Request,"id");
					provRet     = trans.ThreeDSecureCheck(providerRef);
					resultCode  = trans.ResultCode;
				}

				ret   = 0;
				trans = null;
				sql   = sql + ",@ReferenceNumber="    + Tools.DBString(providerRef)
				            + ",@Status="             + Tools.DBString(resultCode)
				            + ",@PaymentBureauCode="  + Tools.DBString(providerCode)
				            + ",@PaymentBureauToken=" + Tools.DBString(token);

				using (MiscList mList = new MiscList())
				{
				//	ret    = 120;
//	Single language
//					sqlRet = mList.ExecQuery(sql,0,"",false,true);

//	Multi language
					sqlRet = mList.ExecQuery(sql,0);
					Tools.LogInfo("PageLoad/45",sql+" (sqlRet="+sqlRet.ToString()+")",222,this);

					if ( sqlRet == 0 )
					{
					//	ret = 0;
					}
					else
					{
					//	ret = 210;
						Tools.LogException("PageLoad/70","sqlRet="+sqlRet.ToString()+" ("+sql+")",null,this);
						Tools.LogInfo     ("PageLoad/75","sqlRet="+sqlRet.ToString()+" ("+sql+")",222,this);
					}
				}
				if ( provRet == 0 )
					SetMessage("Thank You ...","Your application has been successfully received.",false);
				else if ( provRet < 1000 )
					SetMessage("Thank You ...","Your application was received but there was a problem with your card and/or payment.",false);
				else
					SetMessage("Oops ...","Something seems to have gone wrong.<br /><br />We have logged the error and will investigate.",false);
			}
			catch (Exception ex)
			{
				SetMessage("Oops ...","Something seems to have gone wrong.");
				Tools.LogException("PageLoad/90","ret="+ret.ToString(),ex,this);
				Tools.LogInfo     ("PageLoad/95","ret="+ret.ToString()+", "+ex.Message,222,this);
			}
		}

		private void SetMessage(string head1,string head2,bool overWrite=true)
		{
			if ( overWrite || lbl100503.Text.Length < 1 )
				lbl100503.Text = head1;
			if ( overWrite || lbl100504.Text.Length < 1 )
				lbl100504.Text = head2 + ( head2.Length > 0 ? "<br /><br />" : "" )
					            + "<table style='white-space:nowrap'>"
								   + "<tr><td><b>Transaction Result Code</b></td><td> : "        + resultCode     + "</td></tr>"
								   + "<tr><td><b>Transaction Message</b></td><td> : "            + resultMsg      + "</td></tr>"
								   + "<tr><td><b>Transaction Id</b></td><td> : "                 + providerRef    + "</td></tr>"
								   + "<tr><td><b>Contract/Transaction Reference</b></td><td> : " + transRef       + "</td></tr>"
								   + "<tr><td><b>Internal Return Codes</b></td><td> : "
				               + ret.ToString() + " / " + provRet.ToString() + " / " + sqlRet.ToString()      + "</td></tr>"
				               + "</table>";
		}
	}
}