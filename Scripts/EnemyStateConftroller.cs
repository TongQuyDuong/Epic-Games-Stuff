using Godot;
using System;

public partial class EnemyStateConftroller : Node2D
{
	public EnemyState currentState;


	public void ChangeState(EnemyState newState)
	{
		if (currentState == newState) return;
		currentState = newState;
	}
}

public enum EnemyState {
	Idling,
	Attacking,
	Pursuing,
	Controlled
}
