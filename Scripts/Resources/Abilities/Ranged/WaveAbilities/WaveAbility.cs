using Godot;
using System;
using MonoCustomResourceRegistry;
using MEC;
using System.Collections.Generic;

[RegisteredType(nameof(WaveAbility), "", nameof(Resource))]
public partial class WaveAbility : RangedAbility
{
	[Export] PackedScene waveEffect;
	[Export] float delay;
	public override void Initialize()
	{
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;
	}

	public override void TriggerAbility(BaseUnit caster)
	{
		int direction = caster.isFacingRight ? 1 : -1;
		Panel panel;
		int index = 0;
		for (int i = (int)caster.currentPos.X + direction; i >= 0 && i < 8; i += direction)
		{
			panel = GridManager.Instance.GetPanelAtPosition(new Vector2I(i, caster.currentPos.Y));
			caster.stats.TryGetStatValue(scaleStat, out float Damage);
			Timing.RunCoroutine(waitForSecondsAndSpawn(index*delay, panel, Damage));
			index++;
		}
	}

	IEnumerator<double> waitForSecondsAndSpawn(float delay, Panel panel, float Damage)
	{
		yield return Timing.WaitForSeconds(delay);
		panel.animationPlayer.Play("RESET");
		panel.animationPlayer.Queue("Flash");

		var wave = (Node2D)waveEffect.Instantiate();
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(wave, (int)panel.Pos.Y);
		wave.GlobalPosition = panel.GlobalPosition;
		UnitManager.Instance.AddChild(wave);

		if (panel.occupiedUnit != null)
		{
			panel.occupiedUnit.GetNode<IDamageable>("UnitHealth").TakeDamage(Damage);
		}

	}
}
