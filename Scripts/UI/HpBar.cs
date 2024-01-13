using Godot;
using System;

public partial class HpBar : Control
{
	[Export] private TextureProgressBar hpBarMain;
	[Export] private TextureProgressBar hpBarUnder;
	Tween tweenMain;
	Tween tweenUnder;
	public override void _EnterTree()
	{
		hpBarMain = GetNode<TextureProgressBar>("HpBarMain");
		hpBarUnder = GetNode<TextureProgressBar>("HpBarUnder");
	}
	public void UpdateHealth(int damage) {
		//Inflate the value so the tween looks smoother
		int trueDamage = damage*10;
		
		if(tweenMain != null || tweenUnder != null) {
			tweenMain.Kill();
			tweenUnder.Kill();
		}
		tweenMain = GetTree().CreateTween();
		

		if (damage > 0){
			
			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - trueDamage), 0.1);
			tweenMain.Finished += delegate { UpdateColor(); };
			tweenUnder = GetTree().CreateTween();
			tweenUnder.TweenInterval(0.5);
			tweenUnder.TweenProperty(hpBarUnder, "value", (double)(hpBarMain.Value - trueDamage), 0.3);
			
		} else if(damage < 0) {
			
			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - trueDamage), 0.1);
			tweenMain.Finished += delegate { 
				UpdateColor();
				hpBarUnder.Value = hpBarMain.Value;
			};
		}
	}

	private void UpdateColor() {
		if(hpBarMain.Value <= hpBarMain.MaxValue * 0.2) {
			hpBarMain.TintProgress = Color.FromHtml("#fc1d1a");
			
		} else if (hpBarMain.Value <= hpBarMain.MaxValue * 0.5) {
			hpBarMain.TintProgress = Color.FromHtml("#f9ff00");
			
		} else {
			hpBarMain.TintProgress = Color.FromHtml("#16ff12");
			
		}
	}
	public void SetMaxHP(int maxHP) {
		//Inflate the value so the tween looks smoother
		int trueValue = maxHP*10;
		hpBarMain.MaxValue = trueValue;
		hpBarUnder.MaxValue = trueValue;
		hpBarMain.Value = trueValue;
		hpBarUnder.Value = trueValue;
	}




}
