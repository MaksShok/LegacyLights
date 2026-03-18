using CommonLogic.HealthModule;
using UnityEngine;

namespace EnemyModule
{
    /// <summary>
    /// Компонент для получения урона от атаки игрока (через SendMessage)
    /// </summary>
    public class EnemyHealthReceiver : MonoBehaviour
    {
        private HealthModel _healthModel;

        public void Initialize(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        /// <summary>
        /// Вызывается через SendMessage из PlayerController
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (_healthModel != null)
            {
                _healthModel.Spend((int)damage);
            }
        }
    }
}
