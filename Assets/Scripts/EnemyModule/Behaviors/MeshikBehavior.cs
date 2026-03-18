using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using CommonLogic.StateMachine_States.States;
using EnemyModule.Abstract;
using EnemyModule.Configs;

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
        }

        protected override void SetupStates()
        {
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, _meshikConfig.AttackRange);

            var followState = new FollowToPointState(_rb, _targetTransform, _meshikConfig.MoveSpeed, _checkClose);
            var attackState = new AttackState(_damageProvider, _targetHealth, _meshikConfig.AttackCooldownSec, _checkClose);

            _stateMachine.SetState(followState);

            _stateMachine.AddTransition(followState, attackState, () => _checkClose.IsClose);
            _stateMachine.AddTransition(attackState, followState, () => !_checkClose.IsClose);
        }
    }
}