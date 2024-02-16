using Godot;
using System;

public partial class BaseEnemy : BaseUnit
{
    [Export] protected EnemyStateConftroller stateCon;
    [Export] protected EnemyHPBar hpBar;

    public override void _EnterTree()
    {
        base._EnterTree();
        stats.TryGetStatValue(StatType.HP, out float maxHP);
        hpBar.SetMaxHP((int)maxHP);
        if (isFacingRight == false) hpBar.Flip();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    protected void Flip()
    {
        this.isFacingRight = !this.isFacingRight;

        this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
        hpBar.Flip();
    }
}
