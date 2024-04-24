using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(Equipment), "", nameof(Resource))]
public partial class Equipment : Item
{
	public bool isEquipped;
	[Export] Godot.Collections.Array<StatModGlobal> statMods;

	public void Equip(PlayerData data) {
		isEquipped = true;
		foreach (StatModGlobal mod in statMods) {
			data.playerStats.TryAddStatMod(mod.statType,mod.ToStatMod());
		}
	}

	public void Unequip(PlayerData data) {
		isEquipped = false;
		foreach (StatModGlobal mod in statMods)
		{
			data.playerStats.TryRemoveStatMod(mod.statType, mod.modtype, mod.modValue);
		}
	}

	public Equipment(string name, ItemType type, bool isStackable, int quantity) : base(name,type,isStackable,quantity)
	{

	}
}
