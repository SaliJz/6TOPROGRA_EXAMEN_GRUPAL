
namespace Assets.Scripts.Combat.Interfaces
{
    public interface IUsable
    {
        void Use(ICombatant target = null);
        string GetUseDescription();
    }
}