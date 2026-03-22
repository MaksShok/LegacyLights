using CommonLogic.Conditions;
using UnityEngine;

namespace CommonLogic.StateMachine_States.States
{
    public class FollowToPointState : IState
    {
        public bool CanExit => _checkClose.IsClose;

        private readonly Rigidbody2D _rb;
        private readonly Transform _targetPoint;
        private readonly float _moveSpeed;
        private readonly CheckTwoObjectsClose _checkClose;

        public FollowToPointState(Rigidbody2D rb, Transform targetPoint, float moveSpeed, CheckTwoObjectsClose checkClose)
        {
            _rb = rb;
            _targetPoint = targetPoint;
            _moveSpeed = moveSpeed;
            _checkClose = checkClose;
        }

        public void Enter()
        {
        }

        public void Exit() { }

        public void FixedUpdate(float fixedDeltaTime)
        {
            Vector2 currentPosition = _rb.position;
            Vector2 toTarget = (Vector2)_targetPoint.position - currentPosition;
            float distanceToTarget = toTarget.magnitude;

            if (distanceToTarget <= 0.01f)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 desiredVelocity = toTarget.normalized * _moveSpeed;
            _rb.linearVelocity = desiredVelocity;
        }

        public void Update(float deltaTime) { }
    }
}