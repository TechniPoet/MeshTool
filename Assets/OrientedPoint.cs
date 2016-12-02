using UnityEngine;

public class OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public float vCoordinate;


    public OrientedPoint(Vector3 pos, Quaternion rot, float vCoordinate = 0) {
        position = pos;
        rotation = rot;
        this.vCoordinate = vCoordinate;
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
