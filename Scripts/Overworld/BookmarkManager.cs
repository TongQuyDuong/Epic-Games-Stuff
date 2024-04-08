using Godot;
using System;
using MEC;
using System.Collections.Generic;
using System.Diagnostics;

public partial class BookmarkManager : VBoxContainer
{
	[Export] public Godot.Collections.Array<Bookmark> bookmarks;
	public Action<int, PackedScene> onBookmarkChanged;
	public int selectedIndex = 0;

	public override void _EnterTree() {
		MenuBook.onMovementInputReceived += ChangeSelectedBookmark;
		MenuBook.onSelectInputReceived += ApplyBookmark;
	}

	public override void _ExitTree() {
		MenuBook.onMovementInputReceived -= ChangeSelectedBookmark;
		MenuBook.onSelectInputReceived -= ApplyBookmark;
	}

	public void PopOutAllBookmarks() {
		for (int i = 0; i < bookmarks.Count; i++) {
			Debug.Print("Before Coroutine");
			Timing.RunCoroutine(waitForSecondsAndPopOut(0.2*i,i));
		}
	}

	IEnumerator<double> waitForSecondsAndPopOut(double duration, int i) {
		yield return Timing.WaitForSeconds(duration);
		bookmarks[i].PopOut();
		if (i == bookmarks.Count - 1) {
			bookmarks[selectedIndex].Select();
			MenuBook.isActive = true;
		}
	}

	public void PopInAllBookmarks() {
		foreach (Bookmark bookmark in bookmarks) {
			bookmark.PopIn();
		}
	}

	private void ChangeSelectedBookmark(int UiLayer, bool isMovingUp) {
		if(UiLayer == 0) {
			bookmarks[selectedIndex].Deselect();
			selectedIndex += isMovingUp? -1 : 1;
			if(selectedIndex < 0) {
				selectedIndex = bookmarks.Count - 1;
			} else if (selectedIndex >= bookmarks.Count) {
				selectedIndex = 0;
			}
			bookmarks[selectedIndex].Select();
		}
	}

	private void ApplyBookmark(int currentUiLayer) {
		if(currentUiLayer == 1) {
			onBookmarkChanged?.Invoke(selectedIndex, bookmarks[selectedIndex].menuLayout);
		}
	}

	


}
