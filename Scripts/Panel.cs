using Godot;
using System;

public partial class Panel : Node2D
{
	public bool _isWalkable = true;
	[Export] public Vector2 Pos;
	[Export] public BaseUnit occupiedUnit = null;
	[Export] public bool Walkable;

	public override void _Ready()
	{
		UpdateWalkability();
	}

	private void UpdateWalkability()
	{
		Walkable = _isWalkable && occupiedUnit==null;
	}

	public void SetUnit(BaseUnit unit)
	{
		if (unit.occupiedPanel != null)
		{
			unit.occupiedPanel.occupiedUnit = null;
			unit.occupiedPanel.UpdateWalkability();
		}
		unit.Position = Position;
		occupiedUnit = unit;
		unit.occupiedPanel = this;
		unit.currentPos = this.Pos;
		UpdateWalkability();
	}

	public void ResetPanel()
	{
		occupiedUnit = null;
		UpdateWalkability();
	}
}
