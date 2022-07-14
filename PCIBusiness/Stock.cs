using System;

namespace PCIBusiness
{
	public class Stock : BaseData
	{
		private int      stockId;
		private string   symbol;
		private string   securityType;
		private string   currencyCode;
		private string   exchangeCode;
		private string   primaryExchange;
		private string   resolution;
		private long     fromDate;
		private long     toDate;
		private DateTime theDate;
		private int      ticksMax;
		private int      ticksSkip;
//		private int      fromDate;
//		private int      toDate;
//		private DateTime fromDate;
//		private DateTime toDate;

		private double   price;
		private int      quantity;
		private int      tickType;

		public  int      StockId
		{
			get { return  stockId; }
			set { stockId = value; }
		}
		public  int      TickType
		{
			get { return  tickType; }
			set { tickType = value; }
		}
		public  int      Ticks
		{
			get { return  ticksMax; }
			set { ticksMax = value; }
		}
		public  int      TicksSkip
		{
			get { return  ticksSkip; }
			set { ticksSkip = value; }
		}
		public  double   Price
		{
			get { return  price; }
			set { price = value; }
		}
		public  int      Quantity
		{
			get { return  quantity; }
			set { quantity = value; }
		}
		public  string   Symbol
		{
			get { return  Tools.NullToString(symbol); }
			set { symbol = value.Trim(); }
		}
		public  string   CurrencyCode
		{
			get { return  Tools.NullToString(currencyCode).ToUpper(); }
			set { currencyCode = value.Trim(); }
		}
		public  string   SecurityType
		{
			get
			{
				securityType = Tools.NullToString(securityType);
				if ( securityType.Length < 1 )
					securityType = "STK";
				return securityType;
			}
			set { securityType = value.Trim(); }
		}

		public  string  ExchangeCode
		{
			get { return Tools.NullToString(exchangeCode); }
			set { exchangeCode = value.Trim(); }
		}

		public  string  PrimaryExchange
		{
			get { return Tools.NullToString(primaryExchange); }
			set { primaryExchange = value.Trim(); }
		}

		public  string  Resolution
		{
			get { return Tools.NullToString(resolution); }
		}

//		public  int     FromDateUNIX
//		{
//			get
//			{
//				if ( fromDate <= System.Convert.ToDateTime("1970/01/01 00:00:00") )
//					return 0;
//		}

		public  long    FromDate
		{
			get
			{
				if ( fromDate < 1 )
					return 0;
				return    fromDate;
			}
		}

		public  long    ToDate
		{
			get
			{
				if ( toDate < 1 )
					return 0;
				return    toDate;
			}
		}

		public  DateTime Date
		{
			get { return theDate; }
		}

		public int UpdatePrice()
		{
			if ( price > 0 && stockId > 0 && tickType >= 0 )
				try
				{
					sql = "exec sp_Ins_TickerCurrentRaw"
						 + " @StockID = "  + stockId.ToString()
					    + ",@DateTime = " + Tools.DateToSQL(System.DateTime.Now,5)
					    + ",@TickType = " + tickType.ToString()
					    + ",@Value = "    + price.ToString();
					Tools.LogInfo("Stock.UpdatePrice/1",sql,222);
					return ExecuteSQL(null,2);
				}
				catch (Exception ex)
				{
					Tools.LogException("Stock.UpdatePrice/2",sql,ex);
				}
			return 8199;
		}

		public int UpdateQuantity()
		{
			if ( quantity > 0 && stockId > 0 && tickType >= 0 )
				try
				{
					sql = "exec sp_Ins_TickerCurrentRaw"
						 + " @StockID = "  + stockId.ToString()
					    + ",@DateTime = " + Tools.DateToSQL(System.DateTime.Now,5)
					    + ",@TickType = " + tickType.ToString()
					    + ",@Value = "    + quantity.ToString();
					Tools.LogInfo("Stock.UpdateQuantity/1",sql,222);
					return ExecuteSQL(null,2);
				}
				catch (Exception ex)
				{
					Tools.LogException("Stock.UpdateQuantity/2",sql,ex);
				}
			return 9199;
		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "Stock.LoadData";
			stockId           = dbConn.ColLong  ("StockId",0,0);
			symbol            = dbConn.ColString("Symbol");
			exchangeCode      = dbConn.ColString("BrokerExchangeCode",0,0);
			primaryExchange   = dbConn.ColString("PrimaryExchange",0,0);
			securityType      = dbConn.ColString("SecType",0,0);
			currencyCode      = dbConn.ColString("CUR",0,0);
			resolution        = dbConn.ColString("Resolution",0,0);
			theDate           = dbConn.ColDate  ("Date",0,0);
			ticksMax          = dbConn.ColLong  ("Ticks",0,0);
			ticksSkip         = dbConn.ColLong  ("Skip",0,0);
//	UNIX timestamps, BigInt (64-bit)
			fromDate          = dbConn.ColBig   ("FromDate",0,0);
			toDate            = dbConn.ColBig   ("ToDate",0,0);
//	UNIX timestamps, Int (32-bit)
//			fromDate          = dbConn.ColLong  ("FromDate",0,0);
//			toDate            = dbConn.ColLong  ("ToDate",0,0);
//	Date format
//			fromDate          = dbConn.ColDate  ("FromDate",0,0);
//			toDate            = dbConn.ColDate  ("ToDate",0,0);
			price             = 0;
			quantity          = 0;
			tickType          = 0;
		}
	}
}
