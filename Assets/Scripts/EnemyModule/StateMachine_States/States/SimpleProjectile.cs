using CommonLogic.HealthModule;
using UnityEngine;

namespace EnemyModule.StateMachine_States.States
{
    /// <summary>
    /// Простой снаряд. Летит по прямой и наносит урон при столкновении.
    /// </summary>
    public class SimpleProjectile : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private LayerMask _targetLayerMask;

        private Vector2 _direction;
        private int _damage;
        private ISpendHealth _targetHealth;
        private float _lifetimeRemaining;

        public void Initialize(Vector2 direction, int damage, ISpendHealth targetHealth)
        {
            _direction = direction.normalized;
            _damage = damage;
            _targetHealth = targetHealth;
            _lifetimeRemaining = _lifetime;

            // Поворачиваем снаряд по направлению движения
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        private void Update()
        {
            _lifetimeRemaining -= Time.deltaTime;
            if (_lifetimeRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Проверяем, не попали ли в цель
            if (_targetHealth is MonoBehaviour monoBehaviour && other.gameObject == monoBehaviour.gameObject)
            {
                ApplyDamage();
                return;
            }

            // Проверяем слой цели
            if (((1 << other.gameObject.layer) & _targetLayerMask) != 0)
            {
                ApplyDamage();
            }
        }

        private void ApplyDamage()
        {
            if (_targetHealth != null)
            {
                _targetHealth.Spend(_damage);
            }

            Destroy(gameObject);
        }
    }
}
