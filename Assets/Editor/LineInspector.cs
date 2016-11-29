using UnityEditor;
using UnityEngine;


[CustomEditor (typeof(Line))]
public class LineInspector : Editor
{
	public override void OnInspectorGUI()
	{
        Line line = target as Line;
    }
}
