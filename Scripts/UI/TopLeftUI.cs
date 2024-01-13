using Godot;
using System;

public partial class TopLeftUI : Control
{
	[Export] public int maxHP;
	[Export] private TextureRect Portrait;
	[Export] public HpBar hpBar;
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		base._EnterTree();
		Portrait = GetNode<TextureRect>("PortraitBack/CharaPortrait");
		hpBar = GetNode<Control>("HpBar") as HpBar;
	}






}
