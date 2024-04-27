using Godot;
using System;

public partial class ProfileLayout : MenuLayout
{
	[Export] private Label currentHpDisplay;
	[Export] private Label maxHpDisplay;
	[Export] private Label atkDisplay;
	[Export] private Label defDisplay;
	[Export] private Label magicDisplay;
	[Export] private Label resDisplay;
	[Export] private TextureRect portraitDisplay;
	[Export] private EquipmentDisplayManager equipmentDisplay;

	public override void _Ready() {
		portraitDisplay.Texture = player.portrait;
		currentHpDisplay.Text = player.playerData.currentHP.ToString();

		player.playerData.playerStats.TryGetStatValue(StatType.HP, out float hp);
		player.playerData.playerStats.TryGetStatValue(StatType.Attack, out float attack);
		player.playerData.playerStats.TryGetStatValue(StatType.Defence, out float defence);
		player.playerData.playerStats.TryGetStatValue(StatType.Magic, out float magic);
		player.playerData.playerStats.TryGetStatValue(StatType.Resistance, out float resistance);

		maxHpDisplay.Text = hp.ToString();
		atkDisplay.Text = attack.ToString();
		defDisplay.Text = defence.ToString();
		magicDisplay.Text = magic.ToString();
		resDisplay.Text = resistance.ToString();

		equipmentDisplay.DisplayEquipments(player.playerData);
	}
}
