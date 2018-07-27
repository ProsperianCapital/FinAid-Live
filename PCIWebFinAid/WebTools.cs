using System;

namespace PCIWebFinAid
{
	public static class WebTools
	{
		static short debugMode = -888;

		public static string RequestValueString (System.Web.HttpRequest req,string parmName)
		{
//			string ret = "";
			try
			{
				return PCIBusiness.Tools.ObjectToString(req[parmName]);
//				ret = req[parmName];
//				ret = ret.Trim().Replace("<","").Replace(">","");
			}
			catch
			{
//				ret = "";
			}
//			return ret;
			return "";
		}

		public static int RequestValueInt (System.Web.HttpRequest req,string parmName,int minValue=0,int maxValue=int.MaxValue)
		{
			try
			{
				return IntValue((req[parmName]).ToString().Trim(),minValue,maxValue);
			}
			catch
			{ }
			return 0;
		}

		public static int ViewStateInt (System.Web.UI.StateBag viewState,string parmName,int minValue=0,int maxValue=int.MaxValue)
		{
			try
			{
				return IntValue((viewState[parmName]).ToString().Trim(),minValue,maxValue);
			}
			catch
			{ }
			return 0;
		}

		public static int IntValue(string value,int minValue,int maxValue)
		{
			try
			{
				int  tmp  = System.Convert.ToInt32(value);
				if ( tmp >= minValue && tmp <= maxValue )
					return tmp;
			}
			catch
			{ }
			return 0;
		}

		public static int ListAdd ( System.Web.UI.WebControls.ListControl listBox,
		                            int                                   position,
                                  string                                listValue,
                                  string                                listText )
		{
			try
			{
				if ( position > listBox.Items.Count )
					position = listBox.Items.Count;
				else if ( position < 0 )
					position = 0;
				listBox.Items.Insert(position,listText);
				listBox.Items[position].Value = listValue;
				if ( listBox.GetType() == typeof(System.Web.UI.WebControls.RadioButtonList) )
					listBox.Items[position].Attributes.Add("style","white-space:nowrap");
			}
			catch
			{
				return -1;
			}
			return position;
		}

		public static int ListValue ( System.Web.UI.WebControls.ListControl listBox )
		{
			try
			{
				return System.Convert.ToInt32(ListValue(listBox,"0"));
			}
			catch
			{ }
			return 0;
		}

		public static string ListValue ( System.Web.UI.WebControls.ListControl listBox,
		                                 string                                defaultValue )
		{
			string sel = "";
			try
			{
				sel = listBox.SelectedValue;
				if ( sel == null || sel.Length < 1 )
					sel = defaultValue;
			}
			catch
			{
				sel = defaultValue;
			}
			return sel;
		}

		public static void ListSelect ( System.Web.UI.WebControls.ListControl listBox,
		                                string                                selectValue,
		                                string                                defaultValue="0" )
		{
			bool   OK = false;
			string lValue;

			if ( ! selectValue.StartsWith("*/") ) // For lists with values like "3/120"
				try
				{
					listBox.SelectedValue = selectValue;
					OK = true;
				}
				catch { }

			if ( !OK )
			{
				try
				{
					for ( int k = 0 ; k < listBox.Items.Count ; k++ )
					{
						lValue = listBox.Items[k].Value;
						if ( lValue == selectValue )
						{
							listBox.Items[k].Selected = true;
							OK = true;
							break;
						}
						else if ( selectValue.StartsWith("*/") &&
						          lValue.IndexOf("/") > 0 &&
						          selectValue.Substring(2) == lValue.Substring(lValue.IndexOf("/")+1) )
						{
							listBox.Items[k].Selected = true;
							OK = true;
							break;
						}
					}
				}
				catch { }
			}
			if ( !OK && defaultValue.Length > 0 )
			{
				try
				{
					listBox.SelectedValue = defaultValue;
				}
				catch { }
			}
		}

		public static void ListBind ( System.Web.UI.WebControls.ListControl listBox,
		                              object                                dataSource,
		                              string                                dataFieldKey,
		                              string                                dataFieldShow,
		                              string                                addZeroRow,
		                              string                                selectValue )
		{
			ListBind(listBox,dataSource,dataFieldKey,dataFieldShow,addZeroRow,selectValue,-888);
		}

		public static void ListBind ( System.Web.UI.WebControls.ListControl listBox,
		                              object                                dataSource,
		                              string                                dataFieldKey,
		                              string                                dataFieldShow,
		                              string                                addZeroRow,
		                              string                                selectValue,
		                              short                                 selectIndex )
		{
			listBox.DataSource     = dataSource;
			listBox.DataValueField = dataFieldKey;
			listBox.DataTextField  = dataFieldShow;
			listBox.DataBind();
			if ( addZeroRow.Length > 0 )
			{
				listBox.Items.Insert(0,addZeroRow);
				listBox.Items[0].Value = "0";
			}
			if ( listBox.Items.Count > 0 )
			{
				if ( selectValue.Length > 0 )
					ListSelect(listBox,selectValue,"0");
			//	else if ( selectIndex == 0 )
			//		listBox.SelectedIndex = 0;
				else if ( selectIndex >= listBox.Items.Count )
					listBox.SelectedIndex = listBox.Items.Count - 1;
				else if ( selectIndex >= 0 )
					listBox.SelectedIndex = selectIndex;
			}
		}

		public static void Redirect (System.Web.HttpResponse response,string url)
		{
			try
			{
				if ( url.Length < 6 ) url = "Identify.aspx";
				response.Redirect(url,false);
			}
			catch
			{ }
		}

		public static short DebugMode(System.Web.UI.Page webPage,bool load=false)
		{
			if ( load || debugMode < 0 )
				try
				{
					string g  = ((System.Web.UI.WebControls.Literal)webPage.FindControl("DebugMode")).Text;
					debugMode = System.Convert.ToInt16(g);
					if ( debugMode < 0 )
						debugMode = 0;
				}
				catch
				{
					debugMode = 0;
				}

			return debugMode;
		}

		public static string JavaScriptSource(string newScript,string existingScript="",byte beforeOrAfter=2)
		{
			newScript = newScript.Trim();
			if ( newScript.Length < 1 && existingScript.Length < 1 )
				return "";
			else if ( ! existingScript.ToLower().Contains("<script") )
				existingScript = "<script type='text/javascript'>" + existingScript + "</script>";
//			if ( newScript.Length > 0 && beforeOrAfter == 2 ) // After
//				existingScript = existingScript.Replace("</script>",newScript + ( newScript.EndsWith(";") ? "" : ";" ) + "</script>");
			if ( newScript.Length > 0 && beforeOrAfter == 1 ) // Before
				existingScript = existingScript.Replace("script'>","script'>" + newScript + ( newScript.EndsWith(";") ? "" : ";"  ) );
			else if ( newScript.Length > 0 )
				existingScript = existingScript.Replace("</script>",newScript + ( newScript.EndsWith(";") ? "" : ";" ) + "</script>");
			return existingScript;
		}

		public static string ClientIPAddress(System.Web.HttpRequest req)
		{
			string ipAddr = "";

			for ( int k = 1 ; k < 5 ; k++ )
				try
				{
					if      ( k == 1 )
						ipAddr = req.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
					else if ( k == 2 )
						ipAddr = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
					else if ( k == 3 )
						ipAddr = req.ServerVariables["REMOTE_ADDR"];
					else if ( k == 4 )
						ipAddr = req.UserHostAddress;
					if ( !string.IsNullOrEmpty(ipAddr) )
						break;
				}
				catch
				{
					ipAddr = "";
				}

			if ( string.IsNullOrWhiteSpace(ipAddr) )
				ipAddr = "";
			else if ( ipAddr.Contains(",") )
				ipAddr = ipAddr.Split(',')[0];

			return ipAddr;
		}
	}
}