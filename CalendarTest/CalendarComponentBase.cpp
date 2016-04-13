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
CalendarComponentBase.cpp

Author:
Description:	<describe the CalendarComponentBase class here>
*/

#include "CalendarComponentBase.h"

#include "CalendarDateTimeValue.h"
#include "CalendarDurationValue.h"
#include "CalendarIntegerValue.h"
#include "CalendarMultiValue.h"
#include "CalendarPeriodValue.h"
#include "CalendarRecurrenceSet.h"
#include "CalendarRecurrenceValue.h"
#include "CalendarPlainTextValue.h"
#include "CalendarUTCOffsetValue.h"

using namespace iCalendar;

// Integers can be read from varios types of value
bool CalendarComponentBase::LoadValue(const char* value_name, int32_t& value, CalendarValue::EICalValueType type) const
{
	if (GetProperties().count(value_name))
	{
		switch (type)
		{
		case CalendarValue::eValueType_Integer:
		{
			const CalendarIntegerValue* ivalue = (*GetProperties().find(value_name)).second.GetIntegerValue();
			if (ivalue != NULL)
			{
				value = ivalue->GetValue();
				return true;
			}
			break;
		}
		case CalendarValue::eValueType_UTC_Offset:
		{
			const CalendarUTCOffsetValue* uvalue = (*GetProperties().find(value_name)).second.GetUTCOffsetValue();
			if (uvalue != NULL)
			{
				value = uvalue->GetValue();
				return true;
			}
			break;
		}
		default:;
		}
	}

	return false;
}

bool CalendarComponentBase::LoadValue(const char* value_name, cdstring& value) const
{
	if (GetProperties().count(value_name))
	{
		const CalendarPlainTextValue* tvalue = (*GetProperties().find(value_name)).second.GetTextValue();
		if (tvalue != NULL)
		{
			value = tvalue->GetValue();
			return true;
		}
	}

	return false;
}

bool CalendarComponentBase::LoadValue(const char* value_name, CalendarDateTime& value) const
{
	if (GetProperties().count(value_name))
	{
		const CalendarDateTimeValue* dtvalue = (*GetProperties().find(value_name)).second.GetDateTimeValue();
		if (dtvalue != NULL)
		{
			value = dtvalue->GetValue();
			return true;
		}
	}

	return false;
}

bool CalendarComponentBase::LoadValue(const char* value_name, CalendarDuration& value) const
{
	if (GetProperties().count(value_name))
	{
		const CalendarDurationValue* dvalue = (*GetProperties().find(value_name)).second.GetDurationValue();
		if (dvalue != NULL)
		{
			value = dvalue->GetValue();
			return true;
		}
	}

	return false;
}

bool CalendarComponentBase::LoadValue(const char* value_name, CalendarPeriod& value) const
{
	if (GetProperties().count(value_name))
	{
		const CalendarPeriodValue* pvalue = (*GetProperties().find(value_name)).second.GetPeriodValue();
		if (pvalue != NULL)
		{
			value = pvalue->GetValue();
			return true;
		}
	}

	return false;
}

bool CalendarComponentBase::LoadValueRRULE(const char* value_name, CalendarRecurrenceSet& value, bool add) const
{
	// Get RRULEs
	if (GetProperties().count(value_name))
	{
		pair<CalendarPropertyMap::const_iterator, CalendarPropertyMap::const_iterator> result = GetProperties().equal_range(value_name);
		for (CalendarPropertyMap::const_iterator iter = result.first; iter != result.second; iter++)
		{
			const CalendarRecurrenceValue* rvalue = (*iter).second.GetRecurrenceValue();
			if (rvalue != NULL)
			{
				if (add)
					value.Add(rvalue->GetValue());
				else
					value.Subtract(rvalue->GetValue());
			}
		}
		return true;
	}
	else
		return false;
}

bool CalendarComponentBase::LoadValueRDATE(const char* value_name, CalendarRecurrenceSet& value, bool add) const
{
	// Get RDATEs
	if (GetProperties().count(value_name))
	{
		pair<CalendarPropertyMap::const_iterator, CalendarPropertyMap::const_iterator> result = GetProperties().equal_range(value_name);
		for (CalendarPropertyMap::const_iterator iter = result.first; iter != result.second; iter++)
		{
			const CalendarMultiValue* mvalue = (*iter).second.GetMultiValue();
			if (mvalue != NULL)
			{
				for (CalendarValueList::const_iterator iter = mvalue->GetValues().begin(); iter != mvalue->GetValues().end(); iter++)
				{
					// cast to date-time
					CalendarDateTimeValue* dtv = dynamic_cast<CalendarDateTimeValue*>(*iter);
					CalendarPeriodValue* pv = dynamic_cast<CalendarPeriodValue*>(*iter);
					if (dtv != NULL)
					{
						if (add)
							value.Add(dtv->GetValue());
						else
							value.Subtract(dtv->GetValue());
					}
					else if (pv != NULL)
					{
						if (add)
							value.Add(pv->GetValue().GetStart());
						else
							value.Subtract(pv->GetValue().GetStart());
					}
				}
			}
		}
		return true;
	}
	else
		return false;
}

void CalendarComponentBase::WriteProperties(ostream& os) const
{
	for (CalendarPropertyMap::const_iterator iter = mProperties.begin(); iter != mProperties.end(); iter++)
		(*iter).second.Generate(os);
}

bool CalendarComponentBase::LoadPrivateValue(const char* value_name, cdstring& value)
{
	// Read it in from properties list and then delete the property from the main list
	bool result = LoadValue(value_name, value);
	if (result)
		RemoveProperties(value_name);
	return result;
}

void CalendarComponentBase::WritePrivateProperty(ostream& os, const cdstring& key, const cdstring& value) const
{
	CalendarProperty prop(key, value);
	prop.Generate(os);
}

CalendarPropertyMap& CalendarComponentBase::GetProperties()
{
	return mProperties;
}
const CalendarPropertyMap& CalendarComponentBase::GetProperties() const
{
	return mProperties;
}
void CalendarComponentBase::SetProperties(const CalendarPropertyMap& props)
{
	mProperties = props;
}

void CalendarComponentBase::AddProperty(const CalendarProperty& prop)
{
	CalendarPropertyMap::iterator result = mProperties.insert(CalendarPropertyMap::value_type(prop.GetName(), prop));
}

bool CalendarComponentBase::HasProperty(const cdstring& prop) const
{
	return mProperties.count(prop) > 0;
}

uint32_t CalendarComponentBase::CountProperty(const cdstring& prop) const
{
	return mProperties.count(prop);
}

void CalendarComponentBase::RemoveProperties(const cdstring& prop)
{
	mProperties.erase(prop);
}
