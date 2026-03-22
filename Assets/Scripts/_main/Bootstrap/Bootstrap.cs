using _main.GlobalServices.SceneLoading;
using _main.ServiceLoc;
using Misc;
using UnityEngine;

namespace _main.Bootstrap
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        private void Awake()
        {
            var sceneLoader = new SceneLoader(this);

            ServiceLocator.Initialize();
            ServiceLocator.Current.GlobalRegister(sceneLoader);
            
            // Если у нас уже есть прогресс, то загружаем GameScene
            // Если прогресса нет, то загружаем меню
            int sceneIndex = Scenes.Game;
            sceneLoader.LoadScene(sceneIndex);
        }
    }
}