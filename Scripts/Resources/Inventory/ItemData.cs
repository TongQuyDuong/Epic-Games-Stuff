using Godot;
using System;


public partial class ItemData : Resource
{
	public Item item;
	public int quantity = 1;
	public bool isEquipped = false;

	public ItemData(Item item) {
		this.item = item; 
		quantity = 1;
	}

	public ItemData(Item item,int quantity)
	{
		this.item = item;
		this.quantity = quantity;
	}
}
