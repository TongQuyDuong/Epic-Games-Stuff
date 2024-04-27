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
	private Dictionary<int,ItemData> itemList = new Dictionary<int, ItemData>();
	public PlayerData playerData;
	public ItemType currentFilter;
	[Export] private int selectedIndex = 0;
	[Export] private int currentOffset = 0;
	[Export] private int maxOffset;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocus;
		MenuBook.onBackInputReceived += LoseFocus;
		ItemAction.onItemActionSelected += PerformActionOnItem;
		PlayerData.onPlayerInventoryChanged += SyncInventory;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocus;
		MenuBook.onBackInputReceived -= LoseFocus;
		ItemAction.onItemActionSelected -= PerformActionOnItem;
		PlayerData.onPlayerInventoryChanged -= SyncInventory;
	}

	public override void _Ready() {
		foreach (Node item in GetChildren()) {
			itemChoices.Add((MenuChoice)item);
		}

	}

	public void BuildItemList() {
		itemList.Clear();
		int i = 0;
		foreach (KeyValuePair<Item, int> item in playerData.inventory)
		{
			if (item.Key.isStackable)
			{
				itemList[i] = new ItemData(item.Key, item.Value);
				i++;
			}
			else
			{
				for (int j = 0; j < item.Value; j++)
				{
					itemList[i] = new ItemData(item.Key);
					if (j == 0) {
						if (playerData.equippedItems.Values.Contains((Equipment)item.Key)) itemList[i].isEquipped = true;
					}
					i++;
				}
			}
		}

	}

	public void BuildItemList(Func<KeyValuePair<Item,int>,bool> predicate)
	{
		IEnumerable<KeyValuePair<Item,int>> filteredItems = playerData.inventory.Where(predicate);
		itemList.Clear();
		int i = 0;
		foreach (KeyValuePair<Item, int> item in filteredItems)
		{
			if (item.Key.isStackable)
			{
				itemList[i] = new ItemData(item.Key, item.Value);
				i++;
			}
			else
			{
				for (int j = 0; j < item.Value; j++)
				{
					itemList[i] = new ItemData(item.Key);
					if (j == 0)
					{
						if (playerData.equippedItems.Values.Contains((Equipment)item.Key)) itemList[i].isEquipped = true;
					}
					i++;
				}
			}
		}


	}

	public void DisplayItems(ItemType filter) {
		selectedIndex = 0;
		currentOffset = 0;
		currentFilter = filter;

		if (currentFilter == ItemType.None)
		{
			BuildItemList();
		}
		else
		{
			BuildItemList(item => item.Key.itemType == currentFilter);
		}

		maxOffset = itemList.Count > itemChoices.Count ? itemList.Count - itemChoices.Count : 0;


		for (int i = 0; i < itemChoices.Count; i++) {
			if (i >= itemList.Count) { 
				itemChoices[i].Visible = false;
				continue;
			}

			itemChoices[i].Visible = true;
			itemChoices[i].SetContent(itemList[i]);
			
		}
	}

	private void DisplayOffset(int offset) {
		for (int i = 0; i < itemChoices.Count; i++) {
			if (i >= itemList.Count)
			{
				itemChoices[i].Visible = false;
				continue;
			}
			itemChoices[i].SetContent(itemList[i+offset]);
		}
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if(itemList.Count <= 1) return;
		if (UiLayer == UI_LAYER_NUMBER)
		{
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

			onSelectedItemChanged?.Invoke(itemList[selectedIndex].item);
			
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
			onSelectedItemChanged?.Invoke(itemList[selectedIndex-currentOffset].item);
		}

		if (uiLayer == (UI_LAYER_NUMBER + 1)) {
			onItemSelected?.Invoke(itemList[selectedIndex].item);
		}
	}

	private void PerformActionOnItem(string action) {
		Item currentItem = itemList[selectedIndex].item;
		switch (action) {
			case "Equip":
				if (itemList[selectedIndex].isEquipped == true) break;
				playerData.EquipItem((Equipment)currentItem);
				itemList[selectedIndex].isEquipped = true;
				itemChoices[selectedIndex - currentOffset].SetContent(itemList[selectedIndex]);
				break;
			case "Unequip":
				if (itemList[selectedIndex].isEquipped == false) break;
				playerData.UnequipItem(((Equipment)currentItem).equipmentType);
				itemList[selectedIndex].isEquipped = false;
				itemChoices[selectedIndex - currentOffset].SetContent(itemList[selectedIndex]);
				break;
			case "Use":
				if (currentItem.itemType == ItemType.Consumable) {
					((RecoveryItem)currentItem).UseItem();
				} else {
					((KeyItem)currentItem).UseItem();
				}
				break;
			case "Drop":
				if (currentItem.itemType == ItemType.KeyItem) break;
				playerData.RemoveItem(currentItem,1);
				break;
		}
	}

	private void SyncInventory() {
		if (currentFilter == ItemType.None) {
			BuildItemList();
		} else {
			BuildItemList(item => item.Key.itemType == currentFilter);
		}

		maxOffset = itemList.Count > itemChoices.Count ? itemList.Count - itemChoices.Count : 0;
		if (itemList.Count <= 0) {
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER - 1);
			itemChoices[selectedIndex].ToggleSelect();
			itemChoices[selectedIndex].Visible = false;
			return;
		}
		if (currentOffset > maxOffset) currentOffset = maxOffset;
		if (selectedIndex >= itemList.Count) {
			if (maxOffset == 0) {
				itemChoices[selectedIndex].ToggleSelect();
				selectedIndex = itemList.Count - 1;
				itemChoices[selectedIndex].ToggleSelect();
			} else {
				selectedIndex = itemList.Count - 1;
			}
			
		}

		DisplayOffset(currentOffset);
	}
}
