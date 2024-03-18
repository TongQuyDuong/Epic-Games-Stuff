using Godot;
using System;

public partial class CircularSlashBehaviour : SlashBehaviour
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
			if (target == caster || Math.Abs(target.currentPos.Y - caster.currentPos.Y) > 1) return;
			target.STeffectCon.AddStatusEffect(effectsToApply, caster);
			
			IDamageable targetHealth = target.GetNode<IDamageable>("UnitHealth");
			targetHealth.TakeDamage(Damage);

			
		}
	}
}
