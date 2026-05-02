using Assets.Scripts.Joaquin.Interfaces;
using Assets.Scripts.Joaquin.Core;
using UnityEngine;

public class SituationManager : MonoBehaviour, ICombatEventListener
{
    [SerializeField] private CombatController combatController;

    private Player player;

    private void TriggerCombat(Enemy enemy)
    {
        combatController.gameObject.SetActive(true);
        combatController.StartCombat(player, enemy, this);
    }

    public void OnCombatEnded(CombatOutcome outcome)
    {
        switch (outcome)
        {
            case CombatOutcome.PlayerWon:
                break;
            case CombatOutcome.PlayerLost:
                break;
            case CombatOutcome.PlayerFled:
                break;
        }
    }
}