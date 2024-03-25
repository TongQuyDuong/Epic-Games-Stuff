using Godot;
using System;

public partial class TestHOverworld : CharacterBody2D
{
	const int ACCELERATION = 10000;
	const int FRICTION = 500;
	const int MAXSPEED = 240;

	[Export] private PlayerStateController StateController;
	[Export] private AnimationTree animTree;
	
	private AnimationNodeStateMachinePlayback stateMachine;
	private Vector2 blendPos;
	private StringName[] blendPosPaths = {
		"parameters/idle/idle_bs2d/blend_position",
		"parameters/move/move_bs2d/blend_position"
	};

	private StringName[] animTreeStateKeys = {
		"idle",
		"move"
	};


	public override void _Ready() {
		stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get(new StringName("parameters/playback"));
		blendPos = Vector2.Zero;
	}
	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
		Animate();
	}

	private void Move(double delta)
	{
		Vector2 inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (inputVector == Vector2.Zero)
		{
			StateController.ChangePlayerState(PlayerState.Idling);
			Velocity = Vector2.Zero;
		}
		else
		{
			StateController.ChangePlayerState(PlayerState.Moving);
			ApplyMovement(inputVector * ACCELERATION * (float)delta);
			blendPos = inputVector;
		}
		MoveAndSlide();
	}

	private void ApplyFriction(float amount)
	{
		if(Velocity.Length() > amount) {
			Velocity -= Velocity.Normalized() * amount;
		} else {
			Velocity = Vector2.Zero;
		}
	}

	private void ApplyMovement(Vector2 amount)
	{
		Velocity += amount;
		Velocity = Velocity.LimitLength(MAXSPEED);
	}

	private void Animate() {
		stateMachine.Travel(animTreeStateKeys[(int)StateController.CurrentPlayerState]);
		animTree.Set(blendPosPaths[(int)StateController.CurrentPlayerState],blendPos);
	}
}
