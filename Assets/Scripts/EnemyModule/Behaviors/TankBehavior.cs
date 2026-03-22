using CommonLogic.Conditions;
using CommonLogic.HealthModule;
using CommonLogic.StateMachine_States.States;
using EnemyModule.Abstract;
using EnemyModule.Configs;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyModule.Behaviors
{
    public class TankBehavior : Abstract.EnemyBehavior
    {
        [Header("Параметры Танка")]
        [SerializeField] private LayerMask _targetLayerMask = default;

        private TankConfig _tankConfig;
        private CheckTwoObjectsClose _checkClose;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _tankConfig = (TankConfig)baseConfig;
            
            // Создаём проверку близости для области атаки
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, _tankConfig.AttackRange);

            // Создаём состояния
            var followState = new FollowToPointState(_rb, _targetTransform, _tankConfig.MoveSpeed, _checkClose);
            var areaAttackState = new AreaAttackState(
                _tankConfig.Damage,
                _playerHealth,
                _tankConfig.AttackCooldownSec,
                _tankConfig.AreaDamageRadius,
                _checkClose,
                transform,
                _targetLayerMask);

            // Начинаем с преследования
            _stateMachine.SetState(followState);

            // Настраиваем переходы
            _stateMachine.AddTransition(followState, areaAttackState, () => _checkClose.IsClose);
            _stateMachine.AddTransition(areaAttackState, followState, () => !_checkClose.IsClose);
        }

        /// <summary>
        /// Отрисовка радиуса атаки в редакторе
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (_tankConfig == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _tankConfig.AreaDamageRadius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _tankConfig.AttackRange);
        }
    }
}