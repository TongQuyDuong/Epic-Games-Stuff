using Godot;
using System;

[GlobalClass]
public partial class SpawnInfo : Resource
{
    [Export] public PackedScene unitToSpawn;
    [Export] public Vector2 spawnLocation;
    [Export] public bool isFacingRight;
    public SpawnInfo()
    {
        unitToSpawn = null;
        spawnLocation = new Vector2(0,0);
        isFacingRight = true;
    }
    public SpawnInfo(PackedScene unit, Vector2 position,bool isFacingRight)
    {
        unitToSpawn = unit;
        spawnLocation = position;
        this.isFacingRight = isFacingRight;
    }
}
