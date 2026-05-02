using Assets.Scripts.Combat.Core;

namespace Assets.Scripts.Combat.Interfaces
{
    public interface IUsable
    {
        void Use(Entity user, ICombatant target = null);
        string GetUseDescription();
    }
}