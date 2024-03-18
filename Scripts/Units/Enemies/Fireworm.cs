using Godot;
using System;

public partial class Fireworm : BaseEnemy
{


    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _Ready()
	{
		base._Ready();
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	protected override void PerformAction() {
		if(isControlled) return;
		if(isFacingRight) {
			if(this.currentPos.X - UnitManager.Instance.playerPos.X > 0) Flip();
		} else {
			if (this.currentPos.X - UnitManager.Instance.playerPos.X < 0) Flip();
		}

		float yDifference = UnitManager.Instance.playerPos.Y - this.currentPos.Y;

		if (yDifference == 0) {
			if(CheckForAlliesInTheWay()) {
				ReturnToIdle();
				return;
			}
			
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

	public void Move(float direction) {
		direction = direction > 0? 1 : -1;
		animPlayer.Play("Walk");

		Panel nextPanel = GridManager.Instance.NextPanelVertical(currentPos,direction);
		if(nextPanel != null && nextPanel.occupiedUnit == null) {
			runningAnimTween = GetTree().CreateTween();
			runningAnimTween.TweenProperty(this, "position", nextPanel.GlobalPosition, 0.5);
			runningAnimTween.Finished += delegate {
				if (stateCon.currentState == EnemyState.Damaged)
				{
					this.occupiedPanel.SetUnit(this);
				}
				else if (nextPanel.occupiedUnit != null) {
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

	private bool CheckForAlliesInTheWay() {
		int direction = isFacingRight ? 1 : -1;
		for (int i = currentPos.X + direction; i >= 0 && i < 8; i += direction)
		{
			Panel panel = GridManager.Instance.GetPanelAtPosition(new Vector2I(i, currentPos.Y));
			if (panel.occupiedUnit != null && panel.occupiedUnit is BaseEnemy)
			{
				return true;
			} else if(panel.occupiedUnit != null && panel.occupiedUnit is BaseHero) {
				return false;
			}
		}
		return false;
	}

}
