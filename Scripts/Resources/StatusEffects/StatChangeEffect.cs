using Godot;
using System;
using MonoCustomResourceRegistry;
using System.Diagnostics;

[RegisteredType(nameof(StatChangeEffect), "", nameof(Resource))]

public partial class StatChangeEffect : StatusEffectData
{
	[Export] public float modValue;
	[Export] public StatModType modType;
	[Export] public StatType statType;
	private StatModifier mod;

    public override void TriggerEffect(BaseUnit target)
    {
		
		mod = new StatModifier(modValue,modType);
		target.stats.TryAddStatMod(statType,mod);
    }

	public override void RemoveEffect(BaseUnit target)
	{
		target.stats.TryRemoveStatMod(statType, mod);
	}
}
