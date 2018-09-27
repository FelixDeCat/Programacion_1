using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolObj<T>
{
    public readonly T GetObj;
    private readonly Action<T, Action<T>> OnActive;
    private readonly Action<T> OnDisable;
    private readonly Action<T> Advice;

    public PoolObj(T _obj, Action<T,Action<T>> _OnActive, Action<T> _OnDisable, Action<T> elAviso)
    {
        GetObj = _obj;
        OnActive = _OnActive;
        OnDisable = _OnDisable;
        Advice = elAviso;
    }

    public void Activate()
    {
        Debug.Log("Haciendo el Activate");
        OnActive(GetObj, Advice);
    }
    public void Deactivate()
    {
        OnDisable(GetObj);
    }
}
