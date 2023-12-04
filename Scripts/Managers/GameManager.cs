using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;

	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
	}
	public GameState State;

	void _on_play_pressed(){
		UpdateGameState(GameState.GenerateGrid);
	}
	public void UpdateGameState(GameState newState)
	{
		State = newState;
		switch (newState)
		{
			case GameState.GenerateGrid:
				GridManager.Instance.GenerateGrid();
				break;
			case GameState.SpawnHero:
				UnitManager.Instance.SpawnHero();
				break;
			case GameState.SpawnEnemies:
				UnitManager.Instance.SpawnEnemies();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
		}
	}
}
public enum GameState
{
	GenerateGrid = 0,
	SpawnHero = 1,
	SpawnEnemies = 2,
	Victory = 3,
	Defeat = 4
}
