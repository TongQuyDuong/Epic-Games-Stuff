using Godot;
using System;

public partial class BaseEnemy : BaseUnit
{
    [Export] protected EnemyStateConftroller stateCon;
    [Export] protected AnimationPlayer animPlayer;
    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
    }
}
