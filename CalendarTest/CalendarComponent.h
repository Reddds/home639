#pragma once
#include "CalendarComponentBase.h"

#include "CalendarValue.h"

#include <stdint.h>
#include <iostream>
#include <vector>

//#include "cdstring.h"
#include "cdsharedptr.h"


using namespace std;

namespace iCalendar {

	class CalendarComponent;
	typedef vector<CalendarComponent*> CalendarComponentList;

	typedef uint32_t	CalendarRef;	// Unique reference to object
	class Calendar;

	class CalendarComponent : public CalendarComponentBase
	{
	public:
		enum EComponentType
		{
			eVEVENT,
			eVTODO,
			eVJOURNAL,
			eVFREEBUSY,
			eVTIMEZONE,
			eVALARM,

			// Pseudo components
			eVTIMEZONESTANDARD,
			eVTIMEZONEDAYLIGHT
		};

		typedef CalendarComponent* (*CreateComponentPP)(const CalendarRef& calendar);

		CalendarComponent(const CalendarRef& calendar)
		{
			mCalendarRef = calendar; mSeq = 0; mOriginalSeq = 0; mEmbedder = NULL; mEmbedded = NULL; mChanged = false;
		}
		CalendarComponent(const CalendarComponent& copy) :
			CalendarComponentBase(copy)
		{
			mEmbedder = NULL; mEmbedded = NULL; mChanged = false; _copy_CalendarComponent(copy);
		}
		virtual ~CalendarComponent();

		CalendarComponent& operator=(const CalendarComponent& copy)
		{
			if (this != &copy)
			{
				_copy_CalendarComponent(copy);
				CalendarComponentBase::operator=(copy);
			}
			return *this;
		}

		virtual CalendarComponent* clone() const = 0;

		virtual EComponentType GetType() const = 0;
		virtual const cdstring& GetBeginDelimiter() const = 0;
		virtual const cdstring& GetEndDelimiter() const = 0;
		virtual cdstring GetMimeComponentName() const = 0;

		virtual bool AddComponent(CalendarComponent* comp);
		void RemoveComponent(CalendarComponent* comp);
		bool HasEmbeddedComponent(EComponentType type) const;
		CalendarComponent* GetFirstEmbeddedComponent(EComponentType type) const;
		void SetEmbedder(CalendarComponent* embedder)
		{
			mEmbedder = embedder;
		}
		const CalendarComponent* GetEmbedder() const
		{
			return mEmbedder;
		}

		void SetCalendar(const CalendarRef& ref)
		{
			mCalendarRef = ref;
		}
		const CalendarRef& GetCalendar() const
		{
			return mCalendarRef;
		}

		virtual const cdstring& GetMapKey() const
		{
			return mUID;
		}
		virtual cdstring GetMasterKey() const
		{
			return mUID;
		}

		cdstring& GetUID()
		{
			return mUID;
		}
		const cdstring& GetUID() const
		{
			return mUID;
		}
		virtual void SetUID(const cdstring& uid);

		int32_t& GetSeq()
		{
			return mSeq;
		}
		const int32_t& GetSeq() const
		{
			return mSeq;
		}
		void SetSeq(const int32_t& seq);

		const int32_t& GetOriginalSeq() const
		{
			return mOriginalSeq;
		}

		const cdstring& GetRURL() const
		{
			return mRURL;
		}
		void SetRURL(const cdstring& rurl)
		{
			mRURL = rurl;
		}
		void GenerateRURL();

		const cdstring& GetETag() const
		{
			return mETag;
		}
		void SetETag(const cdstring& etag)
		{
			mETag = etag;
		}

		bool GetChanged() const
		{
			return mChanged;
		}
		void SetChanged(bool changed)
		{
			mChanged = changed;
		}

		virtual void InitDTSTAMP();
		virtual void UpdateLastModified();

		virtual void Added();
		virtual void Removed();
		virtual void Duplicated();
		virtual void Changed();

		virtual void Finalise();

		virtual bool CanGenerateInstance() const
		{
			return true;
		}
		virtual void Generate(ostream& os, bool for_cache = false) const;

		virtual void GetTimezones(cdstrset& tzids) const;

	protected:
		CalendarRef				mCalendarRef;
		cdstring					mUID;
		int32_t						mSeq;
		int32_t						mOriginalSeq;
		CalendarComponent*		mEmbedder;
		CalendarComponentList*	mEmbedded;

		// CalDAV stuff
		cdstring					mRURL;
		cdstring					mETag;
		bool						mChanged;

	private:
		void	_copy_CalendarComponent(const CalendarComponent& copy);
	};

}	// namespace iCal