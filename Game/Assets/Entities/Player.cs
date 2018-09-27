using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : Entity {

    public Transform rotator;
    public Transform point_to_fire;

    public Shoot<Bullet> shoot;

    public override void Init()
    {
        shoot = new Shoot<Bullet>(point_to_fire, bullet_model, Bullet.Activar, Bullet.Desactivar);
    }

    public void Attack()
    {
        var obj = shoot.Shot();
        obj.GetObj.Set(point_to_fire, 5);
    }

    public override void Dead()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedRefresh()
    {
        foreach (var move in moves) mypos += move.GetVector(transform);
        myRb.velocity = mypos;
        mypos = Vector3.zero;
    }

    

    public override void Refresh()
    {
        myQuat = rotation.GetQuaternion(rotator);
        rotator.rotation = myQuat;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
}
