using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text;
using System.Web.UI;

namespace PCIBusiness
{
	public static class Tools
	{
//		Date formats:
//		 1	 dd/mm/yyyy                (31/12/2009)
//		 2	 yyyy/mm/dd                (2006/04/22)
//		 3	 DD 3-char Month Abbr YYYY (22 Sep 2008)
//		 4	 DD full Month Name YYYY   (19 August 2003)
//		 5	 yyyymmdd
//		 6  DayName DD MonthName YYYY (Saturday 13 October 2010)
//		 7  YYYY-MM-DD
//		 8	 Day DD 3-char Month Abbr YYYY (Fri 22 Sep 2008)
//		19	 yyyy/mm/dd (for SQL)

//		Time formats:
//		 1  HH:mm:ss                  (17:03:54)
//		 2  HHmmss                    (170354)
//		 3  HH:mm                     (17:03)
//		 4  at HH:mm                  (at 17:03)
//		11  HH:mm:ss                  Hard-code to 00:00:00
//		12  HH:mm:ss                  Hard-code to 23:59:59

		public static bool SystemIsLive()
		{
			string mode = ConfigValue("SystemMode").ToUpper();
			return ( mode.Length >= 4 && ( mode.Contains("PRODUCTION") || mode.Contains("LIVE") ) );
		}

		public static string DecimalToString(decimal theValue,byte decimalPlaces=2)
		{
			return System.Math.Round(theValue,decimalPlaces).ToString();
		}


		public static string IntToDecimal(int theValue,byte theFormat)
		{
			string tmp = theValue.ToString().Trim();

			if ( theFormat == 1 )
			return tmp + ".00";
			else if ( theFormat == 2 && tmp.Length == 0 )
				return "0.00";
			else if ( theFormat == 2 && tmp.Length == 1 )
				return "0.0" + tmp;
			else if ( theFormat == 2 && tmp.Length == 2 )
				return "0." + tmp;
			else if ( theFormat == 2 )
				return tmp.Substring(0,tmp.Length-2) + "." + tmp.Substring(tmp.Length-2);
		
			return tmp;
		}

		public static string ObjectToString(Object theValue)
		{
			if ( theValue == null ) return "";
			return theValue.ToString().Trim().Replace("<","").Replace(">","");
		}

		public static string NullToString(string theValue)
		{
			if ( string.IsNullOrWhiteSpace(theValue) ) return "";
			return theValue.Trim();
		}

		public static string CompressedString(string theValue)
		{
			if ( string.IsNullOrWhiteSpace(theValue) ) return "";
			theValue = theValue.Trim();
			while ( theValue.Contains("  ") )
				theValue = theValue.Replace("  "," ");
			return theValue;
		}

		public static int StringToInt(string theValue,bool allowDecimals=false)
		{
			try
			{
				theValue = NullToString(theValue);
				if ( allowDecimals && theValue.Contains(".") )
					theValue = theValue.Substring(0,theValue.IndexOf("."));
				int ret = System.Convert.ToInt32(theValue);
				return ret;
			}
			catch
			{ }
			return 0;
		}

		public static DateTime StringToDate(string dd,string mm,string yy)
		{
			DateTime ret = Constants.C_NULLDATE();
			try
			{
				ret = new DateTime(System.Convert.ToInt32(yy), System.Convert.ToInt32(mm), System.Convert.ToInt32(dd));
			}
			catch
			{
				ret = Constants.C_NULLDATE();
			}
			return ret;
		}

		public static DateTime StringToDate(string theDate,byte dateFormat)
		{
			DateTime ret = Constants.C_NULLDATE();
			string   dd  = "";
			string   mm  = "";
			string   yy  = "";
			string   hh  = "";
			string   mi  = "";
			string   ss  = "";

			theDate = theDate.Trim();

			if ( dateFormat == 1 && theDate.Length == 10 )
			{
				dd = theDate.Substring(0,2);	
				mm = theDate.Substring(3,2);	
				yy = theDate.Substring(6,4);	
			}
			else if ( dateFormat == 2 && theDate.Length == 10 )
			{
				dd = theDate.Substring(8,2);	
				mm = theDate.Substring(5,2);	
				yy = theDate.Substring(0,4);	
			}
			else if ( dateFormat == 13 && ( theDate.Length == 16 || theDate.Length == 19 ) )
			{
				dd = theDate.Substring(0,2);	
				mm = theDate.Substring(3,2);	
				yy = theDate.Substring(6,4);	
				hh = theDate.Substring(11,2);	
				mi = theDate.Substring(14,2);
				if ( theDate.Length == 19 )
					ss = theDate.Substring(17,2);
			}

			try
			{
				if ( ss.Length == 2 )
					ret = new DateTime(System.Convert.ToInt32(yy), System.Convert.ToInt32(mm), System.Convert.ToInt32(dd), System.Convert.ToInt32(hh), System.Convert.ToInt32(mi), System.Convert.ToInt32(ss));
				else if ( hh.Length == 2 )
					ret = new DateTime(System.Convert.ToInt32(yy), System.Convert.ToInt32(mm), System.Convert.ToInt32(dd), System.Convert.ToInt32(hh), System.Convert.ToInt32(mi), 0);
				else
					ret = new DateTime(System.Convert.ToInt32(yy), System.Convert.ToInt32(mm), System.Convert.ToInt32(dd));
			}
			catch
			{
				ret = Constants.C_NULLDATE();
			}
			return ret;
		}

		public static string DateToSQL(DateTime whatDate,byte timeFormat,bool quotes=true)
		{
			return DateToString(whatDate,19,timeFormat,quotes);
		}

		public static string DateToString(DateTime whatDate,byte dateFormat,byte timeFormat=0,bool quotes=false)
		{
			string theDate = "" ;
			string theTime = "" ;

			if ( whatDate.CompareTo(Constants.C_NULLDATE()) <= 0 && dateFormat == 19 ) // for SQL
				return "NULL";

			if ( whatDate.CompareTo(Constants.C_NULLDATE()) <= 0 )
				return "";

			if ( dateFormat == 1 )        // DD/MM/YYYY
				theDate = whatDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
			else if  ( dateFormat ==  2 ) // YYYY/MM/DD
				theDate = whatDate.ToString("yyyy/MM/dd",CultureInfo.InvariantCulture);
			else if  ( dateFormat ==  3 ) // DD MonthAbbr YYYY
				theDate = whatDate.ToString("dd MMM yyyy");
			else if  ( dateFormat ==  4 ) // DD MonthName YYYY
				theDate = whatDate.ToString("dd MMMMMMMMM yyyy");
			else if  ( dateFormat ==  5 ) // YYYYMMDD
				theDate = whatDate.ToString("yyyyMMdd");
			else if  ( dateFormat ==  6 ) // Saturday 13 October 2010
				theDate = whatDate.ToString("ddddddddd dd MMMMMMMMM yyyy");
			else if  ( dateFormat ==  7 ) // 2010-07-25
				theDate = whatDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture);
			else if  ( dateFormat ==  8 ) // Sat 13 Oct 2010
				theDate = whatDate.ToString("ddd dd MMM yyyy");
			else if  ( dateFormat ==  9 ) // yyMMdd
				theDate = whatDate.ToString("yyMMdd",CultureInfo.InvariantCulture);
			else if  ( dateFormat == 19 ) // YYYY/MM/DD (for SQL)
				theDate = whatDate.ToString("yyyy/MM/dd",CultureInfo.InvariantCulture);

			if ( timeFormat == 1 && ( dateFormat == 6 || dateFormat == 8 ) )
				theTime = "at " + whatDate.ToString("HH:mm:ss",CultureInfo.InvariantCulture);
			else if ( timeFormat == 1 )  // HH:MM:SS
				theTime = whatDate.ToString("HH:mm:ss",CultureInfo.InvariantCulture);
			else if ( timeFormat == 2 )  // HHMMSS
				theTime = whatDate.ToString("HHmmss");
			else if ( timeFormat == 3 )  // HH:MM
				theTime = whatDate.ToString("HH:mm",CultureInfo.InvariantCulture);
			else if ( timeFormat == 4 )  // at HH:MM
				theTime = "at " + whatDate.ToString("HH:mm",CultureInfo.InvariantCulture);
			else if ( timeFormat == 5 )  // HH:MM:SS.999
				theTime = whatDate.ToString("HH:mm:ss.fff",CultureInfo.InvariantCulture);
			else if ( timeFormat == 11 )  // 00:00:00
				theTime = "00:00:00";
			else if ( timeFormat == 12 )  // 23:59:59
				theTime = "23:59:59";

			return ( quotes ? "'" : "" ) + (theDate + " " + theTime).Trim() + ( quotes ? "'" : "" );
		}

		public static bool OpenDB(ref DBConn dbConn)
		{
			if ( dbConn == null )
				dbConn = new DBConn();
			return dbConn.Open();
		}

		public static void CloseDB(ref DBConn dbConn)
		{
         if ( dbConn != null )
         {
            try
            {
               dbConn.Close();
               dbConn.Dispose();
            }
            catch { }
         }
         dbConn = null;
		}

		public static string MixedCase(string str)
		{
			int j;
			int k;
			if ( str == null )
				return "";

			try
			{
				str = str.Trim().ToLower();
				if ( str.Length == 0 )
					return "";
				while ( str.IndexOf("  ") >= 0 )
					str = str.Replace("  "," ");
				str = str.Substring(0,1).ToUpper() + str.Substring(1);
				j   = 0;
				k   = str.IndexOf(" ",0);
				while ( k > j )
				{
					str = str.Substring(0,k+1) + str.Substring(k+1,1).ToUpper() + str.Substring(k+2);
					j   = k + 1;
					k   = str.IndexOf(" ",j);
				}
			}
			catch (Exception ex)
			{
				LogException("Tools.MixedCase","str=" + str,ex);
			}
			return str;
		}

//		public static string JSONPair(string name,string value,byte dataType=1,bool firstPair=false,bool lastPair=false)
//		{
//			string quote = "\"";
//			return ( firstPair ? "{" : "" )
//			     + quote + name.Trim().Replace(quote,"'") + quote + " : "
//			     + ( dataType<10 ? quote : "" ) + value.Trim().Replace(quote,"'") + ( dataType<10 ? quote : "" )
//			     + ( lastPair  ? "}" : "," );
//		}

		public static string JSONPair(string name,string value,byte dataType=1,string prefix="",string suffix=",")
		{
		//	dataType =  1 means STRING
		//          = 11 means NUMERIC
		//          = 12 means BOOLEAN
			string quote = "\"";
			return prefix
			     + quote + name.Trim().Replace(quote,"'") + quote + " : "
			     + ( dataType<10 ? quote : "" ) + value.Trim().Replace(quote,"'") + ( dataType<10 ? quote : "" )
			     + suffix;
		}

//		public static string JSONPair(string name,int value,bool firstPair=false,bool lastPair=false)
//		{
//			string quote = "\"";
//			return (firstPair?"{":"")
//			     + quote + name.Trim().Replace(quote,"'") + quote + " : " + value.ToString()
//			     + (lastPair ?"}":",");
//		}

		public static string JSONValue(string data,string tag)
		{
		//	Handle data in the format
		//	{"key1":"value","key2":"value","key3":"value"}
		//	Spaces on either side of {",:} are handled

			try
			{
				int    j;
				int    k     = 0;
				int    h     = 1;
				string value = "";
				tag          = "\"" + tag.ToUpper() + "\"";

//	Find the tag
				while ( value.Length == 0 )
				{
					k = data.ToUpper().IndexOf(tag,k);
					if ( k < 0 )
						return "";
					for ( j = k+tag.Length ; j < data.Length ; j++ )
						if ( data.Substring(j,1) == " " )
							continue;
						else
						{
							if ( data.Substring(j,1) == ":" )
								value = data.Substring(j+1).Trim();
							else
								k = j;
							break;
						}
				}

				if ( value.Substring(0,1) == "\"" ) // Value starts with " and will end with "
					k = value.IndexOf("\"",1);
				else                                // Value may end with , { }
				{
					h = 0;
					k = value.IndexOf(",");
					if ( k < 0 )
						k = value.IndexOf("{");
					if ( k < 0 )
						k = value.IndexOf("}");
				}
				if ( k <= h )
					return "";
				
				return value.Substring(h,k-h).Trim();

//	Version 2
//				k = data.IndexOf("\"",h);
//				if ( k < 0 )
//					k = data.IndexOf(",",h);
//				if ( k < 0 )
//					k = data.IndexOf("{",h);
//				if ( k < 0 )
//					k = data.IndexOf("}",h);
//				if ( k < 0 )
//					return "";
//				j = data.IndexOf("\"",k+1);
//				if ( j <= k )
//					return "";
//				return data.Substring(k+1,j-k-1).Trim();

//	Version 1
//				j = data.IndexOf(":",k+tag.Length);
//					
//				if ( k < 0 )
//					return "";
//				k = data.IndexOf("\"",k+tag.Length);
//				if ( k < 0 )
//					return "";
//				 j = data.IndexOf("\"",k+1);
//				if ( j <= k )
//					return "";
//				return data.Substring(k+1,j-k-1);
			}
			catch
			{ }
			return "";
		}

		public static string XMLNode(XmlDocument xmlDoc,string xmlTag,string nsPrefix="",string nsURL="")
		{
			try
			{
				string ret = "";
				if ( nsPrefix.Length == 0 || nsURL.Length == 0 )
				{
					try
					{	
						ret = xmlDoc.SelectSingleNode("//"+xmlTag).InnerText.Trim();
					}
					catch { }
					if ( ret == null || ret.Length == 0 )
						ret = xmlDoc.GetElementsByTagName(xmlTag).Item(0).InnerText.Trim();
				}
				else
				{
					XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
					nsMgr.AddNamespace(nsPrefix,nsURL);
					ret = xmlDoc.SelectSingleNode("//"+nsPrefix+":"+xmlTag,nsMgr).InnerText;
				}
				return ret.Trim();
			}
			catch
			{ }
			return "";
		}

		public static string XMLSafe(string str,byte encoding=0)
		{
      // Converts a string to safe format for XML:
      // (1) Removes leading and trailing spaces
      // (2) Replaces illegal chars, which are & < > " '
		//	Note that both \ and / are allowed in XML
    
			if ( string.IsNullOrWhiteSpace(str) )
				return "";

			str = str.Trim();
         str = str.Replace("'","`");
         str = str.Replace("\"","`");
         str = str.Replace("<","[");
         str = str.Replace(">","]");
         str = str.Replace("&"," and ");
         str = str.Replace("  and "," and ");
         str = str.Replace(" and  "," and ");

//	UniCode testing
/*
//	Ver 1
			if ( encoding == 29 ) // Unicode
			{
				byte[] unicodeBytes = Encoding.UTF8.GetBytes(str);
				string ret          = Encoding.UTF8.GetString(unicodeBytes,0,unicodeBytes.Length);
				LogInfo("Tools.XMLSafe/1","Str (in)='"+str+"', Str (out)='"+ret+"'",255);
//				return ret;
			}

//	Ver 2
			if ( encoding == 29 ) // Unicode
			{
				byte[] unicodeBytes = Encoding.UTF8.GetBytes(str);
				string ret          = "";
				for ( int k = 0 ; k < unicodeBytes.Length ; k++ )
					ret = ret + "/" + unicodeBytes[k];
				LogInfo("Tools.XMLSafe/2","Str (in)='"+str+"', Str (out)='"+ret.Substring(1)+"'",255);
//				return ret.Substring(1);
			}

//	Ver 3 & 4
			if ( encoding == 29 ) // Unicode
			{
				byte[] unicodeBytes = Encoding.UTF8.GetBytes(str);
				string ret10        = "";
				string ret16        = "";
				for ( int k = 0 ; k < unicodeBytes.Length ; k++ )
				{
					ret10 = ret10 + "&#"  + unicodeBytes[k] + ";";
					ret16 = ret16 + "&#x" + unicodeBytes[k] + ";";
				}
				LogInfo("Tools.XMLSafe/3","Str (in)='"+str+"', Str (out)='"+ret10+"'",255);
				LogInfo("Tools.XMLSafe/4","Str (in)='"+str+"', Str (out)='"+ret16+"'",255);
//				return ret10;
			}

//	Ver 5 & 6
			if ( encoding == 29 ) // Unicode
			{
				byte[] unicodeBytes5 = Encoding.UTF8.GetBytes(str);
				byte[] unicodeBytes6 = new UTF8Encoding().GetBytes(str);
				string ret5          = "";
				string ret6          = "";
				for ( int k = 0 ; k < unicodeBytes5.Length ; k++ )
					ret5 = ret5 + "/" + unicodeBytes5[k].ToString("x2");
				for ( int k = 0 ; k < unicodeBytes6.Length ; k++ )
					ret6 = ret6 + "&#x" + unicodeBytes6[k].ToString("x2") + ";";
				LogInfo("Tools.XMLSafe/5","Str (in)='"+str+"', Str (out)='"+ret5.Substring(1)+"'",255);
				LogInfo("Tools.XMLSafe/6","Str (in)='"+str+"', Str (out)='"+ret6.Substring(1)+"'",255);
//				return ret6.Substring(1);
			}

//	Ver 7
			if ( encoding == 29 ) // Unicode
			{
				byte[] utf8Bytes = new byte[str.Length];
				for ( int k = 0 ; k < str.Length ; k++ )
					utf8Bytes[k] = (byte)str[k];
				LogInfo("Tools.XMLSafe/7","Str (in)='"+str+"', Str (out)='"+Encoding.UTF8.GetString(utf8Bytes,0,utf8Bytes.Length)+"'",255);
//				return Encoding.UTF8.GetString(utf8Bytes,0,utf8Bytes.Length);
			}

//	Ver 8
			if ( encoding == 29 ) // Unicode
			{
				byte[] unicodeBytes = Encoding.UTF8.GetBytes(str);
				string ret          = Encoding.Unicode.GetString(unicodeBytes);
				LogInfo("Tools.XMLSafe/8","Str (in)='"+str+"', Str (out)='"+ret+"'",255);
//				return ret;
			}

//	Ver 9
			if ( encoding == 29 ) // Unicode
			{
				byte[] utfBytes = Encoding.UTF8.GetBytes(str);
				string ret9     = "";
				for ( int k = 0 ; k < str.Length ; k++ )
					for ( int h = 0 ; h < 3 ; h++ )
						if ( h == 0 )
							ret9 = ret9 + "\\u";
						else
							ret9 = ret9 + utfBytes[(k*3)+h].ToString("x2");
				LogInfo("Tools.XMLSafe/9","Str (in)='"+str+"', Str (out)='"+ret9+"'",255);
//				return ret9;
			}
*/
         return str;
		}

		public static string XMLString(string str)
		{
      // Converts an XML string for use in an SQL statement:
      // (1) Removes leading and trailing spaces
      // (2) Replaces each single quote with a \"
      // (3) Puts a single quote at front and back
    
			if ( string.IsNullOrWhiteSpace(str) )
				return "''";
			str = str.Trim();
         str = "'" + str.Replace("'","\"") + "'";
         return str;
		}

		public static string URLString(string str)
		{
			if ( string.IsNullOrWhiteSpace(str) )
				return "";
//			return System.Net.WebUtility.HtmlEncode(str.Trim());
			return str.Trim();
		}

		public static string DBString(string str,byte mode=0,int maxLength=0)
		{
      // Converts a string for use in an SQL statement:
      // (1) Removes leading and trailing spaces
      // (2) Replaces each single quote with TWO single quotes
      // (3) Removes "<" and ">" characters (no embedded HTML)
      // (4) Puts a single quote at front and back
    
			if ( string.IsNullOrWhiteSpace(str) )
				return "''";

			str = str.Trim();
			if ( str.ToUpper() == "NULL" ) // Leave 'NULL' unchanged, no quotes
				return "NULL";
			if ( mode == 0 )
				str = str.Replace("<","").Replace(">","");
			if ( maxLength > 0 && maxLength < str.Length )
				str = str.Substring(0,maxLength);

         str = "'" + str.Replace("'","''") + "'";
         return str;
		}

		public static int TimeDifference(string time1,string time2)
		{
			try
			{
				DateTime d1 = System.Convert.ToDateTime("1999/12/31 "+time1);
				DateTime d2 = System.Convert.ToDateTime("1999/12/31 "+time2);
				TimeSpan mn = d2 - d1;
				return (int) mn.TotalMinutes;
			}
			catch { }
			return 0;
		}

		public static int DateDifference(DateTime dt1,DateTime dt2)
		{
			try
			{
				TimeSpan mn = dt2 - dt1;
				return (int) mn.Days;
			}
			catch { }
			return 0;
		}

		public static string TimeAdd(string theTime,int minutes)
		{
			try
			{
				DateTime dt = System.Convert.ToDateTime("1999/12/31 " + theTime);
				TimeSpan mn = new TimeSpan(0,minutes,0);
				dt = dt.Add(mn);
				return Tools.DateToString(dt,0,3,false);
			}
			catch { }
			return ""; 
		}

		private static void LogWrite(string settingName, string component, string msg)
		{
		// Use this routine to log internal errors ...
			int          k;
			string       fName   = "";
			FileStream   fHandle = null;
			StreamWriter fOut    = null;

			try
			{
				fName = Tools.ConfigValue(settingName);
				if ( fName == null )
					fName = "";
				else
					fName = fName.Trim();
				if ( fName.Length < 1 )
					fName = "C:\\Temp\\PCILogFile.txt";

				k = fName.LastIndexOf(".");
				if ( k < 1 )
				{
					fName = fName + ".txt";
					k     = fName.LastIndexOf(".");
				}
				fName = fName.Substring(0,k) + "-" + DateToString(System.DateTime.Now,7) + fName.Substring(k);

				if ( File.Exists(fName) )
					fHandle = File.Open(fName, FileMode.Append);
				else
					fHandle = File.Open(fName, FileMode.Create);
				fOut = new StreamWriter(fHandle,System.Text.Encoding.Default);
				fOut.WriteLine( "[v" + SystemDetails.AppVersion + ", " + Tools.DateToString(System.DateTime.Now,1,1,false) + "] " + component + " : " + msg);
			}
			catch (Exception ex)
			{
				if ( settingName.Length > 0 ) // To prevent recursion ...
					LogWrite("","Tools.LogWrite","fName = '" + fName + "', Error = " + ex.Message);
			}
			finally
			{
				if ( fOut != null )
					fOut.Close();
				fOut    = null;
			 	fHandle = null;
			}
		}

		public static void LogException(string component, string msg, Exception ex=null)
		{
		// Use this routine to log error messages
			msg = ( msg.Length == 0 ? "" : " (" + msg + ")" );
			if ( ex == null )
				msg = "Non-exception error" + msg;
			else if ( ex.GetType() == typeof(System.IndexOutOfRangeException) )
				msg = "SQL column not found" + msg;
			else
				msg = ex.Message + msg + " : [" + ex.ToString() + "]";
			LogWrite("LogFileErrors",component,msg);
		}

		public static void LogInfo(string component, string msg, byte severity=10)
		{
		// Use this routine to log debugging/info messages
		//	To decide which messages to write, adjust the severity below
		//	Calling routines must supply a severity between 0-255 (default 10)
		//	If severity == 255 then do NOT log for LIVE

			if ( severity == 255 )
				if ( LiveTestOrDev() == Constants.SystemMode.Live )
					return;
			if ( severity > 100 )
				LogWrite("LogFileInfo",component,msg);
		}

		public static bool CheckEMail(string email)
		{
		//	Simple, quick check ...
			email = NullToString(email);
			if ( email.Length < 6 || email.Contains("(") || email.Contains(")") || email.Contains("<") || email.Contains(">") || email.Contains(" ") )
				return false;
			int at  = email.IndexOf("@");
			int dot = email.LastIndexOf(".");
			return ( at > 0 && dot > at );
		}

		/*
		public static byte CheckCreditCardNumber(ref string ccNumber,byte cardType)
		{
		//	VISA numbers             start with 4
		//	MasterCard numbers       start with 2 or 5
		// American Express numbers start with 34 or 37
		// Diner's Club numbers     start with various

			ccNumber = ccNumber.Trim().Replace(" ","");

			if ( cardType == (byte)Constants.CreditCardType.Visa )
			{
				if ( ccNumber.Length < 13 || ccNumber.Length > 19 )
					return 3;
				if ( ccNumber.Substring(0,1) != "4" )
					return 6;
			}

			else if ( cardType == (byte)Constants.CreditCardType.MasterCard )
			{
				if ( ccNumber.Length != 16 )
					return 33;
				if ( ccNumber.Substring(0,1) != "2" && ccNumber.Substring(0,1) != "5" )
					return 36;
			}

			else if ( cardType == (byte)Constants.CreditCardType.AmericanExpress )
			{
				if ( ccNumber.Length != 15 )
					return 63;
				if ( ccNumber.Substring(0,2) != "34" && ccNumber.Substring(0,2) != "37" )
					return 66;
			}

			else if ( cardType == (byte)Constants.CreditCardType.DinersClub )
			{
				if ( ccNumber.Length < 14 || ccNumber.Length > 16 )
					return 93;
			}

			else if ( ccNumber.Length < 12 || ccNumber.Length > 20 )
				return 23;

			try
			{
				ulong ccNo = System.Convert.ToUInt64(ccNumber);
			}
			catch
			{
				return 99;
			}

			return 0;
		}
		*/

		public static DateTime IDNumberToDate(string idNumber)
		{
			try
			{
				string year    = System.DateTime.Now.Year.ToString();
				int    century = System.Convert.ToInt32(year.Substring(0,2));
	
				if ( idNumber.Substring(0,2).CompareTo(year.Substring(2,2)) > 0 ) 
					century = century - 1;

				return Tools.StringToDate(idNumber.Substring(4,2),idNumber.Substring(2,2),century.ToString()+idNumber.Substring(0,2));
			}
			catch
			{ }
			return Constants.C_NULLDATE();
		}

		public static byte CheckDate(string dd,string mm,string yy,ref DateTime theDate)
		{
			try
			{
				if ( dd.Length == 0 && mm.Length == 0 && yy.Length == 0 )
					return 244;
				theDate = new DateTime(Convert.ToInt32(yy),Convert.ToInt32(mm),Convert.ToInt32(dd));
				return 0;
			}
			catch
			{ }
			return 255;
		}

		public static int CalcAge(DateTime dateOfBirth,DateTime theDate)
		{
			if ( theDate == null || theDate <= Constants.C_NULLDATE() )
				theDate = System.DateTime.Now;

			if ( dateOfBirth <= Constants.C_NULLDATE() || dateOfBirth >= theDate )
				return 0;

			int diff = theDate.Year - dateOfBirth.Year;
			 
			if ( dateOfBirth.Month < theDate.Month )
				return diff;
			if ( dateOfBirth.Month > theDate.Month )
				return diff - 1;
			if ( dateOfBirth.Day > theDate.Day )
				return diff - 1;
			return diff;
		}

		public static string ConfigValue(string configName)
		{
			try
			{
				string ret = System.Configuration.ConfigurationManager.AppSettings[configName.Trim()].ToString();
				return ret;
			}
			catch
			{ }
			return "";
		}

		public static int DeleteFiles(string fileSpec,short ageDays=0,short beforeHour=0,short afterHour=0)
		{
			int deleted = 0;

			try
			{
				if ( beforeHour > 0 && beforeHour < 24 && System.DateTime.Now.Hour >= beforeHour )
					return -5;
				if ( afterHour  > 0 && afterHour  < 24 && System.DateTime.Now.Hour <  afterHour )
					return -10;

				string folder = Tools.ConfigValue("TempFiles");

				if ( ! Directory.Exists(folder) )
					return -15;
				string[] files = Directory.GetFiles(folder,fileSpec);
				if ( files.Length < 1 )
					return -20;

				if ( ageDays < 1 )
					ageDays = 7;

				foreach ( string fileName in files )
					if ( File.GetLastWriteTime(fileName).AddDays(ageDays) < DateTime.Now ) // More "x" days old
					{
						try
						{
							File.Delete(fileName);
							deleted++;
						}
						catch { }
					}
			}
			catch (Exception ex)
			{
				Tools.LogException("Tools.DeleteFiles","",ex);
			}
			return deleted;
		}

		public static string CreateFile(ref StreamWriter fileStream,string fileName,string fileExtension="csv")
		{
			FileStream fileHandle;
			string     fileNameFixed = "";

			try
			{
				fileName      = NullToString(fileName);
				fileExtension = NullToString(fileExtension);

				if ( fileExtension.Length < 1 )
					fileExtension = ".csv";
				else if ( ! fileExtension.StartsWith(".") )
					fileExtension = "." + fileExtension;

				if ( fileName.Length > 0 )
					fileName = fileName.Replace("\\","-").Replace("/","-").Replace(":","-");
				else
					fileName = DateToString(DateTime.Now,5);

				fileStream    = null;
				fileNameFixed = Tools.FixFolderName(ConfigValue("TempFiles")) + fileName + "-";

				for ( int k = 1 ; k < 999999 ; k++ )
				{
					fileName = fileNameFixed + k.ToString().PadLeft(3,'0') + fileExtension;
					if ( ! File.Exists(fileName) )
						break;
				}
				fileHandle = File.Open(fileName, FileMode.Create);
				fileStream = new StreamWriter(fileHandle,System.Text.Encoding.Default);
				return fileName;
			}
			catch (Exception ex)
			{
				LogException("Tools.CreateFile","fileName=" + fileName+", fileExtension=" + fileExtension,ex);
			}
			return "";
		}

		public static string FixFolderName(string folder)
		{
			folder = NullToString(folder);
			if ( folder.Length < 1 )
				return "";
			return ( folder.EndsWith("\\") ? folder : folder + "\\" );
		}

		public static string SystemFolder(string subFolder)
		{
			string folder = ConfigValue("SystemPath");
			subFolder     = NullToString(subFolder);
			if ( folder.Length > 0 && subFolder.Length > 0 )
				return folder + ( folder.EndsWith("\\") ? "" : "\\" ) + subFolder + ( subFolder.EndsWith("\\") ? "" : "\\" );
			if ( folder.Length > 0 )
				return folder + ( folder.EndsWith("\\") ? "" : "\\" );
			if ( subFolder.Length > 0 )
				return subFolder + ( subFolder.EndsWith("\\") ? "" : "\\" );
			return "";
		}

		public static string ConciseName(string theName)
		{
			string ret;
			string ch;
			int    k;

			if ( theName == null )
				return "";

			theName = theName.Trim().Replace(" ","").ToUpper();
			ret     = "";

			for ( k = 0 ; k < theName.Length ; k++ )
			{
				ch = theName.Substring(k,1);
				if ( ch.CompareTo("A") >= 0 && ch.CompareTo("Z") <= 0 )
					ret = ret + ch;
				else if ( ch.CompareTo("0") >= 0 && ch.CompareTo("9") <= 0 )
					ret = ret + ch;
			}
			return ret;
		}

//	Generic "Valid" stuff

		public static string ReverseString(string str)
		{
			if ( string.IsNullOrWhiteSpace(str) )
				return "";
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new String(array);
		}

		public static string SplitString(string str,short lineLength=100)
		{
			int    k;
			string ret = "";
			str        = NullToString(str).Replace("  "," ");
			if ( lineLength < 10 )
				lineLength = 100;

			while ( str.Length > lineLength )
			{
				k   = (str.Substring(lineLength)).IndexOf(" ");
				if ( k < 0 )
					break;
				k   = k + lineLength;
				ret = ret + str.Substring(0,k) + Constants.C_HTMLBREAK();
				str = str.Substring(k+1).Trim();
			}
			return ret + str;
		}

		public static Constants.SystemMode LiveTestOrDev()
		{
			string mode = Tools.ConfigValue("SystemMode").ToUpper();
			if ( mode.StartsWith("LIVE") || mode.StartsWith("PROD") )
				return Constants.SystemMode.Live;
			if ( mode.StartsWith("TEST") )
				return Constants.SystemMode.Test;
			return Constants.SystemMode.Development;
		}

		public static string BureauCode(Constants.PaymentProvider providerCode)
		{
			return ((short)providerCode).ToString().PadLeft(3,'0');
		}

		public static string SQLDebug(string sql)
		{
			sql = Tools.NullToString(sql);
			if ( sql.Length < 3 )
				return "Invalid SQL" + ( sql.Length > 0 ? " (" + sql + ")" : "" );

			DBConn        conn = null;
			StringBuilder ret  = new StringBuilder();
			string        str  = "SQL = " + sql;

			try
			{
				Tools.LogInfo("Tools.SQLDebug/2",str,255);
				ret.Append(str+Constants.C_HTMLBREAK());

				Tools.OpenDB(ref conn);

				if ( conn.Execute(sql) )
				{
					str = "Execution successful, column count = " + conn.ColumnCount.ToString() + ( conn.EOF ? " (NO rows)" : " (At least one row)" );
					Tools.LogInfo("Tools.SQLDebug/3",str,255);
					ret.Append(str+"<hr />"); // Constants.C_HTMLBREAK());

					string colType;

					for ( int k = 0 ; k < conn.ColumnCount ; k++ )
					{
						str = "(Col " + k.ToString()
						    + ") Name = " + conn.ColName(k)
						    + ", Type = " + conn.ColDataType("",k)
						    + ", Value = ";
						colType = conn.ColDataType("",k).ToUpper();
						if ( conn.ColStatus("",k) == Constants.DBColumnStatus.ValueIsNull )
							str = str + "NULL";
						else if ( colType == "NVARCHAR" || colType == "NCHAR" )
							str = str + conn.ColUniCode("",0,k);
						else
							str = str + conn.ColValue(k);
						Tools.LogInfo("Tools.SQLDebug/4",str,255);
						ret.Append(str+Constants.C_HTMLBREAK());
					}
				}
				else
				{
					str = "Execution failed";
					Tools.LogInfo("Tools.SQLDebug/5",str,255);
					ret.Append(str+Constants.C_HTMLBREAK());
				}
			}
			catch (Exception ex)
			{
				str = "Error : " + ex.Message;
				Tools.LogInfo("Tools.SQLDebug/6",str,255);
				ret.Append(str+Constants.C_HTMLBREAK());
			}
			finally
			{
				Tools.CloseDB(ref conn);
			}
			return ret.ToString();
		}


		public static string ReadFile(string fileName)
		{
			StreamReader fHandle = null;

			try
			{
				fHandle  = File.OpenText(fileName);
				string h = fHandle.ReadToEnd();
				return h.Trim();
			}
			catch
			{
				return "";
			}
			finally
			{
				if ( fHandle != null )
					fHandle.Close();
				fHandle = null;
			}
		}

		public static int CreatePDF(string fileSource,string html,ref string fileName)
		{
			try
			{
				StreamWriter fileOut = null;
				fileName = Tools.CreateFile(ref fileOut,fileSource,"pdf");
				if ( fileOut == null )
					return 10;
				fileOut.Close();
				fileOut = null;
				if ( fileName.Length < 1 )
					return 20;

//	NO! Do NOT do this!
//				html = html.Trim().Replace(Environment.NewLine,"<br />");
//	NO!

				SelectPdf.HtmlToPdf   converter = new SelectPdf.HtmlToPdf();
				SelectPdf.PdfDocument doc       = converter.ConvertHtmlString(html);
				doc.Save(fileName);
				doc.Close();
				return 0;
			}
			catch (Exception ex)
			{
				LogException("Tools.CreatePDF","File="+fileName+"\n"+html,ex);
			}
			return 30;
		}
	}
}