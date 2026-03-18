using CommonLogic.Conditions;
using CommonLogic.DamageModule.DamageProvider;
using CommonLogic.HealthModule;
using CommonLogic.StateMachine_States;
using CommonLogic.StateMachine_States.States;
using EnemyModule.Abstract;
using EnemyModule.Configs;
using UnityEngine;

namespace EnemyModule.Behaviors
{
    public class MeshikBehavior : Abstract.EnemyBehavior
    {
        private MeshikConfig _meshikConfig;
        private CheckTwoObjectsClose _checkClose;
        private SimpleDamageProvider _damageProvider;
        private SpriteRenderer _spriteRenderer;
        private float _stunTimeRemaining = 0f;
        private bool _isStunned = false;
        private Transform _playerTransform;

        private float _aggroRange = 5f;

        private Color _originalColor;
        private Color _hitColor = Color.red;

        protected override void OnInitialize(EnemyConfig baseConfig)
        {
            _meshikConfig = (MeshikConfig)baseConfig;

            _damageProvider = new SimpleDamageProvider(_meshikConfig.Damage);

            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // Отключаем гравитацию как у игрока
            if (_rb != null)
            {
                _rb.gravityScale = 0f;
            }

            if (_spriteRenderer != null)
            {
                _originalColor = _spriteRenderer.color;
            }

            if (_healthModel != null)
            {
                _healthModel.OnDie += Die;
            }

            FindPlayer();
        }
        
        private void FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
                _targetTransform = _playerTransform;
            }
        }

        protected new void Update()
        {
            base.Update();

            if (_isStunned)
            {
                _stunTimeRemaining -= Time.deltaTime;
                if (_stunTimeRemaining <= 0)
                {
                    _isStunned = false;
                    if (_spriteRenderer != null)
                    {
                        _spriteRenderer.color = _originalColor;
                    }
                }
                return;
            }
            
            if (_playerTransform == null)
            {
                FindPlayer();
                return;
            }
            
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer <= _aggroRange)
            {
                _targetTransform = _playerTransform;
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
        
        protected override void SetupStates()
        {
            // Останавливаемся на дистанции атаки + небольшой запас
            float stopDistance = _meshikConfig.AttackRange + 0.2f;
            _checkClose = new CheckTwoObjectsClose(transform, _targetTransform, stopDistance);

            var followState = new FollowToPointState(_rb, _targetTransform, _meshikConfig.MoveSpeed, _checkClose);
            var attackState = new AttackState(_damageProvider, _targetHealth, _meshikConfig.AttackCooldownSec, _checkClose);

            _stateMachine.SetState(followState);

            _stateMachine.AddTransition(followState, attackState, () => _checkClose.IsClose && !_isStunned && _targetTransform != null);
            _stateMachine.AddTransition(attackState, followState, () => !_checkClose.IsClose || _isStunned || _targetTransform == null);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _aggroRange);
        }
    }
}