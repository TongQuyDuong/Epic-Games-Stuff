using Godot;
using System;

public partial class BattleScene : GameScene
{
	public SpawnFormation spawnFormation;
	public StringName overworldLevelPath;
	public static Action onReturnToOverworld;

	public override void _EnterTree() {
		onReturnToOverworld += ReturnToOverWorld;
	}

	public override void _ExitTree() {
		onReturnToOverworld -= ReturnToOverWorld;
	}
	public async override void _Ready() {
		if(spawnFormation != null) {
			UnitManager.Instance.UpdateFormation(spawnFormation);
		}

		await ToSignal(transitionEffect, "finished");
		GameManager.Instance.UpdateGameState(GameState.GenerateGrid);
	}

	private void ReturnToOverWorld() {
		SceneGlobalManager manager = GetTree().Root.GetNode("SceneGlobalManager") as SceneGlobalManager;
		manager.ReturnToOverWorld();
	}
}
