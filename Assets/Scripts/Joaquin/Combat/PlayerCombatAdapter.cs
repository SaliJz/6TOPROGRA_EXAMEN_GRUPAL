using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;

public class PlayerCombatAdapter : Entity
{
    private readonly Player wrappedPlayer;

    public PlayerCombatAdapter(Player player)
        : base(player.PlayerName, player.MaxHealth, player.Damage)
    {
        wrappedPlayer = player;
        CurrentHP = player.CurrentHealth; // sincroniza HP si ya tenia danio previo
    }

    /// <summary>
    /// Delega el dano al Player real para que sus eventos se disparen
    /// </summary>
    public override int TakeDamage(int amount)
    {
        int before = wrappedPlayer.CurrentHealth;
        wrappedPlayer.TakeDamage(amount);
        int actual = before - wrappedPlayer.CurrentHealth;
        CurrentHP = wrappedPlayer.CurrentHealth; // mantiene sincronizacion
        return actual;
    }

    /// <summary>
    /// Delega la curacion al Player real y sincroniza HP local.
    /// </summary>
    public override void Heal(int amount)
    {
        wrappedPlayer.Heal(amount);
        CurrentHP = wrappedPlayer.CurrentHealth;
    }

    /// <summary>
    /// Sincroniza el dano desde Player antes de atacar.
    /// </summary>
    public override int Attack(ICombatant target)
    {
        Damage = wrappedPlayer.Damage;
        return target.TakeDamage(Damage);
    }
}