using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Move
{
    float timer;
    public float time_to_up = 1;

    public float OriginalCant = 2;
    public float cant;

    bool gotojump;

    public Vector3 v3;

    public Jump()
    {
        Reset();
    }

    void Reset() {
        gotojump = false;
        timer = 0;
        cant = OriginalCant;
        v3 = new Vector3(0, 0, 0);
    }

    public override Vector3 GetVector(Transform t = null)
    {
        if (Input.GetButtonDown("Jump")) {
            gotojump = true;
        }

        if (gotojump && _canMove)
        {
            if (timer < time_to_up)
            {
                timer = timer + 1 * Time.deltaTime;
                cant = Mathf.Lerp(OriginalCant, 0, timer);
                v3.y = cant;
            }
            else Reset();
        }
        else
        {
            v3 = Vector3.zero;
        }

        return v3;
    }

    public override Quaternion GetQuaternion(Transform t = null)
    {
        return new Quaternion();
    }

    public override void CanMove(bool canMove)
    {
        _canMove = canMove;
    }
}
