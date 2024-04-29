using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class SkillListManager : VBoxContainer
{
	const int UI_LAYER_NUMBER = 1;
	const int OFFSET_BOUNDARY = 3;
	public Action<Ability> onSelectedAbilityChanged;
	public static Action onAbilitySelected;
	[Export] Godot.Collections.Array<MenuChoice> AbilityChoices = new Godot.Collections.Array<MenuChoice>();
	List<AbilityData> abilityList = new List<AbilityData>();
	public PlayerData playerData;
	[Export] private int selectedIndex = 0;
	[Export] private int currentOffset = 0;
	[Export] private int maxOffset;

	public override void _EnterTree()
	{
		MenuBook.onMovementInputReceived += ChangeSelectedChoice;
		MenuBook.onSelectInputReceived += GainFocus;
		MenuBook.onBackInputReceived += LoseFocus;
		AbilityAction.onAbilityActionSelected += PerformActionOnAbility;
	}

	public override void _ExitTree()
	{
		MenuBook.onMovementInputReceived -= ChangeSelectedChoice;
		MenuBook.onSelectInputReceived -= GainFocus;
		MenuBook.onBackInputReceived -= LoseFocus;
		AbilityAction.onAbilityActionSelected -= PerformActionOnAbility;
	}

	public override void _Ready()
	{
		foreach (Node Ability in GetChildren())
		{
			AbilityChoices.Add((MenuChoice)Ability);
		}

		MenuBook.onRequestUIFocus.Invoke(UI_LAYER_NUMBER);
		AbilityChoices[0].ToggleSelect();

	}

	public void BuildAbilityList()
	{
		foreach (KeyValuePair<Ability, bool> skill in playerData.skills)
		{
			abilityList.Add(new AbilityData(skill.Key, skill.Value));
		}

		maxOffset = abilityList.Count >= AbilityChoices.Count ? abilityList.Count - AbilityChoices.Count : 0;

		DisplayOffset(0);

		onSelectedAbilityChanged?.Invoke(abilityList[selectedIndex].ability);
	}

	private void DisplayOffset(int offset)
	{
		for (int i = 0; i < AbilityChoices.Count; i++)
		{
			if (i >= abilityList.Count)
			{
				AbilityChoices[i].Visible = false;
				continue;
			}
			AbilityChoices[i].SetContent(abilityList[i + offset]);

			AbilityChoices[i].SetSize();
		}
	}

	private void ChangeSelectedChoice(int UiLayer, bool isMovingUp)
	{
		if (playerData.skills.Count <= 1) return;
		if (UiLayer == UI_LAYER_NUMBER)
		{
			AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
			selectedIndex += isMovingUp ? -1 : 1;
			if (selectedIndex < 0)
			{
				selectedIndex = playerData.skills.Count - 1;
				currentOffset = maxOffset;
				DisplayOffset(maxOffset);
				AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
			}
			else if ((selectedIndex - currentOffset) < OFFSET_BOUNDARY && currentOffset > 0)
			{
				currentOffset--;
				AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
				DisplayOffset(currentOffset);
			}
			else if ((selectedIndex - currentOffset) > (AbilityChoices.Count - OFFSET_BOUNDARY) && currentOffset < maxOffset)
			{
				currentOffset++;
				AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
				DisplayOffset(currentOffset);
			}
			else if (selectedIndex >= playerData.skills.Count)
			{
				selectedIndex = 0;
				currentOffset = 0;
				DisplayOffset(0);
				AbilityChoices[0].ToggleSelect();
			}
			else
			{
				AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
			}

			onSelectedAbilityChanged?.Invoke(abilityList[selectedIndex].ability);
		}
	}

	private void LoseFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
			onSelectedAbilityChanged?.Invoke(null);
			Debug.Print(UI_LAYER_NUMBER + "Lose Focus");
		}
	}

	private void GainFocus(int uiLayer)
	{
		if (uiLayer == UI_LAYER_NUMBER)
		{
			if (abilityList.Count == 0) return;
			AbilityChoices[selectedIndex - currentOffset].ToggleSelect();
			MenuBook.onRequestUIFocus?.Invoke(UI_LAYER_NUMBER);
			onSelectedAbilityChanged?.Invoke(abilityList[selectedIndex].ability);
		}

		if (uiLayer == (UI_LAYER_NUMBER + 1))
		{
			onAbilitySelected?.Invoke();
		}
	}

	private void PerformActionOnAbility(string action)
	{
		Ability currentAbility = abilityList[selectedIndex].ability;
		switch (action)
		{
			case "Equip":
				if (playerData.skills[currentAbility] == true) break;
				playerData.EquipAbility(currentAbility);
				abilityList[selectedIndex].isEquipped = true;
				AbilityChoices[selectedIndex - currentOffset].SetContent(abilityList[selectedIndex]);
				break;
			case "Unequip":
				if (playerData.skills[currentAbility] == false) break;
				playerData.UnequipAbility(currentAbility);
				abilityList[selectedIndex].isEquipped = false;
				AbilityChoices[selectedIndex - currentOffset].SetContent(abilityList[selectedIndex]);
				break;
		}
	}
}

public class AbilityData
{
	public Ability ability;
	public bool isEquipped;

	public AbilityData(Ability ability, bool isEquipped)
	{
		this.ability = ability;
		this.isEquipped = isEquipped;
	}
}