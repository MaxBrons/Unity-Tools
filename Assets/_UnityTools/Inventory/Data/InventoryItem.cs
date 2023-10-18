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

namespace UnityTools.Inventory
{
    /// <summary>
    /// This scriptable object is used to store the properties used for 
    /// the UI visualization of the inventory items.
    /// </summary>
    [CreateAssetMenu(menuName = "Custom/Inventory/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public string Name => _name;
        public Sprite Icon => _icon;

        [SerializeField] private string _name = string.Empty;
        [SerializeField] private Sprite _icon = null;

        /// <summary>
        /// Create an instance of this scriptable object with the same values.
        /// </summary>
        /// <returns />
        public InventoryItem Clone()
        {
            var instance = CreateInstance<InventoryItem>();
            instance._name = _name;
            instance._icon = _icon;
            return instance;
        }
    }
}