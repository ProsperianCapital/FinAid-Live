//	AJAX Toolkit

//	(c)Paul Kilfoil
//		Software Development & IT Consulting
//		+27 84 438 5400 (phone)
//		PaulKilfoil..[at]..gmail.com
//		www.PaulKilfoil.co.za

//	Do not copy without the permission of the author

var req;
var reqURL;
var reqType;
var xmlDOM;
var busyMode; // Set this to 1 in code to stop AJAX execution ...

function XMLValue(tag)
{
	try
	{
		var p = Trim(xmlDOM.getElementsByTagName(tag)[0].firstChild.nodeValue);
		return p.replace(/\[br \/\]/g,"<br />");
	//	Replace all [br /] with <br />
	}
	catch (x)
	{ }
	return "";
}

function AJAXCallBack()
{
	if (req.readyState == 4) // 4 means "complete"
	{
		if (req.status == 200) // 200 means "OK"
		{
			var urlData = req.responseText;

			try // MS IE
			{
				xmlDOM = new ActiveXObject("Microsoft.XMLDOM");
				xmlDOM.loadXML(urlData);
			}
			catch (x) // non-IE
			{
				var tmpDOM = new DOMParser();
				xmlDOM = tmpDOM.parseFromString(urlData,"text/xml");
				tmpDOM = null;
			}

			try
			{
				if ( reqType == 7 )
					AJAXPayments();
				else if ( reqType == 17 )
					AJAXTeam();
//				else if ( reqType == 19 )
//					AJAXTeamDel();
				else
					AJAXFinalize(reqType);
			}
			catch (y)
			{ }
		}
	}
}

function AJAXInitialize(typeCode,urlParms)
{
	try
	{
		reqType = typeCode;
		reqURL  = "AJAXProcess.aspx?Type=" + typeCode.toString() + "&" + urlParms;

		try // MS IE
		{
			req = new ActiveXObject("Microsoft.XMLHTTP");
		}
		catch (x) // non-IE
		{
			req = new XMLHttpRequest();
		}
		req.onreadystatechange = AJAXCallBack;
		req.open("GET", reqURL, true);
		req.send(null);
	}
	catch (y)
	{ }
}