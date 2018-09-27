using UnityEngine;

public abstract class Move
{
    public float speed;

    public Move(float _speed = 5)
    {
        speed = _speed;
    }

    public abstract Vector3 GetVector(Transform t = null);
    public abstract Quaternion GetQuaternion(Transform t = null);

    protected bool _canMove;
    public abstract void CanMove(bool canMove);
}
