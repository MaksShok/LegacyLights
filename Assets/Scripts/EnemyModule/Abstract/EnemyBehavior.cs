using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using CommonLogic.HealthModule.CollisionHealthProvider;
using CommonLogic.StateMachine_States;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyModule.Abstract
{
    public abstract class EnemyBehavior : MonoBehaviour
    {
        [SerializeField] protected CollisionHealthProvider _collisionHealthProvider;
        [SerializeField] protected Rigidbody2D _rb;
        [SerializeField] private EnemyConfig _config;

        public ISpendHealth SpendHealth => _healthModel;

        protected HealthModel _healthModel;
        protected StateMachine _stateMachine;

        protected Transform _targetTransform;
        protected ISpendHealth _targetHealth;

        private EnemyModule.EnemyHealthReceiver _healthReceiver;

        public void Initialize(Transform target, ISpendHealth targetHealth)
        {
            _targetTransform = target;
            _targetHealth = targetHealth;

            _healthModel = new HealthModel(_config.Health);
            _collisionHealthProvider.Initialize(_healthModel);

            _healthReceiver = GetComponent<EnemyModule.EnemyHealthReceiver>();
            if (_healthReceiver == null)
            {
                _healthReceiver = gameObject.AddComponent<EnemyModule.EnemyHealthReceiver>();
            }
            _healthReceiver.Initialize(_healthModel);

            _stateMachine = new StateMachine();

            OnInitialize(_config);
            SetupStates();
        }

        protected abstract void OnInitialize(EnemyConfig baseConfig);

        protected abstract void SetupStates();

        protected virtual void Update()
        {
            _stateMachine?.Update();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine?.FixedUpdateState(Time.fixedDeltaTime);
        }

        protected virtual void OnDestroy()
        {
            _healthModel?.ClearAllSubscribers();
        }
    }
}
