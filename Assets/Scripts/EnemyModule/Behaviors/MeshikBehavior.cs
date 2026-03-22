using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using CommonLogic.StateMachine_States;
using CommonLogic.StateMachine_States.States;
using EnemyModule.Abstract;
using EnemyModule.Configs;
using UnityEngine;

namespace EnemyModule.Behaviors
{
    public class MeshikBehavior : Abstract.EnemyBehavior
    {
        private MeshikConfig _meshikConfig;
        private CheckTwoObjectsClose _checkClose;
        private SimpleDamageProvider _damageProvider;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _meshikConfig = (MeshikConfig)baseConfig;
            _damageProvider = new SimpleDamageProvider(_meshikConfig.Damage);
            
            if (_healthModel != null)
            {
                _healthModel.OnDie += Die;
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        
        protected override void SetupStates()
        {
            // Останавливаемся на дистанции атаки + небольшой запас
            float stopDistance = _meshikConfig.AttackRange + 0.2f;
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, stopDistance);

            var followState = new FollowToPointState(_rb, _targetTransform, _meshikConfig.MoveSpeed, _checkClose);
            var attackState = new AttackState(_damageProvider, _playerHealth, _meshikConfig.AttackCooldownSec, _checkClose);

            _stateMachine.SetState(followState);

            _stateMachine.AddTransition(followState, attackState, () => _checkClose.IsClose);
            _stateMachine.AddTransition(attackState, followState, () => !_checkClose.IsClose);
        }
    }
}