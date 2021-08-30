using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using PCIBusiness.IkajoService;

namespace PCIBusiness
{
	public class TransactionIkajo : Transaction
	{
		PCIBusiness.IkajoService.PtCardService ikajo;
		PCIBusiness.IkajoService.SaleRequest   saleReq;
		PCIBusiness.IkajoService.RebillRequest rebillReq;

		public  bool Successful
		{
			get
			{
				string x = Tools.NullToString(resultCode).ToUpper();
				return ( x == "SUCCESS" || x == "ACCEPT" );
			}
		}

		public override int GetToken(Payment payment)
		{
			string url = Tools.ConfigValue("SystemURL") + "/Succeed.aspx?TransRef=" + Tools.XMLSafe(payment.MerchantReference);
			int    ret = 300;

			Tools.LogInfo("TransactionIkajo.GetToken/10","Sale, Merchant Ref=" + payment.MerchantReference,10);

			try
			{
				string md5Signature = HashMD5 ( Tools.XMLSafe(payment.CardExpiryMM+payment.CardExpiryYY)
                                          + Tools.XMLSafe(payment.CardNumber)                       
				                              + Tools.XMLSafe(payment.CardType)                         
				                              + Tools.XMLSafe(payment.CardCVV)                          
				                              + Tools.XMLSafe(payment.Address1())                         
				                              + Tools.XMLSafe(payment.Address2())                         
				                              + Tools.XMLSafe(payment.CountryCode())                      
				                              + Tools.XMLSafe(payment.EMail)                            
				                              + Tools.XMLSafe(payment.MandateIPAddress)                          
				                              + Tools.XMLSafe(payment.FirstName+" "+payment.LastName)   
				                              + Tools.XMLSafe(payment.PhoneCell)                        
				                              + Tools.XMLSafe(payment.State)                            
				                              + Tools.XMLSafe(payment.PostalCode())                       
				                              + Tools.XMLSafe("YES")                                                  
				                              + Tools.XMLSafe(payment.ProviderUserID)                  
				                              + Tools.XMLSafe(payment.PaymentAmountDecimal)             
				                              + Tools.XMLSafe(payment.CurrencyCode)                     
				                              + Tools.XMLSafe(payment.PaymentDescription)               
				                              + Tools.XMLSafe(payment.MerchantReference)
				                              + Tools.XMLSafe(url)
				                              + Tools.XMLSafe(payment.ProviderPassword) );

				if ( saleReq == null )
					saleReq    = new SaleRequest();

				saleReq.cardExpiration       = Tools.XMLSafe(payment.CardExpiryMM+payment.CardExpiryYY);
				saleReq.cardNumber           = Tools.XMLSafe(payment.CardNumber);
				saleReq.cardType             = Tools.XMLSafe(payment.CardType); 
				saleReq.cardVerificationCode = Tools.XMLSafe(payment.CardCVV); 
				saleReq.customerAddress      = Tools.XMLSafe(payment.Address1());
				saleReq.customerCity         = Tools.XMLSafe(payment.Address2());
				saleReq.customerCountry      = Tools.XMLSafe(payment.CountryCode());
				saleReq.customerEmail        = Tools.XMLSafe(payment.EMail);
				saleReq.customerIP           = Tools.XMLSafe(payment.MandateIPAddress);
				saleReq.customerName         = Tools.XMLSafe(payment.FirstName+" "+payment.LastName);
				saleReq.customerPhoneNumber  = Tools.XMLSafe(payment.PhoneCell);
				saleReq.customerState        = Tools.XMLSafe(payment.State);
				saleReq.customerZipCode      = Tools.XMLSafe(payment.PostalCode());
				saleReq.initRecurring        = "YES";
				saleReq.merchantID           = Tools.XMLSafe(payment.ProviderUserID);
				saleReq.orderAmount          = Tools.XMLSafe(payment.PaymentAmountDecimal);
				saleReq.orderCurrency        = Tools.XMLSafe(payment.CurrencyCode);
				saleReq.orderDescription     = Tools.XMLSafe(payment.PaymentDescription);
				saleReq.orderReference       = Tools.XMLSafe(payment.MerchantReference);
				saleReq.returnUrl            = Tools.XMLSafe(url);
				saleReq.signature            = md5Signature;

				Tools.LogInfo("TransactionIkajo.GetToken/20","",10);

				if ( ikajo == null )
					ikajo    = new PtCardService();

				PCIBusiness.IkajoService.SaleResult result = ikajo.sale(saleReq);
				string                              x      = "[TokenResult] StatusCode=" + result.statusCode
				                                                      + " | TransactionStatus=" + result.transactionStatus
				                                                      + " | TransactionError=" + result.transactionError
				                                                      + " | RecurringToken=" + result.recurringToken;

				Tools.LogInfo("TransactionIkajo.GetToken/30",x,10);

				payToken   = result.recurringToken;
				payRef     = result.orderReference;
				resultCode = result.transactionStatus;
				resultMsg  = result.transactionError;

//				if ( ret != 0 )
//					Tools.LogInfo("TransactionIkajo.GetToken/50","ResultCode="+ResultCode + ", payToken=" + payToken,220);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionIkajo.GetToken/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionIkajo.GetToken/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 300;

			Tools.LogInfo("TransactionIkajo.TokenPayment/10","Sale, Merchant Ref=" + payment.MerchantReference,10);

			try
			{
				string md5Signature = HashMD5 ( Tools.XMLSafe(payment.ProviderUserID)
                                          + Tools.XMLSafe(payment.PaymentAmountDecimal)
				                              + Tools.XMLSafe(payment.PaymentDescription)                         
				                              + Tools.XMLSafe(payment.MerchantReference)
				                              + Tools.XMLSafe(payment.MerchantReferenceOriginal)
				                              + Tools.XMLSafe(payment.CardToken)
				                              + Tools.XMLSafe(payment.ProviderPassword) );

				if ( rebillReq == null )
					rebillReq    = new RebillRequest();

				rebillReq.merchantID             = Tools.XMLSafe(payment.ProviderUserID);
				rebillReq.orderAmount            = Tools.XMLSafe(payment.PaymentAmountDecimal);
				rebillReq.orderDescription       = Tools.XMLSafe(payment.PaymentDescription);
				rebillReq.orderReference         = Tools.XMLSafe(payment.MerchantReference);
				rebillReq.originalOrderReference = Tools.XMLSafe(payment.MerchantReferenceOriginal);
				rebillReq.recurringToken         = Tools.XMLSafe(payment.CardToken);
				rebillReq.signature              = md5Signature;

				Tools.LogInfo("TransactionIkajo.TokenPayment/20","",10);

				if ( ikajo == null )
					ikajo    = new PtCardService();

				PCIBusiness.IkajoService.RebillResult result = ikajo.rebill(rebillReq);
				string                                x      = "[PaymentResult] StatusCode=" + result.statusCode
				                                                          + " | TransactionStatus=" + result.transactionStatus
				                                                          + " | TransactionError=" + result.transactionError;

				Tools.LogInfo("TransactionIkajo.TokenPayment/30",x,10);

				payRef     = result.orderReference;
				resultCode = result.transactionStatus;
				resultMsg  = result.transactionError;

//				if ( ret != 0 )
//					Tools.LogInfo("TransactionIkajo.GetToken/50","ResultCode="+ResultCode + ", payToken=" + payToken,220);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TransactionIkajo.TokenPayment/98","Ret="+ret.ToString()+", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionIkajo.TokenPayment/99","Ret="+ret.ToString()+", XML Sent="+xmlSent,ex);
			}
			return ret;
		}

		private string HashMD5(string data)
		{
			int           k;
			byte[]        bytes;
			StringBuilder hash = new StringBuilder();

//	Test as per the example in the documentation
//			data = "01204111111111111111VISA112123, Main StreetAnytownUSAjohndoe@example.com123.123.123.123John Doe1234567890XX99999YESMERCHANT-119.90USDSome ProductORDER-123456http://example.com/return_url.phpPa$$W0rD";
//	Test as per the example in the documentation

			using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
			//	UTF-8
				bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(data));

//	Not needed
			//	UTF-32
//				bytes = md5.ComputeHash(new UTF32Encoding().GetBytes(data));
			//	ASCII
//				bytes = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(data));
			//	Default
//				bytes = md5.ComputeHash(Encoding.Default.GetBytes(data));
			//	Unicode
//				bytes = md5.ComputeHash(new UnicodeEncoding().GetBytes(data));

			for (k = 0; k < bytes.Length; k++)
				hash.Append(bytes[k].ToString("x2"));
	
			return hash.ToString();
		}

		public TransactionIkajo() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.Ikajo);
		}

		public override void Close()
		{
			ikajo     = null;
			saleReq   = null;
			rebillReq = null;
			base.Close();
		}
	}
}
