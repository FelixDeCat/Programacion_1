using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shoot<T>
{
    Pool<T> bullet_pool;

    Action<PoolObj<T>, Transform, float> configurer;
    Transform pointToShoot;
    float speed;

    public Shoot(Action<T, Action<T>> OnActive, Action<T> OnDeactivate, Action<PoolObj<T>,Transform, float> configurer, Transform pointToShoot, float speed, GameObject model)
    {
        bullet_pool = new Pool<T>(model, OnActive, OnDeactivate);
        this.configurer = configurer;
        this.pointToShoot = pointToShoot;
        this.speed = speed;
    }

    public void Shot()
    {
        var obj = bullet_pool.GetObject();
        configurer(obj,pointToShoot, speed);
    }
}
