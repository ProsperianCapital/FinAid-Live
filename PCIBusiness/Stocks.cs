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

		public int LoadAll()
		{
			Tools.LogInfo("Stocks.LoadAll",sql,10);
			sql = "exec sp_Get_StockListA";
			return LoadDataFromSQL();
		}
	}
}