
namespace Assets.Scripts.Combat.Core
{
    public enum ActionType 
    { 
        Attack, 
        UseItem, 
        Flee 
    }

    public class CombatAction
    {
        public ActionType Type { get; }
        public string ActorName { get; }
        public string Description { get; }
        public int DamageDealt { get; }
        public Item ItemUsed { get; }

        public CombatAction(ActionType type, string actorName,
                            string description, int damageDealt = 0, Item itemUsed = null)
        {
            Type = type;
            ActorName = actorName;
            Description = description;
            DamageDealt = damageDealt;
            ItemUsed = itemUsed;
        }

        public override string ToString() => Description;
    }
}