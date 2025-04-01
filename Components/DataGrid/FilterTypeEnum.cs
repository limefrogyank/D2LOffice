using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Components.DataGrid
{
    public enum FilterTypeEnum
    {
        None = 0,
        BooleanFilter = 1,
        StringFilter = 2,
        NumericFilter = 3,
        DateFilter = 4,
        DateTimeFilter = 5,
    }
}
