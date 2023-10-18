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
    /// This class is used to store the item and the count as
    /// a managed KeyValuePair for the inventory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InventoryEntry<T>
    {
        public readonly T Item;
        public int Count;

        public InventoryEntry(T item, int amount)
        {
            Item = item;
            Count = amount;
        }

        public InventoryEntry<T> Clone()
        {
            return new InventoryEntry<T>(Item, Count);
        }
    }

    /// <summary>
    /// An interface for default inventory functionality.
    /// </summary>
    /// <typeparam name="T" />
    public interface IInventory<T>
    {
        public event Action<T, int> OnItemAdded;
        public event Action<T, int> OnItemRemoved;

        public bool Add(T item, int amount = 1);
        public bool Remove(T item, int amount = 1);
        public IEnumerable<InventoryEntry<T>> GetItems();
    }

    /// <summary>
    /// A generic inventory class that stores a list of items and can add/remove items.
    /// </summary>
    /// <typeparam name="T" />
    public class Inventory<T> : IInventory<T>
    {
        public virtual event Action<T, int> OnItemAdded;
        public virtual event Action<T, int> OnItemRemoved;

        protected List<InventoryEntry<T>> _items = new();

        /// <summary>
        /// Add an item to the inventory. <br />
        /// If the item already exists, the count will be increased.
        /// </summary>
        /// <param name="item" />
        /// <returns>True if the item was succesfully added.</returns>
        public virtual bool Add(T item, int amount = 1)
        {
            if (item != null) {
                int validAmount = Math.Max(amount, 1);
                var inventoryItem = _items.Find(x => x.Item.Equals(item));

                if (inventoryItem != null)
                    inventoryItem.Count += validAmount;
                else
                    _items.Add(new(item, validAmount));

                OnItemAdded?.Invoke(item, validAmount);
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
        public virtual bool Remove(T item, int amount = 1)
        {
            if (item == null)
                return false;

            var inventoryItem = _items.Find(x => x.Item.Equals(item));
            if (inventoryItem == null)
                return false;

            int validAmount = Math.Max(amount, 1);

            if (inventoryItem.Count - validAmount >= 0) {
                inventoryItem.Count -= validAmount;

                if (inventoryItem.Count == 0)
                    _items.Remove(inventoryItem);

                OnItemRemoved?.Invoke(item, validAmount);
                return true;
            }
            return false;
        }

        public IEnumerable<InventoryEntry<T>> GetItems()
        {
            List<InventoryEntry<T>> itemListCopy = new(_items.Count);

            for (int i = 0; i < _items.Count; i++) {
                itemListCopy.Add(_items[i].Clone());
            }
            return itemListCopy;
        }
    }
}
