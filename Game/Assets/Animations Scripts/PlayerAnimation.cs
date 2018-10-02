using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : GenericAnimator {

    public PlayerAnimation(Animator anim) : base(anim)
    {
    }

    public void SetBool(string s, bool b) { anim_component.SetBool(s, b); }

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
}
