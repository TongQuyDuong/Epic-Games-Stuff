using Godot;
using System;

public partial class BaseEnemy : BaseUnit
{
	[Export] public EnemyStateConftroller stateCon;
	[Export] public EnemyHPBar hpBar;
	[Export] public float waitTime;
	[Export] public Ability ability;
	protected Tween runningAnimTween;

	protected float countdown;

	public override void _EnterTree()
	{
		base._EnterTree();
		stats.TryGetStatValue(StatType.HP, out float maxHP);
		hpBar.SetMaxHP((int)maxHP);
		if (isFacingRight == false) hpBar.Flip();
	}
	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Ready() {
		base._Ready();
		countdown = waitTime;
	}

	public override void _PhysicsProcess(double delta)
	{
		switch (stateCon.currentState)
		{
			case EnemyState.Idling:
				if (countdown > 0)
				{
					countdown -= (float)GetProcessDeltaTime();
				}
				else
				{
					PerformAction();
				}

				break;
			case EnemyState.Pursuing:
				break;
			case EnemyState.Attacking:
				break;
			default:
				break;
		}
	}

	protected virtual void PerformAction() {
		
	}

	protected void Flip()
	{
		this.isFacingRight = !this.isFacingRight;

		this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
		hpBar.Flip();
	}

	public void ReturnToIdle()
	{
		animPlayer.Play("Idle");
		countdown = waitTime;
		stateCon.ChangeState(EnemyState.Idling);
	}

	public void AbortRunningTweens() {
		if(runningAnimTween != null) {
			runningAnimTween.Kill();
		}
		this.occupiedPanel.SetUnit(this);
	}

	public void FreeCurrentPanel() {
		occupiedPanel.ResetPanel();
	}
}
