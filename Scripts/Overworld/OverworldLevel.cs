using Godot;
using System;

public partial class OverworldLevel : GameScene
{
	[Export] TestHOverworld player;
	[Export] Godot.Collections.Array<OverworldExit> exits;

	public override void _Ready() {
		player.SetPhysicsProcess(false);
		player.Visible = false;
		
	}
}
