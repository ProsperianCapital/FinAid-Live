using System;
using System.Xml;
using System.Web;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using PCIBusiness;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class UIApplicationQuery : BasePage
	{
		private byte          inputDataType;
		private string        inputDataJSON;
		private XmlDocument   inputDataXML;

		private int           errorCode;
		private string        errorMsg;
		private string        sql;
		private string        sqlSP;
		private StringBuilder json;

		private string        queryName;
		private string        userCode;
		private string        applicationCode;
		private string        countryCode;
		private string        languageCode;
		private string        languageDialectCode;
		private string        mobileNumber;
		private string        appMode;

		private enum ResultCode
		{
			OK = 77777
		}

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( Page.IsPostBack )
				SendJSON(10001,"Internal error");
			else
				QueryData();
		}

		private int QueryData()
		{
//			string msg = "";

			try
			{
				string secretKey   = "";
				string contentType = Request.ContentType.Trim().ToUpper();
				inputDataType      = (byte)Constants.WebDataType.FormPost;
				errorCode          = 0;
				errorMsg           = "";
//				msg                = msg + contentType;

				if ( contentType.Contains("JSON") || contentType.Contains("XML") )
				{
					System.IO.Stream strIn = Request.InputStream;
					int              ch    = strIn.ReadByte();
					inputDataJSON          = "";

					while ( ch >= 0 )
					{
						inputDataJSON = inputDataJSON + (Convert.ToChar(ch)).ToString();
						ch            = strIn.ReadByte();
					}
					strIn.Close();
					strIn         = null;
					inputDataJSON = inputDataJSON.Replace(Environment.NewLine,"");
//					msg           = msg + "=" + inputDataJSON;

					if ( contentType.Contains("JSON") )
						inputDataType = (byte)Constants.WebDataType.JSON;

					else if ( contentType.Contains("XML") )
						try
						{
							inputDataType = (byte)Constants.WebDataType.XML;
							inputDataXML  = new XmlDocument();
							inputDataXML.LoadXml(inputDataJSON);
						}
						catch
						{ }
				}

				queryName           = ParmValue("QueryName");
				applicationCode     = ParmValue("ApplicationCode");
				countryCode         = ParmValue("CountryCode");
				languageCode        = ParmValue("LanguageCode");
				languageDialectCode = ParmValue("LanguageDialectCode");
				userCode            = ParmValue("UserCode");
				secretKey           = ParmValue("SecretKey");
				mobileNumber        = ParmValue("MobileNumber");
				appMode             = ParmValue("AppMode").ToUpper();

				if ( inputDataType != (byte)Constants.WebDataType.FormPost )
					Tools.LogInfo("QueryData/10","dataType="+inputDataType.ToString()
					                          +", queryName="+queryName
					                          +", applicationCode="+applicationCode
					                          +", countryCode="+countryCode
					                          +", languageCode="+languageCode
					                          +", languageDialectCode="+languageDialectCode
					                          +", userCode="+userCode
					                          +", mobileNumber="+mobileNumber,220,this);
	
				if ( Tools.SystemLiveTestOrDev() != Constants.SystemMode.Development && secretKey != "7e6415a7cb790238fd12430a0ce419b3" )
					return SendJSON(10005,"Invalid secret key");

				if ( queryName.Length < 1 )
					return SendJSON(10006,"Missing query name");

				queryName = queryName.ToUpper();

				if ( json == null )
					json = new StringBuilder();
				else
					json.Length = 0;

				if ( queryName == ("Test").ToUpper() )
					GetTestData();

				else if ( queryName == ("FinTechHelp").ToUpper() )
					GetHelp();

				else if ( queryName == ("FinTechGetApplicationCode").ToUpper() )
					GetApplicationCode();

				else if ( queryName == ("FinTechGetApplicationInfo").ToUpper() )
					GetApplicationInfo();

				else if ( queryName == ("FinTechGetPageInfoStart").ToUpper() )
					GetPageInfoStart();

				else if ( queryName == ("FinTechGetPageInfoLogin").ToUpper() )
					GetPageInfoLogin();

				else if ( queryName == ("FinTechLogOn").ToUpper() )
					LogOn();

				else if ( queryName == ("FinTechGetPageInfo2FA").ToUpper() )
					GetPageInfo2FA();

				else if ( queryName == ("FinTechLogOn2FA").ToUpper() )
					LogOn2FA();

				else if ( queryName == ("FinTechGetMenuStructure").ToUpper() )
					GetMenuStructure();

				else if ( queryName == ("FinTechDashboard").ToUpper() )
					Dashboard();

				else if ( queryName == ("FinTechGeteWalletList").ToUpper() )
					GetEWalletList();

				else if ( queryName == ("FinTechGeteWalletAccountCURList").ToUpper() )
					GeteWalletAccountCURList();

				else if ( queryName == ("FinTechGetPageInfoCreateNeweWallet").ToUpper() )
					GetPageInfoCreateNeweWallet();

				else if ( queryName == ("FinTechGeteWalletFundingMethodList").ToUpper() )
					GeteWalletFundingMethodList();

				else if ( queryName == ("FinTechCreateNeweWalletAccount").ToUpper() )
					CreateNeweWalletAccount();

				else if ( queryName == ("FinTechGetPageInfoEditeWalletAccountDescription").ToUpper() )
					GetPageInfoEditeWalletAccountDescription();

				else if ( queryName == ("FinTechEditeWalletAccountDescription").ToUpper() )
					EditeWalletAccountDescription();

				else if ( queryName == ("FinTechGetiSOSInfo").ToUpper() )
					GetiSOSInfo();

				else if ( queryName == ("FinTechGetiSOSInfo(v2)").ToUpper() )
					GetiSOSInfo(2);

//				else if ( queryName == ("FinTechGetiSOSInfo(Test)").ToUpper() )
//					GetiSOSInfo(199);

				else if ( queryName == ("FinTechRegisteriSOSEvent").ToUpper() )
					RegisteriSOSEvent();

				else if ( queryName == ("FinTechSendSMS").ToUpper() )
					SendSMS();

				else if ( queryName == ("FinTechGetGuruCalendar").ToUpper() )
					GetGuruCalendar();

//	Should this be here at all?
				else if ( queryName == ("FinTechGetPageContent").ToUpper() )
					GetPageContent();
//	Should this be here at all?

				else
					SetError(10007,"Invalid query name");

				return SendJSON();
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("QueryData/99","",ex,this);
			}

			return 0;
		}

		private int SetError(int code,string msg="",string logInfo="",string logError="")
		{
			if ( code > 0 )
			{
				errorCode   = code;
				errorMsg    = ( msg.Length > 0 ? msg : "Internal error 18888" );
				json.Length = 0;
			}
			else
			{
				errorCode = 0;
				errorMsg  = "";
			}

			if ( logInfo.Length > 0 )
				Tools.LogInfo     ("SetError/" + code.ToString(),msg + " (" + logInfo + ")",222,this);

			if ( logError.Length > 0 )
				Tools.LogException("SetError/" + code.ToString(),msg + " (" + logError + ")",null,this);

			return errorCode;
		}

		private int CheckParameters(string parmsRequired)
		{
			try
			{
				SetError(0);
				parmsRequired = "," + parmsRequired.Trim().ToUpper() + ",";

				if ( parmsRequired.Contains(",APP,")     && applicationCode.Length < 1 )
					SetError(11005,"Parameter ApplicationCode is missing");
				if ( parmsRequired.Contains(",COUNTRY,") && countryCode.Length < 1 )
					SetError(11010,"Parameter CountryCode is missing");
				if ( parmsRequired.Contains(",LANG,")    && languageCode.Length < 1 )
					SetError(11015,"Parameter LanguageCode is missing");
				if ( parmsRequired.Contains(",DIALECT,") && languageDialectCode.Length < 1 )
					SetError(11020,"Parameter LanguageDialectCode is missing");
				if ( parmsRequired.Contains(",USER,")    && userCode.Length < 1 )
					SetError(11025,"Parameter UserCode is missing");
				if ( parmsRequired.Contains(",MOBILE,")  && mobileNumber.Length < 8 )
					SetError(11030,"Parameter MobileNumber is missing");
			}
			catch (Exception ex)
			{
				SetError(11099,"Internal error 19999");
			}
			return errorCode;
		}

		private int GetHelp()
		{
			json.Append ( "\"FinTechAPI\":"
		               + Tools.JSONPair("Help","",1,"{")
		               + Tools.JSONPair("GetApplicationCode","")
		               + Tools.JSONPair("GetApplicationInfo","App,Country,Lang,Dialect")
		               + Tools.JSONPair("GetPageInfoStart"  ,"App,Country,Lang,Dialect")
		               + Tools.JSONPair("GetPageInfoLogin"  ,"App,Country,Lang,Dialect")
		               + Tools.JSONPair("GetPageInfo2FA"    ,"App,Country,Lang,Dialect")
		               + Tools.JSONPair("GetMenuStructure"  ,"App,Lang,Dialect,User")
		               + Tools.JSONPair("RegisteriSOSEvent" ,"Mobile,ButtonCode,Location")
		               + Tools.JSONPair("GetiSOSInfo"       ,"Mobile")
		               + Tools.JSONPair("GetPageContent"    ,"")
		               + Tools.JSONPair("LogOn"             ,"App,UserName,UserPassword")
		               + Tools.JSONPair("LogOn2FA"          ,"App,User,2FAChannelCode")
		               + Tools.JSONPair("GetEWalletList"    ,"App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("Dashboard"         ,"App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("GetGuruCalendar"   ,"GuruCode,StartDate,EndDate")
		               + Tools.JSONPair("GetPageInfoCreateNeweWallet"             ,"App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("GeteWalletAccountCURList"                ,"App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("GeteWalletFundingMethodList"             ,"App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("GetPageInfoEditeWalletAccountDescription","App,Country,Lang,Dialect,User")
		               + Tools.JSONPair("CreateNeweWalletAccount"                 ,"App,Country,Lang,Dialect,User,CurrencyCode,FundingMethodCode,eWalletDescription")
		               + Tools.JSONPair("EditeWalletAccountDescription"           ,"App,Country,Lang,Dialect,User,eWalletAccountCode,eWalletDescription")
		               + Tools.JSONPair("SendSMS","App,Mobile,Message",1,"","}"));
			return 0;
		}

		private int GetApplicationCode()
		{
		//	json.Append ( Tools.JSONPair("ApplicationCode",Tools.SystemCode(Constants.ApplicationCode.PayPayYa)) );
			json.Append ( Tools.JSONPair("ApplicationCode",Tools.SystemCode(Constants.ApplicationCode.Mobile)) );
			return 0;
		}

		private int GetApplicationInfo()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("ApplicationDescription","Pay Pay Ya")
			            + Tools.JSONPair("ApplicationURL","https://www.paypayya.com")
			            + Tools.JSONPair("ApplicationStatusCode","001")
			            + Tools.JSONPair("ApplicationStatusDescription","Running") );
			return 0;
		}

		private int GetPageInfoStart()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("StartHeading","Mobile Banking User Interface")
			            + Tools.JSONPair("StartLogo","PayPayYaWide.png")
			            + Tools.JSONPair("StartButtonText","Login Here") );
			return 0;
		}

		private int GetPageInfoLogin()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("LoginHeading","Customer Login")
			            + Tools.JSONPair("LoginLogo","PayPayYa.png")
			            + Tools.JSONPair("LoginLine1","Your Email Address")
			            + Tools.JSONPair("LoginLine2","Password")
			            + Tools.JSONPair("LoginButtonText","Login")
			            + Tools.JSONPair("ForgotPasswordText","Forgot Password") );
			return 0;
		}

		private int GetPageInfo2FA()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("2FALine1Text","Please enter your verification code")
			            + Tools.JSONPair("2FALine2Text","The verification code is sent to your mobile number. It will expire in 5 minutes")
			            + Tools.JSONPair("2FAButtonText","Submit")
			            + Tools.JSONPair("2FAResendText","Resend verification code") );
			return 0;
		}

		private int RegisteriSOSEvent()
		{
			if ( CheckParameters("Mobile") > 0 )
				return errorCode;

			string buttonCode = ParmValue("ButtonCode");
			string location   = ParmValue("Location");
			sqlSP             = "sp_iSOS_Insert_SOSAppRegister";

			using (MiscList mList = new MiscList())
				try
				{
					sql = "exec " + sqlSP + " @MobileNumber=" + Tools.DBString(mobileNumber)
					                      + ",@ButtonCode="   + Tools.DBString(buttonCode)
					                      + ",@Location="     + Tools.DBString(location);
					if ( mList.UpdateQuery(sql) == 0 )
						return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("RegisteriSOSEvent",sql,ex,this);
				}

			return SetError(13999,"Internal error: SQL " + sqlSP,sql,sql);
		}

		private int GetPageContent()
		{
//			Don't need language
//			if ( CheckParameters("Lang") > 0 )
//				return errorCode;

			sqlSP = "sp_iSOS_Get_iSOSContent";

			using (MiscList mList = new MiscList())
				try
				{
					if ( appMode == "TEST" )
						sql = "create table #X (FieldCode varchar(50),MobileFieldName varchar(100),MobileFieldValue varchar(100)) "
						    + "insert into  #X values "
						    + "('1','Name 1','Value 1'),"
						    + "('2','Name 2','Value 2'),"
						    + "('3','Name 3','Value 3'),"
						    + "('4','Name 4','Value 4') "
						    + "select *,'ENG' as LanguageCode from #X";
					else if ( languageCode.Length == 3 )
						sql = "exec " + sqlSP + " @LanguageCode=" + Tools.DBString(languageCode);
					else
						sql = "exec " + sqlSP;

					if ( mList.ExecQuery(sql,0,"",false) != 0 )
						return SetError(11705,"Internal error: SQL " + sqlSP,sql,sql);

					if ( mList.EOF )
						return SetError(11710,"No data returned: SQL " + sqlSP,sql);

					json.Append("\"FieldData\":[");

					while ( ! mList.EOF )
					{
						json.Append ( Tools.JSONPair("FieldCode"       ,mList.GetColumn("FieldCode"), 1, "{")
						            + Tools.JSONPair("LanguageCode"    ,mList.GetColumn("LanguageCode"))
						            + Tools.JSONPair("MobileFieldName" ,mList.GetColumn("MobileFieldName"))
						            + Tools.JSONPair("MobileFieldValue",mList.GetColumn("MobileFieldValue"), 1, "", "},") );
						mList.NextRow();
					}
					json.Append("]");
					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("GetPageContent",sql,ex,this);
				}

			return SetError(11715,"Internal error: SQL " + sqlSP,sql,sql);
		}


		private int GetGuruCalendar()
		{
			string   guruCode  = ParmValue("GuruCode");
			string   startTime = ParmValue("StartDate");
			string   endTime   = ParmValue("EndDate");
			DateTime sessionDate;

			sql = ( guruCode.Length  > 0 ? ",@GuruCode=" + Tools.DBString(guruCode) : "" );

			if ( startTime.Length > 0 )
			{
				sessionDate = Tools.StringToDate(startTime,2,3); // yyyy/mm/dd hh:mm
				if ( sessionDate <= Constants.DateNull )
					sessionDate    = Tools.StringToDate(startTime,1,3); // dd/mm/yyyy hh:mm
				if ( sessionDate <= Constants.DateNull )
					return SetError(21710,"Invalid start date");
				sql = sql + ",@StartDateTime=" + Tools.DateToSQL(sessionDate,23);
			}

			if ( endTime.Length > 0 )
			{
				sessionDate = Tools.StringToDate(endTime,2,3); // yyyy/mm/dd hh:mm
				if ( sessionDate <= Constants.DateNull )
					sessionDate    = Tools.StringToDate(endTime,1,3); // dd/mm/yyyy hh:mm
				if ( sessionDate <= Constants.DateNull )
					return SetError(21720,"Invalid end date");
				sql = sql + ",@EndDatetime=" + Tools.DateToSQL(sessionDate,24);
			}

			sqlSP  = "sp_LG_Get_Calendar";
			if ( sql.StartsWith(",") )
				sql = "  " + sql.Substring(1);
			sql = "exec " + sqlSP + sql;

			using (MiscList mList = new MiscList())
				try
				{
					if ( mList.ExecQuery(sql,0,"",false) != 0 )
						return SetError(21730,"Internal error: SQL " + sqlSP,sql,sql);

					if ( mList.EOF )
						return SetError(21740,"No data returned: SQL " + sqlSP,sql);

					json.Append("\"Calendar\":[");

					while ( ! mList.EOF )
					{
						sessionDate = mList.GetColumnDate("SessionStartDateTime");
						startTime   = sessionDate.Hour.ToString().PadLeft(2,'0') + ":" + sessionDate.Minute.ToString().PadLeft(2,'0');
						sessionDate = mList.GetColumnDate("SessionEndDateTime");
						endTime     = sessionDate.Hour.ToString().PadLeft(2,'0') + ":" + sessionDate.Minute.ToString().PadLeft(2,'0');
						json.Append ( Tools.JSONPair("GuruName"             ,mList.GetColumn("GuruName"), 1, "{")
						            + Tools.JSONPair("SessionDate"          ,Tools.DateToString(sessionDate,2))
						            + Tools.JSONPair("StartTime"            ,startTime)
						            + Tools.JSONPair("EndTime"              ,endTime)
					//	            + Tools.JSONPair("GuruStatusCode"       ,mList.GetColumn("GuruStatusCode"))
						            + Tools.JSONPair("GuruStatusDescription",mList.GetColumn("GuruStatusDescription"))
						            + Tools.JSONPair("ClientDetails"        ,mList.GetColumn("ClientDetails"), 1, "", "},") );
						mList.NextRow();
					}
					json.Append("]");
					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("GetGuruCalendar",sql,ex,this);
				}

			return SetError(21750,"Internal error: SQL " + sqlSP,sql,sql);
		}

		private int GetiSOSInfo(byte verNo=0) // Use verNo = 199 for testing
		{
			if ( CheckParameters("Mobile") > 0 )
				return errorCode;

			if ( appMode == "TEST" )
				sqlSP = "sp_iSOS_Get_iSOSAppDataA";
			else if ( verNo == 2 )
				sqlSP = "sp_iSOS_Get_iSOSAppDataB";
			else
				sqlSP = "sp_iSOS_Get_iSOSAppDataA";

			using (MiscList mList = new MiscList())
				try
				{
					sql = "exec " + sqlSP + " @MobileNumber=" + Tools.DBString(mobileNumber);

					if ( mList.ExecQuery(sql,0,"",false) != 0 )
						return SetError(12905,"Internal error: SQL " + sqlSP,sql,sql);

					if ( mList.EOF )
						return SetError(12910,"No data returned: SQL " + sqlSP,sql);

					json.Append ( Tools.JSONPair("ContractCode"            ,mList.GetColumn("ContractCode"))
					            + Tools.JSONPair("ClientName"              ,mList.GetColumn("ClientName"))
					            + Tools.JSONPair("CountryCode"             ,mList.GetColumn("CountryCode"))
					            + Tools.JSONPair("CountryFlagImageCode"    ,mList.GetColumn("CountryFlagImageCode"))
					            + Tools.JSONPair("CountryFlagImageFileName",mList.GetColumn("CountryFlagImageFileName"))
					            + Tools.JSONPair("LanguageCode"            ,mList.GetColumn("LanguageCode"))
					            + "\"Buttons\":[" );

					string buttonText;
					string colName;

					for ( int k = 1 ; k < 100 ; k++ )
					{
						if ( k < 10 )
							colName = "Button0" + k.ToString();
						else
							colName = "Button"  + k.ToString();
						buttonText = mList.GetColumn(colName+"Text",0);
						if ( buttonText.Length < 1 )
							break;
						json.Append ( Tools.JSONPair("Button"       ,k.ToString(),1,"{")
						            + Tools.JSONPair("Text"         ,buttonText)
						            + Tools.JSONPair("ImageCode"    ,mList.GetColumn(colName+"ImageCode"))
						            + Tools.JSONPair("ImageFileName",mList.GetColumn(colName+"ImageFileName"))
						            + Tools.JSONPair("NumberToDial" ,mList.GetColumn(colName+"NumberToDial"),1,"","") );
						if ( verNo == 2 )
							json.Append ( Tools.JSONPair("SMSText"   ,mList.GetColumn(colName+"SMSText"),1,",","") );
						else if ( appMode == "TEST" )
							json.Append ( Tools.JSONPair("SMSText"   ,"This is some SMS text",1,",","") );
						json.Append("},");
					}
					JSONAppend("]");
					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("GetiSOSInfo",sql,ex,this);
				}

			return SetError(12999,"Internal error: SQL " + sqlSP,sql,sql);
		}

		private int LogOn()
		{
			if ( CheckParameters("App") > 0 )
				return errorCode;

			string userName = ParmValue("UserName");
			string passWord = ParmValue("UserPassword");

			if ( userName.Length < 3 || passWord.Length < 3 )
				return SetError(11105,"Invalid user name and/or password");

			json.Append ( Tools.JSONPair("UserCode","013")
			            + Tools.JSONPair("UserDisplayName","Sheila Coleman")
			            + Tools.JSONPair("CountryCode","RSA")
			            + Tools.JSONPair("LanguageCode","ENG")
			            + Tools.JSONPair("LanguageDialectCode","0002")
			            + Tools.JSONPair("2FAChannelCode",((int)Constants.SystemPassword.MobileDev).ToString()) );
			return 0;
		}

		private int LogOn2FA()
		{
			if ( CheckParameters("App,User") > 0 )
				return errorCode;

			string twoFA = ParmValue("2FAChannelCode");

			if ( twoFA != ((int)Constants.SystemPassword.MobileDev).ToString() )
				return SetError(11205,"Invalid verification code");

			return 0;
		}

		private int GetEWalletList()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			sqlSP = "sp_FinTechGeteWalletList";

			using (MiscList mList = new MiscList())
				try
				{
					sql = "exec " + sqlSP + " @AppplicationCode="    + Tools.DBString(applicationCode)
					                      + ",@UserCode="            + Tools.DBString(userCode)
					                      + ",@CountryCode="         + Tools.DBString(countryCode)
					                      + ",@LanguageCode="        + Tools.DBString(languageCode)
					                      + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

					if ( mList.ExecQuery(sql,0,"",false) != 0 )
						return SetError(11305,"Internal error: SQL " + sqlSP,sql,sql);

					if ( mList.EOF )
						return SetError(11310,"No data returned: SQL " + sqlSP,sql);

					int    k = 0;
					int    p1;
					int    p2;
					string bal;

					json.Append ( Tools.JSONPair("HeadingText1"               ,mList.GetColumn("HeadingText1"))
					            + Tools.JSONPair("HeadingText2"               ,mList.GetColumn("HeadingText2"))
					            + Tools.JSONPair("AccountNumberLabelText"     ,mList.GetColumn("AccountNumberLabelText"))
					            + Tools.JSONPair("AccountDescriptionLabelText",mList.GetColumn("AccountDescriptionLabelText"))
					            + Tools.JSONPair("AccountHolderLabelText"     ,mList.GetColumn("AccountHolderLabelText"))
					            + "\"Accounts\": [" );

					while ( ! mList.EOF )
					{
						k++;
						if ( k > 1 )
							json.Append(",");

						bal = mList.GetColumn("Balance");
						p1  = bal.LastIndexOf(",");
						p2  = bal.LastIndexOf(".");
						if ( p1 > 0 && p1 > p2 && bal.Length > p1 + 3 )
							bal = bal.Substring(0,p1+3);
						if ( p2 > 0 && p2 > p1 && bal.Length > p2 + 3 )
							bal = bal.Substring(0,p2+3);

						json.Append ( Tools.JSONPair("eWalletAccountCode"      ,mList.GetColumn("eWalletAccountCode"), 1, "{")
					               + Tools.JSONPair("eWalletDescription"      ,mList.GetColumn("eWalletDescription"))
					               + Tools.JSONPair("FlagImageCode"           ,mList.GetColumn("FlagImageCode"))
					               + Tools.JSONPair("CUR"                     ,mList.GetColumn("CUR"))
					               + Tools.JSONPair("Balance"                 ,bal, 11)
					               + Tools.JSONPair("AccountIconImageCode"    ,mList.GetColumn("AccountIconImageCode"))
					               + Tools.JSONPair("AssociationIconImageCode",mList.GetColumn("AssociationIconImageCode"), 1, "", "}") );
						mList.NextRow();
					}
					json.Append("]");

					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("GetEWalletList",sql,ex,this);
				}

			return SetError(11380,"Internal error: SQL " + sqlSP,sql,sql);
		}

		private int GetMenuStructure()
		{
			if ( CheckParameters("App,Lang,Dialect,User") > 0 )
				return errorCode;

			List<MenuItem> menuList;
			using ( MenuItems menuItems = new MenuItems() )
				menuList = menuItems.LoadMenu(userCode,applicationCode,languageCode,languageDialectCode);

			if ( menuList == null || menuList.Count < 1 )
			{
				SetError(11405,"Internal error retrieving menu structure");
				return 0;
			}

			int k = 0;
			json.Append("\"Menu"+(++k).ToString()+"\":[");
			foreach (MenuItem m1 in menuList)
			{
				json.Append ( Tools.JSONPair("MenuLevel","1",11,"{")
				            + Tools.JSONPair("MenuDescription",m1.Description)
				            + Tools.JSONPair("Blocked",m1.Blocked)
				            + Tools.JSONPair("MenuImage",m1.ImageName)
				            + Tools.JSONPair("SubItems",m1.SubItems.Count.ToString(),11)
				            + Tools.JSONPair("RouterLink",m1.RouterLink) );
				if ( m1.SubItems.Count > 0 )
				{
					json.Append("\"Menu"+(++k).ToString()+"\":[");
					foreach (MenuItem m2 in m1.SubItems)
					{
						json.Append ( Tools.JSONPair("MenuLevel","2",11,"{")
						            + Tools.JSONPair("MenuDescription",m2.Description)
				                  + Tools.JSONPair("Blocked",m2.Blocked)
				                  + Tools.JSONPair("SubItems",m2.SubItems.Count.ToString(),11)
				                  + Tools.JSONPair("RouterLink",m2.RouterLink) );
						if ( m2.SubItems.Count > 0 )
						{
							json.Append("\"Menu"+(++k).ToString()+"\":[");
							foreach (MenuItem m3 in m2.SubItems)
							{
								json.Append ( Tools.JSONPair("MenuLevel","3",11,"{")
								            + Tools.JSONPair("MenuDescription",m3.Description)
				                        + Tools.JSONPair("Blocked",m3.Blocked)
				                        + Tools.JSONPair("SubItems",m3.SubItems.Count.ToString(),11)
				                        + Tools.JSONPair("RouterLink",m3.RouterLink) );
								if ( m3.SubItems.Count > 0 )
								{
									json.Append("\"Menu"+(++k).ToString()+"\":[");
									foreach (MenuItem m4 in m3.SubItems)
										json.Append ( Tools.JSONPair("MenuLevel","4",11,"{")
										            + Tools.JSONPair("MenuDescription",m4.Description)
				                              + Tools.JSONPair("Blocked",m4.Blocked)
				                              + Tools.JSONPair("SubItems","0",11)
				                              + Tools.JSONPair("RouterLink",m4.RouterLink)
										            + Tools.JSONPair("Url",m4.URL,1,"","},") );
									JSONAppend("],");
								}
								else
									json.Append ( Tools.JSONPair("Url",m3.URL) );
								JSONAppend("},");
							}
							JSONAppend("],");
						}
						else
							json.Append ( Tools.JSONPair("Url",m2.URL) );
						JSONAppend("},");
					}
					JSONAppend("],");
				}
				else
					json.Append ( Tools.JSONPair("Url",m1.URL) );
				JSONAppend("},");
			}
			JSONAppend("],");

			return 0;
		}

		private int Dashboard()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			SetError(11505,"Not implemented yet");
			return 0;
		}

		private int GetPageInfoCreateNeweWallet()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("CurrencyListText","Select Currency")
			            + Tools.JSONPair("FundingMethodText","Select Funding Method/Source")
			            + Tools.JSONPair("AccountNumberText","Account Number")
			            + Tools.JSONPair("DescriptionText","Description of eWallet")
			            + Tools.JSONPair("CreateButtonText","CREATE") );
			return 0;
		}

		private int GeteWalletAccountCURList()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			json.Append("\"Currencies\":[");
			json.Append ( Tools.JSONPair("CurrencyCode","ZAR",1,"{")
			            + Tools.JSONPair("CurrencyDescription","SA Rand",1,"","},") );
			json.Append ( Tools.JSONPair("CurrencyCode","USD",1,"{")
			            + Tools.JSONPair("CurrencyDescription","US Dollar",1,"","},") );
			json.Append ( Tools.JSONPair("CurrencyCode","EUR",1,"{")
			            + Tools.JSONPair("CurrencyDescription","Euro",1,"","},") );
			json.Append ( Tools.JSONPair("CurrencyCode","GBP",1,"{")
			            + Tools.JSONPair("CurrencyDescription","Pound Sterling",1,"","},") );
			json.Append ( Tools.JSONPair("CurrencyCode","SIK",1,"{")
			            + Tools.JSONPair("CurrencyDescription","Solomon Islands Kowrie",1,"","}") );
			json.Append("]");

			return 0;
		}

		private int GeteWalletFundingMethodList()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			json.Append("\"FundingMethods\":[");
			json.Append ( Tools.JSONPair("FundingMethodCode","XYZ",1,"{")
			            + Tools.JSONPair("FundingMethodDescription","Cash under my bed",1,"","},") );
			json.Append ( Tools.JSONPair("FundingMethodCode","GHT",1,"{")
			            + Tools.JSONPair("FundingMethodDescription","Inheritance",1,"","},") );
			json.Append ( Tools.JSONPair("FundingMethodCode","DER",1,"{")
			            + Tools.JSONPair("FundingMethodDescription","Robbed a bank",1,"","},") );
			json.Append ( Tools.JSONPair("FundingMethodCode","FAW",1,"{")
			            + Tools.JSONPair("FundingMethodDescription","Got lucky in Sun City",1,"","}") );
			json.Append ( Tools.JSONPair("FundingMethodCode","MNS",1,"{")
			            + Tools.JSONPair("FundingMethodDescription","Sold moonshine during lockdown",1,"","}") );
			json.Append("]");

			return 0;
		}

		private int GetPageInfoEditeWalletAccountDescription()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("SelectListLabelText","Select eWallet")
			            + Tools.JSONPair("DescriptionFieldText","New Description")
			            + Tools.JSONPair("ConfirmButtonText","CONFIRM") );

			return 0;
		}

		private int CreateNeweWalletAccount()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			string cur   = ParmValue("CurrencyCode");
			string fund  = ParmValue("FundingMethodCode");
//			string eCode = ParmValue("eWalletAccountCode");
			string eDesc = ParmValue("eWalletDescription");

			if ( cur.Length < 1 )
				return SetError(11605,"Parameter CurrencyCode is missing");
			if ( fund.Length < 1 )
				return SetError(11610,"Parameter FundingMethodCode is missing");
//			if ( eCode.Length < 1 )
//				return SetError(11615,"Parameter eWalletAccountCode is missing");
			if ( eDesc.Length < 1 )
				return SetError(11620,"Parameter eWalletDescription is missing");

			return 0;
		}

		private int EditeWalletAccountDescription()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			string eCode = ParmValue("eWalletAccountCode");
			string eDesc = ParmValue("eWalletDescription");

			if ( eCode.Length < 1 )
				return SetError(11705,"Parameter eWalletAccountCode is missing");
			if ( eDesc.Length < 1 )
				return SetError(11710,"Parameter eWalletDescription is missing");

			return 0;
		}

		private int SendSMS()
		{
			if ( CheckParameters("App,Mobile") > 0 )
				return errorCode;

			string msgText = ParmValue("Message");

			if ( msgText.Length < 1 )
				return SetError(11715,"Parameter Message is missing");

			using (SMS sms = new SMS())
			{
			//	Clickatell
				sms.ProviderID  = ((int)Constants.MessageProvider.ClickaTell).ToString().PadLeft(3,'0');
				sms.UserID      = "002";
			//	Other, add here
			//	sms.ProviderID  = ((int)Constants.MessageProvider.Blah).ToString().PadLeft(3,'0');
			//	sms.UserID      = "Blah";
				sms.MessageID   = 0;
				sms.MessageBody = msgText;
				if ( mobileNumber.StartsWith("+") )
					sms.PhoneNumber = mobileNumber.Substring(1);
				else if ( Tools.StringToInt(countryCode) > 0 )
					sms.PhoneNumber = countryCode + mobileNumber;
				else
					sms.PhoneNumber = mobileNumber;
				if ( sms.LoadProvider() == 0 )
					errorCode = sms.Send();
				else if ( ! Tools.SystemIsLive() )
					errorCode = sms.Send((byte)Constants.TransactionType.Test);
				else
					return SetError(11720,"Failed to load SMS provider details");
			}
			return errorCode;
		}

		private void JSONAppend(string term)
		{
			while ( json.ToString().EndsWith(" ") || json.ToString().EndsWith(",") )
				json.Remove(json.Length-1,1);
			json.Append(term);
		}

		private int GetTestData()
		{
			json.Append ( Tools.JSONPair("ClientCode","10927483")
		               + Tools.JSONPair("ClientName","John D Klutz",1,"")
		               + Tools.JSONPair("NumberOfAccounts","3",11,"")
		               + "\"Accounts\":["
		               + Tools.JSONPair("AccountNumber","12345678",1,"{")
		               + Tools.JSONPair("AccountType","Current")
		               + Tools.JSONPair("AccountBalance","2093.76",11,"","},")
		               + Tools.JSONPair("AccountNumber","98765432",1,"{")
		               + Tools.JSONPair("AccountType","Savings")
		               + Tools.JSONPair("AccountBalance","143.76",11,"","},")
		               + Tools.JSONPair("AccountNumber","11223344",1,"{")
		               + Tools.JSONPair("AccountType","Investment")
		               + Tools.JSONPair("AccountBalance","112093.76",11,"","}") + "]" );
			return 0;
		}

		private int SendJSON(int errCode=0,string errMessage="")
		{
			if ( errCode < 1 )
				errCode    = errorCode;
			if ( errMessage.Length < 1 )
				errMessage = errorMsg;

			string data = Tools.JSONPair("QueryResultCode",( errCode > 0 ? errCode.ToString() : ((int)ResultCode.OK).ToString() ),11)
			            + Tools.JSONPair("QueryResultMessage",errMessage,1);
			if ( errCode == 0 && json != null && json.Length > 0 )
				data = data + json.ToString();

			data = data.Trim().Replace(",]","]").Replace("[,","[");
			if ( data.EndsWith(",") )
				data = data.Substring(0,data.Length-1);
			data = "{" + data + "}";

			try
			{
				Tools.LogInfo("UIApplicationQuery.SendJSON/1",data,220);

				Response.Clear();
				Response.ContentType = "application/json; charset=utf-8";
//				Response.Write(data.Replace("'","\""));
				Response.Write(data);
				Response.Flush();
				Response.End();
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("SendJSON/99","",ex,this);
			}

			return errCode;
		}

		private string ParmValue(string parmName)
		{
			try
			{
				if ( inputDataType == (byte)Constants.WebDataType.JSON ) // && ! String.IsNullOrEmpty(inputDataJSON) )
					return Tools.JSONValue(inputDataJSON,parmName);

				if ( inputDataType == (byte)Constants.WebDataType.XML ) // && inputDataXML != null )
					return Tools.XMLNode(inputDataXML,parmName);

				if ( inputDataType == (byte)Constants.WebDataType.FormPost )
					return WebTools.RequestValueString(Request,parmName,(byte)Constants.HttpMethod.Post);

				if ( inputDataType == (byte)Constants.WebDataType.FormGetOrPost ||
				     Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development )
					return WebTools.RequestValueString(Request,parmName);
			}
			catch (Exception ex)
			{
				Tools.LogException("ParmValue","parmName="+parmName+", inputDataType="+inputDataType.ToString(),ex,this);
			}
			return "";
		}

		public override void CleanUp()
		{
			json         = null;
			inputDataXML = null;
		}
	}
}
