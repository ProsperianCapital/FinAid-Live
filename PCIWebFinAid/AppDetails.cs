using System;

namespace PCIWebFinAid
{
	public static class AppDetails
	{
		public static string AppName = "Financial Aid Registration";
//		public static string AppDate = "2018/10/18 12:55:37";

		public static string Summary()
		{
			return "<!--" + Environment.NewLine
			     + AppName + Environment.NewLine
			     + "Version " + PCIBusiness.SystemDetails.AppVersion + Environment.NewLine
			     + PCIBusiness.SystemDetails.AppDate + Environment.NewLine
			     + "(c) " + PCIBusiness.SystemDetails.Owner + Environment.NewLine
			     + "Developed by " + PCIBusiness.SystemDetails.Developer + Environment.NewLine
			     + "-->";
		}
	}
}
