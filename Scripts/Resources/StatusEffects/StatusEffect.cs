using Godot;
using System;
using System.Data.Common;
using System.Threading;

[GlobalClass]
public partial class StatusEffect : Resource
{
	[Export] public StatusEffectData effect;
	[Export] public float countdown;
	
	[Export] public StatusEffectType effectType;
	//For DamageOverTime
	public float Damage;
	[Export] public float interval;


	public StatusEffect(StatusEffectData data) {
		effect = data;
		countdown = data.duration;
		effectType = data.type;
		if(effectType == StatusEffectType.DamageOverTime) interval = ((DamageOverTime)data).interval;
		Damage = 0;
	}

	public StatusEffect(StatusEffectData data,float Damage) {
		effect = data;
		countdown = data.duration;
		effectType = data.type;
		if (effectType == StatusEffectType.DamageOverTime) {
			interval = ((DamageOverTime)data).interval;
			this.Damage = Damage;
		}
		
	}

	public void ResetDuration() {
		countdown = effect.duration;
	}

	public void ResetInterval()
	{
		interval = ((DamageOverTime)effect).interval;
	}

	public void ProcessDamageOverTime(double deltaTime,BaseUnit target)
	{

		interval -= (float)deltaTime;
		if (interval <= 0)
		{
			((DamageOverTime)effect).DealDamage(Damage,target);
			ResetInterval();
		}

	}
}

public enum StatusEffectType {
	ControlEffect,
	DamageOverTime,
	StatChangeEffect
}
