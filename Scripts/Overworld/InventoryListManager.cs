using Godot;
using System;

public partial class InventoryListManager : VBoxContainer
{
	const int UI_LAYER_NUMBER = 2;
	[Export] Godot.Collections.Array<MenuChoice> itemChoices;
	[Export] public Godot.Collections.Array<Item> itemList;
	private int selectedIndex = 0;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocus;
		MenuBook.onBackInputReceived += LoseFocus;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocus;
		MenuBook.onBackInputReceived -= LoseFocus;
	}

	public override void _Ready() {
		foreach (Node item in GetChildren()) {
			itemChoices.Add((MenuChoice)item);
		}

	}

	private void DisplayItems(Godot.Collections.Array<Item> items) {
		
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if (UiLayer == UI_LAYER_NUMBER)
		{
			itemChoices[selectedIndex].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = itemChoices.Count - 1;
			}
			else if (selectedIndex >= itemChoices.Count)
			{
				selectedIndex = 0;
			}
			itemChoices[selectedIndex].ToggleSelect();
		}
	}

	private void LoseFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			itemChoices[selectedIndex].ToggleSelect();
		}
	}

	private void GainFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			itemChoices[selectedIndex].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
		}
	}
}
