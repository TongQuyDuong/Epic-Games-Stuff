using Godot;
using System;

public partial class ItemData
{
	public Item item;
	public int quantity = 0;

	public ItemData(Item item) {
		this.item = item; 
		quantity = 1;
	}
}
