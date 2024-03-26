using Godot;
using System;

public partial class SceneGlobalManager : Node2D
{
	public async void ChangeToNewScene(StringName nextScenePath) {
		OverworldLevel currentScene = GetTree().CurrentScene as OverworldLevel;
		LevelDataHandoff incomingData = currentScene.data;

		OverworldLevel incomingScene = GD.Load<PackedScene>(nextScenePath).Instantiate<OverworldLevel>();
		incomingScene.data = incomingData;
		
		currentScene.player.transition.FadeOut();
		await ToSignal(currentScene.player.transition,"finished");

		currentScene.QueueFree();

		GetTree().Root.CallDeferred(MethodName.AddChild,incomingScene);
		GetTree().SetDeferred("current_scene", incomingScene);

		incomingScene.EnterLevel();

	}
}
