using Godot;
using System;

public partial class BaseHero : BaseUnit
{
    public override void _EnterTree()
    {
        base._EnterTree();
        stats.TryGetStatValue(StatType.HP,out int maxHP);
        BattleUI.Instance.SetMaxHP(maxHP);
    }
}
