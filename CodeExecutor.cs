using Godot;
using System;
using System.Diagnostics;

public partial class CodeExecutor : Node2D
{
	PlayerData playerData;
	public override void _Ready() {
		playerData = GD.Load<PlayerData>("res://Resources/SaveData/PlayerData1.tres");
		for (int i = 0; i < 20; i ++) {
			playerData.inventory.Add(new Item("Item"+i,ItemType.Equipment,false,1));
		}

		Debug.Print("Done");
	}
}
