using EnemyModule.Abstract;
using UnityEngine;

namespace EnemyModule
{
    /// <summary>
    /// Спавнер врагов ближнего боя (Meshik).
    /// Автоматически инициализирует врага при создании.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Настройки спавна")]
        [SerializeField] private EnemyBehavior _enemyPrefab;
        [SerializeField] private Transform _spawnPoint1;
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

            // Спавним врагов если префаб задан
            if (_enemyPrefab != null)
            {
                SpawnEnemies();
            }
        }

        /// <summary>
        /// Создать врагов ближнего боя в точках спавна
        /// </summary>
        public void SpawnEnemies()
        {
            if (_enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] Префаб врага не задан!");
                return;
            }

            // Спавн первого врага
            if (_spawnPoint1 != null)
            {
                SpawnEnemyAt(_spawnPoint1.position, "Meshik 1");
            }

            // Спавн второго врага
            if (_spawnPoint2 != null)
            {
                SpawnEnemyAt(_spawnPoint2.position, "Meshik 2");
            }
        }

        private EnemyBehavior SpawnEnemyAt(Vector3 position, string name)
        {
            EnemyBehavior enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.Initialize(_playerTransform, null);
            Debug.Log($"[EnemySpawner] {name} создан в {position}");
            return enemy;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            if (_spawnPoint1 != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_spawnPoint1.position, 0.3f);
            }

            if (_spawnPoint2 != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_spawnPoint2.position, 0.3f);
            }
        }
    }
}
