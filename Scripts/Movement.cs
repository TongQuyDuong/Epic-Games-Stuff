using Godot;
using System;
using MEC;
using System.Collections.Generic;


public partial class Movement : Node2D
{
	[Export] private BaseHero currentUnit;
	[Export] private float cooldown;
	protected float countdown;
	[Export] private Vector2 axis;
	
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		countdown = cooldown;
	}

    public override void _Ready()
    {
        base._Ready();

    }




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (currentUnit.stateController.CurrentPlayerState != PlayerState.Attacking) ProcessInput();

	}
	protected void Flip()
	{
		currentUnit.isFacingRight = !currentUnit.isFacingRight;

		currentUnit.Scale = new Vector2(currentUnit.Scale.X * -1, currentUnit.Scale.Y);
	}

	void ProcessInput()
	{
		if (countdown > 0f) { countdown -= (float)GetPhysicsProcessDeltaTime(); }
		if (countdown <= 0f) {
			if(Input.IsActionJustPressed("MoveUp")) {
				axis.Y = 1;
				}
			 else if (Input.IsActionJustPressed("MoveDown"))
			{
				axis.Y = -1;
			} else if (Input.IsActionJustPressed("MoveLeft"))

			{
				axis.X = -1;
			} else if (Input.IsActionJustPressed("MoveRight"))
			{
				axis.X = 1;
			}
			
		PerformMovement(ref axis);

		
		if (Input.IsActionJustPressed("Flip"))
			{
				Flip();
				
			}
		}
			
	}

	void PerformMovement(ref Vector2 axis){
		if(axis.Y != 0) {
			var nextPanel = GridManager.Instance.NextPanelVertical(currentUnit.currentPos, axis.Y*-1);
			if (nextPanel != null)
			{
				MoveTo(nextPanel);
			}
		}
			 else if (axis.X != 0)
		{
			var nextPanel = GridManager.Instance.NextPanelHorizontal(currentUnit.currentPos, axis.X);
			if (nextPanel != null)
			{
				MoveTo(nextPanel);
			}
		}
		if (axis != new Vector2(0, 0)) countdown = cooldown;
		axis = new Vector2(0, 0);
	}
	void MoveTo(Panel nextPanel)
	{
		if (nextPanel.Walkable == true)
		{
			Timing.RunCoroutine(waitForSecondsAndMove(0.075f, nextPanel));
		}
		
	}

	IEnumerator<double> waitForSecondsAndMove(float delay, Panel nextPanel)
	{
		Events.OnMove?.Invoke();
		yield return Timing.WaitForSeconds(delay);
		int yDifference = (int)(nextPanel.Pos.Y - currentUnit.currentPos.Y);
		nextPanel.SetUnit(currentUnit);
		UnitManager.Instance.playerPos = currentUnit.currentPos;
		if(yDifference != 0) Events.OnRowChange?.Invoke(currentUnit,yDifference);
	}
}
