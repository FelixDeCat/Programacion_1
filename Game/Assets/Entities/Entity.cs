using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IRefresh, IDead, IInitiable
{
    public List<Move> moves;
    public Move rotation;
    public float speed;

    [SerializeField] protected Rigidbody2D myRb;
    protected Vector3 mypos;
    protected Quaternion myQuat;
    protected Vector3 myeuler;

   

    void Awake()
    {
        "asdasd".Log();
        moves = new List<Move>();
        myRb = gameObject.GetComponent<Rigidbody2D>();
    }

    public abstract void Init();
    public abstract void Refresh();
    public abstract void FixedRefresh();
    public abstract void Dead();
}
