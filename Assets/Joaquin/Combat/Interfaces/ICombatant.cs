
namespace Assets.Scripts.Joaquin.Interfaces
{
    public interface ICombatant
    {
        string Name { get; }
        int CurrentHP { get; }
        int MaxHP { get; }
        int Damage { get; }
        bool IsAlive { get; }

        int TakeDamage(int amount);
        int Attack(ICombatant target);
    }
}