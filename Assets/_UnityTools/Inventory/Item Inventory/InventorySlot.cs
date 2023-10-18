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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTools.Inventory.UI
{
    /// <summary>
    /// A simple class displaying the inventory item properties.
    /// </summary>
    public class InventorySlot : MonoBehaviour
    {
        public event Action<InventoryItem> OnSlotSelected;
        public event Action<InventoryItem> OnRemoveButtonPressed;

        [SerializeField] private Button _itemButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Image _removeButtonImage;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private InventoryItem _inventoryItem;

        private void Awake()
        {
            // Bind to the on click events of the buttons.
            _itemButton.onClick.AddListener(() => OnSlotSelected?.Invoke(_inventoryItem));
            _removeButton.onClick.AddListener(() => OnRemoveButtonPressed?.Invoke(_inventoryItem));
        }

        public void UpdateSlot(InventoryItem inventoryItem, int count)
        {
            _inventoryItem = inventoryItem;

            // Validate the given data.
            if (_inventoryItem != null && count > 0) {
                _icon.sprite = _inventoryItem.Icon;
                _countText.text = count.ToString();
            }

            // Enable or disable the components based on the
            // validity of the given data.
            bool enabled = _inventoryItem != null && count > 0;
            _icon.enabled = enabled;
            _removeButtonImage.enabled = enabled;
            _countText.enabled = enabled;
            _itemButton.enabled = enabled;
        }

        private void OnDestroy()
        {
            // Unbind from the on click events of the buttons.
            _itemButton.onClick.RemoveAllListeners();
            _removeButton.onClick.RemoveAllListeners();
        }
    }
}