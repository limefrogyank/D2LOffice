using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Components.DataGrid
{
    public enum FilterOperatorEnum
    {
        None = 0,
        Before = 1,
        After = 2,
        IsEqualTo = 3,
        IsNotEqualTo = 4,
        Contains = 5,
        NotContains = 6,
        StartsWith = 7,
        EndsWith = 8,
        IsNull = 9,
        IsNotNull = 10,
        IsEmpty = 11,
        IsNotEmpty = 12,
        IsGreaterThanOrEqualTo = 13,
        IsGreaterThan = 14,
        IsLessThanOrEqualTo = 15,
        IsLessThan = 16,
        BetweenInclusive = 17,
        BetweenExclusive = 18,
        Yes = 19,
        No = 20,
        All = 21
    }
}
