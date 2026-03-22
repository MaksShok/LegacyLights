using CommonLogic.HealthModule;
using InventoryModule;
using Player;
using UnityEngine;


namespace _main.ServiceLoc
{
    
    public class ServiceLocator_Game : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Player.Player _player;
        
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            RegisterObjects();
        }

        private void RegisterObjects()
        {
            ServiceLocator.Initialize();
            
            InventoryModel weaponInventory = new InventoryModel(_playerConfig.WeaponInventoryCapacity);
            InventoryModel abilitiesInventory = new InventoryModel(_playerConfig.AbilitiesInventoryCapacity);
            HealthModel playerHealth = new HealthModel(100);
            
            ServiceLocator.Current.Register(_player);
            ServiceLocator.Current.Register(new InputController());
            ServiceLocator.Current.Register<ISpendHealth>(playerHealth);
            ServiceLocator.Current.Register<IHealth>(playerHealth);
            ServiceLocator.Current.Register(weaponInventory);
            //ServiceLocator.Current.Register(abilitiesInventory);
            
            _player.HealthBar.Initialize(playerHealth);
        }
    }
}