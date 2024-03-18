using Godot;
using System;

public partial class BattleAnnouncement : Control
{
	[Export] public TextureRect board;
	[Export] public Label content;
	[Export] public bool isBattleStart = true;
	public override void _Ready()
	{
		GameManager.Instance.CallDeferred("PauseBattle");
		if(isBattleStart == false)
		{ 
			content.Text = "Battle\r\nOver!"; 
		} 
		board.Position += new Vector2(0,-200);
		TweenIn();
	}

	public void TweenIn() {
		Tween TweenIn = GetTree().CreateTween();
		TweenIn.SetTrans(Tween.TransitionType.Back);
		TweenIn.SetEase(Tween.EaseType.InOut);
		TweenIn.TweenProperty(board,"position",board.Position + new Vector2(0,200),0.5);
		TweenIn.Finished += TweenOut;
	}

	public void TweenOut() {
		Tween TweenOut = GetTree().CreateTween();
		TweenOut.SetTrans(Tween.TransitionType.Back);
		TweenOut.SetEase(Tween.EaseType.In);
		TweenOut.TweenInterval(1);
		TweenOut.TweenProperty(board, "position", board.Position + new Vector2(0,320), 0.5).FromCurrent();
		TweenOut.Finished += delegate 
		{ 
		if(isBattleStart) {
			Events.OnBattleActive?.Invoke();
		} else {
			GameManager.Instance.ExitBattle();
		}
		
		this.QueueFree(); 
		};
	}
	
}
