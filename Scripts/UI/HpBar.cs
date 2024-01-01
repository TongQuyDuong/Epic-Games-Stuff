using Godot;
using System;

public partial class HpBar : Control
{
	private TextureProgressBar hpBarMain;
	private TextureProgressBar hpBarUnder;
	Tween tweenMain;
	Tween tweenUnder;
	public override void _EnterTree()
	{
		hpBarMain = GetNode<TextureProgressBar>("HpBarMain");
		hpBarUnder = GetNode<TextureProgressBar>("HpBarUnder");
		
	}
    public void UpdateHealth(int damage) {
		if(GetTree().GetProcessedTweens().Count > 0) {
			tweenMain.Kill();
			tweenUnder.Kill();
		}
		tweenMain = GetTree().CreateTween();
		

		if (damage > 0){
			
			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - damage), 0.1);
			tweenMain.Finished += delegate { UpdateColor(); };
			tweenUnder = GetTree().CreateTween();
			tweenUnder.TweenInterval(0.5);
			tweenUnder.TweenProperty(hpBarUnder, "value", (double)(hpBarMain.Value - damage), 0.2);
			
		} else if(damage < 0) {
			
			tweenMain.TweenProperty(hpBarMain, "value", (double)(hpBarMain.Value - damage), 0.1);
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
		hpBarMain.MaxValue = maxHP;
		hpBarUnder.MaxValue = maxHP;
		hpBarMain.Value = maxHP;
		hpBarUnder.Value = maxHP;
	}


}
