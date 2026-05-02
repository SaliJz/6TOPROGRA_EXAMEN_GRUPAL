using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class Inventory
{
    private readonly List<Item> _items = new List<Item>();
    public IReadOnlyList<Item> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public bool IsEmpty => _items.Count == 0;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action OnInventoryChanged;

    public void AddItem(Item item)
    {
        if (item == null) return;

        _items.Add(item);
        OnItemAdded?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }
    public bool RemoveItem(Item item)
    {
        bool removed = _items.Remove(item);
        if (removed)
        {
            OnItemRemoved?.Invoke(item);
            OnInventoryChanged?.Invoke();
        }
        return removed;
    }

    public void Clear()
    {
        _items.Clear();
        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(string itemName)
        => _items.Exists(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

    public Item GetItem(string itemName)
        => _items.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

    public List<Item> GetItemsByType(ItemType type)
        => _items.FindAll(i => i.Type == type);

    public string UseItem(string itemName, Player player)
    {
        Item item = GetItem(itemName);
        if (item == null)
            return $"No tienes '{itemName}' en tu inventario.";

        if (item.Type == ItemType.CombatItem)
            return $"{item.Name} es un ítem de combate. Úsalo contra un enemigo.";

        item.Use(player);
        string result = item.GetUseDescription();
        RemoveItem(item);
        return result;
    }

    public string UseCombatItem(string itemName, IDamageable enemy)
    {
        Item item = GetItem(itemName);
        if (item == null)
            return $"No tienes '{itemName}' en tu inventario.";

        if (item.Type != ItemType.CombatItem)
            return $"{item.Name} no puede usarse directamente en combate de esta forma.";

        CombatItem combatItem = (CombatItem)item;
        string result = combatItem.UseOnEnemy(enemy);
        RemoveItem(combatItem);
        return result;
    }

    public override string ToString()
    {
        if (IsEmpty) return "El inventario está vacío.";

        StringBuilder sb = new StringBuilder("── Inventario ──\n");
        for (int i = 0; i < _items.Count; i++)
            sb.AppendLine($"  {i + 1}. {_items[i]}");
        return sb.ToString();
    }
}
