using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    public TriggerFilter<Player> filter;

    private void Awake()
    {
        gameObject.FindAndLink<Sensor>(SensorFound);
    }

    private void SensorFound(Sensor obj)
    {
        filter = new TriggerFilter<Player>(obj, PlayerFound, Layers.PLAYER, TriggerFilter<Player>.TriggerType._2D);
    }

    private void PlayerFound(Player obj)
    {
        obj.lastCheckPoint = transform.position;
    }
}
