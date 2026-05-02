using System;
using UnityEngine;

[Serializable]
public class HealthPotion : Item
{
    public int HealAmount { get; private set; }

    public HealthPotion(string name, int healAmount, string description = "") : base(name, string.IsNullOrEmpty(description) ? $"Restaura {healAmount} puntos de vida." : description, ItemType.HealthPotion)
    {
        HealAmount = healAmount;
    }

    public override void Use(Player player)
    {
        player.Heal(HealAmount);
    }

    public override string GetUseDescription()
        => $"Usaste {Name}: restauraste {HealAmount} HP.";
}
