using Godot;
using System;
using System.Diagnostics;

public partial class AbilityHolder : Node
{
	[Export] public Ability ability;
	private float cooldownTime;
	private float activeTime;
	private float castTime;
	[Export] private BaseUnit user;
	private AbilityState state = AbilityState.ready;
	[Export] public AbilitySlot slotNumber;
	[Export] private PlayerStateController stateCon;
	[Export] public int numberOfCharges;

	public override void _Ready()
	{
		ability.Initialize();
		BattleUI.Instance.DisplayAbility((int)slotNumber,ability.Icon,numberOfCharges);
	}
	public override void _PhysicsProcess(double delta)
	{
		if(ability != null) {
		switch (state)
		{
			case AbilityState.ready:
				if (user.isControlled == true) break;
				if (Input.IsActionJustPressed("Skill" + ((int)slotNumber+1)) && stateCon.CurrentPlayerState == PlayerState.Idling)
				{
					
					ability.OnCast?.Invoke();
					numberOfCharges -= 1;
					if (numberOfCharges <= 0) {
						
						BattleUI.Instance.DisplayAbility((int)slotNumber, null, 0);
						state = AbilityState.casting;
						castTime = ability.castTime;
						break; 
					}
					BattleUI.Instance.BeginCooldown((int)slotNumber, ability.cooldown + ability.castTime);
					state = AbilityState.casting;
					castTime = ability.castTime;
					
				}
				break;
			case AbilityState.casting:
				if (castTime > 0f)
				{
					castTime -= (float)GetPhysicsProcessDeltaTime();
				}
				else
				{
					ability.TriggerAbility(user);
					state = AbilityState.active;
					activeTime = ability.activeTime;
				}
				break;

			case AbilityState.active:
				if (activeTime > 0f)
				{
					activeTime -= (float)GetPhysicsProcessDeltaTime();
				}
				else
				{
					if(numberOfCharges <= 0) {
						ability = null;
						state = AbilityState.empty;
						break;
					}
					state = AbilityState.cooldown;
					cooldownTime = ability.cooldown;
					
				}
				break;

			case AbilityState.cooldown:
				if (cooldownTime > 0f)
				{
					cooldownTime -= (float)GetPhysicsProcessDeltaTime();
				}
				else
				{
					state = AbilityState.ready;
				}
				break;

			default:
				break;
		}
		}
	}
}

public enum AbilityState
{
	ready,
	casting,
	active,
	cooldown,
	empty

}

public enum AbilitySlot
{
	First = 0,
	Second = 1,
	Third = 2,
	Fourth
}
