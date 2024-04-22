using Godot;
using System;

public partial class InventoryLayout : MenuLayout
{
	const int UI_LAYER_NUMBER = 1;
	[Export] Godot.Collections.Array<MenuChoice> menuChoices;
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
		MenuBook.onRequestUIFocus.Invoke(UI_LAYER_NUMBER);
		menuChoices[0].ToggleSelect();
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if (UiLayer == UI_LAYER_NUMBER)
		{
			menuChoices[selectedIndex].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = menuChoices.Count - 1;
			}
			else if (selectedIndex >= menuChoices.Count)
			{
				selectedIndex = 0;
			}
			menuChoices[selectedIndex].ToggleSelect();
		}
	}

	private void LoseFocus(int uiLayer) {
		if (uiLayer == UI_LAYER_NUMBER) {
			menuChoices[selectedIndex].ToggleSelect();
		}
	}

	private void GainFocus(int uiLayer) {
		if (uiLayer == 10)
		{
			menuChoices[selectedIndex].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
		}
	}
}
