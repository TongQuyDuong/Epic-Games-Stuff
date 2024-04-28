using DialogueManagerRuntime;
using Godot;
using System;


public partial class DialogueSpawner : Node
{
	Resource overworldDialogueResource;
	DialogueType nextDialogue = DialogueType.None;



	[Export] int amount;
	[Export] string itemName;



	public override void _EnterTree() {
		Transition.onTransitionFinished += SpawnDialogue;
	}

	public override void _ExitTree() {
		Transition.onTransitionFinished -= SpawnDialogue;
	}

	public override void _Ready() {
		overworldDialogueResource = GD.Load<Resource>("res://Dialogue/OverworldDialogue.dialogue");
	}

	public void SetItemDropDialogue(string itemName,int amount) {
		nextDialogue = DialogueType.ItemDrop;
		this.amount = amount;
		this.itemName = itemName;
	}

	public void SpawnDialogue() {
		if (nextDialogue == DialogueType.None) return;

		switch (nextDialogue) {
			case DialogueType.ItemDrop:
				DialogueManager.ShowDialogueBalloon(overworldDialogueResource,"item_drop");
				break;
		}

		nextDialogue = DialogueType.None;
	}


}

public enum DialogueType {
	None,
	ItemDrop
}
