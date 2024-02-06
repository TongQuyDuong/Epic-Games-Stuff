using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class StatusEffectsController : Node2D
{
	[Export] BaseUnit unit;
	[Export] public Godot.Collections.Array<StatusEffect> statusEffects = new Godot.Collections.Array<StatusEffect>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		unit = GetParent<CharacterBody2D>() as BaseUnit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (statusEffects.Count > 0)
		{
			ProcessStatusEffects(GetPhysicsProcessDeltaTime());
		}
	}

	private void ProcessStatusEffects(double deltaTime)
	{
		foreach (StatusEffect statusEffect in statusEffects)
		{
			if (!statusEffect.effect.isPermanentEffect) statusEffect.countdown -= (float)deltaTime;

			if (statusEffect.effectType == StatusEffectType.DamageOverTime)
			{
				statusEffect.ProcessDamageOverTime(deltaTime, unit);
			}
		}

		IEnumerable<StatusEffect> timedOutEffects = statusEffects.Where(effect => effect.countdown <= 0);

		if (timedOutEffects.Count() > 0)
		{
			foreach (StatusEffect STeffect in timedOutEffects)
			{
				RemoveStatusEffect(STeffect);
			}
		}


	}

	public void AddStatusEffect(StatusEffectData data)
	{
		if (unit.isStatusImmune) return;

		if (data.type == StatusEffectType.StatChangeEffect)
		{
			statusEffects.Add(new StatusEffect(data));
			data.TriggerEffect(unit);
		}
		else 
		{
			IEnumerable<StatusEffect> alreadyApplied = statusEffects.Where(STeffect => STeffect.effect.Name == data.Name);
			if (alreadyApplied.Count() > 0) {
				alreadyApplied.First().ResetDuration();
			} else {
				statusEffects.Add(new StatusEffect(data));
				data.TriggerEffect(unit);
			}
		}

	}

	public void AddDamageOverTime(DamageOverTime data, BaseUnit source)
	{
		if (unit.isStatusImmune) return;
		float stat;
		source.stats.TryGetStatValue(data.DamageType,out stat);
		float Damage = stat * data.DamageMultiplier;
		statusEffects.Add(new StatusEffect(data, Damage));
		data.TriggerEffect(unit);
	}

	public void RemoveStatusEffect(StatusEffect STeffect)
	{
		STeffect.effect.RemoveEffect(unit);
		statusEffects.Remove(STeffect);
	}

	public void CleanseAllEffectsWith(System.Func<StatusEffect, bool> predicate)
	{
		IEnumerable<StatusEffect> removedEffects = statusEffects.Where(predicate);
		foreach (StatusEffect effect in removedEffects)
		{
			RemoveStatusEffect(effect);
		}
	}
}
