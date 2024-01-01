using Godot;
using System;

public partial class PopupEffect : Marker2D
{
	[Export] public Label label;
	[Export] public AnimationPlayer anim;

	public void SetLabelContent(string content) 
	{
		label.Text = content;
		anim.Play("Popup");
		
	}
	
}
