namespace PCIWebFinAid
{
	public static class AppDetails
	{
		public static string AppName = "Financial Aid Registration";
//		public static string AppDate = "2018/10/18 12:55:37";

		public static string Summary()
		{
			return "<!--" + PCIBusiness.Constants.C_TEXTBREAK()
			     + AppName + PCIBusiness.Constants.C_TEXTBREAK()
			     + "Version " + PCIBusiness.SystemDetails.AppVersion + PCIBusiness.Constants.C_TEXTBREAK()
			     + PCIBusiness.SystemDetails.AppDate + PCIBusiness.Constants.C_TEXTBREAK()
			     + "(c) " + PCIBusiness.SystemDetails.Owner + PCIBusiness.Constants.C_TEXTBREAK()
			     + "Developed by " + PCIBusiness.SystemDetails.Developer + PCIBusiness.Constants.C_TEXTBREAK()
			     + "-->";
		}
	}
}
