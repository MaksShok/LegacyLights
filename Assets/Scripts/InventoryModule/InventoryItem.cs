using _Project.Scripts.InteractableEnvironmentModule;
using UnityEngine;

namespace _Project.Scripts.InventoryModule
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/Items/InventoryItem")]
    public class InventoryItem : ItemType
    {
        [field: SerializeField]
        public Sprite IconSprite { get; private set; }
    }
}