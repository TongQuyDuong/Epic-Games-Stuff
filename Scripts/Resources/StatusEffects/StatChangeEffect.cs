using Godot;
using System;
using MonoCustomResourceRegistry;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[RegisteredType(nameof(StatChangeEffect), "", nameof(Resource))]

public partial class StatChangeEffect : StatusEffectData
{
	[Export] public float modValue;
	[Export] public StatModType modType;
	[Export] public StatType statType;
	public StatModifier mod;

    public override void TriggerEffect(BaseUnit target)
    {
		
		mod = new StatModifier(modValue,modType);
		target.stats.TryAddStatMod(statType,mod);

    }

	public void RemoveEffect(BaseUnit target, StatModifier mod)
	{
		string result = target.stats.TryRemoveStatMod(statType, mod).ToString();
		Debug.Print(result);
	}
}
