using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyModule.Configs
{
    [CreateAssetMenu(menuName = "ScriptableObjects/EnemyConfig/MeshikConfig", fileName = "MeshikConfig")]
    public class MeshikConfig : Abstract.EnemyConfig
    {
        [SerializeField] private float attackDistance = 0.5f;
        [SerializeField] private float _detectionDistance = 5f;

        public float AttackDistance => attackDistance;
        public float DetectionDistance => _detectionDistance;
    }
}