using Godot;
using System;

public partial class EquipmentDisplayManager : VBoxContainer
{
	[Export] Godot.Collections.Array<EquipmentLoadoutDisplay> displays;

	public void DisplayEquipments(PlayerData data) {
		for (int i = 1; i <= 4; i++) {
			if (data.equippedItems.ContainsKey((EquipmentType)i) == false)
			{
				data.equippedItems[(EquipmentType)i] = null;
				ResourceSaver.Save(data);
			}
			displays[i-1].SetItem(data.equippedItems[(EquipmentType)i]);
		}
	}
}
