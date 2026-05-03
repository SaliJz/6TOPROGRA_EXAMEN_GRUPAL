using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyNameES;
    public string enemyNameEN;
    public int health;
    public int damage;
    public ItemData[] drops;
}