using Godot;
using System;


public partial class SkillsLayout : MenuLayout
{
	[Export] SkillListManager skillList;
	[Export] TextureRect skillIconDisplay;
	[Export] Label skillNameDisplay;
	[Export] Label skillDescriptionDisplay;

	public override void _EnterTree() {
		skillList.onSelectedAbilityChanged += UpdateSkillDisplay;
	}

	public override void _ExitTree() {
		skillList.onSelectedAbilityChanged -= UpdateSkillDisplay;
	}

	public override void _Ready() {
		skillList.playerData = player.playerData;
		skillList.BuildAbilityList();
	}

	private void UpdateSkillDisplay(Ability skill) {
		if (skill != null) {
			skillIconDisplay.Texture = skill.Icon;
			skillNameDisplay.Text = skill.Name;
			skillDescriptionDisplay.Text = skill.Description;
		} else {
			skillIconDisplay.Texture = null;
			skillNameDisplay.Text = "";
			skillDescriptionDisplay.Text = "";
		}
	}
}


