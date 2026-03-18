using CommonLogic.Conditions;
using CommonLogic.DamageModule;
using CommonLogic.HealthModule;

namespace CommonLogic.StateMachine_States.States
{
    public class RangedAttackState : IState
    {
        public bool CanExit => !_spendHealth.Alive || !_checkRange.IsInRange;

        private readonly IDamageProvider _damageProvider;
        private readonly ISpendHealth _spendHealth;
        private readonly float _attackCooldown;
        private readonly CheckObjectInRange _checkRange;

        private float _timeSinceLastAttack;

        public RangedAttackState(
            IDamageProvider damageProvider, 
            ISpendHealth spendHealth,
            float attackCooldown, 
            CheckObjectInRange checkRange)
        {
            _damageProvider = damageProvider;
            _spendHealth = spendHealth;
            _attackCooldown = attackCooldown;
            _checkRange = checkRange;
        }

        public void Enter()
        {
            _timeSinceLastAttack = 0f;
        }

        public void Update(float deltaTime)
        {
            _timeSinceLastAttack += deltaTime;

            if (_timeSinceLastAttack >= _attackCooldown && _checkRange.IsInRange)
            {
                _damageProvider.ApplyDamage(_spendHealth);
                _timeSinceLastAttack = 0f;
            }
        }

        public void Exit() { }

        public void FixedUpdate(float fixedDeltaTime) { }
    }
}