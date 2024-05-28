using Godot;
using System;
using System.Diagnostics;

public partial class main : GameScene
{
	[Export] PlayerData playerData;

	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	private async void _on_play_pressed(){
		transitionEffect.FadeOut();
		await ToSignal(transitionEffect,"finished");
		GetTree().ChangeSceneToFile(playerData.currentStage);
	}
}


