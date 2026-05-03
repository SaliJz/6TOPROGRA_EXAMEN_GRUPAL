using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Joaquin.Manager
{
    /// <summary>
    /// Gestiona el flujo de combate por turnos entre el jugador y un enemigo.
    /// </summary>
    public class CombatManager
    {
        #region Internal Classes

        // Clase interna para rastrear buffs correctamente
        private class BuffEntry
        {
            public readonly int BoostValue;
            public int TurnsRemaining;

            public BuffEntry(int boostValue, int turns)
            {
                BoostValue = boostValue;
                TurnsRemaining = turns;
            }
        }

        #endregion

        #region Internal State

        // Participantes
        private readonly Entity player;
        private readonly Entity enemy;

        // Estructuras de datos
        private readonly Queue<System.Action> turnQueue;
        private readonly List<CombatAction> combatLog;
        private readonly Dictionary<string, BuffEntry> activeBuffs;
        private readonly Stack<CombatAction> playerActionHistory;

        // Inventario del jugador
        private readonly Inventory playerInventory;

        // Estado
        private bool combatActive;
        private int currentPlayerDamageBoost;

        #endregion

        #region Public Properties

        // Propiedades publicas
        public bool CombatActive => combatActive;
        public IReadOnlyList<CombatAction> CombatLog => combatLog;
        public Entity Player => player;
        public Entity Enemy => enemy;

        #endregion

        #region Initialization

        public CombatManager(Entity player, Entity enemy, Inventory playerInventory)
        {
            this.player = player;
            this.enemy = enemy;
            this.playerInventory = playerInventory ?? new Inventory();

            turnQueue = new Queue<System.Action>();
            combatLog = new List<CombatAction>();
            activeBuffs = new Dictionary<string, BuffEntry>();
            playerActionHistory = new Stack<CombatAction>();

            combatActive = true;
            currentPlayerDamageBoost = 0;
        }

        #endregion

        #region Player Turn Logic

        /// <summary>
        /// Ejecuta el ataque basico del jugador.
        /// </summary>
        public CombatResult ExecutePlayerAttack()
        {
            int totalDamage = player.Damage + currentPlayerDamageBoost;
            int dealt = enemy.TakeDamage(totalDamage);

            var action = new CombatAction(
                ActionType.Attack,
                player.Name,
                $"{player.Name} ataca a {enemy.Name} causando {dealt} de dano.",
                dealt
            );

            combatLog.Add(action);
            playerActionHistory.Push(action);

            return CheckCombatEnd();
        }

        /// <summary>
        /// El jugador usa un item del inventario.
        /// </summary>
        public CombatResult ExecutePlayerUseItem(Item item)
        {
            bool found = false;
            foreach (var i in playerInventory.Items)
            {
                if (i == item) { found = true; break; }
            }
            if (!found) return null;

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
                return null;
            }

            playerInventory.RemoveItem(item);
            combatLog.Add(action);
            playerActionHistory.Push(action);

            return CheckCombatEnd();
        }

        /// <summary>
        /// El jugador huye del combate.
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

        #endregion

        #region Enemy Turn Logic

        /// <summary>
        /// Ejecuta el ataque del enemigo.
        /// </summary>
        public CombatResult ExecuteEnemyTurn()
        {
            int dealt = enemy.Attack(player);

            var action = new CombatAction(
                ActionType.Attack,
                enemy.Name,
                $"{enemy.Name} ataca a {player.Name} causando {dealt} de dano.",
                dealt
            );

            combatLog.Add(action);
            TickBuffs();

            return CheckCombatEnd();
        }

        #endregion

        #region Buff Management

        private void RegisterBuff(string buffName, int boost, int duration)
        {
            if (activeBuffs.ContainsKey(buffName))
            {
                currentPlayerDamageBoost -= activeBuffs[buffName].BoostValue;
                activeBuffs[buffName] = new BuffEntry(boost, duration);
            }
            else
            {
                activeBuffs[buffName] = new BuffEntry(boost, duration);
            }

            currentPlayerDamageBoost += boost;
        }

        private void TickBuffs()
        {
            var expired = new List<string>();

            foreach (var key in new List<string>(activeBuffs.Keys))
            {
                activeBuffs[key].TurnsRemaining--;
                if (activeBuffs[key].TurnsRemaining <= 0)
                    expired.Add(key);
            }

            foreach (var key in expired)
            {
                currentPlayerDamageBoost -= activeBuffs[key].BoostValue;
                activeBuffs.Remove(key);

                combatLog.Add(new CombatAction(
                    ActionType.UseItem,
                    player.Name,
                    $"El efecto de {key} ha terminado."
                ));
            }
        }

        #endregion

        #region Combat Resolution

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
            return null; // El combate continua
        }

        private CombatResult BuildResult(CombatOutcome outcome)
        {
            List<Item> drops = outcome == CombatOutcome.PlayerWon
                ? GetEnemyDrops()
                : new List<Item>();

            return new CombatResult(outcome, drops, GetCombatSummary());
        }

        /// <summary>
        /// Obtiene los drops del enemigo si es instancia de Enemy.
        /// </summary>
        private List<Item> GetEnemyDrops()
        {
            if (enemy is Enemy e)
            {
                return new List<Item>(e.Drops);
            }
            return new List<Item>();
        }

        #endregion

        #region UI & Data Access

        public string GetCombatSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Resumen del combate ===");
            foreach (var action in combatLog)
            {
                sb.AppendLine($"  * {action}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Retorna solo los items del inventario que implementan IUsable.
        /// </summary>
        public List<Item> GetUsableItems()
        {
            var usable = new List<Item>();
            foreach (var item in playerInventory.Items)
            {
                if (item is IUsable) usable.Add(item);
            }
            return usable;
        }

        public CombatUIData GetUIData()
        {
            string lastLog = combatLog.Count > 0
                ? combatLog[combatLog.Count - 1].ToString()
                : string.Empty;

            return new CombatUIData
            {
                PlayerName = player.Name,
                PlayerCurrentHP = player.CurrentHP,
                PlayerMaxHP = player.MaxHP,
                PlayerDamage = player.Damage,

                EnemyName = enemy.Name,
                EnemyCurrentHP = enemy.CurrentHP,
                EnemyMaxHP = enemy.MaxHP,
                EnemyDamage = enemy.Damage,

                CombatActive = combatActive,
                LastLogLine = lastLog
            };
        }

        #endregion
    }
}