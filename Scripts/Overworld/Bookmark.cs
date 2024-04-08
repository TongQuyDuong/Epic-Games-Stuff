using Godot;
using System;
using System.Diagnostics;

public partial class Bookmark : Control
{
	[Export] private AnimationPlayer animationPlayer;
	[Export] private CompressedTexture2D icon;
	[Export] public PackedScene menuLayout;

	public override void _Ready() {
		if(icon != null) {
			Sprite2D iconSprite = GetNode<Sprite2D>("Icon");
			iconSprite.Texture = icon;
		}
	}

	public void PopOut() {
		Debug.Print("Poppedout");
		animationPlayer.Play("PopOut");
	}

	public void Select() {
		animationPlayer.Queue("Selected");
	}

	public void Deselect() {
		animationPlayer.Queue("Normal");
	}
	public void PopIn()
	{
		animationPlayer.PlayBackwards("PopIn");
	}
}
