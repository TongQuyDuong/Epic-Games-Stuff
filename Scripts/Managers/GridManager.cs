using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;

public partial class GridManager : Node
{
	private int _width = 8, _height = 3;
	
	[Export] public float offsetX, offsetY, camX, camY;
	private Dictionary<Vector2I, Panel> _panels;
	public static GridManager Instance;
	public AStarGrid2D aStarGrid2D = new AStarGrid2D();

	public override void _EnterTree()
	{
		if(Instance == null) Instance = this;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		Instance = null;
	}

	public void GenerateGrid(){
		GenerateAstar();
		_panels = new Dictionary<Vector2I, Panel>();
		var _panelPrefab = GD.Load<PackedScene>("res://Prefabs/Panels/panel.tscn");
		float x = 230;
		for (int i = 0; i < _width; i++)
		{
			float y = 450;
			for (int j = 0; j < _height; j++)
			{
				Panel spawnedPanel = _panelPrefab.Instantiate<Node2D>() as Panel;
				spawnedPanel.Position = new Vector2(x, y);
				y += offsetY;
				_panels[new Vector2I(i, j)] = spawnedPanel;
				spawnedPanel.Pos = new Vector2I(i, j);
				spawnedPanel.Scale = new Vector2I(1,1);
				AddChild(spawnedPanel);
				Tween tweenPanel = GetTree().CreateTween();
				tweenPanel.TweenProperty(spawnedPanel,"scale",new Vector2(30,30),0.5).SetDelay(i*0.1);
				if(i == (_width-1) && j == (_height-1)) tweenPanel.Finished += delegate {GameManager.Instance.UpdateGameState(GameState.GenerateUI);};
			}
			x += offsetX;
		}
	}

	private void GenerateAstar() {
		aStarGrid2D.Region = new Rect2I(Vector2I.Zero, new Vector2I(8,3));
		aStarGrid2D.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		aStarGrid2D.Update();
	}

	public Panel GetPanelAtPosition(Vector2I pos)
	{
		if (_panels.TryGetValue(pos, out var panel))
		{
			return panel;
		}
		return null;
	}

	public Panel GetHeroSpawnPanel()
	{
		Panel spawnPanel = _panels[new Vector2I(1, 1)];
		return spawnPanel;

	}

	public Panel NextPanelHorizontal(Vector2I currentPos, float direction)
	{
		Vector2I Next = currentPos + new Vector2I((int)direction, 0);
		if (Next.X is >= 0 and <= 7 && Next.Y is >= 0 and <= 2) { return _panels[Next]; }
		else { return null; }

	}

	public Panel NextPanelVertical(Vector2I currentPos, float direction)
	{
		Vector2I Next = currentPos + new Vector2I(0, (int)direction);
		if (Next.X is >= 0 and <= 7 && Next.Y is >= 0 and <= 2) { return _panels[Next]; }
		else { return null; }
	}
}
