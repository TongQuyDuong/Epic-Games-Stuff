using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(DamageOverTime), "", nameof(Resource))]

public partial class DamageOverTime : StatusEffectData
{
	[Export] public float interval;
	[Export] public float DamageMultiplier;
	[Export] public StatType DamageType;

}
