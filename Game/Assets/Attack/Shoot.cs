using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shoot<T>
{
    Pool<T> bullet_pool;

    public Shoot(Transform pointToFire, GameObject model, Action<T, Action<T>> OnActive, Action<T> OnDeactivate)
    {
        bullet_pool = new Pool<T>(model, OnActive, OnDeactivate);
    }

    public PoolObj<T> Shot()
    {
        return bullet_pool.GetObject();//aca deveria pasarle una funcion con el point of fire
    }
}
