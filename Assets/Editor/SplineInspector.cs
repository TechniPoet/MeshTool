using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor
{
    private Spline curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;

    private Vector3[] editorPoints;
    bool start = false;

    public override void OnInspectorGUI()
	{
        if (selectedIndex >= 0 && selectedIndex < curve.ControlPointCount) {
            DrawSelectedPointInspector();
            Repaint();
        }
        // Todo: custom array to allow moving point ordor
        if (GUILayout.Button("Add Spline")) {
            Undo.RecordObject(curve, "Add Spline");
            curve.AddSpline();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(curve);
        }
        if (GUILayout.Button("Remove Spline")) {
            Undo.RecordObject(curve, "Remove Spline");
            curve.RemoveSpline();
            SceneView.RepaintAll();
            EditorUtility.SetDirty(curve);
        }
        
	}


    void OnSceneGUI() {
        curve = target as Spline;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        handleTransform.rotation : Quaternion.identity;
        ShowPoints();
        //Handles.DrawBezier(curve.points[0], curve.points[1], curve.points[2], curve.points[3], Color.blue, null, 2f);
        for (int i = 0; i < curve.splines; i++) {
            DrawBezier(i);
        }
        
    }


    void DrawBezier(int splineInd) {

        Vector3[] pnts = curve.GetSplinePoints(splineInd);
        Vector3 start = curve.transform.TransformPoint(BezierUtil.GetPoint(pnts, 0f));
        float i = 0;
        while (i < 1) {
            Handles.color = Color.white;
            i += .01f;
            Vector3 end = curve.transform.TransformPoint(BezierUtil.GetPoint(pnts, i));
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
        editorPoints = new Vector3[curve.ControlPointCount];
        for (int i = 0; i < curve.ControlPointCount; i++) {
            editorPoints[i] = ShowPoint(i);
        }
        Handles.color = Color.grey;
        for (int i = 1; i < editorPoints.Length; i++) {
            Handles.DrawLine(editorPoints[i-1], editorPoints[i]);
        }
    }


    Vector3 ShowPoint (int index) {
        Vector3 point = handleTransform.TransformPoint(curve.GetControlPoint(index));
        // size cubes to be uniform regardless of zoom or scale
        float size = HandleUtility.GetHandleSize(point);
        Handles.color = Color.white;
        // Make points white cubes
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap)) {
            selectedIndex = index;
        }
        if (selectedIndex == index) {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(curve, "MovePoint");
                EditorUtility.SetDirty(curve);
                curve.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        
        return point;
    }



    private void DrawSelectedPointInspector() {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", curve.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.SetControlPoint(selectedIndex, point);
        }
    }
}
