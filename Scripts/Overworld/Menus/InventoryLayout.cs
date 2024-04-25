using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class InventoryLayout : MenuLayout
{
	const int UI_LAYER_NUMBER = 1;
	[Export] Godot.Collections.Array<MenuChoice> menuChoices;
	[Export] InventoryListManager displayList;
	[Export] ItemDescription descriptionDisplay;
	private int selectedIndex = 0;

	[Export] PlayerData playerData;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocusFromBookmark;
		MenuBook.onBackInputReceived += LoseFocus;
		displayList.onSelectedItemChanged += DisplayItem;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocusFromBookmark;
		MenuBook.onBackInputReceived -= LoseFocus;
		displayList.onSelectedItemChanged -= DisplayItem;
	}

	public override void _Ready() {
		MenuBook.onRequestUIFocus.Invoke(UI_LAYER_NUMBER);
		menuChoices[0].ToggleSelect();
		displayList.DisplayItems(playerData.inventory);
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
			
			if(selectedIndex == 0) {
				displayList.DisplayItems(playerData.inventory);
			} else {
				displayList.DisplayItems(playerData.inventory.Where(item => (int)item.Key.itemType == selectedIndex));
			}
		}
	}

	private void LoseFocus(int uiLayer) {
		if (uiLayer == UI_LAYER_NUMBER) {
			menuChoices[selectedIndex].ToggleSelect();
			Debug.Print(UI_LAYER_NUMBER + "Lose Focus");
			return;
		}
		if(uiLayer == (UI_LAYER_NUMBER+1)) {
			descriptionDisplay.Visible = false;
		}
	}

	private void GainFocusFromBookmark(int uiLayer) {
		if (uiLayer == 10)
		{
			menuChoices[selectedIndex].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
		}

	}

	private void DisplayItem(Item item) {
		if(item == null) {
			descriptionDisplay.Visible = false;
		} else {
			descriptionDisplay.UpdateItem(item);
		}
	}
	
}
