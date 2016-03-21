#pragma once
#include <stdint.h>
#include <ostream>

//#include "cdstring.h"

using namespace std;

namespace iCalendar {

	class CalendarDuration
	{
	public:
		CalendarDuration();
		CalendarDuration(int64_t seconds)
		{
			SetDuration(seconds);
		}
		CalendarDuration(const CalendarDuration& copy)
		{
			_copy_CalendarDuration(copy);
		}
		virtual ~CalendarDuration() {}

		CalendarDuration& operator=(const CalendarDuration& copy)
		{
			if (this != &copy) _copy_CalendarDuration(copy); return *this;
		}

		int64_t		GetTotalSeconds() const;
		void		SetDuration(const int64_t seconds);

		bool		GetForward() const
		{
			return mForward;
		}

		uint32_t	GetWeeks() const
		{
			return mWeeks;
		}
		uint32_t	GetDays() const
		{
			return mDays;
		}

		uint32_t	GetHours() const
		{
			return mHours;
		}
		uint32_t	GetMinutes() const
		{
			return mMinutes;
		}
		uint32_t	GetSeconds() const
		{
			return mSeconds;
		}

		void Generate(ostream& os) const;

	protected:
		bool		mForward;

		uint32_t	mWeeks;
		uint32_t	mDays;

		uint32_t	mHours;
		uint32_t	mMinutes;
		uint32_t	mSeconds;

	private:
		void _copy_CalendarDuration(const CalendarDuration& copy);
	};

}	// namespace iCal


