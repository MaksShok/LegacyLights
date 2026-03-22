using _main.ServiceLoc;
using UI;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IService
    {
        [field: SerializeField] 
        public HealthBar HealthBar { get; private set; }
    }
}