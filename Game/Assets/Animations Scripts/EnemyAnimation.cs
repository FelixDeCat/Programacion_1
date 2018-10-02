using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : GenericAnimator {

    public EnemyAnimation(Animator anim) : base(anim)
    {
    }

    public override void Idle()
    {
        Play(IDLE);
    }

    public override void Run()
    {
        Play(RUN);
    }

    public override void Walk()
    {
        Play(WALK);
    }
    public override void Die()
    {
        Play(DIE);
    }

    public void Attack()
    {
        Play("Attack");
    }

    int cont;
    public bool CanDieMore()
    {
        cont++;
        if (cont < 5)
        {
            Play("DieMore" + cont);
            return true;
        }
        else
        {
            return false;
        }
    }
}
