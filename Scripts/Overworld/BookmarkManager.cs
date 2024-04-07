using Godot;
using System;
using MEC;
using System.Collections.Generic;

public partial class BookmarkManager : VBoxContainer
{
	[Export] private Godot.Collections.Array<Bookmark> bookmarks;
	public int selectedIndex = 0;
	public void PopOutAllBookmarks() {
		for (int i = 0; i < bookmarks.Count; i++) {
			Timing.RunCoroutine(waitForSeconds(0.2*i,i));
		}
	}

	IEnumerator<double> waitForSeconds(double duration, int i) {
		yield return Timing.WaitForSeconds(duration);
		bookmarks[i].PopOut();
		if (i == bookmarks.Count - 1) {
			bookmarks[selectedIndex].Select();
		}
	}

	


}
