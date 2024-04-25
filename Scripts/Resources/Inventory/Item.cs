using Godot;
using System;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(Item), "", nameof(Resource))]
public partial class Item : Resource
{
    [Export] public CompressedTexture2D icon;
    [Export] public string itemName;
    [Export] public string itemDescription;
    [Export] public ItemType itemType;
    [Export] public bool isStackable;
}

public enum ItemType {
    None = 0,
    Equipment = 1,
    Consumable = 2,
    MonsterLoot = 3,
    KeyItem = 4
}
