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

	public void SetContent(ItemData itemData)
	{
		if(itemData != null) {
			textDisplay.Text = itemData.item.itemName + (itemData.isEquipped? " (E)" : "");
			if (itemData.item.isStackable) {
				SetNumberDisplay(itemData.quantity);
			} else {
				HideNumber();
			}
		} else {
			this.Visible = false;
		}

	}

	public void SetContent(AbilityData data)
	{
		if (data != null)
		{
			textDisplay.Text = data.ability.Name + (data.isEquipped ? " (E)" : "");
			HideNumber();
		}
		else
		{
			this.Visible = false;
		}

	}

	public string GetText() {
		return textDisplay.Text;
	}

	public void SetSize() {
		this.textDisplay.Size = new Vector2(1180,100);
	}
}
