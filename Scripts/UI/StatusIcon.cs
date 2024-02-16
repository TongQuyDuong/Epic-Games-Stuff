using Godot;
using System;

public partial class StatusIcon : Sprite2D
{
	public CompressedTexture2D icon;
	[Export] private Label label;
	public int numberOfStacks;


    public override void _Ready()
    {
        base._Ready();
		this.Texture = icon;
		if(numberOfStacks > 1) {
			label.Text = numberOfStacks.ToString();
		} else {
			label.Visible = false;
		}

    }

	public void AdjustLable(int amount) {
		numberOfStacks += amount;
		if (numberOfStacks > 1)
		{
			label.Text = numberOfStacks.ToString();
		}
		else
		{
			label.Visible = false;
		}
	}
}
