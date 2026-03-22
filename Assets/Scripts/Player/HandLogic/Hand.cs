using System;
using _main.ServiceLoc;
using InteractableEnvironmentModule;
using InventoryModule;
using UnityEngine;

namespace Player.HandLogic
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] 
        private Transform _handPosition;

        private InventoryModel _inventory;
        private ItemType _currentItemInHand;

        private void Start()
        {
            _inventory = ServiceLocator.Current.Get<InventoryModel>();
        }

        private void Take()
        {
            
        }

        private void Throw()
        {
            
        }
    }
}