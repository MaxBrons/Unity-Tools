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
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTools.Input;
using UnityTools.Inventory.UI;

namespace UnityTools.Inventory
{
    /// <summary>
    /// A specific class for managing and updating an item inventory and corresponding UI.
    /// </summary>
    public class ItemInventoryManager : MonoBehaviour
    {
        [Serializable]
        private struct SerializedInventoryItems
        {
            public InventoryItem Item;
            public int Amount;
        }

        /// <summary>
        /// This is the current selected item in the inventory.
        /// </summary>
        public static InventoryItem CurrentSelectedItem { get; private set; }

        [SerializeField] private ItemInventoryUI _inventoryUI;
        [SerializeField] private List<SerializedInventoryItems> _defaultInventoryItems = new();

        private IInventory<InventoryItem> _inventory;

        static ItemInventoryManager()
        {
            CurrentSelectedItem = null;
        }

        // Initialize the inventory and UI and bind to the inventory events.
        private void Awake()
        {
            _inventory = new Inventory<InventoryItem>();

            foreach (var item in _defaultInventoryItems) {
                _inventory.Add(item.Item, item.Amount);
            }
            _inventoryUI.UpdateUI(_inventory);

            _inventory.OnItemAdded += OnInventoryUpdated;
            _inventory.OnItemRemoved += OnInventoryUpdated;

            _inventoryUI.OnItemSelected += OnItemSelected;
            _inventoryUI.OnItemRemoveButtonPressed += OnItemRemoveButtonPressed;

            InputManager.AddListener(Inventory_OnToggle);
        }

        // Turn the inventory UI on/off.
        private void Inventory_OnToggle(InputAction.CallbackContext context)
        {
            if (context.performed) {
                _inventoryUI.ToggleVisible();
            }
        }

        // Remove the item from the inventory when the remove button is pressed.
        private void OnItemRemoveButtonPressed(InventoryItem item)
        {
            bool result = _inventory.Remove(item);
            if (result)
                _inventoryUI.UpdateUI(_inventory);
        }

        // Set the current selected item to the current selected inventory Slot.
        private void OnItemSelected(InventoryItem item)
        {
            CurrentSelectedItem = item;
        }

        // Update the inventory UI when an item is added or removed.
        private void OnInventoryUpdated(InventoryItem item, int amount)
        {
            _inventoryUI.UpdateUI(_inventory);
        }

        // Unbind from the inventory events when this object is destroyed.
        private void OnDestroy()
        {
            _inventory.OnItemAdded -= OnInventoryUpdated;
            _inventory.OnItemRemoved -= OnInventoryUpdated;

            _inventoryUI.OnItemSelected -= OnItemSelected;
            _inventoryUI.OnItemRemoveButtonPressed -= OnItemRemoveButtonPressed;

            InputManager.RemoveListener(Inventory_OnToggle);
            InputManager.Dinitialize();
        }
    }
}