using EnemyModule.Abstract;
using UnityEngine;

namespace EnemyModule
{
    /// <summary>
    /// Спавнер врагов дальнего боя (Shooter).
    /// Автоматически инициализирует врага при создании.
    /// </summary>
    public class ShooterSpawner : MonoBehaviour
    {
        [Header("Настройки спавна")]
        [SerializeField] private EnemyBehavior _shooterPrefab;
        [SerializeField] private Transform _spawnPoint1;
        [SerializeField] private Transform _spawnPoint2;
        [SerializeField] private Transform _spawnPoint3;

        private Transform _playerTransform;

        private void Start()
        {
            Debug.Log("[ShooterSpawner] Start() вызван");

            // Находим игрока на сцене
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
                Debug.Log("[ShooterSpawner] Игрок найден: " + player.name);
            }
            else
            {
                Debug.LogWarning("[ShooterSpawner] Игрок не найден! Враги не смогут атаковать.");
            }

            // Спавним врагов если префаб задан
            if (_shooterPrefab != null)
            {
                Debug.Log("[ShooterSpawner] Префаб стрелка задан: " + _shooterPrefab.name);
                SpawnShooters();
            }
            else
            {
                Debug.LogError("[ShooterSpawner] Префаб стрелка НЕ задан в Inspector!");
            }
        }

        /// <summary>
        /// Создать врагов дальнего боя в точках спавна
        /// </summary>
        public void SpawnShooters()
        {
            if (_shooterPrefab == null)
            {
                Debug.LogError("[ShooterSpawner] Префаб стрелка не задан!");
                return;
            }

            Debug.Log("[ShooterSpawner] Начинаем спавн стрелков...");

            // Спавн первого врага
            if (_spawnPoint1 != null)
            {
                SpawnShooterAt(_spawnPoint1.position, "Shooter 1");
            }

            // Спавн второго врага
            if (_spawnPoint2 != null)
            {
                SpawnShooterAt(_spawnPoint2.position, "Shooter 2");
            }

            // Спавн третьего врага (опционально)
            if (_spawnPoint3 != null)
            {
                SpawnShooterAt(_spawnPoint3.position, "Shooter 3");
            }
        }

        private EnemyBehavior SpawnShooterAt(Vector3 position, string name)
        {
            Debug.Log($"[ShooterSpawner] Спавн {name} в позиции: {position}");
            
            EnemyBehavior shooter = Instantiate(_shooterPrefab, position, Quaternion.identity);
            Debug.Log($"[ShooterSpawner] {name} создан, объект: {shooter.gameObject.name}");
            
            shooter.Initialize(_playerTransform, null);
            
            Debug.Log($"[ShooterSpawner] {name} успешно создан в {position}");
            Debug.Log($"[ShooterSpawner] {name} позиция: {shooter.transform.position}, активен: {shooter.gameObject.activeSelf}");
            
            return shooter;
        }

        private void OnDrawGizmos()
        {
            // Рисуем точку спавнера
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            // Рисуем точки спавна
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

            if (_spawnPoint3 != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_spawnPoint3.position, 0.3f);
            }
        }
    }
}
