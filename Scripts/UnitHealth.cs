using Godot;
using System;

public partial class UnitHealth : Node, IDamageable
{
	[Export] protected BaseUnit unit;
	[Export] public int currentHP;
	[Export] private int maxHP;
	[Export] public AnimationPlayer spriteAnim;

	// public UnitHealth(int aMaxHp  )
	// {
	// 	maxHP = aMaxHp;
	// 	currentHP = maxHP;
	// }

	public override void _Ready()
	{
		unit = GetParent<CharacterBody2D>() as BaseUnit;
		unit.stats.TryGetStatValue(StatType.HP,out float value);
		maxHP = (int)value;
		currentHP = maxHP;
	}
	public virtual void TakeDamage(float amount)
	{
		int Damage = amount <= 0? 0 : (int)Mathf.Round(amount);
		unit.ShowPopup(Damage.ToString());
		if (unit is BaseEnemy)
		{
			((BaseEnemy)unit).hpBar.UpdateHealth(Damage);
			((BaseEnemy)unit).stateCon.ChangeState(EnemyState.Damaged);
		}
		currentHP -= Damage;
		spriteAnim.Play("Damage");
		if (currentHP <= 0) { Die(); }
		
	}

	public virtual void TakeDamageWithoutAnimation(float amount) {
		int Damage = amount <= 0 ? 0 : (int)Mathf.Round(amount);
		unit.ShowPopup(Damage.ToString());
		if (unit is BaseEnemy)
		{
			((BaseEnemy)unit).hpBar.UpdateHealth(Damage);
		}
		currentHP -= Damage;
		if (currentHP <= 0) { Die(); }
	}


	public void HealUnit(float amount)
	{
		if (currentHP < maxHP)
		{
			currentHP += (int)Mathf.Round(amount);
		}
		if (currentHP > maxHP)
		{
			currentHP = maxHP;
		}
	}
	protected virtual void Die()
	{
		unit.STeffectCon.CleanseAllEffectsWith(effect => effect.effectType == StatusEffectType.ControlEffect);
		unit.isControlled = true;
		GridManager.Instance.GetPanelAtPosition(unit.currentPos).ResetPanel();
		if(unit is BaseEnemy) {
			Events.OnEnemyDeath?.Invoke();
		}
		spriteAnim.Play("Die");
	}
}
