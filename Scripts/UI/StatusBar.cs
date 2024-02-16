using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class StatusBar : Node2D
{
	private Godot.Collections.Array<StatusIcon> statusIcons = new Godot.Collections.Array<StatusIcon>();
	private Dictionary<CompressedTexture2D,int> values = new Dictionary<CompressedTexture2D, int>();
	[Export] PackedScene statusIcon;

	public void DisplayStatusEffects(Godot.Collections.Array<StatusEffect> statusEffects) {
		if(statusEffects.Count == 0) 
		{
			this.Visible = false;
			return;
		}
		this.Visible = true;
		for(int i = 0; i < statusEffects.Count; i++) {
			if(statusEffects[i].effectType == StatusEffectType.ControlEffect) continue;
			if(values.Any(status => status.Key == statusEffects[i].effect.Icon)) {
				values[statusEffects[i].effect.Icon] ++;
			} else {
				values.Add(statusEffects[i].effect.Icon, 1);
			}
		}
		InstantiateIcons();
	}

	private void InstantiateIcons() {	
		ResetStatusBar();
		for(int i = 0; i < values.Count; i++) {
			StatusIcon icon = statusIcon.Instantiate<StatusIcon>();
			statusIcons.Add(icon);
			icon.icon = values.ElementAt(i).Key;
			icon.numberOfStacks = values.ElementAt(i).Value;
			icon.Position = GetNode<Marker2D>("StatusSlot" + (i+1)).Position;
			this.AddChild(icon);
			if (i==4) break;
		}

		values.Clear();
	}

	private void ResetStatusBar() {
		if (statusIcons.Count > 0) {
			foreach (StatusIcon icon in statusIcons) {
				icon.QueueFree();
			}
			statusIcons.Clear();
		}
	}
}
