using Godot;
using System;

public partial class UnitManager : Node
{
	[Export] private PackedScene hero;
	public static UnitManager Instance;
	[Export] private SpawnFormation formation;
	[Export] public bool heroIsFacingRight = true;
	
	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Instance = null;
	}
	public void SpawnHero(){
		SpawnUnit(new SpawnInfo(hero,GridManager.Instance.GetHeroSpawnPanel().Pos));
		GameManager.Instance.UpdateGameState(GameState.SpawnEnemies);
	}

	public void SpawnUnit(SpawnInfo info)
	{
		var spawnedUnit = info.unitToSpawn.Instantiate<Node2D>() as BaseUnit;
		Panel spawnPanel = GridManager.Instance.GetPanelAtPosition(info.spawnLocation);
		spawnPanel.SetUnit(spawnedUnit);
		AddChild(spawnedUnit);
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(spawnedUnit, (int)spawnedUnit.currentPos.Y);
	}

	public void SpawnEnemies()
	{
		foreach (SpawnInfo info in formation.spawns)
		{
			SpawnUnit(info);
		}
		GameManager.Instance.UpdateGameState(GameState.BattleStart);
	}
}
