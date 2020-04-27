//	Misc JavaScript functions

//	(c)Paul Kilfoil
//		Software Development & IT Consulting
//		+27 84 438 5400 (phone)
//		PaulKilfoil..[at]..gmail.com
//		www.PaulKilfoil.co.za

//	Do not copy without the permission of the author

function Help(show)
{
	if ( GetElt('divHelp') == null )
		alert("Help is not available on this page yet ... try again later");
	else
		ShowElt('divHelp',(show>0),0);
}

function CursorStyle(eltID,style)
{
	try
	{
		var p = GetElt(eltID);
		var h = 'auto'; // 1
		if      ( style == 2 ) h = 'help';
		else if ( style == 3 ) h = 'pointer';
		else if ( style == 4 ) h = 'progress';
		else if ( style == 5 ) h = 'not-allowed';
		else if ( style == 6 ) h = 'none';
		p.style.cursor = h;
	}
	catch (x)
	{ }
}

function ToDate(dd,mm,yy)
{
	try
	{
		var h = ValidateDate(dd,mm,yy,'');
		if ( h.length == 0 )
			return new Date(yy,mm-1,dd);
	}
	catch (x)
	{ }
	return null;
}

function ToInteger(theValue,defaultReturn)
{
	try
	{
		if ( defaultReturn == null )
			defaultReturn = 0;
		var p = parseInt(theValue,10); // Force base 10
		if ( ! isNaN(p) ) // It's an INT
			return p;
	}
	catch (x)
	{ }
	return defaultReturn;
}

function GetElt(eltID)
{
	try
	{
		var p;
		if ( typeof(eltID) == 'object' )
			p = eltID;
		else
			p = document.getElementById(eltID);
		return p;
	}
	catch (x)
	{ }
	return null;
}

function GetEltValue(eltID)
{
	var p = GetElt(eltID);
	try
	{
		var h = Trim(p.value.toString());
		return h;
	}
	catch (x)
	{ }
	try
	{
		var k = Trim(p.innerHTML);
		return k;
	}
	catch (x)
	{ }
	return "";
}

function GetEltValueInt(eltID)
{
	var p = GetElt(eltID);
	try
	{
		var h = Trim(p.innerHTML);
		if ( h.length == 0 )
			h = p.value;
		var k = ToInteger(h);
		if ( k > 0 )
			return k;
	}
	catch (x)
	{ }
	return 0;
}

function SetEltValue(eltID,value)
{
	var p = GetElt(eltID);
	try
	{
		p.value = value;
	}
	catch (x)
	{ }
	try
	{
		p.innerHTML = value;
	}
	catch (x)
	{ }
}

function DisableElt(eltID,disable)
{
	try
	{
		var p = GetElt(eltID);
		p.disabled = disable;
		CursorStyle(p,(disable?5:1));
		if ( disable && p.title.length < 1 )
			p.title = 'Disabled';
		else if ( ! disable && p.title == 'Disabled' )
			p.title = '';
	}
	catch (x)
	{ }
}

function EltVisible(eltID)
{
	try
	{
		return GetElt(eltID).style.display.length==0;
	}
	catch (x)
	{ }
	return false;
}

function DisableForm(frmID,disable,excludeID)
{
	var k;
	var elts = GetElt(frmID).elements;
	for ( k=0 ; k < elts.length ; k++ )
	{
		if ( elts[k].id.substring(0,1) == '_' )
			continue;
		if ( excludeID == null ||
		     elts[k].id.substring(0,excludeID.length).toUpperCase() != excludeID.toUpperCase() )
			elts[k].disabled = disable;
	}
}

function Trim(theValue)
{
	return theValue.replace(/^\s+/g, '').replace(/\s+$/g, '');
}

function ValidPhone(phoneNo,country)
{
	try
	{
//		phoneNo = Trim(phoneNo).replace(/ /g, "");
		phoneNo = Trim(phoneNo);

		if ( phoneNo.length < 8 )
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
	}
	catch (x)
	{ }
	return true;
}

function ShowElt(eltID,show,background)
{
	try
	{
		var p = GetElt(eltID);
		if (show)
		{
			p.style.visibility = "visible";
			p.style.display    = "";
			if ( background > 0 )
				document.body.className = 'greyBackground';
		}
		else
		{
			p.style.visibility = "hidden";
			p.style.display    = "none";
			if ( background > 0 )
				document.body.className = '';
		}
		return 0;
	}
	catch (x)
	{ }
	return 73;
}

function ShowBackground(show)
{
	try
	{
		if ( show > 0 )
			document.body.className = '';
		else
			document.body.className = 'greyBackground';
	}
	catch (x)
	{ }
}


function SetListValue(eltID,listValue)
{
	try
	{
		var p = GetElt(eltID);
		var k;

		for (k=0; k < p.options.length; k++)
			if ( p[k].value == listValue )
			{
				p.selectedIndex = k;
				return;
			}
		p.selectedIndex = 0;
	}
	catch (x)
	{ }
}

function GetListValue(eltID)
{
	try
	{
		var p = GetElt(eltID);
		var h = p.options[p.selectedIndex].value;
		return ToInteger(h);
	}
	catch (x)
	{ }
	return 0;
}

function ListAdd(eltID,code,text)
{
	var p   = GetElt(eltID);
	var h   = document.createElement('option');
	h.value = code.toString();
	h.text  = text.toString();

	try
	{
		p.add(h,null); // non-IE
	}
	catch (x)
	{
		p.add(h);      // MS IE
	}
}

function ValidateDate(dd,mm,yy,eltName)
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
		var rad = document.getElementsByName(groupName);
		var k   = 0;
		while ( true && k < rad.length )
			if ( rad[k++].checked )
				return "";
	}
	catch (x)
	{ }
	return "You must choose one of the " + eltName + " options<br />";
}

function Validate(ctlID,lblID,eltType,eltDesc,eltMode,eltParm,eltBool)
{
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
		{
			if ( eltMode == 1 && elt.checked )
				err = eltDesc;
			else if ( eltMode == 2 && ! elt.checked )
				err = eltDesc;
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
				else if ( ValidateDate(ctlID[0],ctlID[1],ctlID[2],"").length > 0 )
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
			else if ( eltMode == 1 && ( numVal < 0 || numVal > eltParm ) ) // 0 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 2 && ( numVal < 1 || numVal > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 3 && numVal < eltParm ) // x >= eltParm
				err = eltDesc;
			else if ( eltMode == 4 && ( numVal < 0 || numVal > eltParm ) && numLen > 0 ) // 0 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
			else if ( eltMode == 5 && ( numVal < 1 || numVal > eltParm ) && numLen > 0 ) // 1 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
//	Length checks
			else if ( eltMode == 6 && numLen != eltParm ) // Must be exactly this length
				err = eltDesc;
			else if ( eltMode == 7 && ( numLen < 1 || numLen > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 8 && numLen < eltParm ) // x <= eltParm
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
				else if ( ValidateDate(eltValue.substring(4,6),eltValue.substring(2,4),'19'+eltValue.substring(0,2),'').length > 0 )
					err = eltDesc;
			}
			else if ( eltMode > 99 && ( numVal < eltMode || numVal > eltParm ) ) // So a number between (eg) 1900 and 2017
				err = eltDesc;
		}

		else if ( eltType == 81 ) // Show error
			err = eltDesc;

		else if ( eltType == 91 ) // Hide error
			err = "";

//		SetErrorLabel(lblID,err.length,err);
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
		alert(z);
	}
	return err + ( err.length == 0 ? '' : '<br />' );
}

function SetErrorLabel(lblID,onOrOff,value,hint)
{
	try
	{
		var lbl = GetElt(lblID);
		if ( onOrOff == 0 )
			lbl.className = '';
		else
			lbl.className = 'Error';
		if ( value != null )
			SetEltValue(lbl,value);
		if ( hint != null )
			lbl.title = hint;
	}
	catch (x)
	{ }
}

function ShowPopup(eltID,info,event,endLR,eltObj)
{
	try
	{
		if ( info.length > 0 )
		{
			var q = GetElt(eltID);
			q.style.visibility = "visible";
			q.style.display = '';
			q.innerHTML = info;
			if ( event != null )
			{
				q.style.left  = "auto";
				q.style.right = "auto";
				q.style.top   = event.clientY.toString() + "px";
				if ( endLR == 'R' )
					q.style.right = (window.innerWidth-event.clientX-10).toString() + "px";
				else
					q.style.left  = (event.clientX+10).toString() + "px";
			}
			if ( eltObj != null )
			{
				var ctl = eltObj.getBoundingClientRect();
				var scr = document.body.getBoundingClientRect();
				q.style.top  = ( ctl.top  - scr.top  + 15 ).toString()  + "px";
				q.style.left = ( ctl.left - scr.left + 15 + (ctl.right-ctl.left)/2 ).toString() + "px";
			}
		}
		else
			ShowElt(eltID,false);
	}
	catch (x)
	{ }
}

function ScreenCheck(a,funcToEval)
{
	if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4)))
		funcToEval();
}