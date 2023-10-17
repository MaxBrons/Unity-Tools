using System;
using System.Collections.Generic;

namespace UnityTools.Inventory
{
    public interface IInventory<T>
    {
        public event Action<T> OnItemAdded;
        public event Action<T> OnItemRemoved;

        public bool Add(T item);
        public bool Remove(T item);
        public Dictionary<T, int> GetItems();
    }

    public class Inventory<T> : IInventory<T>
    {
        public event Action<T> OnItemAdded;
        public event Action<T> OnItemRemoved;

        protected Dictionary<T, int> _items = new();

        public virtual bool Add(T item)
        {
            if (item != null) {
                if (_items.ContainsKey(item))
                    _items[item]++;
                else
                    _items.Add(item, 1);

                InvokeOnItemAdded(item);
                return true;
            }
            return false;
        }

        public virtual bool Remove(T item)
        {
            if (item != null && _items.ContainsKey(item)) {
                if (_items[item] <= 1)
                    _items.Remove(item);
                else
                    _items[item]--;

                InvokeOnItemRemoved(item);
            }
            return false;
        }

        protected void InvokeOnItemAdded(T item)
        {
            OnItemRemoved?.Invoke(item);
        }

        protected void InvokeOnItemRemoved(T item)
        {
            OnItemRemoved?.Invoke(item);
        }

        public Dictionary<T, int> GetItems() => _items;
    }
}
