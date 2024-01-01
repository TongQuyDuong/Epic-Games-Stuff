using Godot;
using System;

public partial class BattleAnnouncement : Control
{
	[Export] public TextureRect board;
	[Export] public Label content;
	[Export] public bool isBattleStart = true;
	public override void _Ready()
	{
		if(isBattleStart == false)
		{ 
			content.Text = "Battle\r\nOver!"; 
			Events.OnBattleEnd?.Invoke();
		} 

		TweenIn();
	}

	public void TweenIn() {
		Tween TweenIn = GetTree().CreateTween();
		TweenIn.SetTrans(Tween.TransitionType.Bounce);
		TweenIn.SetEase(Tween.EaseType.Out);
		TweenIn.TweenProperty(board,"position",board.Position,0.5).From(board.Position + new Vector2(0,-200));
		TweenIn.Finished += TweenOut;
	}

	public void TweenOut() {
		Tween TweenOut = GetTree().CreateTween();
		TweenOut.SetTrans(Tween.TransitionType.Elastic);
		TweenOut.SetEase(Tween.EaseType.In);
		TweenOut.TweenInterval(1);
		TweenOut.TweenProperty(board, "position", board.Position + new Vector2(0,320), 0.75).FromCurrent();
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
