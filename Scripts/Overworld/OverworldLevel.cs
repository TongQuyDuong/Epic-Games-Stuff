using Godot;
using System;

public partial class OverworldLevel : GameScene
{
	public static Action<OverworldExit> onPlayerExited;
	public static Action<SpawnFormation> onEnemyEncountered;
	[Export] public PlayerOverworld player;
	[Export] Godot.Collections.Array<OverworldExit> exits;
	[Export] Godot.Collections.Array<EnemyOverworld> enemies;
	[Export] Camera2D camera;


	public override void _EnterTree() {
		onPlayerExited += OnPlayerExited;
		onEnemyEncountered += OnEnemyEncountered;
		Interactable.onDialogueStart += PauseGame;
		Interactable.onDialogueEnd += ResumeGame;
	}

	public override void _ExitTree() {
		onPlayerExited -= OnPlayerExited;
		onEnemyEncountered -= OnEnemyEncountered;
		Interactable.onDialogueStart -= PauseGame;
		Interactable.onDialogueEnd -= ResumeGame;
	}
	public override async void _Ready() {
		GetTree().Paused = true;
		player.Animate();
		await ToSignal(player.transition,"finished");
		GetTree().Paused = false;
	}

	private void PauseGame() {
		GetTree().Paused = true;
	}

	private void ResumeGame()
	{
		GetTree().Paused = false;
	}

	public void EnterLevel() {
		if(data != null) {
			if(data.isJustOutOfBattle == false) {
				foreach (OverworldExit exit in exits)
				{
					if (exit.name == data.nextDoorName)
					{
						player.Position = exit.GetPlayerEntryVector();
						player.blendPos = exit.GetMoveDirection();
						break;
					}
				}
			} else {
				player.Position = data.playerPosition;
				player.blendPos = data.playerBlendPos;

				foreach(EnemyOverworld enemy in enemies) {
					enemy.QueueFree();
				}
			}
			
		}
	}

	private void OnPlayerExited(OverworldExit exit) {
		GetTree().Paused = true;
		data = new LevelDataHandoff(exit.nextEntranceName);
		SceneGlobalManager manager = GetTree().Root.GetNode("SceneGlobalManager") as SceneGlobalManager;
		manager.ChangeToNewScene(exit.nextScenePath);
	}

	private void OnEnemyEncountered(SpawnFormation spawnFormation) {
		player.SetPhysicsProcess(false);
		SceneGlobalManager manager = GetTree().Root.GetNode("SceneGlobalManager") as SceneGlobalManager;
		manager.InitBattle(spawnFormation);
	}
}
