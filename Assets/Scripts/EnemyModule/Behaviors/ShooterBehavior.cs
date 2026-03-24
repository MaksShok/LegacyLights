using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using EnemyModule.Abstract;
using EnemyModule.Configs;
using EnemyModule.StateMachine_States.States;
using UnityEngine;

namespace EnemyModule.Behaviors
{
    /// <summary>
    /// Поведение врага дальнего боя.
    /// 1. Занимает позицию на оптимальной дистанции от игрока
    /// 2. Стреляет снарядами, пока цель в диапазоне
    /// 3. Отступает/приближается, если цель слишком далеко/близко
    /// </summary>
    public class ShooterBehavior : Abstract.EnemyBehavior
    {
        private ShooterConfig _shooterConfig;
        private CheckObjectInRange _checkInRange;
        private CheckTwoObjectsClose _checkTooClose;
        private CheckTwoObjectsClose _checkTooFar;
        private SimpleDamageProvider _damageProvider;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _shooterConfig = (ShooterConfig)baseConfig;

            _damageProvider = new SimpleDamageProvider(_shooterConfig.Damage);

            // Подписка на смерть
            if (_healthModel != null)
            {
                _healthModel.Die += Die;
            }
            
            Debug.Log($"[ShooterBehavior] {gameObject.name} настройка состояний. AttackRange={_shooterConfig.AttackRange}, MinAttackRange={_shooterConfig.MinAttackRange}, OptimalRange={_shooterConfig.OptimalRange}");
            
            // Проверка: цель в диапазоне атаки
            _checkInRange = new CheckObjectInRange(
                transform,
                _targetTransform,
                _shooterConfig.MinAttackRange,
                _shooterConfig.AttackRange);

            // Проверка: цель слишком близко (нужно отступить)
            _checkTooClose = new CheckTwoObjectsClose(
                transform,
                _targetTransform,
                _shooterConfig.MinAttackRange);

            // Проверка: цель слишком далеко (нужно приблизиться)
            _checkTooFar = new CheckTwoObjectsClose(
                transform,
                _targetTransform,
                _shooterConfig.OptimalRange);

            Debug.Log($"[ShooterBehavior] ProjectilePrefab={_shooterConfig.ProjectilePrefab}, ProjectileSpeed={_shooterConfig.ProjectileSpeed}");

            // Состояния
            var takePositionState = new TakePositionState(
                _rb,
                _targetTransform,
                _shooterConfig.MoveSpeed,
                _shooterConfig.OptimalRange);

            var rangedAttackState = new RangedAttackState(
                transform,
                _damageProvider,
                _playerHealth,
                _shooterConfig.AttackCooldownSec,
                _checkInRange,
                _shooterConfig.ProjectilePrefab,
                _shooterConfig.ProjectileSpeed);

            // Начинаем с занятия позиции
            _stateMachine.SetState(takePositionState);
            Debug.Log($"[ShooterBehavior] Начальное состояние: TakePositionState");

            // Занятие позиции -> Атака (когда позиция занята и цель в диапазоне)
            _stateMachine.AddTransition(takePositionState, rangedAttackState,
                () => takePositionState.CanExit && _checkInRange.IsInRange);

            // Атака -> Занятие позиции (если цель слишком близко или слишком далеко)
            _stateMachine.AddTransition(rangedAttackState, takePositionState,
                () => _checkTooClose.IsClose || _checkTooFar.IsClose);
                
            Debug.Log($"[ShooterBehavior] Настройка состояний завершена");
        }

        private void Die()
        {
            Debug.Log($"[ShooterBehavior] {gameObject.name} умер");
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            // Рисуем сферу на позиции врага для отладки
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            // Рисуем диапазон атаки
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 8f);
        }
    }
}