//	(c) Paul Kilfoil
//	Software Development & IT Consulting
//	www.PaulKilfoil.co.za

function ValidPhone(phoneNo,country)
{
	try
	{
		phoneNo = Trim(phoneNo);

		if ( phoneNo.length < 5 )
			return false;

		if ( country == 1 ) // SA
		{
			if ( phoneNo.substring(0,1) != '0' )
				return false;
			var p = ToInteger(phoneNo,-66);
			if ( p < 100000000 || p > 999999999 ) // Valid range is 010 000 0000 to 099 999 9999
				return false;
		}
//		else
//			return ( phoneNo.substring(0,1) == '0' || phoneNo.substring(0,1) == '+' );
	}
	catch (x)
	{ }
	return true;
}

function ValidCardNumber(cardNo)
{
	try
	{
		cardNo = Trim(cardNo);
		if ( cardNo.length < 10 || cardNo.length > 20 )
			return false;
		
		var digit;
		var total = 0;
		var even  = false;

		for (var k = cardNo.length - 1; k >= 0; k--)
		{
			digit = cardNo.charAt(k);
			if ( isNaN(digit) )
				return false;
			digit = parseInt(digit, 10);
			if (even)
			{
				digit = digit * 2;
				if ( digit > 9 )
					digit = digit - 9;
			}
			total = total + digit;
			even  = ! even;
		}
		return (total % 10) == 0;
	}
	catch (x)
	{ }
	return false;
}

function ValidPIN(pin,minLength)
{
	try
	{
		pin = Trim(pin).toUpperCase();
		if ( minLength == null || minLength < 1 )
			minLength = 1;
		if ( pin.length < minLength )
			return false;
		for (var k = 0; k < pin.length; k++)
			if ( ('0123456789').indexOf(pin.charAt(k)) < 0 )
				return false;
		return true;
	}
	catch (x)
	{ }
	return false;
}

function ValidEmail(email)
{
	try
	{
		email = Trim(email);
		if ( email.length < 6 )
			return false;
		if ( email.indexOf(' ') >= 0 || email.indexOf('/') >= 0 || email.indexOf('\\') >= 0 || email.indexOf('<') >= 0 || email.indexOf('>') >= 0 || email.indexOf('(') >= 0 || email.indexOf(')') >= 0 )
			return false;
		var k = email.indexOf('@');
		if ( k < 1 )
			return false;
		var j = email.lastIndexOf('@');
		if ( k != j )
			return false;
		j = email.lastIndexOf('.');
		if ( j < k )
			return false;
		if ( email.substring(k-1,k) == '.' || email.substring(k+1,k+2) == '.' || email.substring(0,1) == '.' || email.substring(email.length-1,email.length) == '.' )
			return false;
		return true;
	}
	catch (x)
	{ }
	return false;
}

function ValidDate(dd,mm,yy,eltName)
{
// Standard date validation, assuming that all years ending in '00' are leap years (they aren't)

	var msg = eltName + " is not valid<br />";

	try
	{
		dd = ToInteger(dd);
		mm = ToInteger(mm);
		yy = ToInteger(yy);

		if (dd < 1 || dd > 31 || mm < 1 || mm > 12 || yy < 1900 || yy > 2999)
			return msg;
		else if (dd > 30 && (mm == 4 || mm == 6 || mm == 9 || mm == 11))
			return msg;
		else if (mm == 2 && dd > 29)
			return msg;
		else if (mm == 2 && dd == 29 && yy % 4 != 0)
			return msg;
		return "";
	}
	catch (x)
	{ }
	return msg;
}

function CheckElt(eltID,eltName,validationType,validationParm)
{
	try
	{
		var p = GetEltValue(eltID);

		if (validationType == 1 && p.length < validationParm)
			return eltName + " must contain at least " + validationParm.toString() + " characters<br />";

		if (validationType == 2 && p.length > validationParm)
			return eltName + " cannot be longer than " + validationParm.toString() + " characters<br />";

		if (validationType == 3 && p.length > 0 && !ValidEmail(p))
			return eltName + " is not valid<br />";

		return "";
	}
	catch (x)
	{ }
	return "There is a problem with " + eltName + "<br />";
}

function CheckRadio(groupName,eltName)
{
	try
	{
		var r = document.getElementsByName(groupName);
		var k = 0;
		while ( k < r.length )
			if ( r[k++].checked )
				return "";
	}
	catch (x)
	{ }
	return "You must choose one of the " + eltName + " options<br />";
}

function Validate(ctlID,lblID,eltType,eltDesc,eltMode,eltParm,eltBool)
{
	if ( eltDesc == null )
		return "";

	var err = "";
	var elt;
	var eltValue;

	try
	{
		if ( eltBool == null || eltBool )
		{
			eltBool = true;
			elt     = GetElt(ctlID);
			if ( elt.style.display == 'none' ) // Hidden
				eltBool = false;
		}
		if ( ! eltBool )
			return "";
		eltValue = Trim(elt.value);
	}
	catch (w)
	{
		eltValue = "";
	}

	try
	{
		if ( eltType == 1 ) // Text
		{
			if ( eltMode == 1 && eltValue.length != eltParm )
				err = eltDesc;
			else if ( eltMode == 2 && eltValue.length < eltParm )
				err = eltDesc;
			else if ( eltMode == 3 && eltValue.length > eltParm )
				err = eltDesc;
			else if ( eltMode == 4 && eltValue.length > 0 && eltValue.length != eltParm )
				err = eltDesc;
			else if ( eltMode == 5 && eltValue.length > 0 && eltValue == eltParm )
				err = eltDesc;
		}

		else if ( eltType == 2 ) // Radio
		{
			elt   = document.getElementsByName(ctlID);
			err   = eltDesc;
			var k = 0;
			while ( k < elt.length )
				if ( elt[k++].checked )
				{
					err = "";
					break;
				}
		}

		else if ( eltType == 8 ) // CheckBox
			try
			{
				if ( eltMode == 1 && elt.checked )
					err = eltDesc;
				else if ( eltMode == 2 && ! elt.checked )
					err = eltDesc;
			}
			catch (t)
			{
				err = "";
			}

		else if ( eltType == 3 ) // List
		{
			if ( eltMode == 73 && eltValue == eltParm )
				err = eltDesc;
			else if ( eltMode != 73 && eltValue < eltParm )
				err = eltDesc;
		}

		else if ( eltType == 4 ) // Date
			try
			{
				var h   = ( Trim(ctlID[0]) + "/" + Trim(ctlID[1]) + "/" + Trim(ctlID[2]) ).toUpperCase();
				eltDesc = eltDesc + " (" + h + ")";

				if ( eltParm == 3 && ( h == "DD/MM/YYYY" || h == "//" ) ) // Allow DD/MM/YYYY or blank
					err = "";
				else if ( ValidDate(ctlID[0],ctlID[1],ctlID[2],"").length > 0 )
					err = eltDesc;
				else if ( eltMode > 0 )
				{
					var nw = new Date(); // Today
					var dd = ToInteger(ctlID[0]);
					var mm = ToInteger(ctlID[1]);
					var yy = ToInteger(ctlID[2]);
					if ( eltMode == 1 ) // Can't be later than today
					{
						if ( nw.getFullYear() < yy )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1)  < mm )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1) == mm && nw.getDate() < dd )
							err = eltDesc;
					}
					else if ( eltMode == 2 ) // Can't be earlier than today
					{
						if ( nw.getFullYear() > yy )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1)  > mm )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1) == mm && nw.getDate() > dd )
							err = eltDesc;
					}
				}
			}
			catch (x)
			{ }

		else if ( eltType == 5 ) // EMail
		{
			if ( ! ValidEmail(eltValue) )
				err = eltDesc;
		}

		else if ( eltType == 9 ) // Card number
		{
			if ( ! ValidCardNumber(eltValue) )
				err = eltDesc;
		}

		else if ( eltType == 7 ) // Phone number
		{
			if ( eltMode == 1 && eltValue.length == 0 )
			{ }
			else if ( ! ValidPhone(eltValue,eltParm) ) // eltParm is country
				err = eltDesc;
		}

		else if ( eltType == 6 ) // Numeric
		{
			var numVal = ToInteger(eltValue,-66);
			var numLen = Trim(eltValue).length;

			if ( numVal < 0 && eltValue.length > 0 )
				err = eltDesc;
//	Value checks
			else if ( eltMode ==  1 && ( numVal < 0 || numVal > eltParm ) ) // 0 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode ==  2 && ( numVal < 1 || numVal > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode ==  3 && numVal < eltParm ) // x >= eltParm
				err = eltDesc;
			else if ( eltMode ==  4 && ( numVal < 0 || numVal > eltParm ) && numLen > 0 ) // 0 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
			else if ( eltMode ==  5 && ( numVal < 1 || numVal > eltParm ) && numLen > 0 ) // 1 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
//	Length checks
			else if ( eltMode ==  6 && numLen != eltParm ) // Must be exactly this length
				err = eltDesc;
			else if ( eltMode ==  7 && ( numLen < 1 || numLen > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode ==  8 && numLen < eltParm ) // x <= eltParm
				err = eltDesc;
			else if ( eltMode == 44 && ( numLen < 3 || numLen > 6 ) ) // Card CVV
				err = eltDesc;
//	Credit card checks
			else if ( eltMode >= 71 && eltMode <= 74 )
			{
				if ( eltValue.length != numLen )
					err = eltDesc;
				else if ( eltMode == 71 && ( numLen < 13 || numLen > 19 ) ) // Visa
					err = eltDesc;
				else if ( eltMode == 72 && numLen != 16 ) // MC
					err = eltDesc;
				else if ( eltMode == 73 && numLen != 15 ) // AmEx
					err = eltDesc;
				else if ( eltMode == 74 && ( numLen < 14 || numLen > 16 ) ) // Diners
					err = eltDesc;
			}
			else if ( eltMode == 66 ) // SA Id
			{
				if ( eltParm == 3 && numLen == 0 ) // Allow blank
				{ }
				else if ( numLen != 13 )
					err = eltDesc;
				else if ( ValidDate(eltValue.substring(4,6),eltValue.substring(2,4),'19'+eltValue.substring(0,2),'').length > 0 )
					err = eltDesc;
			}
			else if ( eltMode > 99 && ( numVal < eltMode || numVal > eltParm ) ) // So a number between (eg) 1900 and 2017
				err = eltDesc;
		}

		else if ( eltType == 81 ) // Show error
			err = eltDesc;

		else if ( eltType == 91 ) // Hide error
			err = "";

		SetErrorLabel(lblID,37,err);

		try
		{
			if ( err.length > 0 )
				elt.style.borderColor = colorErr;
			else
				elt.style.borderColor = colorOK;
		}
		catch (w)
		{ }
	}
	catch (z)
	{
		alert("Validate('"+ctlID.toString()+"') : " + z.message);
	}
	return err + ( err.length == 0 ? '' : '<br />' );
}
