using Assets.Scripts.Combat.Core;
using Assets.Scripts.Combat.Interfaces;

namespace Assets.Scripts.Combat.Items
{
    public class HealthPotion : Item, IUsable
    {
        private readonly int healAmount;

        public HealthPotion(int healAmount) 
            : base("Poción de Vida", $"Restaura {healAmount} puntos de vida")
        {
            this.healAmount = healAmount;
        }

        public void Use(Entity user, ICombatant target = null)
        {
            user.Heal(healAmount);
        }

        public string GetUseDescription()
        {
            return $"Usas la Poción de Vida y recuperas {healAmount} HP.";
        }
    }
}