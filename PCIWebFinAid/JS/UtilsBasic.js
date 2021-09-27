//	(c) Paul Kilfoil
//	Software Development & IT Consulting
//	www.PaulKilfoil.co.za

function CursorStyle(eltID,style)
{
	try
	{
		var h = 'auto'; // 1
		if      ( style == 2 ) h = 'help';
		else if ( style == 3 ) h = 'pointer';
		else if ( style == 4 ) h = 'progress';
		else if ( style == 5 ) h = 'not-allowed';
		else if ( style == 6 ) h = 'none';
		GetElt(eltID).style.cursor = h;
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

function ToInteger(theValue,defaultRet)
{
	try
	{
		var p;

		if ( defaultRet == null )
			defaultRet = 0;

		for (p = 0; p < theValue.length; p++)
			if ( ('0123456789.,').indexOf(theValue.charAt(p)) < 0 )
				return defaultRet;

		p = parseInt(theValue,10);
		if ( ! isNaN(p) )
			return p;
	}
	catch (x)
	{ }
	return defaultRet;
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
	try
	{
		var p = GetElt(eltID);
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

function ShowElt(eltID,show,background,retType)
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
		if ( retType == 59 )
			return;
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

function GetListValueInt(eltID)
{
	try
	{
		var h = GetListValue(eltID);
		return ToInteger(h);
	}
	catch (x)
	{ }
	return 0;
}

function GetListValue(eltID)
{
	try
	{
		var p = GetElt(eltID);
		return Trim(p.options[p.selectedIndex].value);
	}
	catch (x)
	{ }
	return '';
}

function ListAdd(eltID,code,text,selected)
{
	var p   = GetElt(eltID);
	var h   = document.createElement('option');
	h.value = code.toString();
	h.text  = text.toString();
	if ( selected )
		h.selected = true;

	try
	{
		p.add(h,null); // non-IE
	}
	catch (x)
	{
		p.add(h);      // MS IE
	}
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
