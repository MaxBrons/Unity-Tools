using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTools.Inventory.UI
{
    [Serializable]
    public class ItemInventoryUI : IInventoryUI<Inventory.InventoryItem>
    {
        [SerializeField] private List<InventoryItem> _inventoryItems = new();

        public void UpdateUI(IInventory<Inventory.InventoryItem> inventory)
        {
            if (inventory != null) {
                var items = inventory.GetItems().Keys.ToArray();
                var counts = inventory.GetItems().Values.ToArray();

                for (int i = 0; i < _inventoryItems.Count; i++) {
                    if (i < items.Length) {
                        _inventoryItems[i].UpdateItem(items[i].Icon, counts[i]);
                        continue;
                    }
                    _inventoryItems[i].UpdateItem(null, 0);
                }
            }
        }
    }
}