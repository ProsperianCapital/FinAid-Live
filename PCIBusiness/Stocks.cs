namespace PCIBusiness
{
	public class Stocks : BaseList
	{
		public override BaseData NewItem()
		{
			return new Stock();
		}

		public Stock LoadOne(string stockSymbol)
		{
		//	sql = "exec Blah " + Tools.DBString(bureauCode);
		//	if ( LoadDataFromSQL(1) > 0 )
		//		return (Provider)Item(0);
		//	return null;

			return null;
		}

		public int LoadAll(Constants.TradingProvider tradeProvider,int tickerType=0,string secType="",int counter=0)
		{
		//	if ( secType.ToUpper() == "STK-HISTORY" )

			if ( tickerType == (int)Constants.TickerType.FinnHubStockHistory )
				sql = "exec sp_Get_StockCandles @ProviderCode=" + Tools.DBString(Tools.TradingProviderCode(tradeProvider));

			else if ( tickerType == (int)Constants.TickerType.FinnHubStockTicks )
				sql = "exec sp_GetTickList @ProviderCode=" + Tools.DBString(Tools.TradingProviderCode(tradeProvider))
				    + ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development ? ",@Test="+counter.ToString() : "" );

			else if ( secType.Length > 0 )
				sql = "exec sp_Get_StockListB @SecType = " + Tools.DBString(secType);

			else
				sql = "exec sp_Get_StockListB";

			Tools.LogInfo("Stocks.LoadAll",sql,10);
			return LoadDataFromSQL();
		}
	}
}