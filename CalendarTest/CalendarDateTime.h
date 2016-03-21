#pragma once
#include "CalendarDefinitions.h"

#include <stdint.h>
#include <ostream>
#include <vector>

using namespace std;

namespace iCalendar {
	class CalendarDuration;

	//typedef uint32_t	CalendarRef;	// Unique reference to object

	class CalendarDateTime
	{
	public:
		enum EDayOfWeek
		{
			eMonday,
			eTuesday,
			eWednesday,
			eThursday,
			eFriday,
			eSaturday,
			eSunday
		};

		enum ELocaleDate
		{
			eFullDate,
			eAbbrevDate,
			eNumericDate,

			eFullDateNoYear,
			eAbbrevDateNoYear,
			eNumericDateNoYear
		};

		CalendarDateTime()
		{
			_init_CalendarDateTime();
		}
		explicit CalendarDateTime(int32_t packed);
		CalendarDateTime(uint16_t year, uint8_t month, uint8_t day)
		{
			_init_CalendarDateTime(); 
			mYear = year; mMonth = month; mDay = day; mDateOnly = true;
		}
		CalendarDateTime(uint16_t year, uint8_t month, uint8_t day, uint8_t hours, uint8_t minutes, uint8_t seconds)
		{
			_init_CalendarDateTime(); 
			mYear = year; mMonth = month; mDay = day; mHours = hours; mMinutes = minutes; mSeconds = seconds;
		}
		CalendarDateTime(const CalendarDateTime& copy)
		{
			_copy_CalendarDateTime(copy);
		}

		CalendarDateTime& operator=(const CalendarDateTime& copy)
		{
			if (this != &copy) _copy_CalendarDateTime(copy); return *this;
		}

		CalendarDateTime operator+(const CalendarDuration& duration) const;
		CalendarDuration operator-(const CalendarDateTime& date) const;

		int operator==(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) == 0 ? 1 : 0;
		}
		int operator!=(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) != 0 ? 1 : 0;
		}
		int operator>=(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) >= 0 ? 1 : 0;
		}
		int operator<=(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) <= 0 ? 1 : 0;
		}
		int operator>(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) > 0 ? 1 : 0;
		}
		int operator<(const CalendarDateTime& comp) const
		{
			return CompareDateTime(comp) < 0 ? 1 : 0;
		}

		int CompareDateTime(const CalendarDateTime& comp) const;
		bool CompareDate(const CalendarDateTime& comp) const
		{
			return (mYear == comp.mYear) && (mMonth == comp.mMonth) && (mDay == comp.mDay);
		}
		uint32_t GetPosixTime() const;

		bool IsDateOnly() const
		{
			return mDateOnly;
		}
		void SetDateOnly(bool date_only)
		{
			mDateOnly = date_only;
		}

		int16_t GetYear() const
		{
			return mYear;
		}
		void SetYear(int16_t year)
		{
			if (mYear != year)
			{
				mYear = year;// Changed();
			}
		}
		void OffsetYear(int8_t diff_year)
		{
			mYear += diff_year; Normalise();
		}

		int8_t GetMonth() const
		{
			return mMonth;
		}
		const string GetMonthText(bool short_txt = true) const;
		void SetMonth(uint8_t month)
		{
			if (mMonth != month)
			{
				mMonth = month;// Changed();
			}
		}
		void OffsetMonth(int32_t diff_month)
		{
			mMonth += diff_month; Normalise();
		}

		uint8_t GetDay() const
		{
			return mDay;
		}
		void SetDay(uint8_t day)
		{
			if (mDay != day)
			{
				mDay = day;// Changed();
			}
		}
		void OffsetDay(int16_t diff_day)
		{
			mDay += diff_day; Normalise();
		}

		void SetYearDay(int16_t day);
		int16_t GetYearDay() const;

		void SetMonthDay(int32_t day);
		bool IsMonthDay(int32_t day) const;

		void SetWeekNo(int8_t weekno);
		int8_t GetWeekNo() const;
		bool IsWeekNo(int8_t weekno) const;

		void SetDayOfWeekInYear(int32_t offset, EDayOfWeek day);
		void SetDayOfWeekInMonth(int32_t offset, EDayOfWeek day);
		bool IsDayOfWeekInMonth(int32_t offset, EDayOfWeek day) const;

		EDayOfWeek GetDayOfWeek() const;
		const string GetDayOfWeekText(EDayOfWeek day) const;

		void SetHHMMSS(int8_t hours, int8_t minutes, int8_t seconds)
		{
			if ((mHours != hours) || (mMinutes != minutes) || (mSeconds != seconds))
			{
				mHours = hours; mMinutes = minutes; mSeconds = seconds;// Changed();
			}
		}

		int8_t GetHours() const
		{
			return mHours;
		}
		void SetHours(int8_t hours)
		{
			if (mHours != hours)
			{
				mHours = hours;// Changed();
			}
		}
		void OffsetHours(int8_t diff_hour)
		{
			mHours += diff_hour; Normalise();
		}

		int8_t GetMinutes() const
		{
			return mMinutes;
		}
		void SetMinutes(int32_t minutes)
		{
			if (mMinutes != minutes)
			{
				mMinutes = minutes;// Changed();
			}
		}
		void OffsetMinutes(int32_t diff_minutes)
		{
			mMinutes += diff_minutes; Normalise();
		}

		int32_t GetSeconds() const
		{
			return mSeconds;
		}
		void SetSeconds(int32_t seconds)
		{
			if (mSeconds != seconds)
			{
				mSeconds = seconds;// Changed();
			}
		}
		void OffsetSeconds(int32_t diff_seconds)
		{
			mSeconds += diff_seconds; Normalise();
		}


		void SetToday()
		{
			*this = GetToday();
		}
		static CalendarDateTime GetToday();
		void SetNow()
		{
			*this = GetNow();
		}
		static CalendarDateTime GetNow();
		void SetNowUTC()
		{
			*this = GetNowUTC();
		}
		static CalendarDateTime GetNowUTC();

		void Recur(ERecurrence_FREQ freq, uint16_t interval);

		string GetLocaleDate(ELocaleDate locale) const;
		string GetTime(bool with_seconds, bool am_pm) const;
		//char* GetLocaleDateTime(ELocaleDate locale, bool with_seconds, bool am_pm, bool tzid = false) const;
		string GetText() const;


	protected:
		uint8_t			mYear;		// full 4-digit year
		uint8_t			mMonth;		// 1...12
		uint16_t			mDay;		// 1...31

		uint8_t			mHours;		// 0...23
		uint8_t			mMinutes;	// 0...59
		uint8_t			mSeconds;	// 0...60

		bool			mDateOnly;

		//mutable pair<bool, uint32_t>	mPosixTime;

		void		Normalise();

	private:
		void _init_CalendarDateTime();
		void _copy_CalendarDateTime(const CalendarDateTime& copy);

		//void Changed() const
		//{
		//	mPosixTime.first = false;
		//}

		uint16_t DaysSince1970() const;
	};

	typedef vector<CalendarDateTime> CalendarDateTimeList;

}	// namespace iCal


