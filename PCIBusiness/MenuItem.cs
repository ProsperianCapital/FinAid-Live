using System;
using System.Collections;
using System.Collections.Generic;

namespace PCIBusiness
{
	public class MenuItem : BaseData
	{
		private byte   level;
		private string menuCode;
		private string menuName;
		private string menuDescription;
		private string routerLink;
		private string blocked;
		private string url;
		private string imageName;
		private string displayImageOrText;
		private List<MenuItem> subItems;

		public  byte    Level
		{
			get { return level; }
			set { level = value; }
		}
		public  string  Code
		{
			get { return Tools.NullToString(menuCode); }
			set { menuCode = value.Trim(); }
		}
		public string   Name
		{
			get { return Tools.NullToString(menuName); }
			set { menuName = value.Trim(); }
		}
		public string   Description
		{
			get { return Tools.NullToString(menuDescription); }
			set { menuDescription = value.Trim(); }
		}
		public string   DisplayImageOrText
		{
			get { return Tools.NullToString(displayImageOrText); }
		}
		public string   ImageName
		{
			get { return Tools.NullToString(imageName); }
		}
		public string   Blocked
		{
			get { return Tools.NullToString(blocked); }
		}
		public string   RouterLink
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(routerLink) )
					return Tools.NullToString(url);
				return Tools.NullToString(routerLink);
			}
		}
		public string   URL
		{
			get
			{
				url = Tools.NullToString(url);
				if ( url.ToUpper() == "TBA" )
					url = "XHome.aspx";
				return url;
			}
			set { url = value.Trim(); }
		}
		public List<MenuItem> SubItems
		{
			get
			{
				if ( subItems == null )
					subItems = new List<MenuItem>();
				return subItems;
			}
		}

		public override void LoadData(DBConn dbConn)
		{
			string x           = "Level" + level.ToString();
//			menuCode           = dbConn.ColString("MenuItemCode");
			menuCode           = dbConn.ColString(x+"ItemCode");
			menuName           = dbConn.ColString(x+"ItemDescription");
			menuDescription    = dbConn.ColString(x+"ItemDescription");
			imageName          = dbConn.ColString("MenuLevel1ImageFileName");
			displayImageOrText = dbConn.ColString("DisplayMenuLevel1Image");
			url                = dbConn.ColString("URL");
			routerLink         = dbConn.ColString("RouterLink",0,0);
			blocked            = dbConn.ColString("Blocked",0,0);

//	See RouterLink "Get" method
//			if ( routerLink.Length < 1 && url.Length > 0 )
//				routerLink      = url;

//	Testing
//			url                = dbConn.ColString("URL") + "?UserCode={UserCode}";
		}
	}
}