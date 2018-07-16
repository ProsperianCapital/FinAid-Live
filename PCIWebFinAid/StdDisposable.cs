using System;

namespace PCIWebFinAid
{
   /// <summary>
   /// Summary description for StdDisposable.
   /// This abstract class is a skeleton for all other classes ; it contains the basic
   /// "IDisposable" code to clean up efficiently and correctly. All classes must
   /// derive from "StdDisposable" and must implement abstract method "Close", in which
   /// all cleaning up of resources (managed and unmanaged) should take place.
   /// </summary>

	public abstract class StdDisposable : System.Web.UI.Page, IDisposable
	{
		public abstract void Close(); // Must be overridden in base class ...

		private void Dispose(bool vDispose)
		{
			Close();
		}

		public override sealed void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~StdDisposable()
		{
			Dispose(false) ;
		}
   }
}
