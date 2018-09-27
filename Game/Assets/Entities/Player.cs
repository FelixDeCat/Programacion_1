using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : Entity {

    public Transform rotator;
    public Transform point_to_fire;

    public override void Init()
    {
        base.Init();
    }

    public void Attack()
    {
        bullet_pool
            .GetObject()
            .Move(point_to_fire.position, point_to_fire.rotation, 0.1f);
    }

    public override void Refresh()
    {
        base.Refresh();

        myQuat = rotation.GetQuaternion(rotator);
        rotator.rotation = myQuat;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

    }

    public override void FixedRefresh()
    {
        base.FixedRefresh();
    }

    public override void Dead()
    {
        base.Dead();
    }
}
