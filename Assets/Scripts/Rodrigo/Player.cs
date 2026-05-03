using Assets.Scripts.Joaquin.Interfaces;
using System;
using UnityEngine;

[Serializable]
public class Player : IHealable
{

    public string PlayerName { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Damage { get; private set; }
    public bool IsAlive => CurrentHealth > 0;

    public Inventory Inventory { get; private set; }
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnDamageChanged;
    public event Action OnPlayerDied;
    public event Action OnPlayerRevived;

    public Player(string playerName, int maxHealth, int damage)
    {
        PlayerName = string.IsNullOrWhiteSpace(playerName) ? "Aventurero" : playerName.Trim();
        MaxHealth = Mathf.Max(1, maxHealth);
        CurrentHealth = MaxHealth;
        Damage = Mathf.Max(1, damage);
        Inventory = new Inventory();
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;

        amount = Mathf.Max(0, amount);
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (!IsAlive)
            OnPlayerDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (!IsAlive) return;

        amount = Mathf.Max(0, amount);
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void IncreaseDamage(int amount)
    {
        Damage = Damage + Mathf.Max(0, amount);
        OnDamageChanged?.Invoke(Damage);
    }

    public void IncreaseMaxHealth(int amount)
    {
        amount = Mathf.Max(0, amount);
        MaxHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void Revive()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnPlayerRevived?.Invoke();
    }

    public void Reset(int maxHealth, int damage)
    {
        MaxHealth = Mathf.Max(1, maxHealth);
        CurrentHealth = MaxHealth;
        Damage = Mathf.Max(0, damage);
        Inventory.Clear();

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnDamageChanged?.Invoke(Damage);
    }

    public string GetStatusText()
        => $"Nombre: {PlayerName} | HP: {CurrentHealth}/{MaxHealth} | Daño: {Damage}";

    public override string ToString()
        => $"{GetStatusText()}\n{Inventory}";
}