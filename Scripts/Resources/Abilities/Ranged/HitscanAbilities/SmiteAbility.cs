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
		firingPoint = caster.GetNode<Marker2D>("FiringPoint");
		int direction = caster.isFacingRight? 1 : -1;
		Panel panel;
		for (int i = (int)caster.currentPos.X + direction; i >= 0 && i < 8; i += direction ) {
			panel = GridManager.Instance.GetPanelAtPosition(new Vector2(i,caster.currentPos.Y));
			if(panel.occupiedUnit != null) {
				panel.animationPlayer.Play("Warning");

				caster.stats.TryGetStatValue(StatType.Magic, out float Damage);
				Timing.RunCoroutine(waitForSecondsAndStrike(1,panel,Damage));
				break;
			}
		}
	}

	IEnumerator<double> waitForSecondsAndStrike(float delay, Panel panel,float Damage)
	{
		yield return Timing.WaitForSeconds(delay);
		panel.animationPlayer.Play("RESET");
		panel.animationPlayer.Queue("Flash");

		var smite = (SmiteBehaviour)smiteEffect.Instantiate();
		smite.currentPos = panel.Pos;
		smite.GlobalPosition = panel.GlobalPosition;
		UnitManager.Instance.AddChild(smite);

		if(panel.occupiedUnit != null) {
			panel.occupiedUnit.GetNode<IDamageable>("UnitHealth").TakeDamage(Damage);
		}

	}
}
