#pragma once
#include "CalendarComponent.h"

#include "CalendarDateTime.h"

#include "cdsharedptr.h"
#include "ptrvector.h"

namespace iCalendar {

	class CalendarRecurrenceSet;

	class CalendarComponentExpanded;
	typedef cdsharedptr<CalendarComponentExpanded> CalendarComponentExpandedShared;
	typedef vector<CalendarComponentExpandedShared> CalendarExpandedComponents;

	class CalendarComponentRecur;
	typedef vector<CalendarComponentRecur*> CalendarComponentRecurs;

	class CalendarComponentRecur : public CalendarComponent
	{
	public:

		static cdstring MapKey(const cdstring& uid);
		static cdstring MapKey(const cdstring& uid, const cdstring& rid);

		static bool sort_by_dtstart_allday(CalendarComponentRecur* e1, CalendarComponentRecur* e2);
		static bool sort_by_dtstart(CalendarComponentRecur* e1, CalendarComponentRecur* e2);

		CalendarComponentRecur(const CalendarRef& calendar);
		CalendarComponentRecur(const CalendarComponentRecur& copy);
		virtual ~CalendarComponentRecur()
		{
			_tidy_CalendarComponentRecur();
		}

		CalendarComponentRecur& operator=(const CalendarComponentRecur& copy)
		{
			if (this != &copy)
			{
				_tidy_CalendarComponentRecur();
				_copy_CalendarComponentRecur(copy);
				CalendarComponent::operator=(copy);
			}
			return *this;
		}

		virtual bool CanGenerateInstance() const
		{
			return !mHasRecurrenceID;
		}

		bool Recurring() const
		{
			return (mMaster != NULL) && (mMaster != this);
		}
		void SetMaster(CalendarComponentRecur* master);
		CalendarComponentRecur* GetMaster()
		{
			return mMaster;
		}
		const CalendarComponentRecur* GetMaster() const
		{
			return mMaster;
		}

		virtual const cdstring& GetMapKey() const
		{
			return mMapKey;
		}
		virtual cdstring GetMasterKey() const;

		virtual void InitDTSTAMP();

		const CalendarDateTime& GetStamp() const
		{
			return mStamp;
		}
		bool HasStamp() const
		{
			return mHasStamp;
		}

		const CalendarDateTime& GetStart() const
		{
			return mStart;
		}
		bool HasStart() const
		{
			return mHasStart;
		}

		const CalendarDateTime& GetEnd() const
		{
			return mEnd;
		}
		bool HasEnd() const
		{
			return mHasEnd;
		}

		bool UseDuration() const
		{
			return mDuration;
		}

		bool IsRecurrenceInstance() const
		{
			return mHasRecurrenceID;
		}
		bool IsAdjustFuture() const
		{
			return mAdjustFuture;
		}
		bool IsAdjustPrior() const
		{
			return mAdjustPrior;
		}
		const CalendarDateTime& GetRecurrenceID() const
		{
			return mRecurrenceID;
		}

		bool IsRecurring() const;

		CalendarRecurrenceSet* GetRecurrenceSet()
		{
			return mRecurrences;
		}
		const CalendarRecurrenceSet* GetRecurrenceSet() const
		{
			return mRecurrences;
		}

		virtual void SetUID(const cdstring& uid);

		const cdstring& GetSummary() const
		{
			return mSummary;
		}
		void SetSummary(const cdstring& summary)
		{
			mSummary = summary;
		}

		cdstring GetDescription() const;
		cdstring GetLocation() const;

		bool	 GetTransparent() const;

		virtual void Finalise();

		void ExpandPeriod(const CalendarPeriod& period, CalendarExpandedComponents& list);

		bool WithinPeriod(const CalendarPeriod& period) const;

		void ChangedRecurrence();

		// Editing
		void EditSummary(const cdstring& summary);
		void EditDetails(const cdstring& description, const cdstring& location);
		void EditTransparent(bool transparent);
		void EditTiming();
		void EditTiming(const CalendarDateTime& due);
		void EditTiming(const CalendarDateTime& start, const CalendarDateTime& end);
		void EditTiming(const CalendarDateTime& start, const CalendarDuration& durtaion);
		void EditRecurrenceSet(const CalendarRecurrenceSet& recurs);
		void ExcludeRecurrence(const CalendarDateTime& start);
		void ExcludeFutureRecurrence(const CalendarDateTime& start);

	protected:
		CalendarComponentRecur*	mMaster;		// Component to inherit properties from if none exist in this component

		cdstring				mMapKey;

		cdstring				mSummary;

		CalendarDateTime		mStamp;
		bool					mHasStamp;

		CalendarDateTime		mStart;
		bool					mHasStart;
		CalendarDateTime		mEnd;
		bool					mHasEnd;
		bool					mDuration;

		bool					mHasRecurrenceID;
		bool					mAdjustFuture;
		bool					mAdjustPrior;
		CalendarDateTime		mRecurrenceID;

		CalendarRecurrenceSet*			mRecurrences;

		// These are overridden to allow missing properties to come from the master component
		virtual bool	LoadValue(const char* value_name, int32_t& value, CalendarValue::EICalValueType type = CalendarValue::eValueType_Integer) const;
		virtual bool	LoadValue(const char* value_name, cdstring& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarDateTime& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarDuration& value) const;
		virtual bool	LoadValue(const char* value_name, CalendarPeriod& value) const;
		virtual bool	LoadValueRRULE(const char* value_name, CalendarRecurrenceSet& value, bool add) const;
		virtual bool	LoadValueRDATE(const char* value_name, CalendarRecurrenceSet& value, bool add) const;

		void	InitFromMaster();

		CalendarComponentExpandedShared	CreateExpanded(CalendarComponentRecur* master, const CalendarDateTime& recurid);

	private:
		void	_copy_CalendarComponentRecur(const CalendarComponentRecur& copy);
		void	_tidy_CalendarComponentRecur();

		void	FixStartEnd();
	};

}	// namespace iCal


