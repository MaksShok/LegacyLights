using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField]
        public int WeaponInventoryCapacity { get; private set; }
        
        [field: SerializeField]
        public int AbilitiesInventoryCapacity { get; private set; }
    }
}