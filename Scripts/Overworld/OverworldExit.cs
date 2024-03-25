using Godot;
using System;

public partial class OverworldExit : Area2D
{
	[Export] public ExitDirection exitDirection;
	[Export] public int pushDistance;
	[Export] public PackedScene nextScene;
	[Export] public String name;

	public override void _EnterTree()
	{
		AddUserSignal("PlayerEntered");
	}

	public Vector2 GetPlayerEntryVector() {
		Vector2 direction = Vector2.Zero;
		switch (exitDirection) {
			case ExitDirection.North:
			direction = Vector2.Down;
			break;
			case ExitDirection.East:
			direction = Vector2.Left;
			break;
			case ExitDirection.South:
			direction = Vector2.Up;
			break;
			case ExitDirection.West:
			direction = Vector2.Right;
			break;
		}
		return (direction * pushDistance) + this.Position;
	}

	public Vector2 GetMoveDirection()
	{
		Vector2 direction = Vector2.Zero;
		switch (exitDirection)
		{
			case ExitDirection.North:
				direction = Vector2.Up;
				break;
			case ExitDirection.East:
				direction = Vector2.Right;
				break;
			case ExitDirection.South:
				direction = Vector2.Down;
				break;
			case ExitDirection.West:
				direction = Vector2.Left;
				break;
		}
		return direction;
	}



}

public enum ExitDirection {
	North,
	East,
	South,
	West
}
