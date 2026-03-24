using System;

namespace CommonLogic.HealthModule
{
    public class HealthModel : ISpendHealth
    {
        public event Action<float> Change;
        public event Action Die;
        public bool Alive => Health > 0;
        public int Health { get; private set; }

        private int _maxHealth;
        
        public HealthModel(int value)
        {
            _maxHealth = Math.Max(0, value);
            Set(value);
        }

        public void Spend(int value)
        {
            Set(Health - value);
        }

        public void SetHealth(int value)
        {
            _maxHealth = Math.Max(_maxHealth, value);
            Set(value);
        }

        private void Set(int value)
        {
            Health = Math.Max(0, value);
            if (_maxHealth != 0) Change?.Invoke((float)Health / _maxHealth);
            if (Health == 0) Die?.Invoke();
        }

        public void ClearAllSubscribers()
        {
            Die = null;
            Change = null;
        }
    }
}