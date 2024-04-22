using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public PlayerData playerData;
	public static GameManager Instance;
	[Export] public GameState currentState;
	private bool isSelectSkillReady = false;


	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
		Events.OnBattleActive += BeginBattle;
		Events.OnBattleEnd += EndBattle;
		TopLeftUI.onSpOverFlow += EnableSelectSkill;
	}
    public override void _ExitTree()
    {
		Events.OnBattleActive -= BeginBattle;
		Events.OnBattleEnd -= EndBattle;
		TopLeftUI.onSpOverFlow -= EnableSelectSkill;
		Instance = null;
	}

    public override void _Process(double delta)
    {
		if(Input.IsActionJustPressed("EndBattle")) Events.OnBattleEnd?.Invoke();
		if(Input.IsActionJustPressed("SelectSkill") && isSelectSkillReady) UpdateGameState(GameState.BattleStart);
	}

    public void UpdateGameState(GameState newState)
	{
		if (currentState == newState) return;
		currentState = newState;
		switch (newState)
		{
			case GameState.GenerateGrid:
				GridManager.Instance.GenerateGrid();
				break;
			case GameState.GenerateUI:
				BattleUI.Instance.GenerateUI();
				break;
			case GameState.SpawnHero:
				UnitManager.Instance.SpawnHero();
				break;
			case GameState.SpawnEnemies:
				UnitManager.Instance.SpawnEnemies();
				break;
			case GameState.BattleStart:
				BattleUI.Instance.ShowSkillBook();
				PauseBattle();
				break;
			case GameState.BattlePaused:
				break;
			case GameState.BattleActive:
				ResumeBattle();
				break;
			case GameState.BattleEnd:
				playerData.currentHP = BattleUI.Instance.topLeftUI.hpBar.getCurrentHp();
				BattleUI.Instance.AnnounceBattle(false);
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
	public void PauseBattle() {
		UnitManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
		BattleUI.Instance.topLeftUI.ProcessMode = ProcessModeEnum.Disabled;
	}
	public void ResumeBattle()
	{
		UnitManager.Instance.ProcessMode = ProcessModeEnum.Inherit;
		BattleUI.Instance.topLeftUI.ProcessMode = ProcessModeEnum.Inherit;
		BattleUI.Instance.topLeftUI.ResetSoulPower();
		isSelectSkillReady = false;
	}
	
	private void EnableSelectSkill() {
		isSelectSkillReady = true;
	}
}
public enum GameState
{
	GenerateGrid = 1,
	GenerateUI,
	SpawnHero,
	SpawnEnemies,
	BattleStart,
	BattleActive,
	BattlePaused,
	BattleEnd,
	Victory,
	Defeat
}
