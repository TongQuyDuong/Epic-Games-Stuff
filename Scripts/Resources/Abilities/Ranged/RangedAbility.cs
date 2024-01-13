using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(RangedAbility), "", nameof(Resource))]
public abstract partial class RangedAbility : Ability
{
	[Export] protected Marker2D firingPoint;
	
}
