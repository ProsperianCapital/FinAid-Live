using System.Collections.Generic;

namespace PCIBusiness
{
	public class MenuItems : BaseList
	{
		int err;

		public override BaseData NewItem()
		{
			return new   MenuItem();
		}
		public List<MenuItem> LoadMenu(string userCode,string applicationCode,string languageCode,string languageDialectCode)
		{
			sql = "exec sp_Get_BackOfficeMenuD @UserCode="            + Tools.DBString(userCode)
	                                     + ",@ApplicationCode="     + Tools.DBString(applicationCode)
	                                     + ",@LanguageCode="        + Tools.DBString(languageCode)
	                                     + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
			err = ExecuteSQL(null,false,true);
			if ( err > 0 )
			{
				Tools.LogException("LoadMenu/5","err="+err.ToString()+" ("+sql+")",null,this);
				return null;
			}

			string         descr;
			string         level1   = "X";
			string         level2   = "X";
			string         level3   = "X" ;
			string         level4   = "X";
			MenuItem       menu1    = null;
			MenuItem       menu2    = null;
			MenuItem       menu3    = null;
			MenuItem       menu4    = null;
			List<MenuItem> menuList = new List<MenuItem>();

			while ( ! dbConn.EOF )
			{
				descr = dbConn.ColString("Level1ItemDescription");
				if ( descr.Length > 0 && descr != level1 )
				{
					level1      = descr;
					level2      = "X";
					level3      = "X";
					level4      = "X";
					menu1       = new MenuItem();
					menu1.Level = 1;
					menu1.LoadData(dbConn);
					menuList.Add(menu1);
				}
				descr = dbConn.ColString("Level2ItemDescription");
				if ( descr.Length > 0 && descr != level2 )
				{
					level2      = descr;
					level3      = "X";
					level4      = "X";
					menu2       = new MenuItem();
					menu2.Level = 2;
					menu2.LoadData(dbConn);
					menu1.SubItems.Add(menu2);
				}
				descr = dbConn.ColString("Level3ItemDescription");
				if ( descr.Length > 0 && descr != level3 )
				{
					level3      = descr;
					level4      = "X";
					menu3       = new MenuItem();
					menu3.Level = 3;
					menu3.LoadData(dbConn);
					menu2.SubItems.Add(menu3);
				}
				descr = dbConn.ColString("Level4ItemDescription");
				if ( descr.Length > 0 && descr != level4 )
				{
					level4      = descr;
//					level5      = "X"; // We don't have 5 levels
					menu4       = new MenuItem();
					menu4.Level = 4;
					menu4.LoadData(dbConn);
					menu3.SubItems.Add(menu4);
				}
				dbConn.NextRow();
			}
			return menuList;
		}
	}
}