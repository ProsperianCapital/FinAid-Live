using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text;
using System.Runtime.Serialization.Json;

namespace PCIBusiness
{
	public static class Tools
	{
//		Date formats:
//		 1	 dd/mm/yyyy                    (31/12/2009)
//		 2	 yyyy/mm/dd                    (2006/04/22)
//		 3	 DD 3-char Month Abbr YYYY     (22 Sep 2008)
//		 4	 DD full Month Name YYYY       (19 August 2003)
//		 5	 yyyymmdd
//		 6  DayName DD MonthName YYYY     (Saturday 13 October 2010)
//		 7  YYYY-MM-DD
//		 8	 Day DD 3-char Month Abbr YYYY (Fri 22 Sep 2008)
//		 9  YYMMDD                        (210131)
//		19	 yyyy/mm/dd (for SQL)

//		Time formats:
//		 1  HH:mm:ss                  (17:03:54)
//		 2  HHmmss                    (170354)
//		 3  HH:mm                     (17:03)
//		 4  at HH:mm                  (at 17:03)
//		11  HH:mm:ss                  Hard-code to 00:00:00
//		12  HH:mm:ss                  Hard-code to 23:59:59
//		23  HH:mm:ss                  Use time if non-zero, else hard-code to 00:00:00
//		24  HH:mm:ss                  Use time if non-zero, else hard-code to 23:59:59

		static byte logSeverity;

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

		public static string DecimalToCurrency(decimal theValue)
		{
			string tmp = theValue.ToString().Trim();
			int    k   = tmp.IndexOf(".");
			if ( theValue == 0 )
				return "0.00";
			if ( k < 0 )               // 3478
				return tmp + ".00";
			if ( k == tmp.Length-1 )   // 3478.
				return tmp + "00";
			if ( k == tmp.Length-2 )   // 3478.1
				return tmp + "0";
			if ( k == tmp.Length-3 )   // 3478.19
				return tmp;
			if ( k <= tmp.Length-4 )   // 3478.1966
				return tmp.Substring(0,k+3);
			return tmp;
		}

		public static string ObjectToString(Object theValue)
		{
			if ( theValue == null ) return "";
			return theValue.ToString().Trim().Replace("<","").Replace(">","");
		}

		public static string NullToString(string theValue,string defaultValue="")
		{
			if ( string.IsNullOrWhiteSpace(theValue) ) return defaultValue;
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

		public static decimal StringToDecimal(string theValue)
		{
			try
			{
				decimal ret = System.Convert.ToDecimal(theValue);
				return  ret;
			}
			catch
			{ }
			return 0;
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
			DateTime ret = Constants.DateNull;
			try
			{
				ret = new DateTime(System.Convert.ToInt32(yy), System.Convert.ToInt32(mm), System.Convert.ToInt32(dd));
			}
			catch
			{
				ret = Constants.DateNull;
			}
			return ret;
		}

		public static DateTime StringToDate(string theDate,byte dateFormat,byte timeFormat=0)
		{
			DateTime ret = Constants.DateNull;
			string   dd  = "";
			string   mm  = "";
			string   yy  = "";
			string   hh  = "";
			string   mi  = "";
			string   ss  = "";
			byte     pos = 0;

			theDate = theDate.Trim();

			if ( dateFormat == 1 && theDate.Length >= 10 ) // dd/mm/yyyy
			{
				dd  = theDate.Substring(0,2);
				mm  = theDate.Substring(3,2);
				yy  = theDate.Substring(6,4);
				pos = 12;
			}
			else if ( dateFormat == 5 && theDate.Length >= 8 ) // yyyymmdd
			{
				dd  = theDate.Substring(6,2);
				mm  = theDate.Substring(4,2);
				yy  = theDate.Substring(0,4);
				pos = 10;
			}
			else if ( ( dateFormat == 2 || dateFormat == 7 ) && theDate.Length >= 10 ) // yyyy/mm/dd
			{
				dd  = theDate.Substring(8,2);
				mm  = theDate.Substring(5,2);
				yy  = theDate.Substring(0,4);
				pos = 12;
			}
//			else if ( dateFormat == 13 && ( theDate.Length == 16 || theDate.Length == 19 ) )
//			{
//				dd = theDate.Substring(0,2);
//				mm = theDate.Substring(3,2);
//				yy = theDate.Substring(6,4);
//				hh = theDate.Substring(11,2);
//				mi = theDate.Substring(14,2);
//				if ( theDate.Length == 19 )
//					ss = theDate.Substring(17,2);
//			}
			else if ( dateFormat == 0 && timeFormat > 0 && theDate.Length > 0 )
			{
				dd = "31";
				mm = "12";
				yy = "1999";
				pos = 1;
			}

			if ( pos > 0 && timeFormat > 0 )
			{
				pos--;
				hh = theDate.Substring(pos,2);
				if ( theDate.Length > pos+3 )
					mi = theDate.Substring(pos+3,2);
				if ( theDate.Length > pos+6 )
					ss = theDate.Substring(pos+6,2);
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
				ret = Constants.DateNull;
			}
			return ret;
		}

		public static string DateToSQL(DateTime whatDate,byte timeFormat,bool quotes=true)
		{
			return DateToString(whatDate,19,timeFormat,quotes);
		}

		public static string DateToString(DateTime whatDate,byte dateFormat,byte timeFormat=0,bool quotes=false)
		{
			if ( whatDate.CompareTo(Constants.DateNull) <= 0 )
				if ( dateFormat == 19 ) // for SQL
					return "NULL";
				else
					return "";

			string theDate = "" ;
			string theTime = "" ;

			if       ( dateFormat ==  1 ) // DD/MM/YYYY
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
				theDate = whatDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture);

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
			else if ( timeFormat == 23 )
				if ( whatDate.Hour > 0 || whatDate.Minute > 0 || whatDate.Second > 0 )
					theTime = whatDate.ToString("HH:mm:ss",CultureInfo.InvariantCulture);
				else
					theTime = "00:00:00";
			else if ( timeFormat == 24 )
				if ( whatDate.Hour > 0 || whatDate.Minute > 0 || whatDate.Second > 0 )
					theTime = whatDate.ToString("HH:mm:ss",CultureInfo.InvariantCulture);
				else
					theTime = "23:59:59";

			return ( quotes ? "'" : "" ) + (theDate + " " + theTime).Trim() + ( quotes ? "'" : "" );
		}

		public static bool OpenDB(ref DBConn dbConn,string connectionName="")
		{
			if ( dbConn == null )
				dbConn = new DBConn();
			return dbConn.Open(connectionName);
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

		public static string JSONSafe(string value,byte mode=0)
		{
			if ( string.IsNullOrWhiteSpace(value) )
				return "";
			if ( mode == 1 ) // Email or Web address
				return value.Trim().Replace("\"","").Replace("'","");
			return value.Trim().Replace("\"","'");
		}

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

		public static string JSONRaw(string data)
		{
			if ( string.IsNullOrWhiteSpace(data) )
				return "";

			data          = data.Trim();
			string[,] fix = new string[,] {{ "26", "&"  },
			                               { "40", "@"  },
			                               { "3F", "?"  },
			                               { "2F", "/"  },
			                               { "5C", "\\" }};
//			Unicode
			if ( data.Contains("\\u00") )
				for ( int k = 0 ; k < fix.GetLength(0) ; k++ )
					data = data.Replace("\\u00"+fix[k,0],fix[k,1]);

//			ASCII
			if ( data.Contains("%") )
				for ( int k = 0 ; k < fix.GetLength(0) ; k++ )
					data = data.Replace("%"+fix[k,0],fix[k,1]);
			
			return data;
		}

		public static string JSONValue(string data,string tag,string tagOuter="",short arrayPosition=0,byte decode=0)
		{
		//	Handle data in the format
		//	{"key1":"value","key2":"value","key3":"value"}
		//	Spaces on either side of {",:} are handled

			if ( string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(tag) )
				return "";

			try
			{
				int    j;
				int    k     = 0;
				int    h     = 1;
				string value = "";
				tag          = "\"" + tag.ToUpper() + "\"";

//	Find the outer tag
				if ( tagOuter.Length > 0 ) // So the tag is embedded in another tag
				{
					tagOuter = "\"" + tagOuter.ToUpper() + "\"";
					while ( value.Length == 0 )
					{
						k = data.ToUpper().IndexOf(tagOuter,k);
						if ( k < 0 )
							return "";
						for ( j = k+tagOuter.Length ; j < data.Length ; j++ )
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
					data  = value;
					value = "";
					k     = 0;
				}

//	Find the inner (main) tag
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

				if ( arrayPosition > 0 ) // Look for [...]
				{
					h = value.IndexOf("[");
					k = value.IndexOf("]",h+1);
					if ( h >= 0 && k > h )
					{
						value = value.Substring(h+1,k-h-1).Trim();
						j     = 0;
						while ( j < arrayPosition && value.Length > 0 )
						{
							j++;
							h = value.IndexOf("{");
							k = value.IndexOf("}",h+1);
							if ( h >= 0 && k > h )
								if ( j == arrayPosition )
								{
									value = value.Substring(h+1,k-h-1).Trim();
		  							if ( decode == 1 ) // URL decode
										return System.Net.WebUtility.UrlDecode(value);
									if ( decode == 2 ) // HTML decode
										return System.Net.WebUtility.HtmlDecode(value);
									return value;
								}
								else
									value = value.Substring(k+1).Trim();
							else
								break;
						}
					}
					return "";
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

				value = value.Substring(h,k-h).Trim();

				if ( decode == 1 ) // URL decode
					return System.Net.WebUtility.UrlDecode(value);
				
				if ( decode == 2 ) // HTML decode
					return System.Net.WebUtility.HtmlDecode(value);
				
				return value;
			}
			catch
			{ }
			return "";
		}

		public static string HTMLValue(string data,string htmlName,string htmlId="")
		{
			try
			{
				data     = Tools.NullToString(data);
				htmlName = Tools.NullToString(htmlName).ToUpper();
				htmlId   = Tools.NullToString(htmlId).ToUpper();
				if ( data.Length < 1 )
					return "";
				if ( htmlName.Length < 1 && htmlId.Length < 1 )
					return "";

				int k;
				int j;

				if ( htmlId.Length > 0 )
					k = data.ToUpper().Replace("\"","'").IndexOf("ID='"+htmlId+"'");
				else
					k = data.ToUpper().Replace("\"","'").IndexOf("NAME='"+htmlName+"'");
				if ( k < 1 )
					return "";
				data = data.Substring(k);
				j    = data.IndexOf(">");
				k    = data.ToUpper().IndexOf("VALUE=");
				if ( k > 0 && k < j-6 )
				{
					data = data.Substring(k+6,j-k-6);
					k    = data.Replace("\"","'").IndexOf("'");
					j    = data.Replace("\"","'").IndexOf("'",k+1);
					if ( j > k+1 )
						return data.Substring(k+1,j-k-1).Trim();					
				}
			}
			catch
			{ }
			return "";
		}

		public static string XMLCell(string tagName,string tagData,int maxLength=0)
		{
			string p = XMLSafe(tagData);
			if ( p.Length < 1 )
				p = " ";
			else if ( maxLength > 0 && p.Length > maxLength )
				p = p.Substring(0,maxLength);
			return "<" + tagName + ">" + p + "</" + tagName + ">";
		}

		public static string XMLNode(XmlDocument xmlDoc,string xmlTag,string nsPrefix="",string nsURL="",string parentNode="",string attribute="",byte trimMode=0)
		{
			try
			{
			//	Quick check, if not there just exit
				if ( xmlTag.Length > 0 && ! xmlDoc.OuterXml.ToString().ToUpper().Contains(xmlTag.ToUpper()))
					return "";

				string ret = "";

				if ( parentNode.Length > 0 )
					try
					{	
						XmlNodeList xList = xmlDoc.GetElementsByTagName(parentNode);
						if ( xList != null )
							foreach (XmlNode p in xList.Item(0).ChildNodes)
								if ( p.Name == xmlTag )
									return ( trimMode == 93 ? p.InnerText : p.InnerText.Trim() );
					}
					catch { }

				if ( attribute.Length > 0 )
					try
					{
						XmlNode xNode = xmlDoc.SelectSingleNode("//"+xmlTag);
						return xNode.Attributes[attribute].Value;
					}
					catch { }

				if ( nsPrefix.Length == 0 || nsURL.Length == 0 )
				{
					try
					{	
						ret = xmlDoc.SelectSingleNode("//"+xmlTag).InnerText;
					//	XmlElement p = xmlDoc.GetElementById(xmlTag);
					}
					catch { }
					if ( ret == null || ret.Length == 0 )
						ret = xmlDoc.GetElementsByTagName(xmlTag).Item(0).InnerText;
				}
				else
				{
					XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
					nsMgr.AddNamespace(nsPrefix,nsURL);
					ret = xmlDoc.SelectSingleNode("//"+nsPrefix+":"+xmlTag,nsMgr).InnerText;
				}
				if ( trimMode == 93 ) // Don't trim
					return ret;
				return ret.Trim();
			}
			catch
			{ }
			return "";
		}

		public static string HTMLSafe(string str)
		{
		// Converts a string to safe format for HTML:

			if ( string.IsNullOrWhiteSpace(str) )
				return "";
			return str.Replace("<","&lt;").Replace(">","&gt;").Replace("'","&#39;").Replace(Environment.NewLine,"<br />");
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

		public static string URLString(string str,byte exceptionType=0)
		{
			if ( string.IsNullOrWhiteSpace(str) )
				return "";
			if ( exceptionType == 1 && ( str.Contains("{{{") || str.Contains("}}}") ) ) // TokenEx
				return str.Trim();
			return System.Net.WebUtility.UrlEncode(str.Trim());
//			return System.Net.WebUtility.HtmlEncode(str.Trim());
//			return str.Trim();
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
			if ( mode == 63 ) // It is actually as numeric so no quotes on each side
				str = str.Replace("'"," ");
			else
				str = "'" + str.Replace("'","''") + "'";
			if ( mode == 47 ) // Unicode
				return "N" + str;
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

		public static string LogFileName(string settingName, DateTime fileDate)
		{
			try
			{
				string fName = Tools.ConfigValue(settingName);
				if ( fName.Length < 1 )
				{
					fName = SystemFolder("");
					if ( fName.Length > 0 )
						fName = fName + "Logs";
					else
						fName = "C:\\Temp";
					fName = fName + "\\PCILogFile.txt";
				}

				int k = fName.LastIndexOf(".");
				if ( k < 1 )
				{
					fName = fName + ".txt";
					k     = fName.LastIndexOf(".");
				}
				if ( fileDate <= Constants.DateNull )
					fileDate = System.DateTime.Now;
				fName = fName.Substring(0,k) + "-" + DateToString(fileDate,7) + fName.Substring(k);
				return fName;
			}
			catch
			{
			}
			return "C:\\Temp\\Error-X.txt";
		}

		private static void LogWrite(string settingName, string component, string msg, Object caller)
		{
		// Use this routine to log internal errors ...
			string       fNameX  = "";
			FileStream   fHandle = null;
			StreamWriter fOut    = null;

			try
			{
				if ( caller != null )
				{
					if ( caller.GetType() == Type.GetType("System.String") )
						component = caller + "." + component;
					else
					{
						string h = caller.GetType().ToString();
						int    p = h.IndexOf(",");
						if ( p > 0 )
							h = h.Substring(0,p).Trim();
						if ( h.Length > 0 )
							component = h + "." + component;
					}
				}
				fNameX = Tools.LogFileName(settingName,System.DateTime.Now);
				if ( File.Exists(fNameX) )
					fHandle = File.Open(fNameX, FileMode.Append);
				else
					fHandle = File.Open(fNameX, FileMode.Create);
				fOut = new StreamWriter(fHandle,System.Text.Encoding.Default);
				fOut.WriteLine( "[v" + SystemDetails.AppVersion + ", " + Tools.DateToString(System.DateTime.Now,1,1,false) + "] " + component + " : " + msg);
			}
			catch (System.Threading.ThreadAbortException)
			{
			//	Ignore
			}
			catch (Exception ex)
			{
				if ( settingName.Length > 0 ) // To prevent recursion ...
					LogWrite("","Tools.LogWrite","fName = '" + fNameX + "', Error = " + ex.Message,SystemDetails.AppID);
			}
			finally
			{
				if ( fOut != null )
					try
					{
						fOut.Close();
					}
					catch { }
				if ( fHandle != null )
					try
					{
						fHandle.Close();
					}
					catch { }
				fOut    = null;
				fHandle = null;
			}
		}

		private static void LogWriteWithSuffix(string settingName, string component, string msg)
		{
		// Use this routine to log internal errors ...
		//	Use (A), (B), (C), etc after the file name (eg. Error-2020-06-19(A).txt)
			int    k      = 0;
			string fName1 = "";

			try
			{
				fName1 = Tools.ConfigValue(settingName);
				if ( fName1.Length < 1 )
				{
					fName1 = SystemFolder("");
					if ( fName1.Length > 0 )
						fName1 = fName1 + "Logs";
					else
						fName1 = "C:\\Temp";
					fName1 = fName1 + "\\PCILogFile.txt";
				}

				k = fName1.LastIndexOf(".");
				if ( k < 1 )
				{
					fName1 = fName1 + ".txt";
					k      = fName1.LastIndexOf(".");
				}
				fName1 = fName1.Substring(0,k) + "-" + DateToString(System.DateTime.Now,7) + "(x)" + fName1.Substring(k);

				for ( k = 1 ; k < 10 ; k++ )
				{
					FileStream   fHandle = null;
					StreamWriter fOut    = null;
					try
					{
						string fName2 = fName1.Replace("(x)","(" + System.Convert.ToChar(k+64) + ")");
						if ( File.Exists(fName2) )
							fHandle = File.Open(fName2, FileMode.Append);
						else
							fHandle = File.Open(fName2, FileMode.Create);
						fOut = new StreamWriter(fHandle,System.Text.Encoding.Default);
						fOut.WriteLine( "[v" + SystemDetails.AppVersion + ", " + Tools.DateToString(System.DateTime.Now,1,1,false) + "] " + component + " : " + msg);
						break;
					}
					catch
					{ }
					finally
					{
						if ( fOut != null )
							try
							{
								fOut.Close();
							}
							catch { }
						if ( fHandle != null )
							try
							{
								fHandle.Close();
							}
							catch { }
						fOut    = null;
						fHandle = null;
					}
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
			//	Ignore
			}
			catch (Exception ex2)
			{
				if ( settingName.Length > 0 ) // To prevent recursion ...
					LogWriteWithSuffix("","Tools.LogWrite","fName = '" + fName1 + "', k = " + k.ToString() + ", Error = " + ex2.Message);
			}
		}

//		public static void LogException(string component, string msg, Exception ex=null)
//		{
//		// Use this routine to log error messages
//			msg = ( msg.Length == 0 ? "" : " (" + msg + ")" );
//			if ( ex == null )
//				msg = "Non-exception error" + msg;
//			else if ( ex.GetType() == typeof(System.IndexOutOfRangeException) )
//				msg = "SQL column not found" + msg;
//			else
//				msg = ex.Message + msg + " : [" + ex.ToString() + "]";
//			LogWrite("LogFileErrors",component,msg);
//		}

		public static void LogException(string component, string msg, Exception ex=null, Object caller=null)
		{
			if ( ex != null )
			{
				if ( msg.Length > 0 )
					msg = " (" + msg + ")";
				if ( ex.GetType() == typeof(System.IndexOutOfRangeException) )
					msg = "SQL column not found" + msg;
				else
					msg = ex.Message + msg + " : [" + ex.ToString() + "]";
			}
			LogWrite("LogFileErrors",component,msg,caller);
		}

		public static void LogInfo(string component, string msg, byte severity=10, Object caller=null)
		{
		// Use this routine to log debugging/info messages
		//	To decide which messages to write, adjust the severity below
		//	Calling routines must supply a severity between 0-255 (default 10)
		//	If severity == 255 then do NOT log for LIVE

			if ( severity == 255 && SystemIsLive() )
				return;

			if ( logSeverity < 1 )
			{
				int h = Tools.StringToInt(ConfigValue("LogSeverity"));
				if ( h > 0 && h < byte.MaxValue )
					logSeverity = (byte)h;
				else
					logSeverity = 100;
			}

			if ( severity >= logSeverity )
				LogWrite("LogFileInfo",component,msg,caller);

//			{
//				string h = "";
//				if ( caller != null )
//				{
//					h      = caller.ToString();
//					int  k = h.IndexOf(",");
//					if ( k > 0 )
//						h = h.Substring(0,k).Trim();
//					if ( h.Length > 0 )
//						h = h + ".";
//				}
//				LogWrite("LogFileInfo",h+component,msg,caller);
//			}

		}

		public static bool CheckPIN(string pin,byte length=0)
		{
			pin   = NullToString(pin);
			int x = StringToInt(pin);
			return ( x > 0 && ( pin.Length == length || length < 1 ) );
		}

		public static bool CheckPhone(string phone)
		{
			phone = NullToString(phone).Replace(" ","").Replace("(","").Replace(")","");
			if ( phone.Length < 8 )
				return false;
			if ( phone.StartsWith("0") || phone.StartsWith("+") )
				for ( int k = 0 ; k < phone.Length ; k++ )
					if ( ! ("0123456789").Contains(phone.Substring(k,1)) )
						return false;
			return true;
		}

		public static bool CheckEMail(string email,byte mode=2)
		{
		//	Mode = 1. Exactly 1 address allowed (ie. no commas, semi-colons or spaces), but not blank.
		//	Mode = 2. 1 or more (multiple) addresses allowed, but not blank.
		//	Mode = 3. 0 or more (multiple) addresses allowed, OR blank (no addresses).

			email = NullToString(email);

			if ( email.Length < 1 )
				return ( mode == 3 );

			if ( email.Length < 6 || email.Contains("(") || email.Contains(")") || email.Contains("<") || email.Contains(">") )
				return false;
		
			if ( mode == 1 && ( email.Contains(",") || email.Contains(";") || email.Contains(" ") ) ) 
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
			return Constants.DateNull;
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
			if ( theDate == null || theDate <= Constants.DateNull )
				theDate = System.DateTime.Now;

			if ( dateOfBirth <= Constants.DateNull || dateOfBirth >= theDate )
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

		public static string ProviderCredentials(string providerName,string keyType,string keySubType="")
		{
			string keyValue = "";
			string keyName  = providerName + "/" + keyType;
			if ( keySubType.Length > 0 )
				keyValue = ConfigValue(keyName+"/"+keySubType);
			if ( keyValue.Length < 1 )
				keyValue = ConfigValue(keyName);
			return keyValue;
		}

		public static string ConfigValue(string configName)
		{
			try
			{
				object p = System.Configuration.ConfigurationManager.AppSettings[configName.Trim()];
				if ( p  != null )
					return p.ToString().Trim();
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

				string folder = Tools.SystemFolder("TempFiles");

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
				fileNameFixed = Tools.SystemFolder("TempFiles") + fileName + "-";

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
				ret = ret + str.Substring(0,k) + Constants.HTMLBreak;
				str = str.Substring(k+1).Trim();
			}
			return ret + str;
		}

		public static bool SystemViaBackDoor()
		{
			return ( ConfigValue("Access/BackDoor") == ((int)Constants.SystemPassword.BackDoor).ToString() );
		}

		public static bool SystemIsLive()
		{
			return SystemLiveTestOrDev() == Constants.SystemMode.Live;
		}

		public static Constants.SystemMode SystemLiveTestOrDev()
		{
			string mode = Tools.ConfigValue("SystemMode").ToUpper();
			if ( mode.StartsWith("LIVE") || mode.StartsWith("PROD") )
				return Constants.SystemMode.Live;
			if ( mode.StartsWith("TEST") )
				return Constants.SystemMode.Test;
			return Constants.SystemMode.Development;
		}

//		public static Constants.PaymentProvider BureauCode(string providerCode)
//		{
//			int   provider    = Tools.StringToInt(providerCode);
//			int[] bureauCodes = (int[])Enum.GetValues(typeof(Constants.PaymentProvider));
//
//			for ( int k = 0 ; k < bureauCodes.Length ; k++ )
//				if ( bureauCodes[k] == provider )
//					return Constants.PaymentProvider[]
//				{
//					base.LoadBureauDetails(Tools.BureauCode()
//				}
//		}

		public static string BureauCode(Constants.PaymentProvider providerCode)
		{
			return ((short)providerCode).ToString().PadLeft(3,'0');
		}

		public static string BureauURL(string bureauCode) // Constants.PaymentProvider providerCode)
		{
//		//	Providers where live and test are the same URL

			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA) ||
				  bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_EU)  ||
				  bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_Asia) )
				return "https://api.stripe.com";

			else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate) )
				return "https://secure.paygate.co.za/payhost/process.trans";

		//	Providers where live and test are different

		//	LIVE
			else if ( Tools.SystemIsLive() )
			{
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
					return "https://api.nets.com.sg/GW2/TxnReqListener";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate) )
					return "https://www.mygate.co.za/Collections/1x0x0/pinManagement.cfc?wsdl";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
					return "https://secureacceptance.cybersource.com/silent";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
					return "https://secureacceptance.cybersource.com/silent";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "https://api.paymentsos.com";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "https://secure.payu.co.za";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
					return "https://pay.ms.fnb.co.za";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentCloud) )
					return "https://api.authorize.net/xml/v1/request.api";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
					return "https://secure.worldpay.com/jsp/merchant/xml/paymentService.jsp";
			}

		//	TESTING
			else
			{
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "https://staging.payu.co.za";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
					return "https://sandbox.ms.fnb.co.za";
				if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentCloud) )
					return "https://apitest.authorize.net/xml/v1/request.api";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
					return "https://secure-test.worldpay.com/jsp/merchant/xml/paymentService.jsp";
			}

//		//	Providers where live and test are the same URL
//
//			if ( providerCode == Constants.PaymentProvider.Stripe_USA ||
//				  providerCode == Constants.PaymentProvider.Stripe_EU  ||
//				  providerCode == Constants.PaymentProvider.Stripe_Asia )
//				return "https://api.stripe.com";
//
//			else if ( providerCode == Constants.PaymentProvider.PayGate )
//				return "https://secure.paygate.co.za/payhost/process.trans";
//
//		//	Providers where live and test are different
//
//		//	LIVE
//			else if ( Tools.SystemIsLive() )
//			{
//				if ( providerCode == Constants.PaymentProvider.eNETS )
//					return "https://api.nets.com.sg/GW2/TxnReqListener";
//				if ( providerCode == Constants.PaymentProvider.MyGate )
//					return "https://www.mygate.co.za/Collections/1x0x0/pinManagement.cfc?wsdl";
//				if ( providerCode == Constants.PaymentProvider.CyberSource )
//					return "https://secureacceptance.cybersource.com/silent";
//				if ( providerCode == Constants.PaymentProvider.CyberSource_Moto )
//					return "https://secureacceptance.cybersource.com/silent";
//				if ( providerCode == Constants.PaymentProvider.PaymentsOS )
//					return "https://api.paymentsos.com";
//				if ( providerCode == Constants.PaymentProvider.PayU )
//					return "https://secure.payu.co.za";
//				if ( providerCode == Constants.PaymentProvider.FNB )
//					return "https://pay.ms.fnb.co.za";
//				if ( providerCode == Constants.PaymentProvider.PaymentCloud )
//					return "https://api.authorize.net/xml/v1/request.api";
//			}
//
//		//	TESTING
//			else
//			{
//				if ( providerCode == Constants.PaymentProvider.PayU )
//					return "https://staging.payu.co.za";
//				if ( providerCode == Constants.PaymentProvider.FNB )
//					return "https://sandbox.ms.fnb.co.za";
//				if ( providerCode == Constants.PaymentProvider.PaymentCloud )
//					return "https://apitest.authorize.net/xml/v1/request.api";
//			}

			return "";
		}

		public static string SystemCode(Constants.ApplicationCode appCode)
		{
			return ((short)appCode).ToString().PadLeft(3,'0');
		}

		public static string TradingProviderCode(Constants.TradingProvider providerCode)
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
				Tools.LogInfo("Tools.SQLDebug/2",str,250);
				ret.Append(str+Constants.HTMLBreak);

				Tools.OpenDB(ref conn);

				if ( conn.Execute(sql) )
				{
					str = "Execution successful, column count = " + conn.ColumnCount.ToString() + ( conn.EOF ? " (NO rows)" : " (At least one row)" );
					Tools.LogInfo("Tools.SQLDebug/3",str,250);
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
							str = str + conn.ColUniCode("",k);
						else
							str = str + conn.ColValue(k);
						Tools.LogInfo("Tools.SQLDebug/4",str,250);
						ret.Append(str+Constants.HTMLBreak);
					}
				}
				else
				{
					str = "Execution failed";
					Tools.LogInfo("Tools.SQLDebug/5",str,250);
					ret.Append(str+Constants.HTMLBreak);
				}
			}
			catch (Exception ex)
			{
				str = "Error : " + ex.Message;
				Tools.LogInfo("Tools.SQLDebug/6",str,250);
				ret.Append(str+Constants.HTMLBreak);
			}
			finally
			{
				Tools.CloseDB(ref conn);
			}
			return ret.ToString();
		}


		public static string MaskedValue(string strToMask)
		{
			strToMask = NullToString(strToMask);
			if ( strToMask.Length >= 40 )
				return strToMask.Substring(0,10) + "****************" + strToMask.Substring(26);
			if ( strToMask.Length >= 30 )
				return strToMask.Substring(0, 8) + "**********"       + strToMask.Substring(18);
			if ( strToMask.Length >= 20 )
				return strToMask.Substring(0, 6) + "********"         + strToMask.Substring(14);
			if ( strToMask.Length >= 13 )
				return strToMask.Substring(0, 6) + "******"           + strToMask.Substring(12);
			if ( strToMask.Length >= 11 )
				return strToMask.Substring(0, 4) + "******"           + strToMask.Substring(10);
			if ( strToMask.Length >=  9 )
				return strToMask.Substring(0, 3) + "*****"            + strToMask.Substring( 8);
			if ( strToMask.Length >=  5 )
				return strToMask.Substring(0, 2) + "***"              + strToMask.Substring( 5);
			if ( strToMask.Length >=  1 )
				return strToMask.Substring(0, 1) + "****";
			return "";
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
			byte severity = 229;

			try
			{
				LogInfo("Tools.CreatePDF/10","Create empty file for PDF",severity);
				StreamWriter fileOut = null;
				fileName = Tools.CreateFile(ref fileOut,fileSource,"pdf");
				if ( fileOut == null )
					return 10;
				fileOut.Close();
				fileOut = null;
				if ( fileName.Length < 1 )
					return 20;
				LogInfo("Tools.CreatePDF/20","File created : " + fileName,severity);

//	NO! Do NOT do this!
//				html = html.Trim().Replace(Environment.NewLine,"<br />");
//	NO!

//	PDF code removed so as not to create a huge DLL

//	SelectPDF code, works fine on Windows but NOT on MS Azure
//
//				SelectPdf.HtmlToPdf   converter = new SelectPdf.HtmlToPdf();
//				SelectPdf.PdfDocument doc       = converter.ConvertHtmlString(html);
//				doc.Save(fileName);
//				doc.Close();

//	IronPDF code, works fine on Windows but NOT on MS Azure
//
//				LogInfo("Tools.CreatePDF/30","Set up IronPDF object",severity);
//				IronPdf.HtmlToPdf   converter = new IronPdf.HtmlToPdf();
//				LogInfo("Tools.CreatePDF/40","Convert HTML to PDF",severity);
//				IronPdf.PdfDocument doc       = converter.RenderHtmlAsPdf(html);
//				LogInfo("Tools.CreatePDF/50","Save PDF to file " + fileName,severity);
//				doc.SaveAs(fileName);
//				LogInfo("Tools.CreatePDF/60","Exit",severity);

				return 0;
			}
			catch (Exception ex)
			{
				LogInfo("Tools.CreatePDF/98","Failed ... " + ex.Message,severity);
				LogException("Tools.CreatePDF/99","File="+fileName+"\n"+html,ex);
			}
			return 30;
		}

		public static string TickerName(int tickerType)
		{
			if ( tickerType == (int)Constants.TickerType.IBStockPrices          ) return "IB/Stock Prices";
			if ( tickerType == (int)Constants.TickerType.IBExchangeRates        ) return "IB/Exchange Rates";
			if ( tickerType == (int)Constants.TickerType.IBPortfolio            ) return "IB/Portfolio";
//			if ( tickerType == (int)Constants.TickerType.IBOrders               ) return "IB/Orders";
			if ( tickerType == (int)Constants.TickerType.FinnHubStockPrices     ) return "FH/Stock Prices";
			if ( tickerType == (int)Constants.TickerType.FinnHubStockHistory    ) return "FH/Stock History";
			if ( tickerType == (int)Constants.TickerType.FinnHubExchangeRates   ) return "FH/Exchange Rates";
			if ( tickerType == (int)Constants.TickerType.FinnHubStockTicks      ) return "FH/Stock Tick Data";
			return "";
		}

		public static JObj JSONToObject<JObj>(string jsonStr)
		{
			JObj                       obj;
			MemoryStream               ms = null;
			DataContractJsonSerializer serializer;

			jsonStr = NullToString(jsonStr);
			if ( jsonStr.Length < 1 )
				return default(JObj);

			try
			{
				obj        = Activator.CreateInstance<JObj>();
				ms         = new MemoryStream(Encoding.Unicode.GetBytes(jsonStr));
				serializer = new DataContractJsonSerializer(obj.GetType());
				obj        = (JObj)serializer.ReadObject(ms);
				ms.Close();
				ms         = null;
				return obj;
			}
			catch (Exception ex)
			{
				LogException("Tools.JSONToObject/10",jsonStr,ex);
				LogInfo     ("Tools.JSONToObject/20",jsonStr,220);
			}
			finally
			{
				if ( ms != null )
					ms.Close();
				ms         = null;
				serializer = null;
			}
			return default(JObj);
		}

		public static string GenerateHMAC(string infoToHash, string secretKey)
		{
//		Used for TokenEx
			var hmac = new System.Security.Cryptography.HMACSHA256();
			hmac.Key = System.Text.Encoding.UTF8.GetBytes(secretKey);
			var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(infoToHash));
			return Convert.ToBase64String(hash); // Ensure the string returned is Base64 Encoded
		}

		public static string DecodeWebException(System.Net.WebException ex1,string callingModule="",string extraData="")
		{
			string responseContent = "";
			string errorContent    = "";
			callingModule          = callingModule + "[DecodeWebException/";
			extraData              = extraData.Trim() + " ";

			try
			{
				System.Net.HttpWebResponse errorResponse = ex1.Response as System.Net.HttpWebResponse;
				if ( errorResponse == null )
				{
					Tools.LogInfo     (callingModule+"1]",extraData + "(" + ex1.Message + ")",245);
					Tools.LogException(callingModule+"2]",extraData,ex1);
					return "";
				}

				int k = 0;

				using ( StreamReader sR = new StreamReader(errorResponse.GetResponseStream()) )
					responseContent = sR.ReadToEnd();

				errorContent = responseContent + ", Response Headers=";
				foreach (string key in errorResponse.Headers.AllKeys )
					errorContent = errorContent + Environment.NewLine + "[" + (k++).ToString() + "] " + key + " : " + errorResponse.Headers[key];

				Tools.LogInfo     (callingModule+"4]",extraData + errorContent,245);
				Tools.LogException(callingModule+"5]",extraData + errorContent,ex1);

//				if ( callingModule.Length > 0 )
//				{
//					extraData = ( extraData.Length == 0 ? "" : extraData + ". " ) + "(WebException) ";
//					Tools.LogInfo     (callingModule+"6]",extraData + errorContent,245);
//					Tools.LogException(callingModule+"7]",extraData + errorContent,ex1);
//				}
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     (callingModule+"8]",extraData + errorContent,245);
				Tools.LogException(callingModule+"9]",extraData + errorContent,ex2);
			}
			return responseContent;
		}

		public static string CardIssuer(string cardNumber)
		{
			cardNumber = Tools.NullToString(cardNumber);
			if ( cardNumber.Length < 4 )
				return "Invalid";
			if ( cardNumber.StartsWith("4") )
				return "Visa";
			if ( cardNumber.Substring(0,2).CompareTo("51") >= 0 && cardNumber.Substring(0,2).CompareTo("55") <= 0 )
				return "MasterCard";
			if ( cardNumber.Substring(0,4).CompareTo("2221") >= 0 && cardNumber.Substring(0,4).CompareTo("2720") <= 0 )
				return "MasterCard";
			if ( cardNumber.StartsWith("34") || cardNumber.StartsWith("37") )
				return "AmEx";
			if ( cardNumber.StartsWith("36") )
				return "Diners";
			if ( cardNumber.StartsWith("6011") || cardNumber.StartsWith("644") || cardNumber.StartsWith("645") || cardNumber.StartsWith("646") || cardNumber.StartsWith("647") || cardNumber.StartsWith("648") || cardNumber.StartsWith("649") || cardNumber.StartsWith("65") )
				return "Discover";
			if ( cardNumber.Substring(0,6).CompareTo("622126") >= 0 && cardNumber.Substring(0,6).CompareTo("622925") <= 0 )
				return "Discover";
			return "Unknown";
		}

		public static string ImageFolder(string defaultDir="")
		{
			string folder = ConfigValue("ImageFolder");
			if ( folder.Length < 1 )
				if ( defaultDir.Length < 1 )
					return "Images/";
				else
					folder = defaultDir;
			if ( folder.EndsWith("/") )
				return folder;
			return folder + "/";
		}

		public static Transaction CreateTransaction(string bureauCode)
		{
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU)             ) return new TransactionPayU();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ikajo)            ) return new TransactionIkajo();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24)              ) return new TransactionT24();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate)           ) return new TransactionMyGate();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate)          ) return new TransactionPayGate();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentCloud)     ) return new TransactionPaymentCloud(); // Renamed from Authorize.Net
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB)              ) return new TransactionFNB();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay)         ) return new TransactionWorldPay();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGenius)        ) return new TransactionPayGenius();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ecentric)         ) return new TransactionEcentric();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS)            ) return new TransactionENets();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Peach)            ) return new TransactionPeach();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx)          ) return new TransactionTokenEx();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS)       ) return new TransactionPaymentsOS();
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA)       ) return new TransactionStripe(Constants.PaymentProvider.Stripe_USA);
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_EU)        ) return new TransactionStripe(Constants.PaymentProvider.Stripe_EU);
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_Asia)      ) return new TransactionStripe(Constants.PaymentProvider.Stripe_Asia);
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource)      ) return new TransactionCyberSource(Constants.PaymentProvider.CyberSource);
			if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) ) return new TransactionCyberSource(Constants.PaymentProvider.CyberSource_Moto);
			return null;
		}

		public static string TransactionTypeName(byte transactionType)
		{
			if ( transactionType == (byte)Constants.TransactionType.CardPayment           ) return "Card Payment";
			if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty ) return "Payment via 3rd Party";
			if ( transactionType == (byte)Constants.TransactionType.DeleteToken           ) return "Delete Token";
			if ( transactionType == (byte)Constants.TransactionType.GetCardFromToken      ) return "Get Card from Token";
			if ( transactionType == (byte)Constants.TransactionType.GetToken              ) return "Get Token from Card";
			if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty    ) return "Token via 3rd Party";
			if ( transactionType == (byte)Constants.TransactionType.ManualPayment         ) return "Manual Payment";
			if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment   ) return "3d Secure Payment";
			if ( transactionType == (byte)Constants.TransactionType.ThreeDSecureCheck     ) return "3d Secure Check";
			if ( transactionType == (byte)Constants.TransactionType.TokenPayment          ) return "Token Payment";
			if ( transactionType == (byte)Constants.TransactionType.Reversal              ) return "Payment Reversal";
			if ( transactionType == (byte)Constants.TransactionType.Refund                ) return "Refund";
			if ( transactionType == (byte)Constants.TransactionType.Transfer              ) return "Transfer";
			if ( transactionType == (byte)Constants.TransactionType.TransactionLookup     ) return "Transaction Lookup";
			if ( transactionType == (byte)Constants.TransactionType.ZeroValueCheck        ) return "Zero-Value Validation";
			if ( transactionType == (byte)Constants.TransactionType.AccountUpdate         ) return "Account Update";
			if ( transactionType == (byte)Constants.TransactionType.Test                  ) return "Test";
			return "Unknown (transactionType=" + transactionType.ToString() + ")";
		}

		public static string ErrorTypeName(int errType)
		{
			if ( errType == (int)Constants.ErrorType.InvalidMenu )
				return "Invalid/missing menu for this application/language";
			return "";
		}

		public static string LoadGoogleAnalytics(string productCode,byte version=3,string transactionId="",byte noScript=0)
		{
			string sql     = "exec sp_WP_Get_GoogleACA @ProductCode=" + Tools.DBString(productCode);
			string gScript = "";
			string gaCode  = "";
			string url     = "";

			if ( version < 1 )
				version   = 3;

//	Version 3. See code further down
//			if ( version == 3 && noScript == 0 )
//				return "<script>" + Environment.NewLine
//				     + "(function(w,d,s,l,i)" + Environment.NewLine
//				     + "{w[l]=w[l]||[];w[l].push({'gtm.start':new Date().getTime(),event:'gtm.js'});" + Environment.NewLine
//				     + "var f=d.getElementsByTagName(s)[0]," + Environment.NewLine
//				     + "j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;" + Environment.NewLine
//				     + "j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})" + Environment.NewLine
//				     + "(window,document,'script','dataLayer','GTM-5VHQMGR');" + Environment.NewLine
//				     + "</script>";
//
//			if ( version == 3 && noScript > 0 )
//				return "<noscript>"
//				     + "<iframe src='https://www.googletagmanager.com/ns.html?id=GTM-5VHQMGR' height='0' width='0' style='display:none;visibility:hidden'>" + Environment.NewLine
//				     + "</iframe>"
//				     + "</noscript>";

//			Tools.LogInfo("Tools.LoadGoogleAnalytics/3","ProductCode="+productCode+", SQL="+sql,233);

			using (MiscList miscList = new MiscList())
				try
				{
					if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
					{
						gaCode = miscList.GetColumn("GoogleAnalyticCode");
						url    = miscList.GetColumn("URL");

						if ( gaCode.Length < 1 )
							gaCode  = "GTM-5VHQMGR";
						//	gaCode  = "AW-11030275536"

						if ( version == 3 && noScript == 0 )
						//	From Johrika Burger via Anton Koekemoer at Open Circle Solutions, 2023/04/21
							gScript = "<script>" + Environment.NewLine
							        + "(function(w,d,s,l,i)" + Environment.NewLine
							        + "{w[l]=w[l]||[];w[l].push({'gtm.start':new Date().getTime(),event:'gtm.js'});" + Environment.NewLine
							        + "var f=d.getElementsByTagName(s)[0]," + Environment.NewLine
							        + "j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;" + Environment.NewLine
							        + "j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})" + Environment.NewLine
							        + "(window,document,'script','dataLayer','" + gaCode + "');" + Environment.NewLine
							        + "</script>";

						else if ( version == 3 && noScript > 0 )
						//	From Johrika Burger via Anton Koekemoer at Open Circle Solutions, 2023/04/21
							gScript = "<noscript>"
							        + "<iframe src='https://www.googletagmanager.com/ns.html?id=" + gaCode + "' height='0' width='0' style='display:none;visibility:hidden'>" + Environment.NewLine
							        + "</iframe>"
							        + "</noscript>";

						else if ( version == 2 || gaCode.ToUpper().StartsWith("G") )
							gScript = "<script async src='https://www.googletagmanager.com/gtag/js?id=" + gaCode + "'></script>" + Environment.NewLine
							        + "<script>" + Environment.NewLine
							        + "window.dataLayer = window.dataLayer || [];" + Environment.NewLine
							        + "function gtag(){dataLayer.push(arguments);}" + Environment.NewLine
							        + "gtag('js', new Date());" + Environment.NewLine
							        + "gtag('config', '" + gaCode + "', { 'linker': { 'domains': ['" + url + "'] } } );" + Environment.NewLine
							        + "</script>";

/* From https://developers.google.com/analytics/devguides/collection/gtagjs/cross-domain
gtag('config', 'GA_MEASUREMENT_ID', {
  'linker': {
    'domains': ['example.com']
  }
});
*/
						else
							gScript = "<script>" + Environment.NewLine
							        + "(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){"
							        + "(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),"
							        + "m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)"
							        + "})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');" + Environment.NewLine
							        + "ga('create', '" + gaCode + "', 'auto', {'allowLinker': true});" + Environment.NewLine
							        + "ga('require', 'linker');" + Environment.NewLine
							        + "ga('linker:autoLink', ['" + url + "'] );" + Environment.NewLine
							        + "ga('send', 'pageview');" + Environment.NewLine
							        + "</script>";

					//	Tools.LogInfo("Tools.LoadGoogleAnalytics/5","gCode="+gaCode+", gScript="+gScript,233);
					}
					else
						LogException("Tools.LoadGoogleAnalytics/7","Failed to load Google UA code ("+sql+")");
				}
				catch (Exception ex)
				{
					LogException("Tools.LoadGoogleAnalytics/9",sql,ex);
				}

			if ( gScript.Length > 0 && gaCode.Length > 0 )
			{
				if ( transactionId.Length > 0 )
					gScript = gScript + Environment.NewLine + "<script>"
					                  + "gtag('event', 'conversion', { 'send_to': '" + gaCode + "/CHq3CJGyloMYENDL0osp', 'transaction_id': '" + transactionId + "' });"
					                  + Environment.NewLine + "</script>";
				return Environment.NewLine + gScript + Environment.NewLine;
			}
			return "";
		}

		public static string WebDataTypeName(byte dataType)
		{
			if ( dataType == (byte)Constants.WebDataType.JSON )          return "JSON";
			if ( dataType == (byte)Constants.WebDataType.XML )           return "XML";
			if ( dataType == (byte)Constants.WebDataType.FormPost )	    return "Http form";
			if ( dataType == (byte)Constants.WebDataType.URLParameters ) return "URL string";
			return "Unknown (" + dataType.ToString() + ")";
		}

		public static string LoadChat(string productCode)
		{
			string sql = "exec sp_WP_Get_ChatSnip @ProductCode=" + Tools.DBString(productCode);

			using (MiscList miscList = new MiscList())
				try
				{
					if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
						return miscList.GetColumn("ChatSnippet");
					LogException("Tools.LoadChat/1","Failed to load Chat widget ("+sql+")");
				}
				catch (Exception ex)
				{
					LogException("Tools.LoadChat/2",sql,ex);
				}
			return "";
		}
	}
}