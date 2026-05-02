using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;
using System;
using UnityEngine;

[Serializable]
public class HealthPotion : Item, IUsable
{
    private readonly int healAmount;

    public HealthPotion(string name, int healAmount, string description = "")
        : base(name,
               string.IsNullOrEmpty(description) ? $"Restaura {healAmount} puntos de vida." : description,
               ItemType.HealthPotion)
    {
        this.healAmount = healAmount;
    }

    public void Use(IHealable user, ICombatant target = null)
    {
        user.Heal(healAmount);
    }

    public override string GetUseDescription()
        => $"Usaste {Name}: restauraste {healAmount} HP.";
}