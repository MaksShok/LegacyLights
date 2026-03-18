namespace CommonLogic.StateMachine_States.States
{
    public class IdleState : IState
    {
        public bool CanExit => true;

        private readonly float _idleDuration;
        private float _idleTimer;

        public IdleState(float idleDuration = 0f)
        {
            _idleDuration = idleDuration;
        }

        public void Enter()
        {
            _idleTimer = 0f;
        }

        public void Exit() { }

        public void Update(float deltaTime)
        {
            if (_idleDuration > 0)
            {
                _idleTimer += deltaTime;
            }
        }

        public void FixedUpdate(float fixedDeltaTime) { }

        /// <summary>
        /// Проверка завершения бездействия (если задана длительность)
        /// </summary>
        public bool IsIdleComplete => _idleDuration <= 0 || _idleTimer >= _idleDuration;
    }
}