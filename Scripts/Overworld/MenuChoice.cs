using Godot;
using System;

public partial class MenuChoice : HBoxContainer
{
	[Export] TextureRect selector;
	[Export] Label textDisplay;
	[Export] Label numberDisplay;

	public void ToggleSelect() {
		selector.Visible = !selector.Visible;
	}

	public void HideNumber() {
		numberDisplay.Visible = false;
	}

	public void SetNumberDisplay(int number) {
		numberDisplay.Visible = true;
		numberDisplay.Text = "x" + number;
	}

	public void SetContent(string content) {
		this.Visible = true;
		textDisplay.Text = content;
	}

	public void SetContent(Item item)
	{
		if(item != null) {
			textDisplay.Text = item.itemName;
			if (item.isStackable) {
				SetNumberDisplay(item.quantity);
			} else {
				HideNumber();
			}

		}

	}

	public string GetText() {
		return textDisplay.Text;
	}
}
