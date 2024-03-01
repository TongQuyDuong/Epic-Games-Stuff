using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public partial class SelectSkillBook : Control
{
	[Export] Marker2D firstSlotPosition;
	[Export] float xOffset;
	[Export] float yOffset;
	[Export] public AddButton addButton;
	private Dictionary<Vector2,SelectSkillButton> buttons = new Dictionary<Vector2, SelectSkillButton>();
	[Export] public Godot.Collections.Array<Ability> abilities = new Godot.Collections.Array<Ability>();
	[Export] public Godot.Collections.Array<SelectSkillButton> fixedButtons = new Godot.Collections.Array<SelectSkillButton>();
	[Export] public Godot.Collections.Array<AbilityHolder> abilityHolders = new Godot.Collections.Array<AbilityHolder>();
	[Export] PackedScene smallAbilityIcon;
	[Export] Vector2 selectedPos;

	private bool isOnAddButton = true;


	public override void _EnterTree()
	{
		GenerateAbilityIcons();
		selectedPos = new Vector2(0,0);
		foreach(SelectSkillButton button in fixedButtons) {
			buttons[button.buttonPos] = button;
		}
		SelectSkillButton emptyButton = new SelectSkillButton();
		emptyButton.isActive = false;
		emptyButton.buttonPos = new Vector2(3, 3);
		buttons[emptyButton.buttonPos] = emptyButton;
	}

	public override void _Ready()
	{
		buttons[selectedPos].ToggleSelectOn();
		UpdateAbilityIcons();
	}

	private void GenerateAbilityIcons() {
		for (int i = 0; i < 4; i++) {
			for(int j = 0; j < 3; j++) {
				SmallAbilityIcon smallButton = smallAbilityIcon.Instantiate<SmallAbilityIcon>();
				smallButton.Visible = false;
				smallButton.buttonPos = new Vector2(i,j);
				smallButton.Position = firstSlotPosition.Position + new Vector2(i*xOffset,j*yOffset);
				buttons[smallButton.buttonPos] = smallButton;
				this.AddChild(smallButton);
			}
		}
	}

	private void UpdateAbilityIcons() {
		for(int i = 0; i < abilities.Count; i++) {
			if (i == 12) break;
			((SmallAbilityIcon)buttons[new Vector2 (i%4,i/4)]).UpdateAbility(abilities[i]);
			
		}
	}

	public override void _Process(double delta)
	{
		
		ProcessInput();
	}

	private void ProcessInput() {
		if(Input.IsActionJustPressed("Select")){
			buttons[selectedPos].SelectButton();
		} else if (Input.IsActionJustPressed("Back")) {
			if(isOnAddButton) {
				DeSelectAddButton();
			} else {
				this.Visible = !this.Visible;
			}
		}

		if (isOnAddButton == true) return;

		if (Input.IsActionJustPressed("MoveUp"))
		{
			Vector2 nextPos = selectedPos + new Vector2(0, -1);
			if (nextPos.Y < 0)
			{
				MoveToPosition(new Vector2(0, 3));
			}
			else
			{
				MoveTo(new Vector2(0, -1));
			}
			
		}
		else if (Input.IsActionJustPressed("MoveDown"))
		{
			Vector2 nextPos = selectedPos + new Vector2(0, 1);
			if (nextPos.Y > 3)
			{
				MoveToPosition(new Vector2(0, 0));
			}
			else if(buttons[nextPos].isActive == false)
			{
				MoveToPosition(new Vector2(0, 3));
			} else {
				MoveTo(new Vector2(0, 1));
			}
		}
		else if (Input.IsActionJustPressed("MoveLeft"))
		{
			Vector2 nextPos = selectedPos + new Vector2(-1, 0);
			if (nextPos.X < 0)
			{
				MoveToPosition(FindFurthestRightPos(selectedPos.Y));
			}
			else
			{
				MoveTo(new Vector2(-1,0));
			}
		}
		else if (Input.IsActionJustPressed("MoveRight"))
		{
			Vector2 nextPos = selectedPos + new Vector2(1, 0);
			if (nextPos.X > 3 || buttons[nextPos].isActive == false)
			{
				MoveToPosition(new Vector2(0,selectedPos.Y));
			}
			else
			{
				MoveTo(new Vector2(1,0));
			}
		}
	}

	private void MoveTo(Vector2 direction) {
		int loops = 0;
		while(loops < 4) {
			loops++;
			Vector2 nextPos = selectedPos + direction*loops;
			if(buttons[nextPos].isActive && buttons[nextPos] != null) {
				buttons[selectedPos].ToggleSelectOff();
				selectedPos += direction*loops;
				buttons[selectedPos].ToggleSelectOn();
				break;
			}
		}
	}

	private void MoveToPosition(Vector2 Pos) {
		buttons[selectedPos].ToggleSelectOff();
		selectedPos = Pos;
		buttons[selectedPos].ToggleSelectOn();
	}

	private Vector2 FindFurthestRightPos(float rowNumber) {
		for(int i = 0; i < 4; i++) {
			Vector2 nextPos = new Vector2(i+1,rowNumber);
			if(nextPos.X > 3 || buttons[nextPos].isActive == false) {
				return new Vector2(i,rowNumber);
			}
		}
		return new Vector2(0,0);
	}

	private void SelectAddButton() {
		buttons[selectedPos].ToggleSelectOff();
		isOnAddButton = true;
		addButton.ToggleSelectOn();
	}

	private void DeSelectAddButton()
	{
		buttons[selectedPos].ToggleSelectOn();
		isOnAddButton = false;
		addButton.ToggleSelectOff();
	}


}
