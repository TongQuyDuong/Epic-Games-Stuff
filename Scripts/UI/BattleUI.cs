using Godot;
using System;

public partial class BattleUI : Control
{
	public static BattleUI Instance;
	[Export] public Godot.Collections.Array<AbilityIcon> icons = new Godot.Collections.Array<AbilityIcon>();
	[Export] public TopLeftUI topLeftUI;
	[Export] public SelectSkillBook selectSkillBook;
	[Export] public PackedScene announcement;
	[Export] public PackedScene PopupPrefab;


	public override void _EnterTree()
	{
		PopupPrefab = GD.Load<PackedScene>("res://Prefabs/Effects/PopupEffect.tscn");
		if (Instance == null) { Instance = this; }

		SelectSkillBook.onConfirmButtonPressed += HideSkillBook;
	}
	public override void _ExitTree()
	{
		Instance = null;
		SelectSkillBook.onConfirmButtonPressed -= HideSkillBook;
	}

//MoveUI elements out of the screen
	public override void _Ready()
	{
		topLeftUI.Position += new Vector2(0,-180);
		foreach(AbilityIcon icon in icons) {
			icon.Position += new Vector2(-150, 0);
		}
	}

	public void SetMaxHP(int maxHP) {
		topLeftUI.hpBar.SetMaxHP(maxHP);
	}

	public void ResetIcon(int iconIndex) {
		icons[iconIndex].ResetIcon();
	}

	public void DisplayAbility(Godot.Collections.Array<AbilityHolder> abilityHolders) {
		for (int i = 0; i < 3; i++) {
			icons[i].SetAbility(abilityHolders[i].ability);
			icons[i].numberOfCharges = abilityHolders[i].numberOfCharges;
			icons[i].UpdateLabel();
		}
	}

//MoveUI elements back in with animation
	public void GenerateUI() {
		Tween tweenUI = GetTree().CreateTween();
		tweenUI.SetEase(Tween.EaseType.Out);
		tweenUI.SetParallel();
		tweenUI.TweenProperty(topLeftUI,"position",new Vector2(0,0),0.5);
		for (int i = 0; i < 3; i++) {
			tweenUI.TweenProperty(icons[i], "position", icons[i].Position + new Vector2(150, 0), 0.5).SetDelay(i*0.1);
		}
		tweenUI.TweenInterval(0.2);
		tweenUI.Finished += delegate {GameManager.Instance.UpdateGameState(GameState.SpawnHero);};
	}

	public void BeginCooldown(int abilityIndex,float cooldown) {
		icons[abilityIndex].BeginCooldown(cooldown);		
	}

	public void AnnounceBattle(bool isBattleStart) {
		BattleAnnouncement announce = announcement.Instantiate<BattleAnnouncement>();
		announce.isBattleStart = isBattleStart;
		AddChild(announce);
	}

	public void ShowSkillBook() {
		Tween tweenBook = GetTree().CreateTween();
		tweenBook.SetEase(Tween.EaseType.Out);
		tweenBook.TweenProperty(selectSkillBook,"position",new Vector2(0,0),0.5f);
		tweenBook.Finished += delegate{selectSkillBook.ProcessMode = ProcessModeEnum.Inherit;};
	}

	public void HideSkillBook(Godot.Collections.Array<AbilityIcon> icons)
	{
		Tween tweenBook = GetTree().CreateTween();
		tweenBook.SetEase(Tween.EaseType.In);
		tweenBook.TweenProperty(selectSkillBook, "position", new Vector2(0, 654), 0.5f);
		tweenBook.Finished += delegate { 
			selectSkillBook.ProcessMode = ProcessModeEnum.Disabled;
			AnnounceBattle(true);
			};
	}
}
