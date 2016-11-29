using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}

    void OnDrawGizmos() {

    }
}
