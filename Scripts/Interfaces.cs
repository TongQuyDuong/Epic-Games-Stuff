using Godot;
using System;

public interface IDamageable
{
	void TakeDamage(float amount);
	private void Die()
	{

	}
}
