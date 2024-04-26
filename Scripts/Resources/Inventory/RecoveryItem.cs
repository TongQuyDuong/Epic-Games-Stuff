using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(RecoveryItem), "", nameof(Resource))]
public partial class RecoveryItem : Consumable
{
	public static Action<Item> onHealingItemConsumed;
	[Export] public int recoveryValue;


    public override void UseItem()
    {
        onHealingItemConsumed?.Invoke(this);
    }
}

