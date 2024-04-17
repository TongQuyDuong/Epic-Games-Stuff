using Godot;
using System;

public partial class MenuLayoutManager : Control
{
	public void RemoveLayout() {
		MenuLayout currentLayout = GetChild<MenuLayout>(0);
		if (currentLayout != null) {
			currentLayout.QueueFree();
		}
	}

	public void AddLayout(PackedScene layout,PlayerOverworld player) {
		MenuLayout newLayout = layout.Instantiate<MenuLayout>();
		newLayout.player = player;
		AddChild(newLayout);
	}
}
