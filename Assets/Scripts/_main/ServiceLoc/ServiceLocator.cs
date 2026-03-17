using System;
using System.Collections.Generic;
using UnityEngine;

namespace _main.ServiceLoc
{
    public class ServiceLocator
    {
        public static ServiceLocator Current { get; private set; }
        public static void Initialize()
        {
            if (Current != null)
                return;
            Current = new ServiceLocator();
        }

        private readonly Dictionary<string, IService> _globalServices = new();
        public void GlobalRegister<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (!CheckRegistration(key))
            {
                _globalServices.Add(key, service);
            }
        }

        private readonly Dictionary<string, IService> _localServices = new();
        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (!CheckRegistration(key))
            {
                _localServices.Add(key, service);
            }
        }

        public void UnRegister<T>() where T : IService
        {
            string key = typeof(T).Name;
            
            _globalServices.Remove(key);
            _localServices.Remove(key);
        }

        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;
            IService service = null;

            if (_globalServices.TryGetValue(key, out service))
                return (T)service;
            
            if (_localServices.TryGetValue(key, out service))
                return (T)service;
            
            Debug.LogErrorFormat($"Сервис {key} отсутствует в контейнере");
            throw new InvalidOperationException();
        }

        public void ClearGlobal() => _globalServices.Clear();
        public void ClearLocal() => _localServices.Clear();

        private bool CheckRegistration(string key)
        {
            if (_globalServices.ContainsKey(key))
            {
                Debug.LogErrorFormat($"Зависимость {key} уже зарегистрирована в глобальном контейнере");
                return true;
            }
            
            if (_localServices.ContainsKey(key))
            {
                Debug.LogErrorFormat($"Зависимость {key} уже зарегистрирована в локальном контейнере");
                return true;
            }

            return false;
        }
    }
}