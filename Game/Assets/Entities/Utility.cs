using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static class Utils
{
    public static T Log<T>(this T param, string message = "")
    {
        Debug.Log(message + param.ToString());
        return param;
    }

    public static void Foreach<T>(this IEnumerable<T> col, Action<T> action)
    {
        var seed = col.First();
        Generate(seed, x => { action(x); return x; });
    }

    public static IEnumerable<Src> Generate<Src>(Src seed, Func<Src, Src> generator)
    {
        while (true)
        {
            yield return seed;
            seed = generator(seed);
        }
    }
}