using Godot;
using System;

public partial class SceneGlobalManager : Node2D
{
	private StringName battleScenePath = "res://Prefabs/Battles/TestBattle.tscn";
	public async void ChangeToNewScene(StringName nextScenePath) {
		OverworldLevel currentScene = GetTree().CurrentScene as OverworldLevel;
		LevelDataHandoff incomingData = currentScene.data;
		
		currentScene.player.transition.FadeOut();
		await ToSignal(currentScene.player.transition,"finished");

		OverworldLevel incomingScene = GD.Load<PackedScene>(nextScenePath).Instantiate<OverworldLevel>();
		incomingScene.data = incomingData;

		currentScene.QueueFree();

		GetTree().Root.CallDeferred(MethodName.AddChild,incomingScene);
		GetTree().SetDeferred("current_scene", incomingScene);

		incomingScene.EnterLevel();

		incomingScene.player.playerData.ChangeCurrentStage(nextScenePath);
	}

	public async void InitBattle(SpawnFormation spawnFormation) {
		OverworldLevel currentScene = GetTree().CurrentScene as OverworldLevel;

		currentScene.player.transition.FadeOut();
		await ToSignal(currentScene.player.transition, "finished");

		BattleScene battleScene = GD.Load<PackedScene>(battleScenePath).Instantiate<BattleScene>();
		battleScene.spawnFormation = spawnFormation;
		battleScene.overworldLevelPath = currentScene.SceneFilePath;
		battleScene.data = new LevelDataHandoff(currentScene.player.Position, currentScene.player.blendPos);

		currentScene.QueueFree();

		GetTree().Root.CallDeferred(MethodName.AddChild, battleScene);
		GetTree().SetDeferred("current_scene", battleScene);
	}

	public async void ReturnToOverWorld() {
		BattleScene currentScene = GetTree().CurrentScene as BattleScene;

		currentScene.transitionEffect.FadeOut();
		await ToSignal(currentScene.transitionEffect,"finished");

		OverworldLevel incomingScene = GD.Load<PackedScene>(currentScene.overworldLevelPath).Instantiate<OverworldLevel>();
		incomingScene.data = currentScene.data;

		currentScene.QueueFree();

		GetTree().Root.CallDeferred(MethodName.AddChild, incomingScene);
		GetTree().SetDeferred("current_scene", incomingScene);

		incomingScene.EnterLevel();
	}
}
