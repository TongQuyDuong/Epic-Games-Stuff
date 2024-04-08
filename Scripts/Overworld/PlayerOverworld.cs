using Godot;
using System;
using DialogueManagerRuntime;

public partial class PlayerOverworld : CharacterBody2D
{
	const int ACCELERATION = 10000;
	const int FRICTION = 500;
	const int MAXSPEED = 240;
	const int MAXSPEEDRUNNING = 400;

	[Export] private PlayerStateController StateController;
	[Export] private AnimationTree animTree;
	[Export] public Transition transition;
	[Export] private Area2D interactFinder;
	Vector2 inputVector;


	private AnimationNodeStateMachinePlayback stateMachine;
	public Vector2 blendPos;
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
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (OverworldLevel.isActive == false) return;

        if(Input.IsActionJustPressed("Select")) {
			Godot.Collections.Array<Area2D> interactables = interactFinder.GetOverlappingAreas();
			if(interactables.Count > 0) {
				((Interactable)interactables[0]).Action();
				inputVector = Vector2.Zero;
			}
		}

		if(Input.IsActionJustPressed("Menu")) {
			OpenMenu();
		}

		inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
	}
    public override void _PhysicsProcess(double delta)
	{
		if (OverworldLevel.isActive == false) return;
		
		Move(delta);
		Animate();
	}

	private void Move(double delta)
	{
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
		Velocity = Velocity.LimitLength(Input.IsActionPressed("Back")? MAXSPEEDRUNNING : MAXSPEED);
	}

	public void Animate() {
		stateMachine.Travel(animTreeStateKeys[(int)StateController.CurrentPlayerState]);
		animTree.Set(blendPosPaths[(int)StateController.CurrentPlayerState],blendPos);
	}

	private void OpenMenu() {
		OverworldLevel.onMenuOpened?.Invoke();
	}
}
