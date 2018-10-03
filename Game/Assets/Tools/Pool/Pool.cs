using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool<T>
{
    private int initial_cant;

    public List<PoolObj<T>> pool = new List<PoolObj<T>>();
    public List<PoolObj<T>> active = new List<PoolObj<T>>();

    public Factory<T> factory;

    private Action<T, Action<T>>   OnActive;
    private Action<T>           OnDisable;

    public void Clear()
    {
        pool.Clear();
        active.Clear();
    }

    public Pool(GameObject model, Action<T, Action<T>> _OnActive, Action<T> _OnDisable, int cant = 10)
    {
        initial_cant = cant;
        factory = new Factory<T>(model);

        OnActive = _OnActive;
        OnDisable = _OnDisable;
    }

    public void Create()
    {
        for (int i = 0; i < initial_cant*2; i++)
        {
            PoolObj<T> obj = new PoolObj<T>(factory.IntanciateObject(),OnActive, OnDisable, ReleaseObject);
            obj.Deactivate();
            pool.Add(obj);
        }
    }

    public PoolObj<T> GetObject()
    {
        //si no tengo creo mas
        if (pool.Count <= 0) Create();

        //como ya tengo devuelvo el proximo
        var toreturn = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        toreturn.Activate();
        active.Add(toreturn);
        return toreturn;
    }

    public void ReleaseObject(T obj)
    {
        foreach (var a in active)
        {
            if (obj.Equals(a.GetObj))
            {
                pool.Add(a);
                a.Deactivate();
                active.Remove(a);
                return;
            }
        }
        //Debug.LogError("no se encontro el objeto a relesear");
    }
}
