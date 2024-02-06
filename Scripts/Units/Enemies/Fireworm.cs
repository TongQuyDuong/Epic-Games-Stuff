using Godot;
using System;

public partial class Fireworm : BaseEnemy
{
	[Export] public Ability ability;
	[Export] public float waitTime;
	private float countdown;

	public override void _Ready()
	{
		countdown = waitTime;
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		switch (stateCon.currentState) {
			case EnemyState.Idling:
			if(countdown > 0) {
				countdown -= (float)GetProcessDeltaTime(); 
			} else {
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

	public void PerformAction() {
		if(isFacingRight) {
			if(this.currentPos.X - UnitManager.Instance.playerPos.X > 0) Flip();
		} else {
			if (this.currentPos.X - UnitManager.Instance.playerPos.X < 0) Flip();
		}

		float yDifference = UnitManager.Instance.playerPos.Y - this.currentPos.Y;

		if (yDifference == 0) {
			stateCon.ChangeState(EnemyState.Attacking);
			animPlayer.Play("Attack");
 
		} else {
			stateCon.ChangeState(EnemyState.Pursuing);
			Move(yDifference);
		}
	}

	public void UseAbility() {
		ability.TriggerAbility(this);
	}

	public void ReturnToIdle() {
		animPlayer.Play("Idle");
		countdown = waitTime;
		stateCon.ChangeState(EnemyState.Idling);
		
	}

	public void Move(float direction) {
		direction = direction > 0? 1 : -1;
		animPlayer.Play("Walk");

		Panel nextPanel = GridManager.Instance.NextPanelVertical(currentPos,direction);
		if(nextPanel != null && nextPanel.occupiedUnit == null) {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(this,"position",nextPanel.GlobalPosition, 0.5);
			tween.Finished += delegate {
                if(nextPanel.occupiedUnit != null) {
                    this.occupiedPanel.SetUnit(this);
                    PerformAction();
                } else {
					Events.OnRowChange?.Invoke(this,(int)direction);
                    nextPanel.SetUnit(this);
                    PerformAction();
                }

			};
		} else {
            stateCon.ChangeState(EnemyState.Attacking);
            animPlayer.Play("Attack");
        }
	}

	protected void Flip()
	{
		this.isFacingRight = !this.isFacingRight;

		this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
	}
}
