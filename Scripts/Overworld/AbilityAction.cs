using Godot;
using System;

public partial class AbilityAction : PanelContainer
{
	const int UI_LAYER_NUMBER = 2;
	[Export] Godot.Collections.Array<MenuChoice> actionChoices;
	public static Action<string> onAbilityActionSelected;
	int numberOfActions;
	int selectedIndex = 0;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocus;
		MenuBook.onBackInputReceived += LoseFocus;
		SkillListManager.onAbilitySelected += PopOut;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocus;
		MenuBook.onBackInputReceived -= LoseFocus;
		SkillListManager.onAbilitySelected -= PopOut;
	}

	public override void _Ready()
	{
		this.Visible = false;
	}

	public void PopOut()
	{
		numberOfActions = 2;
		actionChoices[0].SetContent("Equip");
		actionChoices[1].SetContent("Unequip");
		actionChoices[2].Visible = false;
		this.Visible = true;
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if (this.Visible == false || numberOfActions <= 1) return;
		if (UiLayer == UI_LAYER_NUMBER)
		{
			actionChoices[selectedIndex].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = numberOfActions - 1;
			}
			else if (selectedIndex >= numberOfActions)
			{
				selectedIndex = 0;
			}
			actionChoices[selectedIndex].ToggleSelect();
		}
	}

	private void LoseFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			this.Visible = false;
			actionChoices[selectedIndex].ToggleSelect();
		}
	}

	private void GainFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			selectedIndex = 0;
			actionChoices[selectedIndex].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
		}

		if (uiLayer == (UI_LAYER_NUMBER + 1))
		{
			onAbilityActionSelected?.Invoke(actionChoices[selectedIndex].GetText());
			actionChoices[selectedIndex].ToggleSelect();
			this.Visible = false;
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER - 1);
		}
	}
}
