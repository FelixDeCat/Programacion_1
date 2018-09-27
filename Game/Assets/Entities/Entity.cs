using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IRefresh, IDead, IInitiable
{
    public List<Move> moves;
    public Move rotation;
    public float speed;

    public Attack[] attacks;

    public Pool<Bullet,Entity> bullet_pool;
    public GameObject bullet_model;

    Rigidbody2D myRb;
    Vector3 mypos;
    protected Quaternion myQuat;

    void Awake()
    {
        moves = new List<Move>();
        myRb = gameObject.GetComponent<Rigidbody2D>();

        bullet_pool = new Pool<Bullet,Entity>(this,bullet_model,5);
    }

    public virtual void Init()
    {

    }
    public virtual void Refresh()
    {
        
    }
    public virtual void FixedRefresh()
    {
        foreach (var move in moves) mypos += move.GetVector(transform);
        myRb.velocity = mypos;
        mypos = Vector3.zero;
    }
    public virtual void Dead()
    {
        
    }
}
