using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Gravity : Move
{
    private Vector3 gravity;
    public Vector3 Gravity { set {  gravity = value; } }

    public Move_Gravity()
    {
        _canMove = true;
    }

    public override void CanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public Move_Gravity(float _gravityValue = -1)
    {
        if (_canMove)
        {
            gravity = new Vector3(0, _gravityValue, 0);
        }
        else
        {
            gravity = Vector3.zero;
        }
    }

    public override Vector3 GetVector(Transform t = null)
    {
        return gravity;
    }
    public override Quaternion GetQuaternion(Transform t = null)
    {
        return new Quaternion();
    }
}
