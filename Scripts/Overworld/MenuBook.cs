using Godot;
using System;
using MEC;
using System.Collections.Generic;

public partial class MenuBook : CanvasLayer
{
	public static Action<int, bool> onMovementInputReceived;
	public static Action<int> onSelectInputReceived;
	public static Action<int> onBackInputReceived;
	public static bool isActive = false;
	private int currentUiLayer = 0;
	private int currentBookmarkIndex = 0;

	[Export] private AnimationPlayer animationPlayer;
	[Export] private BookmarkManager bookmarkManager;
	[Export] private MenuLayoutManager layoutManager;

	public override void _EnterTree() {
		bookmarkManager.onBookmarkChanged += FlipPage;
	}

	public override void _ExitTree() {
		bookmarkManager.onBookmarkChanged -= FlipPage;
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
				onSelectInputReceived(currentUiLayer);
			}
			else
			{
				CloseMenu();
			}
		}
	}


	public void SetInitialLayout() {
		layoutManager.AddLayout(bookmarkManager.bookmarks[0].menuLayout);
	}

	private void CloseMenu() {
		layoutManager.RemoveLayout();
		animationPlayer.Play("CloseMenu");
	}

	private void FlipPage(int bookmarkIndex, PackedScene menuLayout) {
		if (bookmarkIndex == currentBookmarkIndex) return;
		currentBookmarkIndex = bookmarkIndex;

		layoutManager.RemoveLayout();
		animationPlayer.Play("FlipPage");

		Timing.RunCoroutine(waitforSecondsAndSetlayout(animationPlayer.CurrentAnimationLength,menuLayout));
		
	}

	IEnumerator<double> waitforSecondsAndSetlayout(double duration, PackedScene layout) {
		yield return Timing.WaitForSeconds(duration);
		layoutManager.AddLayout(layout);
	}

	public void ResumeGameAfterClosing() {
		OverworldLevel.isActive = true;
		this.QueueFree();
	}
}
