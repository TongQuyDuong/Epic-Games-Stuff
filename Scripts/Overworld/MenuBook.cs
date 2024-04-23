using Godot;
using System;
using MEC;
using System.Collections.Generic;
using System.Diagnostics;

public partial class MenuBook : CanvasLayer
{
	public static Action<int, bool> onMovementInputReceived;
	public static Action<int> onSelectInputReceived;
	public static Action<int> onBackInputReceived;
	public static Action<int> onRequestUIFocus;

	public PlayerOverworld player;
	public static bool isActive = false;
	[Export] private int currentUiLayer = 0;
	private int currentBookmarkIndex = 0;

	[Export] private AnimationPlayer animationPlayer;
	[Export] private BookmarkManager bookmarkManager;
	[Export] private MenuLayoutManager layoutManager;

	public override void _EnterTree() {
		bookmarkManager.onBookmarkChanged += FlipPage;
		onRequestUIFocus += GrantUIFocus;
	}

	public override void _ExitTree() {
		bookmarkManager.onBookmarkChanged -= FlipPage;
		onRequestUIFocus -= GrantUIFocus;
	}

	public override void _Ready() {
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (isActive == false) return;

		if (Input.IsActionJustPressed("MoveUp"))
		{
			onMovementInputReceived?.Invoke(currentUiLayer, true);
		}
		if (Input.IsActionJustPressed("MoveDown"))
		{
			onMovementInputReceived?.Invoke(currentUiLayer, false);
		}

		if (Input.IsActionJustPressed("Select"))
		{
			onSelectInputReceived?.Invoke(currentUiLayer + 1);
		}

		if (Input.IsActionJustPressed("Back"))
		{
			if (currentUiLayer > 0)
			{
				onBackInputReceived?.Invoke(currentUiLayer);
				currentUiLayer -= 1;
			}
			else
			{
				CloseMenu();
			}
		}
	}


	public void SetInitialLayout() {
		layoutManager.AddLayout(bookmarkManager.bookmarks[0].menuLayout,player);
	}

	private void CloseMenu() {
		layoutManager.RemoveLayout();
		animationPlayer.Play("CloseMenu");
	}

	private void FlipPage(int bookmarkIndex, PackedScene menuLayout) {
		if (bookmarkIndex == currentBookmarkIndex) {
			onSelectInputReceived?.Invoke(10);
			return;
		}
		currentBookmarkIndex = bookmarkIndex;

		layoutManager.RemoveLayout();
		animationPlayer.Play("FlipPage");

		Timing.RunCoroutine(waitforSecondsAndSetlayout(animationPlayer.CurrentAnimationLength,menuLayout));
		
	}

	IEnumerator<double> waitforSecondsAndSetlayout(double duration, PackedScene layout) {
		yield return Timing.WaitForSeconds(duration);
		layoutManager.AddLayout(layout,player);
	}

	public void ResumeGameAfterClosing() {
		OverworldLevel.isActive = true;
		this.QueueFree();
	}

	private void GrantUIFocus(int uiLayer) {
		currentUiLayer = uiLayer;
		Debug.Print("Grant focus to layer" + uiLayer);
	}
}
