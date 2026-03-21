using _Project.Scripts.InventoryModule;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.InteractableEnvironmentModule.InteractableActions
{
    public class TakeInventoryItemAction : BaseInteractableAction
    {
        [Inject] 
        private InventoryModel _inventoryModel;

        public InventoryItem Item { get; private set; }

        public void Init(InventoryItem inventoryItem)
        {
            Item = inventoryItem;
        }
        
        public override void Action()
        {
            if (Item != null)
                _inventoryModel.AddItem(Item);
            else
            {
                Debug.Log($"Inventory Item is null");
            }
        }
    }
}