using Assets.Scripts.Combat.Core;
using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Combat.Items
{
    public class CombatItem : Item, IUsable
    {
        private readonly int damageToTarget;

        public CombatItem(string name, string description, int damageToTarget)
            : base(name, description)
        {
            this.damageToTarget = damageToTarget;
        }

        public void Use(Entity user, ICombatant target = null)
        {
            target?.TakeDamage(damageToTarget);
        }

        public string GetUseDescription()
        {
            return $"Usas {Name} y causas {damageToTarget} de daño al enemigo.";
        }
    }
}