namespace PCIBusiness
{
	public class Providers : BaseList
	{

		public override BaseData NewItem()
		{
			return new Provider();
		}

		public Provider LoadOne(byte mode,string bureauCode,string userCode)
		{
			sql = "";
			if ( mode == 1 ) // Messaging providers
				sql = "exec sp_Messaging_Get_ProviderInfo "
				    +     " @MessageBureauCode="     + Tools.DBString(bureauCode)
				    +     ",@MessageBureauUserCode=" + Tools.DBString(userCode);

			if ( sql.Length > 0 )
				if ( LoadDataFromSQL(1,"Providers.LoadOne") > 0 )
				{
					Provider msgProvider = (Provider)Item(0);
					string   prData      = msgProvider.BureauCode + " / " + msgProvider.BureauType
			                                                    + " / " + msgProvider.BureauURL
			                                                    + " / " + msgProvider.MerchantUserID
			                                                    + " / " + msgProvider.MerchantPassword
			                                                    + " / " + msgProvider.Sender
			                                                    + " / " + msgProvider.Port.ToString()
			                                                    + " / " + msgProvider.UserCode;
					Tools.LogInfo("LoadOne/1","(Success) "+sql+" = "+prData,Constants.LogSeverity,this);
					return msgProvider;
				}
				else
					Tools.LogInfo("LoadOne/2","(Failure) "+sql,233,this);

			return null;
		}

		public int LoadAll(byte mode=0)
		{
			sql = "";
			if ( mode == 1 ) // Messaging providers
				sql = "exec sp_Messaging_Get_ProviderInfo";

			if ( sql.Length > 0 )
				return LoadDataFromSQL();

			return 0;
		}
	}
}