using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using CommonLogic.HealthModule.CollisionHealthProvider;
using EnemyModule.StateMachine_States;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyModule.Abstract
{
    public abstract class EnemyBehavior : MonoBehaviour
    {
        [SerializeField] protected CollisionHealthProvider _collisionHealthProvider;
        [SerializeField] protected Rigidbody2D _rb;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private HealthBar _healthBar;
        
        public IHealth Health => _healthModel;

        protected HealthModel _healthModel;
        protected StateMachine _stateMachine;

        protected Transform _targetTransform;
        protected ISpendHealth _playerHealth;
        
        public void Initialize(Transform target, ISpendHealth targetHealth)
        {
            _targetTransform = target;
            _playerHealth = targetHealth;
            _stateMachine = new StateMachine();
            _healthModel = new HealthModel(_enemyConfig.Health);
            
            _collisionHealthProvider.Initialize(_healthModel);
            _healthBar.Initialize(_healthModel);
            
            OnInitialize(_enemyConfig);
        }

        protected abstract void OnInitialize(EnemyConfig baseConfig);

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
