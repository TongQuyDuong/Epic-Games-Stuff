using Godot;
using System;

public partial class EnemyOverworld : CharacterBody2D
{
	const int ACCELERATION = 1000;
	const int MAXSPEED = 220;
	private bool isChasing;
	private PlayerOverworld player;
	[Export] AnimationPlayer animPlayer;
	[Export] Sprite2D sprite;
	[Export] public SpawnFormation spawnFormation;

	public override void _Ready() {
	}
    public override void _PhysicsProcess(double delta)
    {
        if(isChasing) {
			Vector2 moveVelo = (player.Position - this.Position).Normalized();
			Velocity += moveVelo * (float)delta * ACCELERATION;
			Velocity = Velocity.LimitLength(MAXSPEED);
			MoveAndSlide();

			if(moveVelo.X > 0 && sprite.FlipH == true) {
				sprite.FlipH = false;
			} else if (moveVelo.X < 0 && sprite.FlipH == false) {
				sprite.FlipH = true;
			}
		}
    }

    private void _on_body_detected(Node2D body)
	{
		if(body is PlayerOverworld) {
			SetPhysicsProcess(true);
			player = (PlayerOverworld)body;
			isChasing = true;
			animPlayer.Play("Run");
		}
	}

	private void _on_body_encountered(Node2D body)
	{
		if (body is PlayerOverworld)
		{
			animPlayer.Play("Attack");
			SetPhysicsProcess(false);
			OverworldLevel.onEnemyEncountered?.Invoke(spawnFormation);
		}
	}

	private void _on_body_lost(Node2D body)
	{
		if (body is PlayerOverworld)
		{
			SetPhysicsProcess(true);
			player = null;
			isChasing = false;
			animPlayer.Play("Idle");
		}
	}
}
