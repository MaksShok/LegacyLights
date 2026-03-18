using UnityEngine;

namespace EnemyModule.Configs
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyConfig/MeshikConfig", fileName = "MeshikConfig")]
    public class MeshikConfig : Abstract.EnemyConfig
    {
        [Header("Параметры Мешика")]
        [SerializeField] private float _attackRange = 0.5f;
        [SerializeField] private float _detectionRange = 5f;

        public float AttackRange => _attackRange;
        public float DetectionRange => _detectionRange;
    }
}