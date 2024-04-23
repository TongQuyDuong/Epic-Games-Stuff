using Godot;
using System;

public partial class ItemDescription : HBoxContainer
{
	[Export] TextureRect itemDisplay;
	[Export] Label itemNameDisplay;
	[Export] Label itemDescriptionDisplay;

	public override void _Ready() {
		this.Visible = false;
	}

	public void UpdateItem(Item item) {
		itemDisplay.Texture = item.icon;
		itemNameDisplay.Text = item.itemName;
		itemDescriptionDisplay.Text = item.itemDescription;
	}
}
