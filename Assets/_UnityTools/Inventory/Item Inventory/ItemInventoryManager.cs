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

using UnityEngine;
using UnityTools.Inventory.UI;

namespace UnityTools.Inventory
{
    /// <summary>
    /// A specific class for managing and updating an item inventory and corresponding UI.
    /// </summary>
    public class ItemInventoryManager : MonoBehaviour
    {
        [SerializeField] private ItemInventoryUI _inventoryUI;

        private IInventory<InventoryItem> _inventory;

        // Initialize the inventory and UI and bind to the inventory events.
        private void Awake()
        {
            _inventory = new Inventory<InventoryItem>();
            _inventory.OnItemAdded += OnInventoryUpdate;
            _inventory.OnItemRemoved += OnInventoryUpdate;

            _inventoryUI.UpdateUI(_inventory);
        }

        // Update the inventory UI when an item is added or removed.
        private void OnInventoryUpdate(InventoryItem item)
        {
            _inventoryUI.UpdateUI(_inventory);
        }

        // Unbind from the inventory events when this object is destroyed.
        private void OnDestroy()
        {
            _inventory.OnItemAdded -= OnInventoryUpdate;
            _inventory.OnItemRemoved -= OnInventoryUpdate;
        }
    }
}