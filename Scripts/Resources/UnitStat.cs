using Godot;
using MonoCustomResourceRegistry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[GlobalClass]

public partial class UnitStat : Resource
{
    [Export] public StatType statType { get; set; }
    [Export] public float BaseValue { get; set; }
    protected float lastBaseValue = float.MinValue;
    public float CurrentValue
    {
        get
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }
    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    protected bool isDirty = true;
    protected float _value;
    public UnitStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
        statType = StatType.Attack;
        BaseValue = 0;
    }
    public UnitStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModOrder);
    }

    public bool RemoveModifier(StatModifier mod)
    {

        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }

        return false;

    }

    protected float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            switch (mod.Type)
            {
                case StatModType.Flat:
                    finalValue += mod.Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += mod.Value;
                    if (i + 1 > statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                        finalValue *= sumPercentAdd;
                    break;
                case StatModType.PercentMult:
                    finalValue *= mod.Value;
                    break;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }

    public int CompareModOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order) return -1;
        else if (a.Order > b.Order) return 1;
        return 0; //if a.Order=b.Order
    }

    public bool RemoveAllModsFromSource(object source)
    {
        bool DidRemove = false;
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                DidRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return DidRemove;
    }
}

public enum StatType
{
    Attack,
    Defence,
    Magic,
    Resistance
}