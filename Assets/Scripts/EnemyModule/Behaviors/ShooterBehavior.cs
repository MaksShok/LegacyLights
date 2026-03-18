using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.StateMachine_States.States;
using EnemyModule.Abstract;
using EnemyModule.Configs;

namespace EnemyModule.Behaviors
{
    public class ShooterBehavior : Abstract.EnemyBehavior
    {
        private ShooterConfig _shooterConfig;
        private CheckObjectInRange _checkInRange;
        private CheckTwoObjectsClose _checkClose;
        private SimpleDamageProvider _damageProvider;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _shooterConfig = (ShooterConfig)baseConfig;

            _damageProvider = new SimpleDamageProvider(_shooterConfig.Damage);
        }

        protected override void SetupStates()
        {
            // Создаём проверки диапазона
            _checkInRange = new CheckObjectInRange(
                transform, 
                _targetTransform, 
                _shooterConfig.MinAttackRange, 
                _shooterConfig.AttackRange);
            
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, _shooterConfig.AttackRange);

            // Создаём состояния
            var followState = new FollowToPointState(_rb, _targetTransform, _shooterConfig.MoveSpeed, _checkClose);
            var rangedAttackState = new RangedAttackState(
                _damageProvider, 
                _targetHealth, 
                _shooterConfig.AttackCooldownSec, 
                _checkInRange);

            // Начинаем с преследования
            _stateMachine.SetState(followState);

            // Настраиваем переходы
            // Преследование -> Атака (когда в диапазоне)
            _stateMachine.AddTransition(followState, rangedAttackState, () => _checkInRange.IsInRange);
            
            // Атака -> Преследование (если цель слишком далеко)
            _stateMachine.AddTransition(rangedAttackState, followState, () => !_checkClose.IsClose);
        }
    }
}