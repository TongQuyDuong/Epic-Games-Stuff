using Godot;
using System;
using MonoCustomResourceRegistry;
using System.Diagnostics;

[RegisteredType(nameof(ProjectileAbility), "", nameof(Ability))]
public partial class ProjectileAbility : RangedAbility
{
	[Export] PackedScene projectile;
	public override void Initialize(BaseUnit owner)
	{
		
		caster = owner;
		firingPoint = owner.GetNode<Node2D>("FiringPoint");
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;

	}
	public override void TriggerAbility()
	{
		var proj = (ProjectileBehaviour)projectile.Instantiate();
		proj.rowNumber = (int)caster.currentPos.Y;
		caster.stats.TryGetStatValue(StatType.Magic, out float power);
		proj.Damage = power;
		proj.Position = caster.GetNode<Node2D>("FiringPoint").GlobalPosition;
		caster.GetParent().AddChild(proj);
		
	}
}
