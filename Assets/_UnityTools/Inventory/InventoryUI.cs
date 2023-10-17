namespace UnityTools.Inventory.UI
{
    public interface IInventoryUI<T>
    {
        public void UpdateUI(IInventory<T> inventory);
    }
}