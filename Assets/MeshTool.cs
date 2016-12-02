using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class MeshTool : MonoBehaviour {

    ProceduralMesh pMesh;
    public ProceduralMesh _PMesh {
        get {
            if (pMesh == null) {
                pMesh = GetComponentInChildren<ProceduralMesh>();
            }
            return pMesh;
        }
    }
    Spline spline;
    public Spline _Spline {
        get {
            if (spline == null) {
                spline = GetComponentInChildren<Spline>();
            }
            return spline;
        }
    }

    public bool manuallyUpdateMesh;

    void Update() {
        if (!manuallyUpdateMesh) {
            _PMesh.GenerateMesh();
        }
    }
}
