using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

[GlobalClass]

public partial class UnitStat : Resource
{
    [Export] public StatType statType { get; set; }
    [Export] public float BaseValue { get; set; }
    [Export] public int numberOfMods = 0;
    protected float lastBaseValue = float.MinValue;
    public int CurrentValue
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
    protected int _value;
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
        numberOfMods += 1;
    }

    public bool RemoveModifier(StatModifier mod)
    {

        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            numberOfMods -= 1;
            return true;
        }

        return false;
        

    }

    protected int CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            switch (statModifiers[i].Type)
            {
                case StatModType.Flat:
                    finalValue += statModifiers[i].Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += statModifiers[i].Value;
                    if (i + 1 > statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                        finalValue *= sumPercentAdd;
                    break;
                case StatModType.PercentMult:
                    finalValue *= statModifiers[i].Value;
                    break;
                default:
                Debug.Print("Defaulted");
                break;
                    
            }
        }
        

        return (int)Math.Round(finalValue, 4);
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
    Resistance,
    HP
}