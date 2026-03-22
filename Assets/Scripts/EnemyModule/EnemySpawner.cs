using _main.ServiceLoc;
using CommonLogic.HealthModule;
using EnemyModule.Abstract;
using UnityEngine;

namespace EnemyModule
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] 
        private EnemyBehavior _enemyPrefab;

        private Transform _playerTransform;
        private ISpendHealth _playerSpendHealth;

        private void Start()
        {
            _playerSpendHealth = ServiceLocator.Current.Get<ISpendHealth>();
            
            // Сделать через ServiceLocator
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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

            EnemyBehavior enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            enemy.Initialize(_playerTransform, _playerSpendHealth);
        }
    }
}
