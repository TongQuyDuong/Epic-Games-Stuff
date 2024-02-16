using Godot;
using System;

public partial class EnemyHPBar : Node2D
{
	[Export] private TextureProgressBar hpBarMain;
	[Export] private TextureProgressBar hpBarUnder;
	Tween tweenMain;
	Tween tweenUnder;

    public override void _Ready()
    {
		this.Visible = false;
    }

	public void Flip() {
		this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
	}

    public void UpdateHealth(int damage)
	{
		this.Visible = true;
		//Inflate the value so the tween looks smoother
		int trueDamage = damage * 10;

		if (tweenMain != null || tweenUnder != null)
		{
			tweenMain.Kill();
			tweenUnder.Kill();
		}
		tweenMain = GetTree().CreateTween();


		if (damage > 0)
		{

			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - trueDamage), 0.1);
			tweenUnder = GetTree().CreateTween();
			tweenUnder.TweenInterval(0.5);
			tweenUnder.TweenProperty(hpBarUnder, "value", (double)(hpBarMain.Value - trueDamage), 0.3);

		}
		else if (damage < 0)
		{

			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - trueDamage), 0.1);
			tweenMain.Finished += delegate
			{
				hpBarUnder.Value = hpBarMain.Value;
			};
		}
	}
	public void SetMaxHP(int maxHP)
	{
		//Inflate the value so the tween looks smoother
		int trueValue = maxHP * 10;
		hpBarMain.MaxValue = trueValue;
		hpBarUnder.MaxValue = trueValue;
		hpBarMain.Value = trueValue;
		hpBarUnder.Value = trueValue;
	}
}
