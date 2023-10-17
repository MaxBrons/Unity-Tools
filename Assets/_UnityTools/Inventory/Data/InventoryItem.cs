using UnityEngine;

namespace UnityTools.Inventory
{
    [CreateAssetMenu(menuName = "Custom/Inventory/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public string Name => _name;
        public Sprite Icon => _icon;

        [SerializeField] private string _name = string.Empty;
        [SerializeField] private Sprite _icon = null;

        public InventoryItem Clone()
        {
            var instance = CreateInstance<InventoryItem>();
            instance._name = _name;
            instance._icon = _icon;
            return instance;
        }
    }
}