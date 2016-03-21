#pragma once
//#include "cdstring.h"
#include <stdint.h>
#include <ostream>
using namespace std;

namespace iCalendar {

	class CalendarLocale
	{
	public:
		enum EStringLength
		{
			eLong,
			eShort,
			eAbbreviated
		};
		static const string& GetDay(int32_t day, EStringLength strl = eLong);		// 0..6 - Sunday - Saturday
		static const string& GetMonth(int32_t month, EStringLength strl = eLong);	// 1..12 - January - December
		static bool Use24HourTime();												// Use 24 hour time display
		static bool UseDDMMDate();													// Use dd/mm order for dates

	private:
		static bool s24HourTime;
		static bool sDDMMDate;

	};

}	// namespace iCal

