using System;

namespace PCIBusiness
{
	public class Account : BaseData
	{
		private int     reqId;
		private string  accountNumber;
		private string  accountType;
		private string  cushion;
		private string  lookAheadNextChange;
		private string  accruedCash;
		private string  availableFunds;
		private string  buyingPower;
		private string  equityWithLoanValue;
		private string  excessLiquidity;
		private string  fullAvailableFunds;
		private string  fullExcessLiquidity;
		private string  fullInitMarginReq;
		private string  fullMaintMarginReq;
		private string  grossPositionValue;
		private string  initMarginReq;
		private string  lookAheadAvailableFunds;
		private string  lookAheadExcessLiquidity;
		private string  lookAheadInitMarginReq;
		private string  lookAheadMaintMarginReq;
		private string  maintMarginReq;
		private string  netLiquidation;
		private string  totalCashValue;

		public  int     RequestId
		{
			get { return reqId; }
			set { reqId = value; }
		}
		public  string  AccountNumber
		{
			get { return Tools.NullToString(accountNumber); }
			set { accountNumber = value.Trim(); }
		}
		public  string  AccountType
		{
			get { return Tools.NullToString(accountType); }
			set { accountType = value.Trim(); }
		}
		public  string  Cushion
		{
			get { return Tools.NullToString(cushion); }
			set { cushion = value.Trim(); }
		}
		public  string  LookAheadNextChange
		{
			get { return Tools.NullToString(lookAheadNextChange); }
			set { lookAheadNextChange = value.Trim(); }
		}
		public  string  AccruedCash
		{
			get { return Tools.NullToString(accruedCash,"0"); }
			set { accruedCash = value.Trim(); }
		}
		public  string  AvailableFunds
		{
			get { return Tools.NullToString(availableFunds,"0"); }
			set { availableFunds = value.Trim(); }
		}
		public  string  BuyingPower
		{
			get { return Tools.NullToString(buyingPower,"0"); }
			set { buyingPower = value.Trim(); }
		}
		public  string  EquityWithLoanValue
		{
			get { return Tools.NullToString(equityWithLoanValue,"0"); }
			set { equityWithLoanValue = value.Trim(); }
		}
		public  string  ExcessLiquidity
		{
			get { return Tools.NullToString(excessLiquidity,"0"); }
			set { excessLiquidity = value.Trim(); }
		}
		public  string  FullAvailableFunds
		{
			get { return Tools.NullToString(fullAvailableFunds,"0"); }
			set { fullAvailableFunds = value.Trim(); }
		}
		public  string  FullExcessLiquidity
		{
			get { return Tools.NullToString(fullExcessLiquidity,"0"); }
			set { fullExcessLiquidity = value.Trim(); }
		}
		public  string  FullInitMarginReq
		{
			get { return Tools.NullToString(fullInitMarginReq,"0"); }
			set { fullInitMarginReq = value.Trim(); }
		}
		public  string  FullMaintMarginReq
		{
			get { return Tools.NullToString(fullMaintMarginReq,"0"); }
			set { fullMaintMarginReq = value.Trim(); }
		}
		public  string  GrossPositionValue
		{
			get { return Tools.NullToString(grossPositionValue,"0"); }
			set { grossPositionValue = value.Trim(); }
		}
		public  string  InitMarginReq
		{
			get { return Tools.NullToString(initMarginReq,"0"); }
			set { initMarginReq = value.Trim(); }
		}
		public  string  LookAheadAvailableFunds
		{
			get { return Tools.NullToString(lookAheadAvailableFunds,"0"); }
			set { lookAheadAvailableFunds = value.Trim(); }
		}
		public  string  LookAheadExcessLiquidity
		{
			get { return Tools.NullToString(lookAheadExcessLiquidity,"0"); }
			set { lookAheadExcessLiquidity = value.Trim(); }
		}
		public  string  LookAheadInitMarginReq
		{
			get { return Tools.NullToString(lookAheadInitMarginReq,"0"); }
			set { lookAheadInitMarginReq = value.Trim(); }
		}
		public  string  LookAheadMaintMarginReq
		{
			get { return Tools.NullToString(lookAheadMaintMarginReq,"0"); }
			set { lookAheadMaintMarginReq = value.Trim(); }
		}
		public  string  MaintMarginReq
		{
			get { return Tools.NullToString(maintMarginReq,"0"); }
			set { maintMarginReq = value.Trim(); }
		}
		public  string  NetLiquidation
		{
			get { return Tools.NullToString(netLiquidation,"0"); }
			set { netLiquidation = value.Trim(); }
		}
		public  string  TotalCashValue
		{
			get { return Tools.NullToString(totalCashValue,"0"); }
			set { totalCashValue = value.Trim(); }
		}

		public override void LoadData(DBConn dbConn)
		{
			dbConn.SourceInfo = "Account.LoadData";
		}

		public Account() : base()
		{
			reqId         = 0;
			accountNumber = "";
		}		
	}
}
