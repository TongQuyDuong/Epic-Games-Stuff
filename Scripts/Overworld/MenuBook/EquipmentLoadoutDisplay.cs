using Godot;
using System;

public partial class EquipmentLoadoutDisplay : HBoxContainer
{
	[Export] TextureRect itemPicDisplay;
	[Export] Label itemNameDisplay;
	[Export] CompressedTexture2D defaultIcon;

	public void SetItem(Equipment item) {
		if (item != null) {
			itemPicDisplay.Texture = item.icon;
			itemNameDisplay.Text = item.itemName;
		} else {
			itemPicDisplay.Texture = defaultIcon;
			itemNameDisplay.Text = "";
		}
	}
}
