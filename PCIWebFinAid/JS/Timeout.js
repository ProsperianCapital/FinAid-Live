//	JavaScript session timeout functionality

//	(c)Paul Kilfoil
//		Software Development & IT Consulting
//		+27 84 438 5400 (phone)
//		PaulKilfoil..[at]..gmail.com
//		www.PaulKilfoil.co.za

//	Do not copy without the permission of the author

try
{
	if ( userMode > 0 )
		CheckSessionTime();
}
catch (x)
{ }

function CheckSessionTime()
{
//	GetElt("lblTime").innerText = sessionTime;

	var dt1;
	var dt2;

	sessionTime = sessionTime - 1;

	while (true)
	{
		if (sessionTime <= 0)
		{
			alert("We're sorry, but you took too long. Your session has EXPIRED.");
			location.href = "Identify.aspx?ErrNo=9187362";
		//	location.href = "Login.aspx?ErrNo=9187362";
			return;
		}
		dt1 = new Date();

		if (sessionTime <= 1)
			alert("Your session will expire in ONE MINUTE. Please finish what you are doing on this page NOW.");
		else if (sessionTime >= 4 && sessionTime <= 5)
			alert("Your session will expire in FIVE MINUTES.");

		dt2         = new Date();
		sessionTime = sessionTime - ( ( dt2 - dt1 ) / 60000 );

		if (sessionTime > 0)
			break;
	}
	window.defaultStatus = sessionTime.toString() + " minutes(s) remaining";
	window.setTimeout("CheckSessionTime()",60000);
}

function ShowSessionTime()
{
	try
	{
		if ( userMode < 1 )
			alert("You are not logged on, so your session will continue indefinitely.");
		else if ( sessionTime > 0 && Math.round(sessionTime) == 0 )
			alert("You only have a few seconds remaining before your session expires.");
		else if ( Math.round(sessionTime) > 0 )
			alert("You have " + Math.round(sessionTime) + " minute(s) remaining before your session expires.");
		else
			alert("Your session has expired.");
	}
	catch (x)
	{ }
}