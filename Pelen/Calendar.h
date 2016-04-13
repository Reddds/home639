﻿// Calendar.h
/*
Here is an example of evaluating multiple BYxxx rule parts.

	DTSTART;TZID=America/New_York:19970105T083000
	RRULE:FREQ=YEARLY;INTERVAL=2;BYMONTH=1;BYDAY=SU;BYHOUR=8,9;
	BYMINUTE=30

First, the "INTERVAL=2" would be applied to "FREQ=YEARLY" to
arrive at "every other year".  Then, "BYMONTH=1" would be applied
to arrive at "every January, every other year".  Then, "BYDAY=SU"
would be applied to arrive at "every Sunday in January, every
other year".  Then, "BYHOUR=8,9" would be applied to arrive at
"every Sunday in January at 8 AM and 9 AM, every other year".
Then, "BYMINUTE=30" would be applied to arrive at "every Sunday in
January at 8:30 AM and 9:30 AM, every other year".  Then, lacking
information from "RRULE", the second is derived from "DTSTART", to
end up in "every Sunday in January at 8:30:00 AM and 9:30:00 AM,
every other year".  Similarly, if the BYMINUTE, BYHOUR, BYDAY,
BYMONTHDAY, or BYMONTH rule part were missing, the appropriate
minute, hour, day, or month would have been retrieved from the
"DTSTART" property.
*/

#pragma once


#include <stdint.h>
#include "../CalendarTest/CalendarDateTime.h"

class RdCalendar
{
public:
	enum RuleTypes
	{
		FREQ, COUNT = 2, INTERVAL, BYSECOND, BYMINUTE, BYHOUR, BYDAY, BYMONTHDAY, BYYEARDAY, BYWEEKNO, BYMONTH, BYSETPOS
	};
	enum Freqs {SECONDLY, MINUTELY, HOURLY, DAILY, WEEKLY, MONTHLY, YEARLY};
	enum Weekdays {SU, MO, TU, WE, TH, FR, SA};
	struct RdDateTime
	{
		uint8_t Year;
		uint8_t Month;
		uint8_t Day;
		uint8_t Hour;
		uint8_t Minute;
	};

	/**
	  The FREQ rule part identifies the type of recurrence rule.  This
      rule part MUST be specified in the recurrence rule.  Valid values
      include SECONDLY, to specify repeating events based on an interval
      of a second or more; MINUTELY, to specify repeating events based
      on an interval of a minute or more; HOURLY, to specify repeating
      events based on an interval of an hour or more; DAILY, to specify
      repeating events based on an interval of a day or more; WEEKLY, to
      specify repeating events based on an interval of a week or more;
      MONTHLY, to specify repeating events based on an interval of a
      month or more; and YEARLY, to specify repeating events based on an
      interval of a year or more.
	*/
	struct RuleFreq
	{
		Freqs Freq : 8;
	};
	/*
	  The INTERVAL rule part contains a positive integer representing at
      which intervals the recurrence rule repeats.  The default value is
      "1", meaning every second for a SECONDLY rule, every minute for a
      MINUTELY rule, every hour for an HOURLY rule, every day for a
      DAILY rule, every week for a WEEKLY rule, every month for a
      MONTHLY rule, and every year for a YEARLY rule.  For example,
      within a DAILY rule, a value of "8" means every eight days.
	*/
	struct RuleInterval
	{
		uint16_t Interval;
	};
	/*
	  The COUNT rule part defines the number of occurrences at which to
      range-bound the recurrence.  The "DTSTART" property value always
      counts as the first occurrence.
	*/
	struct RuleCount
	{
		uint16_t Count;
	};
	/*
	  The BYSECOND rule part specifies a COMMA-separated list of seconds
      within a minute.  Valid values are 0 to 60.  The BYMINUTE rule
      part specifies a COMMA-separated list of minutes within an hour.
      Valid values are 0 to 59.  The BYHOUR rule part specifies a COMMA-
      separated list of hours of the day.  Valid values are 0 to 23.
      The BYSECOND, BYMINUTE and BYHOUR rule parts MUST NOT be specified
      when the associated "DTSTART" property has a DATE value type.
      These rule parts MUST be ignored in RECUR value that violate the
      above requirement (e.g., generated by applications that pre-date
      this revision of iCalendar).
	*/
	struct RuleBySecond
	{
		uint8_t Length;
		uint8_t *Seconds;
	};
	struct RuleByMinute
	{
		uint8_t Length;
		uint8_t *Minutes;
	};
	struct RuleByHour
	{
		uint8_t Length;
		uint8_t *Hours;
	};
	/*
	The BYDAY rule part specifies a COMMA-separated list of days of
	the week; SU indicates Sunday; MO indicates Monday; TU indicates
	Tuesday; WE indicates Wednesday; TH indicates Thursday; FR
	indicates Friday; and SA indicates Saturday.

	Each BYDAY value can also be preceded by a positive (+n) or
	negative (-n) integer.  If present, this indicates the nth
	occurrence of a specific day within the MONTHLY or YEARLY "RRULE".

	BYDAY has some special behavior depending on the FREQ value and
	this is described in separate notes below the table.

	╒══════════╤════════╤════════╤═══════╤═══════╤══════╤═══════╤══════╕
	│          │SECONDLY│MINUTELY│HOURLY │DAILY  │WEEKLY│MONTHLY│YEARLY│
	╞══════════╪════════╪════════╪═══════╪═══════╪══════╪═══════╪══════╡
	│BYMONTH   │Limit   │Limit   │Limit  │Limit  │Limit │Limit  │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYWEEKNO  │N/A     │N/A     │N/A    │N/A    │N/A   │N/A    │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYYEARDAY │Limit   │Limit   │Limit  │N/A    │N/A   │N/A    │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYMONTHDAY│Limit   │Limit   │Limit  │Limit  │N/A   │Expand │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYDAY     │Limit   │Limit   │Limit  │Limit  │Expand│Note 1 │Note 2│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYHOUR    │Limit   │Limit   │Limit  │Expand │Expand│Expand │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYMINUTE  │Limit   │Limit   │Expand │Expand │Expand│Expand │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYSECOND  │Limit   │Expand  │Expand │Expand │Expand│Expand │Expand│
	├──────────┼────────┼────────┼───────┼───────┼──────┼───────┼──────┤
	│BYSETPOS  │Limit   │Limit   │Limit  │Limit  │Limit │Limit  │Limit │
	└──────────┴────────┴────────┴───────┴───────┴──────┴───────┴──────┘

	Note 1:  Limit if BYMONTHDAY is present; otherwise, special expand
	for MONTHLY.

	Note 2:  Limit if BYYEARDAY or BYMONTHDAY is present; otherwise,
	special expand for WEEKLY if BYWEEKNO present; otherwise,
	special expand for MONTHLY if BYMONTH present; otherwise,
	special expand for YEARLY.
	*/
	struct RuleByDay
	{
		uint8_t Length;
		iCalendar::CalendarDateTime::EDayOfWeek *Days;
	};

	/*
	The BYMONTHDAY rule part specifies a COMMA-separated list of days
	of the month.  Valid values are 1 to 31 or -31 to -1.  For
	example, -10 represents the tenth to the last day of the month.
	The BYMONTHDAY rule part MUST NOT be specified when the FREQ rule
	part is set to WEEKLY.
	*/

	struct RuleByMonthday
	{
		uint8_t Length;
		uint8_t *Monthdays;
	};
	/*
	The BYYEARDAY rule part specifies a COMMA-separated list of days
	of the year.  Valid values are 1 to 366 or -366 to -1.  For
	example, -1 represents the last day of the year (December 31st)
	and -306 represents the 306th to the last day of the year (March
	1st).  The BYYEARDAY rule part MUST NOT be specified when the FREQ
	rule part is set to DAILY, WEEKLY, or MONTHLY.
	*/
	struct RuleByYearday
	{
		uint8_t Length;
		uint16_t *Yeardays;
	};
	/*
	The BYWEEKNO rule part specifies a COMMA-separated list of
	ordinals specifying weeks of the year.  Valid values are 1 to 53
	or -53 to -1.  This corresponds to weeks according to week
	numbering as defined in [ISO.8601.2004].  A week is defined as a
	seven day period, starting on the day of the week defined to be
	the week start (see WKST).  Week number one of the calendar year
	is the first week that contains at least four (4) days in that
	calendar year.  This rule part MUST NOT be used when the FREQ rule
	part is set to anything other than YEARLY.  For example, 3
	represents the third week of the year.

	Note: Assuming a Monday week start, week 53 can only occur when
	Thursday is January 1 or if it is a leap year and Wednesday is
	January 1.
	*/
	struct RuleByWeekNo
	{
		uint8_t Length;
		uint8_t *Weeknos;
	};
	/*
	The BYMONTH rule part specifies a COMMA-separated list of months
	of the year.  Valid values are 1 to 12.
	*/
	struct RuleByMonth
	{
		uint8_t Length;
		uint8_t *Months;
	};

	/*
	The BYSETPOS rule part specifies a COMMA-separated list of values
      that corresponds to the nth occurrence within the set of
      recurrence instances specified by the rule.  BYSETPOS operates on
      a set of recurrence instances in one interval of the recurrence
      rule.  For example, in a WEEKLY rule, the interval would be one
      week A set of recurrence instances starts at the beginning of the
      interval defined by the FREQ rule part.  Valid values are 1 to 366
      or -366 to -1.  It MUST only be used in conjunction with another
      BYxxx rule part.  For example "the last work day of the month"
      could be represented as:

       FREQ=MONTHLY;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=-1

      Each BYSETPOS value can include a positive (+n) or negative (-n)
      integer.  If present, this indicates the nth occurrence of the
      specific occurrence within the set of occurrences specified by the
      rule.
	*/
	struct RuleBySetPos
	{
		uint8_t Length;
		int16_t *SetPoss;
	};
	/*
	; The rule parts are not ordered in any
    ; particular sequence.
    ;
    ; The FREQ rule part is REQUIRED,
    ; but MUST NOT occur more than once.
    ;
    ; The UNTIL or COUNT rule parts are OPTIONAL,
    ; but they MUST NOT occur in the same 'recur'.
	*/
	struct Rrule
	{
		RuleTypes RuleType;
		union
		{
			RuleFreq Freq;
			RuleInterval Interval;
			RuleCount Count;
			RuleBySecond BySecond;
			RuleByMinute ByMinute;
			RuleByHour ByHour;
			RuleByDay ByDay;
			RuleByMonthday ByMonthday;
			RuleByYearday ByYearday;
			RuleByWeekNo ByWeekNo;
			RuleByMonth ByMonth;
			RuleBySetPos BySetPos;
		};
	};
	struct CalendarRecord
	{
		uint16_t RecordLength;
		RdDateTime start;
		RdDateTime end;
		uint8_t RulesCount;
		uint8_t MessageLen;
	};

	static void SetByMonth(Rrule &r, uint8_t *months, uint8_t count)
	{
		RuleByMonth bmo;
		bmo.Length = count;
		bmo.Months = months;

		r.RuleType = BYMONTH;
		r.ByMonth = bmo;
	}

	static void SetByWeekNo(Rrule &r, uint8_t *weekNos, uint8_t count)
	{
		RuleByWeekNo bwn;
		bwn.Length = count;
		bwn.Weeknos = weekNos;

		r.RuleType = BYWEEKNO;
		r.ByWeekNo = bwn;
	}


	static void SetByMonthDay(Rrule &r, uint8_t *monthsDays, uint8_t count)
	{
		RuleByMonthday bmd;
		bmd.Length = count;
		bmd.Monthdays = monthsDays;

		r.RuleType = BYMONTHDAY;
		r.ByMonthday = bmd;
	}

	static void SetByDay(Rrule &r, iCalendar::CalendarDateTime::EDayOfWeek *days, uint8_t count)
	{
		RuleByDay bd;
		bd.Length = count;
		bd.Days = days;

		r.RuleType = BYDAY;
		r.ByDay = bd;

	}

	static void SetBySetPos(Rrule &r, int16_t *days, uint8_t count)
	{
		RuleBySetPos bsp;
		bsp.Length = count;
		bsp.SetPoss = days;

		r.RuleType = BYSETPOS;
		r.BySetPos = bsp;

	}

	static void SetByHour(Rrule &r, uint8_t *hours, uint8_t count)
	{
		RuleByHour bh;
		bh.Length = count;
		bh.Hours = hours;

		r.RuleType = BYHOUR;
		r.ByHour = bh;
	}

	static void SetByMinute(Rrule &r, uint8_t *minutes, uint8_t count)
	{
		RuleByMinute bm;
		bm.Length = count;
		bm.Minutes = minutes;

		r.RuleType = BYMINUTE;
		r.ByMinute = bm;
	}

	static void SetBySecond(Rrule &r, uint8_t *seconds, uint8_t count)
	{
		RuleBySecond bs;
		bs.Length = count;
		bs.Seconds = seconds;

		r.RuleType = BYSECOND;
		r.BySecond = bs;
	}
};

