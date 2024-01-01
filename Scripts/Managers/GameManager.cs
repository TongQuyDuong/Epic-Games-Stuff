using Godot;
using System;

public partial class GameManager : Node
{
	
	public static GameManager Instance;
	public GameState currentState;


	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
	}
    public override void _Ready()
    {
        base._Ready();
		UpdateGameState(GameState.GenerateGrid);
		Events.OnBattleActive += BeginBattle;
		Events.OnBattleEnd += EndBattle;
	}

    public override void _ExitTree()
    {
        base._ExitTree();
		Events.OnBattleActive -= BeginBattle;
		Events.OnBattleEnd -= EndBattle;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		if(Input.IsActionJustPressed("EndBattle")) BattleUI.Instance.AnnounceBattle(false);
    }

    public void UpdateGameState(GameState newState)
	{
		currentState = newState;
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
			case GameState.BattleStart:
				BattleUI.Instance.AnnounceBattle(true);
				break;
			case GameState.BattleActive:
				break;
			case GameState.BattleEnd:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
		}
	}

	public void BeginBattle() {
		UpdateGameState(GameState.BattleActive);
	}

	public void EndBattle() {
		UpdateGameState(GameState.BattleEnd);
	}

	public void ExitBattle() {
		// this.GetTree().ChangeSceneToPacked(menuScene);
	}
}
public enum GameState
{
	GenerateGrid,
	SpawnHero,
	SpawnEnemies,
	BattleStart,
	BattleActive,
	BattleEnd,
	Victory,
	Defeat
}
