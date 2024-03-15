using Godot;
using System;

public partial class SlashBehaviour : Node2D
{
	[Export] public BaseUnit caster;
	[Export] public Godot.Collections.Array<StatusEffectData> effectsToApply = new Godot.Collections.Array<StatusEffectData>();
	public Vector2I slashPos;
	public StatType damageType;
	public float Damage;

	public override void _Ready()
	{
		caster.stats.TryGetStatValue(damageType, out Damage);
	}
}
