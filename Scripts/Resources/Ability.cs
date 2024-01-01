using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(Ability), "", nameof(Resource))]
public partial class Ability : Resource
{
    [Export] public string aName;
    [Export] public string aDescription;
    [Export] public float castTime;
    [Export] public float cooldown;
    [Export] public float activeTime;
    public Action OnCast;
    protected BaseUnit caster;
    [Export] public CompressedTexture2D Icon; 

    public virtual void Initialize(BaseUnit owner)
    {

    }
    public virtual void TriggerAbility()
    {

    }
    public virtual void TriggerAbility(Vector2 currentPos)
    {

    }
}
