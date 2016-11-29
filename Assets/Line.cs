using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class Line : MonoBehaviour
{
    public Vector3 p0, p1;

    public void OnSceneGUI() {
        
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(p0, p1);
    }
}
