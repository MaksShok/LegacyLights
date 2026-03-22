using InventoryModule;
using Misc.Collisions;
using UnityEngine;

namespace InteractableEnvironmentModule.InteractableActions
{
    public class TakeItemAction : BaseInteractableAction
    {
        [SerializeField] 
        private TriggerCollisionEvent _triggerEvent;
        
        
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