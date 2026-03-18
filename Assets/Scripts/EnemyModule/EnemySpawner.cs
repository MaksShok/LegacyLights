using EnemyModule.Abstract;
using UnityEngine;

namespace EnemyModule
{
    /// <summary>
    /// Спавнер врагов. Автоматически инициализирует врага при создании.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Настройки спавна")]
        [SerializeField] private EnemyBehavior _enemyPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _spawnPoint2;

        private Transform _playerTransform;

        private void Start()
        {
            // Находим игрока на сцене
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("[EnemySpawner] Игрок не найден! Враги не смогут атаковать.");
            }

            // Спавним двух врагов если префаб задан
            if (_enemyPrefab != null)
            {
                SpawnEnemies();
            }
        }

        /// <summary>
        /// Создать двух врагов в точках спавна
        /// </summary>
        public void SpawnEnemies()
        {
            if (_enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] Префаб врага не задан!");
                return;
            }

            // Спавн первого врага
            Vector3 spawnPosition1 = _spawnPoint != null ? _spawnPoint.position : transform.position;
            EnemyBehavior enemy1 = Instantiate(_enemyPrefab, spawnPosition1, Quaternion.identity);
            enemy1.Initialize(_playerTransform, null);
            Debug.Log($"[EnemySpawner] Враг 1 создан в {spawnPosition1}");

            // Спавн второго врага
            Vector3 spawnPosition2 = _spawnPoint2 != null ? _spawnPoint2.position : transform.position + Vector3.right * 2f;
            EnemyBehavior enemy2 = Instantiate(_enemyPrefab, spawnPosition2, Quaternion.identity);
            enemy2.Initialize(_playerTransform, null);
            Debug.Log($"[EnemySpawner] Враг 2 создан в {spawnPosition2}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            if (_spawnPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_spawnPoint.position, 0.3f);
            }

            if (_spawnPoint2 != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_spawnPoint2.position, 0.3f);
            }
        }
    }
}
