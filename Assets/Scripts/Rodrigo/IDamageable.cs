
public interface IDamageable
{
    string Name { get; }
    int CurrentHealth { get; }
    bool IsAlive { get; }

    void TakeDamage(int amount);
}