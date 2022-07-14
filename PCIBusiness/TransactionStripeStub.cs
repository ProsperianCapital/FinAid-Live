using System;

namespace PCIBusiness
{
	public class TransactionStripe : Transaction
	{
		public  bool Successful
		{
			get { return false; }
		}

		public override int GetToken(Payment payment)
		{
			return 99010;
		}

		public override int TokenPayment(Payment payment)
		{
			return 99020;
		}

		public override int CardPayment(Payment payment)
		{
			return 99030;
		}

		public TransactionStripe() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.Stripe);
			xmlResult = null;
		}
	}
}
