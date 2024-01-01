using Godot;
using System;
using MEC;
using System.Collections.Generic;
using System.Diagnostics;
public partial class SpriteManager : Node
{
	public static SpriteManager Instance;
	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Instance = null;
	}

	public void FlashDamage(Sprite2D sprite) 
	{
		sprite.Modulate = Colors.Red;
		
		Timing.RunCoroutine(Wait());
		sprite.Modulate = Colors.White;
	}

	IEnumerator<double> Wait() 
	{
		yield return Timing.WaitForSeconds(0.1);
	}
}
