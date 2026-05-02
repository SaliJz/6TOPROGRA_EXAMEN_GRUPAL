using Assets.Scripts.Joaquin.Core;

namespace Assets.Scripts.Joaquin.Interfaces
{
    public interface IUsable
    {
        void Use(Entity user, ICombatant target = null);
        string GetUseDescription();
    }
}