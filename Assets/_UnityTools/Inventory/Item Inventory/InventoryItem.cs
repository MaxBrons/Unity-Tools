using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTools.Inventory.UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Button _itemButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        public void UpdateItem(Sprite icon, int count)
        {
            if (icon != null && count > 0) {
                _icon.sprite = icon;
                _countText.text = count.ToString();
            }

            bool enabled = icon != null && count > 0;
            _icon.enabled = enabled;
            _removeButton.enabled = enabled;
            _countText.enabled = enabled;
            _itemButton.enabled = enabled;
        }
    }
}