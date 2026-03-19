using CommonLogic.Conditions;
using CommonLogic.DamageModule;
using CommonLogic.HealthModule;
using UnityEngine;

namespace CommonLogic.StateMachine_States.States
{
    /// <summary>
    /// Состояние атаки на расстоянии со стрельбой снарядами.
    /// </summary>
    public class RangedAttackState : IState
    {
        public bool CanExit => !_spendHealth.Alive || !_checkRange.IsInRange;

        private readonly Transform _shooterTransform;
        private readonly IDamageProvider _damageProvider;
        private readonly ISpendHealth _spendHealth;
        private readonly float _attackCooldown;
        private readonly CheckObjectInRange _checkRange;
        private readonly GameObject _projectilePrefab;
        private readonly float _projectileSpeed;

        private float _timeSinceLastAttack;

        public RangedAttackState(
            Transform shooterTransform,
            IDamageProvider damageProvider,
            ISpendHealth spendHealth,
            float attackCooldown,
            CheckObjectInRange checkRange,
            GameObject projectilePrefab,
            float projectileSpeed)
        {
            _shooterTransform = shooterTransform;
            _damageProvider = damageProvider;
            _spendHealth = spendHealth;
            _attackCooldown = attackCooldown;
            _checkRange = checkRange;
            _projectilePrefab = projectilePrefab;
            _projectileSpeed = projectileSpeed;
        }

        public void Enter()
        {
            _timeSinceLastAttack = 0f;
        }

        public void Exit() { }

        public void Update(float deltaTime)
        {
            _timeSinceLastAttack += deltaTime;

            if (_timeSinceLastAttack >= _attackCooldown && _checkRange.IsInRange)
            {
                Debug.Log($"[RangedAttackState] Стрельба! Дистанция={_checkRange.Distance}, время={_timeSinceLastAttack}");
                Shoot();
                _timeSinceLastAttack = 0f;
            }
            else
            {
                if (_timeSinceLastAttack >= _attackCooldown)
                {
                    Debug.Log($"[RangedAttackState] Не стреляю: IsInRange={_checkRange.IsInRange}, дистанция={_checkRange.Distance}");
                }
            }
        }

        public void FixedUpdate(float fixedDeltaTime) { }

        private void Shoot()
        {
            if (_projectilePrefab == null)
            {
                Debug.LogError("[RangedAttackState] Projectile prefab is null! Назначьте префаб снаряда в ShooterConfig!");
                return;
            }

            Debug.Log($"[RangedAttackState] Создаю снаряд: {_projectilePrefab.name}");

            // Находим направление к цели
            Vector2 targetPosition;

            if (_spendHealth is MonoBehaviour monoBehaviour)
            {
                targetPosition = monoBehaviour.transform.position;
            }
            else if (_spendHealth is Transform targetTransform)
            {
                targetPosition = targetTransform.position;
            }
            else
            {
                targetPosition = (Vector2)_shooterTransform.position + Vector2.right;
            }

            Vector2 direction = (targetPosition - (Vector2)_shooterTransform.position).normalized;
            Debug.Log($"[RangedAttackState] Направление: {direction}");

            // Создаём снаряд
            GameObject projectile = Object.Instantiate(
                _projectilePrefab,
                _shooterTransform.position,
                Quaternion.LookRotation(Vector3.forward, direction));

            Debug.Log($"[RangedAttackState] Снаряд создан: {projectile.name}");

            // Запускаем снаряд через Rigidbody2D
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log($"[RangedAttackState] Запуск снаряда со скоростью: {_projectileSpeed}");
                rb.linearVelocity = direction * _projectileSpeed;
            }
            else
            {
                Debug.LogError("[RangedAttackState] У снаряда нет Rigidbody2D!");
            }

            // Уничтожаем снаряд через 3 секунды
            Object.Destroy(projectile, 3f);
        }
    }
}
