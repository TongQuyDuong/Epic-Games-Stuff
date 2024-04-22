using Godot;
using System;

public partial class MenuChoice : HBoxContainer
{
	[Export] TextureRect selector;

	public void ToggleSelect() {
		selector.Visible = !selector.Visible;
	}
}
