using Godot;
using System;
using System.Diagnostics;

public partial class AbilityHolder : Node
{
	[Export] private Ability ability;
	private float cooldownTime;
	private float activeTime;
	private float castTime;
	[Export] private BaseUnit user;
	private AbilityState state = AbilityState.ready;
	public AbilitySlot slot = AbilitySlot.first;
	[Export] private PlayerStateController stateCon;

	public override void _Ready()
	{
		
		ability.Initialize(user);
		
	}
	public override void _PhysicsProcess(double delta)
	{
		switch (state)
		{
			case AbilityState.ready:
				if (Input.IsActionJustPressed("Skill1") && stateCon.CurrentPlayerState == PlayerState.Idling)
				{
					
					ability.OnCast?.Invoke();
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
					
					ability.TriggerAbility();
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

public enum AbilityState
{
	ready,
	casting,
	active,
	cooldown

}

public enum AbilitySlot
{
	first,
	second,
	third,
	fourth
}
