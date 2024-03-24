using Godot;
using System;

public partial class Transition : Node2D
{
    [Export] public AnimationPlayer animationPlayer;
    public override void _EnterTree() {
        AddUserSignal("finished");
    }
    public void FadeOut() {
        animationPlayer.PlayBackwards("Open");
    }

    private void _on_animation_finished(StringName animName) {
        EmitSignal("finished");
    }
}
