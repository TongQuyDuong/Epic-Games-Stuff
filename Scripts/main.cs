using Godot;
using System;
using System.Diagnostics;

public partial class main : Node2D
{
	[Export] PackedScene battleScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	private void _on_play_pressed(){
		Debug.Print("Play");
		GetTree().ChangeSceneToPacked(battleScene);
	}
}


