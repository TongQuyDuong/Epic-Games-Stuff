using Godot;
using System;

[GlobalClass]
public partial class StatModGlobal : Resource
{
	[Export] public StatModType modtype;
	[Export] public float modValue;
	[Export] public StatType statType;

	public StatModifier ToStatMod() {
		return new StatModifier(modValue, modtype);
	}
}
