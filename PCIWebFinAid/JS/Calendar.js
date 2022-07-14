function getPointer(obj)
{
	if (typeof(obj) == "object")
		return obj;
	return document.getElementById(obj);
}

function positionInfo(obj)
{
	var pElt = obj;

	this.getEltLeft = getEltLeft;
	function getEltLeft()
	{
		var x = 0;
		var p = getPointer(pElt);
		while (p != null)
		{
			x+= p.offsetLeft;
			p = p.offsetParent;
		}
		return parseInt(x);
	}

	this.getEltWidth = getEltWidth;
	function getEltWidth()
	{
		var p = getPointer(pElt);
		return parseInt(p.offsetWidth);
	}

	this.getEltRight = getEltRight;
	function getEltRight()
	{
		return getEltLeft(pElt) + getEltWidth(pElt);
	}

	this.getEltTop = getEltTop;
	function getEltTop()
	{
		var y = 0;
		var p = getPointer(pElt);
		while (p != null)
		{
			y+= p.offsetTop;
			p = p.offsetParent;
		}
		return parseInt(y);
	}

	this.getEltHeight = getEltHeight;
	function getEltHeight()
	{
		var p = getPointer(pElt);
		return parseInt(p.offsetHeight);
	}

	this.getEltBottom = getEltBottom;
	function getEltBottom()
	{
		return getEltTop(pElt) + getEltHeight(pElt);
	}
}

function CalendarControl()
{
	var calendarId = 'ctlCalendar';
	var curYear = 0;
	var curMonth = 0;
	var curDay = 0;
	var selYear = 0;
	var selMonth = 0;
	var selDay = 0;

	var months = ['January','February','March','April','May','June','July','August','September','October','November','December'];
	var dateField = null;

	function getProperty(p_property)
	{
		try
		{
			var p = getPointer(calendarId);
			return p[p_property];
		}
		catch (e) {}

		return null;
	}

	function setEltProperty(p_property, p_value, p_elmId)
	{
		var p = getPointer(p_elmId);

		if ((p != null) && (p.style != null))
		{
			p = p.style;
			p[p_property] = p_value;
		}
	}

	function setProperty(p_property, p_value)
	{
		setEltProperty(p_property, p_value, calendarId);
	}

	function getDaysInMonth(year, month)
	{
		return [31,((!(year % 4 ) && ( (year % 100 ) || !( year % 400 ) ))?29:28),31,30,31,30,31,31,30,31,30,31][month-1];
	}

	function getDayOfWeek(year, month, day)
	{
		var date = new Date(year,month-1,day);
		return date.getDay();
	}

	this.setDate = setDate;
	function setDate(year, month, day)
	{
		if (dateField)
		{
			if (year < 60)
				year = "20" + year;
			else if (year < 100)
				year = "19" + year;
			if (month < 10)
				month = "0" + month;
			if (day < 10)
				day = "0" + day;
//			var dateString = day+"/"+month+"/"+year;
//			dateField.value = dateString;
			dateField.value = year + "-" + month + "-" + day;
			hide();
		}
	}

	this.changeMonth = changeMonth;
	function changeMonth(change)
	{
		curMonth += change;
		curDay = 0;
		if (curMonth > 12)
		{
			curMonth = 1;
			curYear++;
		}
		else if (curMonth < 1)
		{
			curMonth = 12;
			curYear--;
		}
		calendar = document.getElementById(calendarId);
		calendar.innerHTML = calendarDrawTable();
	}

	this.changeYear = changeYear;
	function changeYear(change)
	{
		curYear += change;
		curDay = 0;
		calendar = document.getElementById(calendarId);
		calendar.innerHTML = calendarDrawTable();
	}

	function getCurrentYear()
	{
		var year = new Date().getYear();
		if (year < 1900)
			year += 1900;
		return year;
	}

	function getCurrentMonth()
	{
		return new Date().getMonth() + 1;
	}

	function getCurrentDay()
	{
		return new Date().getDate();
	}

	function calendarDrawTable()
	{
		var dayOfMonth = 1;
		var validDay = 0;
		var startDayOfWeek = getDayOfWeek(curYear, curMonth, dayOfMonth);
		var daysInMonth = getDaysInMonth(curYear, curMonth);
		var css_class = null; //CSS class for each day
		var table = "<table cellspacing='0' cellpadding='0' border='0'><tr class='header'>";

		table = table + "<td colspan='2' class='previous'><a href='JavaScript:changeCalendarMonth(-1)'>&lt;</a> <a href='JavaScript:changeCalendarYear(-1)'>&laquo;</a></td>";
		table = table + "<td colspan='3' class='title'>" + months[curMonth-1] + "<br />" + curYear + "</td>";
		table = table + "<td colspan='2' class='next'><a href='JavaScript:changeCalendarYear(1)'>&raquo;</a> <a href='JavaScript:changeCalendarMonth(1)'>&gt;</a></td></tr>";
		table = table + "<tr><th>S</th><th>M</th><th>T</th><th>W</th><th>T</th><th>F</th><th>S</th></tr>";

		for (var week=0; week < 6; week++)
		{
			table = table + "<tr>";
			for (var dayOfWeek=0; dayOfWeek < 7; dayOfWeek++)
			{
				if (week == 0 && startDayOfWeek == dayOfWeek)
					validDay = 1;
				else if (validDay == 1 && dayOfMonth > daysInMonth)
					validDay = 0;

				if (validDay)
				{
					if (dayOfMonth == selDay && curYear == selYear && curMonth == selMonth)
						css_class = 'current';
					else if (dayOfWeek == 0 || dayOfWeek == 6)
						css_class = 'weekend';
					else
						css_class = 'weekday';

					table = table + "<td><a class='"+css_class+"' href='JavaScript:setCalendarDate("+curYear+","+curMonth+","+dayOfMonth+")'>"+dayOfMonth+"</a></td>";
					dayOfMonth++;
				}
				else
					table = table + "<td class='empty'>&nbsp;</td>";
			}
			table = table + "</tr>";
		}
		table = table + "<tr class='header'><th colspan='7' style='padding:3px'><a href='JavaScript:hideCalendar()'>Close</a></td></tr></table>";

		return table;
	}

	this.show = show;
	function show(field)
	{
	//	If the calendar is visible and associated with this field do not do anything
		if (dateField == field)
			return;

		window.onbeforeunload = null;
		dateField = field;

		if (dateField)
		{
			try
			{
				var dateStr = new String(dateField.value);
				var dateParts = dateStr.split("/");

				selDay = parseInt(dateParts[0],10);
				selMonth = parseInt(dateParts[1],10);
				selYear = parseInt(dateParts[2],10);
			}
			catch(e) {}
		}
		if (!(selYear && selMonth && selDay))
		{
			selMonth = getCurrentMonth();
			selDay = getCurrentDay();
			selYear = getCurrentYear();
		}
		curMonth = selMonth;
		curDay = selDay;
		curYear = selYear;

		if (document.getElementById)
		{
			calendar = document.getElementById(calendarId);
			calendar.innerHTML = calendarDrawTable(curYear, curMonth);

			setEltProperty('display', 'block', 'calendarIFrame');
			setProperty('display', 'block');

			var fieldPos = new positionInfo(dateField);
			var calendarPos = new positionInfo(calendarId);
			var x = fieldPos.getEltLeft();
			var y = fieldPos.getEltBottom();
			x=x+105;
			y=y-21;
			setProperty('left', x + "px");
			setProperty('top', y + "px");
			setEltProperty('left', x + "px", 'calendarIFrame');
			setEltProperty('top', y + "px", 'calendarIFrame');
			setEltProperty('width', calendarPos.getEltWidth() + "px", 'calendarIFrame');
			setEltProperty('height', calendarPos.getEltHeight() + "px", 'calendarIFrame');
		}
	}

	this.hide = hide;
	function hide()
	{
		setProperty('display', 'none');
		setEltProperty('display', 'none', 'calendarIFrame');
		if (dateField)
		{
			dateField = null;
		//	window.onbeforeunload = ConfirmExit;
		}
	}
}

var ctlCalendar = new CalendarControl();

function showCalendar(txtDate)
{
	ctlCalendar.show(txtDate);
}

function hideCalendar()
{
//	window.onbeforeunload = ConfirmExit;
	ctlCalendar.hide();
}

function setCalendarDate(year,month,day)
{
	ctlCalendar.setDate(year,month,day);
}

function changeCalendarYear(change)
{
	ctlCalendar.changeYear(change);
}

function changeCalendarMonth(change)
{
	ctlCalendar.changeMonth(change);
}

document.write("<iframe id='calendarIFrame' frameBorder='0' scrolling='no'></iframe>");
document.write("<div id='ctlCalendar'></div>");
