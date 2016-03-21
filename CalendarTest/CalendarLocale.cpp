/*
Copyright (c) 2007 Cyrus Daboo. All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
CalendarLocale.cpp

Author:
Description:	<describe the CalendarLocale class here>
*/

#include "CalendarLocale.h"
#include <tchar.h>

//#if defined(__MULBERRY) || defined(__MULBERRY_CONFIGURE)
//#include "CXStringResources.h"
//#else
//#include "CStringResources.h"
//#endif

using namespace iCalendar;

bool CalendarLocale::s24HourTime = false;
bool CalendarLocale::sDDMMDate = false;

// 0..6 - Sunday - Saturday
const string& CalendarLocale::GetDay(int32_t day, EStringLength strl)
{
	const char* cLongDays[] = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
	const char* cShortDays[] = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
	const char* cAbbrevDays[] = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
	const char** cDays = NULL;

	switch (strl)
	{
	case eLong:
		cDays = cLongDays;
		break;
	case eShort:
		cDays = cShortDays;
		break;
	case eAbbreviated:
		cDays = cAbbrevDays;
		break;
	}
	return cDays[day];
}

// 1..12 - January - December
const string& CalendarLocale::GetMonth(int32_t month, EStringLength strl)
{
	const char* cLongMonths[] = { "", "January::Long", "February::Long", "March::Long", "April::Long", "May::Long", "June::Long", "July::Long", "August::Long", "September::Long", "October::Long", "November::Long", "December::Long" };
	const char* cShortMonths[] = { "", "January::Short", "February::Short", "March::Short", "April::Short", "May::Short", "June::Short", "July::Short", "August::Short", "September::Short", "October::Short", "November::Short", "December::Short" };
	const char* cAbbrevMonths[] = { "", "January::Abbrev", "February::Abbrev", "March::Abbrev", "April::Abbrev", "May::Abbrev", "June::Abbrev", "July::Abbrev", "August::Abbrev", "September::Abbrev", "October::Abbrev", "November::Abbrev", "December::Abbrev" };
	const char** cMonths = NULL;

	switch (strl)
	{
	case eLong:
		cMonths = cLongMonths;
		break;
	case eShort:
		cMonths = cShortMonths;
		break;
	case eAbbreviated:
		cMonths = cAbbrevMonths;
		break;
	}
	return cMonths[month];
}

// Use 24 hour time display
bool CalendarLocale::Use24HourTime()
{
	static auto _init_done = false;

	if (!_init_done)
	{
		s24HourTime = true;

		_init_done = true;
	}

	return s24HourTime;
}

// Use dd/mm order for dates
bool CalendarLocale::UseDDMMDate()
{
	static auto _init_done_date = false;

	if (!_init_done_date)
	{
		sDDMMDate = true;

		_init_done_date = true;
	}

	return sDDMMDate;
}
