using UnityEngine;

public enum SituationType 
{ 
    Text, 
    Event, 
    Combat 
}

[CreateAssetMenu(menuName = "TextAdventure/Situation")]
public class SituationData : ScriptableObject
{
    [TextArea(3, 8)]
    public string descriptionES;
    [TextArea(3, 8)]
    public string descriptionEN;

    public SituationType type;
    public SituationOption[] options;

    public EnemyData enemyData;
}

[System.Serializable]
public class SituationOption
{
    public string labelES;
    public string labelEN;
    public SituationEffect effect;
    public int nextSituationIndex; // -1 = final
}

[System.Serializable]
public class SituationEffect
{
    public int hpChange;
    public int damageChange;
    public ItemData[] itemsGiven;
    public int karmaChange; // para calcular el final (bueno/neutral/malo)
}