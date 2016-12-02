using UnityEngine;
using System;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralMesh : UniqueMesh
{
    Spline s;
    MeshFilter mf;
    Mesh mesh;
    ExtrudeShape shape;

    MeshTool parent;
    public MeshTool Parent {
        get {
            if (parent == null) {
                parent = GetComponentInParent<MeshTool>();
            }
            return parent;
        }
    }

    public Vector2 vert0, vert1, vert2, vert3;
    public Vector2 norm0, norm1, norm2, norm3;
    public float u0, u1, u2, u3;

    public void GenerateMesh() {
        mf = GetComponent<MeshFilter>();
        mf.mesh = null;
        mf.sharedMesh = new Mesh();
        mesh = mf.sharedMesh;
        s = Parent._Spline;
        shape = new ExtrudeShape();
        shape.verts = new Vector2[] {
            vert0, vert1, vert2, vert3
        };
        shape.normals = new Vector2[] {
            norm0, norm1, norm2, norm3
        };
        shape.uCoords = new float[] {
            u0, u1, u2, u3
        };
        if (s != null) {
            OrientedPoint[] op = new OrientedPoint[s.curvePoints.Count];
            
            for (int i = 0; i < op.Length; i++) {
                op[i] = new OrientedPoint(s.curvePoints[i], Quaternion.identity, (i / s.ControlPointCount));
            }
            BezierUtil.Extrude(ref mesh, shape, op);
        } else {
            Debug.Log("no s");
        }
    }

    
}
