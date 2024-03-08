using Godot;
using System;
using System.Reflection.Metadata;

public partial class TopLeftUI : Control
{
	private const float SoulPowerStartValue = 18;
	// Soul Power bar have 82 values so this will fill to full in 10 secs
	public const float BaseRechargeSpeed = 8.2f;
	private double _timeElasped = 0;
	[Export] public int maxHP;
	[Export] private TextureRect Portrait;
	[Export] public HpBar hpBar;
	[Export] public Label SoulPowerDisplay;
	[Export] public GlowableBar SoulPowerBar;
	public int soulPowerPerCycle;
	private int maxSoulPowerPerCycle;
	public int currentSoulPower = 0;
	private float rechargeSpeed;
	private double rechargeInterval;
	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		SoulPowerBar.Value = SoulPowerStartValue;
		SoulPowerDisplay.Visible = false;
	}

    public override void _PhysicsProcess(double delta)
    {
		if(currentSoulPower < maxSoulPowerPerCycle) {
			_timeElasped += GetPhysicsProcessDeltaTime();
			if(_timeElasped >= rechargeInterval) {
				_timeElasped = 0;
				currentSoulPower += 1;
				SoulPowerDisplay.Text = currentSoulPower.ToString();
			}
		}
    }

	public void UpdateSPInfo(int SPperCycle, float rechargeSpd) {
		soulPowerPerCycle = SPperCycle;
		maxSoulPowerPerCycle = (int)Math.Round(SPperCycle*1.5);
		rechargeSpeed = rechargeSpd;
		rechargeInterval = (1f/SPperCycle)*(10f/rechargeSpd);
	}

	public void ResetSoulPower() {
		currentSoulPower = 0;
		SoulPowerDisplay.Text = currentSoulPower.ToString();
		_timeElasped = 0;
		SoulPowerBar.Value = SoulPowerStartValue;
		SoulPowerBar.StopGlow();
		StartTween();
	}

	public void StartTween() {
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(SoulPowerBar,"value",100f,10f/rechargeSpeed);
		tween.Finished += delegate {
			SoulPowerBar.StartGlow();
		};
	}

	public void ShowSP(int SP) {
		currentSoulPower = SP;
		SoulPowerDisplay.Text = currentSoulPower.ToString();
		SoulPowerDisplay.Visible = true;
	}
}
