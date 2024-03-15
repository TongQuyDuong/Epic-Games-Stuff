using Godot;
using System;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(ProjectileAbility), "", nameof(Resource))]
public partial class ProjectileAbility : RangedAbility
{
	[Export] PackedScene projectile;
	public override void Initialize() {
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;
	}
	public override void TriggerAbility(BaseUnit caster)
	{
		Marker2D firingPoint = caster.GetNode<Marker2D>("FiringPoint");

		var proj = (ProjectileBehaviour)projectile.Instantiate();
		proj.damageType = scaleStat;
		proj.caster = caster;
		proj.effectsToApply = effectsToApply;
		proj.GlobalPosition = firingPoint.GlobalPosition;
		caster.GetParent().AddChild(proj);
	}
}
