using Godot;
using System;

public partial class BaseUnit : CharacterBody2D
{
	public Panel occupiedPanel;
	[Export] public Vector2 currentPos;
	
	[Export] public UnitStatList stats;
}
