using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Principal;

public partial class SelectSkillBook : Control
{
	[Export] Marker2D firstSlotPosition;
	float xOffset = 72.3f;
	float yOffset = 72.375f;
	[Export] private AddButton addButton;
	[Export] private TextureRect displayIcon;
	[Export] private Label displayName;
	[Export] private Label displayDescription;
	[Export] private Label SPcost;
	[Export] private Label SoulPowerAmount;
	private Dictionary<Vector2,SelectSkillButton> buttons = new Dictionary<Vector2, SelectSkillButton>();
	[Export] public Godot.Collections.Array<Ability> abilities = new Godot.Collections.Array<Ability>();
	[Export] public Godot.Collections.Array<SelectSkillButton> fixedButtons = new Godot.Collections.Array<SelectSkillButton>();
	[Export] public Godot.Collections.Array<AbilityIcon> abilityIcons = new Godot.Collections.Array<AbilityIcon>();
	[Export] PackedScene smallAbilityIcon;
	Vector2 selectedPos;
	private int currentSlotIndex = 0;
	private bool isOnAddButton = false;
	public static Action<Godot.Collections.Array<AbilityIcon>> onConfirmButtonPressed;
	public int currentSoulPower = 0;


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

		SmallAbilityIcon.onAbilityPress += SelectAbility;
		AddButton.onAmountChanged += AdjustSoulPower;
	}

	public override void _Ready()
	{
		Initialize();
	}

	public void Initialize() {
		UpdateAbilityIcons();
		SmallAbilityIcon nextButton = (SmallAbilityIcon)buttons[selectedPos];
		nextButton.ToggleSelectOn();
		UpdateDisplay(nextButton.ability);
		this.ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _ExitTree()
	{
		SmallAbilityIcon.onAbilityPress -= SelectAbility;
		AddButton.onAmountChanged -= AdjustSoulPower;
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

	public bool UpdateSoulPower(int amount) {
		if(currentSoulPower + amount >= 0) {
			currentSoulPower += amount;
			SoulPowerAmount.Text = currentSoulPower.ToString();
			return true;
		} 
		return false;
	}

	private void AdjustSoulPower(bool isIncrease) {
		SmallAbilityIcon currentButton = (SmallAbilityIcon)buttons[selectedPos];
		if(isIncrease) {
			UpdateSoulPower(-1*currentButton.ability.SPCost);
		} else {
			UpdateSoulPower(currentButton.ability.SPCost);
		}
	}

	public override void _Process(double delta)
	{
		ProcessInput();
	}

	private void ProcessInput() {
		if(Input.IsActionJustPressed("Flip")) this.Visible = !this.Visible;

		if(!this.Visible) return;

		if(Input.IsActionJustPressed("Select")){
			if (isOnAddButton && currentSlotIndex < 3)
			{
				AddAbility();
			} 
			else if (selectedPos == new Vector2(0,3)) {
				//FuseButton
			}
			else if (selectedPos == new Vector2(1, 3))
			{
				//SkipButton
			}
			else if (selectedPos == new Vector2(2, 3))
			{
				//ConfirmButton
				onConfirmButtonPress();
			}
			else
			{
				//Ability buttons
				buttons[selectedPos].SelectButton();
			}
		} else if (Input.IsActionJustPressed("Back")) {
			if(isOnAddButton) {
				DeSelectAddButton();
			} else if (currentSlotIndex > 0) {
				RemoveAbility();
			}
		}

		if (isOnAddButton == true) return;

		if (Input.IsActionJustPressed("MoveUp"))
		{
				MoveTo(new Vector2(0, -1));			
		}
		else if (Input.IsActionJustPressed("MoveDown"))
		{
				MoveTo(new Vector2(0, 1));
		}
		else if (Input.IsActionJustPressed("MoveLeft"))
		{
				MoveTo(new Vector2(-1,0));
		}
		else if (Input.IsActionJustPressed("MoveRight"))
		{
				MoveTo(new Vector2(1,0));
		}
	}

	private void MoveTo(Vector2 direction) {
		int loops = 0;
		while(loops < 4) {
			loops++;
			Vector2 nextPos = selectedPos + direction*loops;
			if (nextPos.X < 0) {
				MoveToPosition(FindFurthestRightPos(nextPos.Y));
				break;
			}
			if(nextPos.Y < 0) {
				MoveToPosition(new Vector2(0,3));
				break;
			}
			nextPos = new Vector2(nextPos.X % 4,nextPos.Y % 4);
			if(buttons[nextPos].isActive) {
				MoveToPosition(nextPos);
				break;
			}
		}
	}

	private void MoveToPosition(Vector2 Pos) {
		buttons[selectedPos].ToggleSelectOff();

		selectedPos = Pos;
		buttons[selectedPos].ToggleSelectOn();

		if(Pos.Y < 3) {
			SmallAbilityIcon nextButton = (SmallAbilityIcon)buttons[selectedPos];
			UpdateDisplay(nextButton.ability);
		}
	}

	private Vector2 FindFurthestRightPos(float rowNumber) {
		for(int i = 3; i >= 0; i--) {
			if(buttons[new Vector2(i, rowNumber)].isActive) {
				return new Vector2(i,rowNumber);
			}
		}
		return new Vector2(0,0);
	}

	private Vector2 FindFurthestLeftPos(float rowNumber)
	{
		for (int i = 0; i < 4; i++)
		{
			if (buttons[new Vector2(i, rowNumber)].isActive)
			{
				return new Vector2(i, rowNumber);
			}
		}
		return new Vector2(0, 0);
	}

	private void SelectAddButton() {
		isOnAddButton = true;
		SmallAbilityIcon currentButton = (SmallAbilityIcon)buttons[selectedPos];
		addButton.maxCharges = currentSoulPower/currentButton.ability.SPCost;
		addButton.ToggleSelectOn();
	}

	private void DeSelectAddButton()
	{
		isOnAddButton = false;
		if(selectedPos.Y < 3) {
			SmallAbilityIcon currentButton = (SmallAbilityIcon)buttons[selectedPos];
			UpdateSoulPower(currentButton.ability.SPCost * addButton.numberOfCharges);
		}
		addButton.numberOfCharges = 0;
		addButton.numberDisplay.Text = 0.ToString();
		addButton.ToggleSelectOff();
	}

	private void SelectAbility() {
		SelectAddButton();
	}

	private void AddAbility() {
		if(addButton.numberOfCharges == 0) {
			DeSelectAddButton();
			return;
		}
		SmallAbilityIcon currentButton = (SmallAbilityIcon)buttons[selectedPos];
		abilityIcons[currentSlotIndex].SetAbility(currentButton.ability);
		abilityIcons[currentSlotIndex].numberOfCharges = addButton.numberOfCharges;
		abilityIcons[currentSlotIndex].UpdateLabel();
		currentSlotIndex++;

		addButton.numberOfCharges = 0;

		currentButton.ToggleSelectOff();
		currentButton.isActive = false;
		currentButton.animPlayer.Play("Added");
		selectedPos = FindFirstActiveAbility();
		if (currentSlotIndex == 3)
		{
			selectedPos = new Vector2(2, 3);
		}
		buttons[selectedPos].ToggleSelectOn();
		if (selectedPos.Y < 3)
		{
			SmallAbilityIcon nextButton = (SmallAbilityIcon)buttons[selectedPos];
			UpdateDisplay(nextButton.ability);
		}
		DeSelectAddButton();
	}

	private void RemoveAbility() {
		AbilityIcon currentSlot = abilityIcons[currentSlotIndex - 1];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 4;j++) {
				if(((SmallAbilityIcon)buttons[new Vector2(j,i)]).ability == currentSlot.ability) {
					buttons[new Vector2(j,i)].isActive = true;
					buttons[new Vector2(j, i)].animPlayer.Play("RESET");
					break;
				}
			}
		}
		UpdateSoulPower(currentSlot.ability.SPCost*currentSlot.numberOfCharges);
		currentSlot.ResetIcon();
		currentSlotIndex--;
	}

	private Vector2 FindFirstActiveAbility() 
	{
		for (int i = 0; i < 4; i++) {
			if (buttons[new Vector2(i,0)].isActive) {
				return new Vector2(i, 0);
			}
		}
		return new Vector2(0,0);
	}

	private void UpdateDisplay(Ability ability) {
		if(ability != null) {
			displayIcon.Texture = ability.Icon;
			displayName.Text = ability.Name;
			displayDescription.Text = ability.Description;
			SPcost.Text = ability.SPCost.ToString();
		}
	}

	private void onConfirmButtonPress() {
		onConfirmButtonPressed?.Invoke(abilityIcons);
		foreach (AbilityIcon icon in abilityIcons) {
			icon.ResetIcon();
		}
	}

	public void ResetBook() {
		for (int i = 0; i < 12; i++)
		{
			if (buttons[new Vector2(i%4, i/4)].isActive)
			{
				((SmallAbilityIcon)buttons[new Vector2(i % 4, i / 4)]).RemoveAbility();
			}
		}
		currentSlotIndex = 0;
		UpdateAbilityIcons();
	}
}
