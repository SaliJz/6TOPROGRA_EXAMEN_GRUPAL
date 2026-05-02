using System;
using UnityEngine;

[Serializable]
public class CombatItem : Item
{
    public int DamageToEnemy { get; private set; }

    public CombatItem(string name, int damageToEnemy, string description = "") : base(name, string.IsNullOrEmpty(description) ? $"Inflige {damageToEnemy} de daño a un enemigo." : description, ItemType.CombatItem)
    {
        DamageToEnemy = damageToEnemy;
    }
    public override void Use(Player player)
    {
        Debug.LogWarning($"[CombatItem] {Name} requiere un objetivo enemigo. Usa UseOnEnemy().");
    }

    public string UseOnEnemy(IDamageable enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("[CombatItem] El objetivo enemigo es null.");
            return "No hay enemigo objetivo.";
        }

        enemy.TakeDamage(DamageToEnemy); // leer esto emerson
        return $"Usaste {Name} sobre {enemy.Name}: infligiste {DamageToEnemy} de daño.";  // leer esto emerson
    }

    public override string GetUseDescription()
        => $"{Name} inflige {DamageToEnemy} de daño al enemigo.";
}