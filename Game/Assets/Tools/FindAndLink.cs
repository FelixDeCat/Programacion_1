using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Tools
{

    public static void FindAndLink<R>(this GameObject go, Action<R> del)
    {
        R sample = default(R);

        try
        {
            var obj = go.GetComponentsInChildren<R>().First();
            del(obj);
        }
        catch (System.InvalidOperationException ex)
        {
            Debug.LogError("Che bobo... me estas un pidiendo un \"" + sample.ToString() + "\" pero no hay ninguno entre los childrens");
        }
    }

    public static Vector3 Only_X(this Vector3 v3, float value) { return new Vector3(value, v3.y, v3.z); }
    public static Vector3 Only_Y(this Vector3 v3, float value) { return new Vector3(v3.x, value, v3.z); }
    public static Vector3 Only_Z(this Vector3 v3, float value) { return new Vector3(v3.x, v3.y, value); }
}
