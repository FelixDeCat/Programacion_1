using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour, IRefresh, IDead, IInitiable
{
    public List<Move> moves;
    public Move rotation;
    public float speed;

    public bool canmove;

    [SerializeField] protected Rigidbody2D myRb;
    protected Vector3 mypos;
    protected Quaternion myQuat;
    protected Vector3 myeuler;

    public abstract void Awake();

    public abstract void Init();
    public abstract void Refresh();
    public abstract void FixedRefresh();
    public abstract void Dead();
}
