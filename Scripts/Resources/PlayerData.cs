using Godot;
using System;
using System.Collections.Generic;
using MonoCustomResourceRegistry;
using System.Linq;
using System.Diagnostics;

[RegisteredType(nameof(PlayerData), "", nameof(Resource))]
public partial class PlayerData : Resource
{
	[Export] public int currentHP;
	[Export] public UnitStatList playerStats;
	[Export] public Godot.Collections.Dictionary<Item,int> inventory;

	public void AddItem(Item item) {
		if (inventory == null) inventory = new Godot.Collections.Dictionary<Item, int>();
		if (inventory.ContainsKey(item)) {
			inventory[item] += 1;
		} else {
			inventory[item] = 1;
		}

		Debug.Print(inventory.Count.ToString());

		ResourceSaver.Save(this);
	}
}
