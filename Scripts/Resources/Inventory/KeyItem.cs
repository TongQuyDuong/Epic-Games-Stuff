using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(KeyItem), "", nameof(Resource))]
public partial class KeyItem : Item
{
	public virtual void UseItem()
	{

	}
}
