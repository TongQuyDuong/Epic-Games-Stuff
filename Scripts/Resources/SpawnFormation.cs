using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(SpawnFormation), "", nameof(Resource))]
public partial class SpawnFormation : Resource
{
	[Export] public Godot.Collections.Array<SpawnInfo> spawns = new Godot.Collections.Array<SpawnInfo>();
}

