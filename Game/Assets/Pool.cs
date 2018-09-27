using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T,Owner> where T : IPooleable<Owner>
{
    int initial_cant;

    Owner owner;

    public List<T> pool = new List<T>();
    public List<T> active = new List<T>();
    public Factory<T> factory;

    public Pool(Owner owner, GameObject model, int cant = 10)
    {
        this.owner = owner;
        initial_cant = cant;
        factory = new Factory<T>(model);
    }

    public void Create()
    {
        for (int i = 0; i < initial_cant*2; i++)
        {
            var obj = factory.IntanciateObject();
            obj.Deactivate();
            pool.Add(obj);
        }
    }

    public T GetObject()
    {
        //si no tengo creo mas
        if (pool.Count <= 0) Create();

        //como ya tengo devuelvo el proximo
        var toreturn = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        active.Add(toreturn);
        toreturn.SetOwner(owner);
        toreturn.Activate();
        return toreturn;
    }

    public void ReleaseObject(T obj)
    {
        foreach (var o in active)
        {
            if (obj.Equals(o))
            {
                o.Deactivate();
                pool.Add(obj);
                active.Remove(obj);
                return;
            }
        }
        Debug.LogError("no se encontro el objeto a relesear");
    }
}
