using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CustomEditor(typeof(Spline))]
public class BezierCurveInspector : Editor
{
    private Spline curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private Vector3[] editorPoints;
    bool start = false;

    public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
        // Todo: custom array to allow moving point ordor
        if (GUILayout.Button("Add Point")) {
            List<Vector3> temp = new List<Vector3>();
            temp.AddRange(curve.points);
            temp.Add(new Vector3(20, 10, 0));
            curve.points = temp.ToArray();
        }
        
	}


    void OnSceneGUI() {
        curve = target as Spline;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        handleTransform.rotation : Quaternion.identity;
        ShowPoints();
        //Handles.DrawBezier(curve.points[0], curve.points[1], curve.points[2], curve.points[3], Color.blue, null, 2f);
        DrawBezier();
    }


    void DrawBezier() {
        
        Debug.Log(curve.points);
        Vector3 start = curve.transform.TransformPoint(BezierUtil.GetPoint(curve.points, 0f));
        float i = 0;
        while (i < 1) {
            Handles.color = Color.white;
            i += .01f;
            Vector3 end = curve.transform.TransformPoint(BezierUtil.GetPoint(curve.points, i));
            Handles.DrawLine(start, end);
            /*
            Handles.color = Color.green;
            Vector3 velocity = (end - start) * 15;
            Handles.DrawLine(end, end + velocity);
            */
            start = end;
        }
    }


    void ShowPoints() {
        editorPoints = new Vector3[curve.points.Length];
        for (int i = 0; i < curve.points.Length; i++) {
            editorPoints[i] = ShowPoint(i);
        }
        Handles.color = Color.grey;
        for (int i = 1; i < editorPoints.Length; i++) {
            Handles.DrawLine(editorPoints[i-1], editorPoints[i]);
        }
    }


    Vector3 ShowPoint (int index) {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(curve, "MovePoint");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}
