using Godot;
using System;

public partial class ProjectileLauncher : Node2D
{
	[Export] public ProjectileAbility ability;
	[Export] public BaseUnit caster;
	private float countdown;
	// Called when the node enters the scene tree for the first time.
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		countdown -= (float)GetPhysicsProcessDeltaTime();
		if(countdown <= 0) {
			ability.TriggerAbility(caster);
			countdown = 1;
		}
	}

}
