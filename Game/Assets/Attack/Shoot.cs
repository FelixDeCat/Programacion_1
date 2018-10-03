using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shoot<T>
{
    Pool<T> bullet_pool;

    Action<PoolObj<T>, Transform, float, int> bullet_configurer_delegate;
    Transform pointToShoot;
    float speed;
    int damage;

    public void Clear() { bullet_pool.Clear(); }

    public Shoot(Action<T, Action<T>> OnActive, Action<T> OnDeactivate, Action<PoolObj<T>,Transform, float, int> configurer, Transform pointToShoot, float speed, GameObject model, int damage)
    {
        bullet_pool = new Pool<T>(model, OnActive, OnDeactivate);
        this.bullet_configurer_delegate = configurer;
        this.pointToShoot = pointToShoot;
        this.speed = speed;
        this.damage = damage;
    }

    public void Shot()
    {
        var obj = bullet_pool.GetObject();
        bullet_configurer_delegate(obj,pointToShoot, speed, damage);
    }
}
