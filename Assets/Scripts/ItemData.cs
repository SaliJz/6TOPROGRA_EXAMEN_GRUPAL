using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Item")]
public class ItemData : ScriptableObject
{
    public string nameES;
    public string nameEN;
    public string descriptionES;
    public string descriptionEN;
    public ItemType type;

    public int healAmount;

    public int damageBoost;
    public int boostDurationTurns = 2;

    public int damageToTarget;

    /// <summary>
    /// Construye el Item concreto en runtime según el tipo.
    /// </summary>
    public Item Build()
    {
        string n = LocalizationManager.GetText(nameES, nameEN);
        string desc = LocalizationManager.GetText(descriptionES, descriptionEN);

        return type switch
        {
            ItemType.HealthPotion => new HealthPotion(n, healAmount, desc),
            ItemType.DamagePotion => new DamagePotion(n, damageBoost, desc, boostDurationTurns),
            ItemType.CombatItem => new CombatItem(n, damageToTarget, desc),
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }
}