using Godot;
using System;

public partial class PlayerAnimation : Node
{
	[Export] private AnimationPlayer animPlayer;
	
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = this.GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
		Events.OnIdle += AnimateIdle;
		Events.OnAttackRanged += AnimateAttackRanged;
		Events.OnMove += AnimateMovement;
	}

	public override void _ExitTree()
	{
		Events.OnIdle -= AnimateIdle;
		Events.OnAttackRanged -= AnimateAttackRanged;
		Events.OnMove -= AnimateMovement;
	}

	public void AnimateMovement() {
		animPlayer.Play("Vanish");
		animPlayer.Queue("Appear");
	}

	public void AnimateAttackRanged(){
		animPlayer.Play("Attack");
	}

	public void AnimateIdle() {
		animPlayer.Play("Idle");
	}
}
