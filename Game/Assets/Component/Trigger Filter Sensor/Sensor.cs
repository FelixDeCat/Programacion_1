using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sensor : MonoBehaviour
{
    Action<Collider2D, LayerMask> callback2D;
    Action<Collider, LayerMask> callback;
    LayerMask layer;

    public void SetFilter2D(Action<Collider2D, LayerMask> col, LayerMask layer) {
        callback2D = col;
        this.layer = layer;
    }
    public void SetFilter(Action<Collider, LayerMask> col, LayerMask layer) {
        callback = col;
        this.layer = layer;
    }

    public void OnTriggerEnter(Collider other) { callback(other, layer); }
    public void OnTriggerEnter2D(Collider2D other) { callback2D(other, layer); }
    public void OnTriggerExit(Collider other) {  }
    public void OnTriggerExit2D(Collider2D other) { }

}