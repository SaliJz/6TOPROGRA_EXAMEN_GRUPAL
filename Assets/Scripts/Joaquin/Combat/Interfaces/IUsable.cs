
namespace Assets.Scripts.Joaquin.Interfaces
{
    public interface IUsable
    {
        void Use(IHealable user, ICombatant target = null);
        string GetUseDescription();
    }
}