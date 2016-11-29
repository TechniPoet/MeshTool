using UnityEngine;

public class OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;

    public OrientedPoint(Vector3 pos, Quaternion rot) {
        position = pos;
        rotation = rot;
    }

    public Vector3 LocalToWorld(Vector3 point) {
        return position + rotation * point;
    }

    public Vector3 WorldToLocal(Vector3 point) {
        return Quaternion.Inverse(rotation) * (point - position);
    }

    public Vector3 LocalToWorldDirection(Vector3 dir) {
        return rotation * dir;
    }
}
