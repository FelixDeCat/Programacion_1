using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T>
{
    public GameObject model;

    T obj;

    public Factory(GameObject model) {
        this.model = model;
    }

    public T IntanciateObject()
    {
        GameObject go = GameObject.Instantiate(model); 
        return go.gameObject.GetComponent<T>();
    }
        
}
