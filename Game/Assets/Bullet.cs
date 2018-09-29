using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    SpriteRenderer my_sp;
    float speed;
    float timer;
    bool canmove;
    static Action<Bullet> aviso;

    public static void Activar(Bullet bullet, Action<Bullet> _aviso)
    {
        aviso = _aviso;
        bullet.Activate();
    }
    public static void Desactivar(Bullet bullet)
    {
        bullet.Deactivate();
    }

    public static void SetPoolObj(PoolObj<Bullet> pool_obj, Transform pointtoshoot, float _speed)
    {
        pool_obj.GetObj.Set(pointtoshoot, _speed);
    }

    public void Set(Transform pointtoshoot , float _speed)
    {
        speed = _speed;
        transform.position = pointtoshoot.position;
        transform.rotation = pointtoshoot.rotation;
    }
    public void Activate()
    {
        timer = 0;
        gameObject.name = gameObject.name + " (ON AIR)";
        my_sp.enabled = true;
        canmove = true;
    }
    public void Deactivate()
    {
        timer = 0;
        gameObject.name = "Bullet";
        my_sp.enabled = false;
        canmove = false;
    }

    void Awake()
    {
        my_sp = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        if (!canmove) return;
        transform.position += transform.up * speed * Time.deltaTime;
        if (timer < 3) timer = timer + 1 * Time.deltaTime;
        else aviso(this);
    }
}
