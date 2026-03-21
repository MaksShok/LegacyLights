using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InventoryModule
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] 
        private Image _iconImage;

        public void UpdateIcon(Sprite newIconSprite)
        {
            ApplyTransparency(1);
            _iconImage.sprite = newIconSprite;
        }

        public void DeleteIcon()
        {
            ApplyTransparency(0);
            _iconImage.sprite = default;
        }
        
        private void ApplyTransparency(float a)
        {
            Color color = _iconImage.color;
            color.a = a;
            _iconImage.color = color;
        }
    }
}