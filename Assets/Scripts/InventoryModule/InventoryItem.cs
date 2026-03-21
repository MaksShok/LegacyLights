using InteractableEnvironmentModule;
using UnityEngine;

namespace InventoryModule
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/Items/InventoryItem")]
    public class InventoryItem : ItemType
    {
        [field: SerializeField]
        public Sprite IconSprite { get; private set; }
    }
}