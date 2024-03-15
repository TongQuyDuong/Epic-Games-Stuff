using Godot;
using System;
using MonoCustomResourceRegistry;
using MEC;
using System.Collections.Generic;

[RegisteredType(nameof(SmiteAbility ), "", nameof(Resource))]
public partial class SmiteAbility : RangedAbility
{
	[Export] PackedScene smiteEffect;
	public override void Initialize()
	{
		//Makes the casting animation that of a Ranged Ability animation
		OnCast = Events.OnAttackRanged;
	}
	public override void TriggerAbility(BaseUnit caster)
	{
		int direction = caster.isFacingRight? 1 : -1;
		for (int i = (int)caster.currentPos.X + direction; i >= 0 && i < 8; i += direction ) {
			Panel panel = GridManager.Instance.GetPanelAtPosition(new Vector2(i,caster.currentPos.Y));
			if(panel.occupiedUnit != null) {
				panel.animationPlayer.Play("Warning");

				caster.stats.TryGetStatValue(StatType.Magic, out float Damage);
				Timing.RunCoroutine(waitForSecondsAndStrike(0.5f,panel,Damage));
				break;
			}
		}
	}

	public void TriggerAbilityAtPosition(BaseUnit caster, Vector2 pos) {
		Panel panel = GridManager.Instance.GetPanelAtPosition(pos);
		panel.animationPlayer.Play("Warning");

		caster.stats.TryGetStatValue(StatType.Magic, out float Damage);
		Timing.RunCoroutine(waitForSecondsAndStrike(0.5f, panel, Damage));
	}

	IEnumerator<double> waitForSecondsAndStrike(float delay, Panel panel,float Damage)
	{
		yield return Timing.WaitForSeconds(delay);
		panel.animationPlayer.Play("RESET");
		panel.animationPlayer.Queue("Flash");

		var smite = (SmiteBehaviour)smiteEffect.Instantiate();
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(smite, (int)panel.Pos.Y);
		smite.GlobalPosition = panel.GlobalPosition;
		UnitManager.Instance.AddChild(smite);

		if(panel.occupiedUnit != null) {
			panel.occupiedUnit.GetNode<IDamageable>("UnitHealth").TakeDamage(Damage);
		}

	}
}
