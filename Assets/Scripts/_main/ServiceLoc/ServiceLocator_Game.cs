using UnityEngine;

namespace _main.ServiceLoc
{
    
    public class ServiceLocator_Game : MonoBehaviour
    {
        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Clear()
        {
            
        }
    }
}