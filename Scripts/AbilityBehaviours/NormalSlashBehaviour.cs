using Godot;
using System;

public partial class NormalSlashBehaviour : SlashBehaviour
{
	public override void _Ready()
	{
		base._Ready();
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is BaseUnit)
		{
			BaseUnit target = (BaseUnit)body;
			if (target == caster || target.currentPos != slashPos) return;
			IDamageable targetHealth = target.GetNode<IDamageable>("UnitHealth");
			targetHealth.TakeDamage(Damage);

			target.STeffectCon.AddStatusEffect(effectsToApply, caster);
		}
	}
}
