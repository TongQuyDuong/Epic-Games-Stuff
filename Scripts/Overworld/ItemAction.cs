using Godot;
using System;
using System.Diagnostics;

public partial class ItemAction : PanelContainer
{
	const int UI_LAYER_NUMBER = 3;
	[Export] Godot.Collections.Array<MenuChoice> actionChoices;
	public static Action<string> onItemActionSelected;
	int numberOfActions;
	int selectedIndex = 0;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocus;
		MenuBook.onBackInputReceived += LoseFocus;
		InventoryListManager.onItemSelected += PopOut;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocus;
		MenuBook.onBackInputReceived -= LoseFocus;
		InventoryListManager.onItemSelected -= PopOut;
	}

	public override void _Ready() {
		this.Visible = false;
	}

	public void PopOut(Item item) {
		switch(item.itemType) {
			case ItemType.Equipment:
			numberOfActions = 3;
			actionChoices[0].SetContent("Equip");
			actionChoices[1].SetContent("Unequip");
			actionChoices[2].SetContent("Drop");
			break;
			case ItemType.Consumable:
			numberOfActions = 2;
			actionChoices[0].SetContent("Use");
			actionChoices[1].SetContent("Drop");
			actionChoices[2].Visible = false;
			break;
			case ItemType.MonsterLoot:
			numberOfActions = 1;
			actionChoices[0].SetContent("Drop");
			actionChoices[1].Visible = false;
			actionChoices[2].Visible = false;
			break;
			case ItemType.KeyItem:
			numberOfActions = 2;
			actionChoices[0].SetContent("Use");
			actionChoices[1].SetContent("Drop");
			actionChoices[2].Visible = false;
			break;
		}
		this.Visible = true;
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if (this.Visible == false || numberOfActions <= 1) return;
		if (UiLayer == UI_LAYER_NUMBER)
		{
			actionChoices[selectedIndex].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = numberOfActions - 1;
			}
			else if (selectedIndex >= numberOfActions)
			{
				selectedIndex = 0;
			}
			actionChoices[selectedIndex].ToggleSelect();
		}
	}

	private void LoseFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			this.Visible = false;
			actionChoices[selectedIndex].ToggleSelect();
			Debug.Print(UI_LAYER_NUMBER + "Lose Focus");
		}
	}

	private void GainFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			selectedIndex = 0;
			actionChoices[selectedIndex].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
		}

		if(uiLayer == (UI_LAYER_NUMBER + 1)) {
			onItemActionSelected?.Invoke(actionChoices[selectedIndex].GetText());
			this.Visible = false;
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER - 1);
		}
	}
}
