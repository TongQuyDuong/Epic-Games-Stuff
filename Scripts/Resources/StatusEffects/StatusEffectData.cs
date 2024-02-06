using Godot;
using System;
using System.Diagnostics;

public abstract partial class StatusEffectData : Resource
{
    [Export] public string Name;
    [Export] public string Description;
    [Export] public bool isNegativeEffect;
    [Export] public bool isPermanentEffect;
    [Export] public float duration;
    [Export] public StatusEffectType type;
    [Export] public CompressedTexture2D Icon;

    public virtual void TriggerEffect(BaseUnit target){
        
    }

    public virtual void RemoveEffect(BaseUnit target)
    {

    }
}
