using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooleable<Entity> {

    SpriteRenderer my_sp;
    Entity owner;
    float speed;
    float timer;

    public bool canmove;

    public void Activate() {
        gameObject.name = gameObject.name + " (ON AIR)";
        my_sp.enabled = true;
        canmove = true;
    }
    public void Deactivate() {
        Debug.Log("Deactivate");
        gameObject.name = "Bullet";
        my_sp.enabled = false;
        canmove = false;
    }

    public Bullet Move(Vector2 _pos, Quaternion _dir, float _speed)
    {
        speed = _speed;
        transform.position = _pos;
        transform.rotation = _dir;
        return this;
    }

    void Awake()
    {
        my_sp = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        if (canmove)
        {
            transform.position += transform.up * speed;

            if (timer < 3)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                owner.bullet_pool.ReleaseObject(this);
            }
        }
	}

    public void SetOwner(Entity ent)
    {
        owner = ent;
    }
}
