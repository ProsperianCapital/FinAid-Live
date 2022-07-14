using System;

namespace PCIWebFinAid
{
	public static class AppDetails
	{
		public static string Summary()
		{
			return "<!--" + Environment.NewLine
			     + SystemDetails.AppName + Environment.NewLine
			     + "App Version " + SystemDetails.AppVersion + " ("
			     + SystemDetails.AppDate + ")" + Environment.NewLine
			     + "DLL Version " + PCIBusiness.SystemDetails.AppVersion + " ("
			     + PCIBusiness.SystemDetails.AppDate + ")" + Environment.NewLine
			     + "(c) " + PCIBusiness.SystemDetails.Owner + Environment.NewLine
			     + "Developed by " + PCIBusiness.SystemDetails.Developer + Environment.NewLine
			     + "-->";
		}
	}
}
