using System;
using UnityEngine;

public class TriggerFilter<T>
{
    public enum TriggerType { _2D, _3D };
    Action<T> filtered;
    GenericFilter<T> genfilter;
    Sensor sensor;
    public TriggerFilter(Sensor sensor, Action<T> filtered, LayerMask layer, TriggerType tt)
    {
        this.sensor = sensor;
        genfilter = new GenericFilter<T>(GetObject);
        if (tt == TriggerType._2D) { sensor.SetFilter2D(genfilter.Check, layer); }
        else sensor.SetFilter(genfilter.Check, layer);
        this.filtered = filtered;
    }
    public void GetObject(T e) { filtered(e); }

    public void Off()
    {
        sensor.Off();
    }
}

internal class GenericFilter<T>
{
    public Action<T> callback;
    public GenericFilter(Action<T> callback) { this.callback = callback; }
    public void Check(Collider2D col, LayerMask layer) { if (layer.value == col.gameObject.layer ) callback(col.gameObject.GetComponent<T>()); }
    public void Check(Collider col, LayerMask layer) { if (col.gameObject.layer == layer) callback(col.gameObject.GetComponent<T>()); }
}
