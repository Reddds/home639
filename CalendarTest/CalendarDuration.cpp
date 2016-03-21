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
CalendarDuration.cpp

Author:
Description:	<describe the CalendarDuration class here>
*/

#include "CalendarDuration.h"

#include <cerrno>

using namespace iCalendar;

CalendarDuration::CalendarDuration()
{
	mForward = true;

	mWeeks = 0;
	mDays = 0;

	mHours = 0;
	mMinutes = 0;
	mSeconds = 0;
}

void CalendarDuration::_copy_CalendarDuration(const CalendarDuration& copy)
{
	mForward = copy.mForward;

	mWeeks = copy.mWeeks;
	mDays = copy.mDays;

	mHours = copy.mHours;
	mMinutes = copy.mMinutes;
	mSeconds = copy.mSeconds;
}

int64_t CalendarDuration::GetTotalSeconds() const
{
	return (mForward ? 1LL : -1LL) * (mSeconds + (mMinutes + (mHours + (mDays + (mWeeks * 7LL)) * 24LL) * 60LL) * 60LL);
}

void CalendarDuration::SetDuration(const int64_t seconds)
{
	mForward = seconds >= 0;

	int64_t remainder = seconds;
	if (remainder < 0)
		remainder = -remainder;

	// Is it an exact number of weeks - if so use the weeks value, otherwise days, hours, minutes, seconds
	if (remainder % (7 * 24 * 60 * 60) == 0)
	{
		mWeeks = remainder / (7 * 24 * 60 * 60);
		mDays = 0;

		mHours = 0;
		mMinutes = 0;
		mSeconds = 0;
	}
	else
	{
		mSeconds = remainder % 60;
		remainder -= mSeconds;
		remainder /= 60;

		mMinutes = remainder % 60;
		remainder -= mMinutes;
		remainder /= 60;

		mHours = remainder % 24;
		remainder -= mHours;

		mDays = remainder / 24;

		mWeeks = 0;
	}
}

void CalendarDuration::Generate(ostream& os) const
{
	if (!mForward)
	{
		os << '-';
	}
	os << 'P';

	if (mWeeks != 0)
	{
		os << mWeeks << 'W';
	}
	else
	{
		if (mDays)
		{
			os << mDays << 'D';
		}

		if ((mHours != 0) || (mMinutes != 0) || (mSeconds != 0))
		{
			os << 'T';
		}

		if (mHours != 0)
		{
			os << mHours << 'H';
		}

		if ((mMinutes != 0) || ((mHours != 0) && (mSeconds != 0)))
		{
			os << mMinutes << 'M';
		}

		if (mSeconds != 0)
		{
			os << mSeconds << 'S';
		}
	}
}
