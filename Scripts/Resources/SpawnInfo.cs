using Godot;
using System;

[GlobalClass]
public partial class SpawnInfo : Resource
{
    [Export] public PackedScene unitToSpawn;
    [Export] public Vector2 spawnLocation;
    public SpawnInfo()
    {
        unitToSpawn = null;
        spawnLocation = new Vector2(0,0);
    }
    public SpawnInfo(PackedScene unit, Vector2 position)
    {
        unitToSpawn = unit;
        spawnLocation = position;
    }
}
