using Godot;
using System;

public partial class OverworldExit : Area2D
{
	[Export] public ExitDirection exitDirection;
	[Export] public int pushDistance;
	[Export] public StringName nextScenePath;
	[Export] public String name;
	[Export] public String nextEntranceName;

	private void _on_body_entered(Node2D body) {
		if(body is TestHOverworld) {
			OverworldLevel.onPlayerExited?.Invoke(this);
			QueueFree();
		}
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
		return direction;
	}



}

public enum ExitDirection {
	North,
	East,
	South,
	West
}
