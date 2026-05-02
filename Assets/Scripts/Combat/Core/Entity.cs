
using UnityEngine;
using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Combat.Core
{
    public abstract class Entity : ICombatant
    {
        public string Name { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsAlive => CurrentHP > 0;

        protected Entity(string name, int maxHP, int damage)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Damage = damage;
        }

        public virtual int TakeDamage(int amount)
        {
            int actual = Mathf.Max(0, amount);
            CurrentHP = Mathf.Max(0, CurrentHP - actual);
            return actual;
        }

        public virtual int Attack(ICombatant target)
        {
            return target.TakeDamage(Damage);
        }

        public virtual void Heal(int amount)
        {
            CurrentHP = Mathf.Min(MaxHP, CurrentHP + amount);
        }
    }
}