using System;

namespace CommonLogic.HealthModule
{
    public interface IHealth
    {
        event Action OnDie;
        int Health { get; }
        bool Alive { get; }
    }

    public interface ISpendHealth : IHealth
    {
        void Spend(int value);
    }
}