#pragma once
#include "CalendarProperty.h"

#include <stdint.h>
#include <iostream>
#include <map>

//#include "cdstring.h"

using namespace std;

namespace iCalendar {

	class CalendarDateTime;
	class CalendarDuration;
	class CalendarPeriod;
	class CalendarRecurrenceSet;

	class CalendarComponentBase
	{
	public:
		CalendarComponentBase() {}
		CalendarComponentBase(const CalendarComponentBase& copy)
		{
			_copy_CalendarComponentBase(copy);
		}
		virtual ~CalendarComponentBase() {}

		CalendarComponentBase& operator=(const CalendarComponentBase& copy)
		{
			if (this != &copy) _copy_CalendarComponentBase(copy); return *this;
		}

		CalendarPropertyMap& GetProperties();
		const CalendarPropertyMap& GetProperties() const;
		void SetProperties(const CalendarPropertyMap& props);

		void AddProperty(const CalendarProperty& prop);
		bool HasProperty(const cdstring& prop) const;
		uint32_t CountProperty(const cdstring& prop) const;
		void RemoveProperties(const cdstring& prop);

		bool GetProperty(const cdstring& prop, int32_t& value, CalendarValue::EICalValueType type = CalendarValue::eValueType_Integer) const
		{
			return LoadValue(prop.c_str(), value, type);
		}
		bool GetProperty(const cdstring& prop, cdstring& value) const
		{
			return LoadValue(prop.c_str(), value);
		}
		bool GetProperty(const cdstring& prop, CalendarDateTime& value) const
		{
			return LoadValue(prop.c_str(), value);
		}

		virtual void Finalise() = 0;

		virtual void Generate(ostream& os, bool for_cache = false) const = 0;

#ifdef _GCC_2_95
	public:
#else
	protected:
#endif
		CalendarPropertyMap		mProperties;

		virtual bool	LoadValue(const char* value_name, int32_t& value, CalendarValue::EICalValueType type = CalendarValue::eValueType_Integer) const;
		virtual bool	LoadValue(const char* value_name, cdstring& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarDateTime& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarDuration& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarPeriod& value) const;
		virtual bool	LoadValueRRULE(const char* value_name, CalendarRecurrenceSet& value, bool add) const;
		virtual bool	LoadValueRDATE(const char* value_name, CalendarRecurrenceSet& value, bool add) const;

		void	WriteProperties(ostream& os) const;

		bool	LoadPrivateValue(const char* value_name, cdstring& value);
		void	WritePrivateProperty(ostream& os, const cdstring& key, const cdstring& value) const;

	private:
		void	_copy_CalendarComponentBase(const CalendarComponentBase& copy)
		{
			mProperties = copy.mProperties;
		}
	};

}	// namespace iCal
