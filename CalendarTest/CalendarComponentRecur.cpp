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
CalendarComponentRecur.cpp

Author:
Description:	Base class for iCal components that can have recurrence
*/

#include "CalendarComponentRecur.h"

#include "Calendar.h"
#include "CalendarComponentExpanded.h"
#include "CalendarDateTimeValue.h"
#include "CalendarDefinitions.h"
#include "CalendarDuration.h"
#include "CalendarRecurrenceSet.h"

#include <algorithm>

using namespace iCalendar;

bool CalendarComponentRecur::sort_by_dtstart_allday(CalendarComponentRecur* e1, CalendarComponentRecur* e2)
{
	if (e1->mStart.IsDateOnly() && e2->mStart.IsDateOnly())
		return e1->mStart < e2->mStart;
	if (e1->mStart.IsDateOnly())
		return true;
	if (e2->mStart.IsDateOnly())
		return false;
	if (e1->mStart == e2->mStart)
	{
		if (e1->mEnd == e2->mEnd)
		// Put ones created earlier in earlier columns in day view
			return e1->mStamp < e2->mStamp;
		// Put ones that end later in earlier columns in day view
		return e1->mEnd > e2->mEnd;
	}
	return e1->mStart < e2->mStart;
}

bool CalendarComponentRecur::sort_by_dtstart(CalendarComponentRecur* e1, CalendarComponentRecur* e2)
{
	if (e1->mStart == e2->mStart)
	{
		if (e1->mStart.IsDateOnly() ^ e2->mStart.IsDateOnly())
			return e1->mStart.IsDateOnly();
		else
			return false;
	}
	else
		return e1->mStart < e2->mStart;
}

CalendarComponentRecur::CalendarComponentRecur(const CalendarRef& calendar) :
	CalendarComponent(calendar)
{
	mMaster = this;
	mHasStamp = false;
	mHasStart = false;
	mHasEnd = false;
	mDuration = false;
	mHasRecurrenceID = false;
	mAdjustFuture = false;
	mAdjustPrior = false;
	mRecurrences = NULL;
}

CalendarComponentRecur::CalendarComponentRecur(const CalendarComponentRecur& copy) :
	CalendarComponent(copy)
{
	mRecurrences = NULL;
	_copy_CalendarComponentRecur(copy);
}

void CalendarComponentRecur::_copy_CalendarComponentRecur(const CalendarComponentRecur& copy)
{
	// Special determination of master
	if (copy.Recurring())
		mMaster = copy.mMaster;
	else
		mMaster = this;

	mMapKey = copy.mMapKey;

	mSummary = copy.mSummary;

	mStamp = copy.mStamp;
	mHasStamp = copy.mHasStamp;

	mStart = copy.mStart;
	mHasStart = copy.mHasStart;
	mEnd = copy.mEnd;
	mHasEnd = copy.mHasEnd;
	mDuration = copy.mDuration;

	mHasRecurrenceID = copy.mHasRecurrenceID;
	mAdjustFuture = copy.mAdjustFuture;
	mAdjustPrior = copy.mAdjustPrior;
	mRecurrenceID = copy.mRecurrenceID;

	if (copy.mRecurrences != NULL)
		mRecurrences = new CalendarRecurrenceSet(*copy.mRecurrences);
}

void CalendarComponentRecur::_tidy_CalendarComponentRecur()
{
	delete mRecurrences;
	mRecurrences = NULL;
}

void CalendarComponentRecur::Finalise()
{
	CalendarComponent::Finalise();

	// Get DTSTAMP
	mHasStamp = LoadValue(cICalProperty_DTSTAMP, mStamp);

	// Get DTSTART
	mHasStart = LoadValue(cICalProperty_DTSTART, mStart);

	// Get DTEND
	if (!LoadValue(cICalProperty_DTEND, mEnd))
	{
		// Try DURATION instead
		CalendarDuration temp;
		if (LoadValue(cICalProperty_DURATION, temp))
		{
			mEnd = mStart + temp;
			mDuration = true;
		}
		else
		{
			// Force end to start, which will then be fixed to sensible value later
			mEnd = mStart;
		}
	}
	else
	{
		mHasEnd = true;
		mDuration = false;
	}

	// Make sure start/end values are sensible
	FixStartEnd();

	// Get SUMMARY
	LoadValue(cICalProperty_SUMMARY, mSummary);

	// Get RECURRENCE-ID
	mHasRecurrenceID = LoadValue(cICalProperty_RECURRENCE_ID, mRecurrenceID);

	// Update the map key
	if (mHasRecurrenceID)
	{
		mMapKey = MapKey(mUID, mRecurrenceID.GetText());

		// Also get the RANGE attribute
		const CalendarAttributeMap& attrs = (*GetProperties().find(cICalProperty_RECURRENCE_ID)).second.GetAttributes();
		CalendarAttributeMap::const_iterator found = attrs.find(cICalAttribute_RANGE);
		if (found != attrs.end())
		{
			mAdjustFuture = ((*found).second.GetFirstValue() == cICalAttribute_RANGE_THISANDFUTURE);
			mAdjustPrior = ((*found).second.GetFirstValue() == cICalAttribute_RANGE_THISANDPRIOR);
		}
		else
		{
			mAdjustFuture = false;
			mAdjustPrior = false;
		}
	}
	else
		mMapKey = MapKey(mUID);

	// May need to create items
	if ((GetProperties().count(cICalProperty_RRULE) != 0) ||
		(GetProperties().count(cICalProperty_RDATE) != 0) ||
		(GetProperties().count(cICalProperty_EXRULE) != 0) ||
		(GetProperties().count(cICalProperty_EXDATE) != 0))
	{
		if (mRecurrences == NULL)
			mRecurrences = new CalendarRecurrenceSet;

		// Get RRULEs
		LoadValueRRULE(cICalProperty_RRULE, *mRecurrences, true);

		// Get RDATEs
		LoadValueRDATE(cICalProperty_RDATE, *mRecurrences, true);

		// Get EXRULEs
		LoadValueRRULE(cICalProperty_EXRULE, *mRecurrences, false);

		// Get EXDATEs
		LoadValueRDATE(cICalProperty_EXDATE, *mRecurrences, false);
	}
}

void CalendarComponentRecur::FixStartEnd()
{
	// End is always greater than start if start exists
	if (mHasStart && (mEnd <= mStart))
	{
		// Use the start
		mEnd = mStart;
		mDuration = false;

		// Adjust to approriate non-inclusive end point
		if (mStart.IsDateOnly())
		{
			mEnd.OffsetDay(1);

			// For all day events it makes sense to use duration
			mDuration = true;
		}
		else
		{
			// USe end of current day
			mEnd.OffsetDay(1);
			mEnd.SetHHMMSS(0, 0, 0);
		}
	}
}

cdstring CalendarComponentRecur::MapKey(const cdstring& uid)
{
	return cdstring("u:") + uid;
}

cdstring CalendarComponentRecur::MapKey(const cdstring& uid, const cdstring& rid)
{
	return cdstring("r:") + uid + rid;
}


void CalendarComponentRecur::SetUID(const cdstring& uid)
{
	CalendarComponent::SetUID(uid);

	// Update the map key
	if (mHasRecurrenceID)
	{
		mMapKey = MapKey(mUID, mRecurrenceID.GetText());
	}
	else
	{
		mMapKey = MapKey(mUID);
	}
}

cdstring CalendarComponentRecur::GetMasterKey() const
{
	return MapKey(mUID);
}

void CalendarComponentRecur::InitDTSTAMP()
{
	// Save new one
	CalendarComponent::InitDTSTAMP();

	// Get the new one
	mHasStamp = LoadValue(cICalProperty_DTSTAMP, mStamp);
}

cdstring CalendarComponentRecur::GetDescription() const
{
	// Get DESCRIPTION
	cdstring txt;
	LoadValue(cICalProperty_DESCRIPTION, txt);
	return txt;
}

cdstring CalendarComponentRecur::GetLocation() const
{
	// Get LOCATION
	cdstring txt;
	LoadValue(cICalProperty_LOCATION, txt);
	return txt;
}

bool CalendarComponentRecur::GetTransparent() const
{
	cdstring txt;
	if (LoadValue(cICalProperty_TRANSP, txt))
		return txt == cICalProperty_TRANSPARENT;
	else
		return false;
}

void CalendarComponentRecur::SetMaster(CalendarComponentRecur* master)
{
	mMaster = master;
	InitFromMaster();
}

void CalendarComponentRecur::InitFromMaster()
{
	// Only if not master
	if (Recurring())
	{
		// Redo this to get cached values from master
		Finalise();

		// If this component does not have its own start property, use the recurrence id
		// i.e. the start time of this instance has not changed - something else has
		if (GetProperties().count(cICalProperty_DTSTART) == 0)
			mStart = mRecurrenceID;

		// If this component does not have its own end/duration property, the determine
		// the end from the master duration
		if ((GetProperties().count(cICalProperty_DTEND) == 0) && (GetProperties().count(cICalProperty_DURATION) == 0))
		{
			// End is based on original events settings
			mEnd = mStart + (mMaster->GetEnd() - mMaster->GetStart());
		}

		// If this instance has a duration, but no start of its own, then we need to readjust the end
		// to account for the start being changed to the recurrence id
		else if ((GetProperties().count(cICalProperty_DURATION) != 0) && (GetProperties().count(cICalProperty_DTSTART) == 0))
		{
			CalendarDuration temp;
			LoadValue(cICalProperty_DURATION, temp);
			mEnd = mStart + temp;
		}
	}
}

void CalendarComponentRecur::ExpandPeriod(const CalendarPeriod& period, CalendarExpandedComponents& list)
{
	// Check for recurrence and true master
	if ((mRecurrences != NULL) && mRecurrences->HasRecurrence() && !IsRecurrenceInstance())
	{
		// Expand recurrences within the range
		CalendarDateTimeList items;
		mRecurrences->Expand(mStart, period, items);

		// Look for overridden recurrence items
		Calendar* cal = Calendar::GetICalendar(GetCalendar());
		if (cal != NULL)
		{
			// Remove recurrence instances from the list of items
			CalendarDateTimeList recurs;
			cal->GetRecurrenceInstances(CalendarComponent::eVEVENT, GetUID(), recurs);
			::sort(recurs.begin(), recurs.end());
			if (recurs.size() != 0)
			{
				CalendarDateTimeList temp;
				::set_difference(items.begin(), items.end(), recurs.begin(), recurs.end(), back_inserter(temp));
				items = temp;

				// Now get actual instances
				CalendarComponentRecurs instances;
				cal->GetRecurrenceInstances(CalendarComponent::eVEVENT, GetUID(), instances);

				// Get list of each ones with RANGE
				CalendarComponentRecurs prior;
				CalendarComponentRecurs future;
				for (CalendarComponentRecurs::const_iterator iter = instances.begin(); iter != instances.end(); iter++)
				{
					if ((*iter)->IsAdjustPrior())
						prior.push_back(*iter);
					if ((*iter)->IsAdjustFuture())
						future.push_back(*iter);
				}

				// Check for special behaviour
				if (prior.empty() && future.empty())
				{
					// Add each expanded item
					for (CalendarDateTimeList::const_iterator iter = items.begin(); iter != items.end(); iter++)
					{
						list.push_back(CreateExpanded(this, *iter));
					}
				}
				else
				{
					// Sort each list first
					std::sort(prior.begin(), prior.end(), sort_by_dtstart);
					std::sort(future.begin(), future.end(), sort_by_dtstart);

					// Add each expanded item
					for (CalendarDateTimeList::const_iterator iter1 = items.begin(); iter1 != items.end(); iter1++)
					{
						// Now step through each using the slave item instead of the master as appropriate
						CalendarComponentRecur* slave = NULL;

						// Find most appropriate THISANDPRIOR item
						for (CalendarComponentRecurs::reverse_iterator riter2 = prior.rbegin(); riter2 != prior.rend(); riter2++)
						{
							if ((*riter2)->GetStart() > *iter1)
							{
								slave = *riter2;
								break;
							}
						}

						// Find most appropriate THISANDFUTURE item
						for (CalendarComponentRecurs::reverse_iterator riter2 = future.rbegin(); riter2 != future.rend(); riter2++)
						{
							if ((*riter2)->GetStart() < *iter1)
							{
								slave = *riter2;
								break;
							}
						}

						list.push_back(CreateExpanded(slave != NULL ? slave : this, *iter1));
					}
				}
			}
			else
			{
				// Add each expanded item
				for (CalendarDateTimeList::const_iterator iter = items.begin(); iter != items.end(); iter++)
				{
					list.push_back(CreateExpanded(this, *iter));
				}
			}
		}
	}

	else if (WithinPeriod(period))
		list.push_back(CalendarComponentExpandedShared(new CalendarComponentExpanded(this, IsRecurrenceInstance() ? &mRecurrenceID : NULL)));
}

bool CalendarComponentRecur::WithinPeriod(const CalendarPeriod& period) const
{
	// Check for recurrence
	if ((mRecurrences != NULL) && mRecurrences->HasRecurrence())
	{
		CalendarDateTimeList items;
		mRecurrences->Expand(mStart, period, items);
		return !items.empty();
	}
	else
	{
		// Does event span the period (assume mEnd > mStart)
		// Check start (inclusive) and end (exclusive)
		if ((mEnd <= period.GetStart()) || (mStart >= period.GetEnd()))
			return false;
		else
			return true;
	}
}

CalendarComponentExpandedShared CalendarComponentRecur::CreateExpanded(CalendarComponentRecur* master, const CalendarDateTime& recurid)
{
	return CalendarComponentExpandedShared(new CalendarComponentExpanded(master, &recurid));
}

bool CalendarComponentRecur::IsRecurring() const
{
	return (mRecurrences != NULL) && mRecurrences->HasRecurrence();
}

// Clear out all cached recurrence data
void CalendarComponentRecur::ChangedRecurrence()
{
	// Clear cached values
	if (mRecurrences != NULL)
		mRecurrences->Changed();
}

void CalendarComponentRecur::EditSummary(const cdstring& summary)
{
	// Updated cached value
	mSummary = summary;

	// Remove existing items
	RemoveProperties(cICalProperty_SUMMARY);

	// Now create properties
	if (summary.length())
	{
		CalendarProperty prop(cICalProperty_SUMMARY, summary);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditDetails(const cdstring& description, const cdstring& location)
{
	// Remove existing items
	RemoveProperties(cICalProperty_DESCRIPTION);
	RemoveProperties(cICalProperty_LOCATION);

	// Now create properties
	if (description.length())
	{
		cdstring convert(description);
		convert.ConvertEndl(eEndl_LF);
		CalendarProperty prop(cICalProperty_DESCRIPTION, convert);
		AddProperty(prop);
	}
	if (location.length())
	{
		CalendarProperty prop(cICalProperty_LOCATION, location);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditTransparent(bool transparent)
{
	// Remove existing items
	RemoveProperties(cICalProperty_TRANSP);

	// Only need to add if TRANSPARENT, as OPAQUE is default if property is not present.
	if (transparent)
	{
		CalendarProperty prop(cICalProperty_TRANSP, cICalProperty_TRANSPARENT);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditTiming()
{
	// Updated cached values
	mHasStart = mHasEnd = mDuration = false;
	mStart.SetToday();
	mEnd.SetToday();

	// Remove existing DTSTART & DTEND & DURATION & DUE items
	RemoveProperties(cICalProperty_DTSTART);
	RemoveProperties(cICalProperty_DTEND);
	RemoveProperties(cICalProperty_DURATION);
	RemoveProperties(cICalProperty_DUE);
}

void CalendarComponentRecur::EditTiming(const CalendarDateTime& due)
{
	// Updated cached values
	mHasStart = false;
	mHasEnd = true;
	mDuration = false;
	mStart = due;
	mEnd = due;

	// Remove existing DUE & DTSTART & DTEND & DURATION items
	RemoveProperties(cICalProperty_DUE);
	RemoveProperties(cICalProperty_DTSTART);
	RemoveProperties(cICalProperty_DTEND);
	RemoveProperties(cICalProperty_DURATION);

	// Now create properties
	{
		CalendarProperty prop(cICalProperty_DUE, due);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditTiming(const CalendarDateTime& start, const CalendarDateTime& end)
{
	// Updated cached values
	mHasStart = mHasEnd = true;
	mStart = start;
	mEnd = end;
	mDuration = false;
	FixStartEnd();

	// Remove existing DTSTART & DTEND & DURATION & DUE items
	RemoveProperties(cICalProperty_DTSTART);
	RemoveProperties(cICalProperty_DTEND);
	RemoveProperties(cICalProperty_DURATION);
	RemoveProperties(cICalProperty_DUE);

	// Now create properties
	{
		CalendarProperty prop(cICalProperty_DTSTART, start);
		AddProperty(prop);
	}

	// If its an all day event and the end one day after the start, ignore it
	CalendarDateTime temp(start);
	temp.OffsetDay(1);
	if (!start.IsDateOnly() || (end != temp))
	{
		CalendarProperty prop(cICalProperty_DTEND, end);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditTiming(const CalendarDateTime& start, const CalendarDuration& duration)
{
	// Updated cached values
	mHasStart = true;
	mHasEnd = false;
	mStart = start;
	mEnd = start + duration;
	mDuration = true;

	// Remove existing DTSTART & DTEND & DURATION & DUE items
	RemoveProperties(cICalProperty_DTSTART);
	RemoveProperties(cICalProperty_DTEND);
	RemoveProperties(cICalProperty_DURATION);
	RemoveProperties(cICalProperty_DUE);

	// Now create properties
	{
		CalendarProperty prop(cICalProperty_DTSTART, start);
		AddProperty(prop);
	}

	// If its an all day event and the duration is one day, ignore it
	if (!start.IsDateOnly() || (duration.GetWeeks() != 0) || (duration.GetDays() > 1))
	{
		CalendarProperty prop(cICalProperty_DURATION, duration);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::EditRecurrenceSet(const CalendarRecurrenceSet& recurs)
{
	// Must have items
	if (mRecurrences == NULL)
		mRecurrences = new CalendarRecurrenceSet;

	// Updated cached values
	*mRecurrences = recurs;

	// Remove existing RRULE, EXRULE, RDATE & EXDATE
	RemoveProperties(cICalProperty_RRULE);
	RemoveProperties(cICalProperty_EXRULE);
	RemoveProperties(cICalProperty_RDATE);
	RemoveProperties(cICalProperty_EXDATE);

	// Now create properties
	for (CalendarRecurrenceList::const_iterator iter = mRecurrences->GetRules().begin(); iter != mRecurrences->GetRules().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_RRULE, *iter);
		AddProperty(prop);
	}
	for (CalendarRecurrenceList::const_iterator iter = mRecurrences->GetExrules().begin(); iter != mRecurrences->GetExrules().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_EXRULE, *iter);
		AddProperty(prop);
	}
	for (CalendarDateTimeList::const_iterator iter = mRecurrences->GetDates().begin(); iter != mRecurrences->GetDates().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_RDATE, *iter);
		AddProperty(prop);
	}
	for (CalendarDateTimeList::const_iterator iter = mRecurrences->GetExdates().begin(); iter != mRecurrences->GetExdates().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_EXDATE, *iter);
		AddProperty(prop);
	}
}

void CalendarComponentRecur::ExcludeRecurrence(const CalendarDateTime& start)
{
	// Must have items
	if (mRecurrences == NULL)
		return;

	// Add to recurrence set and clear cache
	mRecurrences->Subtract(start);

	// Add property
	CalendarProperty prop(cICalProperty_EXDATE, start);
	AddProperty(prop);
}

void CalendarComponentRecur::ExcludeFutureRecurrence(const CalendarDateTime& start)
{
	// Must have items
	if (mRecurrences == NULL)
		return;

	// Adjust RRULES to end before start
	mRecurrences->ExcludeFutureRecurrence(start);

	// Remove existing RRULE & RDATE
	RemoveProperties(cICalProperty_RRULE);
	RemoveProperties(cICalProperty_RDATE);

	// Now create properties
	for (CalendarRecurrenceList::const_iterator iter = mRecurrences->GetRules().begin(); iter != mRecurrences->GetRules().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_RRULE, *iter);
		AddProperty(prop);
	}
	for (CalendarDateTimeList::const_iterator iter = mRecurrences->GetDates().begin(); iter != mRecurrences->GetDates().end(); iter++)
	{
		CalendarProperty prop(cICalProperty_RDATE, *iter);
		AddProperty(prop);
	}
}

// Integers can be read from varios types of value
bool CalendarComponentRecur::LoadValue(const char* value_name, int32_t& value, CalendarValue::EICalValueType type) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValue(value_name, value, type);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValue(value_name, value, type);

	return result;
}

bool CalendarComponentRecur::LoadValue(const char* value_name, cdstring& value) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValue(value_name, value);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValue(value_name, value);

	return result;
}

bool CalendarComponentRecur::LoadValue(const char* value_name, CalendarDateTime& value) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValue(value_name, value);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValue(value_name, value);

	return result;
}

bool CalendarComponentRecur::LoadValue(const char* value_name, CalendarDuration& value) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValue(value_name, value);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValue(value_name, value);

	return result;
}

bool CalendarComponentRecur::LoadValue(const char* value_name, CalendarPeriod& value) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValue(value_name, value);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValue(value_name, value);

	return result;
}

bool CalendarComponentRecur::LoadValueRRULE(const char* value_name, CalendarRecurrenceSet& value, bool add) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValueRRULE(value_name, value, add);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValueRRULE(value_name, value, add);

	return result;
}

bool CalendarComponentRecur::LoadValueRDATE(const char* value_name, CalendarRecurrenceSet& value, bool add) const
{
	// Try to load from this component
	bool result = CalendarComponent::LoadValueRDATE(value_name, value, add);

	// Try to load from master if we didn't get it from this component
	if (!result && (mMaster != NULL) && (mMaster != this))
		result = mMaster->LoadValueRDATE(value_name, value, add);

	return result;
}
