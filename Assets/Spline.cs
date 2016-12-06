using UnityEngine;
using System.Collections.Generic;


public class Spline : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private Vector3[] points;

    public List<Vector3> curvePoints = new List<Vector3>();

    MeshTool parent;
    public MeshTool Parent {
        get {
            if (parent == null) {
                parent = GetComponentInParent<MeshTool>();
            }
            return parent;
        }
    }


    public int ControlPointCount {
        get {
            return points.Length;
        }
    }

    public Vector3 GetControlPoint(int index) {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 point) {
        points[index] = point;
    }

    public Vector3[] GetControlPoints() {
        return points;
    }

    [HideInInspector]
    private int splines = 1;
    public int Splines { get { return splines; } }


    public void Reset() {
        points = new Vector3[] {
            new Vector3(0f, 0f, -1.5f),
            new Vector3(0f, 0f, -.5f),
            new Vector3(0f, 0f, 1.5f),
            new Vector3(0f, 0f, 2.5f)
        };
        splines = 1;
    }

    public void AddSpline() {
        splines++;
        List<Vector3> temp = new List<Vector3>();
        Vector3 vel = points[points.Length - 1] - points[points.Length - 2];
        //vel *= 5;
        Vector3 lastPoint = points[points.Length - 1];
        temp.AddRange(points);
        temp.Add(lastPoint + (vel));
        temp.Add(lastPoint + (vel*2));
        temp.Add(lastPoint + (vel*3));
        points = temp.ToArray();
    }


    public void RemoveSpline() {
        splines--;
        List<Vector3> temp = new List<Vector3>();
        temp.AddRange(points);
        temp.RemoveAt(points.Length - 1);
        temp.RemoveAt(points.Length - 2);
        temp.RemoveAt(points.Length - 3);
        points = temp.ToArray();
    }


    public Vector3[] GetSplinePoints(int Ind) {
        Vector3[] pnts = new Vector3[4];
        for (int i = 0; i < 4; i++) {
            pnts[i] = points[(Ind * 3) + i];
        }
        return pnts;
    }
}
