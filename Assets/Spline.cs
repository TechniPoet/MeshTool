using UnityEngine;
using System.Collections.Generic;


public class Spline : MonoBehaviour
{
    public Vector3[] points;

    public int splines = 1;
    public void Reset() {
        points = new Vector3[] {
            new Vector3(10f, 0f, 0f),
            new Vector3(20f, 0f, 0f),
            new Vector3(30f, 0f, 0f),
            new Vector3(40f, 0f, 0f)
        };
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
