using Godot;
using System;
using DialogueManagerRuntime;

public partial class Interactable : Area2D
{
	[Export] public Resource dialougeResource;
	[Export] public String dialougeStartKey = "start";

	public void Action()
	{
		GetTree().Paused = true;
		DialogueManager.ShowDialogueBalloon(dialougeResource, dialougeStartKey);
		DialogueManager.DialogueEnded += delegate {
			GetTree().Paused = false;
		};
	}
}
