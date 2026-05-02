using UnityEngine;
using Assets.Scripts.Joaquin.Interfaces;

namespace Assets.Scripts.Joaquin.Core
{
    public abstract class Entity : ICombatant, IHealable
    {
        public string Name { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsAlive => CurrentHP > 0;

        protected Entity(string name, int maxHP, int damage)
        {
            Name = name;
            MaxHP = Mathf.Max(1, maxHP);
            CurrentHP = MaxHP;
            Damage = Mathf.Max(0, damage);
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
            CurrentHP = Mathf.Min(MaxHP, CurrentHP + Mathf.Max(0, amount));
        }

        public virtual void IncreaseDamage(int amount)
        {
            Damage += Mathf.Max(0, amount);
        }
    }
}