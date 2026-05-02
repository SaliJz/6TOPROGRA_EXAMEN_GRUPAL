using System.Collections.Generic;

namespace Assets.Scripts.Joaquin.Core
{
    public enum CombatOutcome 
    { 
        PlayerWon, 
        PlayerLost, 
        PlayerFled 
    }

    public class CombatResult
    {
        public CombatOutcome Outcome { get; }
        public List<Item> Drops { get; }
        public string Summary { get; }

        public CombatResult(CombatOutcome outcome, List<Item> drops, string summary)
        {
            Outcome = outcome;
            Drops = drops ?? new List<Item>();
            Summary = summary;
        }
    }
}