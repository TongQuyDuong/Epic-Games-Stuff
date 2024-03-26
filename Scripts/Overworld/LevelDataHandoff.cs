using Godot;
using System;

public partial class LevelDataHandoff : Resource 
{
    public String doorName;
    public Vector2 moveDirection;

    public LevelDataHandoff(string doorName, Vector2 moveDirection) {
        this.doorName = doorName;
        this.moveDirection = moveDirection;
    }
}