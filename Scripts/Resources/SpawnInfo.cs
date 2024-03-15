using Godot;
using System;

[GlobalClass]
public partial class SpawnInfo : Resource
{
    [Export] public PackedScene unitToSpawn;
    [Export] public Vector2I spawnLocation;
    [Export] public bool isFacingRight;
    public SpawnInfo()
    {
        unitToSpawn = null;
        spawnLocation = new Vector2I(0,0);
        isFacingRight = true;
    }
    public SpawnInfo(PackedScene unit, Vector2I position,bool isFacingRight)
    {
        unitToSpawn = unit;
        spawnLocation = position;
        this.isFacingRight = isFacingRight;
    }
}
