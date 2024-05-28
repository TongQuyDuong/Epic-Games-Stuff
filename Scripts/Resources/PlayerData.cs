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
	[Export] public string currentStage = "res://Prefabs/Scenes/stage_1.tscn";
	[Export] public int currentHP;
	[Export] public UnitStatList playerStats;
	[Export] public Godot.Collections.Dictionary<Item,int> inventory;
	[Export] public Godot.Collections.Dictionary<EquipmentType, Equipment> equippedItems;
	[Export] public Godot.Collections.Dictionary<Ability,bool> skills;

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

	public void AddAbility(Ability ability)
	{
		if (skills == null) inventory = new Godot.Collections.Dictionary<Item, int>();
		if (skills.ContainsKey(ability))
		{

		}
		else
		{
			skills[ability] = false;
		}

		Debug.Print(skills.Count.ToString());

		ResourceSaver.Save(this);
	}

	public void EquipAbility(Ability ability) {
		skills[ability] = true;
		ResourceSaver.Save(this);
	}

	public void UnequipAbility(Ability ability)
	{
		skills[ability] = false;
		ResourceSaver.Save(this);
	}

	public void ChangeCurrentStage(StringName nextStagePath) {
		currentStage = nextStagePath;
		ResourceSaver.Save(this);
	}
}
