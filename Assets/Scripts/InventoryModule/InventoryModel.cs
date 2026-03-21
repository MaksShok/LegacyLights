using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts.InventoryModule
{
    public class InventoryModel
    {
        public readonly int Capacity;
        
        public event Action OnChangeInventory;

        public IReadOnlyList<InventoryItem> Items => _items;
        private List<InventoryItem> _items;

        public InventoryModel(int capacity)
        {
            Capacity = capacity;
            _items = new List<InventoryItem>(Capacity);
        }

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count == Capacity)
                return false;
            
            _items.Add(item);
            OnChangeInventory?.Invoke();
            
            return true;
        }

        public void RemoveItem(InventoryItem removedItemType)
        {
            var removedItem = _items.Find(i => i.Id == removedItemType.Id);

            if (removedItem == null)
                return;
            
            _items.Remove(removedItem);
            OnChangeInventory?.Invoke();
        }
    }
}