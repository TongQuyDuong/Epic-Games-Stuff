using Godot;
using System;

public partial class PlayerHealth : UnitHealth
{
	public override void _Ready()
	{
		base._Ready();
	}
	public override void TakeDamage(float amount)
	{
		int Damage = amount <= 0 ? 0 : (int)Mathf.Round(amount);
		unit.ShowPopup(Damage.ToString());
		currentHP -= Damage;
		spriteAnim.Play("Damage");

		BattleUI.Instance.topLeftUI.hpBar.UpdateHealth(Damage);

		if (currentHP <= 0) { 
			Die(); 
			}

	}
	
	protected override void Die() {
		GridManager.Instance.GetPanelAtPosition(unit.currentPos).ResetPanel();
		spriteAnim.Play("Die");
	}

	public void EndBattle() {
		Events.OnBattleEnd?.Invoke();
	}
}
