using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;
using System;

[Serializable]
public class DamagePotion : Item, IUsable
{
    private readonly int boost;
    private readonly int durationTurns;

    public int Boost => boost;
    public int DurationTurns => durationTurns;

    public DamagePotion(string name, int boost, string description = "", int durationTurns = 1)
        : base(name,
               string.IsNullOrEmpty(description) ? $"Incrementa el dano en {boost} por {durationTurns} turnos." : description,
               ItemType.DamagePotion)
    {
        this.boost = boost;
        this.durationTurns = durationTurns;
    }

    public void Use(IHealable user, ICombatant target = null)
    {
        user.IncreaseDamage(boost);
    }

    public override string GetUseDescription()
        => $"Usaste {Name}: tu dano aumenta en {boost}.";
}