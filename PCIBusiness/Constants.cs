using System;

namespace PCIBusiness
{
	public static class Constants
	{

	//	General stand-alone constants
	//	-----------------------------
		public static DateTime DateNull
		{
			get { return Convert.ToDateTime("1799/12/31"); }
		}
		public static short MaxRowsSQL
		{
			get { return 1000; }
		}
		public static short MaxRowsPayment
		{
			get { return 50; }
		}
		public static string HTMLBreak
		{
			get { return "<br />"; }
		}
		public static byte LogSeverity
		{
			get { return 233; }
		}
//		public static string TextBreak
//		{
//			return { Environment.NewLine; }
//		}

		public enum SystemPassword : int
		{
			Login     = 901317,
			BackDoor  = 615702,
			Technical = 463228,
			MobileDev = 183206
		}

		public enum DBColumnStatus : byte
		{
			InvalidColumn = 1,
			EOF           = 2,
			ValueIsNull   = 3,
			ColumnOK      = 4
		}

		public enum PaymentStatus : byte
		{
			WaitingToPay     =   1,
			FailedTryAgain   =  21,
			FailedDoNotRetry =  22,
			BusyProcessing   =  51,
			Successful       = 101
		}

		public enum PaymentProvider : int
		{
			MyGate           =  2,
			T24              =  6,
			Ikajo            = 15,
			PayU             = 16,
			PayGate          = 17,
			PayGenius        = 18,
			Ecentric         = 19,
			eNETS            = 20,
			Peach            = 21,
			TokenEx          = 22,
			SatchelPay       = 23,
			FNB              = 24,
			CyberSource      = 25,
			CyberSource_Moto = 26,
			PaymentsOS       = 27, // PayU Hub
			Stripe_USA       = 28,
			Stripe_EU        = 29,
			Stripe_Asia      = 30,
			PaymentCloud     = 31, // Authorize.Net
			WorldPay         = 32  // FlutterWave
		}

		public enum MessageProvider : int
		{
			ClickaTell = 1,
			GlobalSMS  = 2,
			SendGrid   = 3,
			SocketLabs = 4
		}

		public enum TradingProvider : int
		{
			InteractiveBrokers = 1,
			FinnHub            = 2
		}

		public enum CreditCardType : byte
		{
			Visa            = 1,
			MasterCard      = 2,
			AmericanExpress = 3,
			DinersClub      = 4
		}

		public enum PagingMode : byte
		{
			None              = 0,
			AllowScreenPaging = 209,
			DoNotReadNextRow  = 244
		}

		public enum ApplicationCode : short
		{
			Registration =   0,
			BackOffice   =   1,
			CRM          =   2,
			Mobile       =   6,
			CareAssist   = 100,
			iSOS         = 110,
			LifeGuru     = 120,
			PayPayYa     = 170
		}

		public enum BureauStatus : byte
		{
			Unknown     = 0,
			Development = 1,
			Testing     = 2,
			Live        = 3
		}

		public enum SystemMode : byte
		{
			Development = 1,
			Test        = 2,
			Live        = 3
//			Debug       = 4
		}
		public enum ProcessMode : int
		{
			FullUpdate                 =  0, // Live
			UpdateToken                = 10,
			DeleteToken                = 11,
			UpdatePaymentStep1         = 21,
			UpdatePaymentStep2         = 22,
			UpdatePaymentStep1AndStep2 = 23,
			NoUpdate                   = 99
		}
		public enum TransactionType : byte
		{
			GetToken              =   1,
			GetTokenThirdParty    =   8,
			CardPayment           =   3,
			CardPaymentThirdParty =   6,
			TokenPayment          =   2,
			DeleteToken           =   4,
			GetCardFromToken      =   5,
			ThreeDSecurePayment   =   7,
			ThreeDSecureCheck     =  12,
			Transfer              =   9,
			Reversal              =  10,
			Refund                =  11,
			ZeroValueCheck        =  13,
			ManualPayment         =  73,
			TransactionLookup     =  81,
			Test                  = 197
		}

//	iTextSharp stuff

		public enum PdfFontSize : int
		{
			HugeHeading      = 40,
			MajorHeading     = 32,
			MinorHeading     = 20,
			SubHeading       = 16,
			TableHeading     = 12,
			TableCell        = 10,
			ParagraphSpacing = 10,
			ParagraphPadding =  5
		}

		public enum PdfAlign : int // These must match iTextSharp.Element.ALIGN_LEFT, etc values
		{
			Left   = 0,
			Right  = 2,
			Centre = 1,
			Middle = 5
		}

		public enum TickerType : int
		{
			IBStockPrices          =  1,
			IBExchangeRates        =  2,
			IBPortfolio            =  3,
			IBOrders               =  4, // Not implemented yet
			IBExchangeCandles      =  5,
			FinnHubStockPrices     = 21,
			FinnHubStockHistory    = 22,
			FinnHubExchangeRates   = 23,
			FinnHubStockTicks      = 24,
			FinnHubExchangeCandles = 29
		}

		public enum TickerStatus : byte
		{
			Stopped  = 11,
			Stopping = 21,
			Starting = 31,
			Running  = 41,
			Disabled = 88,
			Unused   = 99
		}

		public enum TickerAction : byte
		{
			ShutDown = 1,
			Stop     = 2,
			Run      = 3
		}

		public enum DataFormat : int
		{
			CSV = 31,
			PDF = 32			
		}

		public enum HttpMethod : int
		{
			Get  = 1,
			Post = 2			
		}

		public enum TechnicalQuery : byte
		{
		//	Values greater than 99 need the system password
			SQLStatus       =   1,
			SQLObject       = 102,
			SQLExecute      = 103,
			ConfigNet       =   4,
			ConfigSoftware  =   5,
			ConfigApp       = 106,
			ErrorLogView    =   7,
			InfoLogView     =   8,
			ErrorLogWrite   = 109,
			InfoLogWrite    = 110,
			EMailSend       = 111,
			ClientDetails   =  12,
			CertDetails     =  13,
			ServerVariables =  14
		}


		public enum MessageType : byte
		{
			EMail    = 1,
			SMS      = 2,
			WhatsApp = 3
		}

		public enum WebDataType : byte
		{
			FormGetOrPost = 1,
			FormPost      = 2,
			JSON          = 3,
			XML           = 4
		}

		public enum ErrorType : int
		{
			InvalidMenu = 1
		}

//		public enum PaymentType : byte
//		{
//			Tokens      = 10,
//			CardNumbers = 20,
//			Vault       = 30
//		}
	}
}