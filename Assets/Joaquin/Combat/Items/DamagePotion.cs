using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;

namespace Assets.Scripts.Joaquin.Items
{
    public class DamagePotion : Item, IUsable
    {
        private readonly int boost;
        private readonly int durationTurns;

        public DamagePotion(int boost, int durationTurns) 
            : base("Poción de Fuerza", $"Aumenta el daño en {boost} por {durationTurns} turnos")
        {
            this.boost = boost;
            this.durationTurns = durationTurns;
        }

        public void Use(Entity user, ICombatant target = null)
        {
        }

        public int Boost => boost;
        public int DurationTurns => durationTurns;

        public string GetUseDescription()
        {
            return $"Usas la Poción de Fuerza. Tu daño aumenta en {boost} por {durationTurns} turnos.";
        }
    }
}