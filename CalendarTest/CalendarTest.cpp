// CalendarTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "../Pelen/Calendar.h"
#include "CalendarDateTime.h"
#include "CalendarDefinitions.h"
#include <string>
#include <iostream>

void PrintDateTime(string name, iCalendar::CalendarDateTime dateTime);

int main()
{



	printf("Hello, motherfuckers!\n");

	RdCalendar::CalendarRecord calRec;
	char *testString = "Test String";

	calRec.RecordLength = 100;
	iCalendar::CalendarDateTime start(116, 2, 3, 4, 5, 0);


	cout << sizeof(start) << endl;
	//RdCalendar::RdDateTime start;
	//start.Year = 16;
	//start.Month = 2;
	//start.Day = 3;
	//start.Hour = 4;
	//start.Minute = 5;

	iCalendar::CalendarDateTime end(136, 7, 8, 9, 10, 0);
	//RdCalendar::RdDateTime end;
	//end.Year = 16;
	//end.Month = 7;
	//end.Day = 8;
	//end.Hour = 9;
	//end.Minute = 10;

//	calRec.start = start;
//	calRec.end = end;
	calRec.RulesCount = 5;
	calRec.MessageLen = 16;


	//EEPROM.put(eeAddress, calRec);

	int recSize = sizeof(calRec);

	auto strPos = recSize;

	RdCalendar::Rrule rFreq;
	rFreq.RuleType = RdCalendar::FREQ;
	rFreq.Freq.Freq = RdCalendar::DAILY;

	//BYHOUR=8,9; BYMINUTE = 30
	uint8_t Hours[] = {8,9};
	RdCalendar::Rrule rByHour;
	RdCalendar::SetByHour(rByHour, Hours, 2);

	uint8_t Minutes[] = { 30 };
	RdCalendar::Rrule rByMinute;
	RdCalendar::SetByMinute(rByMinute, Minutes, 1);

	uint8_t Seconds[] = { 15,30,45 };
	RdCalendar::Rrule rBySecond;
	RdCalendar::SetBySecond(rBySecond, Seconds, 3);

	uint8_t Months[] = { 4, 6 };
	RdCalendar::Rrule rByMonth;
	RdCalendar::SetByMonth(rByMonth, Months, 2);

	uint8_t WeekNos[] = { 3, 5, 10, 20, 42 };
	RdCalendar::Rrule rWeekNo;
	RdCalendar::SetByWeekNo(rWeekNo, WeekNos, 5);



	strPos += sizeof(rFreq);

	//EEPROM.put(eeAddress + recSize, r);

	//for (int i = 0; i < 11; i++)
	//{
	//	EEPROM[eeAddress + strPos + i] = testString[i];
	//}

	//EEPROM.put(eeAddress + sizeof(calRec), testString);


	//for (int i = 0; i < 0x40; i++)
	//{
	//	if (i % 16 == 0)
	//	{
	//		Serial.print("\n");
	//		Serial.print(i, HEX);
	//		Serial.print(": ");
	//	}
	//	Serial.print(EEPROM[i], HEX);
	//	Serial.print(" ");
	//}

	//----------------------------------------------------------------
	auto curYear = 2016;
	uint8_t curMonth = 3;
	uint8_t curDate = 8;
	uint8_t curHour = 17;
	uint8_t curMin = 16;


	//EEPROM.get(eeAddress, calRec);

	PrintDateTime("Start", start);
	PrintDateTime("End", end);

	auto next = start;


	auto freq = iCalendar::eRecurrence_YEARLY;
	uint16_t interval = 1;

	vector<iCalendar::CalendarDateTime> dateTimes;
	for (;;)
	{
		dateTimes.push_back(iCalendar::CalendarDateTime(next));
		next.Recur(freq, interval);
		if (next > end)
			break;
	}

	/*
	If multiple BYxxx rule parts are specified, then after evaluating
      the specified FREQ and INTERVAL rule parts, the BYxxx rule parts
      are applied to the current set of evaluated occurrences in the
      following order: BYMONTH, BYWEEKNO, BYYEARDAY, BYMONTHDAY, BYDAY,
      BYHOUR, BYMINUTE, BYSECOND and BYSETPOS; then COUNT and UNTIL are
      evaluated.
	*/

	if (freq != iCalendar::eRecurrence_YEARLY)
	{
		ByMonthLimit(dateTimes, rByMonth.ByMonth);
	}
	else
	{
		ByMonthExpand(dateTimes, rByMonth.ByMonth);
	}

	if(freq == iCalendar::eRecurrence_YEARLY)
	{
		ByWeekNoExpand(dateTimes, rWeekNo.ByWeekNo);
	}

	switch (freq)
	{
	case iCalendar::eRecurrence_SECONDLY:
	case iCalendar::eRecurrence_MINUTELY:
	case iCalendar::eRecurrence_HOURLY:
		ByYearDayLimit();
		break;
	case iCalendar::eRecurrence_DAILY:
	case iCalendar::eRecurrence_WEEKLY:
	case iCalendar::eRecurrence_MONTHLY:
		break;
	case iCalendar::eRecurrence_YEARLY:
		ByYearDayExpand(dateTimes, rByHour.ByHour);
		break;
	default: break;
	}


	switch (freq)
	{
		case iCalendar::eRecurrence_SECONDLY: 
		case iCalendar::eRecurrence_MINUTELY: 
		case iCalendar::eRecurrence_HOURLY: 
		
			break;
		case iCalendar::eRecurrence_DAILY: 
		case iCalendar::eRecurrence_WEEKLY: 
		case iCalendar::eRecurrence_MONTHLY: 
		case iCalendar::eRecurrence_YEARLY: 
			ByHourExpand(dateTimes, rByHour.ByHour);
			break;
		default: break;
	}
	
	switch (freq)
	{
	case iCalendar::eRecurrence_SECONDLY:
	case iCalendar::eRecurrence_MINUTELY:

		break;
	case iCalendar::eRecurrence_HOURLY:
	case iCalendar::eRecurrence_DAILY:
	case iCalendar::eRecurrence_WEEKLY:
	case iCalendar::eRecurrence_MONTHLY:
	case iCalendar::eRecurrence_YEARLY:
		ByMinuteExpand(dateTimes, rByMinute.ByMinute);
		break;
	default: break;
	}
	if(freq == iCalendar::eRecurrence_SECONDLY)
	{
		
	}
	else
	{
		BySecondExpand(dateTimes, rBySecond.BySecond);
	}

	vector<iCalendar::CalendarDateTime>::iterator the_iterator;
	//the_iterator = dateTimes.begin();


	//while (the_iterator != dateTimes.end()) 
	//{
	//	for (auto i = 0; i < rByHour.ByHour.Length; i++)
	//	{
	//		if (i == 0)
	//		{
	//			next.SetHours(rByHour.ByHour.Hours[i]);
	//			continue;
	//		}
	//		auto temp(next);
	//		temp.SetHours(rByHour.ByHour.Hours[i]);
	//		dateTimes.push_back(temp);
	//		//PrintDateTime("Next", temp);
	//	}
	//}


	//for (;;)
	//{
	//	for (auto i = 0; i < rByHour.ByHour.Length; i++)
	//	{
	//		auto temp(next);
	//		temp.SetHours(rByHour.ByHour.Hours[i]);
	//		//PrintDateTime("Next", temp);
	//	}



	//	next.Recur(iCalendar::eRecurrence_WEEKLY, 1);
	//	if (next > end)
	//		break;
	//	//PrintDateTime("Next", next);
	//}
	the_iterator = dateTimes.begin();
	while (the_iterator != dateTimes.end())
	{
		PrintDateTime("Next", *the_iterator++);
	}
	cout << "Press ENTER";
	scanf_s("");

	// Список всех событий

	



    return 0;
}

void PrintDateTime(string name, iCalendar::CalendarDateTime dateTime)
{
	cout << name << " = " << dateTime.GetLocaleDate(iCalendar::CalendarDateTime::eFullDate) << " " << dateTime.GetTime(true, true) << endl;

}

