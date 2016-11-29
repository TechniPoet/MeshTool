using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private Vector3[] oldPoints;
    bool start = false;

    public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
        
	}


    void OnSceneGUI() {
        curve = target as BezierCurve;
        if (curve.points.Length < 3) {
            curve.Reset();
        }
        
        if (oldPoints != curve.points) {
            curve.Reset();
        }
        
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        handleTransform.rotation : Quaternion.identity;

        ShowPoints();
        oldPoints = curve.points;
    }


    void ShowPoints() {
        for (int i = 0; i < curve.points.Length; i++) {
            curve.points[i] = ShowPoint(i);
        }
        Handles.color = Color.white;
        for (int i = 1; i < curve.points.Length; i++) {
            Handles.DrawLine(curve.points[i-1], curve.points[i]);
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
