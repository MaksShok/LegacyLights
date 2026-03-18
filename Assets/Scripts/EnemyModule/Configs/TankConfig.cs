using UnityEngine;

namespace EnemyModule.Configs
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyConfig/TankConfig", fileName = "TankConfig")]
    public class TankConfig : Abstract.EnemyConfig
    {
        [Header("Параметры Танка")]
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _areaDamageRadius = 2f;
        [SerializeField] private float _knockbackForce = 5f;

        public float AttackRange => _attackRange;
        public float AreaDamageRadius => _areaDamageRadius;
        public float KnockbackForce => _knockbackForce;
    }
}