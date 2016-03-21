// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <algorithm>
#include <tchar.h>

#include "..\Pelen\Calendar.h"
#include <vector>
#include "CalendarDateTime.h"
#include "..\Pelen\Calendar.h"
// ----------------------- Limits
inline void ByMonthLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMonth rByMonth)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMONTH and indicate keep if input month matches
		for (auto i = 0; i < rByMonth.Length; i++)
		{
			if((*iter1).GetMonth() == rByMonth.Months[i])
				output.push_back(*iter1);
		}
	}

	dates = output;
}

inline void ByWeekNoLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByWeekNo rByWeekNo)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYWEEKNO and indicate keep if input month matches
		auto keep = false;
		for (auto i = 0; i < rByWeekNo.Length; i++)
		{
			keep = (*iter1).IsWeekNo(rByWeekNo.Weeknos[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void ByMonthDayLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMonthday rByMonthday)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMONTHDAY and indicate keep if input month matches
		auto keep = false;
		for (auto i = 0; i < rByMonthday.Length; i++)
		{
			keep = (*iter1).IsMonthDay(rByMonthday.Monthdays[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void ByDayLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByDay rByDay)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYDAY and indicate keep if input month matches
		auto keep = false;
		for (auto i = 0; i < rByDay.Length; i++)
		{
			keep = (*iter1).IsDayOfWeekInMonth(0, rByDay.Days[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void ByHourLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByHour rByHour)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYHOUR and indicate keep if input hour matches
		auto keep = false;
		for (auto i = 0; i < rByHour.Length; i++)
		{
			keep = ((*iter1).GetHours() == rByHour.Hours[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void ByMinuteLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMinute rByMinute)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMINUTE and indicate keep if input minute matches
		auto keep = false;
		for (auto i = 0; i < rByMinute.Length; i++)
		{
			keep = ((*iter1).GetMinutes() == rByMinute.Minutes[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void BySecondLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleBySecond rBySecond)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYSECOND and indicate keep if input second matches
		auto keep = false;
		for (auto i = 0; i < rBySecond.Length; i++)
		{
			keep = ((*iter1).GetSeconds() == rBySecond.Seconds[i]);
		}
		if (keep)
			output.push_back(*iter1);
	}

	dates = output;
}

inline void BySetPosLimit(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleBySetPos rBySetPos)
{
	// The input dates MUST be sorted in order for this to work properly
	std::sort(dates.begin(), dates.end());

	// Loop over each BYSETPOS and extract the relevant component from the input array and add to the output
	vector<iCalendar::CalendarDateTime> output;
	auto input_size = dates.size();
	for (auto i = 0; i < rBySetPos.Length; i++)
	{
		auto iter = rBySetPos.SetPoss[i];
		if (iter > 0)
		{
			// Positive values are offset from the start
			if (iter <= input_size)
				output.push_back(dates[iter - 1]);
		}
		else if (iter < 0)
		{
			// Negative values are offset from the end
			if (-iter <= input_size)
				output.push_back(dates[input_size + iter]);
		}
	}

	dates = output;
}

// ----------------------- Expands
inline void ByMonthExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMonth rByMonth)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMONTH and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByMonth.Length; i++)
		{
			auto temp(*iter1);
			temp.SetMonth(rByMonth.Months[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void ByWeekNoExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByWeekNo rByWeekNo)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYWEEKNO and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByWeekNo.Length; i++)
		{
			auto temp(*iter1);
			temp.SetWeekNo(rByWeekNo.Weeknos[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void ByYearDayExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByYearday rYearday)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYYEARDAY and generating a new date-time for it and insert into output
		for (auto i = 0; i < rYearday.Length; i++)
		{
			auto temp(*iter1);
			temp.SetYearDay(rYearday.Yeardays[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void ByMonthDayExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMonthday rByMonthday)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMONTHDAY and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByMonthday.Length; i++)
		{
			auto temp(*iter1);
			temp.SetMonthDay(rByMonthday.Monthdays[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void ByDayExpandYearly(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByDay rByDay)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYDAY and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByDay.Length; i++)
		{
			// Numeric value means specific instance
			auto iter2 = rByDay.Days[i];
			//if ((*iter2).first != 0)
			//{
			//	auto temp(*iter1);
			//	temp.SetDayOfWeekInYear((*iter2).first, iter2);
			//	output.push_back(temp);
			//}
			//else
			{
				// Every matching day in the year
				for (auto j = 1; j < 54; j++)
				{
					auto temp(*iter1);
					temp.SetDayOfWeekInYear(j, iter2);
					if (temp.GetYear() == (*iter1).GetYear())
						output.push_back(temp);
				}
			}
		}
	}

	dates = output;
}

inline void ByDayExpandMonthly(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByDay rByDay)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYDAY and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByDay.Length; i++)
		{
			auto iter2 = rByDay.Days[i];
			// Numeric value means specific instance
			//if ((*iter2).first != 0)
			//{
			//	CICalendarDateTime temp(*iter1);
			//	temp.SetDayOfWeekInMonth((*iter2).first, (*iter2).second);
			//	output.push_back(temp);
			//}
			//else
			{
				// Every matching day in the month
				for (auto j = 1; j < 6; j++)
				{
					auto temp(*iter1);
					temp.SetDayOfWeekInMonth(i, iter2);
					if (temp.GetMonth() == (*iter1).GetMonth())
						output.push_back(temp);
				}
			}
		}
	}

	dates = output;
}

inline void ByDayExpandWeekly(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByDay rByDay)
{
	// Must take into account the WKST value

	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYDAY and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByDay.Length; i++)
		{
			auto iter2 = rByDay.Days[i];
			// Numeric values are meaningless so ignore them
			//if ((*iter2).first == 0)
			{
				auto temp(*iter1);

				// Determine amount of offset to apply to temp to shift it to the start of the week (backwards)
				int32_t week_start_offset = temp.GetDayOfWeek();
				//if (week_start_offset > 0)
				//	week_start_offset -= 7;

				// Determine amount of offset from the start of the week to the day we want (forwards)
				int32_t day_in_week_offset = iter2;
				//if (day_in_week_offset < 0)
				//	day_in_week_offset += 7;

				// Apply offsets
				temp.OffsetDay(week_start_offset + day_in_week_offset);
				output.push_back(temp);
			}
		}
	}

	dates = output;
}

inline void ByHourExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByHour rByHour)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYHOUR and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByHour.Length; i++)
		{
			auto temp(*iter1);
			temp.SetHours(rByHour.Hours[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void ByMinuteExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleByMinute rByMinute)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYMINUTE and generating a new date-time for it and insert into output
		for (auto i = 0; i < rByMinute.Length; i++)
		{
			auto temp(*iter1);
			temp.SetMinutes(rByMinute.Minutes[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

inline void BySecondExpand(vector<iCalendar::CalendarDateTime> &dates, RdCalendar::RuleBySecond rBSecond)
{
	// Loop over all input items
	vector<iCalendar::CalendarDateTime> output;
	for (vector<iCalendar::CalendarDateTime>::const_iterator iter1 = dates.begin(); iter1 != dates.end(); ++iter1)
	{
		// Loop over each BYSECOND and generating a new date-time for it and insert into output
		for (auto i = 0; i < rBSecond.Length; i++)
		{
			auto temp(*iter1);
			temp.SetSeconds(rBSecond.Seconds[i]);
			output.push_back(temp);
		}
	}

	dates = output;
}

// TODO: reference additional headers your program requires here
