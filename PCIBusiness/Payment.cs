using System;

namespace PCIBusiness
{
	public class Payment : BaseData
	{
//		private int      paymentCode;
//		private int      paymentAuditCode;
//	...Ecentric fields, but not needed after all
//		private string   authorizationCode;         // This is payment provider's auth code (may not be there)
//		private string   orderNumber;               // This is a Prosperian reference (maybe not unique)

		private string   merchantReference;         // This is Prosperian's unique reference for THIS transaction
		private string   merchantReferenceOriginal; // This is Prosperian's reference for the original token transaction 
		private string   transactionID;             // This is payment provider's transaction reference (may not be there)
		private string   countryCode;
		private string   firstName;
		private string   lastName;
		private string   address1;
		private string   address2;
//		private string   address3;
		private string   postalCode;
		private string   provinceCode;
		private string   regionalId;
		private string   email;
		private string   phoneCell;
//		private string   ipAddress;
		private int      paymentAmount;
//		private byte     paymentStatus;
		private string   paymentDescription;
		private string   currencyCode;

		private string   ccNumber;
		private string   ccType;
		private string   ccExpiryMonth;
		private string   ccExpiryYear;
		private string   ccName;
		private string   ccCVV;
		private string   ccPIN;
		private string   ccToken;

//	Contract/mandate for payment
		private DateTime mandateDateTime;
		private string   mandateIPAddress;
		private string   mandateBrowser;
		private string   contractCode;

//	Payment Provider (eg. Peach)
		private string   bureauCode;
		private string   providerAccount;
		private string   providerKey;
		private string   providerKeyPublic;
		private string   providerUserID;
		private string   providerPassword;
		private string   providerURL;
		private string   providerProfileID;

//	Token Provider (eg. TokenEx)
		private string   tokenizerCode;
		private string   tokenizerKey;
		private string   tokenizerID;
		private string   tokenizerURL;

		private int      processMode;
		private byte     transactionType;
		private string   webForm;

//	WorldPay fields
		private string   sessionId;
		private string   machineCookie;

//	Stripe fields
		private string   customerID;
		private string   paymentMethodID;

		private Transaction transaction;


//		Token Provider stuff
		public string    TokenizerCode
		{
			get { return  Tools.NullToString(tokenizerCode); }
			set { tokenizerCode = Tools.NullToString(value); }
		}
		public string    TokenizerKey
		{
			get { return  Tools.NullToString(tokenizerKey); }
			set { tokenizerKey = Tools.NullToString(value); }
		}
		public string    TokenizerID
		{
			get { return  Tools.NullToString(tokenizerID); }
			set { tokenizerID = Tools.NullToString(value); }
		}
//		public string    TokenizerURL
//		{
//			get { return  Tools.NullToString(tokenizerURL); }
//			set { tokenizerURL = Tools.NullToString(value); }
//		}

		public DateTime  MandateDateTime
		{
			get
			{
				return     mandateDateTime;

//	This code was in case the mandate date/time was returned as string
//				try
//				{
//					DateTime x = System.Convert.ToDateTime(mandateDateTime);
//					if ( x < new DateTime(1950,01,01) || x > System.DateTime.Now.AddDays(1) )
//						return Constants.DateNull;
//					return x;
//				}
//				catch
//				{ }
//				return Constants.DateNull;
			}
		}
		public string    MandateIPAddress
		{
			get { return  Tools.NullToString(mandateIPAddress); }
			set { mandateIPAddress = value.Trim(); }
		}
		public string    MandateBrowser
		{
			get { return  Tools.NullToString(mandateBrowser); }
			set { mandateBrowser = value.Trim(); }
		}
		public string    ContractCode
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(contractCode) )
					contractCode = merchantReference;
				return Tools.NullToString(contractCode);
			}
			set { contractCode = value.Trim(); }
		}

//		Stripe stuff
		public string    CustomerID
		{
			get { return  Tools.NullToString(customerID); }
		}
		public string    PaymentMethodID
		{
			get { return  Tools.NullToString(paymentMethodID); }
		}

//		Payment Provider stuff
		public string    BureauCode
		{
			get { return  Tools.NullToString(bureauCode); }
			set { bureauCode = value.Trim(); }
		}

		public string    ProviderAccount
		{
			get 
			{
				if ( Tools.NullToString(providerAccount).Length > 0 )
					return providerAccount;

				else if ( Tools.SystemIsLive() )
					return "";

//	Testing
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "2237055";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.T24) )
					return "567654452";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Ikajo) )
					return "6861-finaidhk";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.MyGate) )
					return "MY014473";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
					return "UMID_858445001";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
//					return "2744639";
//					return "testmid";
//					return "absa_test_merchant";
//					return "thutomoloi4";
					return "000000002744639";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
					return "000000002744639";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "mu.prosperian.rtr";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
					return "100000001099570";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
				//	return "PROSPERIANTEST";
					return "PROSPERIANSGPECOM";
				return "";
			}
			set { providerAccount = value.Trim(); }
		}
		public string    ProviderKey
		{
			set { providerKey = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerKey).Length > 0 )
					return providerKey;

				else if ( Tools.SystemIsLive() )
					return "";

//	Testing
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "{A580B3C7-3EF3-47F1-9B90-4047CE0EC54C}";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate) )
					return "27ededae-4ba3-486a-a243-8da1e4c1a067";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
					return "REVqzPb4PTiD4n7Fo3e1p1VyQUbvmy5YZuhxhUpqL0EcUTGWHPchIUd8m3LeixLf";
//					return "yZgqflutOnJ3nSN7s9ylGMAZDNpmllGzuPMHPVBwDOW8riDT3qu8Uivbg7xlWbeK";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Peach) )
					return "OGFjN2E0Yzc3MmI3N2RkZjAxNzJiN2VkMDFmODA2YTF8akE0aEVaOG5ZQQ==";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
					return "IcJSjbVloKPQsS5PJrCdGOz8W/pLOBjzO4QVqKG4Ai8=";
//					return "6o/jJqk5K+abVz057+G2X4H5XnkEKqEK0gz53MB0fjQ=";
//					return "Zh24hLoQTpDj1n2g5ahwfDGiLaQryCQHi+DGEl0dcP8=";
//					return "0123k20MBbIB2t012345678993gHCIZsQKFpf7dR0hY=";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
					return "IcJSjbVloKPQsS5PJrCdGOz8W/pLOBjzO4QVqKG4Ai8=";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "daea1771-d849-4fa4-a648-230a54186964"; // Public key
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA) ) // Public key
					return "pk_test_51It78gGmZVKtO2iKXD0LEFRDvEs1Wkld93qRjifDLyWRoOgxXwGDJZzs9i902shBJqEk8v3XYg1WLLdButIK0QfU00xtFyxDQf";
//					return "pk_test_51It78gGmZVKtO2iKc4eB6JveDn9HZAWR7F9cbiISEcYHGquyNoqb1YNnSQuzlJlR8maNlTUmaH0pBHHw4tZAOUBc00KZH2PeKW";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentCloud) )
					return "859v6V4N8H67pvAk";
				return "";
			}
		}
		public string    ProviderKeyPublic
		{
			set { providerKeyPublic = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerKeyPublic).Length > 0 )
					return providerKeyPublic;
				else if ( Tools.SystemIsLive() )
					return "";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "daea1771-d849-4fa4-a648-230a54186964";
//				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
//					return "Blah";

				return "";
			}
		}
		public string    ProviderProfileID
		{
			set { providerProfileID = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerProfileID).Length > 0 )
					return providerProfileID;
//				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
//					return "601591F8-04DA-4E9D-94E1-A5D7649C0EB7";
				return "";
			}
		}

		public string    ProviderUserID
		{
			set { providerUserID = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerUserID).Length > 0 )
					return providerUserID;

				else if ( Tools.NullToString(providerAccount).Length > 0 )
					return providerAccount;

				else if ( Tools.SystemIsLive() )
					return "";

//	Testing
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "200208";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
					return "4311038889209736";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Peach) )
					return "8ac7a4ca72b781310172b7ed08860114"; // Payments
				//	return "8ac7a4c772b77ddf0172b7ed1cd206df"; // 3d
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
					return "31c799cd-18da-47c3-be95-f93bd90748e0";
//					return "3C857FA4-ED86-4A08-A119-24170A74C760";
//					return "baa4366b-6a39-4a7f-99a2-442a91200a46";
//					return "410c3964-6c30-4e71-a60e-057cff71a547";
//					return "01234567-0123-0123-0123-012345678912";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource_Moto) )
					return "31c799cd-18da-47c3-be95-f93bd90748e0";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "800060";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentCloud) )
					return "7a5bb4SW9GY";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
				//	return "2LHRK1HBEPDYVP9OKG8S";
					return "AI5QP9YBY291AGF5AD7I";

				return "";
			}
		}
		public string    ProviderPassword
		{
			set { providerPassword = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerPassword).Length > 0 )
					return providerPassword;
				else if ( Tools.SystemIsLive() )
					return "";

//	Testing
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayU) )
					return "g1Kzk8GY";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PaymentsOS) )
					return "3790d1d5-4847-43e6-a29a-f22180cc9fda"; // Private/secret key
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.Stripe_USA) ) // Secret key
					return "sk_test_51It78gGmZVKtO2iKBZF7DA5JisJzRqvibQdXSfBj9eQh4f5UDvgCShZIjznOWCxu8MtcJG5acVkDcd8K184gIegx001uXlHI5g";
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.FNB) )
					return "sbyq0CUAvUSPMifwRH0f68fByQ5ZgSjyEpbeKg77o1Cuh9BD30ucakuXtpCCUMJN"; // Instance key
				else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
					return "st0nE#481";

				return "";
			}
		}
		public string    ProviderURL
		{
			set { providerURL = value.Trim(); }
			get
			{
				if ( Tools.NullToString(providerURL).Length < 1 )
					providerURL = Tools.BureauURL(bureauCode);
				return providerURL;
			}
		}

		public string    ProviderHost
		{
		//	set { providerHost = value.Trim(); }
			get
			{
				string host = Tools.NullToString(ProviderURL);
				if ( host.Length < 3 )
					return "";
				if ( host.ToUpper().StartsWith("HTTPS://") )
					host = host.Substring(8);
				else if ( host.ToUpper().StartsWith("HTTP://") )
					host = host.Substring(7);
				int k = host.IndexOf("/");
				if ( k > 0 )
					return host.Substring(0,k);
				return host;
			}
		}

		public string    TokenizerURL
		{
			set { tokenizerURL = value.Trim(); }
			get
			{
				if ( Tools.NullToString(tokenizerURL).Length > 0 )
					return tokenizerURL;

				else if ( Tools.SystemIsLive() )
				{
					if ( tokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
						return "https://api.tokenex.com";
				}

				else
				{
					if ( tokenizerCode == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
						return "https://test-api.tokenex.com";
				}

				return "";
			}
		}

//		public string    Address(byte line)
//		{
//			if ( address == null || line < 1 || ( line > address.Length && line < 255 ) )
//				return "";
//			if ( line == 255 ) // Last non-blank address
//			{
//				for ( int k = address.Length ; k > 0 ; k-- )
//					if ( address[k-1].Length > 0 )
//						return address[k-1];
//				return "";
//			}
//			return address[line-1];
////			while ( line < address.Length )
////			{
////				if ( address[line].Length > 0 )
////					return address[line];
////				line++;
////			}
////			return "";
//		}
		public string    WebForm
		{
			get { return  Tools.NullToString(webForm); }
		}

//		Customer stuff
		public string    FirstName
		{
			get { return  Tools.NullToString(firstName); }
			set { firstName = value.Trim(); }
		}
		public string    LastName
		{
			get { return  Tools.NullToString(lastName); }
			set { lastName = value.Trim(); }
		}
//		public string    Name
//		{
//			get
//			{
//				if ( FirstName.Length < 1 )
//					return LastName;
//				if ( LastName.Length < 1 )
//					return FirstName;
//				return FirstName + " " + LastName;

//	//			if ( FirstName.Length > 0 && LastName.Length > 0 )
//	//				return FirstName + " " + LastName;
//	//			if ( FirstName.Length > 0 )
//	//				return FirstName;
//	//			return LastName;
//			}
//		}
		public string    EMail
		{
			get { return  Tools.NullToString(email); }
			set { email = value.Trim(); }
		}
		public string    PhoneCell
		{
			get { return  Tools.NullToString(phoneCell); }
			set { phoneCell = value.Trim(); }
		}
		public string    RegionalId
		{
			get { return  Tools.NullToString(regionalId); }
			set { regionalId = value.Trim(); }
		}
		public string    Address1(byte mode=0)
		{
			if ( mode == 65 && string.IsNullOrWhiteSpace(address1) )
				return "Care Assist";
			return Tools.NullToString(address1);
		}
		public string    Address2(byte mode=0)
		{
			if ( mode == 65 && string.IsNullOrWhiteSpace(address2) )
				return "Cape Town";
			return Tools.NullToString(address2);
		}
		public string    Address3(byte mode=0)
		{
//			if ( mode == 65 && string.IsNullOrWhiteSpace(address3) )
//				return "Western Cape";
//			return Tools.NullToString(address3);

			if ( mode == 65 )
				return "Western Cape";
			return "";
		}
		public string    PostalCode(byte mode=0)
		{
			if ( mode == 65 && string.IsNullOrWhiteSpace(postalCode) )
				return "7530";
			return Tools.NullToString(postalCode);
		}
		public string    CountryCode(byte mode=0)
		{
			if ( mode == 65 && string.IsNullOrWhiteSpace(countryCode) )
				return "ZA";
			return Tools.NullToString(countryCode);
		}
		public string    ProvinceCode
		{
			get { return  Tools.NullToString(provinceCode); }
		}
		public string    State // Province, but only if USA
		{
			get
			{
				string p = Tools.NullToString(provinceCode);
				string y = Tools.NullToString(countryCode).ToUpper();
				if ( p.Length > 0 && y.Length >= 2 && y.StartsWith("US") )
					return p;
				return "";
			}
		}

//		payment stuff
//		public string    OrderNumber
//		{
//			get { return  Tools.NullToString(orderNumber); }
//		}
		public string    MerchantReference
		{
			get
			{
				if ( string.IsNullOrWhiteSpace(merchantReference) )
					merchantReference = contractCode;
				return Tools.NullToString(merchantReference);
			}
			set { merchantReference = value.Trim(); }
		}
		public string    MerchantReferenceOriginal
		{
			get { return  Tools.NullToString(merchantReferenceOriginal); }
		}
		public string    SessionID
		{
//			Used by WorldPay
			get
			{
				if ( Tools.NullToString(sessionId).Length < 1 )
					sessionId = (Guid.NewGuid()).ToString();
				return Tools.NullToString(sessionId);
			}
		}
		public string    Cookie
		{
//			Used by WorldPay
			get { return Tools.NullToString(machineCookie); }
			set { machineCookie = value.Trim(); }
		}
		public string    TransactionID
		{
			get
			{
				if ( Tools.NullToString(transactionID).Length < 1 )
					transactionID = (Guid.NewGuid()).ToString();
				return Tools.NullToString(transactionID);
			}
			set { transactionID = value.Trim(); }
		}
//		public string    AuthorizationCode
//		{
//			get { return  Tools.NullToString(authorizationCode); }
//		}
		public string    CurrencyCode
		{
			get { return  Tools.NullToString(currencyCode); }
			set { currencyCode = value.Trim().ToUpper(); }
		}
		public string    PaymentDescription
		{
			set { paymentDescription = value.Trim(); }
			get
			{
				paymentDescription = Tools.NullToString(paymentDescription);
				if ( paymentDescription.Length > 0 )
					return paymentDescription;
				paymentDescription = Tools.ConfigValue("AppDescription");
				if ( paymentDescription.Length > 0 )
					return paymentDescription;
				paymentDescription = SystemDetails.Owner;
				if ( paymentDescription.Length > 0 )
					return paymentDescription;
				return "Prosperian Capital International";
			}	
		}
		public string    PaymentDescriptionLeft(short maxLength)
		{
			string h = Tools.NullToString(paymentDescription);
			if ( h.Length < 1 )
				h = "CareAssist";
			if ( maxLength <= 0 || maxLength >= h.Length )
				return h;
			return h.Substring(0,maxLength);
		}
		public  int      PaymentAmount
		{
//	Cents
			get { return  paymentAmount; }
			set
			{
				paymentAmount = value;
				if ( paymentAmount < 1 )
					paymentAmount = 0;
			}
		}
		public  string   PaymentAmountDecimal
		{
//	Rands
			get
			{
				if ( paymentAmount < 1 )
					return "0.00";
				string amt = paymentAmount.ToString();
				if ( amt.Length == 1 )
					return "0.0" + amt;
				if ( amt.Length == 2 )
					return "0." + amt;
				return amt.Substring(0,amt.Length-2) + "." + amt.Substring(amt.Length-2);
			}
		}
//		public  byte     PaymentStatus
//		{
//			get { return  paymentStatus; }
//		}

//		Card stuff
		public  string   CardToken
		{
			get { return  Tools.NullToString(ccToken); }
			set { ccToken = value.Trim(); }
		}
		public  string   CardPIN
		{
			get { return  Tools.NullToString(ccPIN); }
		}
		public  string   CardType
		{
			get { return  Tools.NullToString(ccType); }
			set { ccType = value.Trim(); }
		}
		public  string   CardNumber
		{
			get { return  Tools.NullToString(ccNumber); }
			set { ccNumber = value.Trim(); }
		}
		public  byte     CardExpiryMonth
		{
			get
			{
				try
				{
					byte x = Convert.ToByte(ccExpiryMonth);
					if ( x > 0 && x < 13 )
						return x;
				}
				catch
				{ }
				return 12;
			}
		}
		public  int      CardExpiryYear
		{
			get
			{
				try
				{
					int x = Convert.ToInt32(ccExpiryYear);
					if ( x > 1900 && x < 9999 )
						return x;
				}
				catch
				{ }
				return System.DateTime.Now.Year+1;
			}
		}
		public  string   CardExpiryDD // Always the last day of the month
		{
			get
			{
				byte mm = CardExpiryMonth;
				if ( mm == 1 || mm == 3 || mm == 5 || mm == 7 || mm == 8 || mm == 10 || mm == 12 )
					return "31";
				if ( mm == 4 || mm == 6 || mm == 9 || mm == 11 )
					return "30";
				if ( CardExpiryYear == 2100 || CardExpiryYear == 2200 || CardExpiryYear == 2300 ) // Not leap years
					return "28";
				if ( ( CardExpiryYear % 4 ) == 0 )
					return "29";
				return "28";
			}
		}
		public  string   CardExpiryMM // Pad with zeroes, eg. 07
		{
			get
			{
				ccExpiryMonth = Tools.NullToString(ccExpiryMonth);
				if ( ccExpiryMonth.Length == 2 )
					return ccExpiryMonth;
				if ( ccExpiryMonth.Length == 1 )
					return "0" + ccExpiryMonth;
				return "12";
			//	return Tools.NullToString(ccExpiryMonth).PadLeft(2,'0');
			}
			set 
			{
				ccExpiryMonth = value.Trim();
				if ( Tools.StringToInt(ccExpiryMonth) < 1 || Tools.StringToInt(ccExpiryMonth) > 12 || ccExpiryMonth.Length > 2 )
					ccExpiryMonth = "";
			}
		}
		public  string   CardExpiryYY
		{
			get
			{
				ccExpiryYear = Tools.NullToString(ccExpiryYear);
				if ( ccExpiryYear.Length == 4 )
					return ccExpiryYear.Substring(2,2);
				if ( ccExpiryYear.Length == 2 )
					return ccExpiryYear;
				return (System.DateTime.Now.Year+1).ToString().Substring(2,2);
			}
		}
		public  string   CardExpiryYYYY
		{
			get
			{
				ccExpiryYear = Tools.NullToString(ccExpiryYear);
				if ( ccExpiryYear.Length == 4 )
					return ccExpiryYear;
				if ( ccExpiryYear.Length == 2 )
					return "20" + ccExpiryYear;
				return (System.DateTime.Now.Year+1).ToString();
			}
			set 
			{
				ccExpiryYear = value.Trim();
				if ( Tools.StringToInt(ccExpiryYear) < System.DateTime.Now.Year || Tools.StringToInt(ccExpiryYear) > 2999 || ccExpiryYear.Length != 4 )
					ccExpiryYear = "";
			}
		}

		public long CardExpiryMilliSeconds
		{
			get
			{
				try
				{
//	Version 3, no need to bother with the hours, minutes or seconds
					DateTimeOffset expDate = new DateTime ( System.Convert.ToInt32(CardExpiryYYYY),
					                                        System.Convert.ToInt32(CardExpiryMM),
					                                        System.Convert.ToInt32(CardExpiryDD) );
					return expDate.ToUnixTimeMilliseconds();

//	Version 2
//					DateTime jan1970 = new DateTime ( 1970, 01, 01, 00, 00, 00 , System.DateTimeKind.Utc );
//					DateTime expDate = new DateTime ( System.Convert.ToInt32(CardExpiryYYYY),
//					                                  System.Convert.ToInt32(CardExpiryMM),
//					                                  System.Convert.ToInt32(CardExpiryDD), 23, 59, 59, 59 );
//					TimeSpan diff    = expDate - jan1970;
//					return System.Convert.ToInt64(diff.TotalMilliseconds);

//	Version 1
//					DateTime jan1970 = new DateTime ( 1970, 01, 01, 00, 00, 00 );
//					DateTime expDate = new DateTime ( System.Convert.ToInt32(CardExpiryYYYY),
//					                                  System.Convert.ToInt32(CardExpiryMM),
//					                                  System.Convert.ToInt32(CardExpiryDD), 23, 59, 59, 59 );
//					TimeSpan diff    = expDate - jan1970;
//					return System.Convert.ToInt64(diff.TotalMilliseconds);
				}
				catch
				{ }
				DateTimeOffset h = System.DateTime.Now.AddYears(1);
				return h.ToUnixTimeMilliseconds();
			}
		}

		public  string   CardName
		{
			get { return  Tools.NullToString(ccName); }
			set { ccName = value.Trim(); }
		}
		public  string   CardNameSplit(byte firstLast)
		{
			string nm = Tools.NullToString(ccName);
			if ( nm.Length < 1 )
				return "";
			int k = nm.IndexOf(" ");
			if ( k < 1 )
				return nm;
			if ( firstLast <= 1 )
				return nm.Substring(0,k);
			return nm.Substring(k).Trim();
		}
		public  string   CardCVV
		{
			get { return  Tools.NullToString(ccCVV); }
			set { ccCVV = value.Trim(); }
		}
		public  byte     TransactionType
		{
			get { return  transactionType; }
			set { transactionType = value; }
		}

		public int Detokenize()
		{
			int retProc = 87020;
			int retSQL  = 87020;
			sql         = "";
			Tools.LogInfo("Detokenize/10","Token=" + CardToken,10,this);

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;
			if ( CardToken.Length < 1 )
				return 87030;

			retProc = transaction.Detokenize(this);

			if ( retProc == 0 )
			{
				sql = "exec sp_TokenEx_UpdateVault @PaymentBureauCode = "  + Tools.DBString(bureauCode) 
			                                  + ",@PaymentBureauToken = " + Tools.DBString(CardToken)
			                                  + ",@ContractCode = "       + Tools.DBString(MerchantReference)
			                                  + ",@CardNumber = "         + Tools.DBString(transaction.CardNumber);
				Tools.LogInfo("Detokenize/20","SQL=" + sql,218,this);
				retSQL = ExecuteSQLUpdate();
			}
			else
				Tools.LogInfo("Detokenize/30","retProc=" + retProc.ToString() + " | data="
				            + CardToken + " | " + transaction.PaymentReference + " | " + transaction.CardNumber,218,this);

			Tools.LogInfo("Detokenize/90","retProc=" + retProc.ToString()+", retSQL=" + retSQL.ToString(),40,this);
			return retProc;
		}

		public int DeleteToken()
		{
			int retProc = 59020;
			int retSQL  = 59020;
			sql         = "";
			Tools.LogInfo("DeleteToken/10","Token=" + CardToken,10,this);

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			retProc = transaction.DeleteToken(this);

			if ( processMode == (int)Constants.ProcessMode.FullUpdate ||
			     processMode == (int)Constants.ProcessMode.DeleteToken )
			{
				sql = "exec sp_Upd_TokenToDelete @PaymentBureauCode = " + Tools.DBString(BureauCode)
			                                + ",@Token = "             + Tools.DBString(CardToken)
			                                + ",@StatusName = "        + Tools.DBString(transaction.ResultStatus)
			                                + ",@StatusDesc = "        + Tools.DBString(transaction.ResultMessage);
				Tools.LogInfo("DeleteToken/20","SQL=" + sql,10,this);
				retSQL = ExecuteSQLUpdate();
			}
			Tools.LogInfo("DeleteToken/90","retProc=" + retProc.ToString()+", retSQL=" + retSQL.ToString(),40,this);
			return retProc;
		}

		public int Reversal()
		{
			int retProc = 64020;
			int retSQL  = 64020;
			sql         = "";

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			retProc = transaction.Reversal(this);
//	To Do
//			sql     = "exec sp_Upd_Reversal @MerchantReference = "     + Tools.DBString(merchantReference) // nvarchar(20),
//				                         + ",@PaymentBureauCode = "     + Tools.DBString(bureauCode)        // char(3),
//			                            + ",@CardToken = "             + Tools.DBString(transaction.PaymentToken)
//			                            + ",@TransactionId = "         + Tools.DBString(transaction.PaymentReference)
//			                            + ",@ReversalId = "            + Tools.DBString(transaction.OtherReference)
//			                            + ",@BureauSubmissionSoap = "  + Tools.DBString(transaction.XMLSent,3)
//			                            + ",@BureauResultSoap = "      + Tools.DBString(transaction.XMLResult,3)
//			                            + ",@TransactionStatusCode = " + Tools.DBString(transaction.ResultCode)
//		                               + ",@ReversalStatusCode = '" + ( retProc == 0 ? "007'" : "001'" );
//			retSQL = ExecuteSQLUpdate();
//			Tools.LogInfo("Reversal/90","retProc=" + retProc.ToString()+", retSQL=" + retSQL.ToString()+", SQL=" + sql,240,this);

			return retProc;
		}

		public int Lookup()
		{
			int retProc = 64120;
			int retSQL  = 64120;
			sql         = "";

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			retProc = transaction.Lookup(this);
			sql     = "exec sp_Upd_CardPayment_Ref @TransactionID = "           + Tools.DBString(TransactionID)
			                                   + ",@TransactionStatusCode = "   + Tools.DBString(transaction.ResultCode);
//			                                   + ",@TransactionStatusReason = " + Tools.DBString(transaction.ResultMessage);
			Tools.LogInfo("Lookup/20","SQL=" + sql,20,this);
			retSQL = ExecuteSQLUpdate();

			return retProc;
		}

		public int Refund()
		{
			int retProc = 64020;
			int retSQL  = 64020;
			sql         = "";

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			retProc = transaction.Refund(this);

//	To Do
//			sql     = "exec sp_Upd_Refund @MerchantReference = "     + Tools.DBString(merchantReference) // nvarchar(20),
//				                       + ",@PaymentBureauCode = "     + Tools.DBString(bureauCode)        // char(3),
//			                          + ",@CardToken = "             + Tools.DBString(transaction.PaymentToken)
//			                          + ",@TransactionId = "         + Tools.DBString(transaction.PaymentReference)
//			                          + ",@RefundId = "              + Tools.DBString(transaction.OtherReference)
//			                          + ",@BureauSubmissionSoap = "  + Tools.DBString(transaction.XMLSent,3)
//			                          + ",@BureauResultSoap = "      + Tools.DBString(transaction.XMLResult,3)
//			                          + ",@TransactionStatusCode = " + Tools.DBString(transaction.ResultCode)
//		                             + ",@RefundStatusCode = '"     + ( retProc == 0 ? "007'" : "001'" );
//			retSQL = ExecuteSQLUpdate();
//			Tools.LogInfo("Refund/90","retProc=" + retProc.ToString()+", retSQL=" + retSQL.ToString()+", SQL=" + sql,240,this);

			return retProc;
		}

		public int GetToken()
		{
			int retProc = 64020;
			int retSQL  = 64020;
			sql         = "";
			Tools.LogInfo("GetToken/10","Merchant Ref=" + merchantReference,10,this);

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			if ( transactionType == (byte)Constants.TransactionType.GetTokenThirdParty )
				retProc = transaction.GetToken3rdParty(this);
			else
				retProc = transaction.GetToken(this);

			if ( processMode == (int)Constants.ProcessMode.FullUpdate ||
			     processMode == (int)Constants.ProcessMode.UpdateToken )
			{
				sql = "exec sp_Upd_CardTokenVault @MerchantReference = "           + Tools.DBString(merchantReference) // nvarchar(20),
				                              + ",@PaymentBureauCode = "           + Tools.DBString(bureauCode)        // char(3),
			                                 + ",@PaymentBureauToken = "          + Tools.DBString(transaction.PaymentToken)
			                                 + ",@PaymentMethodId = "             + Tools.DBString(transaction.PaymentMethodId)
			                                 + ",@CustomerId = "                  + Tools.DBString(transaction.CustomerId)
			                                 + ",@BureauSubmissionSoap = "        + Tools.DBString(transaction.XMLSent,3)
			                                 + ",@BureauResultSoap = "            + Tools.DBString(transaction.XMLResult,3)
			                                 + ",@TransactionStatusCode = "       + Tools.DBString(transaction.ResultCode)
		                                    + ",@CardTokenisationStatusCode = '" + ( retProc == 0 ? "007'" : "001'" );
				Tools.LogInfo("GetToken/20","SQL=" + sql,20,this);
				retSQL = ExecuteSQLUpdate();
			}
			Tools.LogInfo("GetToken/90","retProc=" + retProc.ToString()+", retSQL=" + retSQL.ToString(),40,this);
			return retProc;
		}

		public int ProcessPayment()
		{
			int retProc   = 37020;
			int retSQL    = 37020;
			returnMessage = "Invalid payment provider";
			Tools.LogInfo("ProcessPayment/10","Merchant Ref=" + merchantReference,10,this);

			if ( transaction == null || transaction.BureauCode != bureauCode )
				transaction = Tools.CreateTransaction(bureauCode);
			if ( transaction == null )
				return retProc;

			if ( transactionType == (byte)Constants.TransactionType.ManualPayment ) // Manual card payment
				Tools.LogInfo("ProcessPayment/20","Manual card payment",20,this);

			else if ( processMode == (int)Constants.ProcessMode.FullUpdate         ||
			          processMode == (int)Constants.ProcessMode.UpdatePaymentStep1 ||
			          processMode == (int)Constants.ProcessMode.UpdatePaymentStep1AndStep2 )
			{
				sql = "exec sp_Upd_CardPayment @MerchantReference     = " + Tools.DBString(merchantReference)
			                              + ",@TransactionID         = " + Tools.DBString(transaction.PaymentReference)
			                              + ",@TransactionStatusCode = '77'";
				Tools.LogInfo("ProcessPayment/30","SQL 1=" + sql,20,this);
				retSQL = ExecuteSQLUpdate();
				Tools.LogInfo("ProcessPayment/40","SQL 1 complete",20,this);
			}
			else
				Tools.LogInfo("ProcessPayment/50","SQL 1 skipped",20,this);

			if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
				retProc    = transaction.TokenPayment(this);
			else if ( transactionType == (byte)Constants.TransactionType.CardPaymentThirdParty )
				retProc    = transaction.CardPayment3rdParty(this);
			else if ( transactionType == (byte)Constants.TransactionType.Test )
				retProc    = transaction.CardTest(this);
			else
				retProc    = transaction.CardPayment(this);

			webForm       = "";
			returnMessage = transaction.ResultMessage;

			if ( transactionType == (byte)Constants.TransactionType.ManualPayment ) // Manual card payment
			{
				Tools.LogInfo("ProcessPayment/60","Manual card payment, retProc=" + retProc.ToString() + ", acsUrl=" + transaction.ThreeDacsUrl,199,this);
				if ( transaction.ThreeDRequired )
					if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.eNETS) )
						webForm = "<html><body onload='document.forms[\"frm3D\"].submit()'>"
						        + "<form name='frm3D' method='POST'   action='" + transaction.ThreeDacsUrl + "'>"
						        + "<input type='hidden' name='PaReq'   value='" + transaction.ThreeDpaReq + "' />"
						        + "<input type='hidden' name='TermUrl' value='" + transaction.ThreeDtermUrl + "' />"
						        + "<input type='hidden' name='MD'      value='" + transaction.ThreeDmd + "' />"
						        + "</form></body></html>";
					else if ( bureauCode == Tools.BureauCode(Constants.PaymentProvider.PayGate) )
						webForm = "<html><body onload='document.forms[\"frm3D\"].submit()'>"
						        + "<form name='frm3D' method='POST' action='" + transaction.ThreeDacsUrl + "'>"
						        + transaction.ThreeDKeyValuePairs
						        + "</form></body></html>";
			}
			else if ( transactionType == (byte)Constants.TransactionType.Test && transaction.WebForm.Length > 0 )
			{
				webForm = transaction.WebForm;
			}
			else if ( processMode == (int)Constants.ProcessMode.FullUpdate         ||
			          processMode == (int)Constants.ProcessMode.UpdatePaymentStep2 ||
			          processMode == (int)Constants.ProcessMode.UpdatePaymentStep1AndStep2 )
			{
				sql = "exec sp_Upd_CardPayment @MerchantReference = "     + Tools.DBString(merchantReference)
			                              + ",@TransactionStatusCode = " + Tools.DBString(transaction.ResultCode)
			                              + ",@TransactionID = "         + Tools.DBString(transaction.PaymentReference);
				Tools.LogInfo("ProcessPayment/70","SQL 2=" + sql,20,this);
				retSQL = ExecuteSQLUpdate();
				Tools.LogInfo("ProcessPayment/80","SQL 2 complete",20,this);
			}
			else
				Tools.LogInfo("ProcessPayment/90","SQL 2 skipped",20,this);

			return retProc;
		}

		public override void LoadData(DBConn dbConn)
		{
			if ( dbConn.ColStatus("PaymentBureauToken") == Constants.DBColumnStatus.ColumnOK )
			{
				ccToken           = dbConn.ColString("PaymentBureauToken");
				providerUserID    = dbConn.ColString("TxID");
				providerKey       = dbConn.ColString("TxKey");
				providerURL       = dbConn.ColString("TxURL");
				contractCode      = dbConn.ColString("ContractCode");
				merchantReference = contractCode;
				return;
			}

		//	Payment Provider
			providerKey       = dbConn.ColString("SafeKey");
			providerURL       = dbConn.ColString("url"                 ,0,0);
			providerAccount   = dbConn.ColString("MerchantAccount"     ,0,0);
			providerProfileID = dbConn.ColString("MerchantProfileId"   ,0,0);
			providerUserID    = dbConn.ColString("MerchantUserId"      ,0,0);
			providerPassword  = dbConn.ColString("MerchantUserPassword",0,0);
			providerKeyPublic = dbConn.ColString("MerchantKeyPublic"   ,0,0);

		//	Lookup
			if ( dbConn.ColStatus("TransactionId") == Constants.DBColumnStatus.ColumnOK &&
			     dbConn.ColStatus("CardNumber")    != Constants.DBColumnStatus.ColumnOK &&
			     dbConn.ColStatus("Token")         != Constants.DBColumnStatus.ColumnOK )
			{
				transactionID = dbConn.ColString("TransactionId");
				return;
			}

		//	Customer
			if ( dbConn.ColStatus("lastName") == Constants.DBColumnStatus.ColumnOK )
			{
				firstName     = dbConn.ColUniCode("firstName"  ,0,0);
				lastName      = dbConn.ColUniCode("lastName"   ,0,0);
				email         = dbConn.ColString ("email"      ,0,0);
				phoneCell     = dbConn.ColString ("mobile"     ,0,0);
				regionalId    = dbConn.ColString ("regionalId" ,0,0);
				address1      = dbConn.ColUniCode("address1"   ,0,0);
				address2      = dbConn.ColUniCode("city"       ,0,0);
			//	address3      = dbConn.ColUniCode("blah"       ,0,0);
				postalCode    = dbConn.ColString ("zip_code"   ,0,0);
				provinceCode  = dbConn.ColString ("state"      ,0,0);
				countryCode   = dbConn.ColString ("countryCode",0,0);
			//	ipAddress     = dbConn.ColString ("IPAddress"  ,0,0);
			}


		//	Payment
			merchantReference         = dbConn.ColString("merchantReference"        ,0,0);
			merchantReferenceOriginal = dbConn.ColString("merchantReferenceOriginal",0,0); // Only really for Ikajo, don't log error
			paymentAmount             = dbConn.ColLong  ("amountInCents"            ,0,0);
			currencyCode              = dbConn.ColString("currencyCode"             ,0,0);
			paymentDescription        = dbConn.ColString("description"              ,0,0);

		//	Card/token/transaction details, not always present, don't log errors
			ccName           = dbConn.ColUniCode("NameOnCard"     ,0,0);
			ccNumber         = dbConn.ColString ("CardNumber"     ,0,0);
			ccExpiryMonth    = dbConn.ColString ("CardExpiryMonth",0,0);
			ccExpiryYear     = dbConn.ColString ("CardExpiryYear" ,0,0);
			ccType           = dbConn.ColString ("CardType"       ,0,0);
			ccCVV            = dbConn.ColString ("CVV"            ,0,0);
			ccToken          = dbConn.ColString ("Token"          ,0,0);
			ccPIN            = dbConn.ColString ("PIN"            ,0,0);
			transactionID    = dbConn.ColString ("TransactionId"  ,0,0);
		//	Used by Stripe (bureauCode 028)
			customerID       = dbConn.ColString ("CustomerId"     ,0,0);
			paymentMethodID  = dbConn.ColString ("PaymentMethodId",0,0);

		//	Contract/customer mandate
			mandateDateTime  = dbConn.ColDate   ("ContractDate"   ,0,0);
			mandateIPAddress = dbConn.ColString ("IPAddres"       ,0,0);
			mandateBrowser   = dbConn.ColString ("Browser"        ,0,0);
			contractCode     = dbConn.ColString ("ContractCode"   ,0,0);

		//	Token Provider (if empty, then it is the same as the payment provider)
			if ( dbConn.ColStatus("TxKey") == Constants.DBColumnStatus.ColumnOK )
			{
				tokenizerID   = dbConn.ColString("TxID");
				tokenizerKey  = dbConn.ColString("TxKey");
				tokenizerURL  = dbConn.ColString("TxURL");
			}
			if ( dbConn.ColStatus("TxToken") == Constants.DBColumnStatus.ColumnOK )
				ccToken       = dbConn.ColString("TxToken");

//	Testing
//			countryCode      = "ZA";
//			address1         = "231 Lagoon's Edge";
//			address2         = "Port Alfred";
//			email            = "Joe@DontPhoneMe.net";
//			phoneCell        = "084 666 8888";
//			mandateDateTime  = "2019/12/31 23:59:59";
//			mandateIPAddress = "10.0.231.45";
//			mandateBrowser   = "FireFox 89.0.1";
		}

		public override void CleanUp()
		{
			transaction     = null;
			transactionType = 0;
		}

		public Payment(string bureau) : base()
		{
			processMode     = Tools.StringToInt(Tools.ConfigValue("ProcessMode"));
			bureauCode      = Tools.NullToString(bureau);
			webForm         = "";
			transactionType = 0;
		}

		public Payment() : base()
		{
			processMode     = Tools.StringToInt(Tools.ConfigValue("ProcessMode"));
			bureauCode      = "";
			webForm         = "";
			transactionType = 0;
		}
	}
}
