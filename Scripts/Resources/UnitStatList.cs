using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using MonoCustomResourceRegistry;

[GlobalClass]
[RegisteredType(nameof(UnitStatList), "", nameof(Resource))]
public partial class UnitStatList : Resource
{
    [Export] public Godot.Collections.Array<UnitStat> StatList = new Godot.Collections.Array<UnitStat>();

    public bool TryGetStatValue(StatType statType, out int value)
    {
        for (int i = 0; i < StatList.Count; i++)
        {
            if (StatList[i].statType == statType)
            {
                value = StatList[i].CurrentValue;
                return true;
            }
        }
        value = 0;
        return false;
    }
}
