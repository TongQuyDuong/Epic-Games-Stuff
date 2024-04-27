using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerDataGlobalManager : Node2D
{
	private StringName playerDataPath = "res://Resources/SaveData/PlayerData1.tres";
	private PlayerData playerData;

	public override void _EnterTree() {
		RecoveryItem.onHealingItemConsumed += HealPlayer;
	}

	public override void _ExitTree() {
		RecoveryItem.onHealingItemConsumed -= HealPlayer;
	}

	public override void _Ready() {
		playerData = GD.Load<PlayerData>("res://Resources/SaveData/PlayerData1.tres");

		foreach(KeyValuePair<EquipmentType,Equipment> equippedItem in playerData.equippedItems) {
			if (equippedItem.Value != null)
				equippedItem.Value.Equip(playerData);
		}
	}

	private void HealPlayer(Item item) {
		playerData.playerStats.TryGetStatValue(StatType.HP, out float maxHP);
		playerData.currentHP += ((RecoveryItem)item).recoveryValue;
		if (playerData.currentHP > maxHP) playerData.currentHP = (int)maxHP;

		playerData.RemoveItem(item,1);
	}

	public void DropItem(Item item)
	{
		if (item.itemType == ItemType.KeyItem) return;
		playerData.RemoveItem(item, 1);
	}

}
