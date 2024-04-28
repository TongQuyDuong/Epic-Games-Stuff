using Godot;
using System;
using System.Collections.Generic;
using MonoCustomResourceRegistry;
using System.Linq;
using System.Diagnostics;

[RegisteredType(nameof(PlayerData), "", nameof(Resource))]
public partial class PlayerData : Resource
{
	public static Action onPlayerInventoryChanged;
	[Export] public int currentHP;
	[Export] public UnitStatList playerStats;
	[Export] public Godot.Collections.Dictionary<Item,int> inventory;
	[Export] public Godot.Collections.Dictionary<EquipmentType, Equipment> equippedItems;

	public void AddItem(Item item, int quantity) {
		if (inventory == null) inventory = new Godot.Collections.Dictionary<Item, int>();
		if (inventory.ContainsKey(item)) {
			inventory[item] += quantity;
		} else {
			inventory[item] = quantity;
		}

		Debug.Print(inventory.Count.ToString());

		ResourceSaver.Save(this);
		onPlayerInventoryChanged?.Invoke();
	}

	public void RemoveItem(Item item, int quantity)
	{
		if (inventory.ContainsKey(item))
		{
			inventory[item] -= quantity;
		}
		else
		{
			return;
		}

		if (inventory[item] <= 0) {
			inventory.Remove(item);
		}

		ResourceSaver.Save(this);
		onPlayerInventoryChanged?.Invoke();
	}

	public void EquipItem(Equipment item) {
		if (equippedItems[item.equipmentType] != null) {
			UnequipItem(item.equipmentType);
		}
		equippedItems[item.equipmentType] = item;
		item.Equip(this);
		ResourceSaver.Save(this);
	}

	public void UnequipItem(EquipmentType type) {
		equippedItems[type].Unequip(this);
		equippedItems[type] = null;
		ResourceSaver.Save(this);	
	}
}
