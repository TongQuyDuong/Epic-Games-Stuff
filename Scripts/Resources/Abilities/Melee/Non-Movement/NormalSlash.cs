using Godot;
using System;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(NormalSlash), "", nameof(Resource))]
public partial class NormalSlash : MeleeAbility
{
	public override void Initialize()
	{
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;
	}

	public override void TriggerAbility(BaseUnit caster)
	{
		var slash = slashEffect.Instantiate<NormalSlashBehaviour>();
		slash.slashPos = caster.currentPos + new Vector2I(caster.isFacingRight? 1 : -1,0);
		slash.damageType = scaleStat;
		slash.caster = caster;
		slash.effectsToApply = effectsToApply;
		slash.Position = caster.CenterPoint.Position;
		caster.AddChild(slash);
	}
}
