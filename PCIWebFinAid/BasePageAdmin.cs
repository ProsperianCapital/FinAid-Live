// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePageAdmin : BasePageLogin
	{
		protected override void StartOver(int errNo,int errType=0,string pageName="")
		{
			base.StartOver ( errNo, errType, ( pageName.Length > 0 ? pageName : "pgLogon.aspx" ) );
		}
	}
}
