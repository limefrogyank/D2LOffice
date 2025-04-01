using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2LOffice.Models
{
    public class GroupedItem<TItem, TKey> : IGrouping<TKey, TItem>
    {
        public TKey Key { get; private set; }
        private IEnumerable<TItem> _items;

        public GroupedItem(IEnumerable<TItem> items, TKey key)
        {
            Key = key;
            _items = items;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
