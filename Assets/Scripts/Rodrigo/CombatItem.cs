using Assets.Scripts.Joaquin.Interfaces;
using System;
using UnityEngine;

[Serializable]
public class CombatItem : Item, IUsable
{
    private readonly int damageToTarget;

    public CombatItem(string name, int damageToTarget, string description = "")
        : base(
            name, 
            string.IsNullOrEmpty(description) ? $"Inflige {damageToTarget} de dano a un enemigo." : description, 
            ItemType.CombatItem
        )
    {
        this.damageToTarget = damageToTarget;
    }

    public void Use(IHealable user, ICombatant target = null)
    {
        target?.TakeDamage(damageToTarget);
    }

    public string UseOnEnemy(IDamageable enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("[CombatItem] El objetivo enemigo es null.");
            return "No hay enemigo objetivo.";
        }
        enemy.TakeDamage(damageToTarget);
        return $"Usaste {Name} sobre {enemy.Name}: infligiste {damageToTarget} de dano.";
    }

    public override string GetUseDescription()
        => $"{Name} inflige {damageToTarget} de dano al enemigo.";
}