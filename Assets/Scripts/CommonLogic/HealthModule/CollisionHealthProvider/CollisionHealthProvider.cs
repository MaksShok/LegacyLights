using CommonLogic.DamageModule;
using Misc.Collisions;
using UnityEngine;

namespace CommonLogic.HealthModule.CollisionHealthProvider
{
    public class CollisionHealthProvider : MonoBehaviour
    {
        [SerializeField] 
        private TriggerCollisionEvent _triggerCollisionEvent;

        private ISpendHealth _health;

        private void Start()
        {
            _triggerCollisionEvent.TriggerEnter += ProvideHealth;
        }

        public void Initialize(ISpendHealth health)
        {
            _health = health;
        }

        private void ProvideHealth(Collider2D col)
        {
            if (col.TryGetComponent<IDamageProvider>(out var damageProvider) && _health != null)
            {
                damageProvider.ApplyDamage(_health);
            }
        }

        private void OnDestroy()
        {
            _triggerCollisionEvent.TriggerEnter -= ProvideHealth;
        }
    }
}