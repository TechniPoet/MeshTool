using UnityEngine;
using System.Collections.Generic;


public class BezierCurve : MonoBehaviour
{
    protected Transform[] bezierPoints;
    [System.NonSerialized]
    public Vector3[] points = new Vector3[] { };

    public void Reset() {
        
        List<Transform> tempList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++) {
            tempList.Add(transform.GetChild(i));
        }

        while (transform.childCount < 3) {
            GameObject newChild = new GameObject("Point" + transform.childCount);
            newChild.transform.parent = transform;
            tempList.Add(newChild.transform);
        }
        bezierPoints = tempList.ToArray();
        
        points = new Vector3[bezierPoints.Length];
        for (int i = 0; i < bezierPoints.Length; i++) {
            points[i] = bezierPoints[i].transform.position;
        }
    }
}
