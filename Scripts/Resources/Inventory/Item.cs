using Godot;
using System;
using MonoCustomResourceRegistry;


[GlobalClass]
[RegisteredType(nameof(Item), "", nameof(Resource))]
public partial class Item : Resource
{
    [Export] public CompressedTexture2D icon;
    [Export] public string itemName;
    [Export] public string itemDescription;
    [Export] public ItemType itemType;
    [Export] public bool isStackable;
    [Export] public int quantity;

    public Item(string name,ItemType type,bool isStackable,int quantity){
        itemName = name;
        itemType = type;
        this.quantity = quantity;
        this.isStackable = isStackable;
    }

    public Item() {
    }
}

public enum ItemType {
    None = 0,
    Equipment = 1,
    Consumable = 2,
    MonsterLoot = 3,
    KeyItem = 4
}
