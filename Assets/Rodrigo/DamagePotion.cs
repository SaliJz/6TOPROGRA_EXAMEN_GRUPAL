using System;

[Serializable]
public class DamagePotion : Item
{
    public int DamageBoost { get; private set; }

    public DamagePotion(string name, int damageBoost, string description = "") : base(name, string.IsNullOrEmpty(description) ? $"Incrementa el daño en {damageBoost} puntos." : description, ItemType.DamagePotion)
    {
        DamageBoost = damageBoost;
    }

    public override void Use(Player player)
    {
        player.IncreaseDamage(DamageBoost);
    }

    public override string GetUseDescription()
        => $"Usaste {Name}: tu daño aumentó en {DamageBoost} puntos.";
}
