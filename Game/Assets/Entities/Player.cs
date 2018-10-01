using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public Transform rotator;
    public Transform point_to_fire;


    public GameObject bullet_model;
    public Shoot<Bullet> shoot;

    public Sensor enemySensor;
    public TriggerFilter<Enemy> enemyTrigger;
    public LayerMask layerEnemy;

    public override void Init()
    {
        shoot = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, point_to_fire, 5f, bullet_model);
        enemyTrigger = new TriggerFilter<Enemy>(enemySensor, HitWithEnemy, layerEnemy, TriggerFilter<Enemy>.TriggerType._2D);
    }

    public void HitWithEnemy(Enemy e)
    {
        Debug.Log("Hit With Enemy");
    }

    public void Attack()
    {
        shoot.Shot();
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
        //myQuat = rotation.GetQuaternion(transform);

        myeuler = rotation.GetVector(transform);
        rotator.localEulerAngles = myeuler;

        //rotator.rotation = myQuat;

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
}

