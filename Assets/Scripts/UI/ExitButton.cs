using UnityEngine;

namespace UI
{
    public class ExitButton : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        
            Debug.Log("Приложение закрывается");
        }
    }
}