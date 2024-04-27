using Godot;
using System;
using MonoCustomResourceRegistry;
using System.Diagnostics;

[RegisteredType(nameof(Equipment), "", nameof(Resource))]
public partial class Equipment : Item
{
	[Export] public EquipmentType equipmentType;
	[Export] Godot.Collections.Array<StatModGlobal> statMods;

	public void Equip(PlayerData data) {
		foreach (StatModGlobal mod in statMods) {
			if (data.playerStats.TryAddStatMod(mod.statType, mod.ToStatMod())) {
				Debug.Print("Added " + mod.statType.ToString() + mod.modValue);
				ResourceSaver.Save(data.playerStats);
			};
			
		}
	}

	public void Unequip(PlayerData data) {
		foreach (StatModGlobal mod in statMods)
		{
			if (data.playerStats.TryRemoveStatMod(mod.statType, mod.modtype, mod.modValue))
			{
				Debug.Print("Removed " + mod.statType.ToString() + mod.modValue);
				ResourceSaver.Save(data.playerStats);
			};
			
		}
	}
}

public enum EquipmentType {
	None = 0,
	Weapon = 1,
	HeadGear = 2,
	BodyArmor = 3,
	Boots = 4
}