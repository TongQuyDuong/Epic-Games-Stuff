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

}

public enum ItemType {
    Equipment,
    Consumable,
    MonsterLoot,
    KeyItem
}
