using Godot;
using System;

public partial class Bookmark : Control
{
	[Export] private AnimationPlayer animationPlayer;
	[Export] private CompressedTexture2D icon;

	public override void _Ready() {
		if(icon != null) {
			Sprite2D iconSprite = GetNode<Sprite2D>("Icon");
			iconSprite.Texture = icon;
		}
	}

	public void PopOut() {
		animationPlayer.Play("PopOut");
	}

	public void Select() {
		animationPlayer.Queue("Select");
	}

	public void Deselect() {
		animationPlayer.Queue("RESET");
	}
	public void PopIn()
	{
		animationPlayer.PlayBackwards("PopIn");
	}
}
