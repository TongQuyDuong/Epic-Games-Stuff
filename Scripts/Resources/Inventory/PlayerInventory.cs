using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(PlayerInventory), "", nameof(Resource))]
public partial class PlayerInventory : Resource {
    [Export] private Godot.Collections.Array<Item> items;

}