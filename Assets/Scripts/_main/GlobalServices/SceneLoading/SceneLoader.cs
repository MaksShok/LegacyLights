using System;
using System.Collections;
using _main.ServiceLoc;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _main.GlobalServices.SceneLoading
{
    /// <summary>
    /// Сервис для загрузки сцен синхронным и асинхронным способом
    /// </summary>
    public class SceneLoader : IService
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string sceneName, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, loadMode);
            onComplete?.Invoke();
        }
        
        public void LoadScene(int sceneIndex, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneIndex, loadMode);
            onComplete?.Invoke();
        }

        public void LoadSceneAsync(string sceneName, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            _coroutineRunner.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onComplete, loadMode));
        }
        
        public void LoadSceneAsync(int sceneIndex, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            _coroutineRunner.StartCoroutine(LoadSceneAsyncCoroutine(sceneIndex, onComplete, loadMode));
        }

        public void LoadSceneAsync(string sceneName, Action<float> onProgress, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            _coroutineRunner.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onProgress, onComplete, loadMode));
        }
        
        public void LoadSceneAsync(int sceneIndex, Action<float> onProgress, Action onComplete = null, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            _coroutineRunner.StartCoroutine(LoadSceneAsyncCoroutine(sceneIndex, onProgress, onComplete, loadMode));
        }

        private IEnumerator LoadSceneAsyncCoroutine(
            string sceneName,
            Action onComplete = null,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            yield return LoadSceneAsyncInternal(sceneName, null, onComplete, loadMode);
        }

        private IEnumerator LoadSceneAsyncCoroutine(
            int sceneIndex,
            Action onComplete = null,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            yield return LoadSceneAsyncInternal(sceneIndex, null, onComplete, loadMode);
        }

        private IEnumerator LoadSceneAsyncCoroutine(
            string sceneName,
            Action<float> onProgress,
            Action onComplete = null,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            yield return LoadSceneAsyncInternal(sceneName, onProgress, onComplete, loadMode);
        }

        private IEnumerator LoadSceneAsyncCoroutine(
            int sceneIndex,
            Action<float> onProgress,
            Action onComplete = null,
            LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            yield return LoadSceneAsyncInternal(sceneIndex, onProgress, onComplete, loadMode);
        }

        private IEnumerator LoadSceneAsyncInternal(
            string sceneName,
            Action<float> onProgress,
            Action onComplete,
            LoadSceneMode loadMode)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadMode);
            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                onProgress?.Invoke(asyncLoad.progress);
                yield return null;
            }

            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }

        private IEnumerator LoadSceneAsyncInternal(
            int sceneIndex,
            Action<float> onProgress,
            Action onComplete,
            LoadSceneMode loadMode)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, loadMode);
            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                onProgress?.Invoke(asyncLoad.progress);
                yield return null;
            }

            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }

        // public void UnloadUnusedScenes(Action onComplete = null)
        // {
        //     _coroutineRunner.StartCoroutine(UnloadUnusedScenesCoroutine(onComplete));
        // }

        // private IEnumerator UnloadUnusedScenesCoroutine(Action onComplete)
        // {
        //     
        //     AsyncOperation asyncUnload = SceneManager.UnloadUnusedAssetsAsync();
        //     
        //     while (!asyncUnload.isDone)
        //     {
        //         yield return null;
        //     }
        //
        //     onComplete?.Invoke();
        // }
    }
}
