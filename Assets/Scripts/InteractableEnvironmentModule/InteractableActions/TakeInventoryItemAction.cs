using InventoryModule;
using UnityEngine;

namespace InteractableEnvironmentModule.InteractableActions
{
    public class TakeInventoryItemAction : BaseInteractableAction
    {
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