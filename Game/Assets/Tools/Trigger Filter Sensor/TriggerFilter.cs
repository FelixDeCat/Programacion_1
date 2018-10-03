using System;
using UnityEngine;

public class TriggerFilter<T>
{

    // IMPORTANTE
    //  cuando creamos los bullets... solo el gameobject que tiene el collider
    // tiene que tener la Layer que vamos a pedir, porque sino estariamos obteniendo todo y no todo tiene un componente bullet
    //
    //

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

    public void Enable(bool b)
    {
        sensor.Enable(b);
    }
}

internal class GenericFilter<T>
{
    public Action<T> callback;
    public GenericFilter(Action<T> callback) { this.callback = callback; }
    public void Check(Collider2D col, LayerMask layer)
    {
        if (layer.value == col.gameObject.layer)
        {
            try
            {
                callback(col.gameObject.GetComponent<T>());
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogWarning("Es es por si nos olvidamos que quitarle el layer a ojetos " +
                    "hijos que no contienen el tipo de dato que nosotros estamos buscando");
            }
            
        }
    }
    public void Check(Collider col, LayerMask layer) { if (col.gameObject.layer == layer) callback(col.gameObject.GetComponent<T>()); }
}
