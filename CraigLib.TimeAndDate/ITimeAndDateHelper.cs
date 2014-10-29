using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.TimeAndDate
{
    public interface ITimeAndDateHelper
    {
        int NumberMonthsBetweenDates(DateTime beginningDate, DateTime endingDate);
    }
}
