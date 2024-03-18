using Godot;
using System;

public partial class FireMage : BaseEnemy
{
	bool isMovingUp = true;
	public override void _EnterTree()
	{
		base._EnterTree();
	}
	public override void _Ready()
	{
		base._Ready();
		if(currentPos.Y == 0f) isMovingUp = false;
	}
	public override void _PhysicsProcess(double delta)
	{
		if(isControlled) return;
		switch (stateCon.currentState)
		{
			case EnemyState.Idling:
				stateCon.ChangeState(EnemyState.Pursuing);
				Move(isMovingUp? -1 : 1);
				break;
			case EnemyState.Pursuing:
				if (countdown > 0)
				{
					countdown -= (float)GetProcessDeltaTime();
				}
				break;
			case EnemyState.Attacking:
				break;
			default:
				break;
		}
	}

	protected override void PerformAction()
	{
		if (isFacingRight)
		{
			if (this.currentPos.X - UnitManager.Instance.playerPos.X > 0) Flip();
		}
		else
		{
			if (this.currentPos.X - UnitManager.Instance.playerPos.X < 0) Flip();
		}

		stateCon.ChangeState(EnemyState.Attacking);
		animPlayer.Play("Attack");
	}

	public void UseAbility()
	{
		((SmiteAbility)ability).TriggerAbilityAtPosition(this,UnitManager.Instance.playerPos);
	}

	public void Move(float direction)
	{
		if (isControlled) return;
		direction = direction > 0 ? 1 : -1;
		animPlayer.Play("Walk");

		Panel nextPanel = GridManager.Instance.NextPanelVertical(currentPos, direction);
		if (nextPanel != null && nextPanel.occupiedUnit == null)
		{
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "position", nextPanel.GlobalPosition, 0.5);
			tween.Finished += delegate
			{
				if (nextPanel.occupiedUnit != null)
				{
					this.occupiedPanel.SetUnit(this);
					Move(isMovingUp? -1 : 1);
				}
				else
				{
					Events.OnRowChange?.Invoke(this, (int)direction);
					nextPanel.SetUnit(this);
					if(countdown <= 0f) {
						PerformAction();
						return;
					}
					if(currentPos.Y != 1f) {
						isMovingUp = !isMovingUp;
					}
					Move(isMovingUp ? -1 : 1);

				}

			};
		}
		else
		{
			isMovingUp = !isMovingUp;
			stateCon.ChangeState(EnemyState.Idling);
		}
	}
}
