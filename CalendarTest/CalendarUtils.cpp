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
CalendarUtils.cpp

Author:
Description:	<describe the CalendarUtils class here>
*/

#include "CalendarUtils.h"

#include "CalendarDateTime.h"

#include <strstream>

using namespace iCalendar;


// Write out iCal encoded text value
/*
void CalendarUtils::WriteTextValue(ostream& os, const string value)
{
	string::size_type start_pos = 0;
	string::size_type end_pos = value.find_first_of("\r\n;\\,", start_pos);
	string::size_type size_pos = value.length();
	if (end_pos != string::npos)
	{
		while (true)
		{
			// Write current segment
			os.write(value.c_str() + start_pos, end_pos - start_pos);

			// Write escape
			os << '\\';
			switch (value[end_pos])
			{
			case '\r':
				os << 'r';
				break;
			case '\n':
				os << 'n';
				break;
			case ';':
				os << ';';
				break;
			case '\\':
				os << '\\';
				break;
			case ',':
				os << ',';
				break;
			}

			// Bump past escapee and look for next segment
			start_pos = end_pos + 1;

			end_pos = value.find_first_of("\r\n;\\,", start_pos);
			if (end_pos == cdstring::npos)
			{
				os.write(value.c_str() + start_pos, size_pos - start_pos);
				break;
			}
		}
	}
	else
		os.write(value.c_str(), size_pos);
}
*/

uint8_t CalendarUtils::DaysInMonth(const uint8_t month, const uint8_t year)
{
	// NB month is 1..12 so use dummy value at start of array to avoid index adjustment
	int32_t days_in_month[] = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
	int32_t days_in_month_leap[] = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	return IsLeapYear(year) ? days_in_month_leap[month] : days_in_month[month];
}

uint16_t CalendarUtils::DaysUptoMonth(const uint8_t month, const uint8_t year)
{
	// NB month is 1..12 so use dummy value at start of array to avoid index adjustment
	int32_t days_upto_month[] = { 0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
	int32_t days_upto_month_leap[] = { 0, 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };

	return IsLeapYear(year) ? days_upto_month_leap[month] : days_upto_month[month];
}

bool CalendarUtils::IsLeapYear(const uint8_t year)
{
	uint16_t fullYear = 1900 + year;
	return ((fullYear % 4 == 0) && (fullYear % 100 != 0)) || (fullYear % 400 == 0);
}

uint16_t CalendarUtils::LeapDaysSince1970(const uint8_t year_offset)
{
	if (year_offset > 2)
		return (year_offset + 1) / 4;
	else if (year_offset < -1)
		return (year_offset - 2) / 4;
	else
		return 0;
}

void CalendarUtils::GetMonthTable(const int32_t month, const int32_t year, const CalendarDateTime::EDayOfWeek weekstart, CalendarTable& table, pair<int32_t, int32_t>& today_index)
{
	// Get today
	CalendarDateTime today(CalendarDateTime::GetToday());
	today_index = make_pair(-1, -1);

	// Start with empty table
	table.clear();

	// Determine first weekday in month
	CalendarDateTime temp(year, month, 1);
	int32_t row = -1;
	int32_t initial_col = temp.GetDayOfWeek() - weekstart;
	if (initial_col < 0)
		initial_col += 7;
	int32_t col = initial_col;

	// Counters
	int32_t max_day = DaysInMonth(month, year);

	// Fill up each row
	for (int32_t day = 1; day <= max_day; day++)
	{
		// Insert new row if we are at the start of a row
		if ((col == 0) || (day == 1))
		{
			vector<int32_t> row_data;
			row_data.insert(row_data.begin(), 7, 0);
			table.push_back(row_data);
			row++;
		}

		// Set the table item to the curret day
		table[row][col] = PackDate(temp.GetYear(), temp.GetMonth(), day);

		// Check on today
		if ((temp.GetYear() == today.GetYear()) && (temp.GetMonth() == today.GetMonth()) && (day == today.GetDay()))
		{
			today_index = make_pair(row, col);
		}

		// Bump column (modulo 7)
		col++;
		if (col > 6)
			col = 0;
	}

	// Add next month to remainder
	temp.OffsetMonth(1);
	if (col != 0)
	{
		for (int32_t day = 1; col < 7; day++, col++)
		{
			table[row][col] = PackDate(temp.GetYear(), temp.GetMonth(), -day);

			// Check on today
			if ((temp.GetYear() == today.GetYear()) && (temp.GetMonth() == today.GetMonth()) && (day == today.GetDay()))
			{
				today_index = make_pair(row, col);
			}
		}
	}

	// Add previous month to start
	temp.OffsetMonth(-2);
	if (initial_col != 0)
	{
		int32_t day = DaysInMonth(temp.GetMonth(), temp.GetYear());
		for (int32_t back_col = initial_col - 1; back_col >= 0; back_col--, day--)
		{
			table[0][back_col] = PackDate(temp.GetYear(), temp.GetMonth(), -day);

			// Check on today
			if ((temp.GetYear() == today.GetYear()) && (temp.GetMonth() == today.GetMonth()) && (day == today.GetDay()))
			{
				today_index = make_pair(0, back_col);
			}
		}
	}
}
