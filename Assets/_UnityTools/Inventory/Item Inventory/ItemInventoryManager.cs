using UnityEngine;
using UnityTools.Inventory.UI;

namespace UnityTools.Inventory
{
    public class ItemInventoryManager : MonoBehaviour
    {

        [SerializeField] private ItemInventoryUI _inventoryUI;

        private IInventory<InventoryItem> _inventory;

        private void Awake()
        {
            _inventory = new Inventory<InventoryItem>();
            _inventory.OnItemAdded += OnInventoryUpdate;
            _inventory.OnItemRemoved += OnInventoryUpdate;

            _inventoryUI.UpdateUI(_inventory);
        }

        private void OnInventoryUpdate(InventoryItem item)
        {
            _inventoryUI.UpdateUI(_inventory);
        }

        private void OnDestroy()
        {
            _inventory.OnItemAdded -= OnInventoryUpdate;
            _inventory.OnItemRemoved -= OnInventoryUpdate;
        }
    }
}