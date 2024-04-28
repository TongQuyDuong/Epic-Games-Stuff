using Godot;
using System;

public partial class Events 
{
	public static Action OnMove;
	public static Action OnIdle;
	public static Action OnAttackRanged;
	public static Action<BaseUnit,int> OnRowChange;
	public static Action OnBattleActive;
	public static Action OnBattleEnd;
	public static Action<BaseUnit> OnEnemyDeath;
}
