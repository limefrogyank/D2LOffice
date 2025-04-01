using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Components.DataGrid
{
    // <summary>
    /// Holds state to represent filters in a <see cref="FluentDataGrid{TGridItem}"/>.
    /// </summary>
    public class FilterState
    {
        /// <summary>
        /// Read-Only View of Column Filters.
        /// </summary>
        public IReadOnlyDictionary<string, FilterDescriptor> Filters => _filters;

        /// <summary>
        /// Column Filters.
        /// </summary>
        private readonly ConcurrentDictionary<string, FilterDescriptor> _filters = new();

        /// <summary>
        /// Invoked, when the list of filters change.
        /// </summary>
        public EventCallback<FilterState> CurrentFiltersChanged { get; } = new();

        /// <summary>
        /// Applies a Filter.
        /// </summary>
        /// <param name="filter"></param>
        public Task AddFilterAsync(FilterDescriptor filter)
        {
            _filters[filter.PropertyName] = filter;

            return CurrentFiltersChanged.InvokeAsync(this);
        }

        /// <summary>
        /// Removes a Filter.
        /// </summary>
        /// <param name="propertyName"></param>
        public Task RemoveFilterAsync(string propertyName)
        {
            _filters.Remove(propertyName, out var _);

            return CurrentFiltersChanged.InvokeAsync(this);
        }
    }
}
