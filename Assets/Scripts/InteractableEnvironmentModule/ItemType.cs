using UnityEngine;

namespace _Project.Scripts.InteractableEnvironmentModule
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "ScriptableObjects/Items/ItemType")]
    public class ItemType : ScriptableObject
    {
        [field: SerializeField]
        public string Id { get; private set; }
		
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
    }
}