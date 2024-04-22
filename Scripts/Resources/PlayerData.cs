using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(PlayerData), "", nameof(Resource))]
public partial class PlayerData : Resource
{
	[Export] public int currentHP;
	[Export] public UnitStatList playerStats;
}
