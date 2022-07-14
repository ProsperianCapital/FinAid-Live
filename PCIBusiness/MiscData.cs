using System;

namespace PCIBusiness
{
	public class MiscData : BaseData
	{
		private System.Drawing.Color foreColor;
		private System.Drawing.Color backColor;
		private string[]             theData;
		private short                colNo;
		private int                  rowNo;

		public string GetColumn(short index)
		{
			try
			{
				return theData[index];
			}
			catch
			{ }
			return "";
		}
		public string NextColumnSubstring(int maxLength=0,string suffix="")
		{
			string h = NextColumn;
			if ( maxLength > 0 && h.Length > maxLength )
				return h.Substring(0,maxLength) + suffix;
			return h;
		}


		public string NextColumnStr
		{
//			Return an empty string if the value is zero
			get
			{
				string h = NextColumn;
				if ( h == "0" ) return "";
				return h;
			}
		}

		public string NextColumn
		{
			get
			{
				if ( colNo < 0 )
					colNo = 0;
				else
					colNo++;
				return GetColumn(colNo);
			}
		}

		public string LastColumn
		{
			get
			{
				if ( colNo < 0 )
					colNo = 0;
				return GetColumn(colNo);
			}
		}

		public override int RowNumber
		{
			get { return rowNo; }
			set { rowNo = value; }
		}

		public System.Drawing.Color ForegroundColor
		{
			get { return foreColor; }
		}

		public System.Drawing.Color BackgroundColor
		{
			get { return backColor; }
		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "MiscData.LoadData";
			colNo             = -88;
			string   dataType;
			decimal  curr;
			DateTime dt;

			try
			{
				for ( int k = 0 ; k < dbConn.ColumnCount ; k++ )
				{
					if ( k > theData.Length )
						break;
					dataType = dbConn.ColDataType("",k).ToUpper();
					if ( dataType.Contains("DATE") )
					{
						dt         = dbConn.ColDate("",k);
						theData[k] = Tools.DateToString(dt,7,0); // yyyy-mm-dd
					}
					else if ( dataType.Contains("DECIMAL") || dataType.Contains("FLOAT") )
					{
						curr       = dbConn.ColDecimal("",k);
						theData[k] = Tools.DecimalToString(curr,2);
					}
					else
						theData[k] = dbConn.ColValue(k);
					if ( theData[k].ToUpper().StartsWith("[FORECOLOR]") )
						foreColor = System.Drawing.Color.FromName(theData[k].Substring(11).Trim());
					else if ( theData[k].ToUpper().StartsWith("[BACKCOLOR]") )
						backColor = System.Drawing.Color.FromName(theData[k].Substring(11).Trim());
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("LoadData","",ex,this);
			}
		}

		public MiscData()
		{
			colNo     = -88;
			theData   = new string[50];
			foreColor = System.Drawing.Color.Empty;
			backColor = System.Drawing.Color.Empty;
		}

		public override void CleanUp()
		{
			theData = null;
		}
	}
}
