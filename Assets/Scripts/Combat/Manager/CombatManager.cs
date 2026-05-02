using Assets.Scripts.Combat.Core;
using Assets.Scripts.Combat.Interfaces;
using Assets.Scripts.Combat.Items;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Combat.Manager
{
    /// <summary>
    /// Gestiona el flujo de combate por turnos entre el jugador y un enemigo.
    /// </summary>
    public class CombatManager
    {
        // Participantes 
        private readonly Entity player;
        private readonly Entity enemy;

        // Estructuras de datos
        private readonly Queue<System.Action> turnQueue;           // cola de turnos
        private readonly List<CombatAction> combatLog;             // historial completo
        private readonly Dictionary<string, int> activeBuffs;      // buff y turnos restantes
        private readonly Stack<CombatAction> playerActionHistory;  // acciones reversibles del jugador

        // Inventario del jugador
        private readonly List<Item> playerInventory;

        // Estado del combate
        private bool combatActive;
        private int currentPlayerDamageBoost;

        // Propiedades públicas
        public bool CombatActive => combatActive;
        public IReadOnlyList<CombatAction> CombatLog => combatLog;
        public Entity Player => player;
        public Entity Enemy => enemy;

        public CombatManager(Entity player, Entity enemy, List<Item> playerInventory)
        {
            this.player = player;
            this.enemy = enemy;
            this.playerInventory = playerInventory ?? new List<Item>();

            turnQueue = new Queue<System.Action>();
            combatLog = new List<CombatAction>();
            activeBuffs = new Dictionary<string, int>();
            playerActionHistory = new Stack<CombatAction>();

            combatActive = true;
            currentPlayerDamageBoost = 0;
        }

        /// <summary>
        /// Ejecuta el ataque básico del jugador. Retorna el resultado si el combate terminó.
        /// </summary>
        public CombatResult ExecutePlayerAttack()
        {
            int totalDamage = player.Damage + currentPlayerDamageBoost;
            int dealt = enemy.TakeDamage(totalDamage);

            var action = new CombatAction(
                ActionType.Attack,
                player.Name,
                $"{player.Name} ataca a {enemy.Name} causando {dealt} de daño.",
                dealt
            );

            combatLog.Add(action);
            playerActionHistory.Push(action);

            return CheckCombatEnd();
        }

        /// <summary>
        /// El jugador usa un item del inventario. Retorna null si el combate continúa.
        /// </summary>
        public CombatResult ExecutePlayerUseItem(Item item)
        {
            if (!playerInventory.Contains(item)) return null;

            CombatAction action;

            if (item is DamagePotion damagePotion)
            {
                RegisterBuff(damagePotion.Name, damagePotion.Boost, damagePotion.DurationTurns);
                action = new CombatAction(
                    ActionType.UseItem, 
                    player.Name, 
                    damagePotion.GetUseDescription(), 
                    0, 
                    item
                );
            }
            else if (item is IUsable usable)
            {
                usable.Use(player, enemy);
                action = new CombatAction(
                    ActionType.UseItem, 
                    player.Name,
                    usable.GetUseDescription(), 
                    0, 
                    item
                );
            }
            else
            {
                return null; // item no usable
            }

            playerInventory.Remove(item);
            combatLog.Add(action);
            playerActionHistory.Push(action);

            return CheckCombatEnd();
        }

        /// <summary>
        /// El jugador intenta huir del combate.
        /// </summary>
        public CombatResult ExecutePlayerFlee()
        {
            combatActive = false;
            var action = new CombatAction(
                ActionType.Flee, 
                player.Name,
                $"{player.Name} huye del combate."
                );

            combatLog.Add(action);

            return BuildResult(CombatOutcome.PlayerFled);
        }

        /// <summary>
        /// Ejecuta el ataque del enemigo. Retorna el resultado si el combate terminó.
        /// </summary>
        public CombatResult ExecuteEnemyTurn()
        {
            int dealt = enemy.Attack(player);

            var action = new CombatAction(
                ActionType.Attack,
                enemy.Name,
                $"{enemy.Name} ataca a {player.Name} causando {dealt} de daño.",
                dealt
            );

            combatLog.Add(action);
            TickBuffs();

            return CheckCombatEnd();
        }

        private void RegisterBuff(string buffName, int boost, int duration)
        {
            if (activeBuffs.ContainsKey(buffName))
            {
                activeBuffs[buffName] = duration; // reinicia duración
            }
            else
            {
                activeBuffs[buffName] = duration;
            }

            currentPlayerDamageBoost += boost;
        }

        private void TickBuffs()
        {
            var expired = new List<string>();

            foreach (var buff in new List<string>(activeBuffs.Keys))
            {
                activeBuffs[buff]--;
                if (activeBuffs[buff] <= 0) expired.Add(buff);
            }

            foreach (var buff in expired)
            {
                activeBuffs.Remove(buff);
                combatLog.Add(new CombatAction(
                    ActionType.UseItem, 
                    player.Name,
                    $"El efecto de {buff} ha terminado.")
                );
            }
        }

        private CombatResult CheckCombatEnd()
        {
            if (!enemy.IsAlive)
            {
                combatActive = false;
                return BuildResult(CombatOutcome.PlayerWon);
            }

            if (!player.IsAlive)
            {
                combatActive = false;
                return BuildResult(CombatOutcome.PlayerLost);
            }

            return null; // El combate continúa
        }

        private CombatResult BuildResult(CombatOutcome outcome)
        {
            List<Item> drops = outcome == CombatOutcome.PlayerWon
                ? GetEnemyDrops()
                : new List<Item>();

            return new CombatResult(outcome, drops, GetCombatSummary());
        }

        private List<Item> GetEnemyDrops()
        {
            return new List<Item>();
        }

        public string GetCombatSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Resumen del combate ===");
            foreach (var action in combatLog)
            {
                sb.AppendLine($"  • {action}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Devuelve los items disponibles del jugador que implementan IUsable.
        /// </summary>
        public List<Item> GetUsableItems()
        {
            var usable = new List<Item>();
            foreach (var item in playerInventory)
            {
                if (item is IUsable) usable.Add(item);
            }
            return usable;
        }
    }
}