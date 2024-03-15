using Godot;
using System;
using System.Linq;

public partial class LightBandit : BaseEnemy
{
	private Godot.Collections.Array<Vector2I> currentPath;
	private bool pathBlocked;

	public override void _EnterTree()
	{
		base._EnterTree();
	}

	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
	}

	public void UseAbility()
	{
		ability.TriggerAbility(this);
	}

	protected override void PerformAction()
	{
		if (isControlled) return;
		if (isFacingRight)
		{
			if (this.currentPos.X - UnitManager.Instance.playerPos.X > 0) Flip();
		}
		else
		{
			if (this.currentPos.X - UnitManager.Instance.playerPos.X < 0) Flip();
		}

		Vector2I posDifference = UnitManager.Instance.playerPos - this.currentPos;

		if(Math.Abs(posDifference.X) <= 1 && posDifference.Y == 0) {
			stateCon.ChangeState(EnemyState.Attacking);
			animPlayer.Play("Attack");
		} else {
			Pursue();
		}

	}


	private void Pursue() {
		
		currentPath = GridManager.Instance.aStarGrid2D.GetIdPath(currentPos, UnitManager.Instance.playerPos + new Vector2I(isFacingRight ? -1 : 1, 0));
		if (currentPath.Count <= 1) {
			pathBlocked = true;
			currentPath = GridManager.Instance.aStarGrid2D.GetIdPath(currentPos, UnitManager.Instance.playerPos + new Vector2I((isFacingRight ? -1 : 1) * (pathBlocked ? -1 : 1), 0));
			if (currentPath.Count <= 1) {
				ReturnToIdle();
				return;
			}
		} else {
			pathBlocked = false;
		}

		if (stateCon.currentState != EnemyState.Pursuing) {
			stateCon.currentState = EnemyState.Pursuing;
		}

		animPlayer.Play("Walk");
		Panel nextPanel = GridManager.Instance.GetPanelAtPosition(currentPath.Slice(1).First());
		if (nextPanel != null && nextPanel.Walkable == true)
		{
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "position", nextPanel.GlobalPosition, 0.5);
			tween.Finished += delegate
			{
				if (nextPanel.Walkable == false)
				{
					this.occupiedPanel.SetUnit(this);
					PerformAction();
				}
				else
				{
					nextPanel.SetUnit(this);
					SpriteLayerManager.Instance.AdjustLayerOnInstantiation(this, currentPos.Y);
					PerformAction();
				}

			};
		} else {
			ReturnToIdle();
		}
	}
}
