﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : Move {
    public HorizontalMove(float _speed = 5) : base(_speed)
    {
    }

    public override void CanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public override Quaternion GetQuaternion(Transform t = null)
    {
        return new Quaternion();
    }

    public override Vector3 GetVector(Transform t = null)
    {
        return t.right * Input.GetAxis("Horizontal") * speed;
    }
}
