using Godot;
using System;

public partial class PlayerStateController : Node2D
{
	[Export] private PlayerState currentState = PlayerState.Idling;


	public PlayerState CurrentPlayerState
	{
		get { return currentState; }
	}

	public override void _EnterTree()
	{
		Events.OnMove += ToMove;
		Events.OnAttackRanged += ToAttack;
	}

	public override void _ExitTree()
	{
		Events.OnMove -= ToMove;
		Events.OnAttackRanged -= ToAttack;
	}

	private void ToAttack()
	{
		ChangePlayerState(PlayerState.Attacking);
	}
	private void ToMove()
	{
		ChangePlayerState(PlayerState.Moving);
	}

	public void ReturnToIdle(){
		ChangePlayerState(PlayerState.Idling);
		Events.OnIdle?.Invoke();
	}
	public void ChangePlayerState(PlayerState newState)
	{
		if (currentState == newState) return;
		currentState = newState;
	}
}

public enum PlayerState
{
	Idling,
	Moving,
	Attacking,
	Flinched
}