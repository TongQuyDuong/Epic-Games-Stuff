using Godot;
using System;

public partial class Transition : Node2D
{
    public static Action onTransitionFinished;
    [Export] public AnimationPlayer animationPlayer;
    public override void _EnterTree() {
        AddUserSignal("finished");
    }
    public void FadeOut() {
        animationPlayer.PlayBackwards("Close");
    }

    private void _on_animation_finished(StringName animName) {
        EmitSignal("finished");
        if (animName == "Open") {
            onTransitionFinished?.Invoke();
        }
    }
}
