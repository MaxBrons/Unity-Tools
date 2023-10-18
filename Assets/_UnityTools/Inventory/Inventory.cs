// MIT License
//
// Copyright (c) 2023 Max Bronstring
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;

namespace UnityTools.Inventory
{
    /// <summary>
    /// An interface for default inventory functionality.
    /// </summary>
    /// <typeparam name="T" />
    public interface IInventory<T>
    {
        public event Action<T> OnItemAdded;
        public event Action<T> OnItemRemoved;

        public bool Add(T item);
        public bool Remove(T item);
        public Dictionary<T, int> GetItems();
    }

    /// <summary>
    /// A generic inventory class that stores a list of items and can add/remove items.
    /// </summary>
    /// <typeparam name="T" />
    public class Inventory<T> : IInventory<T>
    {
        public virtual event Action<T> OnItemAdded;
        public virtual event Action<T> OnItemRemoved;

        protected Dictionary<T, int> _items = new();

        /// <summary>
        /// Add an item to the inventory. <br />
        /// If the item already exists, the count will be increased.
        /// </summary>
        /// <param name="item" />
        /// <returns>True if the item was succesfully added.</returns>
        public virtual bool Add(T item)
        {
            if (item != null) {
                if (_items.ContainsKey(item))
                    _items[item]++;
                else
                    _items.Add(item, 1);

                OnItemAdded?.Invoke(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an item from the inventory. <br />
        /// It will decrease the item's count first untill it reaches zero. <br /> 
        /// Then it will remove the item.
        /// </summary>
        /// <param name="item" />
        /// <returns>True if the item was succesfully removed.</returns>
        public virtual bool Remove(T item)
        {
            if (item != null && _items.ContainsKey(item)) {
                if (_items[item] <= 1)
                    _items.Remove(item);
                else
                    _items[item]--;

                OnItemRemoved?.Invoke(item);
            }
            return false;
        }

        public Dictionary<T, int> GetItems() => _items;
    }
}
