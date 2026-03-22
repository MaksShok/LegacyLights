using System;
using _main.ServiceLoc;

namespace CommonLogic.HealthModule
{
    public interface IHealth : IService
    {
        event Action<float> Change;
        event Action OnDie;
        int Health { get; }
        bool Alive { get; }
    }

    public interface ISpendHealth : IHealth
    {
        void Spend(int value);
    }
}