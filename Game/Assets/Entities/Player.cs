using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public Transform rotator;
    public Transform point_to_fire;

    int life;
    const int MAX = 100;
    bool isdead;

    public GameObject bullet_model;
    public Shoot<Bullet> shoot;
    public int bulletDamage;

    public Vector3 lastCheckPoint;

    [SerializeField] UI_HeartManager heartManager;

    public Sensor enemySensor;
    public TriggerFilter<Bullet> enemyTrigger;
    public LayerMask LayerBullet;

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

    public int BulletDamage
    {
        get
        {
            return bulletDamage;
        }

        set
        {
            bulletDamage = value;
            shoot.Clear();
            shoot = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, point_to_fire, 5f, bullet_model, bulletDamage);
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
        shoot = new Shoot<Bullet>(Bullet.Activar, Bullet.Desactivar, Bullet.SetPoolObj, point_to_fire, 5f, bullet_model, BulletDamage);
        enemyTrigger = new TriggerFilter<Bullet>(enemySensor, HitWithBUllet, Layers.ENEMY_BULLET, TriggerFilter<Bullet>.TriggerType._2D);
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

    public void HitWithBUllet(Bullet e)
    {
        e.Desactivar();
        ReceiveDamage(e.Damage);

        if (isdead)
        {
            enemyTrigger.Enable(false);
        }
    }

    public void Resurrect(Vector2 position)
    {
        enemyTrigger.Enable(true);
        transform.position = position;
        Life = MAX;
        canmove = true;
        isdead = false;
        anim.Idle();
        myRb.simulated = true;
        Main.instancia.msj.ShowMessage(false);
    }

    public void Attack()
    {
        shoot.Shot();
    }

    public override void Dead()
    {
        isdead = true;
        canmove = false;
        anim.Die();
        myRb.simulated = false;
        Main.instancia.msj.ShowMessage(true);
        Main.instancia.msj.SetEndMessage(MessageDeadEnd);
    }

    private void MessageDeadEnd()
    {
        Resurrect(lastCheckPoint);
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

