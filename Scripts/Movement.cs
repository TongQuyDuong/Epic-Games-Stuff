using Godot;
using System;
using MEC;
using System.Collections.Generic;


public partial class Movement : Node2D
{
	[Export] private BaseUnit currentUnit;
	[Export] private float cooldown;
	protected float countdown;
	[Export] private Vector2 axis;
	[Export] private PlayerStateController stateCon;
	private AnimationPlayer anim;
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		countdown = cooldown;
		stateCon = currentUnit.GetNode("PlayerStateController") as PlayerStateController;
		anim = currentUnit.GetNode("AnimationPlayer") as AnimationPlayer;
	}

    public override void _Ready()
    {
        base._Ready();
		Events.OnBattleActive += EnableMovement;
		Events.OnBattleEnd += DisableMovement;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		Events.OnBattleActive -= EnableMovement;
		Events.OnBattleEnd -= DisableMovement;
	}
    private void EnableMovement() {
		this.ProcessMode = ProcessModeEnum.Inherit;
	}
	private void DisableMovement()
	{
		this.ProcessMode = ProcessModeEnum.Disabled;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (stateCon.CurrentPlayerState != PlayerState.Attacking) ProcessInput();

	}
	protected void Flip()
	{
		UnitManager.Instance.heroIsFacingRight = !UnitManager.Instance.heroIsFacingRight;

		currentUnit.Scale = new Vector2(currentUnit.Scale.X * -1, 1);
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
		nextPanel.SetUnit(currentUnit);
		Events.OnRowChange?.Invoke(currentUnit);
	}
}
