// Core/Enemy.cs
using System.Collections.Generic;
using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;

public class Enemy : Entity
{
    private readonly List<Item> drops = new List<Item>();
    public IReadOnlyList<Item> Drops => drops.AsReadOnly();

    public Enemy(string name, int health, int damage) : base(name, health, damage) { }

    public void AddDrop(Item item)
    {
        if (item != null) drops.Add(item);
    }

    public int AttackPlayer(ICombatant target)
    {
        return Attack(target);
    }

    public override string ToString()
        => $"{Name} | HP: {CurrentHP}/{MaxHP} | Dano: {Damage}";
}