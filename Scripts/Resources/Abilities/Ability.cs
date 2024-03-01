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
    [Export] public Godot.Collections.Array<StatusEffectData> effectsToApply;
    public Action OnCast;
    [Export] public CompressedTexture2D Icon;
    [Export] public CompressedTexture2D smallIcon;

    public virtual void Initialize() {

    }

    public virtual void TriggerAbility(BaseUnit caster)
    {

    }
    public virtual void TriggerAbility(Vector2 currentPos)
    {

    }
}
