using Godot;
using System;

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
	}

	private void HealPlayer(Item item) {
		playerData.playerStats.TryGetStatValue(StatType.HP, out float maxHP);
		playerData.currentHP += ((RecoveryItem)item).recoveryValue;
		if (playerData.currentHP > maxHP) playerData.currentHP = (int)maxHP;

		playerData.RemoveItem(item,1);
	}

	private void DropItem(Item item)
	{
		playerData.RemoveItem(item, 1);
	}

}
