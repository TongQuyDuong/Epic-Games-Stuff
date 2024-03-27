using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MEC;

public partial class UnitManager : Node
{
	[Export] private PackedScene hero;
	public static UnitManager Instance;
	[Export] private SpawnFormation formation;
	[Export] public bool heroIsFacingRight = true;
	[Export] private int EnemiesAlive = 0;
	public Vector2I playerPos;
	public BaseHero chara;
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

	public void UpdateFormation(SpawnFormation formation) {
		this.formation = formation;
	}
	public void SpawnHero()
	{
		chara = SpawnUnit(new SpawnInfo(hero, formation.playerSpawnPos, true)) as BaseHero;
		playerPos = chara.currentPos;
		Timing.RunCoroutine(waitForSecondsAndSpawnEnemies(1f));
	}

	public BaseUnit SpawnUnit(SpawnInfo info)
	{
		var spawnedUnit = info.unitToSpawn.Instantiate<Node2D>() as BaseUnit;
		spawnedUnit.isFacingRight = info.isFacingRight;
		Panel spawnPanel = GridManager.Instance.GetPanelAtPosition(info.spawnLocation);
		spawnPanel.SetUnit(spawnedUnit);
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(spawnedUnit, (int)spawnedUnit.currentPos.Y);
		AddChild(spawnedUnit);
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
			chara.isControlled = true;
			Timing.RunCoroutine(waitForSecondsAndEndBattle(1f));
		}
	}

	IEnumerator<double> waitForSecondsAndSpawnEnemies(float delay)
	{
		yield return Timing.WaitForSeconds(delay);
		GameManager.Instance.UpdateGameState(GameState.SpawnEnemies);
	}

	IEnumerator<double> waitForSecondsAndEndBattle(float delay)
	{
		yield return Timing.WaitForSeconds(delay);
		Events.OnBattleEnd?.Invoke();
	}
}
