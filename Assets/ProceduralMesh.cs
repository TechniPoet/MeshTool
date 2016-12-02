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

    public Vector2[] verts;

    public int GetVertCount() {
        return verts.Length;
    }


    public Vector2 GetVert(int ind) {
        return verts[ind];
    }
    

    public void SetVert(int ind, Vector2 val) {
        verts[ind] = val;
    }

    public void GenerateMesh() {
        mf = GetComponent<MeshFilter>();
        mf.mesh = null;
        mf.sharedMesh = new Mesh();
        mesh = mf.sharedMesh;
        s = Parent._Spline;
        shape = new ExtrudeShape();
        shape.verts = verts;
        shape.normals = new Vector2[GetVertCount()];
        shape.uCoords = new float[GetVertCount()];
        for (int i = 0; i < GetVertCount(); i++) {
            shape.uCoords[i] = i / GetVertCount();
            shape.normals[i] = new Vector2(1, 0);
        }
        if (s != null) {
            OrientedPoint[] op = new OrientedPoint[s.curvePoints.Count];
            
            for (int i = 0; i < op.Length; i++) {
                op[i] = new OrientedPoint(s.curvePoints[i], Quaternion.identity, (i / s.ControlPointCount));
            }
            BezierUtil.Extrude(ref mesh, shape, op);
            mesh.RecalculateNormals();
        } else {
            Debug.Log("no s");
        }
    }

    
}
