using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IRefresh, IDead, IInitiable
{
    public List<Move> moves;
    public Move rotation;
    public float speed;
    
    public GameObject bullet_model;

    protected Rigidbody2D myRb;
    protected Vector3 mypos;
    protected Quaternion myQuat;

    void Awake()
    {
        moves = new List<Move>();
        myRb = gameObject.GetComponent<Rigidbody2D>();

        //bullet_pool = new Pool<Bullet,Entity>(this,bullet_model,5);
    }

    public abstract void Init();
    public abstract void Refresh();
    public abstract void FixedRefresh();
    public abstract void Dead();
}
