namespace PCIWebFinAid
{
	public partial class SessionGeneral
	{
//		User details
		private string  userCode;
		private string  userName;
		private string  accessType;

//		Admin stuff

//		Client stuff
		private string  contractCode;
		private string  productCode;
		private string  languageCode;
		private string  languageDialectCode;

//		System details
//		private string  startPage;
//		private string  applicationCode;

		public  string  UserCode
		{
			get { return PCIBusiness.Tools.NullToString(userCode); }
			set { userCode = value.Trim(); }
		}
		public  string  UserName
		{
			get { return PCIBusiness.Tools.NullToString(userName); }
			set { userName = value.Trim(); }
		}
		public  string  ContractCode
		{
			get { return PCIBusiness.Tools.NullToString(contractCode); }
			set { contractCode = value.Trim(); }
		}
		public  bool    AdminUser
		{
			get { return AccessType == "A"; }
		}
		public  string  AccessName
		{
			get
			{
				if ( AccessType == "N" ) return "Client";
				if ( AccessType == "A" ) return "Admin";
				if ( AccessType == "C" ) return "CRM Agent";
				if ( AccessType == "X" ) return "Not secure";
				return "AccessType " + AccessType;
			}
		}

		public  string  AccessType
		{
			get
			{
				if ( UserCode.Length < 1 )
					accessType = "Q";
				else
				{
					accessType = PCIBusiness.Tools.NullToString(accessType).ToUpper();
					if ( accessType.Length < 1 )
						accessType = "N"; // Client/Normal = "N", Admin = "A"
					else if ( accessType.Length > 1 )
						accessType = accessType.Substring(0,1);
				}
				return accessType;
			}
			set { accessType = value.Trim().ToUpper(); }
		}

		public string   StartPage
		{
			get
			{
				if ( AccessType == "A" ) // Admin
					return "XHome.aspx";
				if ( AccessType == "N" ) // Client
					return "pgViewProductDashboard.aspx";
				if ( AccessType == "C" ) // CRM Agent
					return "pgViewProductDashboard.aspx";
				return "pgLogonCRM.aspx";

//				if ( PCIBusiness.Tools.NullToString(startPage).Length < 6 )
//					if ( AccessType == "A" ) // Admin
//						startPage = "XHome.aspx";
//					else if ( AccessType == "N" ) // Client
//						startPage = "pgViewProductDashboard.aspx";
//					else if ( AccessType == "C" ) // CRM Agent
//						startPage = "pgViewProductDashboard.aspx";
//					else
//						startPage = "pgLogonCRM.aspx";
//				return startPage;
			}
//			set { startPage = value.Trim(); }
		}
		public string   LogonPage
		{
			get
			{
				if ( AccessType == "A" ) // Admin
					return "pgLogon.aspx";
				return "pgLogonCRM.aspx";
			}
		}
		public  string  ProductCode
		{
			get { return productCode; }
			set { productCode = value.Trim(); }
		}
		public  string  LanguageCode
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(languageCode) )
					return "ENG";
				return languageCode;
			}
			set { languageCode = value.Trim(); }
		}
		public  string  LanguageDialectCode
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(languageDialectCode) )
					return "0001";
				return languageDialectCode;
			}
			set { languageDialectCode = value.Trim(); }
		}

		public int      CheckExpiry
		{
			get {	return ( userCode.Length > 0 ? 1 : 0 ); }
		}

		public void Clear()
		{
			userCode            = "";
			contractCode        = "";
			userName            = "";
			accessType          = "";
			productCode         = "";
			languageCode        = "";
			languageDialectCode = "";
//			startPage           = "";
		}

		public SessionGeneral()
		{
			Clear();
		}
		~SessionGeneral()
		{
			Clear();
		}
	}
}
