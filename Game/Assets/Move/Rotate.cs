using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : Move {

    public Rotate(float _speed = 5) : base(_speed)
    {
    }

    public override void CanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public override Vector3 GetVector(Transform t = null)
    {
        return Vector3.zero;
    }

    public override Quaternion GetQuaternion(Transform t = null)
    {
        //float AngleRad = Mathf.Atan2(Input.mousePosition.y - t.position.y, Input.mousePosition.x - t.position.x);
        //float AngleDeg = (360 / Mathf.PI) * AngleRad * speed;
        //return Quaternion.Euler(0, 0, AngleDeg);

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(t.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg * speed;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
