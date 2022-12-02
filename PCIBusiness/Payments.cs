using System;

namespace PCIBusiness
{
	public class Payments : BaseList
	{
		private string  bureauCode;
		private int     success;
		private int     fail;
		private int     err;

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

		public void     Summary(Provider provider,string bureau="")
		{
			if ( provider == null )
			{
				provider            = new Provider();
				provider.BureauCode = bureau;
			}

			int tok = 0;
			int pay = 0;

			if ( provider.PaymentType == (byte)Constants.TransactionType.TokenPayment )
				try
				{
		  			sql = "exec sp_Get_CardToToken " + Tools.DBString(provider.BureauCode) + "," + Constants.MaxRowsPayment.ToString();
					err = ExecuteSQL(null,false,false);
					if ( err == 0 )
						while ( ! dbConn.EOF && tok < Constants.MaxRowsPayment )
						{
							if ( pay == 0 && tok == 0 )
								provider.LoadData(dbConn);
							tok++;
							dbConn.NextRow();
						}
//					else
//						Tools.LogException("Summary/10",sql + " failed, return code " + err.ToString(),null,this);

					provider.CardsToBeTokenized = tok;

					sql = "exec sp_Get_TokenPayment " + Tools.DBString(provider.BureauCode) + "," + Constants.MaxRowsPayment.ToString();
					err = ExecuteSQL(null,false,false);
					if ( err == 0 )
						while ( ! dbConn.EOF && pay < Constants.MaxRowsPayment )
						{
							if ( pay == 0 && tok == 0 )
								provider.LoadData(dbConn);
							pay++;
							dbConn.NextRow();
						}
//					else
//						Tools.LogException("Summary/20",sql + " failed, return code " + err.ToString(),null,this);

					provider.PaymentsToBeProcessed = pay;
				}
				catch (Exception ex)
				{
					Tools.LogException("Summary/30","",ex,this);
				}
				finally
				{
					Tools.CloseDB(ref dbConn);
				}

			else if ( provider.PaymentType == (byte)Constants.TransactionType.CardPayment )
				try
				{
					provider.CardsToBeTokenized = 0;

					sql = "exec sp_Get_CardPayment " + Tools.DBString(provider.BureauCode) + "," + Constants.MaxRowsPayment.ToString();
					err = ExecuteSQL(null,false,false);
					if ( err > 0 )
						Tools.LogException("Summary/40",sql + " failed, return code " + err.ToString(),null,this);
					else
						while ( ! dbConn.EOF && pay < Constants.MaxRowsPayment )
						{
							if ( pay == 0 )
								provider.LoadData(dbConn);
							pay++;
							dbConn.NextRow();
						}
					provider.PaymentsToBeProcessed = pay;
				}
				catch (Exception ex)
				{
					Tools.LogException("Summary/50","",ex,this);
				}
				finally
				{
					Tools.CloseDB(ref dbConn);
				}
		}

		public int ProcessCards(string bureau,byte transactionType=0,int rowsToProcess=0,string bureaCodeTokenize="",byte logPriority=222)
		{
			int    maxRows    = Tools.StringToInt(Tools.ConfigValue("MaximumRows"));
			int    iter       = 0;
			int    rowsDone   = 0;
			string spr        = "";
			bureauCode        = Tools.NullToString(bureau);
			bureaCodeTokenize = Tools.NullToString(bureau);
			success           = 0;
			fail              = 0;
			maxRows           = ( maxRows < 1 ? Constants.MaxRowsPayment : maxRows );
			sql               = "";
			string desc       = Tools.TransactionTypeName(transactionType) + " (BureauCode " + bureauCode + ")";

			if ( bureauCode.Length < 1 )
				return 0;

//	Transaction types with stored procedures to return data
			else if ( transactionType == (byte)Constants.TransactionType.GetToken )
				spr  = "sp_Get_CardToToken";
			else if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty )
				spr  = "sp_Get_CardToToken";
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
				spr  = "sp_Get_TokenPayment";
			else if ( transactionType == (byte)Constants.TransactionType.CardPayment )
				spr  = "sp_Get_CardPayment";
			else if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				spr  = "sp_Get_CardPayment";
			else if ( transactionType == (byte)Constants.TransactionType.DeleteToken )
				spr  = "sp_Get_TokenToDelete";
			else if ( transactionType == (byte)Constants.TransactionType.GetCardFromToken )
				spr  = "sp_TokenEx_Detoken";
			else if ( transactionType == (byte)Constants.TransactionType.TransactionLookup )
				spr  = "sp_Get_CardPayment_Ref";
			else if ( transactionType == (byte)Constants.TransactionType.ZeroValueCheck )
				spr  = "sp_Get_PaymentZeroValue";
			else if ( transactionType == (byte)Constants.TransactionType.AccountUpdate )
				spr  = "sp_Get_AccountUpdate";

			//	Testing
			//	sql  = "select 1 as Seq,'EoD35wlB34RtDeIVG74RqEexmT8aYkNOw9T2kEH3nyu21zWwQ10Ls9Y8zXfs4pVc' as TransactionId"
			//      + " union select 2,'M4lbEu9BvQoM6PwmYsym2RrEfM58nh2VpO7vrEZleJ5JbypaP8PYn3wbIEMa1ndZ'"
			//      + " union select 3,'r39TNmEhjjveTJOY4ZavHE4jmy7WbltlgB0pOYhoV7rxlo72nBTBBWk2ctRcptiZ'"
			//      + " union select 4,'cWbkjnKdfCnJUnJWxPFqb3i6cQYOw1frg7mFp8VjvxkVh7S7Dn1T2Th1VTpIKRmc'"
			//      + " union select 5,'ZlXzGHCFCxx5lrj9lwic80hdJ4P8PCKwd04fiAi7bGhErdAI6Dqyp2sGvhgZzXDe'"
			//      + " union select 6,'GonzoTheSlodFace-WithAnInvalidTransactionIdThatWillFailEveryTime'"
			//      + " order by Seq";

//	Transaction types with SELECT statements (testing)
			else if ( transactionType == (byte)Constants.TransactionType.Reversal )
				sql  = "select 'Rev-001' as merchantReference,'1234' as token,'cj8WeFwDcTCkE74vxYR3V8nX0yv5pyf4Hp0j9oxZtFVZDyI6exUxL7gmlkoC7o2r' as transactionId,899 as amountInCents,'ZAR' as currencyCode,2 as Seq"
			        + " union select 'Rev-002','1234','vEYFbdWHvcnXTXsyYguNwO7vgDl4DeoCJmfCQAZXnk26oureLdSV88xiXXIy3raP',899,'ZAR',3"
			        + " union select 'Rev-003','1234','O1xHMzGWuk4PgBQkClQeP3l7ndwO7h3Szu6iNb7yIpw8CtTH03gZ3wgBLBk6ZcbQ',899,'ZAR',4"
			        + " union select 'Rev-005','1234','qd765s7iBnXxzdif515p7OSFGlPmcr5YhPLZlNkwtQqNtoFrWmR8BG0piRiBOyoH',899,'ZAR',1"
			        + " union select 'Rev-004','1234','cvDhXW4DMaxeoTSOd2afjDphVdBdYJ5gJTYYXc2ig2I1D6Lw2QgjVZtt5X9r8Mgq',899,'ZAR',5"
			        + " order by Seq";
			else if ( transactionType == (byte)Constants.TransactionType.Refund )
				sql  = "select 'Ref-001' as merchantReference,'' as token,'' as transactionId,388 as amountInCents,'ZAR' as currencyCode";
			else if ( transactionType == (byte)Constants.TransactionType.Transfer )
				sql  = "select 'ZA' as CountryCode,'Janet Smith' as AccountName,'111111' as SortCode,'111111' as AccountNumber,'ZAR' as CurrencyCode,997 as Amount";

//	Transaction types unknown
			else
				return 0;

			if ( rowsToProcess < 1 )
				rowsToProcess = 0;

			if ( spr.Length > 0 ) // Stored proc, not SELECT
			{
				sql = "exec " + spr + " " + Tools.DBString(bureauCode) + ",";
				if ( maxRows > 0 && rowsToProcess > 0 )
					sql = sql + Math.Min(maxRows,rowsToProcess).ToString();
				else if ( maxRows > 0 )
					sql = sql + maxRows.ToString();
				else if ( rowsToProcess > 0 )
					sql = sql + rowsToProcess.ToString();
				else
					sql = sql + Constants.MaxRowsPayment.ToString();
			}

			Tools.LogInfo("ProcessCards/15",desc + ", MaxRows=" + maxRows.ToString()+", RowsToProcess=" + rowsToProcess.ToString()+", SQL="+sql,logPriority,this);

			while ( rowsToProcess < 1 || rowsToProcess > success + fail )
				try
				{
					if ( LoadDataFromSQL(maxRows,"Payments.ProcessCards ("+desc+")") < 1 )
						break;
					Tools.CloseDB(ref dbConn);
					rowsDone = 0;
					iter++;
					foreach (Payment payment in objList)
					{
						payment.BureauCode      = bureauCode;
						payment.TransactionType = transactionType;
						if ( transactionType == (byte)Constants.TransactionType.GetToken )
							err = payment.GetToken();
						else if ( transactionType == (byte)Constants.TransactionType.ZeroValueCheck )
							err = payment.ZeroValueCheck();
						else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
							err = payment.ProcessPayment();
						else if ( transactionType == (byte)Constants.TransactionType.CardPayment )
							err = payment.ProcessPayment();
						else if ( transactionType == (byte)Constants.TransactionType.DeleteToken )
							err = payment.DeleteToken();
						else if ( transactionType == (byte)Constants.TransactionType.GetCardFromToken )
							err = payment.Detokenize();
						else if ( transactionType == (byte)Constants.TransactionType.Reversal )
							err = payment.Reversal();
						else if ( transactionType == (byte)Constants.TransactionType.Refund )
							err = payment.Refund();
						else if ( transactionType == (byte)Constants.TransactionType.TransactionLookup )
							err = payment.Lookup();
						else if ( transactionType == (byte)Constants.TransactionType.AccountUpdate )
							err = payment.AccountUpdate();
						else if ( transactionType == (byte)Constants.TransactionType.Transfer )
						//	err = payment.Transfer();
							err = 41999;
						else if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty )
						{
							payment.TokenizerCode = bureaCodeTokenize;
							err                   = payment.GetToken(); 
						}
						else if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
						{
							payment.TokenizerCode = bureaCodeTokenize;
							err                   = payment.ProcessPayment(); 
						}

						if ( err == 0 )
							success++;
						else
							fail++;
						rowsDone++;
						if ( rowsToProcess > 0 && rowsToProcess <= success + fail )
							break;
					}
					Tools.LogInfo("ProcessCards/40",desc + ", iteration " + iter.ToString() + ", " + rowsDone.ToString() + " rows done",logPriority,this);
//	In case of a runaway loop where failures are not rectified ...
					if ( fail > 99 && success == 0 )
						break;
					if ( fail > 999 )
						break;
				}
				catch (Exception ex)
				{
					fail++;
					rowsDone++;
					Tools.LogException("ProcessCards/50",desc + ", iteration " + iter.ToString() + ", " + (success+fail).ToString() + " rows done",ex,this);
				}
				finally
				{
					Tools.CloseDB(ref dbConn);
					Tools.LogInfo("ProcessCards/90",desc + " finished, success " + success.ToString() + ", fail " + fail.ToString(),logPriority,this);
				}

			return success+fail;
		}
	}
}
