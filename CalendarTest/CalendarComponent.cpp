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
CalendarComponent.cpp

Author:
Description:	<describe the CalendarComponent class here>
*/

#include "CalendarComponent.h"

#include "Calendar.h"
#include "CalendarDateTimeValue.h"
#include "CalendarDefinitions.h"

#ifdef __MULBERRY
#include "CTCPSocket.h"
#endif

#include <algorithm>

using namespace iCalendar;

CalendarComponent::~CalendarComponent()
{
	// Delete any embedded components
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			delete *iter;
		mEmbedded->clear();
		delete mEmbedded;
		mEmbedded = NULL;
	}
}

void CalendarComponent::_copy_CalendarComponent(const CalendarComponent& copy)
{
	mCalendarRef = copy.mCalendarRef;
	mUID = copy.mUID;
	mSeq = copy.mSeq;
	mOriginalSeq = copy.mOriginalSeq;

	if (copy.mEmbedded != NULL)
	{
		// Do deep copy of element list
		mEmbedded = new CalendarComponentList;
		for (CalendarComponentList::const_iterator iter = copy.mEmbedded->begin(); iter != copy.mEmbedded->end(); iter++)
		{
			mEmbedded->push_back((*iter)->clone());
			mEmbedded->back()->SetEmbedder(this);
		}
	}

	mETag = copy.mETag;
	mRURL = copy.mRURL;
	mChanged = copy.mChanged;
}

bool CalendarComponent::AddComponent(CalendarComponent* comp)
{
	// Sub-classes decide what can be embedded
	return false;
}

void CalendarComponent::RemoveComponent(CalendarComponent* comp)
{
	if (mEmbedded != NULL)
	{
		mEmbedded->erase(std::remove(mEmbedded->begin(), mEmbedded->end(), comp), mEmbedded->end());
	}
}

bool CalendarComponent::HasEmbeddedComponent(EComponentType type) const
{
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::const_iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
		{
			if ((*iter)->GetType() == type)
				return true;
		}
	}

	return false;
}

CalendarComponent* CalendarComponent::GetFirstEmbeddedComponent(EComponentType type) const
{
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::const_iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
		{
			if ((*iter)->GetType() == type)
				return *iter;
		}
	}

	return NULL;
}

void CalendarComponent::SetUID(const cdstring& uid)
{
	if (uid.length())
		mUID = uid;
	else
	{
		// Get left-side of UID (first 24 chars of MD5 digest of time, clock and ctr)
		static unsigned long ctr = 1;
		cdstring lhs_txt;
		lhs_txt.reserve(256);
		::snprintf(lhs_txt.c_str_mod(), 256, "%lu.%lu.%lu", (time_t)clock(), time(NULL), ctr++);
		cdstring lhs;
		lhs_txt.md5(lhs);
		lhs[(cdstring::size_type)24] = 0;

		// Get right side (domain) of message-id
		cdstring rhs;
#ifdef __MULBERRY
		cdstring host = CTCPSocket::TCPGetLocalHostName();
		host.trimspace();
		if (host.length())
		{
			// Must put IP numbers inside [..]
			if (CTCPSocket::TCPIsHostName(host))
				rhs = host;
			else
			{
				rhs = "[";
				rhs += host;
				rhs += "]";
			}
		}
		else
#endif
		{
			// Use app name
			cdstring domain("mulberry");
			domain += cdstring(ctr);

			// Use first 24 chars of MD5 digest of the domain as the right-side of message-id
			domain.md5(rhs);
			rhs[(cdstring::size_type)24] = 0;
		}

		// Generate the UID string
		cdstring uid;
		uid += lhs;
		uid += "@";
		uid += rhs;

		mUID = uid;
	}

	RemoveProperties(cICalProperty_UID);

	CalendarProperty prop(cICalProperty_UID, mUID);
	AddProperty(prop);
}

void CalendarComponent::SetSeq(const int32_t& seq)
{
	mSeq = seq;

	RemoveProperties(cICalProperty_SEQUENCE);

	CalendarProperty prop(cICalProperty_SEQUENCE, mSeq);
	AddProperty(prop);
}

void CalendarComponent::GenerateRURL()
{
	// Format is:
	//
	// <<hash code>> *("-0"> .ics
	if (mRURL.empty())
	{
		// Generate hash code
		cdstring hash;
		hash += GetMapKey();
		hash += ":";
		hash += cdstring((long)GetSeq());
		hash += ":";

		CalendarDateTime dt;
		if (LoadValue(cICalProperty_DTSTAMP, dt))
		{
			hash += dt.GetText();
		}

		hash.md5(mRURL);

		// Truncate at 16 chars
		hash.erase(16);
	}
	else
	{
		// Strip off .ics
		mRURL.erase(mRURL.rfind(".ics", cdstring::npos, 4, true));
	}

	// Add trailer
	mRURL += "-0.ics";
}

void CalendarComponent::InitDTSTAMP()
{
	RemoveProperties(cICalProperty_DTSTAMP);

	CalendarProperty prop(cICalProperty_DTSTAMP, CalendarDateTime::GetNowUTC());
	AddProperty(prop);
}

void CalendarComponent::UpdateLastModified()
{
	RemoveProperties(cICalProperty_LAST_MODIFIED);

	CalendarProperty prop(cICalProperty_LAST_MODIFIED, CalendarDateTime::GetNowUTC());
	AddProperty(prop);
}

void CalendarComponent::Added()
{
	// Also add sub-components if present
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			(*iter)->Added();
	}

	mChanged = true;
}

void CalendarComponent::Removed()
{
	// Also remove sub-components
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			(*iter)->Removed();
	}

	mChanged = true;
}

void CalendarComponent::Duplicated()
{
	// Remove SEQ, UID, DTSTAMP
	// These will be re-created when it is added to the calendar
	RemoveProperties(cICalProperty_UID);
	RemoveProperties(cICalProperty_SEQUENCE);
	RemoveProperties(cICalProperty_DTSTAMP);

	// Remove the cached values as well
	mUID.clear();
	mSeq = 0;
	mOriginalSeq = 0;

	// Also duplicate sub-components
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			(*iter)->Duplicated();
	}

	// Reset CalDAV items
	mETag = cdstring::null_str;
	mRURL = cdstring::null_str;
	mChanged = true;
}

void CalendarComponent::Changed()
{
	// Bump the sequence
	SetSeq(GetSeq() + 1);

	// Update last-modified
	UpdateLastModified();

	// Also change sub-components
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			(*iter)->Changed();
	}

	mChanged = true;

	// Mark calendar as dirty
	Calendar* cal = Calendar::GetICalendar(GetCalendar());
	if (cal != NULL)
		cal->ChangedComponent(this);
}

void CalendarComponent::Finalise()
{
	// Get UID
	LoadValue(cICalProperty_UID, mUID);

	// Get SEQ
	LoadValue(cICalProperty_SEQUENCE, mSeq);

	// Cache the original sequence when the component is read in.
	// This will be used to synchronise changes between two instances of the same calendar
	mOriginalSeq = mSeq;

	// Get CalDAV info if present
	LoadPrivateValue(cICalProperty_X_PRIVATE_RURL, mRURL);
	LoadPrivateValue(cICalProperty_X_PRIVATE_ETAG, mETag);
}

void CalendarComponent::GetTimezones(cdstrset& tzids) const
{
	// Look for all date-time properties
	for (CalendarPropertyMap::const_iterator iter = mProperties.begin(); iter != mProperties.end(); iter++)
	{
		// Try to get a date-time value from the property
		const CalendarDateTimeValue* dtv = (*iter).second.GetDateTimeValue();
		if ((dtv != NULL) && !dtv->GetValue().IsDateOnly())
		{
			// Add timezone id if appropriate
			if (dtv->GetValue().GetTimezone().HasTZID())
				tzids.insert(dtv->GetValue().GetTimezone().GetTimezoneID());
		}
	}
}

void CalendarComponent::Generate(ostream& os, bool for_cache) const
{
	// Header
	os << GetBeginDelimiter() << net_endl;

	// Write each property
	WriteProperties(os);

	// Do private properties if caching
	if (for_cache)
	{
		if (!mRURL.empty())
			WritePrivateProperty(os, cICalProperty_X_PRIVATE_RURL, mRURL);
		if (!mETag.empty())
			WritePrivateProperty(os, cICalProperty_X_PRIVATE_ETAG, mETag);
	}

	// Write each embedded component
	if (mEmbedded != NULL)
	{
		for (CalendarComponentList::const_iterator iter = mEmbedded->begin(); iter != mEmbedded->end(); iter++)
			(*iter)->Generate(os, for_cache);
	}

	// Footer
	os << GetEndDelimiter() << net_endl;
}
