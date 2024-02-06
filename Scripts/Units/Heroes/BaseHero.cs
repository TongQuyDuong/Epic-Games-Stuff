using Godot;
using System;

public partial class BaseHero : BaseUnit
{
    [Export] public PlayerAnimation playerAnim;
    [Export] public PlayerStateController stateController;
    public override void _EnterTree()
    {
        base._EnterTree();
        stats.TryGetStatValue(StatType.HP,out float maxHP);
        BattleUI.Instance.SetMaxHP((int)maxHP);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
