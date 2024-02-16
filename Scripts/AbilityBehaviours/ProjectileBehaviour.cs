using Godot;
using System;

public partial class ProjectileBehaviour : RigidBody2D
{
	[Export] public BaseUnit caster;
	[Export] private float projectileDuration;
	[Export] private float projectileSpeed;
	[Export] private Node2D impactPoint;
	[Export] public int rowNumber;
	[Export] private AnimationPlayer animationPlayer;
	[Export] public Godot.Collections.Array<StatusEffectData> effectsToApply;
	public float Damage;
	Vector2 velo = new Vector2();


	public override void _Ready()
	{
		rowNumber = (int)caster.currentPos.Y;
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(this, rowNumber);
		caster.stats.TryGetStatValue(StatType.Magic, out Damage);
		if (!caster.isFacingRight) this.Scale = new Vector2(this.Scale.X*-1,this.Scale.Y);
		projectileSpeed *= caster.isFacingRight ? 1 : -1;
	}

	public override void _PhysicsProcess(double delta)
	{	
		velo.X = projectileSpeed*(float)delta;
		Translate(velo);
		projectileDuration -= (float)GetPhysicsProcessDeltaTime();
		if (projectileDuration <= 0f) this.QueueFree();
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		BaseUnit target = (BaseUnit)body;
		if (target.currentPos.Y == rowNumber)
		{
			this.Sleeping = true;
			this.SetPhysicsProcess(false);
			this.GlobalPosition = impactPoint.GlobalPosition;
			animationPlayer.Play("Explode");

			IDamageable targetHealth = target.GetNode<IDamageable>("UnitHealth");
			targetHealth.TakeDamage(Damage);
			
			target.STeffectCon.AddStatusEffect(effectsToApply,caster);
		}
	}

	private void ScreenExited() 
	{
		this.QueueFree();
	}
	

}



