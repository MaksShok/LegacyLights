using CommonLogic.Conditions;
using CommonLogic.DamageModule;
using CommonLogic.HealthModule;
using UnityEngine;

namespace EnemyModule.StateMachine_States.States
{
    public class AreaAttackState : IState
    {
        public bool CanExit => !_spendHealth.Alive || !_checkClose.IsClose;

        private readonly float _damage;
        private readonly ISpendHealth _spendHealth;
        private readonly float _attackCooldown;
        private readonly float _areaRadius;
        private readonly CheckTwoObjectsClose _checkClose;
        private readonly Transform _attackOrigin;
        private readonly LayerMask _targetLayerMask;

        private Collider2D[] _hits;
        private float _timeSinceLastAttack;

        public AreaAttackState(
            float damage,
            ISpendHealth spendHealth,
            float attackCooldown,
            float areaRadius,
            CheckTwoObjectsClose checkClose,
            Transform attackOrigin,
            LayerMask targetLayerMask)
        {
            _damage = damage;
            _spendHealth = spendHealth;
            _attackCooldown = attackCooldown;
            _areaRadius = areaRadius;
            _checkClose = checkClose;
            _attackOrigin = attackOrigin;
            _targetLayerMask = targetLayerMask;
        }

        public void Enter()
        {
            _timeSinceLastAttack = 0f;
        }

        public void Update(float deltaTime)
        {
            _timeSinceLastAttack += deltaTime;

            if (_timeSinceLastAttack >= _attackCooldown && _checkClose.IsClose)
            {
                PerformAreaAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void PerformAreaAttack()
        {
            Collider2D hit = Physics2D.OverlapCircle(_attackOrigin.position, _areaRadius, _targetLayerMask);
            
            if (hit.TryGetComponent<IDamageProvider>(out var damageProvider))
            {
                damageProvider.ApplyDamage(_spendHealth);
            }
        }

        public void Exit() { }

        public void FixedUpdate(float fixedDeltaTime) { }
    }
}