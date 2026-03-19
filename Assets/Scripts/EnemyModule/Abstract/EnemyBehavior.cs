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

        protected virtual void Awake()
        {
            // Автоматически получаем компоненты если не назначены
            if (_rb == null)
                _rb = GetComponent<Rigidbody2D>();
            
            if (_collisionHealthProvider == null)
                _collisionHealthProvider = GetComponent<CollisionHealthProvider>();

            // Добавляем SpriteRenderer если нет
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                
                // Создаём простой спрайт (круг) программно
                Sprite sprite = CreateCircleSprite();
                spriteRenderer.sprite = sprite;
                spriteRenderer.color = new Color(1, 0, 0, 1); // Красный цвет
                Debug.Log($"[EnemyBehavior] Добавлен SpriteRenderer на {gameObject.name}");
            }
        }

        private Sprite CreateCircleSprite()
        {
            // Создаём текстуру 32x32 с белым кругом
            Texture2D texture = new Texture2D(32, 32);
            Color[] pixels = new Color[32 * 32];
            
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(16, 16));
                    pixels[y * 32 + x] = dist <= 14 ? Color.white : Color.clear;
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        }

        public void Initialize(Transform target, ISpendHealth targetHealth)
        {
            if (target == null)
            {
                Debug.LogError($"[EnemyBehavior] targetTransform равен null на {gameObject.name}!");
                enabled = false;
                return;
            }

            _targetTransform = target;
            _targetHealth = targetHealth;

            if (_config == null)
            {
                Debug.LogError($"[EnemyBehavior] Config не назначен на {gameObject.name}!");
                enabled = false;
                return;
            }

            _healthModel = new HealthModel(_config.Health);
            
            if (_collisionHealthProvider != null)
            {
                _collisionHealthProvider.Initialize(_healthModel);
            }

            _healthReceiver = GetComponent<EnemyModule.EnemyHealthReceiver>();
            if (_healthReceiver == null)
            {
                _healthReceiver = gameObject.AddComponent<EnemyModule.EnemyHealthReceiver>();
            }
            _healthReceiver.Initialize(_healthModel);

            _stateMachine = new StateMachine();

            OnInitialize(_config);
            SetupStates();
            
            Debug.Log($"[EnemyBehavior] {gameObject.name} успешно инициализирован");
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
