using Godot;
using System;

public partial class SelectSkillButton : Node2D
{
	[Export] public AnimationPlayer animPlayer;
	[Export] public Sprite2D icon;
	[Export] public Sprite2D selector;
	[Export] public bool isSelected;
	[Export] public Vector2 buttonPos;
	[Export] public bool isActive;

	public virtual void ToggleSelectOn()
	{
		isSelected = true;
		selector.Visible = true;
		animPlayer.Play("Selected");
	}

	public virtual void ToggleSelectOff()
	{
		isSelected = false;
		animPlayer.Play("RESET");
	}

	public virtual void SelectButton() {

	}

}
