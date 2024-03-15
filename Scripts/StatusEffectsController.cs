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
	[Export] private StatusBar statusBar;
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
				statusEffect.interval -= (float)deltaTime;
				// if (statusEffect.interval <= 0)
				// {
				// 	((DamageOverTime)statusEffect.effect).DealDamage(statusEffect.Damage, unit.GetNode<UnitHealth>("UnitHealth"));
				// 	statusEffect.ResetInterval();
				// }
			}
		}

		IEnumerable<StatusEffect> timedOutEffects = statusEffects.Where(effect => effect.countdown <= 0);
		IEnumerable<StatusEffect> triggeredEffects = statusEffects.Where(effect => effect.effectType == StatusEffectType.DamageOverTime && effect.interval <= 0f);

		if (timedOutEffects.Count() > 0)
		{
			RemoveStatusEffect(timedOutEffects.ToList());
		}
		
		if(triggeredEffects.Count() > 0) {
			float sumDamage = 0;
			foreach(StatusEffect DoT in triggeredEffects) {
				sumDamage += DoT.Damage;
				DoT.ResetInterval();
			}
			unit.GetNode<UnitHealth>("UnitHealth").TakeDamage(sumDamage);
			sumDamage = 0;
		}





	}

	public void AddStatusEffect(Godot.Collections.Array<StatusEffectData> datas, BaseUnit source)
	{
		if (unit.isStatusImmune) return;

		for(int i = 0; i < datas.Count; i++) {
			switch(datas[i].type) {
				case StatusEffectType.StatChangeEffect:
					datas[i].TriggerEffect(unit);
					statusEffects.Add(new StatusEffect(datas[i]));

					break;
				case StatusEffectType.ControlEffect:
						IEnumerable<StatusEffect> alreadyApplied = statusEffects.Where(STeffect => STeffect.effect.Name == datas[i].Name);
						if (alreadyApplied.Count() > 0) {
							alreadyApplied.First().ResetDuration();
						} else {
						datas[i].TriggerEffect(unit);
						statusEffects.Add(new StatusEffect(datas[i]));
						}
					break;
				case StatusEffectType.DamageOverTime:

						alreadyApplied = statusEffects.Where(STeffect => STeffect.effect.Name == datas[i].Name);
						if (alreadyApplied.Count() > 0)
						{
							alreadyApplied.First().ResetDuration();
						}
						else
						{
							float stat;
							source.stats.TryGetStatValue(((DamageOverTime)datas[i]).DamageType, out stat);
							float Damage = stat * ((DamageOverTime)datas[i]).DamageMultiplier;
							statusEffects.Add(new StatusEffect(datas[i], Damage));
						}
					break;
				default:
				break;

			}
		}
		if(statusBar != null) {
			statusBar.DisplayStatusEffects(statusEffects);
		}

	}

	public void RemoveStatusEffect(List<StatusEffect> timedOutEffects)
	{
		for (int i = 0;i < timedOutEffects.Count; i++)
		{
			switch(timedOutEffects[i].effectType) {
				case StatusEffectType.StatChangeEffect:
					((StatChangeEffect)timedOutEffects[i].effect).RemoveEffect(unit, timedOutEffects[i].mod);
					statusEffects.Remove(timedOutEffects[i]).ToString();
					break;
				case StatusEffectType.ControlEffect:
					((ControlEffect)timedOutEffects[i].effect).RemoveEffect(unit, timedOutEffects[i].Ceffect);
					statusEffects.Remove(timedOutEffects[i]).ToString();
					break;
				default:
					timedOutEffects[i].effect.RemoveEffect(unit);
					statusEffects.Remove(timedOutEffects[i]).ToString();
					break;
			}

		}
		

		statusBar.DisplayStatusEffects(statusEffects);

	}

	public void CleanseAllEffectsWith(System.Func<StatusEffect, bool> predicate)
	{
		IEnumerable<StatusEffect> removedEffects = statusEffects.Where(predicate);
		foreach (StatusEffect effect in removedEffects)
		{
			RemoveStatusEffect(removedEffects.ToList());
		}
	}
}
