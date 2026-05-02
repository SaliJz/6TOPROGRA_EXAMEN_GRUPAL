using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Joaquin.Interfaces;

[Serializable]
public class Inventory
{
    private readonly List<Item> items = new List<Item>();
    public IReadOnlyList<Item> Items => items.AsReadOnly();
    public int Count => items.Count;
    public bool IsEmpty => items.Count == 0;

    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action OnInventoryChanged;

    public void AddItem(Item item)
    {
        if (item == null) return;
        items.Add(item);
        OnItemAdded?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(Item item)
    {
        bool removed = items.Remove(item);
        if (removed)
        {
            OnItemRemoved?.Invoke(item);
            OnInventoryChanged?.Invoke();
        }
        return removed;
    }

    public void Clear()
    {
        items.Clear();
        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(string itemName)
        => items.Exists(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

    public Item GetItem(string itemName)
        => items.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

    public List<Item> GetItemsByType(ItemType type)
        => items.FindAll(i => i.Type == type);

    public string UseItem(string itemName, IHealable user)
    {
        Item item = GetItem(itemName);
        if (item == null) return $"No tienes '{itemName}' en tu inventario.";
        if (item.Type == ItemType.CombatItem)
        {
            return $"{item.Name} es un item de combate. Usalo contra un enemigo.";
        }

        if (item is IUsable usable)
        {
            usable.Use(user);
            string result = item.GetUseDescription();
            RemoveItem(item);
            return result;
        }

        return $"{item.Name} no es usable.";
    }

    public string UseCombatItem(string itemName, IDamageable enemy)
    {
        Item item = GetItem(itemName);
        if (item == null) return $"No tienes '{itemName}' en tu inventario.";
        if (item.Type != ItemType.CombatItem)
        {
            return $"{item.Name} no puede usarse directamente de esta forma.";
        }

        CombatItem combatItem = (CombatItem)item;
        string result = combatItem.UseOnEnemy(enemy);
        RemoveItem(combatItem);
        return result;
    }

    public override string ToString()
    {
        if (IsEmpty) return "El inventario esta vacio.";
        StringBuilder sb = new StringBuilder("== Inventario ==\n");
        for (int i = 0; i < items.Count; i++)
        {
            sb.AppendLine($"  {i + 1}. {items[i]}");
        }
        return sb.ToString();
    }
}