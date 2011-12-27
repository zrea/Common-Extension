using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateTimeExtensions
    {
        public static DateTimeConfiguration FirstDate(this DateTime dt)
        {
            return new FirstDateConfiguration(dt);
        }

        public static DateTimeConfiguration LastDate(this DateTime dt)
        {
            return new LastDateConfiguration(dt);
        }
    }

    public class FirstDateConfiguration : DateTimeConfiguration
    {
        internal FirstDateConfiguration(DateTime dt)
            : base(dt)
        {
            Day = 1;
        }
    }

    public class LastDateConfiguration : DateTimeConfiguration
    {
        internal LastDateConfiguration(DateTime dt)
            : base(dt)
        {

        }

        protected override DateTime CreateDateTime(bool isAdjust)
        {
            Adjust();

            Day = DateTime.DaysInMonth(Year, Month);

            return base.CreateDateTime(false);
        }
    }

    public class DateTimeConfiguration
    {
        protected int Year { get; set; }
        protected int Month { get; set; }
        protected int Day { get; set; }
        protected int Minute { get; set; }
        protected int Second { get; set; }
        protected int Hour { get; set; }
        protected int Millisecond { get; set; }

        internal DateTimeConfiguration(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Minute = dt.Minute;
            Hour = dt.Hour;
            Second = dt.Second;
            Millisecond = dt.Millisecond;
        }

        public virtual DateTime OfNextMonth()
        {
            Month++;

            return CreateDateTime();
        }

        public virtual DateTime OfLastMonth()
        {
            Month--;

            return CreateDateTime();
        }

        public virtual DateTime OfThisMonth()
        {
            return CreateDateTime();
        }

        public virtual DateTime OfMonth(int month)
        {
            Month = month;

            return CreateDateTime();
        }

        protected virtual DateTime CreateDateTime(bool useAdjust = true)
        {
            if (useAdjust)
                Adjust();

            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
        }

        protected void Adjust()
        {
            if (Month > 12)
            {
                Year += Month / 12;
                Month %= 12;
            }

            while (Month < 1)
            {
                Month += 12;
                Year--;
            }
        }
    }
}
