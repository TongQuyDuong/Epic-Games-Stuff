using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(ControlEffect), "", nameof(Resource))]

public partial class ControlEffect : StatusEffectData
{
    public override void TriggerEffect(BaseUnit target)
    {
        target.isControlled = true;
    }

    public override void RemoveEffect(BaseUnit target)
    {
        target.isControlled = false;
    }
}
