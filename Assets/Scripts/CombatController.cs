using System.Collections.Generic;
using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;
using Assets.Scripts.Joaquin.Manager;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("Referencia a la UI de combate")]
    [SerializeField] private CombatUI combatUI;

    // Estado interno
    private CombatManager combatManager;
    private PlayerCombatAdapter playerAdapter;
    private Player currentPlayer;
    private Enemy currentEnemy;
    private ICombatEventListener combatListener;

    // Bloqueo durante animacion de turno
    private bool processingTurn = false;

    private void Awake()
    {
        combatUI.OnAttackPressed += HandleAttack;
        combatUI.OnFleePressed += HandleFlee;
        combatUI.OnItemSelected += HandleItemSelected;
        combatUI.OnResultContinuePressed += HandleResultContinue;
    }

    private void OnDestroy()
    {
        combatUI.OnAttackPressed -= HandleAttack;
        combatUI.OnFleePressed -= HandleFlee;
        combatUI.OnItemSelected -= HandleItemSelected;
        combatUI.OnResultContinuePressed -= HandleResultContinue;
    }

    /// <summary>
    /// Punto de entrada publico.
    /// </summary>
    public void StartCombat(Player player, Enemy enemy, ICombatEventListener listener)
    {
        currentPlayer = player;
        currentEnemy = enemy;
        combatListener = listener;

        playerAdapter = new PlayerCombatAdapter(player);
        combatManager = new CombatManager(playerAdapter, enemy, player.Inventory);

        combatUI.ClearLog();
        combatUI.HideResult();

        // Render inicial
        RefreshUI();
        combatUI.AppendLog($"Encuentras a {enemy.Name}. El combate comienza!");
    }

    private void HandleAttack()
    {
        if (!CanAct()) return;
        processingTurn = true;

        // Turno del jugador
        var result = combatManager.ExecutePlayerAttack();
        RefreshUI();

        if (result != null) { EndCombat(result); return; }

        // Turno del enemigo
        result = combatManager.ExecuteEnemyTurn();
        RefreshUI();

        if (result != null) EndCombat(result);
        else processingTurn = false;
    }

    private void HandleFlee()
    {
        if (!CanAct()) return;
        processingTurn = true;

        var result = combatManager.ExecutePlayerFlee();
        RefreshUI();
        EndCombat(result);
    }

    private void HandleItemSelected(Item item)
    {
        if (!CanAct()) return;
        processingTurn = true;

        // Turno del jugador usando el item
        var result = combatManager.ExecutePlayerUseItem(item);
        RefreshUI();

        if (result != null) { EndCombat(result); return; }

        // Turno del enemigo
        result = combatManager.ExecuteEnemyTurn();
        RefreshUI();

        if (result != null) EndCombat(result);
        else processingTurn = false;
    }

    private void HandleResultContinue()
    {
        combatListener?.OnCombatEnded(
            currentPlayer.IsAlive ? CombatOutcome.PlayerWon : CombatOutcome.PlayerLost
        );
        gameObject.SetActive(false);
    }

    private void EndCombat(CombatResult result)
    {
        if (result.Outcome == CombatOutcome.PlayerWon)
        {
            foreach (var drop in result.Drops)
            {
                currentPlayer.Inventory.AddItem(drop);
            }
        }

        string title, body, btnLabel;
        BuildResultText(result, out title, out body, out btnLabel);

        combatUI.AppendLog(result.Summary);
        combatUI.ShowResult(title, body, btnLabel);

        processingTurn = false;
    }

    private void BuildResultText(CombatResult result, 
        out string title, out string body, out string btnLabel)
    {
        switch (result.Outcome)
        {
            case CombatOutcome.PlayerWon:
                title = "Victoria!";
                btnLabel = "Continuar";
                body = result.Drops.Count > 0
                    ? BuildDropsText(result.Drops)
                    : $"Has derrotado a {currentEnemy.Name}.";
                break;

            case CombatOutcome.PlayerLost:
                title = "Derrota...";
                body = $"Has sido derrotado por {currentEnemy.Name}.";
                btnLabel = "Volver a intentar";
                break;

            case CombatOutcome.PlayerFled:
                title = "Huiste";
                body = "Escapaste del combate.";
                btnLabel = "Continuar";
                break;

            default:
                title = body = btnLabel = string.Empty;
                break;
        }
    }

    private string BuildDropsText(List<Item> drops)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine($"Has derrotado a {currentEnemy.Name}.");
        sb.AppendLine("Obtuviste:");
        foreach (var drop in drops) sb.AppendLine($"  + {drop.Name}");
        return sb.ToString();
    }

    private void RefreshUI()
    {
        combatUI.Refresh(combatManager.GetUIData());
        combatUI.PopulateItemPanel(combatManager.GetUsableItems());
    }

    private bool CanAct()
    {
        if (processingTurn)
        {
            Debug.LogWarning("[CombatController] Turno en proceso, accion ignorada.");
            return false;
        }
        if (combatManager == null || !combatManager.CombatActive) return false;
        return true;
    }
}