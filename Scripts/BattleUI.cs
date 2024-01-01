using Godot;
using System;

public partial class BattleUI : Control
{
	public static BattleUI Instance;
	[Export] public AbilityIcon Slot1;
	[Export] public AbilityIcon Slot2;
	[Export] public AbilityIcon Slot3;
	[Export] public TopLeftUI topLeftUI;
	[Export] public PackedScene announcement;

	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
		
	}

	
	public void SetMaxHP(int maxHP) {
		topLeftUI.maxHP = maxHP;
	}
	public void DisplayAbility(int abilityIndex, CompressedTexture2D image, int numberOfCharges) {
		switch(abilityIndex) {
			case 0:
				Slot1.SetImage(image);
				Slot1.numberOfCharges = numberOfCharges;
				Slot1.UpdateLabel();
				break;
			case 1:
				Slot2.SetImage(image);
				Slot2.numberOfCharges = numberOfCharges;
				Slot2.UpdateLabel();
				break;
			case 2:
				Slot3.SetImage(image);
				Slot3.numberOfCharges = numberOfCharges;
				Slot3.UpdateLabel();
				break;
		}
		
	}
	public void BeginCooldown(int abilityIndex,float cooldown) {
		switch (abilityIndex)
		{
			case 0:
				Slot1.BeginCooldown(cooldown);
				break;
			case 1:
				Slot2.BeginCooldown(cooldown);
				break;
			case 2:
				Slot3.BeginCooldown(cooldown);
				break;
		}
		
	}

	public void AnnounceBattle(bool isBattleStart) {
		BattleAnnouncement announce = announcement.Instantiate<BattleAnnouncement>();
		announce.isBattleStart = isBattleStart;
		AddChild(announce);
		
	}
}
