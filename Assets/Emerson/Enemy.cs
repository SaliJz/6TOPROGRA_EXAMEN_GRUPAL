using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string Name;
    public int Health;
    public int Damage;

    public Enemy(string name, int health, int damage)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public void Attack(Player player)
    {
        player.TakeDamage(Damage);
        Debug.Log(Name + " atacó e hizo " + Damage + " de daño");
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void TakeTurn(Player player)
    {
        if (IsAlive())
        {
            Attack(player);
        }
    }
}
