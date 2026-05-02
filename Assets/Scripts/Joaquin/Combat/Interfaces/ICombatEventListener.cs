using Assets.Scripts.Joaquin.Core;

namespace Assets.Scripts.Joaquin.Interfaces
{
    public interface ICombatEventListener
    {
        void OnCombatEnded(CombatOutcome outcome);
    }
}