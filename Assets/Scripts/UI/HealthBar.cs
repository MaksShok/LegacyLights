using System;
using CommonLogic.HealthModule;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image _filler;

        private IHealth _healthModel;
    
        public void Initialize(IHealth healthModel)
        {
            _healthModel = healthModel;
            _healthModel.Change += OnHealthChange;
        }

        private void OnHealthChange(float healthPercentage)
        {
            _filler.fillAmount = Mathf.Clamp01(healthPercentage);
        }

        private void OnDestroy()
        {
            _healthModel.Change -= OnHealthChange;
        }
    }
}