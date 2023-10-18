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
using System.Linq;
using UnityEngine;

namespace UnityTools.Inventory.UI
{
    /// <summary>
    /// A class for displaying and updating all corresponding inventory UI items.
    /// </summary>
    [Serializable]
    public class ItemInventoryUI : IInventoryUI<InventoryItem>
    {
        [SerializeField] private List<InventorySlot> _inventoryItems = new();

        /// <summary>
        /// Update all the slots with the inventory items. <br />
        /// If there are more slots than inventory items, the remaining 
        /// slot will be cleared.
        /// </summary>
        /// <param name="inventory" />
        public void UpdateUI(IInventory<InventoryItem> inventory)
        {
            if (inventory == null)
                return;

            var items = inventory.GetItems().Keys.ToArray();
            var counts = inventory.GetItems().Values.ToArray();
            for (int i = 0; i < _inventoryItems.Count; i++) {
                if (i < items.Length) {
                    _inventoryItems[i].UpdateSlot(items[i].Icon, counts[i]);
                    continue;
                }
                _inventoryItems[i].UpdateSlot(null, 0);
            }
        }
    }
}