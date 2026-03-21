using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InventoryModule
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private LayoutGroup _container;
        [SerializeField] private InventoryItemView _itemViewPrefab;
        
        private List<InventoryItemView> _itemViews;
        private InventoryModel _model;

        public void InitializeInventoryModel(InventoryModel model)
        {
            _model = model;
            _itemViews = new List<InventoryItemView>(_model.Capacity);

            for (int i = 0; i < _model.Capacity; i++)
            {
                var itemView = Instantiate(_itemViewPrefab, _container.transform);
                _itemViews.Add(itemView);
            } 
            
            UpdateItemViews();

            _model.OnChangeInventory += UpdateItemViews;
        }

        private void UpdateItemViews()
        {
            int i;
            
            for (i = 0; i < _model.Items.Count; i++)
            {
                var view = _itemViews[i];
                var data = _model.Items[i];
                
                view.UpdateIcon(data.IconSprite);
            }

            if (i == _model.Capacity - 1)
                return;

            for (int j = i; j < _model.Capacity; j++)
            {
                _itemViews[j].DeleteIcon();
            }
        }

        private void OnDestroy()
        {
            _model.OnChangeInventory -= UpdateItemViews;
        }
    }
}