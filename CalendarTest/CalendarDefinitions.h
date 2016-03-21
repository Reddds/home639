#pragma once
#include <stdint.h>

//using namespace std;

namespace iCalendar {

	// 2445 4.2.12
	enum EPartStat
	{
		ePartStat_NeedsAction = 0,
		ePartStat_Accepted,
		ePartStat_Declined,
		ePartStat_Tentative,
		ePartStat_Delegated,
		ePartStat_Completed,
		ePartStat_InProcess
	};
	// 2445 4.2.16
	enum EPartRole
	{
		ePartRole_Chair = 0,
		ePartRole_Required,
		ePartRole_Optional,
		ePartRole_Non
	};
	// Enums
	// Use ascending order for sensible sorting

	// 2445 4.3.10

	enum ERecurrence_FREQ
	{
		eRecurrence_SECONDLY,
		eRecurrence_MINUTELY,
		eRecurrence_HOURLY,
		eRecurrence_DAILY,
		eRecurrence_WEEKLY,
		eRecurrence_MONTHLY,
		eRecurrence_YEARLY

	};

	const unsigned long cICalValue_RECUR_FREQ_LEN = 5;

	enum ERecurrence_WEEKDAY
	{
		eRecurrence_WEEKDAY_SU,
		eRecurrence_WEEKDAY_MO,
		eRecurrence_WEEKDAY_TU,
		eRecurrence_WEEKDAY_WE,
		eRecurrence_WEEKDAY_TH,
		eRecurrence_WEEKDAY_FR,
		eRecurrence_WEEKDAY_SA

	};

	// 2445 4.8.1.11
	enum EStatus_VEvent
	{
		eStatus_VEvent_None,
		eStatus_VEvent_Confirmed,
		eStatus_VEvent_Tentative,
		eStatus_VEvent_Cancelled
	};

	enum EStatus_VToDo
	{
		eStatus_VToDo_None,
		eStatus_VToDo_NeedsAction,
		eStatus_VToDo_InProcess,
		eStatus_VToDo_Completed,
		eStatus_VToDo_Cancelled
	};

	enum EStatus_VJournal
	{
		eStatus_VJournal_None,
		eStatus_VJournal_Final,
		eStatus_VJournal_Draft,
		eStatus_VJournal_Cancelled
	};

	// 2445 4.8.6.1
	enum EAction_VAlarm
	{
		eAction_VAlarm_Audio,
		eAction_VAlarm_Display,
		eAction_VAlarm_Email,
		eAction_VAlarm_Procedure,
		eAction_VAlarm_Unknown
	};

	enum EAlarm_Status
	{
		eAlarm_Status_Pending,
		eAlarm_Status_Completed,
		eAlarm_Status_Disabled
	};

}	// namespace iCal

