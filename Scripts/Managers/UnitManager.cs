using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class UnitManager : Node
{
	[Export] private PackedScene hero;
	public static UnitManager Instance;
	[Export] private SpawnFormation formation;
	[Export] public bool heroIsFacingRight = true;
	[Export] private int EnemiesAlive = 0;
	public Vector2 playerPos;
	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
		Events.OnEnemyDeath += RemoveEnemiesAlive;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Instance = null;
		Events.OnEnemyDeath -= RemoveEnemiesAlive;
	}
	public void SpawnHero()
	{
		BaseHero chara = SpawnUnit(new SpawnInfo(hero, GridManager.Instance.GetHeroSpawnPanel().Pos, true)) as BaseHero;
		chara.playerAnim.AnimateEntrance();
		playerPos = chara.currentPos;
		GameManager.Instance.UpdateGameState(GameState.SpawnEnemies);
	}

	public BaseUnit SpawnUnit(SpawnInfo info)
	{
		var spawnedUnit = info.unitToSpawn.Instantiate<Node2D>() as BaseUnit;
		spawnedUnit.isFacingRight = info.isFacingRight;
		Panel spawnPanel = GridManager.Instance.GetPanelAtPosition(info.spawnLocation);
		spawnPanel.SetUnit(spawnedUnit);
		AddChild(spawnedUnit);
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(spawnedUnit, (int)spawnedUnit.currentPos.Y);
		return spawnedUnit;
	}

	public void SpawnEnemies()
	{
		for (int i = 0; i < formation.spawns.Count; i++)
		{
			BaseEnemy unit = SpawnUnit(formation.spawns[i]) as BaseEnemy;
			EnemiesAlive += 1;
			unit.Position += new Vector2(0, -600);
			Tween tweenUnit = GetTree().CreateTween();
			tweenUnit.SetEase(Tween.EaseType.In);
			tweenUnit.TweenProperty(unit, "position", unit.Position + new Vector2(0, 600), 0.6).SetDelay(i * 0.3);
			if (i == (formation.spawns.Count - 1)) tweenUnit.Finished += delegate { GameManager.Instance.UpdateGameState(GameState.BattleStart); };
		}

	}

	private void RemoveEnemiesAlive()
	{
		EnemiesAlive -= 1;
		if (EnemiesAlive == 0)
		{
			Events.OnBattleEnd?.Invoke();
		}
	}
}
