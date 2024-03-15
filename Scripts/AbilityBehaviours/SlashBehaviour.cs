using Godot;
using System;

public partial class SlashBehaviour : Node2D
{
	[Export] public BaseUnit caster;
	[Export] public Godot.Collections.Array<StatusEffectData> effectsToApply = new Godot.Collections.Array<StatusEffectData>();
	public float Damage;

	public override void _Ready()
	{
		caster.stats.TryGetStatValue(StatType.Magic, out Damage);
		if (!caster.isFacingRight) this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is BaseUnit)
		{
			BaseUnit target = (BaseUnit)body;
			if (target == caster || Math.Abs(target.currentPos.Y - caster.currentPos.Y) > 1) return;
			IDamageable targetHealth = target.GetNode<IDamageable>("UnitHealth");
			targetHealth.TakeDamage(Damage);

			target.STeffectCon.AddStatusEffect(effectsToApply, caster);
		}
	}
}
