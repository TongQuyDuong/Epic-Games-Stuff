using Godot;
using System;

public partial class LevelDataHandoff : Resource 
{
    public String nextDoorName;
    public Vector2 playerPosition;
    public Vector2 playerBlendPos;
    public bool isJustOutOfBattle = false;

    public LevelDataHandoff(string doorName) {
        this.nextDoorName = doorName;
    }

    public LevelDataHandoff(Vector2 playerPos, Vector2 blendPos)
    {
        playerPosition = playerPos;
        playerBlendPos = blendPos;
        isJustOutOfBattle = true;
    }
}