namespace PCIBusiness
{
	public class Providers : BaseList
	{

		public override BaseData NewItem()
		{
			return new Provider();
		}

		public Provider LoadOne(string bureauCode)
		{
		//	sql = "exec Blah " + Tools.DBString(bureauCode);
		//	if ( LoadDataFromSQL(1) > 0 )
		//		return (Provider)Item(0);
		//	return null;

			return null;
		}

		public int LoadAll()
		{
		//	sql = "exec Blah";
		//	return LoadDataFromSQL();

			return 0;
		}
	}
}