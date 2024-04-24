using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class InventoryListManager : VBoxContainer
{
	const int UI_LAYER_NUMBER = 2;
	const int OFFSET_BOUNDARY = 3;
	public Action<Item> onSelectedItemChanged;
	public static Action<Item> onItemSelected;
	[Export] Godot.Collections.Array<MenuChoice> itemChoices = new Godot.Collections.Array<MenuChoice>();
	private Dictionary<int,Item> itemList = new Dictionary<int, Item>();

	[Export] private int selectedIndex = 0;
	[Export] private int currentOffset = 0;
	[Export] private int maxOffset;

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

	public void DisplayItems(IEnumerable<Item> items) {
		selectedIndex = 0;
		currentOffset = 0;
		Debug.Print("Items :" + items.Count());
		itemList.Clear();
		int i = 0;
		foreach (Item item in items) {
			if(item.isStackable) {
				itemList[i] = item;
				i++;
			} else {
				for (int j = 0; j < item.quantity; j++) {
					itemList[i] = item;
					i++;
				}
			}
		}

		Debug.Print("Items list :" + itemList.Count);
		Debug.Print("Items choice:" + itemChoices.Count);

		maxOffset = itemList.Count > itemChoices.Count ? itemList.Count - itemChoices.Count : 0;


		for (i = 0; i < itemChoices.Count; i++) {
			if (i >= itemList.Count) { 
				itemChoices[i].Visible = false;
				continue;
			}

			itemChoices[i].Visible = true;
			itemChoices[i].SetContent(itemList[i]);
			
		}
	}

	private void DisplayOffset(int offset) {
		if(maxOffset == 0) return;
		for (int i = 0; i < itemChoices.Count; i++) {
			itemChoices[i].SetContent(itemList[i+offset]);
		}
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if(itemList.Count <= 1) return;
		if (UiLayer == UI_LAYER_NUMBER)
		{
			Debug.Print("Current Offset: " + currentOffset);
			itemChoices[selectedIndex - currentOffset].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = itemList.Count - 1;
				currentOffset = maxOffset;
				DisplayOffset(maxOffset);
				itemChoices[selectedIndex - currentOffset].ToggleSelect();
			}
			else if ((selectedIndex - currentOffset) < OFFSET_BOUNDARY && currentOffset > 0) {
				currentOffset--;
				itemChoices[selectedIndex - currentOffset].ToggleSelect();
				DisplayOffset(currentOffset);
			}
			else if ((selectedIndex - currentOffset) > (itemChoices.Count-OFFSET_BOUNDARY) && currentOffset < maxOffset)
			{
				currentOffset++;
				itemChoices[selectedIndex - currentOffset].ToggleSelect();
				DisplayOffset(currentOffset);
			}
			else if (selectedIndex >= itemList.Count)
			{
				selectedIndex = 0;
				currentOffset = 0;
				DisplayOffset(0);
				itemChoices[0].ToggleSelect();
			} else {
				itemChoices[selectedIndex - currentOffset].ToggleSelect();
			}

			onSelectedItemChanged?.Invoke(itemList[selectedIndex]);
			
		}
	}

	private void LoseFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			itemChoices[selectedIndex - currentOffset].ToggleSelect();
			Debug.Print(UI_LAYER_NUMBER + "Lose Focus");
		}
	}

	private void GainFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			if (itemList.Count == 0) return;
			itemChoices[selectedIndex-currentOffset].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
			onSelectedItemChanged?.Invoke(itemList[selectedIndex-currentOffset]);
		}

		if (uiLayer == (UI_LAYER_NUMBER + 1)) {
			onItemSelected?.Invoke(itemList[selectedIndex]);
		}
	}
}
