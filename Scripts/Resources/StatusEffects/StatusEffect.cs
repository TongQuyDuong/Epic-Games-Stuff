using Godot;
using System;



public partial class StatusEffect : Resource
{
	[Export] public StatusEffectData effect;
	[Export] public float countdown;
	[Export] public StatusEffectType effectType;
	//For StatChanges
	public StatModifier mod;
	//For DamageOverTime
	public float Damage;
	[Export] public float interval;
	// For ControlEffects
	public Node2D Ceffect;


	public StatusEffect(StatusEffectData data) {
		effect = data;
		countdown = data.duration;
		effectType = data.type;
		if(effectType == StatusEffectType.StatChangeEffect) mod = ((StatChangeEffect)data).mod;
		if (effectType == StatusEffectType.ControlEffect) Ceffect = ((ControlEffect)data).Ceffect;
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

}

public enum StatusEffectType {
	ControlEffect,
	DamageOverTime,
	StatChangeEffect
}
