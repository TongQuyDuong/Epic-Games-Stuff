using Godot;
using System;
using System.Diagnostics;

public partial class main : GameScene
{
	[Export] PackedScene battleScene;

	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	private async void _on_play_pressed(){
		transitionEffect.FadeOut();
		await ToSignal(transitionEffect,"finished");
		GetTree().ChangeSceneToPacked(battleScene);
	}
}


