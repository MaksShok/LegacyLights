using System.Collections;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule.CollisionHealthProvider;
using Misc.Collisions;
using UnityEngine;

namespace WeaponModule
{
    public class SimpleWeapon : MonoBehaviour
    {
        [SerializeField] 
        private TriggerCollisionEvent _damageTrigger;

        [SerializeField] 
        private int _damage = 10;

        [SerializeField] 
        private float _cooldown = 1f;

        private SimpleDamageProvider _simpleDamageProvider;
        private float _lastAttackTime;
        private bool _isAttacking;
    
        private void Start()
        {
            _simpleDamageProvider = new SimpleDamageProvider(_damage);
            _damageTrigger.TriggerEnter += TryGetDamage;
            _damageTrigger.gameObject.SetActive(false);
        }

        public void Attack()
        {
            if (Time.time > _lastAttackTime + _cooldown && !_isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }

        private IEnumerator AttackCoroutine()
        {
            Debug.Log("Start Attack");
            _isAttacking = true;
            _lastAttackTime = Time.time;

            Vector2 direction = GetAttackDirection();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _damageTrigger.transform.position = (Vector2)transform.position + direction.normalized;
            _damageTrigger.transform.rotation = Quaternion.Euler(0, 0, angle);
            _damageTrigger.gameObject.SetActive(true);

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            _damageTrigger.gameObject.SetActive(false);

            _isAttacking = false;
        }

        private void TryGetDamage(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") &&
                col.TryGetComponent<CollisionHealthProvider>(out var healthProvider))
            {
                Debug.Log("Attack " + col.gameObject.name);
                healthProvider.ProvideHealth(_simpleDamageProvider);
            }
        }

        private Vector2 GetAttackDirection()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2) transform.position;
            return direction;
        }

        private void OnDestroy()
        {
            _damageTrigger.TriggerEnter -= TryGetDamage;
        }
    }
}