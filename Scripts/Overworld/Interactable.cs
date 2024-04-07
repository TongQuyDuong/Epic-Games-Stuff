using Godot;
using System;
using DialogueManagerRuntime;

public partial class Interactable : Area2D
{
	[Export] public Resource dialougeResource;
	[Export] public String dialougeStartKey = "start";
	public static Action onDialogueStart;
	public static Action onDialogueEnd;

	public void Action()
	{
		onDialogueStart?.Invoke();
		GetTree().Paused = true;
		DialogueManager.ShowDialogueBalloon(dialougeResource, dialougeStartKey);
		DialogueManager.DialogueEnded += delegate {
			onDialogueEnd?.Invoke();
		};
	}
}
