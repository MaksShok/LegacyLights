using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using EnemyModule.Abstract;
using EnemyModule.Configs;
using EnemyModule.StateMachine_States.States;
using UnityEngine;

namespace EnemyModule.Behaviors
{
    public class MeshikBehavior : Abstract.EnemyBehavior
    {
        private MeshikConfig _config;
        private CheckTwoObjectsClose _checkClose;
        private SimpleDamageProvider _damageProvider;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _config = (MeshikConfig)baseConfig;
            _damageProvider = new SimpleDamageProvider(_config.Damage);
            
            _healthModel.Die += Die;
            
            float closeDistance = _config.AttackDistance + 0.2f;
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, closeDistance);

            var followState = new FollowToPointState(_rb, _targetTransform, _config.MoveSpeed, _checkClose);
            var attackState = new AttackState(_damageProvider, _playerHealth, _config.AttackCooldownSec, _checkClose);

            _stateMachine.AddTransition(followState, attackState, () => _checkClose.IsClose);
            _stateMachine.AddTransition(attackState, followState, () => !_checkClose.IsClose);

            _stateMachine.SetState(followState);
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}