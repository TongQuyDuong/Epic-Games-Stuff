using Godot;
using System;
using System.Diagnostics;


public partial class CodeExecutor : Node2D
{
	[Export] PlayerData playerData;
	[Export] Item itemToAdd;

	public override void _Process(double delta) {
	}
}