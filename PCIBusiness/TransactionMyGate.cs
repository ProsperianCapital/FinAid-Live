using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
//	MyGate no longer exist.
//	See www.wirecard.com

	public class TransactionMyGate : Transaction
	{
//	v1
//		static string soapEnvelope =
//			@"<SOAP-ENV:Envelope
//				xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/http' 
//				xmlns:ns1='PinManagement' 
//				xmlns:ns2='http://rpc.xml.coldfusion'>
//				<SOAP-ENV:Header>
//				</SOAP-ENV:Header>
//				<SOAP-ENV:Body>
//				</SOAP-ENV:Body>
//			</SOAP-ENV:Envelope>";

//	v2
//		static string soapEnvelope =
//			@"<SOAP-ENV:Envelope
//          SOAP-ENV:encodingStyle='http://schemas.xmlsoap.org/soap/encoding'
//				xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope'
//				xmlns:ns1='PinManagement'> 
//				<SOAP-ENV:Body>
//				</SOAP-ENV:Body>
//			</SOAP-ENV:Envelope>";

//	v3
//		static string soapEnvelope =
//			@"<SOAP-ENV:Envelope
//				xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope'
//				xmlns:ns1='PinManagement'
//				xmlns:xsd='http://www.w3.org/2001/XMLSchema'
//				xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
//				xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding'
//				SOAP-ENV:encodingStyle='http://schemas.xmlsoap.org/soap/encoding'>
//				<SOAP-ENV:Body> 
//				</SOAP-ENV:Body> 
//			</SOAP-ENV:Envelope>";

		private MyGateToken.PinManagement                         myGateToken;
		private MyGatePayment.MyGate_DebitOrder_WebServiceService myGatePay;
		private object[]                                          results;
		private string[]                                          resultLine;

		public override int GetToken(Payment payment)
		{
			int ret = 300;

			try
			{
				if ( myGateToken == null )
					myGateToken = new MyGateToken.PinManagement();

				ret       = 310;
				xmlResult = null;
				strResult = "";
				xmlSent   = "fLoadPinCC ( '" + payment.ProviderUserID + "'"
				                     + ", '" + payment.ProviderKey + "'"
				                     + ", '" + payment.CardNumber + "'"
				                     + ", '" + payment.CardName + "'"
				                     + ", '" + payment.CardExpiryMM + "'"
				                     + ", '" + payment.CardExpiryYYYY + "'"
				                     + ", '" + payment.CardType + "'"
				                     + ", '" + payment.CardPIN + "'"
				                     + ", '" + payment.MerchantReference + "' )";

				Tools.LogInfo("GetToken/10",xmlSent,10,this);

				ret     = 315;
				results = myGateToken.fLoadPinCC ( payment.ProviderUserID,
				                                   payment.ProviderKey,
				                                   payment.CardNumber,
				                                   payment.CardName,
				                                   payment.CardExpiryMM,
				                                   payment.CardExpiryYYYY,
				                                   payment.CardType,
				                                   payment.CardPIN,
				                                   payment.MerchantReference );

//				object[] fLoadPinCC(string ClientID, string ApplicationID, string CardNumber, string CardHolder, string ExpiryMonth, string ExpiryYear, string CardType, string ClientPin, string ClientUCI);

				ret          = 320;
				for ( int k  = 0 ; k < results.Length ; k++ )
					strResult = strResult + " [" + results[k].ToString().Trim() + "]";
				strResult    = strResult.Trim();

				ret        = 325;
				resultLine = results[1].ToString().Split(new string[]{"||"},StringSplitOptions.None);
				ret        = 330;
				payRef     = resultLine[1]; // TransactionIndex
				payToken   = resultLine[1];
				resultCode = "0";
				resultMsg  = "";
				ret        = 340;
				resultLine = results[0].ToString().Split(new string[]{"||"},StringSplitOptions.None);
				ret        = 350;

				if ( resultLine[1] == "0" ) // All OK
					return 0;

				ret        = 360;
				resultLine = results[2].ToString().Split(new string[]{"||"},StringSplitOptions.None);
				ret        = 370;
				resultCode = resultLine[1];
				ret        = 380;
				resultMsg  = resultLine[3] + ( resultLine[4].Length > 0 ? " (" + resultLine[4] + ")" : "" );

				Tools.LogInfo("GetToken/20","ResultCode=" + resultCode + ", Message=" + resultMsg,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", "+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", "+xmlSent, ex,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 600;
			xmlSent = "";

			try
			{
				ret     = 610;
				xmlSent = "<?xml version='1.0' encoding='utf-16'?>"
				        + "<debitorder>"
				        + "<header>"
				        +   "<merchantno>" + payment.ProviderAccount + "</merchantno>" // merchant account number, NOT MID
				        +   "<applicationid>" + payment.ProviderKey + "</applicationid>"
				        +   "<servicetype>1</servicetype>"
				        +   "<totaltransactions>1</totaltransactions>"
				        +   "<firstactiondate>" + Tools.DateToString(System.DateTime.Now,9) + "</firstactiondate>" // yymmdd
				        +   "<lastactiondate>" + Tools.DateToString(System.DateTime.Now,9) + "</lastactiondate>"
				        +   "<merchantcellnotify/>"
				        +   "<merchantemailnotify/>"
				        + "</header>"
				        + "<transaction>"
				        +   "<sequenceno>1</sequenceno>"
				        +   "<clientpin>" + payment.CardPIN + "</clientpin>"
				        +   "<clientuci>" + payment.CardPIN + "</clientuci>"
				        +   "<clientuid>" + payment.CardToken + "</clientuid>"
				        +   "<debitamount>" + payment.PaymentAmountDecimal + "</debitamount>"
				        +   "<debitdate>" + Tools.DateToString(System.DateTime.Now,9) + "</debitdate>"
				        +   "<debitreference>" + payment.PaymentDescription.Substring(0,13) + "</debitreference>"
				        +   "<debitcellnotify/>"
				        +   "<debitemailnotify/>"
				        +   "<transactionrefno>" + payment.MerchantReference + "</transactionrefno>"
				        + "</transaction>"
				        + "<footer>"
				        +   "<totaltransactions>1</totaltransactions>"
				        +   "<firstactiondate>" + Tools.DateToString(System.DateTime.Now,9) + "</firstactiondate>"
				        +   "<lastactiondate>" + Tools.DateToString(System.DateTime.Now,9) + "</lastactiondate>"
				        +   "<debittotal>" + payment.PaymentAmountDecimal + "</debittotal>"
				        + "</footer>"
				        + "</debitorder>";

				if ( myGatePay == null )
					myGatePay = new MyGatePayment.MyGate_DebitOrder_WebServiceService();

				ret       = 620;
				xmlResult = null;
				strResult = "";

				Tools.LogInfo("TokenPayment/20","uploadDebitFile(\"" + xmlSent + "\")",10,this);

				ret       = 630;
				strResult = myGatePay.uploadDebitFile (xmlSent);

				Tools.LogInfo("TokenPayment/30","Result=" + strResult,10,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		public TransactionMyGate() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.MyGate);
		}

		public override void Close()
		{
			myGateToken = null;
			myGatePay   = null;
			results     = null;
			resultLine  = null;
			base.Close();
		}
	}
}
