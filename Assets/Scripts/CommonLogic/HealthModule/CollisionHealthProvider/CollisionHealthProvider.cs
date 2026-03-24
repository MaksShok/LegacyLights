using CommonLogic.DamageModule;
using UnityEngine;

namespace CommonLogic.HealthModule.CollisionHealthProvider
{
    public class CollisionHealthProvider : MonoBehaviour
    {
        private ISpendHealth _health;

        public void Initialize(ISpendHealth health)
        {
            _health = health;
        }

        public void ProvideHealth(IDamageProvider damageProvider)
        {
            if (_health != null)
            {
                damageProvider.ApplyDamage(_health);
            }
        }
    }
}