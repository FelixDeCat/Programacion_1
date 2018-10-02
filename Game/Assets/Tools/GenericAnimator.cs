using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericAnimator {

    public Animator anim_component;

    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string RUN = "Run";
    public const string DIE = "Die";

    public GenericAnimator(Animator anim)
    {
        anim_component = anim;
    }

    protected void Play(string p)
    {
        anim_component.Play(p);
    }

    public abstract void Idle();
    public abstract void Walk();
    public abstract void Run();
    public abstract void Die();
}
