using System;

namespace PCIBusiness
{
//	FinnHub
//	Used to deserialize a JSON result

	public class StockHistory
	{
		public decimal[] c;
		public decimal[] h;
		public decimal[] l;
		public decimal[] o;
		public int[]     t;
		public int[]     v;
		public string    s;

		public int Count
		{
			get
			{
				try
				{
					if ( c != null && h != null && l != null && o != null && t != null && v != null )
						return c.Length;
				}
				catch
				{ }
				return 0;
			}
		}

		public void Clear()
		{
			c = null;
			h = null;
			l = null;
			o = null;
			t = null;
			v = null;
			s = null;
		}
	}
}
