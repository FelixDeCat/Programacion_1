using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack {

    protected Vector3 pos;

    public Attack(Vector3 _pos)
    {
        pos = _pos;
    }

    public abstract void Shot(Vector3 dir);
    public abstract void Hit();
}
