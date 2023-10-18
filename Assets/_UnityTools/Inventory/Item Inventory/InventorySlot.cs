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
        [SerializeField] private Button _itemButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        public void UpdateSlot(Sprite icon, int count)
        {
            // Validate the given data.
            if (icon != null && count > 0) {
                _icon.sprite = icon;
                _countText.text = count.ToString();
            }

            // Enable or disable the components based on the
            // validity of the given data.
            bool enabled = icon != null && count > 0;
            _icon.enabled = enabled;
            _removeButton.enabled = enabled;
            _countText.enabled = enabled;
            _itemButton.enabled = enabled;
        }
    }
}