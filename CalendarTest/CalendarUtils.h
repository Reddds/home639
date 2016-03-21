#pragma once
#include "CalendarDateTime.h"

#include <iostream>
#include <vector>

//#include "cdstring.h"

using namespace std;

namespace iCalendar {

	class CalendarUtils
	{
	public:
		typedef vector<vector<int32_t> > CalendarTable;
		//static void WriteTextValue(ostream& os, const string& value);

		// Date/time calcs
		static uint8_t	DaysInMonth(const uint8_t month, const uint8_t year);
		static uint16_t	DaysUptoMonth(const uint8_t month, const uint8_t year);
		static bool	IsLeapYear(const uint8_t year);
		static uint16_t	LeapDaysSince1970(const uint8_t year_offset);

		// Packed date
		static int32_t PackDate(const int32_t year, const int32_t month, const int32_t day)
		{
			return (year << 16) | (month << 8) | (day + 128);
		}

		static void UnpackDate(const int32_t data, int32_t& year, int32_t& month, int32_t& day)
		{
			year = (data & 0xFFFF0000) >> 16; month = (data & 0x0000FF00) >> 8; day = (data & 0xFF) - 128;
		}
		static int32_t UnpackDateYear(const int32_t data)
		{
			return (data & 0xFFFF0000) >> 16;
		}
		static int32_t UnpackDateMonth(const int32_t data)
		{
			return (data & 0x0000FF00) >> 8;
		}
		static int32_t UnpackDateDay(const int32_t data)
		{
			return (data & 0xFF) - 128;
		}

		// Display elements
		static void		GetMonthTable(const int32_t month, const int32_t year, const CalendarDateTime::EDayOfWeek weekstart, CalendarTable& table, pair<int32_t, int32_t>& today_index);
	};

}	// namespace iCal


