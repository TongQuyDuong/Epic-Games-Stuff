using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(Consumable), "", nameof(Resource))]
public partial class Consumable : Item
{
	
	public virtual void UseItem() {

	}
}
