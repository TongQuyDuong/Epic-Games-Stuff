using Godot;
using System;

public partial class SmiteBehaviour : Node2D
{
	public Vector2 currentPos;
    public override void _Ready()
    {
		SpriteLayerManager.Instance.AdjustLayerOnInstantiation(this,(int)currentPos.Y);
    }
}
