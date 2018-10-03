using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterPreset : MonoBehaviour {

    public List<EnemyShooter> enemies;

    [Header("Model")]
    public float speedRotation;

    public Sprite sprite_Search;
    public Sprite sprite_OnSight;

    private void Awake()
    {
        gameObject.FindAndLink<EnemyShooter>(EnemiesFound);

        foreach (var e in enemies)
        {
            e.Preset = this;
            e.rotationSpeed = speedRotation;
        }
    }

    private void EnemiesFound(List<EnemyShooter> obj) { enemies = obj; }
}
