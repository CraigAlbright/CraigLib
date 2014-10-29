using System;

namespace CraigLib.TimeAndDate
{
    public class TimeAndDateHelper : ITimeAndDateHelper
    {
        /// <summary>
        /// This is a quick and slightly inaccurate way to get the number of months in a date range. Follows
        /// the LinkedIn model of "months" at a job.
        /// </summary>
        /// <param name="beginningDate"></param>
        /// <param name="endingDate"></param>
        /// <returns></returns>
        public int NumberMonthsBetweenDates(DateTime beginningDate, DateTime endingDate)
        {
            if (beginningDate.Year == endingDate.Year)
            {
                return Math.Abs(endingDate.Month - beginningDate.Month + 1);
            }

            if (endingDate.Year > beginningDate.Year)
            {
                var monthOffset = (endingDate.Year - beginningDate.Year)*12;
                return Math.Abs((endingDate.Month - beginningDate.Month) + monthOffset);
            }

            var backwardsMonthOffset = (beginningDate.Year - endingDate.Year) * 12;
            return Math.Abs((endingDate.Month - beginningDate.Month) + backwardsMonthOffset);
        }

    }
}
