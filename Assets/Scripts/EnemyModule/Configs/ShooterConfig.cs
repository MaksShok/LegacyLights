using UnityEngine;

namespace EnemyModule.Configs
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyConfig/ShooterConfig", fileName = "ShooterConfig")]
    public class ShooterConfig : Abstract.EnemyConfig
    {
        [Header("Параметры Стрелка")]
        [SerializeField] private float _attackRange = 8f;
        [SerializeField] private float _minAttackRange = 2f;
        [SerializeField] private float _projectileSpeed = 10f;

        public float AttackRange => _attackRange;
        public float MinAttackRange => _minAttackRange;
        public float ProjectileSpeed => _projectileSpeed;
    }
}