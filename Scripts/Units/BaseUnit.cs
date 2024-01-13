using Godot;
using System;

public partial class BaseUnit : CharacterBody2D
{
	public Panel occupiedPanel;
	[Export] public Vector2 currentPos;
	[Export] public UnitStatList stats;
	[Export] public PackedScene PopupPrefab;
	[Export] public Marker2D PopupPoint;
	[Export] public bool isFacingRight;

	public override void _EnterTree()
	{
		PopupPrefab = GD.Load<PackedScene>("res://Prefabs/Effects/PopupEffect.tscn");
		this.ProcessMode = ProcessModeEnum.Disabled;
		Events.OnBattleActive += Enable;
		Events.OnBattleEnd += Disable;
	}
	public override void _ExitTree()
	{
		Events.OnBattleActive -= Enable;
		Events.OnBattleEnd -= Disable;
	}
	private void Enable()
	{
		this.ProcessMode = ProcessModeEnum.Inherit;
	}
	private void Disable()
	{
		this.ProcessMode = ProcessModeEnum.Disabled;
	}
	public void ShowPopup(string content) 
	{
		var popup = PopupPrefab.Instantiate<Marker2D>() as PopupEffect;
		popup.Position = PopupPoint.GlobalPosition;
		popup.SetLabelContent(content);
		GetTree().CreateTween().TweenProperty(popup,"position", popup.Position + new Vector2(0,-30), 0.55f);

		GetTree().CurrentScene.AddChild(popup);
	}
}