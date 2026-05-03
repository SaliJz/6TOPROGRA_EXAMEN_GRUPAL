using System;

[Serializable]
public abstract class Item
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public ItemType Type { get; protected set; }

    protected Item(string name, string description, ItemType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }

    public abstract string GetUseDescription();
    public override string ToString() => $"[{Type}] {Name}: {Description}";
}