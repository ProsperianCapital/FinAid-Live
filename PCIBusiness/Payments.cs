using System;
using System.Text;

namespace PCIBusiness
{
	public class Payments : BaseList
	{
		private string    bureauCode;
		private int       success;
		private int       fail;
		private int       err;

//		private const int MAX_ROWS = 50; // Default maximum per iteration
//		See Constants.C_MAXPAYMENTROWS()

		public override BaseData NewItem()
		{
			return new   Payment("");
		}

		public int      CountSucceeded
		{
			get { return success; }
		}

		public int      CountFailed
		{
			get { return fail; }
		}
		public Provider Summary(string bureau)
		{
			int      tok        = 0;
			int      pay        = 0;
			Provider provider   = new Provider();
			provider.BureauCode = bureau;

			try
			{
    			sql = "exec sp_Get_CardToToken " + Tools.DBString(bureau) + "," + Constants.C_MAXPAYMENTROWS().ToString();
				err = ExecuteSQL(null,false,false);
				if ( err > 0 )
					Tools.LogException("Payments.Summary/10",sql + " failed, return code " + err.ToString());
				else
					while ( ! dbConn.EOF && tok < Constants.C_MAXPAYMENTROWS() )
					{
						if ( pay == 0 && tok == 0 )
							provider.LoadData(dbConn);
						tok++;
						dbConn.NextRow();
					}
				provider.CardsToBeTokenized = tok;

    			sql = "exec sp_Get_CardPayment " + Tools.DBString(bureau) + "," + Constants.C_MAXPAYMENTROWS().ToString();
				err = ExecuteSQL(null,false,false);
				if ( err > 0 )
					Tools.LogException("Payments.Summary/20",sql + " failed, return code " + err.ToString());
				else
					while ( ! dbConn.EOF && pay < Constants.C_MAXPAYMENTROWS() )
					{
						if ( pay == 0 && tok == 0 )
							provider.LoadData(dbConn);
						pay++;
						dbConn.NextRow();
					}
				provider.PaymentsToBeProcessed = pay;
			}
			catch (Exception ex)
			{
				Tools.LogException("Payments.Summary/30","",ex);
			}
			finally
			{
				Tools.CloseDB(ref dbConn);
			}
			return provider;
		}

		public int ProcessCards(string bureau,byte mode=0,int rowsToProcess=0)
		{
			int    maxRows  = Tools.StringToInt(Tools.ConfigValue("MaximumRows"));
			int    iter     = 0;
			string desc     = "";

			bureauCode = Tools.NullToString(bureau);
			success    = 0;
			fail       = 0;
			maxRows    = ( maxRows < 1 ? Constants.C_MAXPAYMENTROWS() : maxRows );
//			maxRows    = ( maxRows < 1 || maxRows > MAX_ROWS ? MAX_ROWS : maxRows );

			if ( bureauCode.Length < 1 )
				return 0;
			else if ( mode == 1 )
    		{
				sql  = "exec sp_Get_CardToToken " + Tools.DBString(bureauCode);
				desc = "Token";
			}
			else if ( mode == 2 )
    		{
				sql  = "exec sp_Get_CardPayment "  + Tools.DBString(bureauCode);
				desc = "Payment";
			}
			else
				return 0;

			if ( rowsToProcess < 1 )
				rowsToProcess = 0;

			if ( maxRows > 0 && rowsToProcess > 0 )
				sql = sql + "," + Math.Min(maxRows,rowsToProcess).ToString();
			else if ( maxRows > 0 )
				sql = sql + "," + maxRows.ToString();
			else if ( rowsToProcess > 0 )
				sql = sql + "," + rowsToProcess.ToString();
			else
				sql = sql + "," + Constants.C_MAXPAYMENTROWS().ToString();

			Tools.LogInfo("Payments.ProcessCards/10","Mode="+mode.ToString()+", MaxRows=" + maxRows.ToString()+", RowsToProcess=" + rowsToProcess.ToString()+", BureauCode="+bureauCode+", SQL="+sql,199);

			try
			{
				while ( rowsToProcess < 1 || rowsToProcess > success + fail )
				{
					if ( LoadDataFromSQL(maxRows,"Payments.ProcessCards/"+desc+"/"+bureauCode) < 1 )
						break;
					Tools.CloseDB(ref dbConn);
					int rowsDone = 0;
					iter++;
					foreach (Payment payment in objList)
					{
						payment.BureauCode = bureauCode;
						if ( mode == 1 )
							err = payment.GetToken();
						else
							err = payment.ProcessPayment();
						if ( err == 0 )
							success++;
						else
							fail++;
						rowsDone++;
						if ( rowsToProcess > 0 && rowsToProcess <= success + fail )
							break;
					}
					Tools.LogInfo("Payments.ProcessCards/30","Iteration " + iter.ToString() + " (" + rowsDone.ToString() + " " + desc + "s)",199);
//	In case of a runaway loop where failures are not rectified ...
					if ( fail > 99 && success == 0 )
						break;
					if ( fail > 999 )
						break;
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("Payments.ProcessCards/40","Iteration " + iter.ToString() + ", " + desc + " " + (success+fail).ToString(),ex);
			}
			finally
			{
				Tools.CloseDB(ref dbConn);
				Tools.LogInfo("Payments.ProcessCards/90","Finished (" + success.ToString() + " " + desc + "s succeeded, " + fail.ToString() + " "+ desc + "s failed)",199);
			}
			return success+fail;
		}
	}
}
