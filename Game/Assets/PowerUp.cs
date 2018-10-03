using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public TriggerFilter<Player> triggerFilter;

    private void Awake()
    {
        gameObject.FindAndLink<Sensor>(SensorFound);
    }

    private void SensorFound(Sensor obj)
    {
        triggerFilter = new TriggerFilter<Player>(obj, PlayerDetect, Layers.PLAYER, TriggerFilter<Player>.TriggerType._2D);
    }

    private void PlayerDetect(Player obj)
    {
        obj.BulletDamage += obj.BulletDamage * 2;
        Destroy(this.gameObject);
    }
}
