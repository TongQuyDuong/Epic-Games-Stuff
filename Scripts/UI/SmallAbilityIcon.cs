using Godot;
using System;

public partial class SmallAbilityIcon : SelectSkillButton
{
	[Export] public Ability ability;
	public static Action onAbilityPress;

	public override void _Ready()
	{
		isActive = false;
	}
	

	public void UpdateAbility(Ability a) {
		ability = a;
		isActive = true;
		icon.Texture = ability.smallIcon;
		this.Visible = true;
		animPlayer.Play("RESET");
	}

	public void RemoveAbility() {
		ability = null;
		isActive = false;
		this.Visible = false;
		
	}

    public override void SelectButton()
    {
        onAbilityPress?.Invoke();
    }
}
