using _main.ServiceLoc;
using CommonLogic.HealthModule;
using EnemyModule.Abstract;
using UnityEngine;

namespace SpawnModule
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] 
        private EnemyBehavior _enemyPrefab;

        [SerializeField] 
        private int _spawnCount = 3;

        private int _spawned = 0;
        private EnemyBehavior _currentSpawned;
        private Transform _playerTransform;
        private ISpendHealth _playerSpendHealth;

        private void Start()
        {
            _playerSpendHealth = ServiceLocator.Current.Get<ISpendHealth>();
            
            var player = ServiceLocator.Current.Get<Player.Player>();
            _playerTransform = player.transform;
            
            SpawnEnemy();
        }
        
        public void SpawnEnemy()
        {
            if (_enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] Префаб врага не задан!");
                return;
            }
            
            if (_currentSpawned != null) _currentSpawned.Health.Die -= SpawnEnemy;
            if (_spawned == _spawnCount) return;
            
            _currentSpawned = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            _currentSpawned.Initialize(_playerTransform, _playerSpendHealth);
            _currentSpawned.Health.Die += SpawnEnemy;
            _spawned++;
        }

        public void ResetSpawning()
        {
            _spawned = 0;
            SpawnEnemy();
        }
    }
}
