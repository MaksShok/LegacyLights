using CommonLogic.HealthModule;

namespace CommonLogic.DamageModule
{
    public interface IDamageProvider
    {
        int Damage { get; }
        void ApplyDamage(ISpendHealth health);
    }
}