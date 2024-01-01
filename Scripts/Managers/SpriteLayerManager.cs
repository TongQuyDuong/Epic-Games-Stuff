using Godot;
using System;

public partial class SpriteLayerManager : Node
{
    public static SpriteLayerManager Instance;

    public override void _EnterTree()
    {
        if (Instance == null) { Instance = this; }
    }

    public override void _Ready()
    {
        Events.OnRowChange += AdjustLayerOnMovement;
    }

    public override void _ExitTree()
    {
        Events.OnRowChange -= AdjustLayerOnMovement;
        Instance = null;
    }
    private void AdjustLayerOnMovement(BaseUnit unit)
    {
        unit.ZIndex = (int)unit.currentPos.Y;
    }
    public void AdjustLayerOnInstantiation(Node2D unit, int rowNumber)
    {
        unit.ZIndex = rowNumber;
    }
}