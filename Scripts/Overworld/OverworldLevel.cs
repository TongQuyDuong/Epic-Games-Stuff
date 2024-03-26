using Godot;
using System;

public partial class OverworldLevel : GameScene
{
	public static Action<OverworldExit> onPlayerExited;
	public LevelDataHandoff data;
	[Export] public TestHOverworld player;
	[Export] Godot.Collections.Array<OverworldExit> exits;

	public override void _EnterTree() {
		onPlayerExited += OnPlayerExited;
	}

	public override void _ExitTree() {
		onPlayerExited -= OnPlayerExited;
	}
	public override void _Ready() {
		if(data == null) {
			player.SetPhysicsProcess(true);
			player.Visible = true;
		}
	}

	public void EnterLevel() {
		if(data != null) {
			foreach(OverworldExit exit in exits) {
				if(exit.name == data.doorName) {
					player.Position = exit.GetPlayerEntryVector();
					player.blendPos = exit.GetMoveDirection();
					break;
				}
			}
		}
	}

	private void OnPlayerExited(OverworldExit exit) {
		data = new LevelDataHandoff(exit.nextEntranceName,exit.GetMoveDirection());
		SceneGlobalManager manager = GetTree().Root.GetNode("SceneGlobalManager") as SceneGlobalManager;
		manager.ChangeToNewScene(exit.nextScenePath);
	}
}
