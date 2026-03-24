using UnityEngine;

namespace EnemyModule.StateMachine_States.States
{
    /// <summary>
    /// Состояние занятия позиции на оптимальной дистанции от цели.
    /// Враг двигается к точке на линии между ним и целью, пока не достигнет оптимальной дистанции.
    /// </summary>
    public class TakePositionState : IState
    {
        public bool CanExit => _isAtPosition;

        private readonly Rigidbody2D _rb;
        private readonly Transform _targetTransform;
        private readonly float _moveSpeed;
        private readonly float _optimalRange;
        private readonly float _rangeTolerance;

        private bool _isAtPosition = false;
        private Vector2 _targetPosition;

        public TakePositionState(
            Rigidbody2D rb,
            Transform targetTransform,
            float moveSpeed,
            float optimalRange,
            float rangeTolerance = 0.5f)
        {
            _rb = rb;
            _targetTransform = targetTransform;
            _moveSpeed = moveSpeed;
            _optimalRange = optimalRange;
            _rangeTolerance = rangeTolerance;
        }

        public void Enter()
        {
            _isAtPosition = false;
            CalculateTargetPosition();
        }

        public void Exit()
        {
        }

        public void Update(float deltaTime)
        {
            // Проверяем, не сместилась ли цель слишком сильно
            float currentDistance = Vector2.Distance(_rb.position, _targetTransform.position);
            float distanceToTargetPos = Vector2.Distance(_rb.position, _targetPosition);

            Debug.Log($"[TakePositionState] Дистанция до цели={currentDistance:F2}, до позиции={distanceToTargetPos:F2}, оптимальная={_optimalRange}, CanExit={_isAtPosition}");

            // Если цель сильно сместилась или мы уже близко к целевой позиции
            if (distanceToTargetPos <= _rangeTolerance)
            {
                _isAtPosition = true;
                Debug.Log($"[TakePositionState] Позиция занята! _isAtPosition=true");
            }
            else if (currentDistance > _optimalRange + _rangeTolerance * 2)
            {
                // Если цель сместилась слишком сильно, пересчитываем позицию
                CalculateTargetPosition();
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            if (_isAtPosition)
                return;

            Vector2 currentPosition = _rb.position;
            Vector2 toTargetPos = _targetPosition - currentPosition;
            float distanceToTargetPos = toTargetPos.magnitude;

            if (distanceToTargetPos <= 0f)
            {
                _isAtPosition = true;
                return;
            }

            float maxStep = _moveSpeed * fixedDeltaTime;
            Vector2 step;

            if (maxStep >= distanceToTargetPos)
            {
                step = toTargetPos;
                _isAtPosition = true;
            }
            else
            {
                step = toTargetPos.normalized * maxStep;
            }

            Vector2 newPosition = currentPosition + step;
            _rb.MovePosition(newPosition);
        }

        private void CalculateTargetPosition()
        {
            // Находим направление от цели к врагу
            Vector2 directionToEnemy = ((Vector2)_rb.position - (Vector2)_targetTransform.position).normalized;

            // Целевая позиция - точка на оптимальной дистанции от игрока в направлении врага
            _targetPosition = (Vector2)_targetTransform.position + directionToEnemy * _optimalRange;
        }
    }
}
