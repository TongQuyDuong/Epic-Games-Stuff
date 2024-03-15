using Godot;
using System;

public partial class BaseEnemy : BaseUnit
{
    [Export] protected EnemyStateConftroller stateCon;
    [Export] protected EnemyHPBar hpBar;
    [Export] public float waitTime;
    [Export] public Ability ability;

    protected float countdown;

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
        switch (stateCon.currentState)
        {
            case EnemyState.Idling:
                if (countdown > 0)
                {
                    countdown -= (float)GetProcessDeltaTime();
                }
                else
                {
                    PerformAction();
                }

                break;
            case EnemyState.Pursuing:
                break;
            case EnemyState.Attacking:
                break;
            default:
                break;
        }
    }

    protected virtual void PerformAction() {
        
    }

    protected void Flip()
    {
        this.isFacingRight = !this.isFacingRight;

        this.Scale = new Vector2(this.Scale.X * -1, this.Scale.Y);
        hpBar.Flip();
    }

    public void ReturnToIdle()
    {
        animPlayer.Play("Idle");
        countdown = waitTime;
        stateCon.ChangeState(EnemyState.Idling);
    }
}
