using Godot;
using System;
using MonoCustomResourceRegistry;
using MEC;

[RegisteredType(nameof(ControlEffect), "", nameof(Resource))]

public partial class ControlEffect : StatusEffectData
{
    [Export] public PackedScene controlEffect;
    public Node2D Ceffect;
    public override void TriggerEffect(BaseUnit target)
    {
        target.isControlled = true;
        Ceffect = controlEffect.Instantiate<Node2D>();
        target.ShowControlEffect(Ceffect);
    }

    public void RemoveEffect(BaseUnit target,Node2D Ceffect)
    {
        target.isControlled = false;
        target.RemoveControlEffect(Ceffect);
    }
}
