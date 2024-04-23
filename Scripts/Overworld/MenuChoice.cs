using Godot;
using System;

public partial class MenuChoice : HBoxContainer
{
	[Export] TextureRect selector;
	[Export] Label numberDisplay;

	public void ToggleSelect() {
		selector.Visible = !selector.Visible;
	}

	public void ToggleNumber() {
		numberDisplay.Visible = !numberDisplay.Visible;
	}

	public void SetNumberDisplay(int number) {
		numberDisplay.Text = "x" + number;
	}
}
