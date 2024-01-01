using Godot;
using System;
using Godot.Collections;

public partial class GridManager : Node
{
	private int _width = 8, _height = 3;
	
	[Export] public float offsetX, offsetY, camX, camY;
	private Dictionary<Vector2, Panel> _panels;
	public static GridManager Instance;

	public override void _EnterTree()
	{
		if(Instance == null) Instance = this;
	}

	public void GenerateGrid(){
		_panels = new Dictionary<Vector2, Panel>();
		var _panelPrefab = GD.Load<PackedScene>("res://Prefabs/Panels/panel.tscn");
		for (float x = 230, i = 0; i < _width; i++)
		{
			for (float y = 450, j = 0; j < _height; j++)
			{
				Panel spawnedPanel = _panelPrefab.Instantiate<Node2D>() as Panel;
				spawnedPanel.Position = new Vector2(x, y);
				AddChild(spawnedPanel);
				y += offsetY;
				_panels[new Vector2(i, j)] = spawnedPanel;
				spawnedPanel.Pos = new Vector2(i, j);
			}
			x += offsetX;
		}
		 
		GameManager.Instance.UpdateGameState(GameState.SpawnHero);
		

	}

	public Panel GetPanelAtPosition(Vector2 pos)
	{
		if (_panels.TryGetValue(pos, out var panel))
		{
			return panel;
		}
		return null;
	}

	public Panel GetHeroSpawnPanel()
	{
		Panel spawnPanel = _panels[new Vector2(1, 1)];
		return spawnPanel;

	}

	public Panel NextPanelHorizontal(Vector2 currentPos, float direction)
	{
		Vector2 Next = currentPos + new Vector2(direction, 0);
		if (Next.X is >= 0 and <= 7 && Next.Y is >= 0 and <= 2) { return _panels[Next]; }
		else { return null; }

	}

	public Panel NextPanelVertical(Vector2 currentPos, float direction)
	{
		Vector2 Next = currentPos + new Vector2(0, direction);
		if (Next.X is >= 0 and <= 7 && Next.Y is >= 0 and <= 2) { return _panels[Next]; }
		else { return null; }
	}
}
