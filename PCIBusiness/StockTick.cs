using System;

namespace PCIBusiness
{
//	FinnHub
//	Used to deserialize a JSON result

	public class StockTick
	{
		public decimal[] p;
		public int    [] v;
		public long   [] t;
		public string [] x;
		public string    s;
		public int       skip;
		public int       count;
		public int       total;

		public int Count
		{
			get
			{
				try
				{
					if ( p != null && v != null && t != null && x != null )
						return p.Length;
				}
				catch
				{ }
				return 0;
			}
		}

		public void Clear()
		{
			p     = null;
			v     = null;
			t     = null;
			x     = null;
			s     = "";
			skip  = 0;
			count = 0;
			total = 0;
		}
	}
}
