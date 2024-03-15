using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class BaseUnit : CharacterBody2D
{
	public Panel occupiedPanel;
	[Export] public Vector2I currentPos;
	[Export] public UnitStatList stats;
	[Export] public StatusEffectsController STeffectCon;
	[Export] public AnimationPlayer animPlayer;
	[Export] public Marker2D PopupPoint;
	[Export] public Marker2D CenterPoint;
	[Export] public bool isFacingRight;
	[Export] public bool isStatusImmune;
	public bool isControlled;

	public override void _EnterTree()
	{
		isControlled = true;
		Events.OnBattleActive += Enable;
		Events.OnBattleEnd += Disable;
		if(isFacingRight == false) this.Scale = new Vector2(this.Scale.X * -1,this.Scale.Y);
	}
	public override void _ExitTree()
	{
		Events.OnBattleActive -= Enable;
		Events.OnBattleEnd -= Disable;
	}
	public void Enable()
	{
		this.ProcessMode = ProcessModeEnum.Inherit;
		this.isControlled = false;
	}
	public void Disable()
	{
		this.ProcessMode = ProcessModeEnum.Disabled;
		this.isControlled = true;
	}
	public void ShowPopup(string content) 
	{
		var popup = BattleUI.Instance.PopupPrefab.Instantiate<Marker2D>() as PopupEffect;
		popup.Position = PopupPoint.GlobalPosition;
		popup.SetLabelContent(content);
		GetTree().CreateTween().TweenProperty(popup,"position", popup.Position + new Vector2(0,-30), 0.55f);

		GetTree().CurrentScene.AddChild(popup);
	}

    public void ShowControlEffect(Node2D effect) {
		effect.Position = PopupPoint.Position;
		AddChild(effect);
		animPlayer.ProcessMode = ProcessModeEnum.Disabled;
	}

	public void RemoveControlEffect(Node2D effect) {
		effect.QueueFree();
		animPlayer.ProcessMode = ProcessModeEnum.Inherit;
		animPlayer.Play("RESET");
	}


}
