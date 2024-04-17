using Godot;
using System;

public partial class ProfileLayout : MenuLayout
{
	public CompressedTexture2D portrait;
	public int currentHealth;
	public UnitStatList statList;

	public override void _Ready() {
		portrait = player.portrait;
		currentHealth = player.currentHealth;
		statList = player.playerStats;
	}
}
