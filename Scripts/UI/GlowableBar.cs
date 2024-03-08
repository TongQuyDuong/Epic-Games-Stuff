using Godot;
using System;

public partial class GlowableBar : TextureProgressBar
{
	[Export] public AnimationPlayer animPlayer;

	public void StartGlow() {
		animPlayer.Play("Glowing");
	}

	public void StopGlow() {
		animPlayer.Play("RESET");
	}
}
