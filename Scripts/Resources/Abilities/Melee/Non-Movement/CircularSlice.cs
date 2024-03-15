using Godot;
using System;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(CircularSlice), "", nameof(Resource))]
public partial class CircularSlice : MeleeAbility
{
	[Export] PackedScene slashEffect;
	public override void Initialize()
	{
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;
	}

	public override void TriggerAbility(BaseUnit caster)
	{
		var slash = slashEffect.Instantiate<SlashBehaviour>();
		slash.caster = caster;
		slash.effectsToApply = effectsToApply;
		slash.Position = caster.CenterPoint. Position;
		caster.AddChild(slash);
	}
}
