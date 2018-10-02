using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public Transform rotator;
    public Transform point_to_fire;

    int life;

    public GameObject bullet_model;
    public Shoot<Bullet> shoot;
    public int bulletDamage;

    [SerializeField] UI_HeartManager heartManager;

    public Sensor enemySensor;
    public TriggerFilter<Enemy> enemyTrigger;
    public LayerMask layerEnemy;

    PlayerAnimation anim;

    public int Life
    {
        get { return life; }
        set {
            if (value < 0) { life = 0; Dead(); }
            else life = value;
            try { heartManager.SetLife(life); }
            catch (System.NullReferenceException ex) { Debug.LogError("Todavia no se creo el UI_HeartManager"); }
        }
    }

    public override void Awake()
    {
        moves = new List<Move>();
        myRb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void Init()
    {
        canmove = true;
        shoot = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, point_to_fire, 5f, bullet_model, bulletDamage);
        enemyTrigger = new TriggerFilter<Enemy>(enemySensor, HitWithEnemy, layerEnemy, TriggerFilter<Enemy>.TriggerType._2D);
        Life = 100;

        gameObject.FindAndLink<Animator>(AnimatorFound);
    }

    private void AnimatorFound(Animator obj)
    {
        anim = new PlayerAnimation(obj);
    }

    public void ReceiveDamage(int damage)
    {
        ScreenFeedback.instancia.PerderVida();
        Life -= damage;
    }

    public void HitWithEnemy(Enemy e)
    {
        Debug.Log("Hit With Enemy");
    }

    public void Attack()
    {
        shoot.Shot();
    }

    public override void Dead()
    {
        anim.Die();
    }

    public override void FixedRefresh()
    {
        if (!canmove) return;
        foreach (var move in moves) mypos += move.GetVector(transform);
        myRb.velocity = mypos;
        mypos = Vector3.zero;
    }

    public override void Refresh()
    {
        if (!canmove) return;

        //myQuat = rotation.GetQuaternion(transform);

        myeuler = rotation.GetVector(transform);
        rotator.localEulerAngles = myeuler;

        //rotator.rotation = myQuat;

        anim.SetBool(
            "Run",
            !(Input.GetAxis("Horizontal") < 0.01f && Input.GetAxis("Horizontal") > -0.01f) ||
            !(Input.GetAxis("Vertical") < 0.01f && Input.GetAxis("Vertical") > -0.01f));

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    
}

