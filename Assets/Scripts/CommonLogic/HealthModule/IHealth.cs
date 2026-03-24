using System;
using _main.ServiceLoc;

namespace CommonLogic.HealthModule
{
    public interface IHealth : IService
    {
        event Action<float> Change; // In percentages from 0 to 1
        event Action Die;
        int Health { get; }
        bool Alive { get; }
    }

    public interface ISpendHealth : IHealth
    {
        void Spend(int value);
    }
}