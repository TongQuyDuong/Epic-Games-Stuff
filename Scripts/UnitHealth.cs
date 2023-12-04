using Godot;
using System;

[Tool]
public partial class UnitHealth : Node, IDamageable
{
	[Export] public BaseUnit unit;
	[Export] public int currentHP { get; private set; }
	[Export] private int maxHP;
	[Export] public AnimationPlayer spriteAnim;

	// public UnitHealth(int aMaxHp  )
	// {
	// 	maxHP = aMaxHp;
	// 	currentHP = maxHP;
	// }

	public override void _Ready()
	{
		unit = this.GetParent() as BaseUnit;
		currentHP = maxHP;
	}
	public void TakeDamage(float amount)
	{
		currentHP -= (int)Mathf.Round(amount);
		spriteAnim.Play("Damage");
		if (currentHP < 0) { Die(); }
		
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
	private void Die()
	{
		GridManager.Instance.GetPanelAtPosition(unit.currentPos).ResetPanel();
		unit.QueueFree();
	}
}
