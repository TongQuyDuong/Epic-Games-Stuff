using Godot;
using System;

public partial class AbilityIcon : Control
{
	
	public int numberOfCharges;
	[Export] public TextureProgressBar progBar;
	[Export] public Label label;

    public override void _EnterTree()
    {
		base._EnterTree();
		progBar = GetNode<TextureProgressBar>("TextureRect/TextureProgressBar");
		label = GetNode<Label>("NumberOfCharges");
		label.Visible = false;
    }

    public void SetImage(CompressedTexture2D image) {
		progBar.TextureUnder = image;
		progBar.TextureProgress = image;
	}
	
    public void UpdateLabel() {
		label.Visible = numberOfCharges <= 0 ? false : true;
		label.Text = numberOfCharges.ToString();
	}

	public void BeginCooldown(float cooldown){
		numberOfCharges -= 1;
		UpdateLabel();
		progBar.Value = 100;
		GetTree().CreateTween().TweenProperty(progBar, "value", 0, cooldown);
	}
}
