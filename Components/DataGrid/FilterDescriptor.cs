using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Components.DataGrid
{
    public abstract class FilterDescriptor
    {
        /// <summary>
        /// Gets or sets the Property to filter.
        /// </summary>
        public required string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the Filter Operator.
        /// </summary>
        public required FilterOperatorEnum FilterOperator { get; set; }

        /// <summary>
        /// Gets or sets the Filter Type.
        /// </summary>
        public abstract FilterTypeEnum FilterType { get; }
    }

    public class StringFilterDescriptor : FilterDescriptor
    {
        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets the Filter Type.
        /// </summary>
        public override FilterTypeEnum FilterType => FilterTypeEnum.StringFilter;
    }
}
