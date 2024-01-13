using Godot;
using System;

public partial class BattleUI : Control
{
	public static BattleUI Instance;
	[Export] public AbilityIcon Slot1;
	[Export] public AbilityIcon Slot2;
	[Export] public AbilityIcon Slot3;
	[Export] public TopLeftUI topLeftUI;
	[Export] public PackedScene announcement;

	public override void _EnterTree()
	{
		if (Instance == null) { Instance = this; }
		
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Instance = null;
	}

//MoveUI elements out of the screen
	public override void _Ready()
	{
		base._Ready();
		topLeftUI.Position += new Vector2(0,-180);
		Slot1.Position += new Vector2(-150,0);
		Slot2.Position += new Vector2(-150, 0);
		Slot3.Position += new Vector2(-150, 0);
	}

	public void SetMaxHP(int maxHP) {
		topLeftUI.hpBar.SetMaxHP(maxHP);
	}
	public void DisplayAbility(int abilityIndex, CompressedTexture2D image, int numberOfCharges) {
		switch(abilityIndex) {
			case 0:
				Slot1.SetImage(image);
				Slot1.numberOfCharges = numberOfCharges;
				Slot1.UpdateLabel();
				break;
			case 1:
				Slot2.SetImage(image);
				Slot2.numberOfCharges = numberOfCharges;
				Slot2.UpdateLabel();
				break;
			case 2:
				Slot3.SetImage(image);
				Slot3.numberOfCharges = numberOfCharges;
				Slot3.UpdateLabel();
				break;
		}
		
	}

//MoveUI elements back in with animation
	public void GenerateUI() {
		Tween tweenUI = GetTree().CreateTween();
		tweenUI.SetEase(Tween.EaseType.Out);
		tweenUI.SetParallel();
		tweenUI.TweenProperty(topLeftUI,"position",new Vector2(0,0),0.5);
		tweenUI.TweenProperty(Slot1, "position",Slot1.Position + new Vector2(150, 0), 0.5);
		tweenUI.TweenProperty(Slot2, "position",Slot2.Position + new Vector2(150, 0), 0.5).SetDelay(0.1);
		tweenUI.TweenProperty(Slot3, "position",Slot3.Position + new Vector2(150, 0), 0.5).SetDelay(0.2);
		tweenUI.Finished += delegate {GameManager.Instance.UpdateGameState(GameState.SpawnHero);};
	}

	public void BeginCooldown(int abilityIndex,float cooldown) {
		switch (abilityIndex)
		{
			case 0:
				Slot1.BeginCooldown(cooldown);
				break;
			case 1:
				Slot2.BeginCooldown(cooldown);
				break;
			case 2:
				Slot3.BeginCooldown(cooldown);
				break;
		}
		
	}

	public void AnnounceBattle(bool isBattleStart) {
		BattleAnnouncement announce = announcement.Instantiate<BattleAnnouncement>();
		announce.isBattleStart = isBattleStart;
		AddChild(announce);

	}
}
