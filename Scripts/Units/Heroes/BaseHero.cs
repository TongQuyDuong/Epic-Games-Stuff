using Godot;
using System;

public partial class BaseHero : BaseUnit
{
    [Export] public PlayerAnimation playerAnim;
    [Export] public PlayerStateController stateController;
    [Export] public Godot.Collections.Array<AbilityHolder> abilityHolders = new Godot.Collections.Array<AbilityHolder>();
    public override void _EnterTree()
    {
        base._EnterTree();
        stats.TryGetStatValue(StatType.HP,out float maxHP);
        BattleUI.Instance.SetMaxHP((int)maxHP);

        SelectSkillBook.onConfirmButtonPressed += LoadAbilities;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        SelectSkillBook.onConfirmButtonPressed -= LoadAbilities;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    public void LoadAbilities(Godot.Collections.Array<AbilityIcon> icons) {
        for (int i = 0; i < 3; i++) {
            abilityHolders[i].UpdateAbility(icons[i]);
        }

        BattleUI.Instance.DisplayAbility(abilityHolders);
    }
}
