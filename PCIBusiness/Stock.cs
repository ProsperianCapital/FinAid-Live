using System;

namespace PCIBusiness
{
	public class Stock : BaseData
	{
		private int     stockId;
		private string  symbol;
		private string  exchangeCode;

		private double  price;
		private int     quantity;
		private int     tickerType;

		public  int     StockId
		{
			get { return stockId; }
			set { stockId = value; }
		}
		public  int     TickerType
		{
			get { return tickerType; }
			set { tickerType = value; }
		}
		public  double  Price
		{
			get { return price; }
			set { price = value; }
		}
		public  int     Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		public  string  Symbol
		{
			get { return Tools.NullToString(symbol); }
			set { symbol = value.Trim(); }
		}

		public  string  ExchangeCode
		{
			get { return Tools.NullToString(exchangeCode); }
			set { exchangeCode = value.Trim(); }
		}

		public int UpdatePrice()
		{
			if ( stockId > 0 && tickerType > 0 && price > 0 )
				try
				{
					sql = "exec sp_Ins_TickerCurrentRaw"
						 + " @StockID  = " + stockId.ToString()
					    + ",@DateTime = " + Tools.DateToSQL(System.DateTime.Now,5)
					    + ",@TickType = " + tickerType.ToString()
					    + ",@Value    = " + price.ToString();
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
			if ( stockId > 0 && tickerType > 0 && quantity > 0 )
				try
				{
					sql = "exec sp_Ins_TickerCurrentRaw"
						 + " @StockID  = " + stockId.ToString()
					    + ",@DateTime = " + Tools.DateToSQL(System.DateTime.Now,5)
					    + ",@TickType = " + tickerType.ToString()
					    + ",@Value    = " + quantity.ToString();
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
			stockId           = dbConn.ColLong  ("StockId");
			symbol            = dbConn.ColString("Symbol");
			exchangeCode      = dbConn.ColString("BrokerExchangeCode");
			price             = 0;
			tickerType        = 0;
		}
	}
}
