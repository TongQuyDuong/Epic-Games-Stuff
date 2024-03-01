using Godot;
using System;

public partial class AddButton : SelectSkillButton
{
	[Export] public Label numberDisplay;
	[Export] public int numberOfCharges = 0;

	public override void _Ready()
	{
		animPlayer.Play("RESET");
	}

	public override void _Process(double delta)
	{
		if (isSelected) {
			ProcessInput();
		}
	}

	private void ProcessInput() {
		if (Input.IsActionJustPressed("MoveLeft")) {
			if(numberOfCharges > 0) {
				numberOfCharges--;
				numberDisplay.Text = numberOfCharges.ToString();
			}
		} else if (Input.IsActionJustPressed("MoveRight"))
		{
			if (numberOfCharges < 99)
			{
				numberOfCharges++;
				numberDisplay.Text = numberOfCharges.ToString();
			}

		}
	}

    public override void ToggleSelectOn()
    {
		isSelected = true;
		animPlayer.Play("Selected");
	}

	public override void ToggleSelectOff()
	{
		isSelected = false;
		animPlayer.Play("RESET");
	}
}
