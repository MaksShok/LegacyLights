using UnityEngine;

namespace EnemyModule.Abstract
{
    public abstract class EnemyConfig : ScriptableObject
    {
        [Header("Базовые параметры")]
        [SerializeField] protected int _health = 100;
        [SerializeField] protected int _damage = 10;
        [SerializeField] protected int _moveSpeed = 3;
        [SerializeField] protected float _attackCooldownSec = 1f;

        public int Health => _health;
        public int Damage => _damage;
        public int MoveSpeed => _moveSpeed;
        public float AttackCooldownSec => _attackCooldownSec;
    }
}
